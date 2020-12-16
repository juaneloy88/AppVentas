using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class TicketsCabecerasRestServiceA
    {
        public List<tickets_cabeceras> lstTicketsCabeceras = null;

        /*Método que consume la Api que regresa de la BD del ERP, los datos de las Cabeceras de un específico Folio de Ticket de un Cliente para poder efectuar las Devoluciones en AUTOVENTA*/
        public List<tickets_cabeceras> FtnRegresarTicketsCabecerasA(int iIdCliente, string sNoTicket, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "TicketsCabecerasA?iIdCliente=" + iIdRuta.ToString() + "&sNoTicket=" + sNoTicket;
                string sUri = sConexionUri + "TicketsCabecerasA?iIdCliente=" + iIdCliente.ToString() + "&sNoTicket=" + sNoTicket;
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
                        var ticketsCabecerasJson = JsonConvert.DeserializeObject<List<tickets_cabeceras>>(content);

                        lstTicketsCabeceras = new List<tickets_cabeceras>();

                        foreach (var ticketCabeceraJson in ticketsCabecerasJson)
                        {
                            tickets_cabeceras ticketCabecera = new tickets_cabeceras()
                            {
                                tcn_cliente = ticketCabeceraJson.tcn_cliente,
                                tcc_folio = ticketCabeceraJson.tcc_folio,
                                tcf_movimiento = ticketCabeceraJson.tcf_movimiento,
                                tcn_importe = ticketCabeceraJson.tcn_importe,
                                tcc_cadena_original = ticketCabeceraJson.tcc_cadena_original,
                                tcb_esta_vencido = ticketCabeceraJson.tcb_esta_vencido,
                                tcb_tiene_complemento = ticketCabeceraJson.tcb_tiene_complemento
                            };

                            lstTicketsCabeceras.Add(ticketCabecera);
                        }

                        return lstTicketsCabeceras;
                    }
                    else
                    {
                        return lstTicketsCabeceras;
                    }
                }
                else
                {
                    return lstTicketsCabeceras;
                }
            }
            catch (Exception exp)
            {
                return lstTicketsCabeceras;
            }
        }
    }
}
