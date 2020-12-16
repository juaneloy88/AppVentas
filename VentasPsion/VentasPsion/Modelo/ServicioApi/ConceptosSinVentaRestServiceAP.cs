using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ConceptosNoVentaRestServiceAP
    {
        /*Método que consume la Api de Conceptos de No Venta de AUTOVENTA Y REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarConceptosNoVenta(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ConceptosSinVentaAP";
                string sUri = sConexionUri + "ConceptosSinVentaAP";
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
                        var conceptosNoVentaJson = JsonConvert.DeserializeObject<List<conseptos_no_venta>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<conseptos_no_venta>();

                        List<conseptos_no_venta> conceptosNoVenta = new List<conseptos_no_venta>();

                        foreach (var conceptoNoVentaJson in conceptosNoVentaJson)
                        {

                            conseptos_no_venta conceptoNoVenta = new conseptos_no_venta()
                            {
                                svn_clave = conceptoNoVentaJson.svn_clave,
                                svc_descripcion = conceptoNoVentaJson.svc_descripcion
                            };

                            conceptosNoVenta.Add(conceptoNoVenta);
                        }

                        dbConexion.InsertAll(conceptosNoVenta);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Conceptos de No Venta fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Conceptos de No Venta a cargar."
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
