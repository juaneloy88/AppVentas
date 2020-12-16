using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class PedidosSugeridosRestServiceAP
    {
        /*Método que consume la Api de Pedidos Sugeridos de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarPedidosSugeridosAP(int iIdRuta, char cDiaVisita, string sConexionUri, string sTipoRecepcion)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "PedidosSugeridosAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita;
                string sUri = sConexionUri + "PedidosSugeridosAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita;
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
                        var pedidosSugeridosJson = JsonConvert.DeserializeObject<List<pedido>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        /*Se comentaron porque la información de dichas tablas se borra al inicio de la Recepción Total*/
                        //dbConexion.DeleteAll<pedido>();

                        List<pedido> pedidosSugeridos = new List<pedido>();

                        foreach (var pedidoSugeridoJson in pedidosSugeridosJson)
                        {

                            pedido pedidoSugerido = new pedido()
                            {
                                cln_clave = pedidoSugeridoJson.cln_clave,
                                run_clave = pedidoSugeridoJson.run_clave,
                                arc_clave = pedidoSugeridoJson.arc_clave,
                                pen_prm_cantidad = pedidoSugeridoJson.pen_prm_cantidad
                            };

                            if (sTipoRecepcion == "PARCIAL")
                            {
                                var existePedidoSugerido = dbConexion.Table<pedido>().Where(x => ((x.cln_clave == pedidoSugerido.cln_clave) && (x.arc_clave == pedidoSugerido.arc_clave))).FirstOrDefault();

                                if (existePedidoSugerido == null)
                                {
                                    pedidosSugeridos.Add(pedidoSugerido);
                                }
                            }
                            else
                            {
                                pedidosSugeridos.Add(pedidoSugerido);
                            }
                        }

                        dbConexion.InsertAll(pedidosSugeridos);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Pedidos Sugeridos de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Pedidos Sugeridos a cargar de la Ruta " + iIdRuta.ToString() + ".",
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
