using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class clientes_competenciaSR
    {
        conexionDB cODBC = new conexionDB();
        
        public bool Guardar(clientes_competencia cliente)
        {
            try
            {
                int iResultado = 0;
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    iResultado=  conn.Insert(cliente);
                }

                if (iResultado > 0)
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
    }
}
