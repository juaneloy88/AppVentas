using VentasPsion.Modelo.Entidad;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class lista_maestraSR
    {
        conexionDB cODBC = new conexionDB();


        //* busca el precio de un producto por medio de la clave y la lista de precio  *//
        public decimal ProductoxListaPrecio(string sClave,int sListaprecio)
        {
            try
            {
                decimal vPrecio =0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select lmc_precio from lista_maestra WHERE lmc_producto = ? and lmc_tipo = ? ";
                    vPrecio = conn.ExecuteScalar<decimal>(sQuery, sClave, sListaprecio);
                }

                return vPrecio;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }
    }
}
