using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class CandadosProductosRestServiceAPv2
    {
        /*Método que consume la Api de Candados de Productos de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarCandadosProductosAPv2(DateTime dFechaInicio, DateTime dFechaFin, string sParcial, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "CandadosProductosAPv2";
                string sUri = sConexionUri + "CandadosProductosAPv2?dFechaInicio=" + dFechaInicio.ToString("yyyy-MM-dd") + "&dFechaFin=" + dFechaFin.ToString("yyyy-MM-dd");
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
                        var candadosProductosJson = JsonConvert.DeserializeObject<List<candados_productos>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<candados_productos>();

                        List<candados_productos> candadosProductos = new List<candados_productos>();

                        foreach (var candadoProductoJson in candadosProductosJson)
                        {
                            candados_productos candadoProducto = new candados_productos()
                            {
                                arc_clave = candadoProductoJson.arc_clave,
                                cpn_tipo = candadoProductoJson.cpn_tipo,
                                cpn_cantidad = candadoProductoJson.cpn_cantidad,
                                cpc_mod = candadoProductoJson.cpc_mod,
                                cpc_segmento = candadoProductoJson.cpc_segmento
                            };

                            candadosProductos.Add(candadoProducto);
                        }

                        dbConexion.InsertAll(candadosProductos);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Candados de Productos fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Candados de Productos a cargar.",
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
