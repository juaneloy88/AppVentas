using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.ServicioApi
{
    class RequisitosEntregaRestServiceR
    {
        /*Método que consume la Api de Requisitos de Entgrega a Clientes de REPARTO para obtener de la BD del ERP dicha información e insertarla en la BD del dispositivo*/
        public StatusRestService FtnCargarRequisitosEntregaR(int iIdRuta, DateTime dFecha, string sConexionUri)
        {
            try
            {
                //string sUri = ConexionService.FtnRegresarUriPrefijoConexion + "RequisitosEntregaR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
                string sUri = sConexionUri + "RequisitosEntregaR?iIdRuta=" + iIdRuta.ToString() + "&dFecha=" + dFecha.ToString("yyyy-MM-dd");
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
                        var listaRequisitosEntregaJson = JsonConvert.DeserializeObject<List<clientes_requisitos_surtir>>(content);

                        var dbConexion = new conexionDB().CadenaConexion();

                        List<clientes_requisitos_surtir> listaRequisitosEntrega = new List<clientes_requisitos_surtir>();

                        using (SQLiteConnection conn = dbConexion)
                        {
                            foreach (var requisitoEntregaJson in listaRequisitosEntregaJson)
                            {
                                clientes_requisitos_surtir requisitoEntrega = new clientes_requisitos_surtir()
                                {
                                    cln_clave = requisitoEntregaJson.cln_clave,
                                    crc_titular = requisitoEntregaJson.crc_titular,
                                    crc_horario_apertura = requisitoEntregaJson.crc_horario_apertura,
                                    crc_horario_cierre = requisitoEntregaJson.crc_horario_cierre,
                                    crc_horario_sugerido = requisitoEntregaJson.crc_horario_sugerido,
                                    crb_factura = requisitoEntregaJson.crb_factura,
                                    crb_pago_tarjeta = requisitoEntregaJson.crb_pago_tarjeta,
                                    crb_chamuco = requisitoEntregaJson.crb_chamuco,
                                    crb_escaleras = requisitoEntregaJson.crb_escaleras,
                                    crb_rampa = requisitoEntregaJson.crb_rampa,
                                    crb_espacio_estrecho = requisitoEntregaJson.crb_espacio_estrecho,
                                    crb_asaltos = requisitoEntregaJson.crb_asaltos,
                                    crc_avisos = requisitoEntregaJson.crc_avisos
                                };

                                listaRequisitosEntrega.Add(requisitoEntrega);
                            }

                            conn.InsertAll(listaRequisitosEntrega);

                            return new StatusRestService
                            {
                                status = true,
                                mensaje = "Los Requisitos de Entrega de la Ruta " + iIdRuta.ToString() + " fueron cargados correctamente."
                            };
                        }
                    }
                    else
                    {
                        return new StatusRestService
                        {
                            status = true,
                            mensaje = "No hay Requisitos de Entrega a cargar de la Ruta " + iIdRuta.ToString() + ".",
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


