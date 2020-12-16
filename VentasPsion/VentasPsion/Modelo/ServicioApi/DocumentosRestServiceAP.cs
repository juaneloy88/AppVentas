using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class DocumentosRestServiceAP
    {
        /*Método que consume la Api de Documentos de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarDocumentosAP(int iIdRuta, char cDiaVisita, string sConexionUri, string sTipoRecepcion, bool bTeleventa)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "DocumentosAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
                string sUri = sConexionUri + "DocumentosAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
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

                        /*Se comentaron porque la información de dichas tablas se borra al inicio de la Recepción Total*/
                        //dbConexion.DeleteAll<documentos_cabecera>();

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

                                if (sTipoRecepcion == "PARCIAL")
                                {
                                    var existeDocumento = dbConexion.Table<documentos_cabecera>().Where(x => (
                                        (x.vcn_cliente == documento.vcn_cliente) && 
                                        (x.vcn_folio == documento.vcn_folio) && 
                                        (x.vcf_movimiento == documento.vcf_movimiento))
                                    ).FirstOrDefault();

                                    if (existeDocumento == null)
                                        listaDocumentos.Add(documento);
                                }
                                else
                                {
                                    listaDocumentos.Add(documento);
                                }
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
