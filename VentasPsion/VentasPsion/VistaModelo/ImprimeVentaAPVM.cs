using Android.Bluetooth;
using Base;
using Java.IO;
using Java.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class ImprimeVentaAPVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ImprimeVentaAPVM()
        {

        }

        /*Método para realizar la Impresión de un Ticket de Venta en la Impresora configurada*/
        public async void FtnImprimirTicketVentaAP(int iIdClienteParam, string sNoTicketParam)
        {
            try
            {
                utilerias = new Utilerias();
                venta_pagosSR pagosSR = new venta_pagosSR();
                List<venta_pagos> pagos = new List<venta_pagos>();

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

                                        /*Variables para datos generales de un Cliente y Totales de un Ticket*/
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

                                        /*Variables para datos de los Detalles de Venta de un Cliente*/
                                        int iCantidad = 0;
                                        string sIdProducto = "";
                                        string sDescripcion = "";
                                        double dPrecioBase = 0.00;
                                        double dPorcentajeDescuento = 0.00;
                                        double dImporteDescuento = 0.00;
                                        double dPrecioFinal = 0.00;
                                        double dPrecio = 0.00;
                                        double dImporte = 0.00;
                                        string sIdTipo = "";
                                        string sClasificacion = "";
                                        string sOrdenTicket = "";
                                        //double dImporteTotal = 0.00;
                                        double dDescuentoTotal = 0.00;

                                        /*Variables para datos del Importe de los Envases comprados por un Cliente*/
                                        int idClienteEnvase = 0;
                                        string noTicketEnvase = "";
                                        double importeEnvase = 0.00;

                                        /*Variables para datos de los Saldos de Envases de un Cliente*/
                                        string sIdEnvase = "";
                                        string sDescripCorta = "";
                                        int iSaldoInicial = 0;
                                        int iCargo = 0;
                                        int iAbono = 0;
                                        int iVenta = 0;
                                        int iSaldoFinal = 0;

                                        /*Variable para guardar el Total en Efectivo de un Cliente*/
                                        double dTotalEfectivo = 0.00;

                                        /*Variable para guardar el Total en Transferencias de un Cliente*/
                                        double dTotalTransferencia = 0.00;

                                        /*Variables para guardar los datos de los Cheques de un Cliente*/
                                        string sBancoCheque = "";
                                        string sNoCheque = "";
                                        double dImporteCheque = 0.00;

                                        /*Variables para guardar los datos de las Tarjetas de un Cliente*/
                                        string sNoTarjeta = "";
                                        double dImporteTarjeta = 0.00;

                                        /*Variables para guardar los datos de las Bonificaciones de un Cliente*/
                                        int idCliente = 0;
                                        string noTicket = "";
                                        string folioBonific = "";
                                        string tipoBonific = "";
                                        double importeBonific = 0.00;

                                        string sSql = "";

                                        List<CabeceraVenta> listaCabeceraVenta = null;
                                        List<DetallesVenta> listaDetallesVenta = null;
                                        List<ImporteEnvases> listaImporteEnvases = null;
                                        List<EfectivoVenta> listaEfectivoVentas = null;
                                        List<TransferenciaVenta> listaTransferenciaVentas = null;
                                        List<ChequeVenta> listaChequesVentas = null;
                                        List<TarjetaVenta> listaTarjetasVentas = null;
                                        List<BonificacionVenta> listaBonificacionesVentas = null;
                                        List<SaldosEnvases> listaSaldosEnvases = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene los datos generales de un Cliente y Totales de un Ticket*/
                                            sSql = "SELECT " +
                                                    "    DISTINCT cl.run_clave AS idRuta, " +
                                                    "    cl.cln_clave AS idCliente, " +
                                                    "	 vd.vdn_folio AS noTicket, " +
                                                    "    cl.clc_nombre_comercial AS nombreComercial, " +
                                                    "    cl.clc_nombre AS nombreCliente, " +
                                                    "    cl.clc_rfc AS rfc, " +
                                                    "    cl.clc_domicilio AS domicilio, " +
                                                    "    vc.vcn_saldo_ant AS saldoAnterior, " +
                                                    "    vc.vcn_importe AS importeVenta, " +
                                                    "    vc.vcn_monto_pago AS montoPago, " +
                                                    "    cl.cld_impuesto_xml AS impuesto " +
                                                    "FROM venta_detalle AS vd " +
                                                    "INNER JOIN clientes AS cl ON (vd.vdn_cliente = cl.cln_clave) " +
                                                    "INNER JOIN venta_cabecera AS vc ON ((vd.vdn_cliente = vc.vcn_cliente) AND (vd.vdn_folio = vc.vcn_folio)) " +
                                                    "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                    "AND (vd.vdn_folio = '" + sNoTicketParam + "')";

                                            listaCabeceraVenta = conn.Query<CabeceraVenta>(sSql);

                                            if (listaCabeceraVenta.Count == 0)
                                            {
                                                utilerias.crearMensajeLargo("El Cliente NO tiene Ventas para generar el Ticket.");
                                                return;
                                            }

                                            foreach (var cabeceraVenta in listaCabeceraVenta)
                                            {
                                                iIdCliente = cabeceraVenta.idCliente;
                                                iIdRuta = cabeceraVenta.idRuta;
                                                sNoTicket = cabeceraVenta.noTicket == null ? "" : cabeceraVenta.noTicket.Trim();
                                                sNombreCliente = cabeceraVenta.nombreCliente == null ? "" : cabeceraVenta.nombreCliente.Trim();
                                                if (sNombreCliente.Trim() == "")
                                                    sNombreCliente = cabeceraVenta.nombreComercial == null ? "" : cabeceraVenta.nombreComercial.Trim();
                                                sRfc = cabeceraVenta.rfc == null ? "" : cabeceraVenta.rfc.Trim();
                                                sDomicilio = cabeceraVenta.domicilio == null ? "" : cabeceraVenta.domicilio.Trim();
                                                dSaldoAnterior = cabeceraVenta.saldoAnterior;
                                                dImporteVenta = cabeceraVenta.importeVenta;
                                                dMontoPago = cabeceraVenta.montoPago;
                                                sImpuesto = cabeceraVenta.impuesto;
                                            }

                                            FtnImprimirTexto("             TICKET DE VENTA");
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
                                            FtnImprimirTexto("Ticket: " + sNoTicket);
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().Trim());
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().Trim());
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Cliente: " + iIdCliente + " - " + sNombreCliente);
                                            FtnImprimirTexto("RFC: " + sRfc);
                                            FtnImprimirTexto("Direccion: " + sDomicilio);
                                            FtnImprimirTexto("------------------------------------------");

                                            /*Query que obtiene los datos de los Detalles de Venta por Grupos de Productos de un Cliente*/
                                            sSql = "SELECT " +
                                                   "    vd.vdn_venta AS cantidad, " +
                                                   "	pr.arc_clave AS idProducto, " +
                                                   "    pr.ard_descripcion AS descripcion, " +
                                                   "    vd.lmn_preciolm AS precioBase, " +
                                                   "    vd.lmc_porcentaje AS porcentajeDescuento, " +
                                                   "    vd.lmn_totaldescuento AS importeDescuento, " +
                                                   "    vd.lmn_preciofinal AS precioFinal, " +
                                                   "    vd.vdn_precio AS precio, " +
                                                   "    vd.vdn_importe AS importe, " +
                                                   "    pr.arc_produ AS idTipo, " +
                                                   "    CASE " +
                                                   "        WHEN (pr.arc_produ = 'C') THEN 'CERVEZA' " +
                                                   "        WHEN (pr.arc_produ = 'R') THEN 'REFRESCO' " +
                                                   "        WHEN (pr.arc_produ = 'E') THEN 'ENVASE' " +
                                                   "        WHEN (pr.arc_produ = 'A') THEN 'AGUA' " +
                                                   "        WHEN (pr.arc_produ = 'V') THEN 'KERMATO' " +
                                                   "    END AS clasificacion, " +
                                                   "    CASE " +
                                                   "        WHEN (pr.arc_produ = 'C') THEN '1' " +
                                                   "        WHEN (pr.arc_produ = 'R') THEN '2' " +
                                                   "        WHEN (pr.arc_produ = 'A') THEN '3' " +
                                                   "        WHEN (pr.arc_produ = 'V') THEN '4' " +
                                                   "        WHEN (pr.arc_produ = 'E') THEN '5' " +
                                                   "    END AS ordenTicket " +
                                                   "FROM venta_detalle AS vd " +
                                                   "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                                   "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vd.vdn_folio = '" + sNoTicketParam + "') " +
                                                   "UNION " +
                                                   "SELECT " +
                                                   "    SUM(COALESCE(vd.vdn_venta, 0)) AS cantidad, " +
                                                   "    '' AS idProducto, " +
                                                   "    CASE " +
                                                   "        WHEN (pr.arc_produ = 'C') THEN 'TOTAL : CERVEZA' " +
                                                   "        WHEN (pr.arc_produ = 'R') THEN 'TOTAL : REFRESCO' " +
                                                   "        WHEN (pr.arc_produ = 'E') THEN 'TOTAL : ENVASE' " +
                                                   "        WHEN (pr.arc_produ = 'A') THEN 'TOTAL : AGUA' " +
                                                   "        WHEN (pr.arc_produ = 'V') THEN 'TOTAL : KERMATO' " +
                                                   "    END AS descripcion, " +
                                                   "    0.00 AS precioBase, " +
                                                   "    0.00 AS porcentajeDescuento, " +
                                                   "    0.00 AS importeDescuento, " +
                                                   "    0.00 AS precioFinal, " +
                                                   "    0.00 AS precio, " +
                                                   "    0.00 AS importe, " +
                                                   "    '' AS idTipo, " +
                                                   "    CASE " +
                                                   "        WHEN (pr.arc_produ = 'C') THEN 'CERVEZA' " +
                                                   "        WHEN (pr.arc_produ = 'R') THEN 'REFRESCO' " +
                                                   "        WHEN (pr.arc_produ = 'E') THEN 'ENVASE' " +
                                                   "        WHEN (pr.arc_produ = 'A') THEN 'AGUA' " +
                                                   "        WHEN (pr.arc_produ = 'V') THEN 'KERMATO' " +
                                                   "    END AS clasificacion, " +
                                                   "    CASE " +
                                                   "        WHEN (pr.arc_produ = 'C') THEN '1.1' " +
                                                   "        WHEN (pr.arc_produ = 'R') THEN '2.1' " +
                                                   "        WHEN (pr.arc_produ = 'A') THEN '3.1' " +
                                                   "        WHEN (pr.arc_produ = 'V') THEN '4.1' " +
                                                   "        WHEN (pr.arc_produ = 'E') THEN '5.1' " +
                                                   "    END AS ordenTicket " +
                                                   "FROM venta_detalle AS vd " +
                                                   "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                                   "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vd.vdn_folio = '" + sNoTicketParam + "') " +
                                                   "GROUP BY pr.arc_produ " +
                                                   "ORDER BY ordenTicket, idProducto";

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");

                                            if (sImpuesto == "I")
                                            {
                                                /*
                                                FtnImprimirTexto("ID     Producto");
                                                FtnImprimirTexto("----   -----------------------------------");
                                                FtnImprimirTexto("Cant   P.Base     Desc  P.Final    Importe");
                                                FtnImprimirTexto("9999 9,999.99 9,999.99 9,999.99 999,999.99");
                                                FtnImprimirTexto("---- -------- -------- -------- ----------");
                                                FtnImprimirTexto("9999   TOTAL  :  CERVEZA                  ");
                                                FtnImprimirTexto("----   -----------------------------------");
                                                */
                                                //FtnImprimirTexto("ID     Producto");
                                                //FtnImprimirTexto("Cant   P.Base     Desc  P.Final    Importe");

                                                /*
                                                FtnImprimirTexto("ID     Producto");
                                                FtnImprimirTexto("-----  -----------------------------------");
                                                FtnImprimirTexto("Cant.  Precio Base   % Desc.       Importe");
                                                FtnImprimirTexto("9,999  ,999,999.99  9,999.99  9,999,999.99");
                                                FtnImprimirTexto("-----  -----------  --------  ------------");
                                                FtnImprimirTexto("9,999  TOTAL  :  CERVEZA                  ");
                                                FtnImprimirTexto("-----  -----------------------------------");
                                                */
                                                FtnImprimirTexto("ID     Producto");
                                                FtnImprimirTexto("Cant.  Precio Base   % Desc.       Importe");

                                                /*
                                                FtnImprimirTexto("Cant Producto            Precio    Importe");
                                                FtnImprimirTexto("9999 XXXXXXXXXXXXXXXXX 9,999.99 999,999.99");
                                                FtnImprimirTexto("---- ----------------- -------- ----------");
                                                FtnImprimirTexto("9999 TOTAL  :  CERVEZA                    ");
                                                FtnImprimirTexto("---- -------------------------------------");
                                                */
                                                //FtnImprimirTexto("Cant Producto            Precio    Importe");
                                            }
                                            else
                                            {
                                                /*
                                                FtnImprimirTexto("Cant Producto            Precio    Importe");
                                                FtnImprimirTexto("9999 XXXXXXXXXXXXXXXXX 9,999.99 999,999.99");
                                                FtnImprimirTexto("---- ----------------- -------- ----------");
                                                FtnImprimirTexto("9999 TOTAL  :  CERVEZA                    ");
                                                FtnImprimirTexto("---- -------------------------------------");
                                                */
                                                FtnImprimirTexto("Cant Producto            Precio    Importe");
                                            }

                                            FtnImprimirTexto("------------------------------------------");

                                            listaDetallesVenta = conn.Query<DetallesVenta>(sSql);

                                            foreach (var detalleVenta in listaDetallesVenta)
                                            {
                                                iCantidad = detalleVenta.cantidad;
                                                sIdProducto = detalleVenta.idProducto == null ? "" : detalleVenta.idProducto.Trim();
                                                sDescripcion = detalleVenta.descripcion == null ? "" : detalleVenta.descripcion.Trim();
                                                dPrecioBase = detalleVenta.precioBase;
                                                dPorcentajeDescuento = detalleVenta.porcentajeDescuento;
                                                dImporteDescuento = detalleVenta.importeDescuento;
                                                dPrecioFinal = detalleVenta.precioFinal;
                                                dPrecio = detalleVenta.precio;
                                                dImporte = detalleVenta.importe;
                                                sIdTipo = detalleVenta.idTipo == null ? "" : detalleVenta.idTipo.Trim();
                                                sClasificacion = detalleVenta.clasificacion == null ? "" : detalleVenta.clasificacion.Trim();
                                                sOrdenTicket = detalleVenta.ordenTicket == null ? "" : detalleVenta.ordenTicket.Trim();

                                                if (sDescripcion.PadRight(5).Substring(0, 5) == "TOTAL")
                                                {
                                                    FtnImprimirTexto(" ");

                                                    if (sImpuesto == "I")
                                                    {
                                                        /*
                                                        FtnImprimirTexto("ID     Producto");
                                                        FtnImprimirTexto("----   -----------------------------------");
                                                        FtnImprimirTexto("Cant   P.Base     Desc  P.Final    Importe");
                                                        FtnImprimirTexto("9999 9,999.99 9,999.99 9,999.99 999,999.99");
                                                        FtnImprimirTexto("---- -------- -------- -------- ----------");
                                                        FtnImprimirTexto("9999   TOTAL  :  CERVEZA                  ");
                                                        FtnImprimirTexto("----   -----------------------------------");
                                                        */
                                                        //FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + "   " + sDescripcion.ToString().PadRight(35).Substring(0, 35));

                                                        /*
                                                        FtnImprimirTexto("Cant.  Precio Base   % Desc.       Importe");
                                                        FtnImprimirTexto("9,999  ,999,999.99  9,999.99  9,999,999.99");
                                                        FtnImprimirTexto("-----  -----------  --------  ------------");
                                                        FtnImprimirTexto("9,999  TOTAL  :  CERVEZA                  ");
                                                        FtnImprimirTexto("-----  -----------------------------------");
                                                        */
                                                        FtnImprimirTexto(String.Format("{0:0}", iCantidad).PadLeft(5) + "  " + sDescripcion.ToString().PadRight(35).Substring(0, 35));

                                                        /*
                                                        FtnImprimirTexto("Cant Producto            Precio    Importe");
                                                        FtnImprimirTexto("9999 XXXXXXXXXXXXXXXXX 9,999.99 999,999.99");
                                                        FtnImprimirTexto("---- ----------------- -------- ----------");
                                                        FtnImprimirTexto("9999 TOTAL  :  CERVEZA                    ");
                                                        FtnImprimirTexto("---- -------------------------------------");
                                                        */
                                                        //FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " + sDescripcion.ToString().PadRight(37).Substring(0, 37));
                                                    }
                                                    else
                                                    {
                                                        /*
                                                        FtnImprimirTexto("Cant Producto            Precio    Importe");
                                                        FtnImprimirTexto("9999 XXXXXXXXXXXXXXXXX 9,999.99 999,999.99");
                                                        FtnImprimirTexto("---- ----------------- -------- ----------");
                                                        FtnImprimirTexto("9999 TOTAL  :  CERVEZA                    ");
                                                        FtnImprimirTexto("---- -------------------------------------");
                                                        */
                                                        FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " + sDescripcion.ToString().PadRight(37).Substring(0, 37));
                                                    }

                                                    FtnImprimirTexto("------------------------------------------");
                                                }
                                                else
                                                {
                                                    if (sImpuesto == "I")
                                                    {
                                                        /*
                                                        FtnImprimirTexto("ID     Producto");
                                                        FtnImprimirTexto("----   -----------------------------------");
                                                        FtnImprimirTexto("Cant   P.Base     Desc  P.Final    Importe");
                                                        FtnImprimirTexto("9999 9,999.99 9,999.99 9,999.99 999,999.99");
                                                        FtnImprimirTexto("---- -------- -------- -------- ----------");
                                                        */
                                                        //FtnImprimirTexto(sIdProducto.ToString().PadRight(4) + "   " +
                                                        //                 sDescripcion.ToString().PadRight(35).Substring(0, 35));
                                                        //FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " +
                                                        //                 String.Format("{0:0.00}", dPrecioBase).PadLeft(8) + " " +
                                                        //                 String.Format("{0:0.00}", dImporteDescuento).PadLeft(8) + " " +
                                                        //                 String.Format("{0:0.00}", dPrecioFinal).PadLeft(8) + " " +
                                                        //                 String.Format("{0:0.00}", dImporte).PadLeft(10));

                                                        /*
                                                        FtnImprimirTexto("ID     Producto");
                                                        FtnImprimirTexto("-----  -----------------------------------");
                                                        FtnImprimirTexto("Cant.  Precio Base   % Desc.       Importe");
                                                        FtnImprimirTexto("9,999  ,999,999.99  9,999.99  9,999,999.99");
                                                        FtnImprimirTexto("-----  -----------  --------  ------------");
                                                        FtnImprimirTexto("9,999  TOTAL  :  CERVEZA                  ");
                                                        FtnImprimirTexto("-----  -----------------------------------");
                                                        */
                                                        FtnImprimirTexto(sIdProducto.ToString().PadRight(5) + "  " +
                                                                         sDescripcion.ToString().PadRight(35).Substring(0, 35));
                                                        FtnImprimirTexto(String.Format("{0:0}", iCantidad).PadLeft(5) + "  " +
                                                                         String.Format("{0:0.00}", dPrecioBase).PadLeft(11) + "  " +
                                                                         String.Format("{0:0.00}", dPorcentajeDescuento).PadLeft(8) + "  " +
                                                                         String.Format("{0:0.00}", dImporte).PadLeft(12));

                                                        /*
                                                        FtnImprimirTexto("Cant Producto            Precio    Importe");
                                                        FtnImprimirTexto("9999 XXXXXXXXXXXXXXXXX 9,999.99 999,999.99");
                                                        FtnImprimirTexto("---- ----------------- -------- ----------");
                                                        FtnImprimirTexto("9999 TOTAL  :  CERVEZA                    ");
                                                        FtnImprimirTexto("---- -------------------------------------");
                                                        */
                                                        //FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " +
                                                        //                sDescripcion.ToString().PadRight(17).Substring(0, 17) + " " +
                                                        //                String.Format("{0:0.00}", dPrecioFinal).PadLeft(8) + " " +
                                                        //                String.Format("{0:0.00}", dImporte).PadLeft(10));

                                                        dDescuentoTotal = dDescuentoTotal + (iCantidad * dImporteDescuento);
                                                    }
                                                    else
                                                    {
                                                        /*
                                                        FtnImprimirTexto("Cant Producto            Precio    Importe");
                                                        FtnImprimirTexto("9999 XXXXXXXXXXXXXXXXX 9,999.99 999,999.99");
                                                        FtnImprimirTexto("---- ----------------- -------- ----------");
                                                        FtnImprimirTexto("9999 TOTAL  :  CERVEZA                    ");
                                                        FtnImprimirTexto("---- -------------------------------------");
                                                        */
                                                        FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " +
                                                                        sDescripcion.ToString().PadRight(17).Substring(0, 17) + " " +
                                                                        String.Format("{0:0.00}", dPrecioFinal).PadLeft(8) + " " +
                                                                        String.Format("{0:0.00}", dImporte).PadLeft(10));
                                                    }

                                                    //dImporteTotal += dImporte;
                                                }
                                            }

                                            /*Query que obtiene los datos del Importe de los Envases comprados por un Cliente*/
                                            sSql = "SELECT " +
                                                   "	vd.vdn_cliente AS idClienteEnvase, " +
                                                   "    vd.vdn_folio AS noTicketEnvase, " +
                                                   "    SUM(COALESCE(vd.vdn_importe, 0.00)) AS importeEnvase " +
                                                   "FROM venta_detalle AS vd " +
                                                   "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                                   "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vd.vdn_folio = '" + sNoTicketParam + "') " +
                                                   "AND (pr.arc_produ = 'E')";

                                            listaImporteEnvases = conn.Query<ImporteEnvases>(sSql);

                                            foreach (var importeEnvases in listaImporteEnvases)
                                            {
                                                idClienteEnvase = importeEnvases.idClienteEnvase;
                                                noTicketEnvase = importeEnvases.noTicketEnvase == null ? "" : importeEnvases.noTicketEnvase.Trim();
                                                importeEnvase = importeEnvases.importeEnvase;
                                            }

                                            //if (sImpuesto == "I")
                                            //{
                                            //    FtnImprimirTexto(" ");
                                            //    FtnImprimirTexto(" EL DESCUENTO VIENE INCLUIDO EN EL PRECIO");
                                            //}

                                            clientesSR oBuscarCliente = new clientesSR();
                                            var oCliente = await oBuscarCliente.DatosCliente(iIdClienteParam.ToString().Trim());
                                            double dSaldoFinal = Convert.ToDouble(new fnVentaCabecera().SaldoFinal(oCliente, Convert.ToInt32(sNoTicket)));

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Saldo Anterior........: $ " + String.Format("{0:#,0.00}", ((dSaldoFinal + dMontoPago) - dImporteVenta)).PadLeft(15));
                                            FtnImprimirTexto("Total Liquido.........: $ " + String.Format("{0:#,0.00}", (dImporteVenta - importeEnvase)).PadLeft(15));
                                            FtnImprimirTexto("Total Envase..........: $ " + String.Format("{0:#,0.00}", importeEnvase).PadLeft(15));
                                            FtnImprimirTexto("Total Venta...........: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", (dSaldoFinal + dMontoPago)).PadLeft(15));
                                            FtnImprimirTexto("Pago..................: $ " + String.Format("{0:#,0.00}", dMontoPago).PadLeft(15));
                                            FtnImprimirTexto("Saldo Actual..........: $ " + String.Format("{0:#,0.00}", dSaldoFinal).PadLeft(15));
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Total Descuento.......: $ " + String.Format("{0:#,0.00}", dDescuentoTotal).PadLeft(15));
                                            FtnImprimirTexto("------------------------------------------");

                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("------------------------------------------");
                                            //FtnImprimirTexto("Saldo Anterior........: $ " + String.Format("{0:#,0.00}", dSaldoAnterior).PadLeft(15));
                                            //FtnImprimirTexto("Total Liquido.........: $ " + String.Format("{0:#,0.00}", (dImporteVenta - importeEnvase)).PadLeft(15));
                                            //FtnImprimirTexto("Total Envase..........: $ " + String.Format("{0:#,0.00}", importeEnvase).PadLeft(15));
                                            //FtnImprimirTexto("Total Venta...........: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            //FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", (dSaldoAnterior + dImporteVenta)).PadLeft(15));
                                            //FtnImprimirTexto("Pago..................: $ " + String.Format("{0:#,0.00}", dMontoPago).PadLeft(15));
                                            //FtnImprimirTexto("Saldo Actual..........: $ " + String.Format("{0:#,0.00}", ((dSaldoAnterior + dImporteVenta) - dMontoPago)).PadLeft(15));
                                            //FtnImprimirTexto("------------------------------------------");
                                            //FtnImprimirTexto("Total Descuento.......: $ " + String.Format("{0:#,0.00}", dDescuentoTotal).PadLeft(15));
                                            //FtnImprimirTexto("------------------------------------------");

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Su Pago: $" + String.Format("{0:#,0.00}", dMontoPago).PadRight(15));
                                            FtnImprimirTexto("Gracias!!!");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("        FAVOR DE REVISAR SU PRODUCTO");

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("             DESGLOSE DE PAGO");

                                            /*Query que obtiene el Total en Efectivo de un Cliente*/
                                            /*
                                            sSql = "SELECT " +
                                                   "    COALESCE(vcn_monto_efe, 0.00) AS totalEfectivo " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "') " +
                                                   "AND (vcn_monto_efe <> 0.00) " +
                                                   "AND (efec_transf = 'EFECTIVO')";

                                            listaEfectivoVentas = conn.Query<EfectivoVenta>(sSql);
                                            */
                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "EFECTIVO");

                                            if (pagos.Count > 0)
                                            //if (listaEfectivoVentas.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("                 EFECTIVO");

                                                //foreach (var efectivoVenta in listaEfectivoVentas)
                                                foreach (var efectivo in pagos)
                                                {
                                                    //dTotalEfectivo = efectivoVenta.totalEfectivo;
                                                    dTotalEfectivo = Convert.ToDouble(efectivo.vcn_monto);

                                                    FtnImprimirTexto("Importe                    $" + String.Format("{0:#,0.00}", dTotalEfectivo).PadLeft(14));
                                                }

                                                //FtnImprimirTexto("------------------------------------------");
                                                //FtnImprimirTexto(" ");
                                            }

                                            /*Query que obtiene el Total en Transferencia de un Cliente*/
                                            /*
                                            sSql = "SELECT " +
                                                   "    COALESCE(vcn_monto_efe, 0.00) AS totalTransferencia " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "') " +
                                                   "AND (vcn_monto_efe <> 0.00) " +
                                                   "AND (efec_transf = 'TRANSFERENCIA')";

                                            listaTransferenciaVentas = conn.Query<TransferenciaVenta>(sSql);
                                            */
                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "TRANSFERENCIA");

                                            if (pagos.Count > 0)
                                            //if (listaTransferenciaVentas.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("              TRANSFERENCIA");

                                                //foreach (var transferenciaVenta in listaTransferenciaVentas)
                                                foreach (var transferencia in pagos)
                                                {
                                                    //dTotalTransferencia = transferenciaVenta.totalTransferencia;
                                                    dTotalTransferencia = Convert.ToDouble(transferencia.vcn_monto);

                                                    FtnImprimirTexto("Importe                    $" + String.Format("{0:#,0.00}", dTotalTransferencia).PadLeft(14));
                                                }

                                                //FtnImprimirTexto("------------------------------------------");
                                                //FtnImprimirTexto(" ");
                                            }

                                            /*Query que obtiene los datos de los Cheques de un Cliente*/
                                            /*
                                            sSql = "SELECT " +
                                                   "    vcc_banco AS bancoCheque, " +
                                                   "    vcf_num_cheque AS noCheque, " +
                                                   "    COALESCE(vcn_monto_cheque, 0.00) AS importeCheque " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcc_banco <> 'TARJETA') " +
                                                   "AND (vcn_monto_cheque <> 0.00) " +
                                                   "AND (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "') " +
                                                   "UNION " +
                                                   "SELECT " +
                                                   "    vcc_banco2 AS bancoCheque,	" +
                                                   "	vcf_num_cheque2 AS noCheque, " +
                                                   "    COALESCE(vcn_monto_cheque2, 0.00) AS importeCheque " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcc_banco2 <> 'TARJETA') " +
                                                   "AND (vcn_monto_cheque2 <> 0.00) " +
                                                   "AND (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "') " +
                                                   "UNION " +
                                                   "SELECT " +
                                                   "    vcc_banco3 AS bancoCheque, " +
                                                   "    vcf_num_cheque3 AS noCheque, " +
                                                   "    COALESCE(vcn_monto_cheque3, 0.00) AS importeCheque " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcc_banco3 <> 'TARJETA') " +
                                                   "AND (vcn_monto_cheque3 <> 0.00) " +
                                                   "AND (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "')";
                                                   
                                            listaChequesVentas = conn.Query<ChequeVenta>(sSql);
                                            */
                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "CHEQUE");

                                            if (pagos.Count > 0)
                                            //if (listaChequesVentas.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("                  CHEQUES");
                                                FtnImprimirTexto("Banco        No. Cheque            Importe");
                                                //FtnImprimirTexto("-----------  ------------  $000,000,000.00");

                                                //foreach (var chequeVenta in listaChequesVentas)
                                                foreach (var cheque in pagos)
                                                {
                                                    /*
                                                    sBancoCheque = chequeVenta.bancoCheque == null ? "" : chequeVenta.bancoCheque.Trim();
                                                    sNoCheque = chequeVenta.noCheque == null ? "" : chequeVenta.noCheque.Trim();
                                                    dImporteCheque = chequeVenta.importeCheque;
                                                    */
                                                    sBancoCheque = cheque.vcc_banco;
                                                    sNoCheque = cheque.vpc_nocuenta;
                                                    dImporteCheque = Convert.ToDouble(cheque.vcn_monto);

                                                    FtnImprimirTexto(sBancoCheque.PadRight(11) + "  " + sNoCheque.PadRight(12) + "  $" + String.Format("{0:#,0.00}", dImporteCheque).PadLeft(14));
                                                }

                                                //FtnImprimirTexto("------------------------------------------");
                                                //FtnImprimirTexto(" ");
                                            }

                                            /*Query que obtiene los datos de las Tarjetas de un Cliente*/
                                            /*
                                            sSql = "SELECT " +
                                                   "    vcc_banco AS bancoTarjeta, " +
                                                   "    vcf_num_cheque AS noTarjeta, " +
                                                   "    COALESCE(vcn_monto_cheque, 0.00) AS importeTarjeta " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcc_banco = 'TARJETA') " +
                                                   "AND (vcn_monto_cheque <> 0.00) " +
                                                   "AND (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "') " +
                                                   "UNION " +
                                                   "SELECT " +
                                                   "    vcc_banco2 AS bancoTarjeta,	" +
                                                   "	vcf_num_cheque2 AS noTarjeta, " +
                                                   "    COALESCE(vcn_monto_cheque2, 0.00) AS importeTarjeta " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcc_banco2 = 'TARJETA') " +
                                                   "AND (vcn_monto_cheque2 <> 0.00) " +
                                                   "AND (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "') " +
                                                   "UNION " +
                                                   "SELECT " +
                                                   "    vcc_banco3 AS bancoTarjeta, " +
                                                   "    vcf_num_cheque3 AS noTarjeta, " +
                                                   "    COALESCE(vcn_monto_cheque3, 0.00) AS importeTarjeta " +
                                                   "FROM venta_cabecera " +
                                                   "WHERE (vcc_banco3 = 'TARJETA') " +
                                                   "AND (vcn_monto_cheque3 <> 0.00) " +
                                                   "AND (vcn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vcn_folio = '" + sNoTicketParam + "')";

                                            listaTarjetasVentas = conn.Query<TarjetaVenta>(sSql);
                                            */
                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "TARJETA");

                                            if (pagos.Count > 0)
                                            //if (listaTarjetasVentas.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("                 TARJETAS");
                                                FtnImprimirTexto("No. Cuenta                         Importe");
                                                //FtnImprimirTexto("--------------------       $000,000,000.00");

                                                //foreach (var tarjetaVenta in listaTarjetasVentas)
                                                foreach (var tarjeta in pagos)
                                                {
                                                    /*
                                                    sNoTarjeta = tarjetaVenta.noTarjeta == null ? "" : tarjetaVenta.noTarjeta.Trim();
                                                    dImporteTarjeta = tarjetaVenta.importeTarjeta;
                                                    */
                                                    sNoTarjeta = tarjeta.vpc_nocuenta;
                                                    dImporteTarjeta = Convert.ToDouble(tarjeta.vcn_monto);

                                                    FtnImprimirTexto(sNoTarjeta.PadRight(20) + "       $" + String.Format("{0:#,0.00}", dImporteTarjeta).PadLeft(14));
                                                }

                                                //FtnImprimirTexto("------------------------------------------");
                                                //FtnImprimirTexto(" ");
                                            }

                                            /*Query que obtiene los datos de las Bonificaciones de Venta de un Cliente*/
                                            sSql = "SELECT " +
                                                   "    boc_cliente AS idCliente, " +
                                                   "    boc_folio_venta AS noTicket, " +
                                                   "    boc_folio AS folioBonific, " +
                                                   "    CASE " +
                                                   "        WHEN (boc_tipo = 'C') THEN 'CERVEZA' " +
                                                   "        WHEN (boc_tipo = 'R') THEN 'REFRESCO' " +
                                                   "        WHEN (boc_tipo = 'E') THEN 'ENVASE' " +
                                                   "        WHEN (boc_tipo = 'K') THEN 'KERMATO' " +
                                                   "        WHEN (boc_tipo = 'L') THEN 'LICENCIA' " +
                                                   "        WHEN (boc_tipo = 'Z') THEN 'MENSUAL' " +
                                                   "    END AS tipoBonific, " +
                                                   "    boi_documento AS importeBonific " +
                                                   "FROM bonificaciones " +
                                                   "WHERE (boc_folio_venta = '" + sNoTicketParam + "')";

                                            listaBonificacionesVentas = conn.Query<BonificacionVenta>(sSql);

                                            if (listaBonificacionesVentas.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("              BONIFICACIONES");
                                                FtnImprimirTexto("Folio         Tipo                 Importe");
                                                //FtnImprimirTexto("------------  ------------ $000,000,000.00");

                                                foreach (var bonificacionVenta in listaBonificacionesVentas)
                                                {
                                                    idCliente = bonificacionVenta.idCliente;
                                                    noTicket = bonificacionVenta.noTicket == null ? "" : bonificacionVenta.noTicket.Trim();
                                                    folioBonific = bonificacionVenta.folioBonific == null ? "" : bonificacionVenta.folioBonific.Trim();
                                                    tipoBonific = bonificacionVenta.tipoBonific == null ? "" : bonificacionVenta.tipoBonific.Trim();
                                                    importeBonific = bonificacionVenta.importeBonific;

                                                    FtnImprimirTexto(folioBonific.PadRight(12) + "  " + tipoBonific.PadRight(12) + " $" + String.Format("{0:#,0.00}", importeBonific).PadLeft(14));
                                                }

                                                //FtnImprimirTexto("------------------------------------------");
                                                //FtnImprimirTexto(" ");
                                            }

                                            FtnImprimirTexto("------------------------------------------");

                                            /*Query que obtiene los datos de los Saldos de Envases de un Cliente*/
                                            sSql = "SELECT " +
                                                    "    a.cln_clave AS idCliente, " +
                                                    "    a.mec_envase AS idEnvase, " +
                                                    "    a.ard_corta AS descripCorta, " +
                                                    "    a.men_saldo_inicial AS saldoInicial, " +
                                                    "    b.vdn_cargo AS cargo, " +
                                                    "    a.men_abono AS abono, " +
                                                    "    a.men_venta AS venta " +
                                                    "FROM ( " +
                                                    "    SELECT " +
                                                    "       en.cln_clave, " +
                                                    "       en.mec_envase, " +
                                                    "       pr1.ard_corta, " +
                                                    "       en.men_saldo_inicial, " +
                                                    "       en.men_cargo, " +
                                                    "       en.men_abono, " +
                                                    "       en.men_venta" +
                                                    "    FROM envase en " +
                                                    "    JOIN productos pr1 ON (en.mec_envase = pr1.arc_clave) " +
                                                    "    WHERE (en.cln_clave = " + iIdClienteParam + ") " +
                                                    ") AS a " +
                                                    "LEFT JOIN ( " +
                                                    "   SELECT " +
                                                    "       vdp.vdn_cliente, " +
                                                    "       vdp.arc_envase, " +
                                                    "       pr2.ard_corta, " +
                                                    "       0 AS vdn_saldo_inicial, " +
                                                    "       SUM(vdp.vdn_venta) AS vdn_cargo, " +
                                                    "       0 AS vdn_abono, " +
                                                    "       0 AS vdn_venta " +
                                                    "   FROM ( " +
                                                    "       SELECT " +
                                                    "           vd.vdn_cliente, " +
                                                    "           pr1.arc_envase, " +
                                                    "           vd.vdn_venta " +
                                                    "       FROM venta_detalle AS vd " +
                                                    "       JOIN productos pr1 ON (vd.vdn_producto = pr1.arc_clave) " +
                                                    "       WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                    "   ) AS vdp " +
                                                    "   JOIN productos pr2 ON (pr2.arc_clave = vdp.arc_envase) " +
                                                    "   WHERE (vdp.arc_envase <> '') " +
                                                    "   GROUP BY vdp.vdn_cliente, pr2.ard_corta, vdp.arc_envase " +
                                                    ") AS b ON ((a.cln_clave = b.vdn_cliente) AND (a.mec_envase = b.arc_envase)) " +
                                                    "WHERE ((saldoInicial <> 0) OR (cargo <> 0) OR (abono <> 0) OR (venta <> 0)) " +
                                                    "ORDER BY idEnvase";

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("           MOVIMIENTO DE ENVASES");
                                            FtnImprimirTexto("   Envase      Ini   Car   Abo   Vta   Fin");

                                            listaSaldosEnvases = conn.Query<SaldosEnvases>(sSql);

                                            foreach (var SaldoEnvase in listaSaldosEnvases)
                                            {
                                                sIdEnvase = SaldoEnvase.idEnvase == null ? "" : SaldoEnvase.idEnvase.Trim();
                                                sDescripCorta  = SaldoEnvase.descripCorta == null ? "" : SaldoEnvase.descripCorta.Trim();
                                                iSaldoInicial = SaldoEnvase.saldoInicial;
                                                iCargo = SaldoEnvase.cargo;
                                                iAbono = SaldoEnvase.abono;
                                                iVenta = SaldoEnvase.venta;
                                                iSaldoFinal = (((iSaldoInicial + iCargo) - iAbono) - iVenta);
                                                //iSaldoFinal = saldoEnvaseEntrega.saldoFinal;

                                                FtnImprimirTexto(sIdEnvase.PadRight(2).Substring(0, 2) + " " +
                                                                 sDescripCorta.PadRight(10).Substring(0, 10) + 
                                                                 iSaldoInicial.ToString().PadLeft(5) + " " +
                                                                 iCargo.ToString().PadLeft(5) + " " +
                                                                 iAbono.ToString().PadLeft(5) + " " +
                                                                 iVenta.ToString().PadLeft(5) + " " +
                                                                 iSaldoFinal.ToString().PadLeft(5));
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
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Venta.\nDetalle: " + exc.Message);
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

    /*Clase para guardar los datos generales de un Cliente y Totales de un Ticket*/
    class CabeceraVenta
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
    }

    /*Clase para guardar los datos de los Detalles de Venta por Grupos de Productos de un Cliente*/
    class DetallesVenta
    {
        public int cantidad { get; set; }
        public string idProducto { get; set; }
        public string descripcion { get; set; }
        public double precioBase { get; set; }
        public double porcentajeDescuento { get; set; }
        public double importeDescuento { get; set; }
        public double precioFinal { get; set; }
        public double precio { get; set; }
        public double importe { get; set; }
        public string idTipo { get; set; }
        public string clasificacion { get; set; }
        public string ordenTicket { get; set; }
    }

    /*Clase para guardar los datos del Importe de los Envases comprados por un Cliente*/
    class ImporteEnvases
    {
        public int idClienteEnvase { get; set; }
        public string noTicketEnvase { get; set; }
        public double importeEnvase { get; set; }
    }

    /*Clase para guardar el Total en Efectivo de un Cliente*/
    class EfectivoVenta
    {
        public double totalEfectivo { get; set; }
    }

    /*Clase para guardar el Total en Transferencias de un Cliente*/
    class TransferenciaVenta
    {
        public double totalTransferencia { get; set; }
    }

    /*Clase para guardar los datos de los Cheques de un Cliente*/
    class ChequeVenta
    {
        public string bancoCheque { get; set; }
        public string noCheque { get; set; }
        public double importeCheque { get; set; }
    }

    /*Clase para guardar los datos de las Tarjetas de un Cliente*/
    class TarjetaVenta
    {
        public string noTarjeta { get; set; }
        public double importeTarjeta { get; set; }
    }

    /*Clase para guardar los datos de las Bonificaciones de Venta de un Cliente*/
    class BonificacionVenta
    {
        public int idCliente { get; set; }
        public string noTicket { get; set; }
        public string folioBonific { get; set; }
        public string tipoBonific { get; set; }
        public double importeBonific { get; set; }
    }

    /*Clase para guardar los datos de los Saldos de Envases de un Cliente*/
    class SaldosEnvases
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
