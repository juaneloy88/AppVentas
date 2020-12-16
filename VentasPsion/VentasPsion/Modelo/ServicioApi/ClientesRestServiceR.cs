using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ClientesRestServiceR
    {
        /*Método que consume la Api de Clientes de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarClientes(int iIdRuta, DateTime dFecha, string sConexionUri, string sTipoRecepcion)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "ClientesR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "ClientesR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var clientesJson = JsonConvert.DeserializeObject<List<clientes>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        /*Se comentaron porque la información de dichas tablas se borra al inicio de la Recepción Total*/
                        //dbConexion.DeleteAll<clientes>();
                        //dbConexion.DeleteAll<clientes_estatus>();
                        //dbConexion.DeleteAll<pagare_clientes>();                       

                        List<clientes> clientes = new List<clientes>();
                        List<clientes_estatus> statusClientes = new List<clientes_estatus>();
                        List<pagare_clientes> pagaresClientes = new List<pagare_clientes>();

                        using (SQLiteConnection conn = dbConexion)
                        { 
                            foreach (var clienteJson in clientesJson)
                            {
                                clientes cliente = new clientes()
                                {
                                    cln_clave = clienteJson.cln_clave,
                                    clc_clasificacion = clienteJson.clc_clasificacion,
                                    clc_nombre_comercial = clienteJson.clc_nombre_comercial,
                                    clc_nombre = clienteJson.clc_nombre,
                                    clc_domicilio = clienteJson.clc_domicilio,
                                    clc_rfc = clienteJson.clc_rfc,
                                    clc_devolucion = clienteJson.clc_devolucion,
                                    clc_credito = clienteJson.clc_credito,
                                    cln_codigo = clienteJson.cln_codigo,
                                    clc_rfid = clienteJson.clc_rfid,
                                    clc_estatus = clienteJson.clc_estatus,
                                    clc_impacto = clienteJson.clc_impacto,
                                    clc_facturacion_linea = clienteJson.clc_facturacion_linea,
                                    cln_limite_venta = clienteJson.cln_limite_venta,
                                    cln_saldo = clienteJson.cln_saldo,
                                    run_clave = clienteJson.run_clave,
                                    domiciliocliente = clienteJson.domiciliocliente,
                                    cln_cheque = clienteJson.cln_cheque,
                                    clc_dias_credito = clienteJson.clc_dias_credito,
                                    clc_pago_diferencia_preventa = clienteJson.clc_pago_diferencia_preventa,
                                    clc_fecha_movimiento = clienteJson.clc_fecha_movimiento,
                                    clc_cliente_foco = clienteJson.clc_cliente_foco,
                                    clc_cliente_primer_impacto = clienteJson.clc_cliente_primer_impacto,
                                    lmc_tipo = clienteJson.lmc_tipo,
                                    cln_latitud = clienteJson.cln_latitud,
                                    cln_longitud = clienteJson.cln_longitud,
                                    cld_impuesto_xml = clienteJson.cld_impuesto_xml,
                                    clc_cobrador = clienteJson.clc_cobrador,
                                    clc_dias_credito_cero = clienteJson.clc_dias_credito_cero,
                                    clb_ticket_cobranza = clienteJson.clb_ticket_cobranza
                                };
                                
                                clientes_estatus statusCliente = new clientes_estatus()
                                {
                                    cln_clave = clienteJson.cln_clave,
                                    clc_cliente_foco = clienteJson.clc_cliente_foco,
                                    clc_cliente_primer_impacto = clienteJson.clc_cliente_primer_impacto
                                };

                                pagare_clientes pagareCliente = new pagare_clientes()
                                {
                                    cln_clave = clienteJson.cln_clave,
                                    pcb_entregado = clienteJson.pcb_entregado
                                };

                                if (sTipoRecepcion == "PARCIAL")
                                {
                                    string sSql = "UPDATE clientes SET " +
                                                  "clc_credito = '" + cliente.clc_credito.Trim() + "', " +
                                                  "cln_limite_venta = " + cliente.cln_limite_venta + ", " +
                                                  "clc_cobrador = '" + cliente.clc_cobrador + "', " +
                                                  "clc_dias_credito = '" + cliente.clc_dias_credito + "', " +
                                                  "clc_dias_credito_cero = '" + cliente.clc_dias_credito_cero + "' " +
                                                  "WHERE (cln_clave = " + cliente.cln_clave + ");";

                                    SQLiteCommand command = conn.CreateCommand(sSql);

                                    int iResultado = command.ExecuteNonQuery();

                                    if (iResultado == 0)
                                    {
                                        return new StatusRestService
                                        {
                                            status = false,
                                            mensaje = "No se pudieron actualizar los datos del Cliente " + cliente.cln_clave.ToString() + " de la Ruta " + iIdRuta.ToString() + "."
                                        };
                                    }
                                }
                                else
                                {
                                    clientes.Add(cliente);
                                    statusClientes.Add(statusCliente);
                                    pagaresClientes.Add(pagareCliente);
                                }
                            }

                            if (sTipoRecepcion == "PARCIAL")
                            {
                                // No hace nada porque ya actualizó los datos arriba.

                                return new StatusRestService
                                {
                                    status = true,
                                    mensaje = "Los datos de los Clientes de la Ruta " + iIdRuta.ToString() + " fueron actualizados correctamente."
                                };
                            }
                            else
                            {
                                conn.InsertAll(clientes);
                                conn.InsertAll(statusClientes);
                                conn.InsertAll(pagaresClientes);

                                return new StatusRestService
                                {
                                    status = true,
                                    mensaje = "Los Clientes de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                                };
                            }
                        }
                    }
                    else
                    {
                        if (sTipoRecepcion == "PARCIAL")
                        {
                            return new StatusRestService
                            {
                                status = false,
                                mensaje = "No hay Clientes a actualizar de la Ruta " + iIdRuta.ToString() + "."
                            };
                        }
                        else
                        {
                            return new StatusRestService
                            {
                                status = false,
                                mensaje = "No hay Clientes a cargar de la Ruta " + iIdRuta.ToString() + "."
                            };
                        }
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
