using Android.Bluetooth;
using Base;
using Java.IO;
using Java.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.Modelo;

namespace VentasPsion.VistaModelo
{
    public class ZeImpresionZebraMuestra
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ZeImpresionZebraMuestra()
        {

        }

        /*Método para realizar la Impresión de un Ticket de Pago en la Impresora configurada*/
        public void FtnImprimirTicketEntregaR(int iIdClienteParam, string sNoTicketParam)
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

                                        /*Variables para datos generales de un Cliente y Totales de Venta*/
                                        int iIdCliente = 0;
                                        int iIdRuta = 0;
                                        string sNoTicket = "";
                                        string sNombreCliente = "";
                                        string sRfc = "";
                                        string sDomicilio = "";
                                        double dSaldoAnterior = 0.00;
                                        double dImporteVenta = 0.00;
                                        double dMontoPago = 0.00;
                                        string sImpuesto = "";
                                        double sPagoPreventa = 0.00;

                                        /*Variables para datos de los Detalles de Entrega por Grupos de Productos de un Cliente*/
                                        int iCantidad = 0;
                                        string sIdProducto = "";
                                        string sDescripcion = "";
                                        double dPrecioBase = 0.00;
                                        double dDescuento = 0.00;
                                        double dPrecioFinal = 0.00;
                                        double dPrecio = 0.00;
                                        double dImporte = 0.00;
                                        string sIdTipo = "";
                                        string sClasificacion = "";
                                        string sOrdenTicket = "";
                                        //double dImporteTotal = 0.00;
                                        double dDescuentoTotal = 0.00;

                                        /*Variables para datos del Importe de los Envases comprados por un Cliente*/
                                        double importeEnvase = 0.00;

                                        /*Variables para datos de los Saldos de Envases de un Cliente*/
                                        string sIdEnvase = "";
                                        string sDescripCorta = "";
                                        int iSaldoInicial = 0;
                                        int iCargo = 0;
                                        int iAbono = 0;
                                        int iVenta = 0;
                                        int iSaldoFinal = 0;

                                        string sSql = "";

                                        List<CabeceraEntregaZe> listaCabeceraEntrega = null;
                                        List<DetallesEntregaZe> listaDetallesEntrega = null;
                                        List<ImporteEnvasesEntregaZe> listaImporteEnvasesEntrega = null;
                                        List<SaldosEnvasesEntregaZe> listaSaldosEnvasesEntrega = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene los datos generales de un Cliente y Totales de Venta*/
                                            sSql = "SELECT " +
                                                   "    DISTINCT cl.run_clave AS idRuta, " +
                                                   "    cl.cln_clave AS idCliente, " +
                                                   "	vc.vcn_folio AS noTicket, " +
                                                   "    cl.clc_nombre_comercial AS nombreComercial, " +
                                                   "    cl.clc_nombre AS nombreCliente, " +
                                                   "    cl.clc_rfc AS rfc, " +
                                                   "    cl.clc_domicilio AS domicilio, " +
                                                   "    vc.vcn_saldo_ant AS saldoAnterior, " +
                                                   "    vc.vcn_importe AS importeVenta, " +
                                                   "    vc.vcn_monto_pago AS montoPago, " +
                                                   "    cl.cld_impuesto_xml AS impuesto, " +
                                                   "    cl.clc_pago_diferencia_preventa AS pagoPreventa " +
                                                   "FROM venta_detalle AS vd " +
                                                   "INNER JOIN clientes AS cl ON (vd.vdn_cliente = cl.cln_clave) " +
                                                   "INNER JOIN venta_cabecera AS vc ON (vd.vdn_cliente = vc.vcn_cliente) " +
                                                   "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vc.vcn_folio = '" + sNoTicketParam + "')";

                                            listaCabeceraEntrega = conn.Query<CabeceraEntregaZe>(sSql);

                                            if (listaCabeceraEntrega.Count == 0)
                                            {
                                                utilerias.crearMensajeLargo("El Cliente NO tiene Entregas para generar el Ticket.");
                                                return;
                                            }

