using Android.Bluetooth;
using Base;
using Java.IO;
using Java.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class ImprimeResumenRVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ImprimeResumenRVM()
        {

        }

        /*Método para realizar la impresión de un Ticket de Resumen de Reparto en la Impresora configurada*/
        public void FtnImprimirResumenR(int iNoRutaParam)
        {
            try
            {
                utilerias = new Utilerias();
                venta_pagosSR PagosSR = new venta_pagosSR();

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
                                        utilerias.crearMensaje("Se ha conectado con la Impresora Bluetooth: '" + sImpresoraConfigurada + "'.");

                                        bufWriter = new BufferedWriter(new OutputStreamWriter(bthSocket.OutputStream));

                                        /*Variable para guardar la Ruta de Preventa de una Ruta de Reparto*/
                                        int iIdRuta = 0;

                                        /*Variable para guardar la suma en Efectivo de una Ruta de Reparto*/
                                        double dSumaEfectivo = 0.00;

                                        /*Variable para guardar el Total en Transferencia de una Ruta*/
                                        double dTotalTransferencias = 0.00;

                                        /*Variable para guardar la suma en Cheques de una Ruta de Reparto*/
                                        double dSumaCheques = 0.00;

                                        /*Variable para guardar la suma en Tarjetas de una Ruta de Reparto*/
                                        double dSumaTarjetas = 0.00;

                                        /*Variables para guardar los Totales de Envases por Tipo de Envase de una Ruta de Reparto*/
                                        string sIdEnvase = "";
                                        string sDescripCorta = "";
                                        int iSaldoInicial = 0;
                                        int iCargo = 0;
                                        int iAbono = 0;
                                        int iVenta = 0;
                                        int iSaldoFinal = 0;
                                        int iTotalSaldoInicial = 0;
                                        int iTotalCargo = 0;
                                        int iTotalAbono = 0;
                                        int iTotalVenta = 0;
                                        int iTotalSaldoFinal = 0;

                                        string sSql = "";

                                        List<CabeceraResumenR> listaCabeceraResumen = null;
                                        List<PagoEfectivoR> listaPagoEfectivo = null;
                                        List<PagoTransferenciasR> listaPagoTransferencias = null;
                                        List<PagoChequesR> listaPagoCheques = null;
                                        List<PagoTarjetasR> listaPagoTarjetas = null;
                                        List<TotalesEnvasesEntregaR> listaTotalesEnvasesEntrega = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene la Ruta de Preventa de una Ruta de Reparto*/
                                            sSql = "SELECT " +
                                                   "    DISTINCT cl.run_clave AS idRuta " +
                                                   "FROM venta_detalle AS vd " +
                                                   "INNER JOIN clientes AS cl ON (vd.vdn_cliente = cl.cln_clave) " +
                                                   "WHERE (vd.run_clave = " + iNoRutaParam + ")";

                                            listaCabeceraResumen = conn.Query<CabeceraResumenR>(sSql);

                                            if (listaCabeceraResumen.Count == 0)
                                            {
                                                iIdRuta = 0;
                                            }

                                            foreach (var cabeceraResumen in listaCabeceraResumen)
                                            {
                                                iIdRuta = cabeceraResumen.idRuta;
                                            }

                                            FtnImprimirTexto("      TICKET DE RESUMEN DE REPARTO");
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
                                            FtnImprimirTexto("Ruta Reparto: " + iNoRutaParam);
                                            FtnImprimirTexto("Ruta Preventa: " + iIdRuta);
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().Trim());
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().Trim());
                                            FtnImprimirTexto("------------------------------------------");

                                            /*Query que obtiene la suma en Efectivo de una Ruta de Reparto*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(vcn_monto_efe, 0.00)) as sumaEfectivo " +
                                                   "FROM venta_cabecera " + 
                                                   "WHERE (vcc_tipo_pago <> 'D') " +
                                                   "AND (efec_transf = 'EFECTIVO')";

                                            listaPagoEfectivo = conn.Query<PagoEfectivoR>(sSql);

                                            foreach (var pagoEfectivo in listaPagoEfectivo)
                                            {
                                                dSumaEfectivo = pagoEfectivo.sumaEfectivo;
                                            }

                                            /*Query que obtiene el Total en Transferencias de una Ruta de Reparto*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(vcn_monto_efe, 0.00)) AS totalTransferencias " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcc_tipo_pago <> 'D') " +
                                                   "AND (efec_transf = 'TRANSFERENCIA')";

                                            listaPagoTransferencias = conn.Query<PagoTransferenciasR>(sSql);

                                            foreach (var pagoTransferencia in listaPagoTransferencias)
                                            {
                                                dTotalTransferencias = pagoTransferencia.totalTransferencias;
                                            }

                                            /*Query que obtiene la suma en Cheques de una Ruta de Reparto*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(a.cheque, 0.00)) AS sumaCheques " +
                                                   "FROM ( " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque, 0.00)) AS cheque " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco <> 'TARJETA') " +
                                                   "    UNION " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque2, 0.00)) AS cheque " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco2 <> 'TARJETA') " +
                                                   "    UNION " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque3, 0.00)) AS cheque " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco3 <> 'TARJETA') " +
                                                   ") AS a";

                                            listaPagoCheques = conn.Query<PagoChequesR>(sSql);

                                            foreach (var pagoCheque in listaPagoCheques)
                                            {
                                                dSumaCheques = pagoCheque.sumaCheques;
                                            }

                                            /*Query que obtiene la suma en Tarjetas de una Ruta de Reparto*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(a.tarjetas, 0.00)) AS sumaTarjetas " +
                                                   "FROM ( " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque, 0.00)) AS tarjetas " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco = 'TARJETA') " +
                                                   "    UNION " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque2, 0.00)) AS tarjetas " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco2 = 'TARJETA') " +
                                                   "    UNION " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque3, 0.00)) AS tarjetas " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco3 = 'TARJETA') " +
                                                   ") AS a";

                                            listaPagoTarjetas = conn.Query<PagoTarjetasR>(sSql);

                                            foreach (var pagoTarjeta in listaPagoTarjetas)
                                            {
                                                dSumaTarjetas = pagoTarjeta.sumaTarjetas;
                                            }

                                            dSumaEfectivo = Convert.ToDouble(PagosSR.PagosxRuta("EFECTIVO"));
                                            dTotalTransferencias = Convert.ToDouble(PagosSR.PagosxRuta("TRANSFERENCIA"));
                                            dSumaCheques = Convert.ToDouble(PagosSR.PagosxRuta("CHEQUE"));
                                            dSumaTarjetas = Convert.ToDouble(PagosSR.PagosxRuta("TARJETA"));

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Total Efectivo........: $ " + String.Format("{0:#,0.00}", dSumaEfectivo).PadLeft(15));
                                            FtnImprimirTexto("Total Transferencias..: $ " + String.Format("{0:#,0.00}", dTotalTransferencias).PadLeft(15));
                                            FtnImprimirTexto("Total Cheques.........: $ " + String.Format("{0:#,0.00}", dSumaCheques).PadLeft(15));
                                            FtnImprimirTexto("Total Tarjetas........: $ " + String.Format("{0:#,0.00}", dSumaTarjetas).PadLeft(15));
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", (dSumaEfectivo + +dTotalTransferencias + dSumaCheques + dSumaTarjetas)).PadLeft(15));
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");

                                            /*Query que obtiene los Totales de Envases por Tipo de Envase de una Ruta de Reparto*/
                                            sSql = "SELECT " +
                                                    "	en.mec_envase AS idEnvase, " +
                                                    "   pr.ard_corta AS descripCorta, " +
                                                    "   SUM(COALESCE(en.men_saldo_inicial, 0)) AS saldoInicial, " +
                                                    "   SUM(COALESCE(en.men_cargo, 0)) AS cargo, " +
                                                    "   SUM(COALESCE(en.men_abono, 0)) AS abono, " +
                                                    "   SUM(COALESCE(en.men_venta, 0)) AS venta, " +
                                                    "   SUM(COALESCE(en.men_saldo_final, 0)) AS saldoFinal " +
                                                    "FROM envase en " +
                                                    "JOIN productos pr ON (en.mec_envase = pr.arc_clave) " +
                                                    "WHERE ((en.men_saldo_inicial <> 0) OR (en.men_cargo <> 0) OR (en.men_abono <> 0) OR (en.men_venta <> 0) OR (en.men_saldo_final <> 0)) " +
                                                    "GROUP BY en.mec_envase, pr.ard_corta " + 
                                                    "ORDER BY en.mec_envase";

                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("           MOVIMIENTO DE ENVASES");
                                            FtnImprimirTexto("   Envase      Ini   Car   Abo   Vta   Fin");

                                            listaTotalesEnvasesEntrega = conn.Query<TotalesEnvasesEntregaR>(sSql);

                                            foreach (var saldoEnvaseEntrega in listaTotalesEnvasesEntrega)
                                            {
                                                sIdEnvase = saldoEnvaseEntrega.idEnvase == null ? "" : saldoEnvaseEntrega.idEnvase.Trim();
                                                sDescripCorta = saldoEnvaseEntrega.descripCorta == null ? "" : saldoEnvaseEntrega.descripCorta.Trim();
                                                iSaldoInicial = saldoEnvaseEntrega.saldoInicial;
                                                iCargo = saldoEnvaseEntrega.cargo;
                                                iAbono = saldoEnvaseEntrega.abono;
                                                iVenta = saldoEnvaseEntrega.venta;
                                                iSaldoFinal = (((iSaldoInicial + iCargo) - iAbono) - iVenta);
                                                //iSaldoFinal = saldoEnvaseEntrega.saldoFinal;

                                                FtnImprimirTexto(sIdEnvase.PadRight(2).Substring(0, 2) + " " +
                                                                 sDescripCorta.PadRight(10).Substring(0, 10) +
                                                                 iSaldoInicial.ToString().PadLeft(5) + " " +
                                                                 iCargo.ToString().PadLeft(5) + " " +
                                                                 iAbono.ToString().PadLeft(5) + " " +
                                                                 iVenta.ToString().PadLeft(5) + " " +
                                                                 iSaldoFinal.ToString().PadLeft(5));

                                                iTotalSaldoInicial += iSaldoInicial;
                                                iTotalCargo += iCargo;
                                                iTotalAbono += iAbono;
                                                iTotalVenta += iVenta;
                                                iTotalSaldoFinal += (((iSaldoInicial + iCargo) - iAbono) - iVenta);
                                                //iTotalSaldoFinal += iTotalSaldoFinal;
                                            }

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("   TOTALES   " +
                                                             iTotalSaldoInicial.ToString().PadLeft(5) + " " +
                                                             iTotalCargo.ToString().PadLeft(5) + " " +
                                                             iTotalAbono.ToString().PadLeft(5) + " " +
                                                             iTotalVenta.ToString().PadLeft(5) + " " +
                                                             iTotalSaldoFinal.ToString().PadLeft(5));

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
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Resumen de Reparto.\nDetalle: " + exc.Message);
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

    /*Clase para guardar la Ruta de Preventa de una Ruta de Reparto*/
    class CabeceraResumenR
    {
        public int idRuta { get; set; }
    }

    /*Clase para guardar el Total en Efectivo de una Ruta de Reparto*/
    class PagoEfectivoR
    {
        public double sumaEfectivo { get; set; }
    }

    /*Clase para guardar el Total en Transferencias de una Ruta*/
    class PagoTransferenciasR
    {
        public double totalTransferencias { get; set; }
    }

    /*Clase para guardar el Total en Cheques de una Ruta de Reparto*/
    class PagoChequesR
    {
        public double sumaCheques { get; set; }
    }

    /*Clase para guardar el Total en Tarjetas de una Ruta de Reparto*/
    class PagoTarjetasR
    {
        public double sumaTarjetas { get; set; }
    }

    /*Clase para guardar los Totales de Envases por Tipo de Envase de una Ruta de Reparto*/
    class TotalesEnvasesEntregaR
    {
        public string idEnvase { get; set; }
        public string descripCorta { get; set; }
        public int saldoInicial { get; set; }
        public int cargo { get; set; }
        public int abono { get; set; }
        public int venta { get; set; }
        public int saldoFinal { get; set; }
    }
}
