using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class DepartamentosRestServiceARP
    {
        /*Método que consume la Api de Departamentos de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarDepartamentos(string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "DepartamentosARP";
                string sUri = sConexionUri + "DepartamentosARP";
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
                        var departamentosJson = JsonConvert.DeserializeObject<List<departamentos>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<departamentos>();

                        List<departamentos> departamentos = new List<departamentos>();

                        foreach (var departamentoJson in departamentosJson)
                        {

                            departamentos departamento = new departamentos()
                            {
                                dpn_clave = departamentoJson.dpn_clave,
                                dpc_descripcion = departamentoJson.dpc_descripcion
                            };

                            departamentos.Add(departamento);
                        }

                        dbConexion.InsertAll(departamentos);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Departamentos fueron cargados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Departamentos a cargar.",
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
