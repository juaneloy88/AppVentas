
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class existenciaSR
    {
        conexionDB cODBC = new conexionDB();


        //* busca la existencia de un producto por medio de la clave  *//
        public int BuscarExistencia(string sClave)
        {
            try
            {
                int iInventario=0,iVentas=0, iExistencia=0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select exn_existencia from existencia WHERE arc_clave in (select arc_afecta from productos where arc_clave = ?)  ";
                    iInventario = conn.ExecuteScalar<int>(sQuery, sClave);
                        sQuery = "Select coalesce(sum(vdn_venta),0) from venta_detalle WHERE vdn_producto in (select arc_afecta from productos where arc_clave = ?)  ";
                    
                    iVentas = conn.ExecuteScalar<int>(sQuery, sClave);

                    iExistencia = iInventario - iVentas;
                }

                return iExistencia;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -999;
            }
        }
    }
}
