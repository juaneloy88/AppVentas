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
    public class fnResumenDia
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        #region Se obtiene la marca a afectar
        public async Task<string> consultaMarcaAfecta(string sMarca)
        {
            await sl.WaitAsync();

            try
            {                
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sqlQuery = "select arc_afecta from productos where arc_clave = ?";

                    return conn.ExecuteScalar<string>(sqlQuery, sMarca);
                }                
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Se obtiene la marca a afectar

        #region Se obtiene la descripción de la marca a afectar
        public async Task<string> consultaDescripcionAfecta(string sMarca)
        {
            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sqlQuery = "select ard_descripcion from productos where arc_clave = ?";

                    return conn.ExecuteScalar<string>(sqlQuery, sMarca);
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Se obtiene la descripción de la marca a afectar


        
    }
}