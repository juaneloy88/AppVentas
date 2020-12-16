using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    public class kpisSR
    {
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);
        //public fnKpis()
        //{
        //    // constructor
        //}

        public List<kpis> ObtenerListaKpis()
        {


            try
            {
                List<kpis> oListaKpis = new List<kpis>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT *
                                    FROM kpis";


                    oListaKpis = conn.Query<kpis>(sQuery);
                }

                return oListaKpis;

            }
            finally
            {
                sl.Release();
            }
        }
    }
}
