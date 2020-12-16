using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class TelefonosClientesRestServiceAP
    {
        /*Método que consume la Api de Telefonos a Clientes de AUTOVENTA y PREVENTA para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarTelefonosClientesAP(int iIdRuta, char cDiaVisita, string sConexionUri, string sTipoRecepcion, bool bTeleventa)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "EncuentasClientesAP";
                string sUri = sConexionUri + "TelefonosClientesAP?iIdRuta=" + iIdRuta.ToString() + "&cDiaVisita=" + cDiaVisita + "&bTeleventa=" + bTeleventa;
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
                        var TelefonosClientesJson = JsonConvert.DeserializeObject<List<telefonos_clientes>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        if (sTipoRecepcion=="TOTAL")
                            dbConexion.DeleteAll<telefonos_clientes>();

                        List<telefonos_clientes> TelefonosClientes = new List<telefonos_clientes>();

                        foreach (var TelefonosClienteJson in TelefonosClientesJson)
                        {
                            telefonos_clientes TelefonoCliente = new telefonos_clientes()
                            {
                                cln_clave = TelefonosClienteJson.cln_clave,
                                tcc_telefono = TelefonosClienteJson.tcc_telefono,
                                tcc_nombre = TelefonosClienteJson.tcc_nombre,
                                tcb_estatus = TelefonosClienteJson.tcb_estatus,
                                tcb_movil = TelefonosClienteJson.tcb_movil,
                                tct_horarioini = TelefonosClienteJson.tct_horarioini,
                                tct_horariofin = TelefonosClienteJson.tct_horariofin,
                                tcc_comentario = TelefonosClienteJson.tcc_comentario,
                                tcn_rutacaptura = TelefonosClienteJson.tcn_rutacaptura,
                                tct_fechacaptura = TelefonosClienteJson.tct_fechacaptura,
                                tcb_revisado = true
                            };
                            if (sTipoRecepcion == "PARCIAL")
                            {
                                var existeTelefono = dbConexion.Table<telefonos_clientes>().Where(x => (
                                    (x.cln_clave == TelefonoCliente.cln_clave) &&
                                    (x.tcc_telefono == TelefonoCliente.tcc_telefono))
                                ).FirstOrDefault();

                                if (existeTelefono == null)
                                    TelefonosClientes.Add(TelefonoCliente);
                            }
                            else
                            {
                                TelefonosClientes.Add(TelefonoCliente);
                            }

                            //TelefonosClientes.Add(TelefonoCliente);
                        }

                        dbConexion.InsertAll(TelefonosClientes);

                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "Los Telefonos de los Clientes fueron cargadas correctamente."
                        };
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Telefonos de Clientes a cargar.",
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
