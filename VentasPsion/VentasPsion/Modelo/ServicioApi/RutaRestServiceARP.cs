using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class RutaRestServiceARP
    {
        /*Método que consume la Api de Rutas de AUTOVENTA, REPARTO y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarRuta(int iIdRuta, char cTipoVenta, string sTipoVenta, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "RutaARP?iIdRuta=" + iIdRuta.ToString() + "&cTipoVenta=" + cTipoVenta;
                string sUri = sConexionUri + "RutaARP?iIdRuta=" + iIdRuta.ToString() + "&cTipoVenta=" + cTipoVenta;
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
                        var rutaJson = JsonConvert.DeserializeObject<ruta>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<ruta>();

                        ruta ruta = new ruta()
                        {
                            run_clave = rutaJson.run_clave,
                            ruc_descripcion = rutaJson.ruc_descripcion,
                            run_rfid = rutaJson.run_rfid,
                            run_folio = rutaJson.run_folio,
                            alc_clave = rutaJson.alc_clave
                        };

                        string sAlmacen = rutaJson.alc_clave;

                        dbConexion.Insert(ruta);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "La información de la Ruta " + iIdRuta.ToString() + " fue cargada correctamente.",
                            valor = sAlmacen
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = false,
                            mensaje = "No hay información a cargar de la Ruta " + iIdRuta.ToString() + " porque no existe, está inactiva o no pertenece a " + sTipoVenta + ".",
                            valor = ""
                        };
                    }
                }
                else
                {
                    return new StatusRestService
                    {
                        status = false,
                        mensaje = "Error: " + response.ErrorMessage,
                        valor = ""
                    };
                }
            }
            catch (Exception exp)
            {
                return new StatusRestService
                {
                    status = false,
                    mensaje = "Error: " + exp.Message,
                    valor = ""
                };
            }
        }
    }
}
