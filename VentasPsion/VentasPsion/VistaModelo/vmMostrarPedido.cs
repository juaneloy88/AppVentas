using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class vmMostrarPedido
    {
        //mostrarPedidoSR fnmuestraPedido = new mostrarPedidoSR();
        clientes_estatusSR SRclientes_estatus = new clientes_estatusSR();
        venta_detalleSR vntDetSR = new venta_detalleSR();

        public List<MostrarPedido> lMuestraPedido = new List<MostrarPedido>();

        #region Método para conocer el status del cliente
        public async Task<string> validaStatusCliente(string sCliente)
        {
            return await SRclientes_estatus.validaStatusCliente(sCliente);
        }
        #endregion Método para conocer el status del cliente

        #region Método para obtener el pedido y mostrarlo
        public async Task<List<MostrarPedido>> muestraPedido(string sCliente)
        {
            this.lMuestraPedido.Clear();

            var vLista = await vntDetSR.muestraPedido(sCliente);

            foreach (MostrarPedido muestraPed in vLista)
            {
                MostrarPedido muestraPedido = new MostrarPedido();
                muestraPedido.sClaveAfecta = muestraPed.sClaveAfecta;
                muestraPedido.sDescripcion = muestraPed.sDescripcion;
                muestraPedido.iVenta = muestraPed.iVenta;
                muestraPedido.dImporte = muestraPed.dImporte;
                lMuestraPedido.Add(muestraPedido);
            }

            return this.lMuestraPedido;
        }
        #endregion Método para obtener el pedido y mostrarlo
    }

    public class MostrarPedido
    {
        public string sClaveAfecta { get; set; }
        public string sDescripcion { get; set; }
        public int iVenta { get; set; }
        public decimal dImporte { get; set; }

        public MostrarPedido()
        {
            sClaveAfecta = "";
            sDescripcion = "";
            iVenta = 0;
            dImporte = 0;
        }
    }
}