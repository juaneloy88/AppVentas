using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class KpisRestServiceAP
    {
        /*Método que consume la Api de KPI's de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarKpisAP(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "KpisAP?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "KpisAP?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var kpisJson = JsonConvert.DeserializeObject<List<kpis>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<kpis>();

                        List<kpis> kpis = new List<kpis>();

                        foreach (var kpiJson in kpisJson)
                        {
                            kpis kpi = new kpis()
                            {
                                pkn_orden = kpiJson.pkn_orden,
                                pkn_ruta = kpiJson.pkn_ruta,
                                pkc_descripcion = kpiJson.pkc_descripcion,
                                pkn_cuota = kpiJson.pkn_cuota,
                                pkn_venta = kpiJson.pkn_venta,
                                pkn_diferencia = kpiJson.pkn_diferencia,
                                pkn_porcentaje = kpiJson.pkn_porcentaje
                            };

                            kpis.Add(kpi);
                        }

                        dbConexion.InsertAll(kpis);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los KPI's de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay KPI's a cargar de la Ruta " + iIdRuta.ToString() + ".",
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
