using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class PasswordRestServiceARP
    {
        /*Método que consume la Api de Passwords de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarPasswordARP(char cTipoVenta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "PasswordARP?cTipoVenta=" + cTipoVenta + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "PasswordARP?cTipoVenta=" + cTipoVenta + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var passwordJson = JsonConvert.DeserializeObject<password>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<password>();

                        password password = new password()
                        {
                            ppf_fecha = passwordJson.ppf_fecha,
                            ppc_password = passwordJson.ppc_password,
                            ppc_password_ajustes = passwordJson.ppc_password_ajustes
                        };

                        dbConexion.Insert(password);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Passwords de la Fecha '" + dFecha.ToShortDateString() + "' fue cargado correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Passwords a cargar de la Fecha '" + dFecha.ToShortDateString() + "'."
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
