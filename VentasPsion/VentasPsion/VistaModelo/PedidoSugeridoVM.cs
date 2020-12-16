using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    class PedidoSugeridoVM
    {
        fnPedidoSugerido _fnPedidoSugerido = new fnPedidoSugerido();

        public List<pedido> lSugeridoCliente = new List<pedido>();

        #region Método para obtener el listado de envase del cliente con el cargo calculado
        public async Task<List<pedido>> ObtieneSugerido(int iCliente)
        {
            var vLista = await _fnPedidoSugerido.ListaSugerido(iCliente);
            lSugeridoCliente = vLista;
            /*
            foreach (pedido Articulo in vLista)
            {
                //pedido Producto = new pedido();
                Producto.cln_clave = Articulo.cln_clave;
                //capturaEnvase.mec_envase = capturaEnv.mec_envase;
                //capturaEnvase.men_saldo_inicial = capturaEnv.men_saldo_inicial;
                //capturaEnvase.men_cargo = capturaEnv.men_cargo;
                //capturaEnvase.men_abono = capturaEnv.men_abono;
                //capturaEnvase.men_venta = capturaEnv.men_venta;
                //capturaEnvase.men_saldo_final = capturaEnv.men_saldo_final;
                this.lSugeridoCliente.Add(Producto);
            }*/

            return vLista;

        }
        #endregion Método para obtener el listado de envase del cliente con el cargo calculado

        #region Método que actualiza el campo del abono a la lista de CapturaEnvase
        public List<pedido> actualizandoAbono(string sMarca, int iCantidad)
        {
            var vValida = this.lSugeridoCliente.Find(x => x.arc_clave.Contains(sMarca));
            _fnPedidoSugerido.ActualizaInventario(VarEntorno.vCliente.cln_clave, sMarca, iCantidad);
            if (vValida != null)
            {
               this.lSugeridoCliente.Remove(vValida);
  
                pedido _pedido = new pedido();
                vValida.pen_inv_cantidad = iCantidad;
                vValida.pen_sug_cantidad = vValida.pen_prm_cantidad - vValida.pen_inv_cantidad;
                _pedido = vValida;
              this.lSugeridoCliente.Add(_pedido);
               
            }
            return this.lSugeridoCliente.OrderBy(x => x.arc_clave).ToList();
        }
        #endregion Método que actualiza el campo del abono a la lista de CapturaEnvase
    }
}
