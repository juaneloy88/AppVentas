using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class PermisosAppRestServiceARP
    {
        /*Método que consume la Api de Permisos de la App de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarPermisosAppARP(char cTipoVenta, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "PermisosAppARP?cTipoVenta=" + cTipoVenta;
                string sUri = sConexionUri + "PermisosAppARP?cTipoVenta=" + cTipoVenta;
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
                        var permisosAppJson = JsonConvert.DeserializeObject<List<opciones_app>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<opciones_app>();

                        List<opciones_app> permisosApp = new List<opciones_app>();

                        foreach (var permisoAppJson in permisosAppJson)
                        {
                            opciones_app permisoApp = new opciones_app
                            {
                                oan_id = permisoAppJson.oan_id,
                                opn_descripcion = permisoAppJson.opn_descripcion,
                                oab_valor = permisoAppJson.oab_valor
                            };

                            permisosApp.Add(permisoApp);
                        }

                        dbConexion.InsertAll(permisosApp);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Permisos de la App fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Permisos de la App a cargar."
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
            catch (Exception exc)
            {
                return new StatusRestService
                {
                    status = false,
                    mensaje = "Error: " + exc.Message
                };
            }
        }
    }
}
