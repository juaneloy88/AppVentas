using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class DetallesVentasRestServiceR
    {
        /*Método que consume la Api de Detalles de Ventas de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarDetallesVentasR(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "VentaDetallesR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "VentaDetallesR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var ventasDetallesJson = JsonConvert.DeserializeObject<List<venta_detalle>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<venta_detalle>();

                        List<venta_detalle> ventasDetalles = new List<venta_detalle>();

                        foreach (var ventaDetalleJson in ventasDetallesJson)
                        {
                            venta_detalle ventaDetalle = new venta_detalle()
                            {
                                prn_secuencia = ventaDetalleJson.prn_secuencia,
                                vdn_folio = ventaDetalleJson.vdn_folio,
                                vdn_cliente = ventaDetalleJson.vdn_cliente,
                                run_clave = ventaDetalleJson.run_clave,
                                vdn_producto = ventaDetalleJson.vdn_producto,
                                vdn_venta = ventaDetalleJson.vdn_venta,
                                vdn_precio = ventaDetalleJson.vdn_precio,
                                vdn_importe = ventaDetalleJson.vdn_importe,
                                vdd_descripcion = ventaDetalleJson.vdd_descripcion,
                                lmn_precioneto = ventaDetalleJson.lmn_precioneto,
                                lmn_ieps = ventaDetalleJson.lmn_ieps,
                                lmn_preciobruto = ventaDetalleJson.lmn_preciobruto,
                                lmn_iva = ventaDetalleJson.lmn_iva,
                                lmn_preciofinal = ventaDetalleJson.lmn_preciofinal,
                                lmc_porcentaje = ventaDetalleJson.lmc_porcentaje,
                                lmn_descuento_neto = ventaDetalleJson.lmn_descuento_neto,
                                lmn_iepsdescuento = ventaDetalleJson.lmn_iepsdescuento,
                                lmn_descuento_bruto = ventaDetalleJson.lmn_descuento_bruto,
                                lmn_ivadescuento = ventaDetalleJson.lmn_ivadescuento,
                                lmn_totaldescuento = ventaDetalleJson.lmn_totaldescuento,
                                lmn_preciolm_neto = ventaDetalleJson.lmn_preciolm_neto,
                                lmn_preciolm_ipes = ventaDetalleJson.lmn_preciolm_ipes,
                                lmn_preciolm_bruto = ventaDetalleJson.lmn_preciolm_bruto,
                                lmn_preciolm_iva = ventaDetalleJson.lmn_preciolm_iva,
                                lmn_preciolm = ventaDetalleJson.lmn_preciolm
                            };

                            ventasDetalles.Add(ventaDetalle);
                        }

                        dbConexion.InsertAll(ventasDetalles);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Detalles de Ventas de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Detalles de Ventas a cargar de la Ruta " + iIdRuta.ToString() + "."
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
