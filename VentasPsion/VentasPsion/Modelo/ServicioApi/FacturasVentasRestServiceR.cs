using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class FacturasVentasRestServiceR
    {
        /*Método que consume la Api de Facturas de Ventas de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarFacturasVentasR(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "FacturasVentasR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "FacturasVentasR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var listaFacturasVentasJson = JsonConvert.DeserializeObject<List<FacturasVenta>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<FacturasVenta>();

                        List<FacturasVenta> listaFacturasVentas = new List<FacturasVenta>();

                        using (SQLiteConnection conn = dbConexion)
                        {
                            foreach (var facturaVentaJson in listaFacturasVentasJson)
                            {
                                FacturasVenta facturaVenta = new FacturasVenta()
                                {
                                    vcn_ruta = facturaVentaJson.vcn_ruta,
                                    vcn_cliente = facturaVentaJson.vcn_cliente,
                                    vcc_cadena_original = facturaVentaJson.vcc_cadena_original,
                                    vcf_movimiento = facturaVentaJson.vcf_movimiento,
                                    vcn_folio = facturaVentaJson.vcn_folio,
                                    fvc_tipo = facturaVentaJson.fvc_tipo,                                  
                                    vcn_importe= facturaVentaJson.vcn_importe,
                                    fvn_pagos = facturaVentaJson.fvn_pagos
                                };

                                listaFacturasVentas.Add(facturaVenta);
                            }

                            conn.InsertAll(listaFacturasVentas);

                            return new StatusRestService
                            {
                                status = true,
                                mensaje = "Las Facturas de Ventas de la Ruta " + iIdRuta.ToString() + " fueron cargadas correctamente."
                            };
                        }
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Facturas de Ventas a cargar de la Ruta " + iIdRuta.ToString() + ".",
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
