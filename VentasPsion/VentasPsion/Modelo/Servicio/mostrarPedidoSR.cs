using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class mostrarPedidoSR
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);
        
        #region Método para obtener el pedido y mostrarlo
        //public async Task<List<MostrarPedido>> muestraPedidox(string sCliente)
        //{
        //    await sl.WaitAsync();

        //    try
        //    {
        //        using (SQLiteConnection conn = cODBC.CadenaConexion())
        //        {
        //            string sQuery = "select v.vdn_producto as sClaveAfecta, p.ard_descripcion as sDescripcion, " +
        //                            "sum(v.vdn_venta) as iVenta, sum(v.vdn_importe) as dImporte " +
        //                            "from venta_detalle v " +
        //                            "join productos p on p.arc_clave = v.vdn_producto " +
        //                            "where v.vdn_cliente = " + sCliente + " " +
        //                            "GROUP BY v.vdn_producto, p.ard_descripcion";

        //            var lLista = conn.Query<MostrarPedido>(sQuery);

        //            return lLista;
        //        }
        //    }
        //    finally
        //    {
        //        sl.Release();
        //    }
        //}
        #endregion Método para obtener el pedido y mostrarlo
    }
}