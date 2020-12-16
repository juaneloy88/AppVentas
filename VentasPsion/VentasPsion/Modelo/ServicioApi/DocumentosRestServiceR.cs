using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class DocumentosRestServiceR
    {
        /*Método que consume la Api de Documentos de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarDocumentosR(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "DocumentosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "DocumentosR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var listaDocumentosJson = JsonConvert.DeserializeObject<List<documentos_cabecera>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        List<documentos_cabecera> listaDocumentos = new List<documentos_cabecera>();

                        using (SQLiteConnection conn = dbConexion)
                        {
                            foreach (var documentoJson in listaDocumentosJson)
                            {
                                documentos_cabecera documento = new documentos_cabecera
                                {
                                    vcn_cliente = documentoJson.vcn_cliente,
                                    vcn_folio = documentoJson.vcn_folio,
                                    vcf_movimiento = documentoJson.vcf_movimiento,
                                    vcn_importe = documentoJson.vcn_importe,
                                    dcc_tipo = documentoJson.dcc_tipo,
                                    vcc_cadena_original = documentoJson.vcc_cadena_original,
                                    dcn_saldo = documentoJson.dcn_saldo,
                                    dcn_numero_pago_base = documentoJson.dcn_numero_pago_base,
                                    dcn_numero_pago = documentoJson.dcn_numero_pago_base
                                };

                                listaDocumentos.Add(documento);
                            }

                            conn.InsertAll(listaDocumentos);

                            return new StatusRestService
                            {
                                status = true,
                                mensaje = "Los Documentos de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                            };
                        }
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Documentos a cargar de la Ruta " + iIdRuta.ToString() + ".",
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
