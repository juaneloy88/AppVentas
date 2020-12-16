using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class EnvasesSugeridosRestServiceR
    {
        /*Método que consume la Api de Envases Sugeridos de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarEnvasesSugeridosR(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "EnvasesSugeridosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "EnvasesSugeridosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var envasesSugeridosJson = JsonConvert.DeserializeObject<List<envase_sugerido>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<envase_sugerido>();

                        List<envase_sugerido> envasesSugeridos = new List<envase_sugerido>();

                        using (SQLiteConnection conn = dbConexion)
                        {
                            foreach (var envaseSugeridoJson in envasesSugeridosJson)
                            {
                                envase_sugerido envaseSugerido = new envase_sugerido
                                {
                                    cln_clave = envaseSugeridoJson.cln_clave,
                                    esc_envase = envaseSugeridoJson.esc_envase,
                                    esn_cantidad_vacio = envaseSugeridoJson.esn_cantidad_vacio,
                                    esn_cantidad_lleno = envaseSugeridoJson.esn_cantidad_lleno
                                };

                                envasesSugeridos.Add(envaseSugerido);
                            }

                            conn.InsertAll(envasesSugeridos);

                            return new StatusRestService
                            {
                                status = true,
                                mensaje = "Los Envases Sugeridos de la Ruta " + iIdRuta.ToString() + " fueron cargadas correctamente."
                            };
                        }
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            /*Líneas de código para que la carga sea OBLIGATORIA*/
                            status = false,
                            mensaje = "No hay Envases Sugeridos a cargar de la Ruta " + iIdRuta.ToString() + "."

                            /*Líneas de código para que la carga sea OPCIONAL*/
                            //status = true,
                            //mensaje = "No hay Envases Sugeridos a cargar de la Ruta " + iIdRuta.ToString() + ".",
                            //valor = "NO"
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
            catch (Exception exc)
            {
                return new StatusRestService
                {
                    status = false,
                    mensaje = "Error: " + exc.Message
                };
            }
        }
    }
}
