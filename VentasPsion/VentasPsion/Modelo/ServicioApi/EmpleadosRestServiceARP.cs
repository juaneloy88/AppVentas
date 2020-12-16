using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class EmpleadosRestServiceARP
    {
        /*Método que consume la Api de Empleados de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarEmpleados(char cTipoVenta, string sTipoVenta, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "EmpleadosARP?cTipoVenta=" + cTipoVenta;
                string sUri = sConexionUri + "EmpleadosARP?cTipoVenta=" + cTipoVenta;
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
                        var empleadosJson = JsonConvert.DeserializeObject<List<empleados>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<empleados>();

                        List<empleados> listaEmpleados = new List<empleados>();

                        foreach (var empleado in empleadosJson)
                        {
                            listaEmpleados.Add(new empleados
                            {
                                emn_clave = empleado.emn_clave,
                                emc_nombre = empleado.emc_nombre,
                                emc_usuario = empleado.emc_usuario,
                                emc_password = empleado.emc_password,
                                emn_ruta = empleado.emn_ruta
                            });
                        }

                        dbConexion.InsertAll(listaEmpleados);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Empleados de " + sTipoVenta + " fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay Empleados de " + sTipoVenta + " a cargar."
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
