using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class FechaRepartoRestServiceR
    {
        /*Método que consume la Api de Fecha de Reparto de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnObtenerFechaReparto(DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "FechaRepartoR?dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "FechaRepartoR?dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var fechaRepartoJson = JsonConvert.DeserializeObject<fecha_reparto>(content);

                        fecha_reparto fecha_reparto = new fecha_reparto()
                        {
                            fechaReparto = fechaRepartoJson.fechaReparto
                        };

                        DateTime dFechaReparto = fecha_reparto.fechaReparto;

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "La Fecha de Reparto fue obtenida correctamente.",
                            fecha = dFechaReparto
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No se pudo obtener Fecha de Reparto."
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
