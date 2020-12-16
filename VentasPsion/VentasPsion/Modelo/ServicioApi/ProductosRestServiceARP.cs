using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ProductosRestServiceARP
    {
        /*Método que consume la Api de Productos de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarProductos(string sParcial, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ProductosARP";
                string sUri = sConexionUri + "ProductosARP";
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
                        var productosJson = JsonConvert.DeserializeObject<List<productos>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<productos>();

                        List<productos> productos = new List<productos>();

                        foreach (var productoJson in productosJson)
                        {

                            productos producto = new productos()
                            {
                                arc_clave = productoJson.arc_clave,
                                arc_envase = productoJson.arc_envase,
                                arn_puntos = productoJson.arn_puntos,
                                ard_corta = productoJson.ard_corta,
                                arc_estatus = productoJson.arc_estatus,
                                arc_produ = productoJson.arc_produ,
                                ard_descripcion = productoJson.ard_descripcion,
                                pdc_prom_envase = productoJson.pdc_prom_envase,
                                arc_afecta = productoJson.arc_afecta,
                                arc_clasif_estadistica = productoJson.arc_clasif_estadistica,
                                cmc_clave = productoJson.cmc_clave,
                                cuc_clave = productoJson.cuc_clave,
                                arc_contado = productoJson.arc_contado
                            };

                            productos.Add(producto);
                        }

                        dbConexion.InsertAll(productos);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "La información de los Productos fue cargada correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay información a cargar de los Prodcutos."

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
