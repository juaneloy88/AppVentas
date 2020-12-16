using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class OpcionesEncuestasRestServiceAP
    {
        /*Método que consume la Api de Opciones de Encuestas de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarOpcionesEncuestas(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "OpcionesEncuestasAP";
                string sUri = sConexionUri + "OpcionesEncuestasAP";
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
                        var opcionesEncuentasJson = JsonConvert.DeserializeObject<List<opciones>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<opciones>();

                        List<opciones> opcionesEncuentas = new List<opciones>();

                        foreach (var opcionEncuentaJson in opcionesEncuentasJson)
                        {
                            opciones opcionEncuenta = new opciones()
                            {
                                opn_id = opcionEncuentaJson.opn_id,
                                enn_id = opcionEncuentaJson.enn_id,
                                opn_descripcion = opcionEncuentaJson.opn_descripcion
                            };

                            opcionesEncuentas.Add(opcionEncuenta);
                        }

                        dbConexion.InsertAll(opcionesEncuentas);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Opciones de Encuestas fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Opciones de Encuestas a cargar.",
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
