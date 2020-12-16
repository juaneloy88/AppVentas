using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    public class cuotaDiaSR
    {
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);
        public cuotaDiaSR()
        {
            // constructor

        }

        public List<reto_diario> ObtenerListaRetoDia()
        {


            try
            {
                List<reto_diario> oListaRetoDia = new List<reto_diario>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT *
                                    FROM reto_diario";


                    oListaRetoDia = conn.Query<reto_diario>(sQuery);
                }

                return oListaRetoDia;

            }
            finally
            {
                sl.Release();
            }
        }


        ///rdc_nombre
        public int CantidadDesafio(string sDesafios)
        {


            try
            {
                // List<reto_diario> oListaRetoDia = new List<reto_diario>();
                int iCant = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT rdn_cantidad
                                    FROM reto_diario
                                    where rdc_nombre = ? ";

                    iCant = conn.ExecuteScalar<int>(sQuery, sDesafios);
                }

                return iCant;

            }
            catch
            {
                return 0;
            }
            finally
            {
                sl.Release();
            }
        }

    }
}
