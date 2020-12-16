using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class TicketsDetallesRestServiceA
    {
        public List<venta_detalle> lstTicketsDetalles = null;

        /*Método que consume la Api que regresa de la BD del ERP, los datos de los Detalles de un específico Folio de Ticket de un Cliente para poder efectuar las Devoluciones en AUTOVENTA*/
        public List<venta_detalle> FtnRegresarTicketsDetallesA(int iIdCliente, string sNoTicket, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "TicketsDetallesA?iIdCliente=" + iIdRuta.ToString() + "&sNoTicket=" + sNoTicket;
                string sUri = sConexionUri + "TicketsDetallesA?iIdCliente=" + iIdCliente.ToString() + "&sNoTicket=" + sNoTicket + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var ticketsDetallesJson = JsonConvert.DeserializeObject<List<venta_detalle>>(content);

                        lstTicketsDetalles = new List<venta_detalle>();

                        foreach (var ticketDetalleJson in ticketsDetallesJson)
                        {
                            venta_detalle ticketDetalle = new venta_detalle()
                            {
                                prn_secuencia = ticketDetalleJson.prn_secuencia,
                                vdn_folio = ticketDetalleJson.vdn_folio,
                                vdn_cliente = ticketDetalleJson.vdn_cliente,
                                run_clave = ticketDetalleJson.run_clave,
                                vdn_producto = ticketDetalleJson.vdn_producto,
                                vdn_venta = ticketDetalleJson.vdn_venta,
                                vdn_precio = ticketDetalleJson.vdn_precio,
                                vdn_importe = ticketDetalleJson.vdn_importe,
                                vdd_descripcion = ticketDetalleJson.vdd_descripcion,
                                lmn_precioneto = ticketDetalleJson.lmn_precioneto,
                                lmn_ieps = ticketDetalleJson.lmn_ieps,
                                lmn_preciobruto = ticketDetalleJson.lmn_preciobruto,
                                lmn_iva = ticketDetalleJson.lmn_iva,
                                lmn_preciofinal = ticketDetalleJson.lmn_preciofinal,
                                lmc_porcentaje = ticketDetalleJson.lmc_porcentaje,
                                lmn_descuento_neto = ticketDetalleJson.lmn_descuento_neto,
                                lmn_iepsdescuento = ticketDetalleJson.lmn_iepsdescuento,
                                lmn_descuento_bruto = ticketDetalleJson.lmn_descuento_bruto,
                                lmn_ivadescuento = ticketDetalleJson.lmn_ivadescuento,
                                lmn_totaldescuento = ticketDetalleJson.lmn_totaldescuento,
                                lmn_preciolm_neto = ticketDetalleJson.lmn_preciolm_neto,
                                lmn_preciolm_ipes = ticketDetalleJson.lmn_preciolm_ipes,
                                lmn_preciolm_bruto = ticketDetalleJson.lmn_preciolm_bruto,
                                lmn_preciolm_iva = ticketDetalleJson.lmn_preciolm_iva,
                                lmn_preciolm = ticketDetalleJson.lmn_preciolm
                            };

                            lstTicketsDetalles.Add(ticketDetalle);
                        }

                        return lstTicketsDetalles;
                    }
                    else
                    {
                        return lstTicketsDetalles;
                    }
                }
                else
                {
                    return lstTicketsDetalles;
                }
            }
            catch (Exception exp)
            {
                return lstTicketsDetalles;
            }
        }
    }
}
