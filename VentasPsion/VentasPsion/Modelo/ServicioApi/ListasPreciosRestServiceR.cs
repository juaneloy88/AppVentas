using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ListasPreciosRestServiceR
    {
        /*Método que consume la Api de Listas de Precios de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarListasPreciosR(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ListasPreciosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "ListasPreciosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var listasPreciosJson = JsonConvert.DeserializeObject<List<lista_maestra>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<lista_maestra>();

                        List<lista_maestra> listasPrecios = new List<lista_maestra>();

                        foreach (var listaPrecioJson in listasPreciosJson)
                        {

                            lista_maestra listaPrecio = new lista_maestra()
                            {
                                lmc_tipo = listaPrecioJson.lmc_tipo,
                                lmc_producto = listaPrecioJson.lmc_producto,
                                lmc_precio = listaPrecioJson.lmc_precio,
                                lmd_estatus = listaPrecioJson.lmd_estatus,
                                lmn_precioneto = listaPrecioJson.lmn_precioneto,
                                lmn_ieps = listaPrecioJson.lmn_ieps,
                                lmn_preciobruto = listaPrecioJson.lmn_preciobruto,
                                lmn_iva = listaPrecioJson.lmn_iva,
                                lmn_preciofinal = listaPrecioJson.lmn_preciofinal,
                                lmc_porcentaje = listaPrecioJson.lmc_porcentaje,
                                lmn_descuento_neto = listaPrecioJson.lmn_descuento_neto,
                                lmn_iepsdescuento = listaPrecioJson.lmn_iepsdescuento,
                                lmn_descuento_bruto = listaPrecioJson.lmn_descuento_bruto,
                                lmn_ivadescuento = listaPrecioJson.lmn_ivadescuento,
                                lmn_totaldescuento = listaPrecioJson.lmn_totaldescuento,
                                lmn_preciolm_neto = listaPrecioJson.lmn_preciolm_neto,
                                lmn_preciolm_ipes = listaPrecioJson.lmn_preciolm_ipes,
                                lmn_preciolm_bruto = listaPrecioJson.lmn_preciolm_bruto,
                                lmn_preciolm_iva = listaPrecioJson.lmn_preciolm_iva,
                                lmn_preciolm = listaPrecioJson.lmn_preciolm
                            };

                            listasPrecios.Add(listaPrecio);
                        }

                        dbConexion.InsertAll(listasPrecios);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Listas de Precios de la Ruta " + iIdRuta.ToString() + " fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Listas de Precios a cargar de la Ruta " + iIdRuta.ToString() + "."
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
