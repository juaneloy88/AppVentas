using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    public class volumenVentasSR
    {
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);
        public volumenVentasSR()
        {
            // constructor
        }
        public List<volumen_ventas> ObtenerVolumenVentas()
        {


            try
            {
                List<volumen_ventas> oListaRetoDia = new List<volumen_ventas>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT *
                                    FROM volumen_ventas";


                    oListaRetoDia = conn.Query<volumen_ventas>(sQuery);
                }

                return oListaRetoDia;

            }
            finally
            {
                sl.Release();
            }
        }
    }
}
