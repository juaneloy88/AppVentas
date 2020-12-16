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
    public class ImprimeResumenAPVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ImprimeResumenAPVM()
        {

        }

        /*Método para realizar la impresión de un Ticket de Resumen de Venta en la Impresora configurada*/
        public void FtnImprimirResumenAP(int iNoRutaParam)
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

                                        /*Variables para datos del Resumen de Venta de una Ruta*/
                                        int iCantidad = 0;
                                        string sIdProducto = "";
                                        string sDescripcion = "";
                                        string sIdTipo = "";
                                        string sClasificacion = "";
                                        string sOrdenTicket = "";
                                        int iCantTotal = 0;

                                        /*Variable para guardar el Total de Pagos de una Ruta*/
                                        double dTotalPago = 0.00;

                                        /*Variable para guardar el Total en Efectivo de una Ruta*/
                                        double dTotalEfectivo = 0.00;

                                        /*Variable para guardar el Total en Transferencia de una Ruta*/
                                        double dTotalTransferencias = 0.00;

                                        /*Variable para guardar el Total en Cheques de una Ruta*/
                                        double dTotalCheques = 0.00;

                                        /*Variable para guardar el Total en Tarjetas de una Ruta*/
                                        double dTotalTarjetas = 0.00;

                                        /*Variable para guardar el Total en Bonificaciones de una Ruta*/
                                        double dTotalBonificaciones = 0.00;

                                        /*Variables para guardar los Totales de Envases por Tipo de Envase de una Ruta*/
                                        string sIdEnvase = "";
                                        string sDescripCorta = "";
                                        int iSaldoInicial = 0;
                                        int iCargo = 0;
                                        int iAbono = 0;
                                        int iVenta = 0;
                                        int iSaldoFinal = 0;
                                        int iSaldoFinalCalculado = 0;
                                        int iTotalSaldoInicial = 0;
                                        int iTotalCargo = 0;
                                        int iTotalAbono = 0;
                                        int iTotalVenta = 0;
                                        int iTotalSaldoFinal = 0;
                                        int iTotalSaldoFinalCalculado = 0;

                                        string sSql = "";

                                        List<ResumenVentaAP> listaResumenVenta = null;
                                        List<PagoVentaAP> listaPagoVenta = null;
                                        List<PagoEfectivoAP> listaPagoEfectivo = null;
                                        List<PagoTransferenciasAP> listaPagoTransferencias = null;
                                        List<PagoChequesAP> listaPagoCheques = null;
                                        List<PagoTarjetasAP> listaPagoTarjetas = null;
                                        List<PagoBonificacionesAP> listaPagoBonificaciones = null;
                                        List<TotalesEnvasesAP> listaTotalesEnvases = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene los datos del Resumen de Venta por Grupos de Productos de una Ruta*/
                                            sSql = "SELECT " +
                                                    "    CASE WHEN (pr.arc_clave IS NULL) THEN vd.vdn_producto ELSE pr.arc_clave END AS idProducto, " +
                                                    "    pr.ard_descripcion AS descripcion, " +
                                                    "    SUM(COALESCE(vd.vdn_venta, 0)) AS cantidad, " +
                                                    "    pr.arc_produ AS idTipo, " +
                                                    "    CASE " +
                                                    "       WHEN (pr.arc_produ = 'C') THEN 'CERVEZA' " +
                                                    "       WHEN (pr.arc_produ = 'R') THEN 'REFRESCO' " +
                                                    "       WHEN (pr.arc_produ = 'E') THEN 'ENVASE' " +
                                                    "       WHEN (pr.arc_produ = 'A') THEN 'AGUA' " +
                                                    "       WHEN (pr.arc_produ = 'V') THEN 'VARIOS' " +
                                                    "       ELSE 'OBSEQUIO' " +
                                                    "    END AS clasificacion, " +
                                                    "    CASE " +
                                                    "       WHEN (pr.arc_produ = 'C') THEN '1' " +
                                                    "       WHEN (pr.arc_produ = 'R') THEN '2' " +
                                                    "       WHEN (pr.arc_produ = 'A') THEN '3' " +
                                                    "       WHEN (pr.arc_produ = 'V') THEN '4' " +
                                                    "       WHEN (pr.arc_produ = 'E') THEN '5' " +
                                                    "       ELSE '6' " +
                                                    "    END AS ordenTicket " +
                                                    "FROM venta_detalle AS vd " +
                                                    "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                                    "WHERE (vd.run_clave = " + iNoRutaParam + ") " +
                                                    "GROUP BY pr.arc_produ, pr.arc_clave, vd.vdn_producto, pr.ard_descripcion " +
                                                    "UNION " +
                                                    "SELECT " +
                                                    "    'TOTAL' AS idProducto, " +
                                                    "    CASE " +
                                                    "       WHEN (pr.arc_produ = 'C') THEN 'TOTAL : CERVEZA' " +
                                                    "       WHEN (pr.arc_produ = 'R') THEN 'TOTAL : REFRESCO' " +
                                                    "       WHEN (pr.arc_produ = 'E') THEN 'TOTAL : ENVASE' " +
                                                    "       WHEN (pr.arc_produ = 'A') THEN 'TOTAL : AGUA' " +
                                                    "       WHEN (pr.arc_produ = 'V') THEN 'TOTAL : VARIOS' " +
                                                    "       ELSE 'TOTAL : OBSEQUIO' " +
                                                    "    END AS descripcion, " +
                                                    "	 SUM(COALESCE(vd.vdn_venta, 0)) AS cantidad, " +
                                                    "    '' AS idTipo, " +
                                                    "    CASE " +
                                                    "       WHEN (pr.arc_produ = 'C') THEN 'CERVEZA' " +
                                                    "       WHEN (pr.arc_produ = 'R') THEN 'REFRESCO' " +
                                                    "       WHEN (pr.arc_produ = 'E') THEN 'ENVASE' " +
                                                    "       WHEN (pr.arc_produ = 'A') THEN 'AGUA' " +
                                                    "       WHEN (pr.arc_produ = 'V') THEN 'VARIOS' " +
                                                    "       ELSE 'OBSEQUIO' " +
                                                    "    END AS clasificacion, " +
                                                    "    CASE " +
                                                    "       WHEN (pr.arc_produ = 'C') THEN '1.1' " +
                                                    "       WHEN (pr.arc_produ = 'R') THEN '2.1' " +
                                                    "       WHEN (pr.arc_produ = 'A') THEN '3.1' " +
                                                    "       WHEN (pr.arc_produ = 'V') THEN '4.1' " +
                                                    "       WHEN (pr.arc_produ = 'E') THEN '5.1' " +
                                                    "       ELSE '6.1' " +
                                                    "    END AS ordenTicket " +
                                                    "FROM venta_detalle AS vd " +
                                                    "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                                    "WHERE (vd.run_clave = " + iNoRutaParam + ") " +
                                                    "GROUP BY pr.arc_produ " +
                                                    "ORDER BY ordenTicket, idProducto";

                                            listaResumenVenta = conn.Query<ResumenVentaAP>(sSql);

                                            if (listaResumenVenta.Count == 0)
                                            {
                                                utilerias.crearMensajeLargo("La Ruta NO tiene Ventas para generar el Ticket de Resumen de Venta.");
                                                return;
                                            }

                                            FtnImprimirTexto("       TICKET DE RESUMEN DE VENTA");
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
                                            FtnImprimirTexto("Ruta: " + iNoRutaParam);
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().Trim());
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().Trim());
                                            FtnImprimirTexto("------------------------------------------");

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Clave  Producto                   Cantidad");
                                            FtnImprimirTexto("------------------------------------------");

                                            foreach (var resumenVenta in listaResumenVenta)
                                            {
                                                sIdProducto = resumenVenta.idProducto == null ? "" : resumenVenta.idProducto.Trim();
                                                sDescripcion = resumenVenta.descripcion == null ? "" : resumenVenta.descripcion.Trim();
                                                iCantidad = resumenVenta.cantidad;
                                                sIdTipo = resumenVenta.idTipo == null ? "" : resumenVenta.idTipo.Trim();
                                                sClasificacion = resumenVenta.clasificacion == null ? "" : resumenVenta.clasificacion.Trim();
                                                sOrdenTicket = resumenVenta.ordenTicket == null ? "" : resumenVenta.ordenTicket.Trim();

                                                if (sDescripcion.PadRight(5).Substring(0, 5) == "TOTAL")
                                                {
                                                    FtnImprimirTexto(" ");
                                                    FtnImprimirTexto("       " + sDescripcion.ToString().PadRight(26).Substring(0, 26) + "  " + iCantidad.ToString().PadLeft(6));
                                                    FtnImprimirTexto("------------------------------------------");
                                                }
                                                else
                                                {
                                                    FtnImprimirTexto(sIdProducto.ToString().PadRight(4).Substring(0, 4) + "   " +
                                                                     sDescripcion.ToString().PadRight(26).Substring(0, 26) + "  " +
                                                                     iCantidad.ToString().PadLeft(6));

                                                    iCantTotal += iCantidad;
                                                }
                                            }

                                            /*Query que obtiene el el Total de Pagos de una Ruta*/
                                            sSql = "SELECT " +
                                                  "    SUM(COALESCE(vcn_monto_pago, 0.00)) AS totalPago " +
                                                  "FROM venta_cabecera";

                                            listaPagoVenta = conn.Query<PagoVentaAP>(sSql);

                                            foreach (var pagoVenta in listaPagoVenta)
                                            {
                                                dTotalPago = pagoVenta.totalPago;
                                            }

                                            /*Query que obtiene el Total en Efectivo de una Ruta*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(vcn_monto_efe, 0.00)) AS totalEfectivo " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (efec_transf = 'EFECTIVO')";

                                            listaPagoEfectivo = conn.Query<PagoEfectivoAP>(sSql);

                                            foreach (var pagoEfectivo in listaPagoEfectivo)
                                            {
                                                dTotalEfectivo = pagoEfectivo.totalEfectivo;
                                            }

                                            /*Query que obtiene el Total en Transferencias de una Ruta*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(vcn_monto_efe, 0.00)) AS totalTransferencias " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (efec_transf = 'TRANSFERENCIA')";

                                            listaPagoTransferencias = conn.Query<PagoTransferenciasAP>(sSql);

                                            foreach (var pagoTransferencia in listaPagoTransferencias)
                                            {
                                                dTotalTransferencias = pagoTransferencia.totalTransferencias;
                                            }

                                            /*Query que obtiene el Total en Cheques de una Ruta*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(che.cheques, 0.00)) AS totalCheques " +
                                                   "FROM ( " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque, 0.00)) AS cheques " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco <> 'TARJETA') " +
                                                   "    UNION " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque2, 0.00)) AS cheques " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco2 <> 'TARJETA') " +
                                                   "    UNION " +
                                                   "    SELECT " +
                                                   "        SUM(COALESCE(vcn_monto_cheque3, 0.00)) AS cheques " +
                                                   "    FROM venta_cabecera " +
                                                   "    WHERE (vcc_banco3 <> 'TARJETA') " +
                                                   ") AS che";

                                            listaPagoCheques = conn.Query<PagoChequesAP>(sSql);

                                            foreach (var pagoCheques in listaPagoCheques)
                                            {
                                                dTotalCheques = pagoCheques.totalCheques;
                                            }

                                            /*Query que obtiene el Total en Tarjetas de una Ruta*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(trj.tarjetas, 0.00)) AS totalTarjetas " +
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
                                                   ") AS trj";

                                            listaPagoTarjetas = conn.Query<PagoTarjetasAP>(sSql);

                                            foreach (var pagoTarjetas in listaPagoTarjetas)
                                            {
                                                dTotalTarjetas = pagoTarjetas.totalTarjetas;
                                            }

                                            /*Query que obtiene el Total en Bonificaciones de una Ruta*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(boi_documento, 0.00)) AS totalBonificaciones " +
                                                   "FROM bonificaciones " +
                                                   "WHERE (boc_folio_venta != '')";

                                            listaPagoBonificaciones = conn.Query<PagoBonificacionesAP>(sSql);

                                            foreach (var pagoBonificaciones in listaPagoBonificaciones)
                                            {
                                                dTotalBonificaciones = pagoBonificaciones.totalBonificaciones;
                                            }
                                            
                                            dTotalEfectivo = Convert.ToDouble(PagosSR.PagosxRuta("EFECTIVO"));
                                            dTotalTransferencias = Convert.ToDouble(PagosSR.PagosxRuta("TRANSFERENCIA"));
                                            dTotalCheques = Convert.ToDouble(PagosSR.PagosxRuta("CHEQUE"));
                                            dTotalTarjetas = Convert.ToDouble(PagosSR.PagosxRuta("TARJETA"));

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Total Cajas...........:   " + iCantTotal.ToString().PadLeft(15));
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Total Efectivo........: $ " + String.Format("{0:#,0.00}", dTotalEfectivo).PadLeft(15));
                                            FtnImprimirTexto("Total Transferencias..: $ " + String.Format("{0:#,0.00}", dTotalTransferencias).PadLeft(15));
                                            FtnImprimirTexto("Total Cheques.........: $ " + String.Format("{0:#,0.00}", dTotalCheques).PadLeft(15));
                                            FtnImprimirTexto("Total Tarjetas........: $ " + String.Format("{0:#,0.00}", dTotalTarjetas).PadLeft(15));
                                            FtnImprimirTexto("Total Bonificaciones..: $ " + String.Format("{0:#,0.00}", dTotalBonificaciones).PadLeft(15));
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", (dTotalEfectivo + dTotalCheques + dTotalTarjetas + dTotalBonificaciones)).PadLeft(15));
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", dTotalPago).PadLeft(15));
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");

                                            /*Query que obtiene los Totales de Envases por Tipo de Envase de una Ruta*/
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

                                            listaTotalesEnvases = conn.Query<TotalesEnvasesAP>(sSql);

                                            foreach (var totalesEnvases in listaTotalesEnvases)
                                            {
                                                sIdEnvase = totalesEnvases.idEnvase == null ? "" : totalesEnvases.idEnvase.Trim();
                                                sDescripCorta = totalesEnvases.descripCorta == null ? "" : totalesEnvases.descripCorta.Trim();
                                                iSaldoInicial = totalesEnvases.saldoInicial;
                                                iCargo = totalesEnvases.cargo;
                                                iAbono = totalesEnvases.abono;
                                                iVenta = totalesEnvases.venta;
                                                //iSaldoFinal = totalesEnvases.saldoFinal;
                                                iSaldoFinalCalculado = (((iSaldoInicial + iCargo) - iAbono) - iVenta);

                                                FtnImprimirTexto(sIdEnvase.PadRight(2).Substring(0, 2) + " " +
                                                                 sDescripCorta.PadRight(10).Substring(0, 10) +
                                                                 iSaldoInicial.ToString().PadLeft(5) + " " +
                                                                 iCargo.ToString().PadLeft(5) + " " +
                                                                 iAbono.ToString().PadLeft(5) + " " +
                                                                 iVenta.ToString().PadLeft(5) + " " +
                                                                 iSaldoFinalCalculado.ToString().PadLeft(5));

                                                iTotalSaldoInicial += iSaldoInicial;
                                                iTotalCargo += iCargo;
                                                iTotalAbono += iAbono;
                                                iTotalVenta += iVenta;
                                                //iTotalSaldoFinal += iTotalSaldoFinal;
                                                iTotalSaldoFinalCalculado += (((iSaldoInicial + iCargo) - iAbono) - iVenta);
                                            }

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("   TOTALES   " +
                                                             iTotalSaldoInicial.ToString().PadLeft(5) + " " +
                                                             iTotalCargo.ToString().PadLeft(5) + " " +
                                                             iTotalAbono.ToString().PadLeft(5) + " " +
                                                             iTotalVenta.ToString().PadLeft(5) + " " +
                                                             iTotalSaldoFinalCalculado.ToString().PadLeft(5));

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
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Resumen de Venta.\nDetalle: " + exc.Message);
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

    /*Clase para guardar los datos del Resumen de Venta por Grupos de Productos de una Ruta*/
    class ResumenVentaAP
    {
        public string idProducto { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public string idTipo { get; set; }
        public string clasificacion { get; set; }
        public string ordenTicket { get; set; }
    }

    /*Clase para guardar el Total de Pagos de una Ruta*/
    class PagoVentaAP
    {
        public double totalPago { get; set; }
    }

    /*Clase para guardar el Total en Efectivo de una Ruta*/
    class PagoEfectivoAP
    {
        public double totalEfectivo { get; set; }
    }

    /*Clase para guardar el Total en Transferencias de una Ruta*/
    class PagoTransferenciasAP
    {
        public double totalTransferencias { get; set; }
    }

    /*Clase para guardar el Total en Cheques de una Ruta*/
    class PagoChequesAP
    {
        public double totalCheques { get; set; }
    }

    /*Clase para guardar el Total en Tarjetas de una Ruta*/
    class PagoTarjetasAP
    {
        public double totalTarjetas { get; set; }
    }

    /*Clase para guardar el Total en Bonificaciones de una Ruta*/
    class PagoBonificacionesAP
    {
        public double totalBonificaciones { get; set; }
    }

    /*Clase para guardar los Totales de Envases por Tipo de Envase de una Ruta*/
    class TotalesEnvasesAP
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