                                            foreach (var cabeceraEntrega in listaCabeceraEntrega)
                                            {
                                                iIdCliente = cabeceraEntrega.idCliente;
                                                iIdRuta = cabeceraEntrega.idRuta;
                                                sNoTicket = cabeceraEntrega.noTicket == null ? "" : cabeceraEntrega.noTicket.Trim();
                                                sNombreCliente = cabeceraEntrega.nombreCliente == null ? "" : cabeceraEntrega.nombreCliente.Trim();
                                                if (sNombreCliente.Trim() == "")
                                                    sNombreCliente = cabeceraEntrega.nombreComercial == null ? "" : cabeceraEntrega.nombreComercial.Trim();
                                                sRfc = cabeceraEntrega.rfc == null ? "" : cabeceraEntrega.rfc.Trim();
                                                sDomicilio = cabeceraEntrega.domicilio == null ? "" : cabeceraEntrega.domicilio.Trim();
                                                dSaldoAnterior = cabeceraEntrega.saldoAnterior;
                                                dImporteVenta = cabeceraEntrega.importeVenta;
                                                dMontoPago = cabeceraEntrega.montoPago;
                                                sImpuesto = cabeceraEntrega.impuesto;
                                                sPagoPreventa = cabeceraEntrega.pagoPreventa;
                                            }

