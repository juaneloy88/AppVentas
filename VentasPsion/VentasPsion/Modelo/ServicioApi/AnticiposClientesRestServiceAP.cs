using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class AnticiposClientesRestServiceAP
    {
        /*Método que consume la Api de Anticipos a Clientes de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarAnticiposClientesAP(int iIdRuta, char cDiaVisita, string sConexionUri, string sTipoRecepcion, bool bTeleventa)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "EncuentasClientesAP";
                string sUri = sConexionUri + "AnticiposAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
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
                        var AnticiposClientesJson = JsonConvert.DeserializeObject<List<anticipos>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        dbConexion.DeleteAll<anticipos>();

                        List<anticipos> AnticiposClientes = new List<anticipos>();

                        foreach (var AnticiposClienteJson in AnticiposClientesJson)
                        {
                            anticipos AnticiposCliente = new anticipos()
                            {
                                cln_clave = AnticiposClienteJson.cln_clave,
                                vcn_folio = AnticiposClienteJson.vcn_folio,
                               vcf_movimiento  = AnticiposClienteJson.vcf_movimiento,
                               ann_monto_pago = AnticiposClienteJson.ann_monto_pago,
                               ann_cantidad_pagos = AnticiposClienteJson.ann_cantidad_pagos,
                               anc_forma_pago = AnticiposClienteJson.anc_forma_pago
                            };

                            AnticiposClientes.Add(AnticiposCliente);
                        }

                        dbConexion.InsertAll(AnticiposClientes);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Las Anticipos a Clientes fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Anticipos a Clientes a cargar.",
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
