using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class PromocionesRestServiceAP
    {
        /*Método que consume la Api de Promociones de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarPromociones(DateTime dFecha,string sParcial, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "PromocionesAP?dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "PromocionesAP?dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var promocionesJson = JsonConvert.DeserializeObject<List<promociones>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<promociones>();

                        List<promociones> promociones = new List<promociones>();

                        foreach (var promocionJson in promocionesJson)
                        {
                            promociones promocion = new promociones()
                            {
                                ppn_numero_promocion = promocionJson.ppn_numero_promocion,
                                ppc_codigo_venta = promocionJson.ppc_codigo_venta,
                                ppn_cantidad_venta = promocionJson.ppn_cantidad_venta,
                                ppc_codigo_regalo = promocionJson.ppc_codigo_regalo,
                                ppn_cantidad_regalo = promocionJson.ppn_cantidad_regalo,
                                ppc_inicia = promocionJson.ppc_inicia,
                                ppc_termina = promocionJson.ppc_termina,
                                ppc_descripcion = promocionJson.ppc_descripcion,
                                ppc_tipo = promocionJson.ppc_tipo
                            };

                            promociones.Add(promocion);
                        }

                        dbConexion.InsertAll(promociones);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Promociones fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Promociones a cargar.",
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
