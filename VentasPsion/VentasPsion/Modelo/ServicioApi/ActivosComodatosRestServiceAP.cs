using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ActivosComodatosRestServiceAP
    {
        /*Método que consume la Api de Activos Comodatados de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarActivosComodatosAP(int iIdRuta, char cDiaVisita, string sConexionUri, bool bTeleventa)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ActivosComodatosAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
                string sUri = sConexionUri + "ActivosComodatosAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
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
                        var activosComodatosJson = JsonConvert.DeserializeObject<List<activos_comodatos>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<activos_comodatos>();

                        List<activos_comodatos> activosComodatos = new List<activos_comodatos>();

                        foreach (var activoComodatoJson in activosComodatosJson)
                        {
                            activos_comodatos activoComodato = new activos_comodatos()
                            {
                                cln_clave = activoComodatoJson.cln_clave,
                                acn_cantidad = activoComodatoJson.acn_cantidad,
                                acc_descripcion = activoComodatoJson.acc_descripcion
                            };

                            activosComodatos.Add(activoComodato);
                        }

                        dbConexion.InsertAll(activosComodatos);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Activos Comodatados de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Activos Comodatados a cargar de la Ruta " + iIdRuta.ToString() + ".",
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
