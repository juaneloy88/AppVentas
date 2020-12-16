using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class resumenDia
    {
        fnResumenDia fnResDia = new fnResumenDia();
        venta_pagosSR PagosSR = new venta_pagosSR();
        venta_detalleSR vntDetSR = new venta_detalleSR();

        //Total de pagos en Efectivo
        public double vcn_monto_efe()
        {
            //return fnResDia.TotalEfectivo();
            return Convert.ToDouble(PagosSR.PagosxRuta("EFECTIVO"));
        }

        //Total de pagos con Tarjeta
        public double vcn_tarjeta()
        {
            //return fnResDia.TotalTarjeta();
            return Convert.ToDouble(PagosSR.PagosxRuta("TARJETA"));
        }

        //Total de pagos con Transferencia
        public double vcn_transferencia()
        {
            //return fnResDia.TotalTarjeta();
            return Convert.ToDouble(PagosSR.PagosxRuta("TRANSFERENCIA"));
        }

        //Total de pagos con Cheques
        public double vcn_cheque()
        {
            //return fnResDia.TotalCheques();
            return Convert.ToDouble(PagosSR.PagosxRuta("CHEQUE"));
        }

        public List<ResumenDia> lResumenDia = new List<ResumenDia>();        

        #region Método que obtiene el valor de si existen registros de venta
        public  int validaSiExistenRegistrosVenta()
        {
            return vntDetSR.ConsultaVentasDia().Count>0?1:0;
        }
        #endregion Método que obtiene el valor de si existen registros de venta

        #region Método que genera el resumen de venta del día
        public async Task<List<ResumenDia>> consultaResumenDia()
        {
            #region Declaración de Variables e inicialización del objeto
            ResumenDia resumenDia = null;
            string sMarca = string.Empty;
            string sDescripcion = string.Empty;
            #endregion Declaración de Variables e inicialización del objeto

            //Se obtienen los movimientos de ventas
            var vVenta = vntDetSR.ConsultaVentasDia();

            var GroupbyART =
                      vVenta
                      .GroupBy(r => new { r.vdn_producto, r.vdd_descripcion })                      
                      .Select(g => new { vdn_producto = g.Key, venta = g.Sum(z=> z.vdn_venta) }).ToList();

            foreach (var det in GroupbyART)
            {
                resumenDia = new ResumenDia();
                resumenDia.sClaveAfecta = det.vdn_producto.vdn_producto;
                resumenDia.sDescripcion = det.vdn_producto.vdd_descripcion;
                resumenDia.iVenta = det.venta;
                lResumenDia.Add(resumenDia);
            }

            /*
            foreach (venta_detalle venta_det in vVenta)
            {
                resumenDia = new ResumenDia();

                //Se obtiene la marca a afectar
                sMarca = await fnResDia.consultaMarcaAfecta(venta_det.vdn_producto.ToString());

                //Consulta la descripción de la marca a afectar
                sDescripcion = await fnResDia.consultaDescripcionAfecta(sMarca);

                //Se carga el objeto de la clave ResumenDia
                resumenDia.sClaveAfecta = sMarca;
                resumenDia.sDescripcion = sDescripcion;
                resumenDia.iVenta = venta_det.vdn_venta;

                //Valida si ya existe en la clase resumenDia
                var vValida = this.lResumenDia.Find(x => x.sClaveAfecta.Contains(sMarca));

                if (vValida != null)
                {
                    //Si ya existe un registro en la lista, lo elimina e inserta los nuevos valores
                    this.lResumenDia.Remove(vValida);
                    resumenDia.iVenta = vValida.iVenta + venta_det.vdn_venta;
                    this.lResumenDia.Add(resumenDia);
                }
                else
                {
                    //Inserta el objeto a la lista
                    this.lResumenDia.Add(resumenDia);
                }
            }*/

            var vResumen = this.lResumenDia;

            return vResumen;
        }
        #endregion Método que genera el resumen de venta del día
        
    }

    public class ResumenDia
    {
        public string sClaveAfecta { get; set; }
        public string sDescripcion { get; set; }
        public int iVenta { get; set; }

        public ResumenDia()
        {
            sClaveAfecta = "";
            sDescripcion = "";
            iVenta = 0;
        }
    }    
}
