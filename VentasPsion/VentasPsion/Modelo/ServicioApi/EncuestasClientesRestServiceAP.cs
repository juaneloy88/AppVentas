using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class EncuestasClientesRestServiceAP
    {
        /*Método que consume la Api de Encuentas a Clientes de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarEncuestasClientesAP(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "EncuentasClientesAP";
                string sUri = sConexionUri + "EncuentasClientesAP";
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
                        var encuestasClientesJson = JsonConvert.DeserializeObject<List<encuesta>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<encuesta>();

                        List<encuesta> encuestasClientes = new List<encuesta>();

                        foreach (var encuestaClienteJson in encuestasClientesJson)
                        {
                            encuesta encuestaCliente = new encuesta()
                            {
                                enn_id = encuestaClienteJson.enn_id,
                                enc_pregunta = encuestaClienteJson.enc_pregunta,
                                enn_tipo_respuesta = encuestaClienteJson.enn_tipo_respuesta
                            };

                            encuestasClientes.Add(encuestaCliente);
                        }

                        dbConexion.InsertAll(encuestasClientes);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Encuentas a Clientes fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Encuentas a Clientes a cargar.",
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
