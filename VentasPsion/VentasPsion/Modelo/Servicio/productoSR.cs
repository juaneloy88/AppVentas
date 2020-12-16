using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class productoSR
    {
        conexionDB cODBC = new conexionDB();


        //* busca las propiedades de un producto por medio de la clave  *//
        public productos BuscarProducto(string sClave)
        {
            try
            {
                productos pProducto = new productos();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    pProducto = conn.Table<productos>().Where(i => i.arc_clave == sClave).FirstOrDefault();
                    /* 
                    var sQuery = "Select * from productos WHERE arc_clave = ? ";
                     pProducto = conn.ExecuteScalar<productos>(sQuery, sClave);
                    */
                     
                }

                return pProducto;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }
        /*
        public bool ProductoContado(string sProducto)
        {
            try
            {
                List<productos> lProducto ;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select * from productos WHERE arc_clave = ? ";
                    lProducto = conn.Query<productos>(sQuery, sProducto);
                }

                return lProducto[0].arc_contado;
               
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        */
    }
}
