using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class CobranzaVM
    {
        //public decimal dEfectivo;
        //public List<Cheque> lCheque = new List<Cheque>();
        //public List<Tarjeta> lTarjeta = new List<Tarjeta>();
        //public List<Bonificacion> lBonificacion = new List<Bonificacion>();
        //public Transferencia cTransferencia;
        
        public List<venta_pagos> lpCheques = new List<venta_pagos>();
        public List<venta_pagos> lpTarjeta= new List<venta_pagos>();
        public List<venta_pagos> lpBonificacion = new List<venta_pagos>();
        public List<venta_pagos> lpTransferencia = new List<venta_pagos>();
        public venta_pagos pEfectivo = new venta_pagos();

        public List<venta_pagos> lPagos = new List<venta_pagos>();

        public decimal dPagosTicket;

        public decimal dPagosCliente;
        public decimal dVentasCliente;

        public decimal dImporteNeto;
        
        public Vista.frmCobranza vCobranza = null;
        public Vista.frmCobranzaPagos vCobranzaPagos = null;

        public documentos_cabecera dCabecera = new documentos_cabecera();
        public documentos_detalle dDetalle = new documentos_detalle();
        public documentos_detalle dAnticipo = new documentos_detalle();
        private fnVentaCabecera FnCabeceraVen = new fnVentaCabecera();
        private fnVentaDetalle FnDetalleVen = new fnVentaDetalle();

        //public string sUUID = "";
        //public string sFolioPadre = "";

        /******  constructor   *****/
        public CobranzaVM()
        {
            //dEfectivo = 0;
            //cTransferencia = new Transferencia();            
            pEfectivo.vcn_folio = VarEntorno.TraeFolio().ToString().PadLeft(6, '0'); 
            pEfectivo.vcn_cliente = VarEntorno.vCliente.cln_clave;
            pEfectivo.vcf_movimiento = DateTime.Now.ToShortDateString();
            pEfectivo.vcn_ruta = VarEntorno.iNoRuta;
            pEfectivo.cfpc_formapago = "01";
            pEfectivo.vpc_descripcion = "EFECTIVO";
        }
        
        
        /****   inicaliza valores a objeto venta_cabecera para guardar ******/
        public bool GuardaCabecera()
        {
            try
            {
                              

                string sConseptoSAT = "99";
                string sCuentaSAT = "0000";
                decimal dMayorImp = 0M;

                decimal dTTbonis = 0M;

                decimal dTTPago = 0M;
               // decimal dAnticipo = 0M;

                string sMetodoPago = "";
                string sFormaPago = "";
                int iEstatus;

                venta_cabecera vCabecera = new venta_cabecera();
                            
                bool bDocCab = false;
                bool bDocDet = false;

                vCabecera.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                vCabecera.vcn_cliente = VarEntorno.vCliente.cln_clave;
                vCabecera.vcn_estatus = 1;

                if (VarEntorno.dImporteTotal < 0M)
                    vCabecera.vcn_importe = -1 *  VarEntorno.dImporteTotal;
                else
                    vCabecera.vcn_importe = VarEntorno.dImporteTotal;

                switch (VarEntorno.cTipoVenta)
                {
                    case 'R':
                        /* vCabecera.vcn_saldo_ant = VarEntorno.vCliente.cln_saldo
                                                         - VarEntorno.dImporteTotal
                                                         + VarEntorno.vCliente.clc_pago_diferencia_preventa;*/
                        vCabecera.vcn_saldo_envase = null;
                        vCabecera.vcn_saldo_ant = VarEntorno.Saldo (VarEntorno.vCliente)- dImporteNeto;
                        break;
                    default:
                        //vCabecera.vcn_saldo_ant = VarEntorno.vCliente.cln_saldo;
                        vCabecera.vcn_saldo_ant = VarEntorno.Saldo(VarEntorno.vCliente);
                        vCabecera.vcn_saldo_envase = "0";
                        break;
                }
                if (VarEntorno.bAnticipoRelacionado && dPagosTicket > 0)
                {
                    if (VarEntorno.bAnticipoRelacionado)
                        if (VarEntorno.cAnticipos.dMontoPago > 0M)
                            dPagosTicket -= VarEntorno.cAnticipos.dMontoPago;                    
                }

                              
                vCabecera.vcn_saldo_nuevo = vCabecera.vcn_saldo_ant - dPagosTicket + VarEntorno.dImporteTotal;
                vCabecera.vcc_hora_ini = VarEntorno.sHoraInicio;
                vCabecera.vcc_hora_fin = DateTime.Now.ToShortTimeString();

                if (VarEntorno.bEsDevolucion)
                    vCabecera.vcc_tipo_pago = "D";
                else
                    vCabecera.vcc_tipo_pago = "$";

                lPagos.Clear();

                if (pEfectivo.vcn_monto > 0)
                {
                    dTTPago += pEfectivo.vcn_monto;
                    sConseptoSAT = "01";
                    pEfectivo.vcn_folio = VarEntorno.TraeFolio().ToString().PadLeft(6, '0');
                    pEfectivo.vcn_cliente = VarEntorno.vCliente.cln_clave;
                    pEfectivo.vcf_movimiento = DateTime.Now.ToShortDateString();
                    pEfectivo.vcn_ruta = VarEntorno.iNoRuta;
                    pEfectivo.cfpc_formapago = "01";
                    pEfectivo.vpc_descripcion = "EFECTIVO";
                    dMayorImp = VarEntorno.cCobranza.pEfectivo.vcn_monto;
                    lPagos.Add(pEfectivo);
                }

                foreach (var transferencia in VarEntorno.cCobranza.lpTransferencia)
                {
                    if (transferencia.vcn_monto > dMayorImp)
                    {
                        sConseptoSAT = transferencia.cfpc_formapago;
                        sCuentaSAT = transferencia.vpc_nocuenta;
                        dMayorImp = transferencia.vcn_monto;
                    }
                    lPagos.Add(transferencia);
                    dTTPago += transferencia.vcn_monto;                    
                }

                foreach (var tarjeta in VarEntorno.cCobranza.lpTarjeta)
                {
                    if (tarjeta.vcn_monto > dMayorImp)
                    {
                        sConseptoSAT = tarjeta.cfpc_formapago;
                        sCuentaSAT = tarjeta.vpc_nocuenta;
                        dMayorImp = tarjeta.vcn_monto;
                    }
                    lPagos.Add(tarjeta);
                    dTTPago += tarjeta.vcn_monto;
                }

                foreach (var cheque in VarEntorno.cCobranza.lpCheques)
                {
                    if (cheque.vcn_monto > dMayorImp)
                    {
                        sConseptoSAT = cheque.cfpc_formapago;
                        sCuentaSAT = cheque.vpc_nocuenta;
                        dMayorImp = cheque.vcn_monto;
                    }
                    lPagos.Add(cheque);
                    dTTPago += cheque.vcn_monto;
                }

                foreach (var bonificacion in VarEntorno.cCobranza.lpBonificacion)
                {
                    dTTPago += bonificacion.vcn_monto;
                    dTTbonis += bonificacion.vcn_monto;
                    lPagos.Add(bonificacion);
                }

                if (dTTbonis > dMayorImp)
                {
                    sConseptoSAT = "27";
                    sCuentaSAT = "0000";
                }

                #region Area de Pagos en venta Cabecera
                /*
                if (VarEntorno.cCobranza.dEfectivo == 0M && VarEntorno.cCobranza.cTransferencia.dCantidad == 0M)
                {
                    vCabecera.vcn_monto_efe = 0;
                    vCabecera.vcc_banco = "-----------";
                    vCabecera.efec_transf = "";
                }
                else if (VarEntorno.cCobranza.dEfectivo == 0M)
                {
                    vCabecera.vcn_monto_efe = VarEntorno.cCobranza.cTransferencia.dCantidad;
                    dTTPago += VarEntorno.cCobranza.cTransferencia.dCantidad;
                    vCabecera.vcc_banco = "-----------";
                    vCabecera.efec_transf = "TRANSFERENCIA";
                    sConseptoSAT = "03";
                    sCuentaSAT = VarEntorno.cCobranza.cTransferencia.iCuenta.ToString();
                    dMayorImp = VarEntorno.cCobranza.cTransferencia.dCantidad;
                }
                else
                {
                    vCabecera.vcn_monto_efe = VarEntorno.cCobranza.dEfectivo;
                    dTTPago += VarEntorno.cCobranza.dEfectivo;
                    vCabecera.vcc_banco = "-----------";
                    vCabecera.efec_transf = "EFECTIVO";
                    sConseptoSAT = "01";
                    dMayorImp = VarEntorno.cCobranza.dEfectivo;
                }

                ///ingresa los valores de las tajetas 
                foreach (var tarjeta in VarEntorno.cCobranza.lTarjeta)
                {
                    if (tarjeta.dImporte > dMayorImp)
                    {
                        sConseptoSAT = tarjeta.iTipoTarjeta.ToString().PadLeft(2, '0');
                        sCuentaSAT = tarjeta.iNCuenta.ToString();
                        dMayorImp = tarjeta.dImporte;
                    }
                    dTTPago += tarjeta.dImporte;

                    //////guarda las tarjetas en las posiciones disponibles
                    switch (iChequeTarjeta)
                    {
                        case 1:
                            vCabecera.vcc_banco = "TARJETA";
                            vCabecera.vcf_num_cheque = tarjeta.iNCuenta.ToString();
                            vCabecera.vcf_fec_cheque = tarjeta.dFecha;
                            vCabecera.vcn_monto_cheque = tarjeta.dImporte;
                            iChequeTarjeta++;
                            break;
                        case 2:
                            vCabecera.vcc_banco2 = "TARJETA";
                            vCabecera.vcf_num_cheque2 = tarjeta.iNCuenta.ToString();
                            vCabecera.vcf_fec_cheque2 = tarjeta.dFecha;
                            vCabecera.vcn_monto_cheque2 = tarjeta.dImporte;
                            iChequeTarjeta++;
                            break;
                        case 3:
                            vCabecera.vcc_banco3 = "TARJETA";
                            vCabecera.vcf_num_cheque3 = tarjeta.iNCuenta.ToString();
                            vCabecera.vcf_fec_cheque3 = tarjeta.dFecha;
                            vCabecera.vcn_monto_cheque3 = tarjeta.dImporte;
                            iChequeTarjeta++;
                            break;
                    }
                }


                //////guarda los cheques en las posiciones disponibles
                foreach (var cheque in VarEntorno.cCobranza.lCheque)
                {
                    if (cheque.dMonto > dMayorImp)
                    {
                        sConseptoSAT = "02";
                        sCuentaSAT = cheque.iCuenta.ToString();
                        dMayorImp = cheque.dMonto;
                    }

                    dTTPago += cheque.dMonto;
                    switch (iChequeTarjeta)
                    {
                        case 1:
                            vCabecera.vcc_banco = cheque.sBanco;
                            vCabecera.vcf_num_cheque = cheque.iNoCheque;
                            vCabecera.vcf_fec_cheque = Convert.ToDateTime(DateTime.Now.Date);
                            vCabecera.vcn_monto_cheque = cheque.dMonto;
                            iChequeTarjeta++;
                            break;
                        case 2:
                            vCabecera.vcc_banco2 = cheque.sBanco;
                            vCabecera.vcf_num_cheque2 = cheque.iNoCheque;
                            vCabecera.vcf_fec_cheque2 = Convert.ToDateTime(DateTime.Now.Date);
                            vCabecera.vcn_monto_cheque2 = cheque.dMonto;
                            iChequeTarjeta++;
                            break;
                        case 3:
                            vCabecera.vcc_banco3 = cheque.sBanco;
                            vCabecera.vcf_num_cheque3 = cheque.iNoCheque;
                            vCabecera.vcf_fec_cheque3 = Convert.ToDateTime(DateTime.Now.Date);
                            vCabecera.vcn_monto_cheque3 = cheque.dMonto;
                            iChequeTarjeta++;
                            break;
                    }
                }

                foreach (var boni in VarEntorno.cCobranza.lBonificacion)
                    dTTbonis += boni.boi_documento;
                

                if (dTTbonis > dMayorImp)
                {
                    sConseptoSAT = "27";
                    sCuentaSAT = "0000";
                }

                dTTPago += dTTbonis;
                    */
                #endregion
                
                //vCabecera.fcd_uuid = dDetalle.vcc_cadena_original;                
                sFormaPago = sConseptoSAT;

                if (VarEntorno.bEsDevolucion)
                    dCabecera.dcc_tipo = "D";
                else
                    dCabecera.dcc_tipo = "V";

                dCabecera.dcb_nuevo_documento = true;
                dCabecera.dcn_saldo = vCabecera.vcn_importe - dTTPago;
                dCabecera.dPagosVendedor = dTTPago;
                dCabecera.vcf_movimiento = DateTime.Now.Date;
                dCabecera.vcn_cliente = vCabecera.vcn_cliente;
                dCabecera.vcn_folio = vCabecera.vcn_folio;
                dCabecera.vcn_importe = vCabecera.vcn_importe;
                dCabecera.dcn_numero_pago = 1;
                dCabecera.dcn_numero_pago_base = 1;
               
               
                if (String.Format("{0:0.00}", VarEntorno.dImporteTotal) == String.Format("{0:0.00}", dTTPago)
                    && VarEntorno.cTipoVenta != 'R'
                    && dTTPago > 0M                    
                    )
                {
                    if ((sConseptoSAT == "01" || sConseptoSAT == "28" || sConseptoSAT == "04")
                    && lPagos.Count == 1)
                    {
                        sMetodoPago = "PUE";
                    }
                    else
                    {
                        sMetodoPago = "PPD";
                    }
                                       
                    dCabecera.dPagosVendedor = dTTPago;
                    ///// guarda cabecera y detalle doctos
                   // bDoctos = FnCabeceraDoc.GuardaCab(dCabecera);
                   // bDoctos = FnCabeceraDoc.ActualizaDoctoPago("", dDetalle);
                    bDocCab = true;
                    bDocDet = true;
                }
                else
                { 
                    sMetodoPago = "PPD";

                    if (VarEntorno.bSoloCobrar)
                    {
                        ///genera detalle
                        //bDoctos = FnCabeceraDoc.ActualizaDoctoPago("",dDetalle);   
                        
                            bDocDet = true;                        
                    }
                    else
                    {                        
                        if (VarEntorno.cTipoVenta != 'R')
                        {
                            ///genera cabecera   
                            dCabecera.dPagosVendedor = dTTPago;
                            //bDoctos = FnCabeceraDoc.GuardaCab(dCabecera);
                            bDocCab = true;                            
                        }
                        if (dTTPago > 0M)
                        {
                            ///genera detalle
                            //bDoctos = FnCabeceraDoc.ActualizaDoctoPago("", dDetalle);
                            bDocDet = true;                            
                        }
                        
                        sFormaPago = "99";
                    }
                }

                dDetalle.ddc_forma_pago = sConseptoSAT;

                vCabecera.vcn_monto_pago = dTTPago ;
                vCabecera.cmpc_metodopago = sMetodoPago;
                vCabecera.cfpc_formapago = sFormaPago;

                vCabecera.cmpc_clave_sat = sConseptoSAT;
                vCabecera.vcc_ctapgosat = sCuentaSAT;

                vCabecera.vcn_ruta = VarEntorno.iNoRuta;
                vCabecera.vcf_movimiento = DateTime.Now.ToShortDateString();
                vCabecera.vcc_clave = VarEntorno.sAlmacen;

                
                if (VarEntorno.bSoloCobrar)
                    iEstatus = 2;
                else
                    iEstatus = 1;

                if (VarEntorno.bAnticipoRelacionado)
                {
                    VarEntorno.cCobranza.dAnticipo.vcn_cliente = VarEntorno.vCliente.cln_clave;
                    VarEntorno.cCobranza.dAnticipo.vcn_folio = VarEntorno.cAnticipos.vcn_folio;
                    VarEntorno.cCobranza.dAnticipo.vcf_movimiento = VarEntorno.cAnticipos.anfMovimiento;
                    VarEntorno.cCobranza.dAnticipo.vcn_folio_cabecera = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                    VarEntorno.cCobranza.dAnticipo.vcf_movimiento_cabecera = DateTime.Now.Date;
                    VarEntorno.cCobranza.dAnticipo.vcn_monto_pago =  VarEntorno.cAnticipos.dMontoPago;
                    VarEntorno.cCobranza.dAnticipo.ddn_numero_pago = 1;
                    VarEntorno.cCobranza.dAnticipo.vcc_cadena_original = "ANTICIPO";
                    VarEntorno.cCobranza.dAnticipo.ddn_cantidad_pagos = 1;
                    VarEntorno.cCobranza.dAnticipo.ddc_forma_pago = "99";
                }

                return FnCabeceraVen.GuardaNormalCompleta(vCabecera, iEstatus,bDocCab,bDocDet);
              
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        /*****NUMERO DE PAGO ***/
        public int NumeroPagos()
        {
            try
            {
                int nPagos = 0;

                if (pEfectivo.vcn_monto > 0)
                    nPagos++;

                nPagos += lpBonificacion.Count;
                nPagos += lpCheques.Count;
                nPagos += lpTarjeta.Count;
                nPagos += lpTransferencia.Count;

                return nPagos;
            }
            catch
            { return -1; }
        }

        /*****FORMA DE PAGO ***/
        public string FormaPago()
        {
            try
            {
                string sConseptoSAT = "99";
                decimal dMayorImp = 0M, dTTbonis=0M;

                if (pEfectivo.vcn_monto > 0)
                {                    
                    sConseptoSAT = "01";
                    dMayorImp = VarEntorno.cCobranza.pEfectivo.vcn_monto;
                }

                foreach (var transferencia in VarEntorno.cCobranza.lpTransferencia)
                {
                    if (transferencia.vcn_monto > dMayorImp)
                    {
                        sConseptoSAT = transferencia.cfpc_formapago;
                        dMayorImp = transferencia.vcn_monto;
                    }
                }

                foreach (var tarjeta in VarEntorno.cCobranza.lpTarjeta)
                {
                    if (tarjeta.vcn_monto > dMayorImp)
                    {
                        sConseptoSAT = tarjeta.cfpc_formapago;
                        dMayorImp = tarjeta.vcn_monto;
                    }                    
                }

                foreach (var cheque in VarEntorno.cCobranza.lpCheques)
                {
                    if (cheque.vcn_monto > dMayorImp)
                    {
                        sConseptoSAT = cheque.cfpc_formapago;
                        dMayorImp = cheque.vcn_monto;
                    }
                }

                foreach (var bonificacion in VarEntorno.cCobranza.lpBonificacion)
                {                    
                    dTTbonis += bonificacion.vcn_monto;
                }

                if (dTTbonis > dMayorImp)
                {
                    sConseptoSAT = "27";
                }

                return sConseptoSAT;
            }
            catch
            { return ""; }
        }

        public bool GuardaActualizaPago(string sTipo,bool bNuevo,int iIndex
                                ,string sFormaPago,string sBanco,string sTipoTarjeta,decimal dMonto
                                ,string sReferencia,string sCuenta,string sAutorizacion)
        {
            try
            {
                
                venta_pagos cPago = new venta_pagos();
                cPago.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                cPago.vcn_cliente = VarEntorno.vCliente.cln_clave;
                cPago.vcf_movimiento = DateTime.Now.ToShortDateString();
                cPago.vcn_ruta = VarEntorno.iNoRuta;
                cPago.vpc_descripcion = sTipo;
                cPago.cfpc_formapago = sFormaPago;
                cPago.vcc_banco = sBanco;
                cPago.vpc_tipotarjeta = sTipoTarjeta;
                cPago.vcn_monto = dMonto;
                cPago.vcc_referencia = sReferencia;
                cPago.vpc_nocuenta = sCuenta;
                cPago.vpc_autorizacion = sAutorizacion;
                
                switch (sTipo)
                {
                    case "BONIFICACION":
                        lpBonificacion.Add(cPago);
                        break;
                    case "TRANSFERENCIA":
                        if (bNuevo)
                            lpTransferencia.Add(cPago);
                        else
                        {
                            lpTransferencia.Insert(iIndex, cPago);
                            lpTransferencia.RemoveAt(iIndex + 1);
                        }
                        break;
                    case "TARJETA":
                        if (bNuevo)
                            lpTarjeta.Add(cPago);
                        else
                        {
                            lpTarjeta.Insert(iIndex, cPago);
                            lpTarjeta.RemoveAt(iIndex + 1);
                        }
                        break;
                    case "CHEQUE":
                        if (bNuevo)
                            lpCheques.Add(cPago);
                        else
                        {
                            lpCheques.Insert(iIndex, cPago);
                            lpCheques.RemoveAt(iIndex + 1);
                        }
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        /****  funcione que obtiene las ventas del dia del cliente ******/
        public bool VentasClienteHoy()
        {
            try
            {
                dVentasCliente = FnCabeceraVen.VentasImportes(VarEntorno.vCliente.cln_clave);
                if (dVentasCliente == -1M)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        /****  funcione que obtiene los pagos del dia del cliente ******/
        public bool PagosVentasClienteHoy()
        {
            try
            {
                dPagosCliente = FnCabeceraVen.PagosImportesdeVentas(VarEntorno.vCliente.cln_clave);
                if (dPagosCliente == -1M)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        /*
        public decimal Saldo()
        {
            try
            {
                decimal dSaldo = FnCabeceraVen.SaldoFinal(VarEntorno.vCliente);
               
                return dSaldo;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -9999;
            }
        }
        */
        /***    MARCA AL CLIENTE  QUE REALIZO  UN PAGO *****/
        /*
        public bool EstatusClientePagoPro()
        {
            try
            {
                if (VarEntorno.bSoloCobrar)
                   // return (new clientes_estatusSR().fnActualizaPagoProgramado(VarEntorno.vCliente.cln_clave));
                else
                    return true;

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        */
        public decimal fnImporteTotalAntFolio()
        {
            try
            {
                return FnDetalleVen.fnImporteTotalxClienteconExclucion(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public int NumeroTickets()
        {
            try
            {
                return new facturasventasSR().fnListaFacturas(VarEntorno.vCliente.cln_clave).Count;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public string LogroDesafio()
        {
            string sMensaje = "";
            int iCuota = new cuotaDiaSR().CantidadDesafio("CERVEZA");
            int iVenta = FnDetalleVen.fnBuscaProductoVendidoxCategoria("'1','2'");
            int iResultado = iCuota - iVenta;


            int iPorcentaje = (iVenta * 100) / (iCuota==0?1: iCuota);

            if (iPorcentaje >= 0 && iPorcentaje < 50)
                sMensaje = "Muy bien vamos por los siguientes " + iResultado;

            if (iPorcentaje >= 50 && iPorcentaje < 80)
                sMensaje = "Grandioso  estamos cerca " + iResultado;

            if (iPorcentaje >= 80 && iPorcentaje < 100)
                sMensaje = "Excelente faltan pocos " + iResultado;

            if (iPorcentaje >= 100 )
                sMensaje = "Perfecto meta cumplida " + iResultado;

            return sMensaje;
        }
    }
    /*
    
    public class Cheque
    {
        public string sBanco;
        public string iNoCheque;
        public decimal dMonto;
        public int iCuenta;

        public Cheque()
        {
            sBanco = "";
            iNoCheque = "";
            dMonto = 0;
            iCuenta = 0;
        }
    }

    
    public class Tarjeta
    {
        public decimal dCantidad;
        public decimal dComicion;
        public decimal dImporte;
        public string sAutorizacion;
        public string iNCuenta;
        public int iTipoTarjeta;
        public DateTime dFecha;

        public Tarjeta()
        {
            dCantidad = 0;
            dComicion = 0;
            dImporte = 0;

            iNCuenta = "";
            iTipoTarjeta = 0;
        }
    }
    
    public class Bonificacion
    {
        public string boc_folio;
        public decimal boi_documento;
        public string boc_tipo;

        public Bonificacion()
        {
            boc_folio = "";
            boi_documento = 0;
            boc_tipo = "";
        }
    }
   
    public class Transferencia
    {
        public decimal dCantidad;
        public int iCuenta;

        public Transferencia()
        {
            dCantidad = 0;
            iCuenta = 0;
        }
    }
    */
}
