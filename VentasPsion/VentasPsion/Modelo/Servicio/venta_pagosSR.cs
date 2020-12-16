using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class venta_pagosSR
    {
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        public List<venta_pagos> PagosxFolioxCliente(int iIdCliente,string sFolio,string sDescripcion)
        {
            string sQuery = string.Empty;
            //await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    sQuery = " select * " +
                             " from venta_pagos " +
                             " where vcn_cliente = " + iIdCliente + " " +
                             " and vcn_folio = '" + sFolio + "'"+
                             " and vpc_descripcion = '" + sDescripcion + "'";

                    return conn.Query<venta_pagos>(sQuery);
                }
            }
            finally
            {
                //sl.Release();
            }
        }

        public decimal PagosxRuta(string sDescripcion)
        {
            string sQuery = string.Empty;
            //await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    sQuery = " select coalesce(sum(vcn_monto),0) " +
                             " from venta_pagos " +
                             " where vpc_descripcion = '" + sDescripcion + "'";

                    return conn.ExecuteScalar<decimal>(sQuery);
                }
            }
            finally
            {
                //sl.Release();
            }
        }
    }
}
