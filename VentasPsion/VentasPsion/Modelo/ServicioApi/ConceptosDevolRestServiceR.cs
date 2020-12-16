using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ConceptosDevolRestServiceR
    {
        /*Método que consume la Api de Conceptos de Devolución de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarConceptosDevol(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ConceptosDevolR";
                string sUri = sConexionUri + "ConceptosDevolR";
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
                        var conceptosDevolJson = JsonConvert.DeserializeObject<List<concepto_devoluciones>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<concepto_devoluciones>();

                        List<concepto_devoluciones> conceptosDevol = new List<concepto_devoluciones>();

                        foreach (var conceptoDevolJson in conceptosDevolJson)
                        {

                            concepto_devoluciones conceptoDevol = new concepto_devoluciones()
                            {
                                cdn_clave = conceptoDevolJson.cdn_clave,
                                cdc_descripcion = conceptoDevolJson.cdc_descripcion
                            };

                            conceptosDevol.Add(conceptoDevol);
                        }

                        dbConexion.InsertAll(conceptosDevol);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Conceptos de Devolución fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Conceptos de Devolución a cargar."
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
