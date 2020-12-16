using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class OrdenTicketsRestServiceARP
    {
        /*Método que consume la Api de Orden de Tickets de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarOrdenTickets(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "OrdenTicketsARP";
                string sUri = sConexionUri + "OrdenTicketsARP";
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
                        var ordenTicketsJson = JsonConvert.DeserializeObject<List<orden_ticket>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<orden_ticket>();

                        List<orden_ticket> ordenTickets = new List<orden_ticket>();

                        foreach (var ordenTicketJson in ordenTicketsJson)
                        {

                            orden_ticket ordenTicket = new orden_ticket()
                            {
                                arc_clave = ordenTicketJson.arc_clave,
                                oin_orden = ordenTicketJson.oin_orden,
                                cud_descripcion = ordenTicketJson.cud_descripcion
                            };

                            ordenTickets.Add(ordenTicket);
                        }

                        dbConexion.InsertAll(ordenTickets);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "El Orden de Tickets fueron cargado correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Orden de Tickets a cargar."
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
