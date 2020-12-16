using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class VolumenVentasRestServiceAP
    {
        /*Método que consume la Api de Volumen de Ventas de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarVolumenVentasAP(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "VolumenVentasAP?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "VolumenVentasAP?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var volumenesVentasJson = JsonConvert.DeserializeObject<List<volumen_ventas>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<volumen_ventas>();

                        List<volumen_ventas> volumenesVentas = new List<volumen_ventas>();

                        foreach (var volumenVentaJson in volumenesVentasJson)
                        {
                            volumen_ventas volumenVenta = new volumen_ventas()
                            {
                                run_clave = volumenVentaJson.run_clave,
                                vvc_tipo = volumenVentaJson.vvc_tipo,
                                vvn_cuota_dia = volumenVentaJson.vvn_cuota_dia,
                                vvn_tendencia_cartones = volumenVentaJson.vvn_tendencia_cartones,
                                vvn_faltantes = volumenVentaJson.vvn_faltantes,
                                vvn_porcentaje = volumenVentaJson.vvn_porcentaje,
                                vvn_tendencia_porcentaje = volumenVentaJson.vvn_tendencia_porcentaje,
                                vvd_fecha = volumenVentaJson.vvd_fecha
                            };

                            volumenesVentas.Add(volumenVenta);
                        }

                        dbConexion.InsertAll(volumenesVentas);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "El Volumen de Ventas de la Ruta " + iIdRuta.ToString() + " fue cargado correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Volumen de Ventas a cargar de la Ruta " + iIdRuta.ToString() + ".",
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
