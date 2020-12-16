using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.ServicioApi
{
    public class TransmitirRestService
    {
        DocumentosVM Doctos = new DocumentosVM();

        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        bool bTipoBD = VarEntorno.bTipoBaseDatos;
        string sCorreo = "";

        #region Método para obtener la URI de Envío, dependiendo del tipo de conexión
        public string uriConexion()
        {
            string sUriConexion = string.Empty;

            if (CrossConnectivity.Current.IsConnected == true)
            {
                IEnumerable<ConnectionType> connectionTypes;
                connectionTypes = CrossConnectivity.Current.ConnectionTypes;

                if (connectionTypes.Contains(ConnectionType.WiFi))
                {
                    //sUriConexion = "http://192.168.2.14/PublishWebApi/api/";
                    //sUriConexion = "http://192.168.2.23/PublishWebApi/api/";
                    sUriConexion = VarEntorno.sUriConexionEnvio;

                    Ping ping = new Ping();

                    try
                    {
                        if (ping.Send("192.168.2.224", 5).Status == IPStatus.Success)
                            sUriConexion = VarEntorno.sUriConexionEnvio;
                        else                        
                            sUriConexion = "http://200.56.117.142/PublishWebApi/api/";
                    }
                    catch (Exception)
                    {
                        sUriConexion = "No hay conexión";
                    }
                    
                }
                else
                {
                    if (connectionTypes.Contains(ConnectionType.Cellular))
                    {                        
                        sUriConexion = "http://200.56.117.142/PublishWebApi/api/";
                    }
                    else
                    {
                        sUriConexion = "No hay conexión";
                    }
                }
            }
            else
            {
                sUriConexion = "No hay conexión";
            }

            return sUriConexion;
        }
        #endregion Método para obtener la URI de Envío, dependiendo del tipo de conexión

        #region Metodos para Enviar la información a través de la WebApi

        #region Método para válidar si la ruta ya se encuentra cerrada
        public async Task<string> validaCierreRuta(int iRuta)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();            

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "entorno?iRuta=" + iRuta.ToString() + "&bTipoBD=" + bTipoBD.ToString();

                try
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetStringAsync(sUri);
                        sRespuesta = response.ToString();
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }            

            return sRespuesta;
        }
        #endregion Método para válidar si la ruta ya se encuentra cerrada

        #region Método para válidar si los anticipos se encuentran relacionados
        public string ValidaRelacionAnticipos()
        {
            string sRespuesta = string.Empty;

            sRespuesta = Doctos.ValidaAnticiposRelacionados();

            return sRespuesta;
        }
        #endregion Método para válidar si los anticipos se encuentran relacionados

        #region Método para enviar la tabla de venta_cabecera
        public async Task<string> enviarVentaCabecera()
        {
            string sRespuesta = string.Empty;
            string sUri = string.Empty;
            string sQuery = string.Empty;            
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {

                sUri = sUriEnvio + "venta_cabecera";

                try
                {
                    List<InfoVtaCabecera> lCabecera = new List<InfoVtaCabecera>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select vcn_folio, vcn_cliente, vcn_estatus, vcn_importe, vcn_saldo_ant, " +
                                 "vcn_saldo_nuevo, vcn_monto_pago, vcc_hora_ini, vcc_hora_fin, vcc_tipo_pago, " +
                                 "vcc_banco, vcf_num_cheque, vcf_fec_cheque, vcn_monto_cheque, vcf_fec_venta, " +
                                 "vcn_descuento, vcn_producto_insp, vcn_monto_efe, vcc_banco2, vcf_num_cheque2, " +
                                 "vcf_fec_cheque2, vcn_monto_cheque2, vcf_fec_venta2, vcn_descuento2, vcn_producto_insp2, " +
                                 "vcc_banco3, vcf_num_cheque3 vcf_fec_cheque3, vcn_monto_cheque3, vcf_fec_venta3, " +
                                 "vcn_descuento3, vcn_producto_insp3, vcn_saldo_envase, vcn_ruta, vcf_movimiento, " +
                                 "vcc_clave, vcc_contado, cmpc_clave_sat, vcc_ctapgosat, cmpc_metodopago, cfpc_formapago, fcd_uuid, " +
                                 "'" + VarEntorno.bEsTeleventa.ToString() + "' as sEsTeleventa, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from venta_cabecera";
                        lCabecera = conn.Query<InfoVtaCabecera>(sQuery);
                    }

                    if (lCabecera.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedCabecera = JsonConvert.SerializeObject(lCabecera);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedCabecera, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar venta_cabecera";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de venta_cabecera

        #region Método para enviar la tabla venta_pagos
        public async Task<string> enviarVentaPagos()
        {
            string sRespuesta = string.Empty;
            string sUri = string.Empty;
            string sRuta = VarEntorno.iNoRuta.ToString();
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "venta_pagos";

                try
                {
                    List<InfoVentaPagos> lVentaPagos = new List<InfoVentaPagos>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select vpn_numpago, vcn_folio, vcn_cliente, vcf_movimiento, "+ sRuta + " as vcn_ruta, " +
                                        "vpc_descripcion, cfpc_formapago, vcc_banco, vpc_tipotarjeta, vcn_monto, " +
                                        "vcc_referencia, vpc_nocuenta, vpc_autorizacion, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " + 
                                        "from venta_pagos";
                        lVentaPagos = conn.Query<InfoVentaPagos>(sQuery);
                    }

                    if (lVentaPagos.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedCabecera = JsonConvert.SerializeObject(lVentaPagos);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedCabecera, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar venta_pagos";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla venta_pagos

        #region Método para enviar la tabla de venta_detalle
        public async Task<string> enviarVentaDetalle()
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();            

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "venta_detalle";

                try
                {
                    List<InfoVtaDetalle> lVentaDetalle = new List<InfoVtaDetalle>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        if (VarEntorno.cTipoVenta == 'A' || VarEntorno.cTipoVenta == 'P')
                        {
                            sQuery = "select prn_secuencia, vdn_folio, vdn_cliente, run_clave, vdn_producto, " +
                                     "vdn_venta, vdn_importe, vdd_descripcion, vdn_precio, vdc_tipo_precio, " +
                                     "vdc_tipo_entrada, vdc_hora, vdn_venta_dev, vdn_folio_devolucion, vdn_tipo_promo, " +
                                     "lmn_precioneto, lmn_ieps, lmn_preciobruto, lmn_iva, lmn_preciofinal, lmc_porcentaje, " +
                                     "lmn_descuento_neto, lmn_iepsdescuento, lmn_descuento_bruto, lmn_ivadescuento, " +
                                     "lmn_totaldescuento, lmn_preciolm_neto, lmn_preciolm_ipes, lmn_preciolm_bruto, " +
                                     "lmn_preciolm_iva, lmn_preciolm, " +
                                     "'" + VarEntorno.bEsTeleventa.ToString() + "' as sEsTeleventa, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from venta_detalle;";
                        }
                        else
                        {
                            sQuery = "select prn_secuencia, vdn_folio, vdn_cliente, run_clave, vdn_producto, " +
                                     "vdn_venta, vdn_importe, vdd_descripcion, vdn_precio, vdc_tipo_precio, " +
                                     "vdc_tipo_entrada, vdc_hora, vdn_venta_dev, vdn_folio_devolucion, vdn_tipo_promo, " +
                                     "lmn_precioneto, lmn_ieps, lmn_preciobruto, lmn_iva, lmn_preciofinal, lmc_porcentaje, " +
                                     "lmn_descuento_neto, lmn_iepsdescuento, lmn_descuento_bruto, lmn_ivadescuento, " +
                                     "lmn_totaldescuento, lmn_preciolm_neto, lmn_preciolm_ipes, lmn_preciolm_bruto, " +
                                     "lmn_preciolm_iva, lmn_preciolm, " +
                                     "'" + VarEntorno.bEsTeleventa.ToString() + "' as sEsTeleventa, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from venta_detalle where vdn_venta_dev > 0;";
                        }

                        lVentaDetalle = conn.Query<InfoVtaDetalle>(sQuery);
                    }

                    if (lVentaDetalle.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedVentaDetalle = JsonConvert.SerializeObject(lVentaDetalle);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedVentaDetalle, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar venta_detalle";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de venta_detalle

        #region Método para enviar la tabla de envase
        public async Task<string> enviarEnvase()
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "envase";

                try
                {
                    List<infoEnvase> lEnvase = new List<infoEnvase>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        if (VarEntorno.cTipoVenta == 'A' || VarEntorno.cTipoVenta == 'P')
                        {
                            sQuery = "select " + sRuta + " as run_clave, " +
                                     "cln_clave, men_folio, mec_envase, men_saldo_inicial, " +
                                     "men_cargo, men_abono, men_venta, men_saldo_final, " +
                                     "mec_es_devolucion, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from envase " +
                                     "where (men_cargo <> 0 " +
                                     "or men_abono <> 0 " +
                                     "or men_venta <> 0) " +
                                     "order by mec_envase";
                        }
                        else
                        {
                            sQuery = "select " + sRuta + " as run_clave, " +
                                     "cln_clave, men_folio, mec_envase, men_saldo_inicial, " +
                                     "men_cargo, men_abono, men_venta, men_saldo_final, " +
                                     "mec_es_devolucion, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from envase " +
                                     "where (men_cargo <> 0 " +
                                     "or men_abono <> 0) " +
                                     "order by mec_envase";
                        }

                        lEnvase = conn.Query<infoEnvase>(sQuery);
                    }

                    if (lEnvase.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {                            
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedEnvase = JsonConvert.SerializeObject(lEnvase);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedEnvase, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar envase";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de envase        

        #region Método para enviar la tabla de bonificaciones
        public async Task<string> enviarBonificaciones()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "bonificaciones";

                try
                {
                    List<infoBonificaciones> lBonificaciones = new List<infoBonificaciones>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select "+ sRuta + " as run_clave, " +
                                        "boc_cliente, boc_folio, boi_documento, boc_tipo, boc_folio_venta, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from bonificaciones where boc_folio_venta is not null";

                        lBonificaciones = conn.Query<infoBonificaciones>(sQuery);
                    }

                    if (lBonificaciones.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedBonis = JsonConvert.SerializeObject(lBonificaciones);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedBonis, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar bonificaciones";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }            

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de bonificaciones

        #region Método para enviar la tabla de clientes_estatus
        public async Task<string> enviarClientesEstatus()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "clientes_estatus";
                string sRuta = VarEntorno.iNoRuta.ToString();

                try
                {
                    List<infoClientesStatus> lClientesEstatus = new List<infoClientesStatus>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, " +
                                        "cln_clave, cln_visitado, clb_corregido, " +
                                        "clc_programa_pago, clc_cliente_foco, " +
                                        "clc_cliente_primer_impacto, " +
                                        "cln_tipo_no_venta, cln_latitud, cln_longitud, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from clientes_estatus";

                        lClientesEstatus = conn.Query<infoClientesStatus>(sQuery);
                    }

                    if (lClientesEstatus.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedClientesEstatus = JsonConvert.SerializeObject(lClientesEstatus);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedClientesEstatus, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar clientes_estatus";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de clientes_estatus        

        #region Método para enviar la tabla de devoluciones
        public async Task<string> enviarDevoluciones()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "devoluciones";

                try
                {
                    List<infoDevoluciones> lDevoluciones = new List<infoDevoluciones>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {                        
                        string sQuery = "select "+ sRuta + " as run_clave, cln_clave, dev_clave, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from devoluciones";

                        lDevoluciones = conn.Query<infoDevoluciones>(sQuery);
                    }

                    if (lDevoluciones.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedDevoluciones = JsonConvert.SerializeObject(lDevoluciones);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedDevoluciones, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar devoluciones";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }   

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de devoluciones

        #region Método para enviar la tabla de respuestas
        public async Task<string> enviarRespuestas()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            int iRuta = VarEntorno.iNoRuta;

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "respuestas";

                try
                {
                    List<InfoRespuestas> lRespuestas = new List<InfoRespuestas>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select cln_clave, "+ iRuta.ToString() + " as run_clave, enn_id, enc_respuesta, enn_tipo_respuesta, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from respuestas";
                        lRespuestas = conn.Query<InfoRespuestas>(sQuery);
                    }

                    if (lRespuestas.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedRespuestas = JsonConvert.SerializeObject(lRespuestas);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedRespuestas, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar respuestas";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }

                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de respuestas

        #region Método para enviar la tabla de solicitudes
        public async Task<string> enviarSolicitudes()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "solicitudes";

                try
                {
                    List<InfoSolicitudes> lSolicitudes = new List<InfoSolicitudes>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select dpn_clave, soc_descripcion, cln_clave, run_clave, sod_fecha, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from solicitudes";
                        lSolicitudes = conn.Query<InfoSolicitudes>(sQuery);
                    }

                    if (lSolicitudes.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedSolicitudes = JsonConvert.SerializeObject(lSolicitudes);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedSolicitudes, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar solicitudes";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }               

            return sRespuesta;
        }
        #endregion Método para enviar horario, kilometraje y version de ruta

        #region Método para enviar horario, kilometraje y version de ruta
        public async Task<string> enviarInfoRuta()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sVersion = VarEntorno.sVersionApp;
                sVersion = sVersion.Replace("Versión ", "");
                string sUri = sUriEnvio + "infoRuta";

                try
                {

                    List<infoRuta> lInfoRuta = new List<infoRuta>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select run_clave, ruc_inicio, ruc_termino, run_ncamion, " +
                                        "run_kminicio, run_kmfinal, '" + sVersion + "' as ruc_version, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from ruta";
                        lInfoRuta = conn.Query<infoRuta>(sQuery);
                    }

                    if (lInfoRuta.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedInfoRuta = JsonConvert.SerializeObject(lInfoRuta);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedInfoRuta, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar información de la ruta";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }               

            return sRespuesta;
        }
        #endregion Método para enviar horario, kilometraje y version de ruta

        #region Método para enviar ClientesCompetencia
        public async Task<string> enviarClientesCompetencia()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "clientes_competencia_censo";

                try
                {
                    List<InfoClientesCompetencia> lClientesCompetencia = new List<InfoClientesCompetencia>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select ccc_nombre, ccc_negocio, ccn_tipo, ctp_clave, ccn_latitud, ccn_longitud, " +
                                        sRuta.ToString() + " as run_clave, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from clientes_competencia";
                        lClientesCompetencia = conn.Query<InfoClientesCompetencia>(sQuery);
                    }

                    if (lClientesCompetencia.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedlClientesCompetencia = JsonConvert.SerializeObject(lClientesCompetencia);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlClientesCompetencia, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Clientes Competencia";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar ClientesCompetencia

        #region Método para enviar GPS
        public async Task<string> enviarGps()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "gps";

                try
                {
                    List<InfoGps> lGps = new List<InfoGps>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select run_clave, cln_clave, gpd_latitud, gpd_longitud, " +
                                        "ctn_tipo_movimiento, gpd_hora, gpc_folio, gpb_esBase, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from gps;";
                        lGps = conn.Query<InfoGps>(sQuery);
                    }

                    if (lGps.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedlGps = JsonConvert.SerializeObject(lGps);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlGps, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Gps";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }                

            return sRespuesta;
        }
        #endregion Método para enviar GPS

        #region Procedimiento para enviar pagare_clientes
        public async Task<string> enviarPagareClientes()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            int iRuta = VarEntorno.iNoRuta;

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "pagare_clientes";

                try
                {
                    List<infoPagareClientes> lPagareClientes = new List<infoPagareClientes>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + iRuta.ToString() + " as run_clave, cln_clave, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from pagare_clientes where pcb_entregado";
                        lPagareClientes = conn.Query<infoPagareClientes>(sQuery);
                    }

                    if (lPagareClientes.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedlPagareClientes = JsonConvert.SerializeObject(lPagareClientes);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlPagareClientes, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Pagare Clientes";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Procedimiento para enviar pagare_clientes

        #region Método para enviar pagos_programados
        public async Task<string> enviarPagosProgramados()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "pagosProgramados";

                try
                {
                    List<infoClientesStatus> lClientesStatus = new List<infoClientesStatus>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, " +
                                        "cln_clave, cln_visitado, clb_corregido, " +
                                        "clc_programa_pago, clc_cliente_foco, clc_cliente_primer_impacto, " +
                                        "cln_tipo_no_venta, cln_latitud, cln_longitud, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from clientes_estatus " +
                                        "order by cln_clave";
                        lClientesStatus = conn.Query<infoClientesStatus>(sQuery);
                    }

                    if (lClientesStatus.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedlPagosProgramados = JsonConvert.SerializeObject(lClientesStatus);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlPagosProgramados, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Pagos Programados";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }                

            return sRespuesta;
        }
        #endregion Método para enviar pagos_programados

        #region Procedimiento para enviar empleados
        public async Task<string> enviarEmpleados()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();            

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "infoEmpleados";

                try
                {
                    infoEmpleados infoempl = new infoEmpleados();
                    infoempl.sRuta = VarEntorno.iNoRuta.ToString();
                    infoempl.sEmpleado = VarEntorno.iNUsuario.ToString();
                    infoempl.bTipoBD = bTipoBD;
                    infoempl.sCorreo = sCorreo;

                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.Timeout = TimeSpan.FromMinutes(15);
                        string serializedlPagosProgramados = JsonConvert.SerializeObject(infoempl);
                        var response = await httpClient.PostAsync(sUri,
                                                                    new StringContent(serializedlPagosProgramados, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            sRespuesta = "Ok";
                        }
                        else
                        {
                            sRespuesta = "Error al Enviar Empleados";
                        }
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Procedimiento para enviar empleados        

        #region Método para enviar envase_sugerido
        public async Task<string> enviarInfoEnvaseSugerido()
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sRuta = VarEntorno.iNoRuta.ToString();
                string sUri = sUriEnvio + "envase_sugerido";

                try
                {

                    List<infoEnvaseSugerido> lInfoEnvaseSugerido = new List<infoEnvaseSugerido>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, cln_clave, esc_envase, " +
                                        "esn_cantidad_vacio, esn_cantidad_lleno, esc_comentario, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from envase_sugerido " +
                                        "order by cln_clave";
                        lInfoEnvaseSugerido = conn.Query<infoEnvaseSugerido>(sQuery);
                    }

                    if (lInfoEnvaseSugerido.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedInfoEnvaseSugerido = JsonConvert.SerializeObject(lInfoEnvaseSugerido);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedInfoEnvaseSugerido, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar información de la ruta";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar envase_sugerido

        #region Método para enviar la tabla de documentos_cabecera
        public async Task<string> enviarDocumentosCabecera()
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "documentos_cabecera";

                try
                {
                    List<InfoDocumentosCabecera> lDocumentosCabecera = new List<InfoDocumentosCabecera>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        if (VarEntorno.cTipoVenta == 'A' || VarEntorno.cTipoVenta == 'P')
                        {
                            sQuery = "select vcn_cliente, vcn_folio, vcf_movimiento, vcn_importe, dcc_tipo, vcc_cadena_original, " +
                                     sRuta + " as run_clave, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from documentos_cabecera " +
                                     "where dcb_nuevo_documento;";
                        }
                        else
                        {
                            sQuery = "select d.vcn_cliente, d.vcn_folio, d.vcf_movimiento, d.vcn_importe, d.dcc_tipo, d.vcc_cadena_original, " +
                                     sRuta + " as run_clave, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from documentos_cabecera d " +
                                     "join FacturasVenta f on f.vcn_cliente = d.vcn_cliente " +
                                     "and f.vcn_folio = d.vcn_folio and f.vcf_movimiento = d.vcf_movimiento";
                        }

                        lDocumentosCabecera = conn.Query<InfoDocumentosCabecera>(sQuery);
                    }

                    if (lDocumentosCabecera.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedDocumentosCabecera = JsonConvert.SerializeObject(lDocumentosCabecera);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedDocumentosCabecera, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar documentos_cabecera";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de documentos_cabecera

        #region Método para enviar la tabla de documentos_detalle
        public async Task<string> enviarDocumentosDetalle()
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "documentos_detalle";

                try
                {
                    List<InfoDocumentosDetalle> lDocumentosDetalle = new List<InfoDocumentosDetalle>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select DISTINCT dd.*, " +
                                 sRuta + " as run_clave, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from documentos_detalle dd " +
                                 "where vcn_cliente <> 0;";

                        lDocumentosDetalle = conn.Query<InfoDocumentosDetalle>(sQuery);
                    }

                    if (lDocumentosDetalle.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedDocumentosDetalle = JsonConvert.SerializeObject(lDocumentosDetalle);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedDocumentosDetalle, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar documentos_detalle";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de documentos_detalle

        #region Método para enviar la tabla de clientes_datos_surtir
        public async Task<string> enviarClientesDatosSurtir()
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "clientes_datos_surtir";

                try
                {
                    List<InfoClientesDatosSurtir> lRequisitos = new List<InfoClientesDatosSurtir>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select c.*, " +
                                 sRuta + " as run_clave, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from clientes_requisitos_surtir c " +
                                 "where crb_actualizado";

                        lRequisitos = conn.Query<InfoClientesDatosSurtir>(sQuery);
                    }

                    if (lRequisitos.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedRequisitos = JsonConvert.SerializeObject(lRequisitos);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedRequisitos, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Requisitos";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de clientes_datos_surtir

        #region Método para enviar la tabla de anticipos
        public async Task<string> EnviarAnticipos()
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;                
            }
            else
            {
                sUri = sUriEnvio + "anticipos";

                try
                {
                    List<InfoAnticipos> lAnticipos = new List<InfoAnticipos>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select a.*, " +
                                 sRuta + " as run_clave, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from anticipos a " +
                                 "order by a.cln_clave";                                 

                        lAnticipos = conn.Query<InfoAnticipos>(sQuery);
                    }

                    if (lAnticipos.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedAnticipos = JsonConvert.SerializeObject(lAnticipos);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedAnticipos, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Anticipos";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de anticipos

        #region Método para enviar la tabla de telefonos_clientes
        public async Task<string> EnviarTelefonosClientes()
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();            

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "telefonos_clientes";

                try
                {
                    List<InfoTelefonosClientes> lTelefonosClientes = new List<InfoTelefonosClientes>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select t.*, " +                                 
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from telefonos_clientes t " +
                                 "order by t.cln_clave";

                        lTelefonosClientes = conn.Query<InfoTelefonosClientes>(sQuery);
                    }

                    if (lTelefonosClientes.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedTelefonosClientes = JsonConvert.SerializeObject(lTelefonosClientes);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedTelefonosClientes, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Telefonos Clientes";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de telefonos_clientes

        #endregion Metodos para Enviar la información a través de la WebApi


        #region Métodos para Enviar los Archivos Json de cada Entidad

        #region Método para enviar el Json de venta_cabecera
        public async Task<string> EnviarVentaCabeceraJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "venta_cabeceraJson";

                try
                {
                    List<InfoVtaCabecera> lCabecera = new List<InfoVtaCabecera>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select vcn_folio, vcn_cliente, vcn_estatus, vcn_importe, vcn_saldo_ant, " +
                                        "vcn_saldo_nuevo, vcn_monto_pago, vcc_hora_ini, vcc_hora_fin, vcc_tipo_pago, " +
                                        "vcc_banco, vcf_num_cheque, vcf_fec_cheque, vcn_monto_cheque, vcf_fec_venta, " +
                                        "vcn_descuento, vcn_producto_insp, vcn_monto_efe, vcc_banco2, vcf_num_cheque2, " +
                                        "vcf_fec_cheque2, vcn_monto_cheque2, vcf_fec_venta2, vcn_descuento2, vcn_producto_insp2, " +
                                        "vcc_banco3, vcf_num_cheque3 vcf_fec_cheque3, vcn_monto_cheque3, vcf_fec_venta3, " +
                                        "vcn_descuento3, vcn_producto_insp3, vcn_saldo_envase, vcn_ruta, vcf_movimiento, " +
                                        "vcc_clave, vcc_contado, cmpc_clave_sat, vcc_ctapgosat, cmpc_metodopago, cfpc_formapago, fcd_uuid, " +
                                        "'" + VarEntorno.bEsTeleventa.ToString() + "' as sEsTeleventa, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from venta_cabecera";
                        lCabecera = conn.Query<InfoVtaCabecera>(sQuery);
                    }

                    if (lCabecera.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedCabecera = JsonConvert.SerializeObject(lCabecera);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedCabecera, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "venta_cabecera enviada correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar venta_cabecera";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de venta_cabecera

        #region Método para enviar la tabla venta_pagos
        public async Task<string> EnviarVentaPagosJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUri = string.Empty;
            string sRuta = VarEntorno.iNoRuta.ToString();
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "venta_pagosJson";

                try
                {
                    List<InfoVentaPagos> lVentaPagos = new List<InfoVentaPagos>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select vpn_numpago, vcn_folio, vcn_cliente, vcf_movimiento, " + sRuta + " as vcn_ruta, " +
                                        "vpc_descripcion, cfpc_formapago, vcc_banco, vpc_tipotarjeta, vcn_monto, " +
                                        "vcc_referencia, vpc_nocuenta, vpc_autorizacion, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from venta_pagos";
                        lVentaPagos = conn.Query<InfoVentaPagos>(sQuery);
                    }

                    if (lVentaPagos.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedCabecera = JsonConvert.SerializeObject(lVentaPagos);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedCabecera, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "venta_pagos enviada correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar venta_pagos";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla venta_pagos

        #region Método para enviar el Json de venta_detalle
        public async Task<string> EnviarVentaDetalleJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "venta_detalleJson";

                try
                {
                    List<InfoVtaDetalle> lVentaDetalle = new List<InfoVtaDetalle>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        if (VarEntorno.cTipoVenta == 'A' || VarEntorno.cTipoVenta == 'P')
                        {
                            sQuery = "select prn_secuencia, vdn_folio, vdn_cliente, run_clave, vdn_producto, " +
                                             "vdn_venta, vdn_importe, vdd_descripcion, vdn_precio, vdc_tipo_precio, " +
                                             "vdc_tipo_entrada, vdc_hora, vdn_venta_dev, vdn_folio_devolucion, vdn_tipo_promo, " +
                                             "lmn_precioneto, lmn_ieps, lmn_preciobruto, lmn_iva, lmn_preciofinal, lmc_porcentaje, " +
                                             "lmn_descuento_neto, lmn_iepsdescuento, lmn_descuento_bruto, lmn_ivadescuento, " +
                                             "lmn_totaldescuento, lmn_preciolm_neto, lmn_preciolm_ipes, lmn_preciolm_bruto, " +
                                             "lmn_preciolm_iva, lmn_preciolm, " +
                                             "'" + VarEntorno.bEsTeleventa.ToString() + "' as sEsTeleventa, " +
                                             "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                             "'" + sCorreo + "' as sCorreo " +
                                             "from venta_detalle;";
                        }
                        else
                        {
                            sQuery = "select prn_secuencia, vdn_folio, vdn_cliente, run_clave, vdn_producto, " +
                                             "vdn_venta, vdn_importe, vdd_descripcion, vdn_precio, vdc_tipo_precio, " +
                                             "vdc_tipo_entrada, vdc_hora, vdn_venta_dev, vdn_folio_devolucion, vdn_tipo_promo, " +
                                             "lmn_precioneto, lmn_ieps, lmn_preciobruto, lmn_iva, lmn_preciofinal, lmc_porcentaje, " +
                                             "lmn_descuento_neto, lmn_iepsdescuento, lmn_descuento_bruto, lmn_ivadescuento, " +
                                             "lmn_totaldescuento, lmn_preciolm_neto, lmn_preciolm_ipes, lmn_preciolm_bruto, " +
                                             "lmn_preciolm_iva, lmn_preciolm, " +
                                             "'" + VarEntorno.bEsTeleventa.ToString() + "' as bEsTeleventa, " +
                                             "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                             "'" + sCorreo + "' as sCorreo " +
                                             "from venta_detalle where vdn_venta_dev > 0;";
                        }

                        lVentaDetalle = conn.Query<InfoVtaDetalle>(sQuery);
                    }

                    if (lVentaDetalle.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedVentaDetalle = JsonConvert.SerializeObject(lVentaDetalle);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedVentaDetalle, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "venta_detalle enviada correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar venta_detalle";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de venta_detalle

        #region Método para enviar el Json de envase
        public async Task<string> EnviarEnvaseJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "envaseJson";

                try
                {
                    List<infoEnvase> lEnvase = new List<infoEnvase>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        if (VarEntorno.cTipoVenta == 'A' || VarEntorno.cTipoVenta == 'P')
                        {
                            sQuery = "select " + sRuta + " as run_clave, " +
                                             "cln_clave, men_folio, mec_envase, men_saldo_inicial, " +
                                             "men_cargo, men_abono, men_venta, men_saldo_final, " +
                                             "mec_es_devolucion, " +
                                             "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                             "'" + sCorreo + "' as sCorreo " +
                                             "from envase " +
                                             "where (men_cargo <> 0 " +
                                             "or men_abono <> 0 " +
                                             "or men_venta <> 0) " +
                                             "order by mec_envase";
                        }
                        else
                        {
                            sQuery = "select " + sRuta + " as run_clave, " +
                                             "cln_clave, men_folio, mec_envase, men_saldo_inicial, " +
                                             "men_cargo, men_abono, men_venta, men_saldo_final, " +
                                             "mec_es_devolucion, " +
                                             "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                             "'" + sCorreo + "' as sCorreo " +
                                             "from envase " +
                                             "where (men_cargo <> 0 " +
                                             "or men_abono <> 0) " +
                                             "order by mec_envase";
                        }

                        lEnvase = conn.Query<infoEnvase>(sQuery);
                    }

                    if (lEnvase.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedEnvase = JsonConvert.SerializeObject(lEnvase);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedEnvase, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "envase enviado correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar envase";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de envase

        #region Método para enviar el Json de bonificaciones
        public async Task<string> EnviarBonificacionesJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "bonificacionesJson";

                try
                {
                    List<infoBonificaciones> lBonificaciones = new List<infoBonificaciones>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, " +
                                                "boc_cliente, boc_folio, boi_documento, boc_tipo, boc_folio_venta, " +
                                                "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                                "'" + sCorreo + "' as sCorreo " +
                                                "from bonificaciones where boc_folio_venta is not null";

                        lBonificaciones = conn.Query<infoBonificaciones>(sQuery);
                    }

                    if (lBonificaciones.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedBonis = JsonConvert.SerializeObject(lBonificaciones);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedBonis, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "bonificaciones enviadas correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar bonificaciones";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de bonificaciones

        #region Método para enviar el Json de clientes_estatus
        public async Task<string> EnviarClientesEstatusJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "clientes_estatusJson";
                string sRuta = VarEntorno.iNoRuta.ToString();

                try
                {
                    List<infoClientesStatus> lClientesEstatus = new List<infoClientesStatus>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, " +
                                        "cln_clave, cln_visitado, clb_corregido, " +
                                        "clc_programa_pago, clc_cliente_foco, " +
                                        "clc_cliente_primer_impacto, " +
                                        "cln_tipo_no_venta, cln_latitud, cln_longitud, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from clientes_estatus";
                        lClientesEstatus = conn.Query<infoClientesStatus>(sQuery);
                    }

                    if (lClientesEstatus.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedClientesEstatus = JsonConvert.SerializeObject(lClientesEstatus);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedClientesEstatus, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar clientes_estatus";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de clientes_estatus        

        #region Método para enviar el Json de devoluciones
        public async Task<string> EnviarDevolucionesJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "devolucionesJson";

                try
                {
                    List<infoDevoluciones> lDevoluciones = new List<infoDevoluciones>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, cln_clave, dev_clave, " +
                                                "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                                "'" + sCorreo + "' as sCorreo " +
                                                "from devoluciones";

                        lDevoluciones = conn.Query<infoDevoluciones>(sQuery);
                    }

                    if (lDevoluciones.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedDevoluciones = JsonConvert.SerializeObject(lDevoluciones);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedDevoluciones, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar devoluciones";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de devoluciones

        #region Método para enviar el Json de respuestas
        public async Task<string> EnviarRespuestasJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            int iRuta = VarEntorno.iNoRuta;

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "respuestasJson";

                try
                {
                    List<InfoRespuestas> lRespuestas = new List<InfoRespuestas>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select cln_clave, " + iRuta.ToString() + " as run_clave, enn_id, enc_respuesta, enn_tipo_respuesta, " +
                                                "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                                "'" + sCorreo + "' as sCorreo " +
                                                "from respuestas";
                        lRespuestas = conn.Query<InfoRespuestas>(sQuery);
                    }

                    if (lRespuestas.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedRespuestas = JsonConvert.SerializeObject(lRespuestas);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedRespuestas, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar respuestas";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }

                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de respuestas

        #region Método para enviar el Json de solicitudes
        public async Task<string> EnviarSolicitudesJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "solicitudesJson";

                try
                {
                    List<InfoSolicitudes> lSolicitudes = new List<InfoSolicitudes>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select dpn_clave, soc_descripcion, cln_clave, run_clave, sod_fecha, " +
                                                "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                                "'" + sCorreo + "' as sCorreo " +
                                                "from solicitudes";
                        lSolicitudes = conn.Query<InfoSolicitudes>(sQuery);
                    }

                    if (lSolicitudes.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedSolicitudes = JsonConvert.SerializeObject(lSolicitudes);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedSolicitudes, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar solicitudes";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de solicitudes

        #region Método para enviar el Json de horario, kilometraje y version de ruta
        public async Task<string> EnviarInfoRutaJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sVersion = VarEntorno.sVersionApp;
                sVersion = sVersion.Replace("Versión ", "");
                string sUri = sUriEnvio + "infoRutaJson";

                try
                {

                    List<infoRuta> lInfoRuta = new List<infoRuta>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select run_clave, ruc_inicio, ruc_termino, run_ncamion, " +
                                                "run_kminicio, run_kmfinal, '" + sVersion + "' as ruc_version, " +
                                                "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                                "'" + sCorreo + "' as sCorreo " +
                                                "from ruta";
                        lInfoRuta = conn.Query<infoRuta>(sQuery);
                    }

                    if (lInfoRuta.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedInfoRuta = JsonConvert.SerializeObject(lInfoRuta);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedInfoRuta, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar información de la ruta";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de horario, kilometraje y version de ruta

        #region Método para enviar el Json de ClientesCompetencia
        public async Task<string> EnviarClientesCompetenciaJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {               
                string sUri = sUriEnvio + "clientes_competencia_censoJson";

                try
                {
                    List<InfoClientesCompetencia> lClientesCompetencia = new List<InfoClientesCompetencia>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select ccc_nombre, ccc_negocio, ccn_tipo, ctp_clave, ccn_latitud, ccn_longitud, " +
                                        sRuta.ToString() + " as run_clave, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from clientes_competencia";
                        lClientesCompetencia = conn.Query<InfoClientesCompetencia>(sQuery);
                    }

                    if (lClientesCompetencia.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedlClientesCompetencia = JsonConvert.SerializeObject(lClientesCompetencia);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlClientesCompetencia, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar información de la ruta";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de ClientesCompetencia

        #region Método para enviar el Json GPS
        public async Task<string> EnviarGpsJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "gpsJson";

                try
                {
                    List<InfoGps> lGps = new List<InfoGps>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select run_clave, cln_clave, gpd_latitud, gpd_longitud, " +
                                        "ctn_tipo_movimiento, gpd_hora, gpc_folio, gpb_esBase, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from gps;";
                        lGps = conn.Query<InfoGps>(sQuery);
                    }

                    if (lGps.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedlGps = JsonConvert.SerializeObject(lGps);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlGps, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Gps";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de GPS

        #region Método para enviar el Json de pagos_programados
        public async Task<string> EnviarPagosProgramadosJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "pagosProgramadosJson";

                try
                {
                    List<infoClientesStatus> lClientesStatus = new List<infoClientesStatus>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, " +
                                        "cln_clave, cln_visitado, clb_corregido, " +
                                        "clc_programa_pago, clc_cliente_foco, clc_cliente_primer_impacto, " +
                                        "cln_tipo_no_venta, cln_latitud, cln_longitud, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from clientes_estatus " +
                                        "order by cln_clave";
                        lClientesStatus = conn.Query<infoClientesStatus>(sQuery);
                    }

                    if (lClientesStatus.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedlPagosProgramados = JsonConvert.SerializeObject(lClientesStatus);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlPagosProgramados, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Pagos Programados";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar el Json de pagos_programados

        #region Procedimiento para enviar el Json de empleados
        public async Task<string> EnviarEmpleadosJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "infoEmpleadosJson";

                try
                {
                    infoEmpleados infoempl = new infoEmpleados();
                    infoempl.sRuta = VarEntorno.iNoRuta.ToString();
                    infoempl.sEmpleado = VarEntorno.iNUsuario.ToString();
                    infoempl.bTipoBD = bTipoBD;
                    infoempl.sCorreo = sCorreo;

                    using (HttpClient httpClient = new HttpClient())
                    {
                        string serializedlPagosProgramados = JsonConvert.SerializeObject(infoempl);
                        var response = await httpClient.PostAsync(sUri,
                                                                    new StringContent(serializedlPagosProgramados, Encoding.UTF8, "application/json"));
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            sRespuesta = "Ok";
                        }
                        else
                        {
                            sRespuesta = "Error al Enviar Empleados";
                        }
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Procedimiento para enviar el Json de empleados

        #region Procedimiento para enviar el Json de pagare_clientes
        public async Task<string> EnviarPagareClientesJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            int iRuta = VarEntorno.iNoRuta;

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "pagare_clientesJson";

                try
                {
                    List<infoPagareClientes> lPagareClientes = new List<infoPagareClientes>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + iRuta.ToString() + " as run_clave, cln_clave, " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from pagare_clientes where pcb_entregado";
                        lPagareClientes = conn.Query<infoPagareClientes>(sQuery);
                    }

                    if (lPagareClientes.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedlPagareClientes = JsonConvert.SerializeObject(lPagareClientes);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedlPagareClientes, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Pagare Clientes";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Procedimiento para enviar el Json de pagare_clientes

        #region Método para enviar envase_sugerido
        public async Task<string> EnviarEnvaseSugeridoJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "envase_sugeridoJson";

                try
                {

                    List<infoEnvaseSugerido> lInfoEnvaseSugerido = new List<infoEnvaseSugerido>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        string sQuery = "select " + sRuta + " as run_clave, cln_clave, esc_envase, " +
                                        "esn_cantidad_vacio, esn_cantidad_lleno, esc_comentario " +
                                        "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                        "'" + sCorreo + "' as sCorreo " +
                                        "from envase_sugerido " +
                                        "order by cln_clave";
                        lInfoEnvaseSugerido = conn.Query<infoEnvaseSugerido>(sQuery);
                    }

                    if (lInfoEnvaseSugerido.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedInfoEnvaseSugerido = JsonConvert.SerializeObject(lInfoEnvaseSugerido);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedInfoEnvaseSugerido, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar información de la ruta";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar envase_sugerido

        #region Método para enviar documentos_cabecera
        public async Task<string> EnviarDocumentosCabeceraJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sUriEnvio = uriConexion();
            string sQuery = string.Empty;
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                string sUri = sUriEnvio + "documentos_cabeceraJson";

                try
                {
                    List<InfoDocumentosCabecera> lDocumentosCabecera = new List<InfoDocumentosCabecera>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        if (VarEntorno.cTipoVenta == 'A' || VarEntorno.cTipoVenta == 'P')
                        {
                            sQuery = "select vcn_cliente, vcn_folio, vcf_movimiento, vcn_importe, dcc_tipo, vcc_cadena_original, " +
                                     sRuta + " as run_clave, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from documentos_cabecera " +
                                     "where dcb_nuevo_documento;";
                        }
                        else
                        {
                            sQuery = "select d.vcn_cliente, d.vcn_folio, d.vcf_movimiento, d.vcn_importe, d.dcc_tipo, d.vcc_cadena_original, " +
                                     sRuta + " as run_clave, " +
                                     "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                     "'" + sCorreo + "' as sCorreo " +
                                     "from documentos_cabecera d " +
                                     "join FacturasVenta f on f.vcn_cliente = d.vcn_cliente " +
                                     "and f.vcn_folio = d.vcn_folio and f.vcf_movimiento = d.vcf_movimiento";
                        }

                        lDocumentosCabecera = conn.Query<InfoDocumentosCabecera>(sQuery);
                    }

                    if (lDocumentosCabecera.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedDocumentosCabecera = JsonConvert.SerializeObject(lDocumentosCabecera);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedDocumentosCabecera, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "documentos_cabecera enviado correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar documentos_cabecera";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar documentos_cabecera

        #region Método para enviar documentos_detalle
        public async Task<string> EnviarDocumentosDetalleJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "documentos_detalleJson";

                try
                {
                    List<InfoDocumentosDetalle> lDocumentosDetalle = new List<InfoDocumentosDetalle>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select DISTINCT dd.*, " +
                                 sRuta + " as run_clave, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from documentos_detalle dd " +
                                 "where vcn_cliente <> 0;";

                        lDocumentosDetalle = conn.Query<InfoDocumentosDetalle>(sQuery);
                    }

                    if (lDocumentosDetalle.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedDocumentosDetalle = JsonConvert.SerializeObject(lDocumentosDetalle);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedDocumentosDetalle, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "documentos_detalle enviado correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar documentos_detalle";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar documentos_detalle

        #region Método para enviar la tabla de clientes_datos_surtir
        public async Task<string> EnviarClientesDatosSurtirJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "clientes_datos_surtirJson";

                try
                {
                    List<InfoClientesDatosSurtir> lRequisitos = new List<InfoClientesDatosSurtir>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select c.*, " +
                                 sRuta + " as run_clave, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from clientes_requisitos_surtir c " +
                                 "where crb_actualizado";

                        lRequisitos = conn.Query<InfoClientesDatosSurtir>(sQuery);
                    }

                    if (lRequisitos.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedRequisitos = JsonConvert.SerializeObject(lRequisitos);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedRequisitos, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "clientes_requisitos_surtir enviado correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Requisitos";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de clientes_datos_surtir

        #region Método para enviar la tabla de anticipos
        public async Task<string> EnviarAnticiposJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();
            string sRuta = VarEntorno.iNoRuta.ToString();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "anticiposJson";

                try
                {
                    List<InfoAnticipos> lAnticipos = new List<InfoAnticipos>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select a.*, " +
                                 sRuta + " as run_clave, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from anticipos a " +
                                 "order by a.cln_clave";

                        lAnticipos = conn.Query<InfoAnticipos>(sQuery);
                    }

                    if (lAnticipos.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            string serializedAnticipos = JsonConvert.SerializeObject(lAnticipos);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedAnticipos, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "anticipos enviados correctamente";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar anticipos";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de anticipos

        #region Método para enviar la tabla de telefonos_clientes
        public async Task<string> EnviarTelefonosClientesJson(string sCorreo)
        {
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sUri = string.Empty;
            string sUriEnvio = uriConexion();

            if (sUriEnvio == "No hay conexión")
            {
                sRespuesta = sUriEnvio;
            }
            else
            {
                sUri = sUriEnvio + "telefonos_clientesJson";

                try
                {
                    List<InfoTelefonosClientes> lTelefonosClientes = new List<InfoTelefonosClientes>();

                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        sQuery = "select t.*, " +
                                 "'" + bTipoBD.ToString() + "' as sTipoBD, " +
                                 "'" + sCorreo + "' as sCorreo " +
                                 "from telefonos_clientes t " +
                                 "order by t.cln_clave";

                        lTelefonosClientes = conn.Query<InfoTelefonosClientes>(sQuery);
                    }

                    if (lTelefonosClientes.Count >= 1)
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            httpClient.Timeout = TimeSpan.FromMinutes(15);
                            string serializedTelefonosClientes = JsonConvert.SerializeObject(lTelefonosClientes);
                            var response = await httpClient.PostAsync(sUri,
                                                                      new StringContent(serializedTelefonosClientes, Encoding.UTF8, "application/json"));
                            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                sRespuesta = "Ok";
                            }
                            else
                            {
                                sRespuesta = "Error al Enviar Telefonos Clientes";
                            }
                        }
                    }
                    else
                    {
                        sRespuesta = "No existen datos";
                    }
                }
                catch (Exception ex)
                {
                    sRespuesta = "Error: " + ex.ToString();
                }
            }

            return sRespuesta;
        }
        #endregion Método para enviar la tabla de telefonos_clientes

        #endregion Métodos para Enviar los Archivos Json de cada Entidad

    }
}