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
    public class ImprimePagoARPVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método constructor de la clase*/
        public ImprimePagoARPVM()
        {

        }

        /*Método para realizar la Impresión de un Ticket de Pago en la Impresora configurada*/
        public async void FtnImprimirTicketPagoARP(int iIdClienteParam, string sNoTicketParam)
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

                                        /*Variables para datos generales de un Cliente y Totales de un Ticket de Pago*/
                                        int iIdCliente = 0;
                                        int iIdRuta = 0;
                                        string sNoTicket = "";
                                        string sNombreCliente = "";
                                        string sRfc = "";
                                        string sDomicilio = "";
                                        double dSaldoAnterior = 0.00;
                                        double dImporteVenta = 0.00;
                                        double dMontoPago = 0.00;
                                        double importeEnvase = 0.00;

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

                                        List<CabeceraPago> listaCabeceraPago = null;
                                        List<EfectivoPago> listaEfectivoPagos = null;
                                        List<TransferenciaPago> listaTransferenciaPagos = null;
                                        List<ChequePago> listaChequesPagos = null;
                                        List<TarjetaPago> listaTarjetasPagos = null;
                                        List<BonificacionPago> listaBonificacionesPagos = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene los datos generales de un Cliente y Totales de un Ticket*/
                                            sSql = "SELECT " +
                                                    "    DISTINCT cl.run_clave AS idRuta, " +
                                                    "    cl.cln_clave AS idCliente, " +
                                                    "	 vc.vcn_folio AS noTicket, " +
                                                    "    cl.clc_nombre_comercial AS nombreComercial, " +
                                                    "    cl.clc_nombre AS nombreCliente, " +
                                                    "    cl.clc_rfc AS rfc, " +
                                                    "    cl.clc_domicilio AS domicilio, " +
                                                    "    cl.cln_limite_venta AS cln_limite_venta, " +
                                                    "    vc.vcn_saldo_ant AS saldoAnterior, " +
                                                    "    vc.vcn_importe AS importeVenta, " +
                                                    "    vc.vcn_monto_pago AS montoPago " +
                                                    "FROM clientes AS cl " +
                                                    "INNER JOIN venta_cabecera AS vc ON (cl.cln_clave = vc.vcn_cliente) " +
                                                    "WHERE (vc.vcn_cliente = " + iIdClienteParam + ") " +
                                                    "AND (vc.vcn_folio = '" + sNoTicketParam + "')";

                                            listaCabeceraPago = conn.Query<CabeceraPago>(sSql);

                                            if (listaCabeceraPago.Count == 0)
                                            {
                                                utilerias.crearMensajeLargo("El Cliente NO tiene Pagos para generar el Ticket.");
                                                return;
                                            }

                                            foreach (var cabeceraPago in listaCabeceraPago)
                                            {
                                                iIdCliente = cabeceraPago.idCliente;
                                                iIdRuta = cabeceraPago.idRuta;
                                                sNoTicket = cabeceraPago.noTicket == null ? "" : cabeceraPago.noTicket.Trim();
                                                sNombreCliente = cabeceraPago.nombreCliente == null ? "" : cabeceraPago.nombreCliente.Trim();
                                                if (sNombreCliente.Trim() == "")
                                                    sNombreCliente = cabeceraPago.nombreComercial == null ? "" : cabeceraPago.nombreComercial.Trim();
                                                sRfc = cabeceraPago.rfc == null ? "" : cabeceraPago.rfc.Trim();
                                                sDomicilio = cabeceraPago.domicilio == null ? "" : cabeceraPago.domicilio.Trim();
                                                dSaldoAnterior = cabeceraPago.saldoAnterior;
                                                dImporteVenta = cabeceraPago.importeVenta;
                                                dMontoPago = cabeceraPago.montoPago;
                                            }

                                            FtnImprimirTexto("             TICKET DE PAGO");
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

                                            importeEnvase = 0.00;

                                            clientesSR oBuscarCliente = new clientesSR();
                                            var oCliente = await oBuscarCliente.DatosCliente(iIdClienteParam.ToString().Trim());
                                            decimal dSaldoFinal = new fnVentaCabecera().SaldoFinal(oCliente);

                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto("Saldo Anterior........: $ " + String.Format("{0:#,0.00}", ((Convert.ToDouble(dSaldoFinal) + dMontoPago) - dImporteVenta)).PadLeft(15));
                                            FtnImprimirTexto("Total Liquido.........: $ " + String.Format("{0:#,0.00}", (dImporteVenta - importeEnvase)).PadLeft(15));
                                            FtnImprimirTexto("Total Envase..........: $ " + String.Format("{0:#,0.00}", importeEnvase).PadLeft(15));
                                            FtnImprimirTexto("Total Venta...........: $ " + String.Format("{0:#,0.00}", dImporteVenta).PadLeft(15));
                                            FtnImprimirTexto("Total Neto............: $ " + String.Format("{0:#,0.00}", (Convert.ToDouble(dSaldoFinal) + dMontoPago)).PadLeft(15));
                                            FtnImprimirTexto("Pago..................: $ " + String.Format("{0:#,0.00}", dMontoPago).PadLeft(15));
                                            FtnImprimirTexto("Saldo Actual..........: $ " + String.Format("{0:#,0.00}", dSaldoFinal).PadLeft(15));
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Su Pago: $" + String.Format("{0:#,0.00}", dMontoPago).PadRight(15));
                                            FtnImprimirTexto("Gracias!!!");

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
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto("Su Pago: $" + String.Format("{0:#,0.00}", dMontoPago).PadRight(15));
                                            //FtnImprimirTexto("Gracias!!!");

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

                                            listaEfectivoPagos = conn.Query<EfectivoPago>(sSql);
                                            */

                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "EFECTIVO");

                                            if (pagos.Count > 0)

                                            //if (listaEfectivoPagos.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("                 EFECTIVO");

                                                //foreach (var efectivoPago in listaEfectivoPagos)
                                                foreach (var efectivo in pagos)
                                                {
                                                    // dTotalEfectivo = efectivoPago.totalEfectivo;
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

                                            listaTransferenciaPagos = conn.Query<TransferenciaPago>(sSql);*/

                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "TRANSFERENCIA");

                                            if (pagos.Count > 0)

                                            //if (listaTransferenciaPagos.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("              TRANSFERENCIA");

                                                //foreach (var transferenciaPago in listaTransferenciaPagos)
                                                foreach (var transferencia in pagos)
                                                {
                                                    // dTotalTransferencia = transferenciaPago.totalTransferencia;
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

                                            listaChequesPagos = conn.Query<ChequePago>(sSql);
                                            */

                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "CHEQUE");

                                            if (pagos.Count > 0)

                                            //if (listaChequesPagos.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("                  CHEQUES");
                                                FtnImprimirTexto("Banco        No. Cheque            Importe");
                                                //FtnImprimirTexto("-----------  ------------  $000,000,000.00");

                                                //foreach (var chequePago in listaChequesPagos)
                                                foreach (var cheque in pagos)
                                                {
                                                    /*
                                                    sBancoCheque = chequePago.bancoCheque == null ? "" : chequePago.bancoCheque.Trim();
                                                    sNoCheque = chequePago.noCheque == null ? "" : chequePago.noCheque.Trim();
                                                    dImporteCheque = chequePago.importeCheque;
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

                                            listaTarjetasPagos = conn.Query<TarjetaPago>(sSql);
                                            */
                                            pagos = pagosSR.PagosxFolioxCliente(iIdClienteParam, sNoTicketParam, "TARJETA");

                                            if (pagos.Count > 0)
                                            //if (listaTarjetasPagos.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("                 TARJETAS");
                                                FtnImprimirTexto("No. Cuenta                         Importe");
                                                //FtnImprimirTexto("--------------------       $000,000,000.00");

                                                //foreach (var tarjetaPago in listaTarjetasPagos)
                                                foreach (var tarjeta in pagos)
                                                {
                                                    /*
                                                    sNoTarjeta = tarjetaPago.noTarjeta == null ? "" : tarjetaPago.noTarjeta.Trim();
                                                    dImporteTarjeta = tarjetaPago.importeTarjeta;
                                                    */
                                                    sNoTarjeta = tarjeta.vpc_nocuenta;
                                                    dImporteTarjeta = Convert.ToDouble(tarjeta.vcn_monto);

                                                    FtnImprimirTexto(sNoTarjeta.PadRight(20) + "       $" + String.Format("{0:#,0.00}", dImporteTarjeta).PadLeft(14));
                                                }

                                                //FtnImprimirTexto("------------------------------------------");
                                                //FtnImprimirTexto(" ");
                                            }

                                            /*Query que obtiene los datos de las Bonificaciones de un Cliente*/
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

                                            listaBonificacionesPagos = conn.Query<BonificacionPago>(sSql);

                                            if (listaBonificacionesPagos.Count > 0)
                                            {
                                                FtnImprimirTexto(" ");
                                                //FtnImprimirTexto("------------------------------------------");
                                                FtnImprimirTexto("              BONIFICACIONES");
                                                FtnImprimirTexto("Folio         Tipo                 Importe");
                                                //FtnImprimirTexto("------------  ------------ $000,000,000.00");

                                                foreach (var bonificacionPago in listaBonificacionesPagos)
                                                {
                                                    idCliente = bonificacionPago.idCliente;
                                                    noTicket = bonificacionPago.noTicket == null ? "" : bonificacionPago.noTicket.Trim();
                                                    folioBonific = bonificacionPago.folioBonific == null ? "" : bonificacionPago.folioBonific.Trim();
                                                    tipoBonific = bonificacionPago.tipoBonific == null ? "" : bonificacionPago.tipoBonific.Trim();
                                                    importeBonific = bonificacionPago.importeBonific;

                                                    FtnImprimirTexto(folioBonific.PadRight(12) + "  " + tipoBonific.PadRight(12) + " $" + String.Format("{0:#,0.00}", importeBonific).PadLeft(14));
                                                }

                                                //FtnImprimirTexto("------------------------------------------");
                                                //FtnImprimirTexto(" ");
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
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Pago.\nDetalle: " + exc.Message);
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

    /*Clase para guardar los datos generales de un Cliente y Totales de un Ticket de Pago*/
    class CabeceraPago
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
    }

    /*Clase para guardar el Total en Efectivo de un Cliente*/
    class EfectivoPago
    {
        public double totalEfectivo { get; set; }
    }

    /*Clase para guardar el Total en Transferencias de un Cliente*/
    class TransferenciaPago
    {
        public double totalTransferencia { get; set; }
    }

    /*Clase para guardar los datos de los Cheques de un Cliente*/
    class ChequePago
    {
        public string bancoCheque { get; set; }
        public string noCheque { get; set; }
        public double importeCheque { get; set; }
    }

    /*Clase para guardar los datos de las Tarjetas de un Cliente*/
    class TarjetaPago
    {
        public string noTarjeta { get; set; }
        public double importeTarjeta { get; set; }
    }

    /*Clase para guardar los datos de las Bonificaciones de un Cliente*/
    class BonificacionPago
    {
        public int idCliente { get; set; }
        public string noTicket { get; set; }
        public string folioBonific { get; set; }
        public string tipoBonific { get; set; }
        public double importeBonific { get; set; }
    }
}
