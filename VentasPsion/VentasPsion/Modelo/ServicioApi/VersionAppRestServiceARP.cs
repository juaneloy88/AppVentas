using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class VersionAppRestServiceARP
    {
        /*Método que consume la Api de Version de la App de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarVersionAppARP(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "VersionAppARP";
                string sUri = sConexionUri + "VersionAppARP";
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
                        var versionAppJson = JsonConvert.DeserializeObject<version_app>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<version_app>();

                        version_app versionApp = new version_app()
                        {
                            vpf_fecha = versionAppJson.vpf_fecha,
                            vpc_version = versionAppJson.vpc_version
                        };

                        dbConexion.Insert(versionApp);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "La Versión de la App fue cargada correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Versión de la App a cargar."
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
