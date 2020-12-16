using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class Class1
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        public async Task<string> validaCierre(int iRuta)
        {
            string sRespuesta = string.Empty;

            try
            {
                //string sUri = "http://localhost:16329/api/entorno?iRuta=" + iRuta.ToString();
                string sUri = "http://192.168.2.42:16329/api/entorno?iRuta=1050";
                //string sUri = "http://192.168.2.42:16329/api/entorno?iRuta=" + iRuta.ToString();

                HttpClient httpClient = new HttpClient();

                var response = await httpClient.GetStringAsync(sUri);

                sRespuesta = response.ToString();

                return sRespuesta;

                //var client = new RestClient(sUri);
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("cache-control", "no-cache");
                //request.AddHeader("content-type", "application/json");
                //request.AddHeader("accept", "application/json");
                //IRestResponse response = client.Execute(request);

                //if (response.StatusCode == System.Net.HttpStatusCode.OK)

                //var content = response.Content;

                //var clientesJson = JsonConvert.DeserializeObject(content);

                //switch (clientesJson)
                //{
                //    case "":
                //        sRespuesta = "Ok";
                //        break;
                //    case "1":
                //        sRespuesta = "La Ruta ya se encuentra Cerrada";
                //        break;
                //    default:
                //        sRespuesta = clientesJson.ToString();
                //        break;
                //}

            }
            catch (Exception ex)
            {
                sRespuesta = "Error al Válidar Cierre: " + ex.ToString();
            }

            return sRespuesta;
        }

        public async Task<string> enviarVentaDetalle()
        {
            string sRespuesta = string.Empty;

            try
            {
                List<venta_detalle> lVentaDetalle = new List<venta_detalle>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from venta_detalle;";
                    lVentaDetalle = conn.Query<venta_detalle>(sQuery);
                }

                if (lVentaDetalle.Count >= 1)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string serializedVentaDetalle = JsonConvert.SerializeObject(lVentaDetalle);
                        var response = await httpClient.PostAsync("http://localhost:16329/api/venta_detalle",
                                                                  new StringContent(serializedVentaDetalle, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                        }
                        else
                        {
                        }
                    }

                    sRespuesta = "Ok";
                }
                else
                {
                    sRespuesta = "No existen datos en venta_detalle";
                }
            }
            catch (Exception ex)
            {
                sRespuesta = "Error: " + ex.ToString();
            }

            return sRespuesta;
        }

        public async Task<string> enviarEnvase()
        {
            string sRespuesta = string.Empty;

            try
            {
                List<envase> lEnvase = new List<envase>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from envase;";
                    lEnvase = conn.Query<envase>(sQuery);
                }

                if (lEnvase.Count >= 1)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        string serializedEnvase = JsonConvert.SerializeObject(lEnvase);
                        var response = await httpClient.PostAsync("http://localhost:16329/api/envase",
                                                                  new StringContent(serializedEnvase, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                        }
                        else
                        {
                        }
                    }

                    sRespuesta = "Ok";
                }
                else
                {
                    sRespuesta = "No existen datos en envase";
                }
            }
            catch (Exception ex)
            {
                sRespuesta = "Error: " + ex.ToString();
            }

            return sRespuesta;
        }
    }
}
