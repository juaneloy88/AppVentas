using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class fnPedidoSugerido
    {
        conexionDB cODBC = new conexionDB();
        

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        public async Task<List<pedido>> ListaSugerido(int iCliente)
        {
            string sQuery = string.Empty;

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    sQuery = @"select * from pedido where cln_clave = "+ iCliente;
                    var vListaCalculada = conn.Query<pedido>(sQuery);

                    return vListaCalculada;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
            finally
            {
                sl.Release();
            }
        }


        public bool ActualizaInventario(int iCliente, string sClave, int iExistencia)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
             "UPDATE pedido SET " +
                          "  pen_inv_cantidad =  " + iExistencia + " " +
                            " ,pen_sug_cantidad = pen_prm_cantidad -  pen_inv_cantidad " +
                          "WHERE (cln_clave = " + iCliente + ") " +
                          "AND (arc_clave = '" + sClave + "');";

                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();

                    if (iResultado >= 1)
                        return true;
                    else
                        return false;
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
