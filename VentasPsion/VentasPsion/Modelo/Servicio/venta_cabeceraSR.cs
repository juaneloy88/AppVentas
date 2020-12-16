using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class venta_cabeceraSR
    {
        conexionDB cODBC = new conexionDB();

        //obtener los cabeceras de ticket por Tipo mov para reimpresion de ticket de venta o pago
        public List<venta_cabecera> VentasCabeceras(int sClave, string sTipoMov)
        {
            try
            {

                int iRegistros = 0;

                List<string> sFolios = new List<string>();
                List<venta_cabecera> vLista = new List<venta_cabecera>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ?  and vcc_tipo_pago= ?";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, sTipoMov);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select * from venta_cabecera WHERE  vcn_cliente = ?   and vcc_tipo_pago= ?";
                        vLista = conn.Query<venta_cabecera>(sQuery, sClave, sTipoMov);
                        /*
                        foreach (var cabecera in vLista)
                            sFolios.Add(cabecera.vcn_folio);
                            */
                    }

                }

                return vLista;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        public decimal DevolucionesImportes(int sClave, int iFolio)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago== 'D'  ";
                    if (iFolio != 0)
                        sQuery += " and CAST(vcn_folio as INTEGER) <= ? ";

                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, iFolio);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_importe) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago== 'D'   ";

                        if (iFolio != 0)
                            sQuery += " and CAST(vcn_folio as INTEGER) <= ? ";

                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave, iFolio);
                    }
                }

                return iImporteTotal;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public decimal VentasImportes(int sClave, int iFolio)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago!= 'D'  ";

                    if (iFolio != 0)
                        sQuery += " and CAST(vcn_folio as INTEGER) <= ? ";

                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, iFolio);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_importe) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago!= 'D'  ";

                        if (iFolio != 0)
                            sQuery += " and CAST(vcn_folio as INTEGER) <= ? ";

                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave, iFolio);
                    }
                }

                return iImporteTotal;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public decimal PagosImportes(int sClave, int iFolio)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ?  ";

                    if (iFolio != 0)
                        sQuery += " and CAST(vcn_folio as INTEGER) <= ? ";

                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, iFolio);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_monto_pago) from venta_cabecera WHERE  vcn_cliente = ?  ";

                        if (iFolio != 0)
                            sQuery += " and CAST(vcn_folio as INTEGER) <= ? ";

                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave, iFolio);
                    }
                }

                return iImporteTotal;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public decimal SaldoFinal(clientes cCliente, int iFolio)
        {
            try
            {
                decimal dSaldoFinal = 0;
                decimal dVentas = VentasImportes(cCliente.cln_clave, iFolio);
                decimal dDevoluciones = DevolucionesImportes(cCliente.cln_clave, iFolio);
                decimal dPagos = PagosImportes(cCliente.cln_clave, iFolio);

                if (dVentas == -1)
                    dSaldoFinal = -999;
                else
                {
                    if (dPagos == -1)
                        dSaldoFinal = -999;
                    else
                    {
                        if (dDevoluciones == -1)
                            dSaldoFinal = -999;
                        else
                        {
                            if (VarEntorno.cTipoVenta == 'R')
                            {
                                dSaldoFinal = cCliente.cln_saldo - dPagos - dDevoluciones;
                            }
                            else
                            {
                                dSaldoFinal = cCliente.cln_saldo + dVentas - dPagos;
                                if (VarEntorno.cTipoVenta == 'A')
                                    dSaldoFinal -=  dDevoluciones;
                            }
                        }
                    }
                }

                return Math.Round(dSaldoFinal, 2);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -999;
            }
        }
    }
}
