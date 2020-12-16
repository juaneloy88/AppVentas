using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class KpisVM
    {
        public ObservableCollection<kpis> oKpisCollection { get; set; }

        public KpisVM()
        {
            LlenarCollecctionKpis();
        }

        public void LlenarCollecctionKpis()
        {
            oKpisCollection = new ObservableCollection<kpis>();
            kpisSR oFnKpis = new kpisSR();
            var response = oFnKpis.ObtenerListaKpis();
            for (int i = 0; i < response.Count; i++)
            {

                oKpisCollection.Add(new kpis
                {
                    pkn_orden = response[i].pkn_orden,
                    pkc_descripcion = response[i].pkc_descripcion,
                    pkn_cuota = response[i].pkn_cuota,
                    pkn_venta = response[i].pkn_venta,
                    pkn_diferencia = response[i].pkn_diferencia,
                    pkn_porcentaje = response[i].pkn_porcentaje
                });
            }
        }
    }
}
