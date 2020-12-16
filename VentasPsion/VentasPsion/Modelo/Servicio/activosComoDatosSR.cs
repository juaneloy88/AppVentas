using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    public class activosComoDatosSR
    {
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);


        public activosComoDatosSR()
        {
            // constructor
        }



        public List<activos_comodatos> ObtenerActivosComoDatos(int cln_clave)
        {


            try
            {
                List<activos_comodatos> oListaActivos = new List<activos_comodatos>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery;
                    if (cln_clave.Equals(0))
                    {
                         sQuery = @"SELECT *
                                    FROM activos_comodatos";
                    }
                    else
                    {
                         sQuery = @"SELECT *
                                    FROM activos_comodatos WHERE cln_clave=" + cln_clave;
                    }
                 


                    oListaActivos = conn.Query<activos_comodatos>(sQuery);
                }

                return oListaActivos;

            }
            finally
            {
                sl.Release();
            }
        }
    }
}
