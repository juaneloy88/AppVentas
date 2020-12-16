using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ExistenciasRestServiceAP
    {
        /*Método que consume la Api de Existencias de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarExistencias(string sIdAlmacen, string sParcial, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ExistenciasAP?sIdAlmacen=" + sIdAlmacen;
                string sUri = sConexionUri + "ExistenciasAP?sIdAlmacen=" + sIdAlmacen;
                var client = new RestClient(sUri);
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("accept", "application/json");
                IRestResponse response = client.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var content = response.Content;

                    if ((content != null) && (content != "null"))
                    {
                        var existenciasJson = JsonConvert.DeserializeObject<List<existencia>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<existencia>();

                        List<existencia> existencias = new List<existencia>();

                        foreach (var existenciaJson in existenciasJson)
                        {

                            existencia existencia = new existencia()
                            {
                                alc_clave = existenciaJson.alc_clave,
                                arc_clave = existenciaJson.arc_clave,
                                exn_existencia = existenciaJson.exn_existencia
                            };

                            existencias.Add(existencia);
                        }

                        dbConexion.InsertAll(existencias);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Existencias del Almacén " + sIdAlmacen + " fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Existencias del Almacén " + sIdAlmacen + "."
                        };
                    }
                }
                else
                {
                    return new StatusRestService
                    {
                        status = false,
                        mensaje = "Error: " + response.ErrorMessage
                    };
                }
            }
            catch (Exception exp)
            {
                return new StatusRestService
                {
                    status = false,
                    mensaje = "Error: " + exp.Message
                };
            }
        }
    }
}
