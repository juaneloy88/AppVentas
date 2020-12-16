using Android.Bluetooth;
using Base;
using Java.IO;
using Java.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo;

namespace VentasPsion.VistaModelo
{
    class ZeImprimeAbonoEnvaseAVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ZeImprimeAbonoEnvaseAVM()
        {

        }

        /*Método para realizar la Impresión de un Ticket de Venta en la Impresora configurada*/
        public async void FtnImprimirTicketAbonoEnvaseA(int iIdClienteParam, string sNoTicketParam)
        {
            try
            {
                utilerias = new Utilerias();

                string sImpresoraConfigurada = Plugin.Settings.CrossSettings.Current.GetValueOrDefault<string>("Impresora", "");

                if (string.IsNullOrEmpty(sImpresoraConfigurada.Trim()))
                {
                    utilerias.crearMensajeLargo("¡Atención!\nNo hay Impresora Bluetooth configurada actualmente.");
                }
                else
                {
                    bthAdapter = BluetoothAdapter.DefaultAdapter;

                    if (bthAdapter == null)
                    {
                        utilerias.crearMensajeLargo("¡Atención!\nNo se ha encontrado Adaptador Bluetooth.");
                    }
                    else
                    {
                        if (bthAdapter.IsEnabled == false)
                        {
                            utilerias.crearMensajeLargo("¡Atención!\nEl Adaptador Bluetooth no está habilitado.");
                        }
                        else
                        {
                            foreach (var dispositivo in bthAdapter.BondedDevices)
                            {
                                if ((dispositivo.Name.ToUpper().Trim() + " - " + dispositivo.Address.ToUpper().Trim()) == sImpresoraConfigurada.ToUpper().Trim())
                                {
                                    bthDevice = dispositivo;
                                    break;
                                }
                            }

                            if (bthDevice == null)
                            {
                                utilerias.crearMensajeLargo("¡Atención!\nNo se ha encontrado Impresora Bluetooth a la cual conectarse.");
                            }
                            else
                            {
                                UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                                if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                                    bthSocket = bthDevice.CreateInsecureRfcommSocketToServiceRecord(uuid);
                                else
                                    bthSocket = bthDevice.CreateRfcommSocketToServiceRecord(uuid);

                                if (bthSocket == null)
                                {
                                    utilerias.crearMensajeLargo("¡Atención!\nNo se pudo conectar con el Socket Bluetooth.");
                                }
                                else
                                {
                                    bthSocket.Connect();

                                    if (bthSocket.IsConnected == false)
                                    {
                                        utilerias.crearMensajeLargo("¡Atención!\nNo se pudo conectar a la Impresora Bluetooth.");
                                    }
                                    else
                                    {
                                        utilerias.crearMensajeLargo("Se ha conectado con la Impresora Bluetooth: '" + sImpresoraConfigurada + "'.");

                                        bufWriter = new BufferedWriter(new OutputStreamWriter(bthSocket.OutputStream));

                                        /*Variables para datos generales de un Cliente*/
                                        int iIdCliente = 0;
                                        int iIdRuta = 0;
                                        string sNombreCliente = "";
                                        string sRfc = "";
                                        string sDomicilio = "";

                                        /*Variables para datos de los Saldos de Envases despues de un Abono de Envase de un Cliente*/
                                        string sIdEnvase = "";
                                        string sDescripCorta = "";
                                        int iSaldoInicial = 0;
                                        int iCargo = 0;
                                        int iAbono = 0;
                                        int iVenta = 0;
                                        int iSaldoFinal = 0;
                                        int iSaldoFinalCalculado = 0;

                                        string sSql = "";

                                        List<ClienteAbonoEnvase> listaClienteAbonoEnvase = null;
                                        List<SaldosAbonoEnvase> listaSaldosAbonoEnvase = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene los datos generales de un Cliente*/
                                            sSql = "SELECT " +
                                                    "    cl.cln_clave AS idCliente, " +
                                                    "    cl.run_clave AS idRuta, " +
                                                    "    cl.clc_nombre AS nombreCliente, " +
                                                    "    cl.clc_nombre_comercial AS nombreComercial, " +
                                                    "    cl.clc_rfc AS rfc, " +
                                                    "    cl.clc_domicilio AS domicilio " +
                                                    "FROM clientes AS cl " +
                                                    "WHERE (cl.cln_clave = " + iIdClienteParam + ")";

                                            listaClienteAbonoEnvase = conn.Query<ClienteAbonoEnvase>(sSql);

                                            if (listaClienteAbonoEnvase.Count == 0)
                                            {
                                                utilerias.crearMensajeLargo("El Cliente NO tiene Abonos de Envase para generar el Ticket.");
                                                return;
                                            }

                                            foreach (var clienteAbonoEnvase in listaClienteAbonoEnvase)
                                            {
                                                iIdCliente = clienteAbonoEnvase.idCliente;
                                                iIdRuta = clienteAbonoEnvase.idRuta;
                                                sNombreCliente = clienteAbonoEnvase.nombreCliente == null ? "" : clienteAbonoEnvase.nombreCliente.Trim();
                                                if (sNombreCliente.Trim() == "")
                                                    sNombreCliente = clienteAbonoEnvase.nombreComercial == null ? "" : clienteAbonoEnvase.nombreComercial.Trim();
                                                sRfc = clienteAbonoEnvase.rfc == null ? "" : clienteAbonoEnvase.rfc.Trim();
                                                sDomicilio = clienteAbonoEnvase.domicilio == null ? "" : clienteAbonoEnvase.domicilio.Trim();
                                            }

                                            FtnImprimirTexto("   TICKET DE RESUMEN DE ABONO DE ENVASE         ");
                                            FtnImprimirTexto("------------------------------------------------");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto((char)27 + "F" + (char)1 + (char)27 + "k4" + (char)28);
                                            FtnImprimirTexto("            CERVEZA CORONA EN                   ");
                                            FtnImprimirTexto("       AGUASCALIENTES S.A. DE C.V.              ");
                                            //FtnImprimirTexto((char)29 + "");
                                            FtnImprimirTexto("Reg. Fed. de Contribuyentes CCA-810221-N7A      ");
                                            FtnImprimirTexto("Direccion: Blvd. Jose Maria Chavez #2906        ");
                                            FtnImprimirTexto("Cd. Industrial, C.P. 20290, Ags., Ags.          ");
                                            //FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("Ruta: " + iIdRuta.ToString().PadRight(42));
                                            //FtnImprimirTexto("Ticket: " + sNoTicketParam);
                                            //FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().Trim());
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToShortDateString().Trim().PadRight(41));
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().Trim().PadRight(42));
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("Cliente: " + (iIdCliente + " - " + sNombreCliente).PadRight(39).Substring(0, 39));
                                            FtnImprimirTexto("RFC: " + sRfc.Trim().PadRight(43));
                                            FtnImprimirTexto("Direccion: " + sDomicilio.Trim().PadRight(133));
                                            FtnImprimirTexto("------------------------------------------------");

                                            /*Query que obtiene los datos de los Saldos de Envases despues de un Abono de Envase de un Cliente*/
                                            //sSql = "SELECT " +
                                            //        "   en.cln_clave AS idCliente, " +
                                            //        "	en.mec_envase AS idEnvase, " +
                                            //        "   pr.ard_corta AS descripCorta, " +
                                            //        "   en.men_saldo_inicial AS saldoInicial, " +
                                            //        "   en.men_cargo AS cargo, " +
                                            //        "   en.men_abono AS abono, " +
                                            //        "   en.men_venta AS venta, " +
                                            //        "   en.men_saldo_final AS saldoFinal, " +
                                            //        "   (((en.men_saldo_inicial + en.men_cargo) - en.men_abono) - en.men_venta) AS saldoFinalCalculado " +
                                            //        "FROM envase en " +
                                            //        "JOIN productos pr ON (en.mec_envase = pr.arc_clave) " +
                                            //        "WHERE (en.cln_clave = " + iIdClienteParam + ") " +
                                            //        "AND (en.men_folio = '" + sNoTicketParam + "') " +
                                            //        "AND ((en.men_saldo_inicial <> 0) OR (en.men_cargo <> 0) OR (en.men_abono <> 0) OR (en.men_venta <> 0) OR (en.men_saldo_final <> 0)) " +
                                            //        "ORDER BY en.mec_envase";

                                            sSql = "SELECT " +
                                                    "   en.cln_clave AS idCliente, " +
                                                    "	en.mec_envase AS idEnvase, " +
                                                    "   pr.ard_corta AS descripCorta, " +
                                                    "   en.men_saldo_inicial AS saldoInicial, " +
                                                    "   en.men_cargo AS cargo, " +
                                                    "   en.men_abono AS abono, " +
                                                    "   en.men_venta AS venta, " +
                                                    "   en.men_saldo_final AS saldoFinal, " +
                                                    "   (((en.men_saldo_inicial + en.men_cargo) - en.men_abono) - en.men_venta) AS saldoFinalCalculado " +
                                                    "FROM envase en " +
                                                    "JOIN productos pr ON (en.mec_envase = pr.arc_clave) " +
                                                    "WHERE (en.cln_clave = " + iIdClienteParam + ") " +
                                                    "AND ((en.men_saldo_inicial <> 0) OR (en.men_cargo <> 0) OR (en.men_abono <> 0) OR (en.men_venta <> 0) OR (en.men_saldo_final <> 0)) " +
                                                    "ORDER BY en.mec_envase";

                                            FtnImprimirTexto("                                                ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("           MOVIMIENTO DE ENVASES                ");
                                            FtnImprimirTexto("   Envase      Ini   Car   Abo   Vta   Fin      ");

                                            listaSaldosAbonoEnvase = conn.Query<SaldosAbonoEnvase>(sSql);

                                            foreach (var SaldoEnvase in listaSaldosAbonoEnvase)
                                            {
                                                sIdEnvase = SaldoEnvase.idEnvase == null ? "" : SaldoEnvase.idEnvase.Trim();
                                                sDescripCorta = SaldoEnvase.descripCorta == null ? "" : SaldoEnvase.descripCorta.Trim();
                                                iSaldoInicial = SaldoEnvase.saldoInicial;
                                                iCargo = SaldoEnvase.cargo;
                                                iAbono = SaldoEnvase.abono;
                                                iVenta = SaldoEnvase.venta;
                                                iSaldoFinal = SaldoEnvase.saldoFinal;
                                                iSaldoFinalCalculado = (((iSaldoInicial + iCargo) - iAbono) - iVenta);

                                                FtnImprimirTexto(sIdEnvase.PadRight(2).Substring(0, 2) + " " +
                                                                 sDescripCorta.PadRight(10).Substring(0, 10) +
                                                                 iSaldoInicial.ToString().PadLeft(5) + " " +
                                                                 iCargo.ToString().PadLeft(5) + " " +
                                                                 iAbono.ToString().PadLeft(5) + " " +
                                                                 iVenta.ToString().PadLeft(5) + " " +
                                                                 iSaldoFinalCalculado.ToString().PadLeft(5) + "      ");
                                            }

                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("                                                ");
                                            FtnImprimirTexto("POR EL PRESENTE PAGARE PROMETO PAGAR INCONDICIO-");
                                            FtnImprimirTexto("NALMENTE A LA EMPRESA EMISORA EL IMPORTE INDICA-");
                                            FtnImprimirTexto("DO COMO ADEUDO TOTAL EN ESTE DOCUMENTO EN EL DO-");
                                            FtnImprimirTexto("MICILIO DE ESTA ULTIMA, EN VIRTUD DE QUE HE RE- ");
                                            FtnImprimirTexto("CIBIDO A MI ENTERA SATISFACCION LA CANTIDAD IN- ");
                                            FtnImprimirTexto("                                                ");
                                            FtnImprimirTexto("DICADA. LOS SALDOS INDICADOS EN EL PRESENTE DO- ");
                                            FtnImprimirTexto("CUMENTO, INCLUYEN LAS TRANSACCIONES HASTA ESTE  ");
                                            FtnImprimirTexto("MOMENTO Y CONSIDERAN LA RECEPCION DE CHEQUES    ");
                                            FtnImprimirTexto("PENDIENTE DE COBRO, POR LO QUE DICHOS SALDOS ES-");
                                            FtnImprimirTexto("TAN SUJETOS AL COBRO DE LOS MISMOS POR OPERACIO-");
                                            FtnImprimirTexto("NES DE VENTA DE PRODUCTO Y PRESTAMO DE ENVASE.  ");
                                            FtnImprimirTexto("                                                ");
                                            FtnImprimirTexto("       ___________________________________      ");
                                            FtnImprimirTexto("                Firma del Cliente               ");
                                            FtnImprimirTexto("                                                ");
                                            FtnImprimirTexto("       ___________________________________      ");
                                            FtnImprimirTexto("   Nombre del Cliente y/o Representante Legal   ");
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("  Tiene 15 dias naturales para hacer cualquier  ");
                                            FtnImprimirTexto("            cambio de datos fiscales.           ");
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("                                                ");
                                            FtnImprimirTexto("                                                ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Resumen de Abono de Envase.\nDetalle: " + exc.Message);
            }
            finally
            {
                if (bthSocket != null)
                    bthSocket.Close();

                bthDevice = null;
                bthAdapter = null;
            }
        }

        /*Método para imprime el texto recibido como parámetro*/
        public void FtnImprimirTexto(string sTextoPorImprimir)
        {
            sTextoPorImprimir = sTextoPorImprimir.Replace("Ñ", "N");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Á", "A");
            sTextoPorImprimir = sTextoPorImprimir.Replace("É", "E");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Í", "I");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Ó", "O");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Ú", "U");
            bufWriter.Write(sTextoPorImprimir + "\n");
            bufWriter.Flush();
        }
    }
}
