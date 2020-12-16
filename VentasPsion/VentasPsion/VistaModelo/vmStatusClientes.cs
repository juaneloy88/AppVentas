using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using VentasPsion.Modelo.Servicio;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.VistaModelo
{

    public class vmStatusClientes
    {
        fnStatusClientes fnStatusCtes = new fnStatusClientes();
        fnVentaCabecera fnventacab = new fnVentaCabecera();

        public List<StatusClientes> lEstatusClientes = new List<StatusClientes>();
        public List<venta_cabecera> sFolios = null;

        public Vista.frmBuscarCliente vBuscarCliente = null;

        #region Método que devuelve los clientes sin visitar
        public async Task<List<clientes_estatus>> obtieneClientesSinVisita()
        {
            if (VarEntorno.cTipoVenta == 'R')
            {
                return await fnStatusCtes.obtieneClientesSinVisita();
            }
            else
            {
                return await fnStatusCtes.obtieneClientesSinVisitaAP();
            }
        }
        #endregion Método que devuelve los clientes sin visitar

        #region Método para obtener el listado de Clientes con sus status
        public async Task<List<StatusClientes>> obtieneStatusClientes()
        {
            return await fnStatusCtes.obtieneStatusClientes();
        }
        #endregion Método para obtener el listado de Clientes con sus status

        #region Método para liberar la Ruta de reparto
        public async Task<string> liberaReparto(string sPassword)
        {
            return await fnStatusCtes.liberaReparto(sPassword);
        }
        #endregion Método para liberar la Ruta de reparto

        #region Método para borrar la venta
        public async Task<string> borrarVenta(string sCliente)
        {
            return await fnStatusCtes.borrarVenta(sCliente);
        }
        #endregion Método para borrar la venta

        #region Método para Borrar la Devolución
        public async Task<string> borrarDevol(string sCliente)
        {
            return await fnStatusCtes.borrarDevol(sCliente);
        }
        #endregion Método para Borrar la Devolución

        #region Método para borrar la entrega de Reparto
        public async Task<string> borrarEntrega(string sCliente)
        {
            return await fnStatusCtes.borrarEntrega(sCliente);
        }
        #endregion Método para borrar la entrega de Reparto
    }

    public class StatusClientes
    {
        public string sCliente { get; set; }
        public string sNombreComercial { get; set; }
        public int iVisitado { get; set; }
        public Color cstatusColor { get; set; }

        public StatusClientes()
        {
            sCliente = "";
            sNombreComercial = "";
            iVisitado = 0;
        }
    }
}