                                            FtnImprimirTexto("                                                ");
                                            FtnImprimirTexto("                TICKET DE ENTREGA               ");
                                            FtnImprimirTexto("------------------------------------------------");   //48 caracteres
                                            //FtnImprimirTexto("                                                ");
                                            //FtnImprimirTexto((char)27 + "F" + (char)1 + (char)27 + "k4" + (char)28);
                                            FtnImprimirTexto(" CERVEZA CORONA EN AGUASCALIENTES S.A. DE C.V.  ");
                                            //FtnImprimirTexto((char)29 + "");
                                            FtnImprimirTexto("Reg. Fed. de Contribuyentes CCA-810221-N7A");
                                            FtnImprimirTexto("Direccion: Blvd. Jose Maria Chavez #2906");
                                            FtnImprimirTexto("Cd. Industrial, C.P. 20290, Ags., Ags.          ");
                                            //FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("Ruta Reparto: " + VarEntorno.iNoRuta.ToString().PadRight(34));
                                            FtnImprimirTexto("Ruta Preventa: " + iIdRuta.ToString().PadRight(33));
                                            FtnImprimirTexto("Ticket: " + sNoTicket.ToString().PadRight(40));
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().PadRight(41));
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().PadRight(42));
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("Cliente: " + (iIdCliente + " - " + sNombreCliente).PadRight(39).Substring(0, 39));
                                            FtnImprimirTexto("RFC: " + sRfc.PadRight(43));
                                            FtnImprimirTexto("Direccion: " + sDomicilio.PadRight(85));
                                            FtnImprimirTexto("------------------------------------------------");

                                            ///*Query que obtiene los datos de los Detalles de Entrega por Grupos de Productos de un Cliente*/
                                            //sSql = "SELECT " +
                                            //       "    SUM(COALESCE(vd.vdn_venta, 0)) AS cantidad, " +
                                            //       "	pr.arc_clave AS idProducto, " +
                                            //       "    pr.ard_descripcion AS descripcion, " +
                                            //       "    vd.lmn_preciolm AS precioBase, " +
                                            //       "    vd.lmn_totaldescuento AS descuento, " +
                                            //       "    vd.lmn_preciofinal AS precioFinal, " +
                                            //       "    vd.vdn_precio AS precio, " +
                                            //       "    SUM(COALESCE(vd.vdn_importe, 0.00)) AS importe, " +
                                            //       "    pr.arc_produ AS idTipo, " +
                                            //       "    CASE " +
                                            //       "        WHEN (pr.arc_produ = 'C') THEN 'CERVEZA' " +
                                            //       "        WHEN (pr.arc_produ = 'R') THEN 'REFRESCO' " +
                                            //       "        WHEN (pr.arc_produ = 'E') THEN 'ENVASE' " +
                                            //       "        WHEN (pr.arc_produ = 'A') THEN 'AGUA' " +
                                            //       "        WHEN (pr.arc_produ = 'V') THEN 'KERMATO' " +
                                            //       "    END AS clasificacion, " +
                                            //       "    CASE " +
                                            //       "        WHEN (pr.arc_produ = 'C') THEN '1' " +
                                            //       "        WHEN (pr.arc_produ = 'R') THEN '2' " +
                                            //       "        WHEN (pr.arc_produ = 'A') THEN '3' " +
                                            //       "        WHEN (pr.arc_produ = 'V') THEN '4' " +
                                            //       "        WHEN (pr.arc_produ = 'E') THEN '5' " +
                                            //       "    END AS ordenTicket " +
                                            //       "FROM venta_detalle AS vd " +
                                            //       "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                            //       "INNER JOIN venta_cabecera AS vc ON (vd.vdn_cliente = vc.vcn_cliente) " +
                                            //       "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                            //       "AND (vc.vcn_folio = '" + sNoTicketParam + "') " +
                                            //       "GROUP BY pr.arc_clave, pr.ard_descripcion, vd.vdn_precio, pr.arc_produ " +
                                            //       "UNION " +
                                            //       "SELECT " +
                                            //       "    SUM(COALESCE(vd.vdn_venta, 0)) AS cantidad, " +
                                            //       "    '' AS idProducto, " +
                                            //       "    CASE " +
                                            //       "        WHEN (pr.arc_produ = 'C') THEN 'TOTAL : CERVEZA' " +
                                            //       "        WHEN (pr.arc_produ = 'R') THEN 'TOTAL : REFRESCO' " +
                                            //       "        WHEN (pr.arc_produ = 'E') THEN 'TOTAL : ENVASE' " +
                                            //       "        WHEN (pr.arc_produ = 'A') THEN 'TOTAL : AGUA' " +
                                            //       "        WHEN (pr.arc_produ = 'V') THEN 'TOTAL : KERMATO' " +
                                            //       "    END AS descripcion, " +
                                            //       "    0.00 AS precioBase, " +
                                            //       "    0.00 AS descuento, " +
                                            //       "    0.00 AS precioFinal, " +
                                            //       "    0.00 AS precio, " +
                                            //       "    0.00 AS importe, " +
                                            //       "    '' AS idTipo, " +
                                            //       "    CASE " +
                                            //       "        WHEN (pr.arc_produ = 'C') THEN 'CERVEZA' " +
                                            //       "        WHEN (pr.arc_produ = 'R') THEN 'REFRESCO' " +
                                            //       "        WHEN (pr.arc_produ = 'E') THEN 'ENVASE' " +
                                            //       "        WHEN (pr.arc_produ = 'A') THEN 'AGUA' " +
                                            //       "        WHEN (pr.arc_produ = 'V') THEN 'KERMATO' " +
                                            //       "    END AS clasificacion, " +
                                            //       "    CASE " +
                                            //       "        WHEN (pr.arc_produ = 'C') THEN '1.1' " +
                                            //       "        WHEN (pr.arc_produ = 'R') THEN '2.1' " +
                                            //       "        WHEN (pr.arc_produ = 'A') THEN '3.1' " +
                                            //       "        WHEN (pr.arc_produ = 'V') THEN '4.1' " +
                                            //       "        WHEN (pr.arc_produ = 'E') THEN '5.1' " +
                                            //       "    END AS ordenTicket " +
                                            //       "FROM venta_detalle AS vd " +
                                            //       "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                            //       "INNER JOIN venta_cabecera AS vc ON (vd.vdn_cliente = vc.vcn_cliente) " +
                                            //       "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                            //       "AND (vc.vcn_folio = '" + sNoTicketParam + "') " +
                                            //       "GROUP BY pr.arc_produ " +
                                            //       "ORDER BY ordenTicket, idProducto";

                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");

                                            //if (sImpuesto == "I")
                                            //{
                                            //    //FtnImprimirTexto("ID     Producto");
                                            //    //FtnImprimirTexto("Cant   P.Base     Desc  P.Final    Importe");

                                            //    FtnImprimirTexto("Cant Producto              Precio  Importe");
                                            //}
                                            //else
                                            //{
                                            //    FtnImprimirTexto("Cant Producto              Precio  Importe");
                                            //}

                                            //FtnImprimirTexto("------------------------------------------");

                                            //listaDetallesEntrega = conn.Query<DetallesEntregaZe>(sSql);

                                            //foreach (var detalleEntrega in listaDetallesEntrega)
                                            //{
                                            //    iCantidad = detalleEntrega.cantidad;
                                            //    sIdProducto = detalleEntrega.idProducto == null ? "" : detalleEntrega.idProducto.Trim();
                                            //    sDescripcion = detalleEntrega.descripcion == null ? "" : detalleEntrega.descripcion.Trim();
                                            //    dPrecioBase = detalleEntrega.precioBase;
                                            //    dDescuento = detalleEntrega.descuento;
                                            //    dPrecioFinal = detalleEntrega.precioFinal;
                                            //    dPrecio = detalleEntrega.precio;
                                            //    dImporte = detalleEntrega.importe;
                                            //    sIdTipo = detalleEntrega.idTipo == null ? "" : detalleEntrega.idTipo.Trim();
                                            //    sClasificacion = detalleEntrega.clasificacion == null ? "" : detalleEntrega.clasificacion.Trim();
                                            //    sOrdenTicket = detalleEntrega.ordenTicket == null ? "" : detalleEntrega.ordenTicket.Trim();

                                            //    if (sDescripcion.PadRight(5).Substring(0, 5) == "TOTAL")
                                            //    {
                                            //        FtnImprimirTexto(" ");

                                            //        if (sImpuesto == "I")
                                            //        {
                                            //            //FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + "   " + sDescripcion.ToString().PadRight(21).Substring(0, 21));

                                            //            FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " + sDescripcion.ToString().PadRight(21).Substring(0, 21));
                                            //        }
                                            //        else
                                            //        {
                                            //            FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " + sDescripcion.ToString().PadRight(21).Substring(0, 21));
                                            //        }

                                            //        FtnImprimirTexto("------------------------------------------");
                                            //    }
                                            //    else
                                            //    {
                                            //        if (sImpuesto == "I")
                                            //        {
                                            //            //FtnImprimirTexto(sIdProducto.ToString().PadRight(4) + "   " +
                                            //            //                 sDescripcion.ToString().PadRight(35).Substring(0, 35));
                                            //            //FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " +
                                            //            //                 String.Format("{0:0.00}", dPrecioBase).PadLeft(8) + " " +
                                            //            //                 String.Format("{0:0.00}", dDescuento).PadLeft(8) + " " +
                                            //            //                 String.Format("{0:0.00}", dPrecioFinal).PadLeft(8) + " " +
                                            //            //                 String.Format("{0:0.00}", dImporte).PadLeft(10));

                                            //            FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " +
                                            //                            sDescripcion.ToString().PadRight(21).Substring(0, 21) + " " +
                                            //                            String.Format("{0:0.00}", dPrecioFinal).PadLeft(5) + " " +
                                            //                            String.Format("{0:0.00}", dImporte).PadLeft(5));

                                            //            dDescuentoTotal = dDescuentoTotal + (iCantidad * dDescuento);
                                            //        }
                                            //        else
                                            //        {
                                            //            FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " +
                                            //                            sDescripcion.ToString().PadRight(21).Substring(0, 21) + " " +
                                            //                            String.Format("{0:0.00}", dPrecio).PadLeft(5) + " " +
                                            //                            String.Format("{0:0.00}", dImporte).PadLeft(5));
                                            //        }

                                            //        //dImporteTotal += dImporte;
                                            //    }
                                            //}

                                            ///*Query que obtiene los datos del Importe de los Envases comprados por un Cliente*/
                                            //sSql = "SELECT " +
                                            //       "    SUM(COALESCE(vd.vdn_importe, 0.00)) AS importeEnvase " +
                                            //       "FROM venta_detalle AS vd " +
                                            //       "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                            //       "INNER JOIN venta_cabecera AS vc ON (vd.vdn_cliente = vc.vcn_cliente) " +
                                            //       "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                            //       "AND (vc.vcn_folio = '" + sNoTicketParam + "') " +
                                            //       "AND (pr.arc_produ = 'E')";

                                            //listaImporteEnvasesEntrega = conn.Query<ImporteEnvasesEntregaZe>(sSql);

                                            //foreach (var importeEnvaseEntrega in listaImporteEnvasesEntrega)
                                            //{
                                            //    importeEnvase = importeEnvaseEntrega.importeEnvase;
                                            //}

                                            //if (sImpuesto == "I")
                                            //{
                                            //    FtnImprimirTexto(" ");
                                            //    FtnImprimirTexto(" EL DESCUENTO VIENE INCLUIDO EN EL PRECIO");
                                            //}

                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("------------------------------------------");
                                            //FtnImprimirTexto("Saldo Anterior........: $ " + String.Format("{0:#,0.00}", dSaldoAnterior).PadLeft(15));
                                            //FtnImprimirTexto("Total Liquido.........: $ " + String.Format("{0:#,0.00}", (dImporteVenta - importeEnvase)).PadLeft(15));
                                            //FtnImprimirTexto("Total Envase..........: $ " + String.Format("{0:#,0.00}", importeEnvase).PadLeft(15));
                                            //FtnImprimirTexto("Total Venta...........: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            //FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", ((dSaldoAnterior + dImporteVenta) - sPagoPreventa)).PadLeft(15));
                                            //FtnImprimirTexto("Pago..................: $ " + String.Format("{0:#,0.00}", dMontoPago).PadLeft(15));
                                            //FtnImprimirTexto("Saldo Actual..........: $ " + String.Format("{0:#,0.00}", ((dSaldoAnterior + dImporteVenta) - (dMontoPago + sPagoPreventa))).PadLeft(15));
                                            //FtnImprimirTexto("------------------------------------------");
                                            ////FtnImprimirTexto("Total Descuento.......: $ " + String.Format("{0:#,0.00}", dDescuentoTotal).PadLeft(15));
                                            ////FtnImprimirTexto("------------------------------------------");

                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("Su Pago: $" + String.Format("{0:#,0.00}", dMontoPago).PadRight(15));
                                            //FtnImprimirTexto("Gracias!!!");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("        FAVOR DE REVISAR SU PRODUCTO");

                                            ///*Query que obtiene los datos de los Saldos de Envases de un Cliente*/
                                            //sSql = "SELECT " +
                                            //        "   en.cln_clave AS idCliente, " +
                                            //        "	en.mec_envase AS idEnvase, " +
                                            //        "   pr.ard_corta AS descripCorta, " +
                                            //        "   en.men_saldo_inicial AS saldoInicial, " +
                                            //        "   en.men_cargo AS cargo, " +
                                            //        "   en.men_abono AS abono, " +
                                            //        "   en.men_venta AS venta, " +
                                            //        "   en.men_saldo_final AS saldoFinal " +
                                            //        "FROM envase en " +
                                            //        "JOIN productos pr ON (en.mec_envase = pr.arc_clave) " +
                                            //        "WHERE (en.cln_clave = " + iIdClienteParam + ") " +
                                            //        "AND ((en.men_saldo_inicial <> 0) OR (en.men_cargo <> 0) OR (en.men_abono <> 0) OR (en.men_venta <> 0) OR (en.men_saldo_final <> 0)) " +
                                            //        "ORDER BY en.mec_envase";

                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("------------------------------------------");
                                            //FtnImprimirTexto("           MOVIMIENTO DE ENVASES");
                                            //FtnImprimirTexto("   Envase      Ini   Car   Abo   Vta   Fin");

                                            //listaSaldosEnvasesEntrega = conn.Query<SaldosEnvasesEntregaZe>(sSql);

                                            //foreach (var saldoEnvaseEntrega in listaSaldosEnvasesEntrega)
                                            //{
                                            //    sIdEnvase = saldoEnvaseEntrega.idEnvase == null ? "" : saldoEnvaseEntrega.idEnvase.Trim();
                                            //    sDescripCorta = saldoEnvaseEntrega.descripCorta == null ? "" : saldoEnvaseEntrega.descripCorta.Trim();
                                            //    iSaldoInicial = saldoEnvaseEntrega.saldoInicial;
                                            //    iCargo = saldoEnvaseEntrega.cargo;
                                            //    iAbono = saldoEnvaseEntrega.abono;
                                            //    iVenta = saldoEnvaseEntrega.venta;
                                            //    iSaldoFinal = (((iSaldoInicial + iCargo) - iAbono) - iVenta);
                                            //    //iSaldoFinal = saldoEnvaseEntrega.saldoFinal;

                                            //    FtnImprimirTexto(sIdEnvase.PadRight(2).Substring(0, 2) + " " +
                                            //                     sDescripCorta.PadRight(10).Substring(0, 10) +
                                            //                     iSaldoInicial.ToString().PadLeft(5) + " " +
                                            //                     iCargo.ToString().PadLeft(5) + " " +
                                            //                     iAbono.ToString().PadLeft(5) + " " +
                                            //                     iVenta.ToString().PadLeft(5) + " " +
                                            //                     iSaldoFinal.ToString().PadLeft(5));
                                            //}

                                            //FtnImprimirTexto("------------------------------------------");
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
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Entrega.\nDetalle: " + exc.Message);
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
            //Task.Delay(1000);
            //Thread.Sleep(250);
            //bufWriter.Write(sTextoPorImprimir + "\n");
            //bufWriter.Flush();

            Thread.Sleep(500);
            bufWriter.Write(sTextoPorImprimir);
            bufWriter.Flush();
            Thread.Sleep(500);
            bufWriter.Write("\n");
            bufWriter.Flush();

            //Thread.Sleep(500);
        }
    }

    /*Clase para guardar los datos generales de un Cliente y Totales de Venta*/
    class CabeceraEntregaZe
    {
        public int idCliente { get; set; }
        public int idRuta { get; set; }
        public string noTicket { get; set; }
        public string nombreComercial { get; set; }
        public string nombreCliente { get; set; }
        public string rfc { get; set; }
        public string domicilio { get; set; }
        public double saldoAnterior { get; set; }
        public double importeVenta { get; set; }
        public double montoPago { get; set; }
        public string impuesto { get; set; }
        public double pagoPreventa { get; set; }
    }

    /*Clase para guardar los datos de los Detalles de Entrega por Grupos de Productos de un Cliente*/
    class DetallesEntregaZe
    {
        public int cantidad { get; set; }
        public string idProducto { get; set; }
        public string descripcion { get; set; }
        public double precioBase { get; set; }
        public double descuento { get; set; }
        public double precioFinal { get; set; }
        public double precio { get; set; }
        public double importe { get; set; }
        public string idTipo { get; set; }
        public string clasificacion { get; set; }
        public string ordenTicket { get; set; }
    }

    /*Clase para guardar los datos del Importe de los Envases comprados por un Cliente*/
    class ImporteEnvasesEntregaZe
    {
        public double importeEnvase { get; set; }
    }

    /*Clase para guardar los datos de los Saldos de Envases de un Cliente*/
    class SaldosEnvasesEntregaZe
    {
        public int idCliente { get; set; }
        public string idEnvase { get; set; }
        public string descripCorta { get; set; }
        public int saldoInicial { get; set; }
        public int cargo { get; set; }
        public int abono { get; set; }
        public int venta { get; set; }
        public int saldoFinal { get; set; }
    }
}