using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class RetosDiariosRestServiceAP
    {
        /*Método que consume la Api de Reto Diario de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarRetosDiariosAP(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "RetoDiarioAP?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "RetoDiarioAP?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var retosDiariosJson = JsonConvert.DeserializeObject<List<reto_diario>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<reto_diario>();

                        List<reto_diario> retosDiarios = new List<reto_diario>();

                        foreach (var retoDiarioJson in retosDiariosJson)
                        {
                            reto_diario retoDiario = new reto_diario()
                            {
                                run_clave = retoDiarioJson.run_clave,
                                rdc_nombre = retoDiarioJson.rdc_nombre,
                                rdn_cantidad = retoDiarioJson.rdn_cantidad,
                                rdd_fecha = retoDiarioJson.rdd_fecha
                            };

                            retosDiarios.Add(retoDiario);
                        }

                        dbConexion.InsertAll(retosDiarios);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Retos Diarios de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Retos Diarios a cargar de la Ruta " + iIdRuta.ToString() + ".",
                            valor = "NO"
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
