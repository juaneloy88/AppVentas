using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class telefonos_clientesSR
    {
        conexionDB cODBC = new conexionDB();

        public int GuardaTelefono(telefonos_clientes sTelefono)
        {
            try
            {
                int i = 0;
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    i=conn.Insert(sTelefono);
                }
                return i;
            }
            catch (Exception ex)
            {
                
                return -1;
            }
        }

        public List<telefonos_clientes> ListaRegTel()
        {
            try
            {
                List<telefonos_clientes> LRegistros = new List<telefonos_clientes>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT *
                                    FROM telefonos_clientes 
                                    where  cln_clave = " + VarEntorno.vCliente.cln_clave+ @"
                                        and tcb_estatus 
                                    ORDER BY tcc_nombre";

                    LRegistros = conn.Query<telefonos_clientes>(sQuery);
                }

                return LRegistros;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }
    }
}
