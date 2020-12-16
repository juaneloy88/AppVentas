using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class RetoDelDiaVM
    {
      /*  public class RetoDia
        {
            public int run_clave { get; set; }
            public string rdc_nombre { get; set; }
            public int rdn_cantidad { get; set; }
            public DateTime rdd_fecha { get; set; }
            public RetoDia()
            {
                // constructor
            }
        }*/
        public ObservableCollection<reto_diario> oRetoDia { get; set; }
        public RetoDelDiaVM()
        {
            // constructor
            oRetoDia = new ObservableCollection<reto_diario>();
            cuotaDiaSR oCuotaDia = new cuotaDiaSR();
            var oResponseCuotaDia = oCuotaDia.ObtenerListaRetoDia();
            for(int i=0;i< oResponseCuotaDia.Count; i++)
            {

                oRetoDia.Add(new reto_diario
                {
                    run_clave = oResponseCuotaDia[i].run_clave,
                    rdc_nombre = oResponseCuotaDia[i].rdc_nombre,
                    rdn_cantidad = oResponseCuotaDia[i].rdn_cantidad,
                    rdd_fecha = oResponseCuotaDia[i].rdd_fecha
                });
            }



        }
    }
}
