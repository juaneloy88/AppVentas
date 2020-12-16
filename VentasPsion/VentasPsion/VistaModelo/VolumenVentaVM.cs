using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class VolumenVentaVM
    {
        public ObservableCollection<volumen_ventas> volumenventas { get; set; }
        public VolumenVentaVM()
        {
            // constructor

            volumenventas = new ObservableCollection<volumen_ventas>();
            volumenVentasSR oVolumenVentas = new volumenVentasSR();
            var oResponseVolumenVenta = oVolumenVentas.ObtenerVolumenVentas();


            for (int i = 0; i < oResponseVolumenVenta.Count; i++)
            {

                volumenventas.Add(new volumen_ventas
                {
                    vvc_tipo = oResponseVolumenVenta[i].vvc_tipo,
                    vvn_cuota_dia = oResponseVolumenVenta[i].vvn_cuota_dia,
                    vvn_tendencia_cartones = oResponseVolumenVenta[i].vvn_tendencia_cartones,
                    vvn_faltantes = oResponseVolumenVenta[i].vvn_faltantes,
                    vvn_porcentaje = oResponseVolumenVenta[i].vvn_porcentaje,
                    vvn_tendencia_porcentaje = oResponseVolumenVenta[i].vvn_tendencia_porcentaje,
                    vvd_fecha = oResponseVolumenVenta[i].vvd_fecha
                });
            }
        }
    }
}
