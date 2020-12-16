using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class conseptoSinVentaSR
    {
        conexionDB cODBC = new conexionDB();

        public List<conseptos_no_venta> ListaConseptos()
        {
            try
            {
                List<conseptos_no_venta> LCons = new List<conseptos_no_venta>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT svn_clave ,svc_descripcion
                                    FROM conseptos_no_venta                
                                    ORDER BY svn_clave";


                    LCons = conn.Query<conseptos_no_venta>(sQuery);
                }

                return LCons;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }
    }
}
