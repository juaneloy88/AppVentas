using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class CuotasRestServiceAP
    {
        /*Método que consume la Api de Cuotas de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarCuotas(int iIdRuta, char cDiaVisita, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "CuotasAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&dFecha=" + dFecha.ToString("yyyy - MM - dd");
                string sUri = sConexionUri + "CuotasAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&dFecha=" + dFecha.ToString("yyyy - MM - dd");
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
                        var cuotasJson = JsonConvert.DeserializeObject<List<cuota>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<cuota>();

                        List<cuota> cuotas = new List<cuota>();

                        foreach (var cuotaJson in cuotasJson)
                        {

                            cuota cuota = new cuota()
                            {
                                cliente = cuotaJson.cliente,
                                cartonesAnt = cuotaJson.cartonesAnt,
                                cartonesAct = cuotaJson.cartonesAct,
                                cartonesFal = cuotaJson.cartonesFal
                            };

                            cuotas.Add(cuota);
                        }

                        dbConexion.InsertAll(cuotas);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Cuotas de la Ruta " + iIdRuta.ToString() + " fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Cuotas a cargar de la Ruta " + iIdRuta.ToString() + "."
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
