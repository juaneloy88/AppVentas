using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;
using System.Linq;

namespace VentasPsion.VistaModelo
{
    class DevolucionVM
    {
        public List<concepto_devoluciones>  ListaConseptos = new List<concepto_devoluciones>();
        private fnVentaCabecera FNventacab = new fnVentaCabecera();
        private facturasventasSR FNfactura = new facturasventasSR();

        public async Task<bool> ValidaEstatusCliente()
        {
            string sCliente = VarEntorno.vCliente.cln_clave.ToString();

            string sRespuesta = await new vmMostrarPedido().validaStatusCliente(sCliente);

            if (sRespuesta == "Cliente NO Visitado")
                return true;
            else
                return false;
        }

        //******  se realizan todos los procesos   pararelaizar uan devolucion automatica   ***//
        public bool DevolucionReparto(int iMotiDev)
        {
            try
            {                
                decimal sImporte = new venta_detalleSR().fnImporteTotalxCliente(VarEntorno.vCliente.cln_clave) ;
                decimal dSaldo = VarEntorno.Saldo(VarEntorno.vCliente);

                if (sImporte == -1)
                    return false;

                venta_cabecera vCabecera = new venta_cabecera();

                vCabecera.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                vCabecera.vcn_cliente = VarEntorno.vCliente.cln_clave;
                vCabecera.vcn_estatus = 1;
                vCabecera.vcn_importe = sImporte;
                vCabecera.vcn_saldo_ant = dSaldo;
                vCabecera.vcn_saldo_nuevo = dSaldo-sImporte ;
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
                vCabecera.fcd_uuid = FNfactura.fnFacturasDevolucion(vCabecera.vcn_cliente);

                if (FNventacab.GuardaDevolucionCompleta(vCabecera, iMotiDev))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //*****  carga los conseptos de devoluciones para mostrarse en pantalla   *****//
        public bool lConsepDev()
        {
            try
            {
                ListaConseptos = new conseptoDevolucionesSR().ListaConseptosDevoluciones();

                if (ListaConseptos == null)
                {
                    VarEntorno.sMensajeError = "No Hay Conseptos";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }

        }

        public bool FacConPagos()
        {
            try
            {
                bool bResultado = true;
                                               
                List<FacturasVenta> lFacturas = FNfactura.fnListaFacturas(VarEntorno.vCliente.cln_clave).Where(x=>x.fvc_tipo =="CREDITO").ToList();
                List<documentos_cabecera> lDoctos = new documentos_cabeceraSR().ListaDocCab();

                foreach (var Fac in lFacturas)
                    if (Fac.fvn_pagos > 0)
                        bResultado = false;

                foreach (var Doc in lDoctos)
                    if (Doc.dPagosVendedor>0)
                        bResultado = false;
                
                return bResultado;
            }
            catch
            {
                return false;
            }
        }
    }
}
