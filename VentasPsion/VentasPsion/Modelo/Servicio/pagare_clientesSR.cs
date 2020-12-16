using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class pagare_clientesSR
    {
        conexionDB cODBC = new conexionDB();

        public pagare_clientes pagare_clientesFN(int sClave)
        {
            try
            {
                pagare_clientes pg_cliente = new pagare_clientes();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    pg_cliente = conn.Table<pagare_clientes>().Where(i => i.cln_clave == sClave).FirstOrDefault();
                                        
                }

                return pg_cliente;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return new pagare_clientes();
            }
        }

        public bool fnPagareCliente(int sClave)
        {
            try
            {
                bool bEntregado = false;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from pagare_clientes WHERE cln_clave = ? ";
                    int valor = conn.ExecuteScalar<int>(sQuery, sClave);

                    if (valor > 0)
                    {
                        sQuery = "Select pcb_entregado from pagare_clientes WHERE cln_clave = ? ";
                        bEntregado = conn.ExecuteScalar<bool>(sQuery, sClave.ToString());
                    }
                }

                /* if (bEntregado)
                     return 1;
                 else
                     return 0;
                     */
                return bEntregado;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public bool fnActualizaPagare(int iCliente, bool bEstatus)
        {
            try
            {
                int iValor = 2;
                if (bEstatus)
                    iValor = 1;
                else
                    iValor = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        "update pagare_clientes set  pcb_entregado   = '" + iValor + "' where cln_clave = " + iCliente.ToString();
                    /*,
                              pcb_modificado = 'True'*/

                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();

                    //if (conn.Update(sQuery) == -1)
                    bool bResultado;

                    if (iResultado >= 1)
                        bResultado = true;
                    else
                        bResultado = false;

                    return bResultado;
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
