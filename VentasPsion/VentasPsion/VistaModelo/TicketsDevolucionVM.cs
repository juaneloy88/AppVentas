using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;
using VentasPsion.Modelo.ServicioApi;

namespace VentasPsion.VistaModelo
{
    class TicketsDevolucionVM
    {
        public List<tickets_cabeceras> lstTicketsCabeceras = null;
        public List<venta_detalle> lstTicketsDetalles = null;

        private fnVentaCabecera FNventacab = new fnVentaCabecera();
        private fnVentaDetalle FNVenDet = new fnVentaDetalle();
        private venta_detalleSR VenDetSR = new venta_detalleSR();
        private facturasventasSR FNfactura = new facturasventasSR();

        ConexionService conexionWifiDatos = new ConexionService();
        StatusRestService statusConexion = new StatusRestService();

        public string sUUID = "";

        public List<tickets_cabeceras> FtnRegresarTicketsDevolucion(string iIdCliente, string sNoTicket)
        {
            lstTicketsCabeceras = new List<tickets_cabeceras>();
            
            statusConexion = conexionWifiDatos.FtnValidarConexionWifiDatos();

            if (statusConexion.status == true)
            {
                string sConexionUri = statusConexion.mensaje;

                TicketsCabecerasRestServiceA ticketsCabecerasApi = new TicketsCabecerasRestServiceA();

                lstTicketsCabeceras = ticketsCabecerasApi.FtnRegresarTicketsCabecerasA(Convert.ToInt32(iIdCliente), sNoTicket, sConexionUri);
            }
            else
            {
                lstTicketsCabeceras = null;
                //await DisplayAlert("¡Atención!", statusConexion.mensaje + " para realizar la Recepción.", "OK");
            }

            return lstTicketsCabeceras;
        }

        public List<venta_detalle> FtnRegresarTicketsDetallesDevolucion(string iIdCliente, string sNoTicket, DateTime dFecha)
        {
            lstTicketsDetalles = new List<venta_detalle>();

            statusConexion = conexionWifiDatos.FtnValidarConexionWifiDatos();

            if (statusConexion.status == true)
            {
                string sConexionUri = statusConexion.mensaje;

                TicketsDetallesRestServiceA ticketsDetallesApi = new TicketsDetallesRestServiceA();

                lstTicketsDetalles = ticketsDetallesApi.FtnRegresarTicketsDetallesA(Convert.ToInt32(iIdCliente), sNoTicket, dFecha, sConexionUri);
            }
            else
            {
                lstTicketsDetalles = null;
                //await DisplayAlert("¡Atención!", statusConexion.mensaje + " para realizar la Recepción.", "OK");
            }

            return lstTicketsDetalles;
        }

        //******  se realizan todos los procesos   pararelaizar uan devolucion automatica   ***//
        public async Task<bool> DevolucionAutoventa(int index)
        {
            try
            {
                decimal sImporte = 0;
                decimal dSaldo = VarEntorno.Saldo(VarEntorno.vCliente);

                bool bAnticipo = Convert.ToBoolean( lstTicketsCabeceras[index].tcn_contado);
                               
                foreach (var det in lstTicketsDetalles)
                    sImporte = sImporte + det.vdn_importe;

                DocumentosVM Doctos = new DocumentosVM();
                Doctos.anticipo.cln_clave = VarEntorno.vCliente.cln_clave;
                Doctos.anticipo.vcn_folio = VarEntorno.TraeFolio().ToString().PadLeft(6, '0');
                Doctos.anticipo.vcf_movimiento = DateTime.Now.Date;
                Doctos.anticipo.ann_monto_pago = sImporte;
                Doctos.anticipo.ann_cantidad_pagos = 1;
                Doctos.anticipo.anc_forma_pago = lstTicketsCabeceras[index].tcc_forma_pago;
                Doctos.anticipo.anb_nuevo = true;
                Doctos.anticipo.anb_relacionado = false;
                
                venta_cabecera vCabecera = new venta_cabecera();

                vCabecera.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                vCabecera.vcn_cliente = VarEntorno.vCliente.cln_clave;
                vCabecera.vcn_estatus = 1;
                vCabecera.vcn_importe = sImporte;
                vCabecera.vcn_saldo_ant = dSaldo;
                vCabecera.vcn_saldo_nuevo = dSaldo - sImporte;
                vCabecera.vcc_hora_ini = VarEntorno.sHoraInicio;
                vCabecera.vcc_hora_fin = DateTime.Now.ToShortTimeString();
                vCabecera.vcc_tipo_pago = "D";

                vCabecera.vcn_monto_efe = sImporte;

                vCabecera.vcc_banco = "";
                vCabecera.vcf_num_cheque = "0";
                vCabecera.vcf_fec_cheque = DateTime.Now.Date;
                vCabecera.vcn_monto_cheque = 0;


                vCabecera.vcc_banco2 = "";
                vCabecera.vcf_num_cheque2 = "0";
                vCabecera.vcf_fec_cheque2 = DateTime.Now.Date;
                vCabecera.vcn_monto_cheque2 = 0;

                vCabecera.vcc_banco3 = "";
                vCabecera.vcf_num_cheque3 = "0";
                vCabecera.vcf_fec_cheque3 = DateTime.Now.Date;
                vCabecera.vcn_monto_cheque3 = 0;

                vCabecera.vcn_ruta = VarEntorno.iNoRuta;
                vCabecera.vcf_movimiento = DateTime.Now.ToShortDateString();
                vCabecera.vcc_clave = VarEntorno.sAlmacen;

                vCabecera.cmpc_clave_sat = "99";
                vCabecera.vcc_ctapgosat = "0000";

                vCabecera.cmpc_metodopago = "PPD";
                vCabecera.cfpc_formapago = "99";



                //////cargar en el campo uuid las facturas correspondientes a la cancelacion 
                vCabecera.fcd_uuid = sUUID;

                
                if (VenDetSR.GuardaDetalleDEV(lstTicketsDetalles))
                {
                    if (FNventacab.GuardaNormalCompleta(vCabecera, 3, false, false))
                    {
                        if (bAnticipo)
                            Doctos.Guarda_Anticipo();
                        return true;
                    }
                    else
                    {
                        string sResultado = await VenDetSR.BorrarVenta(VarEntorno.iFolio.ToString().PadLeft(6, '0'));

                        if ("Ticket Eliminado" == sResultado)
                            VarEntorno.sMensajeError = "Error al guardar la cabecera ";
                        else
                            VarEntorno.sMensajeError = "Error al guardar la cabecera y  el detalle ";

                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

    }
}
