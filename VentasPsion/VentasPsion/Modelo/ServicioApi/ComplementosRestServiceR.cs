using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class ComplementosRestServiceR
    {
        /*Método que consume la Api de Complementos de Pago de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        //public StatusRestService FtnCargarComplementosPagoR(int iIdRuta, DateTime dFecha, string sConexionUri)
        //{
        //    try
        //    {
        //        //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ComplementosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
        //        string sUri = sConexionUri + "ComplementosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
        //        var client = new RestClient(sUri);
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "application/json");
        //        request.AddHeader("accept", "application/json");
        //        IRestResponse response = client.Execute(request);

        //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //        {
        //            var content = response.Content;

        //            if ((content != null) && (content != "null"))
        //            {
        //                var complementosJson = JsonConvert.DeserializeObject<List<complementos>>(content);

        //                var dbConexion = new conexionDB().CadenaConexion();

        //                dbConexion.DeleteAll<complementos>();

        //                List<complementos> complementos = new List<complementos>();

        //                using (SQLiteConnection conn = dbConexion)
        //                {
        //                    foreach (var complementoJson in complementosJson)
        //                    {
        //                        complementos complemento = new complementos
        //                        {
        //                            fcd_uuid = complementoJson.fcd_uuid,
        //                            cln_clave = complementoJson.cln_clave,
        //                            fcn_foliofactura = complementoJson.fcn_foliofactura,
        //                            ccf_fechamov = complementoJson.ccf_fechamov,
        //                            ccn_imptotal = complementoJson.ccn_imptotal,
        //                            cdd_saldoinsoluto = complementoJson.cdd_saldoinsoluto
        //                        };

        //                        complementos.Add(complemento);
        //                    }

        //                    conn.InsertAll(complementos);

        //                    return new StatusRestService
        //                    {
        //                        status = true,
        //                        mensaje = "Los Complementos de Pago de la Ruta " + iIdRuta.ToString() + " fueron cargadas correctamente."
        //                    };
        //                }
        //            }
        //            else
        //            {
        //                return new StatusRestService
        //                {
        //                    status = true,
        //                    mensaje = "No hay Complementos de Pago a cargar de la Ruta " + iIdRuta.ToString() + ".",
        //                    valor = "NO"
        //                };
        //            }
        //        }
        //        else
        //        {
        //            return new StatusRestService
        //            {
        //                status = false,
        //                mensaje = "Error: " + response.ErrorMessage
        //            };
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        return new StatusRestService
        //        {
        //            status = false,
        //            mensaje = "Error: " + exc.Message
        //        };
        //    }
        //}
    }
}
