using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class SegmentosRestServiceAP
    {
        /*Método que consume la Api de Segmentos de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarSegmentosAP(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "SegmentosAP";
                string sUri = sConexionUri + "SegmentosAP";
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
                        var segmentosJson = JsonConvert.DeserializeObject<List<cat_segmentos>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<cat_segmentos>();

                        List<cat_segmentos> segmentos = new List<cat_segmentos>();

                        foreach (var segmentoJson in segmentosJson)
                        {
                            cat_segmentos segmento = new cat_segmentos()
                            {
                                csc_clave = segmentoJson.csc_clave,
                                csc_nombre = segmentoJson.csc_nombre
                            };

                            segmentos.Add(segmento);
                        }

                        dbConexion.InsertAll(segmentos);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Segmentos fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Segmentos a cargar.",
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
