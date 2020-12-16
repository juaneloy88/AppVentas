using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    class opcionesAppSR
    {
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);


        public List<opciones_app> ObtenerListaOpciones()
        {
            try
            {
                List<opciones_app> ListaOpciones = new List<opciones_app>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT *
                                    FROM opciones_app";

                    ListaOpciones = conn.Query<opciones_app>(sQuery);
                }

                return ListaOpciones;

            }
            finally
            {
                sl.Release();
            }
        }
    }
}
