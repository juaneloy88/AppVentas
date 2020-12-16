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
    class ImprimeAbonoEnvaseAVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ImprimeAbonoEnvaseAVM()
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

                                            FtnImprimirTexto("   TICKET DE RESUMEN DE ABONO DE ENVASE");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto((char)27 + "F" + (char)1 + (char)27 + "k4" + (char)28);
                                            FtnImprimirTexto("            CERVEZA CORONA EN");
                                            FtnImprimirTexto("       AGUASCALIENTES S.A. DE C.V.");
                                            FtnImprimirTexto((char)29 + "");
                                            FtnImprimirTexto("Reg. Fed. de Contribuyentes CCA-810221-N7A");
                                            FtnImprimirTexto("Direccion: Blvd. Jose Maria Chavez #2906");
                                            FtnImprimirTexto("Cd. Industrial, C.P. 20290, Ags., Ags.");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Ruta: " + iIdRuta);
                                            //FtnImprimirTexto("Ticket: " + sNoTicketParam);
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().Trim());
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().Trim());
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Cliente: " + iIdCliente + " - " + sNombreCliente);
                                            FtnImprimirTexto("RFC: " + sRfc);
                                            FtnImprimirTexto("Direccion: " + sDomicilio);
                                            FtnImprimirTexto("------------------------------------------");

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

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("           MOVIMIENTO DE ENVASES");
                                            FtnImprimirTexto("   Envase      Ini   Car   Abo   Vta   Fin");

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
                                                                 iSaldoFinalCalculado.ToString().PadLeft(5));
                                            }

                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
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
            bufWriter.Write(sTextoPorImprimir + "\n");
            bufWriter.Flush();
        }
    }

    /*Clase para guardar los datos generales de un Cliente*/
    class ClienteAbonoEnvase
    {
        public int idCliente { get; set; }
        public int idRuta { get; set; }
        public string nombreCliente { get; set; }
        public string nombreComercial { get; set; }
        public string rfc { get; set; }
        public string domicilio { get; set; }
    }

    /*Clase para guardar los datos de los Saldos de Envases despues de un Abono de Envase de un Cliente*/
    class SaldosAbonoEnvase
    {
        public int idCliente { get; set; }
        public string idEnvase { get; set; }
        public string descripCorta { get; set; }
        public int saldoInicial { get; set; }
        public int cargo { get; set; }
        public int abono { get; set; }
        public int venta { get; set; }
        public int saldoFinal { get; set; }
        public int saldoFinalCalculado { get; set; }
    }
}
