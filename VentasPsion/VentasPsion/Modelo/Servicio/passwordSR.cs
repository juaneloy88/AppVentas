
using SQLite;
using System;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;


namespace VentasPsion.Modelo.Servicio
{
    class passwordSR
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        public async Task<bool> PassAjustes(string sPassword)
        {
            bool sRespuesta = false;

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from password where ppc_password_ajustes = '" + sPassword + "'";

                    var lLista = conn.Query<password>(sQuery);

                    if (lLista.Count == 1)                    
                        sRespuesta = true;                    
                    else                    
                        sRespuesta = false;
                    
                }

                return sRespuesta;
            }
            catch 
            {
                return false;
            }
            finally
            {
                sl.Release();
            }
        }
    }
}
