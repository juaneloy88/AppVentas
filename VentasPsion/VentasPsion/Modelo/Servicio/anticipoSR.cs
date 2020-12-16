using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class anticipoSR
    {
        conexionDB cODBC = new conexionDB();
        
        public bool GuardaAnticipo(anticipos vAnticipo)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    conn.Insert(vAnticipo);
                }

                return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        
        public List<anticipos> ListaAnticipos()
        {
            try
            {
                List<anticipos> LAnticipos = new List<anticipos>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT * from anticipos where cln_clave = ? and anb_relacionado = '0' order by vcf_movimiento";

                    LAnticipos = conn.Query<anticipos>(sQuery, VarEntorno.vCliente.cln_clave);
                }
                return LAnticipos;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        public bool UpdateRelacionado(string sFolioAnticipo,string sFolioCabecera)
        {            
            int iResultado = 0;
            string sQuery = string.Empty;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {

                    #region Se relaciona el anticipo con la cabecera
                    sQuery = "update anticipos " +
                             "set anb_relacionado = '1' " +
                             "where cln_clave = " + VarEntorno.vCliente.cln_clave + " " +
                             "and vcn_folio = '" + sFolioAnticipo + "'";

                    SQLiteCommand command1 = conn.CreateCommand(sQuery);
                    command1.ExecuteNonQuery();
                    #endregion Se realciona el anticipo con la cabecera

                    #region Se inserta el documento detalle
                    iResultado = conn.Insert(VarEntorno.cCobranza.dDetalle);
                    #endregion Se inserta el documento detalle

                    if (iResultado > 0 && VarEntorno.cCobranza.dDetalle.vcn_folio != null)
                    {
                        sQuery = "update documentos_cabecera set dcn_numero_pago=coalesce(dcn_numero_pago,0)+1,dPagosVendedor = dPagosVendedor+? where vcn_folio = ? and vcn_cliente =? ";

                        SQLiteCommand command2 = conn.CreateCommand(sQuery, VarEntorno.cCobranza.dDetalle.vcn_monto_pago
                                                           , sFolioCabecera
                                                           , VarEntorno.vCliente.cln_clave);                        
                        iResultado = command2.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        /*
        public static string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = param.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }
        */
    }
}
