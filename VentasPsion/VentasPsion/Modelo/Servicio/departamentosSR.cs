using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;

using System.Threading;

namespace VentasPsion.Modelo.Servicio
{
    public class departamentosSR
    {
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);        

        /****************************/
        /*********METODOS************/
        /****************************/
        
        public List<departamentos> obtenerDepartamentos()
        {
            try
            {
                List<departamentos> oListaEncuesta = new List<departamentos>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT * FROM departamentos";
                    oListaEncuesta = conn.Query<departamentos>(sQuery);
                }

                return oListaEncuesta;

            }
            finally
            {
                sl.Release();
            }
        }

        /****************************/
        /*********METODOS************/
        /****************************/

    }
}
