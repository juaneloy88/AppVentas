using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class EnvasesRestServiceAP
    {
        /*Método que consume la Api de Envases de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarEnvases(int iIdRuta, char cDiaVisita, string sConexionUri, string sTipoRecepcion, bool bTeleventa)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "EnvasesAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
                string sUri = sConexionUri + "EnvasesAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
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
                        var envasesJson = JsonConvert.DeserializeObject<List<envase>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        /*Se comentaron porque la información de dichas tablas se borra al inicio de la Recepción Total*/
                        //dbConexion.DeleteAll<envase>();

                        List<envase> envases = new List<envase>();

                        foreach (var envaseJson in envasesJson)
                        {
                            envase envase = new envase()
                            {
                                cln_clave = envaseJson.cln_clave,
                                //men_folio = "",
                                mec_envase = envaseJson.mec_envase,
                                men_saldo_inicial = envaseJson.men_saldo_inicial,
                                men_cargo = 0,
                                men_abono = 0
                            };

                            if (sTipoRecepcion == "PARCIAL")
                            {
                                var existeEnvase = dbConexion.Table<envase>().Where(x => ((x.cln_clave == envase.cln_clave) && (x.mec_envase == envase.mec_envase))).FirstOrDefault();

                                if (existeEnvase == null)
                                {
                                    envases.Add(envase);
                                }
                            }
                            else
                            {
                                envases.Add(envase);
                            }
                        }

                        dbConexion.InsertAll(envases);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Envases de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Envases a cargar de la Ruta " + iIdRuta.ToString() + "."
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
