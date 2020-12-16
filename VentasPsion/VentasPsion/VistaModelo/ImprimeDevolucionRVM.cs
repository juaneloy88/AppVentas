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
    public class ImprimeDevolucionRVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ImprimeDevolucionRVM()
        {

        }

        /*Método para realizar la Impresión de un Ticket de Pago en la Impresora configurada*/
        public async void FtnImprimirTicketDevolucionR(int iIdClienteParam, string sNoTicketParam, string sConceptoDevolParam)
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

                                        /*Variables para datos generales de un Cliente y Totales de Devolución*/
                                        int iIdCliente = 0;
                                        int iIdRuta = 0;
                                        string sNoTicket = "";
                                        string sNombreCliente = "";
                                        string sRfc = "";
                                        string sDomicilio = "";
                                        double dSaldoAnterior = 0.00;
                                        double dImporteVenta = 0.00;

                                        /*Variables para datos de los Detalles de Devolución por Grupos de Productos de un Cliente*/
                                        int iCantidad = 0;
                                        string sIdProducto = "";
                                        string sDescripcion = "";
                                        double dPrecio = 0.00;
                                        double dImporte = 0.00;
                                        string sIdTipo = "";
                                        string sClasificacion = "";
                                        string sOrdenTicket = "";
                                        double dImporteTotal = 0.00;

                                        /*Variables para datos del Importe de los Envases devueltos por un Cliente*/
                                        double importeEnvase = 0.00;

                                        /*Variables para datos de los Saldos de Envases despues de la Devolución de un Cliente*/
                                        string sIdEnvase = "";
                                        string sDescripCorta = "";
                                        int iSaldoInicial = 0;
                                        int iCargo = 0;
                                        int iAbono = 0;
                                        int iVenta = 0;
                                        int iSaldoFinal = 0;

                                        string sSql = "";

                                        List<CabeceraDevolucion> listaCabeceraDevolucion = null;
                                        List<DetallesDevolucion> listaDetallesDevolucion = null;
                                        List<ImporteEnvasesDevol> listaImporteEnvasesDevol = null;
                                        List<SaldosEnvasesDevol> listaSaldosEnvasesDevol = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene los datos generales de un Cliente y Totales de Devolución*/
                                            sSql = "SELECT " +
                                                   "    DISTINCT cl.run_clave AS idRuta, " +
                                                   "    cl.cln_clave AS idCliente, " +
                                                   "	vd.vdn_folio_devolucion AS noTicket, " +
                                                   "    cl.clc_nombre_comercial AS nombreComercial, " +
                                                   "    cl.clc_nombre AS nombreCliente, " +
                                                   "    cl.clc_rfc AS rfc, " +
                                                   "    cl.clc_domicilio AS domicilio, " +
                                                   "    vc.vcn_saldo_ant AS saldoAnterior, " +
                                                   "    vc.vcn_importe AS importeVenta " +
                                                   "FROM venta_detalle AS vd " +
                                                   "INNER JOIN clientes AS cl ON (vd.vdn_cliente = cl.cln_clave) " +
                                                   "INNER JOIN venta_cabecera AS vc ON (vd.vdn_cliente = vc.vcn_cliente) " +
                                                   "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vd.vdn_folio_devolucion = '" + sNoTicketParam + "')";

                                            listaCabeceraDevolucion = conn.Query<CabeceraDevolucion>(sSql);

                                            if (listaCabeceraDevolucion.Count == 0)
                                            {
                                                utilerias.crearMensajeLargo("El Cliente NO tiene Devoluciones para generar el Ticket.");
                                                return;
                                            }

                                            foreach (var cabeceraDevolucion in listaCabeceraDevolucion)
                                            {
                                                iIdCliente = cabeceraDevolucion.idCliente;
                                                iIdRuta = cabeceraDevolucion.idRuta;
                                                sNoTicket = cabeceraDevolucion.noTicket == null ? "" : cabeceraDevolucion.noTicket.Trim();
                                                sNombreCliente = cabeceraDevolucion.nombreCliente == null ? "" : cabeceraDevolucion.nombreCliente.Trim();
                                                if (sNombreCliente.Trim() == "")
                                                    sNombreCliente = cabeceraDevolucion.nombreComercial == null ? "" : cabeceraDevolucion.nombreComercial.Trim();
                                                sRfc = cabeceraDevolucion.rfc == null ? "" : cabeceraDevolucion.rfc.Trim();
                                                sDomicilio = cabeceraDevolucion.domicilio == null ? "" : cabeceraDevolucion.domicilio.Trim();
                                                dSaldoAnterior = cabeceraDevolucion.saldoAnterior;
                                                dImporteVenta = cabeceraDevolucion.importeVenta;
                                            }

                                            FtnImprimirTexto("          TICKET DE DEVOLUCION");
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
                                            FtnImprimirTexto("Motivo de Devolucion: " + (char)27 + "F" + (char)1 + (char)27 + "k4" + (char)28);
                                            FtnImprimirTexto(sConceptoDevolParam + (char)29 + "");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Ruta Reparto: " + VarEntorno.iNoRuta);
                                            FtnImprimirTexto("Ruta Preventa: " + iIdRuta);
                                            FtnImprimirTexto("Ticket: " + sNoTicket);
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().Trim());
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().Trim());
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Cliente: " + iIdCliente + " - " + sNombreCliente);
                                            FtnImprimirTexto("RFC: " + sRfc);
                                            FtnImprimirTexto("Direccion: " + sDomicilio);
                                            FtnImprimirTexto("------------------------------------------");

                                            /*Query que obtiene los datos de los Detalles de Devolución por Grupos de Productos de un Cliente*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(vd.vdn_venta_dev, 0)) AS cantidad, " +
                                                   "	pr.arc_clave AS idProducto, " +
                                                   "    pr.ard_descripcion AS descripcion, " +
                                                   "    vd.vdn_precio AS precio, " +
                                                   "    SUM(COALESCE(vd.vdn_importe, 0.00)) AS importe, " +
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
                                                   "AND (vd.vdn_folio_devolucion = '" + sNoTicketParam + "') " +
                                                   "GROUP BY pr.arc_clave, pr.ard_descripcion, vd.vdn_precio, pr.arc_produ " +
                                                   "UNION " +
                                                   "SELECT " +
                                                   "    SUM(COALESCE(vd.vdn_venta_dev, 0)) AS cantidad, " +
                                                   "    '' AS idProducto, " +
                                                   "    CASE " +
                                                   "        WHEN (pr.arc_produ = 'C') THEN 'TOTAL : CERVEZA' " +
                                                   "        WHEN (pr.arc_produ = 'R') THEN 'TOTAL : REFRESCO' " +
                                                   "        WHEN (pr.arc_produ = 'E') THEN 'TOTAL : ENVASE' " +
                                                   "        WHEN (pr.arc_produ = 'A') THEN 'TOTAL : AGUA' " +
                                                   "        WHEN (pr.arc_produ = 'V') THEN 'TOTAL : KERMATO' " +
                                                   "    END AS descripcion, " +
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
                                                   "AND (vd.vdn_folio_devolucion = '" + sNoTicketParam + "') " +
                                                   "GROUP BY pr.arc_produ " +
                                                   "ORDER BY ordenTicket";

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Cant Producto              Precio  Importe");
                                            FtnImprimirTexto("------------------------------------------");

                                            listaDetallesDevolucion = conn.Query<DetallesDevolucion>(sSql);

                                            foreach (var detalleVenta in listaDetallesDevolucion)
                                            {
                                                iCantidad = detalleVenta.cantidad;
                                                sIdProducto = detalleVenta.idProducto == null ? "" : detalleVenta.idProducto.Trim();
                                                sDescripcion = detalleVenta.descripcion == null ? "" : detalleVenta.descripcion.Trim();
                                                dPrecio = detalleVenta.precio;
                                                dImporte = detalleVenta.importe;
                                                sIdTipo = detalleVenta.idTipo == null ? "" : detalleVenta.idTipo.Trim();
                                                sClasificacion = detalleVenta.clasificacion == null ? "" : detalleVenta.clasificacion.Trim();
                                                sOrdenTicket = detalleVenta.ordenTicket == null ? "" : detalleVenta.ordenTicket.Trim();

                                                if (sDescripcion.PadRight(5).Substring(0, 5) == "TOTAL")
                                                {
                                                    FtnImprimirTexto(" ");
                                                    FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " + sDescripcion.ToString().PadRight(21).Substring(0, 21));
                                                    FtnImprimirTexto("------------------------------------------");

                                                }
                                                else
                                                {
                                                    FtnImprimirTexto(iCantidad.ToString().PadLeft(4) + " " +
                                                                     sDescripcion.ToString().PadRight(21).Substring(0, 21) + " " +
                                                                     String.Format("{0:0.00}", dPrecio).PadLeft(5) + " " +
                                                                     String.Format("{0:0.00}", dImporte).PadLeft(5));

                                                    dImporteTotal += dImporte;
                                                }
                                            }

                                            /*Query que obtiene los datos del Importe de los Envases devueltos por un Cliente*/
                                            sSql = "SELECT " +
                                                   "    SUM(COALESCE(vd.vdn_importe, 0.00)) AS importeEnvase " +
                                                   "FROM venta_detalle AS vd " +
                                                   "LEFT OUTER JOIN productos AS pr ON (vd.vdn_producto = pr.arc_clave) " +
                                                   "WHERE (vd.vdn_cliente = " + iIdClienteParam + ") " +
                                                   "AND (vd.vdn_folio_devolucion = '" + sNoTicketParam + "') " +
                                                   "AND (pr.arc_produ = 'E')";

                                            listaImporteEnvasesDevol = conn.Query<ImporteEnvasesDevol>(sSql);

                                            foreach (var importeEnvasesDevol in listaImporteEnvasesDevol)
                                            {
                                                importeEnvase = importeEnvasesDevol.importeEnvase;
                                            }

                                            clientesSR oBuscarCliente = new clientesSR();
                                            var oCliente = await oBuscarCliente.DatosCliente(iIdClienteParam.ToString().Trim());
                                            double dSaldoFinal = Convert.ToDouble(new fnVentaCabecera().SaldoFinal(oCliente, Convert.ToInt32(sNoTicket)));

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Saldo Anterior........: $ " + String.Format("{0:#,0.00}", dSaldoFinal).PadLeft(15));
                                            FtnImprimirTexto("Total Liquido.........: $ " + String.Format("{0:#,0.00}", (dImporteVenta - importeEnvase)).PadLeft(15));
                                            FtnImprimirTexto("Total Envase..........: $ " + String.Format("{0:#,0.00}", importeEnvase).PadLeft(15));
                                            FtnImprimirTexto("Total Venta...........: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", (dSaldoFinal + dImporteVenta)).PadLeft(15));
                                            FtnImprimirTexto("Devolucion............: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            FtnImprimirTexto("Saldo Actual..........: $ " + String.Format("{0:#,0.00}", dSaldoFinal).PadLeft(15));
                                            FtnImprimirTexto("------------------------------------------");

                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("------------------------------------------");
                                            //FtnImprimirTexto("Saldo Anterior........: $ " + String.Format("{0:#,0.00}", dSaldoAnterior).PadLeft(15));
                                            //FtnImprimirTexto("Total Liquido.........: $ " + String.Format("{0:#,0.00}", (dImporteVenta - importeEnvase)).PadLeft(15));
                                            //FtnImprimirTexto("Total Envase..........: $ " + String.Format("{0:#,0.00}", importeEnvase).PadLeft(15));
                                            //FtnImprimirTexto("Total Venta...........: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            //FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", (dSaldoAnterior + dImporteVenta)).PadLeft(15));
                                            //FtnImprimirTexto("Devolucion............: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            //FtnImprimirTexto("Saldo Actual..........: $ " + String.Format("{0:#,0.00}", ((dSaldoAnterior + dImporteVenta) - dImporteVenta)).PadLeft(15));
                                            //FtnImprimirTexto("------------------------------------------");

                                            /*Query que obtiene los datos de los Saldos de Envases despues de la Devolución de un Cliente*/
                                            sSql = "SELECT " +
                                                    "   en.cln_clave AS idCliente, " +
                                                    "	en.mec_envase AS idEnvase, " +
                                                    "   pr.ard_corta AS descripCorta, " +
                                                    "   en.men_saldo_inicial AS saldoInicial, " +
                                                    "   en.men_cargo AS cargo, " +
                                                    "   en.men_abono AS abono, " +
                                                    "   en.men_venta AS venta, " +
                                                    "   en.men_saldo_final AS saldoFinal " +
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
                                            
                                            listaSaldosEnvasesDevol = conn.Query<SaldosEnvasesDevol>(sSql);

                                            foreach (var saldoEnvaseDevol in listaSaldosEnvasesDevol)
                                            {
                                                sIdEnvase = saldoEnvaseDevol.idEnvase == null ? "" : saldoEnvaseDevol.idEnvase.Trim();
                                                sDescripCorta = saldoEnvaseDevol.descripCorta == null ? "" : saldoEnvaseDevol.descripCorta.Trim();
                                                iSaldoInicial = saldoEnvaseDevol.saldoInicial;
                                                iCargo = saldoEnvaseDevol.cargo;
                                                iAbono = saldoEnvaseDevol.abono;
                                                iVenta = saldoEnvaseDevol.venta;
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
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Devolución.\nDetalle: " + exc.Message);
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

    /*Clase para guardar los datos generales de un Cliente y Totales de Devolución*/
    class CabeceraDevolucion
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
    }

    /*Clase para guardar los datos de los Detalles de Devolución por Grupos de Productos de un Cliente*/
    class DetallesDevolucion
    {
        public int cantidad { get; set; }
        public string idProducto { get; set; }
        public string descripcion { get; set; }
        public double precio { get; set; }
        public double importe { get; set; }
        public string idTipo { get; set; }
        public string clasificacion { get; set; }
        public string ordenTicket { get; set; }
    }

    /*Clase para guardar los datos del Importe de los Envases devueltos por un Cliente*/
    class ImporteEnvasesDevol
    {
        public double importeEnvase { get; set; }
    }

    /*Clase para guardar los datos de los Saldos de Envases despues de la Devolución de un Cliente*/
    class SaldosEnvasesDevol
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