using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class BonificacionesRestServiceAP
    {
        /*Método que consume la Api de Bonificaciones de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarBonificaciones(int iIdRuta, char cDiaVisita, string sConexionUri, string sTipoRecepcion, bool bTeleventa)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "BonificacionesAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
                string sUri = sConexionUri + "BonificacionesAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
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
                        var bonificacionesJson = JsonConvert.DeserializeObject<List<bonificaciones>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        /*Se comentaron porque la información de dichas tablas se borra al inicio de la Recepción Total*/
                        //dbConexion.DeleteAll<bonificaciones>();

                        List<bonificaciones> bonificaciones = new List<bonificaciones>();

                        foreach (var bonificacionJson in bonificacionesJson)
                        {
                            bonificaciones bonificacion = new bonificaciones()
                            {
                                boc_cliente = bonificacionJson.boc_cliente,
                                boc_folio = bonificacionJson.boc_folio,
                                boi_documento = bonificacionJson.boi_documento,
                                boc_tipo = bonificacionJson.boc_tipo
                            };

                            if (sTipoRecepcion == "PARCIAL")
                            {
                                var existeBonificacion = dbConexion.Table<bonificaciones>().Where(x => ((x.boc_cliente == bonificacion.boc_cliente) && (x.boc_folio == bonificacion.boc_folio))).FirstOrDefault();

                                if (existeBonificacion == null)
                                {
                                    bonificaciones.Add(bonificacion);
                                }
                            }
                            else
                            {
                                bonificaciones.Add(bonificacion);
                            }
                        }

                        dbConexion.InsertAll(bonificaciones);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Bonificaciones de la Ruta " + iIdRuta.ToString() + " fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Bonificaciones a cargar de la Ruta " + iIdRuta.ToString() + ".",
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
