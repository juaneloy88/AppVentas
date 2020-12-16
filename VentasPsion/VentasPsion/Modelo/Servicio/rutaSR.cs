using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class rutaSR
    {
        conexionDB cODBC = new conexionDB();


        //* se obtiene el siguiente folio para inicializar un ticket  *//
        public int GetFolio()
        {
            try
            {
                int iFolio;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select run_folio from ruta  ";
                    iFolio = conn.ExecuteScalar<int>(sQuery);
                }

                if (iFolio == 0)
                    iFolio++;

                return iFolio;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -999;
            }
        }

        public bool IncFolio()
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        @"UPDATE ruta 
                               SET run_folio = run_folio + 1";

                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();

                    //if (conn.Update(sQuery) == -1)
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

        
        public string GetAlmacen()
        {
            try
            {
                string sAlmacen="";

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select alc_clave from ruta  ";
                    sAlmacen = conn.ExecuteScalar<string>(sQuery);
                }

                return sAlmacen;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return "";
            }
        }

    }
}
