using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class documentos_cabeceraSR
    {
        conexionDB cODBC = new conexionDB();

        public List<documentos_cabecera> ListaDocCab()
        {
            try
            {
                List<documentos_cabecera> LCom = new List<documentos_cabecera>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT   * 
                                    FROM documentos_cabecera  
                                    where vcn_cliente = ?
                                     and dcn_saldo > dPagosVendedor
                                        and dcc_tipo != 'D'

                                    ORDER BY vcf_movimiento ";

                    LCom = conn.Query<documentos_cabecera>(sQuery, VarEntorno.vCliente.cln_clave);

                    foreach (var doc in LCom)                    
                        doc.dcn_saldo = doc.dcn_saldo - doc.dPagosVendedor;                    

                }
                return LCom;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        public List<documentos_cabecera> ListaDocCabRepPend()
        {
            try
            {
                List<documentos_cabecera> LCom = new List<documentos_cabecera>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT   * 
                                    FROM documentos_cabecera  
                                    where vcn_cliente = ?
                                     and dcn_saldo > dPagosVendedor
                                        and dcc_tipo != 'D'
                   and length(vcn_folio)> 5
and vcf_movimiento in (select max(vcf_movimiento) from FacturasVenta)
                                    ORDER BY vcf_movimiento ";

                    LCom = conn.Query<documentos_cabecera>(sQuery, VarEntorno.vCliente.cln_clave);

                    foreach (var doc in LCom)
                        doc.dcn_saldo = doc.dcn_saldo - doc.dPagosVendedor;

                }

                return LCom;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        public string ValidaRelacionAnticipos()
        {
            string sRespuesta = "Ok";

            try
            {
                List<documentos_cabecera> LCom = new List<documentos_cabecera>();
                List<anticipos> LAnt = new List<anticipos>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT   * 
                                    FROM documentos_cabecera  
                                    where dcn_saldo > dPagosVendedor
                                        and dcc_tipo != 'D'
                                    ORDER BY vcn_cliente";
                    LCom = conn.Query<documentos_cabecera>(sQuery);

                    foreach (var doc in LCom)
                    { 
                        doc.dcn_saldo = doc.dcn_saldo - doc.dPagosVendedor;

                        if (sRespuesta == "Ok")
                        {
                            sQuery = "select * from anticipos order by cln_clave";
                            LAnt = conn.Query<anticipos>(sQuery);

                            foreach (var Ant in LAnt)
                            {
                                if (doc.vcn_cliente == Ant.cln_clave)
                                {
                                    if (doc.dcn_saldo >= Ant.ann_monto_pago)
                                    {
                                        if (Ant.anb_relacionado)
                                            sRespuesta = "Ok";
                                        else
                                        {
                                            sRespuesta = "Falta Relacionar Anticipo del Cliente:" + doc.vcn_cliente.ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else                        
                            break;                        
                    }
                }

                return sRespuesta;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }           
        }

        
    }
}
