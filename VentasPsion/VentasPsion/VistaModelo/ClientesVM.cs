using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    class ClientesVM
    {
        private DocumentosVM Doctos = new DocumentosVM();

        private clientesSR Cliente = new clientesSR();
        private clientes_estatusSR Estatus = new clientes_estatusSR();
        private pagare_clientesSR Pagare = new pagare_clientesSR();
        private fnVentaCabecera vCabecera = new fnVentaCabecera(); 
        private fnVentaDetalle vDetalle = new fnVentaDetalle();
        private venta_detalleSR VenDetSR = new venta_detalleSR();

        public string BuscaCliente(string sID)
        {
            try
            {
                return Cliente.ValidaCliente(sID);
            }
            catch
            {
                return "";
            }
        }

        public async Task<clientes> DatosCliente(string sID)
        {
            try
            {
                return await Cliente.DatosCliente(sID);
            }
            catch
            {
                return null;
            }
        }

        public decimal VentaCliente(int iCliente)
        {
            try
            {
                return VenDetSR.fnImporteTotalxCliente(iCliente);
            }
            catch
            {
                return 0;
            }
        }


        public int  Antiguedad_documentos()
        {
            try
            {
                int nDiascredito = VarEntorno.vCliente.clc_dias_credito;
                int nDiasCreditoCero = VarEntorno.vCliente.clc_dias_credito_cero;

                int nDctCero = 0; int nDctFac = 0;

                if (Doctos.lDoctos() == false)
                    return 0;
                else
                {
                    int x = 0;
                    foreach (var docto in Doctos.lDoctosCabecera)
                    {

                        if (docto.vcn_folio.Length <= 5)
                        {
                            //x = DateTime.Now.ToString("dd/MM/yyyy").CompareTo(docto.vcf_movimiento);
                            x = docto.vcf_movimiento.Date.Subtract(DateTime.Now).Days;
                            if (Math.Abs(x) > nDiasCreditoCero)
                                nDctCero++;
                        }
                        else
                        {
                            //x = DateTime.Now.ToString("dd/MM/yyyy").CompareTo(docto.vcf_movimiento);
                            x = docto.vcf_movimiento.Date.Subtract(DateTime.Now).Days;
                            if (Math.Abs(x) > nDiascredito)
                                nDctFac++;
                        }
                    }
                    /*
                    if ((nDctCero + nDctFac) > 0)
                    {
                        return false;
                    }
                    else
                        return true;
                        */
                    return nDctCero + nDctFac;
                }
            }
            catch
            {
                return -1;
            }
        }

        public bool candado_saldo_ant()
        {
            try
            {
                bool bAplica = false;

                if (VarEntorno.oOpciones_app.opciones[1].oab_valor == true)
                {
                    switch (VarEntorno.vCliente.clc_cobrador)
                    {
                        default:
                            bAplica = false;
                            break;
                        case "":
                            bAplica = true;
                            break;
                        case "RP":
                            if (VarEntorno.cTipoVenta == 'R')
                                bAplica = false;
                            else
                                bAplica = true;
                            break;
                        case "A":
                            bAplica = false;
                            break;
                        case "P":
                            if (VarEntorno.cTipoVenta == 'P')
                                bAplica = false;
                            else
                                bAplica = true;
                            break;
                        case "R":
                            if (VarEntorno.cTipoVenta == 'R')
                                bAplica = false;
                            else
                                bAplica = true;
                            break;
                    }
                }
                else
                {
                    bAplica = true;
                }

                if (bAplica == false)
                {
                    if (VarEntorno.Saldo(VarEntorno.vCliente) > 0 && vCabecera.PagosImportes(VarEntorno.vCliente.cln_clave) < 1)
                    {
                        List<venta_cabecera> sFolios = vCabecera.VentasCabeceras(VarEntorno.vCliente.cln_clave, "$");
                        if (sFolios.Count > 0)
                            bAplica = true;
                        else
                            bAplica = false;
                    }
                    else
                    {
                        bAplica = true;
                    }
                }


                if (VarEntorno.bEsTeleventa == true)
                    bAplica = true;

                return bAplica;
            }
            catch 
            {
                //DisplayAlert("Aviso Can_Sal_Ant", ex.Message.ToString(), "OK");
                return false;
            }
        }

        #region ESTATUS
        public bool traePagare(int iCliente)
        {
            return Pagare.fnPagareCliente(iCliente);
        }
        
        public bool traeClienteFoco(int iCliente)
        {
            return Estatus.fnFocoCliente(iCliente);
        }

        public bool traeCoolerCliente(int iCliente)
        {
            return Estatus.fnCoolerCliente(iCliente);
        }

        public bool ActualizaPagare(int iCliente, bool bEstatus)
        {
            return Pagare.fnActualizaPagare(iCliente, bEstatus);
        }

        public bool ActualizaCooler(int iCliente, bool bEstatus)
        {
            return Estatus.fnActualizaCooler(iCliente, bEstatus);
        }

        public bool ActualizaFoco(int iCliente, bool bEstatus)
        {
            return Estatus.fnActualizaFoco(iCliente, bEstatus);
        }
        #endregion
    }
}
