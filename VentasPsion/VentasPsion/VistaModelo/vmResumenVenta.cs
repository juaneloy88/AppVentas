using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class vmResumenVenta
    {
        venta_detalleSR resumenVenta = new venta_detalleSR();
        int nCliente = VarEntorno.vCliente.cln_clave;

        #region Proceso para obtener la consulta del liquido de venta
        public async Task<List<venta_detalle>> cargaLiquido()
        {
            return await resumenVenta.cargaLiquido(Convert.ToString(nCliente));
        }
        #endregion Proceso para obtener la consulta del liquido de venta        
        
    }
}