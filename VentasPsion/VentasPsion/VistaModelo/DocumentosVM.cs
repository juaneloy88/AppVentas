using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
      class DocumentosVM
    {
        public List<documentos_cabecera> lDoctosCabecera = new List<documentos_cabecera>();
        //public documentos_detalle lDoctosDetalle = new documentos_detalle();

        private documentos_cabeceraSR FnCabecera = new documentos_cabeceraSR();
        // private fnDocumentos_detalle FnDetalle = new fnDocumentos_detalle();

        public anticipos anticipo = new anticipos();
        public List<anticipos> Lanticipos = new List<anticipos>() ;

        public facturasventasSR Facturas = new facturasventasSR();

        public bool lDoctos()
        {
            try
            {
                lDoctosCabecera = FnCabecera.ListaDocCab();
                if (lDoctosCabecera is null)
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

        public string ValidaAnticiposRelacionados()
        {
            string sRespuesta = string.Empty;

            try
            {
                sRespuesta = FnCabecera.ValidaRelacionAnticipos();
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }

            return sRespuesta;
        }

        public List<documentos_cabecera> GetDoctActivos()
        {
            List<documentos_cabecera> LDoct = new List<documentos_cabecera>();

            try
            {
                LDoct = FnCabecera.ListaDocCab();
            }
            catch (Exception ex)
            {
                LDoct = null;
            }

            return LDoct;
        }

        public bool UltimoDoc()
        {
            try
            {
                return Facturas.UltimaFactura();
                    

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public int DoctosPendientesR()
        {
            try
            {
                return FnCabecera.ListaDocCabRepPend().Count;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public int N_Pago_Factura(string sFolio)
        {
            try
            {
                return FnCabecera.ListaDocCab().Where(x=> x.vcn_folio == sFolio).Select(y=>y.dcn_numero_pago).FirstOrDefault();
            }
            catch
            {
                return 0;
            }

        }
        
        public bool Guarda_Anticipo()
        {
            try
            {
                return new anticipoSR().GuardaAnticipo(anticipo);
            }
            catch
            {
                return false;
            }
        }

        public bool GeneraAnticiposxDev()
        {
            try
            {
                List<FacturasVenta> lFacturas = Facturas.fnListaFacturas(VarEntorno.vCliente.cln_clave).Where(x => x.fvc_tipo == "CONTADO").ToList();
                anticipos ant = new anticipos();

                foreach (var fac in lFacturas)
                {
                    ant = new anticipos();
                    ant.cln_clave = VarEntorno.vCliente.cln_clave;
                    ant.vcn_folio = fac.vcn_folio;
                    ant.vcf_movimiento = fac.vcf_movimiento;
                    ant.ann_monto_pago = fac.fvn_pagos;
                    ant.ann_cantidad_pagos = 1;    /////////////////          PENDIENTE
                    ant.anc_forma_pago = "27";     /////////////////         PENDIENTE
                    ant.anb_nuevo = true;
                    ant.anb_relacionado = false;
                    Lanticipos.Add(ant);
                }
                return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        
        #region funciones comentadas
        /*
        public double SaldoPedidoReparto(int iCliente)
        {
            try
            {
                return FnCabecera.SaldoReparto(iCliente);
            }
            catch (Exception ex)
            {
                return - 1;
            }
        }
        */
        /*
        public bool GuardaCabDet(documentos_cabecera dcab, documentos_detalle ddet)
        {
            try
            {                
                    return FnCabecera.GuardaCabDet(dcab, ddet);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        */
        /*
        public bool ActualizaDocumento(string sUUID, documentos_detalle lDoctosDetalle)
        {
            try
            {
                //if (lDoctosCabecera.Count > 0)
                    return FnCabecera.ActualizaDoctoPago(sUUID, lDoctosDetalle);
                //else
                  //  return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        */
        /*
        public bool GuardaCab(documentos_cabecera dcab)
        {
            try
            {                
                    return FnCabecera.GuardaCab(dcab);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        */
        /*
        public bool ActualizaCabDev(documentos_cabecera dcab, documentos_detalle ddet)
        {
            try
            {
                
                    return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        */
        #endregion
    }
}
