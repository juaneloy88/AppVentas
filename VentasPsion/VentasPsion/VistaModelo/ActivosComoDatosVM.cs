using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class ActivosComoDatosVM
    {
        private int cln_clave;

        public ObservableCollection<activos_comodatos> activosDatos { get; set; }

        public ActivosComoDatosVM(int cln_clave)
        {
            // constructor
            this.cln_clave = cln_clave;
            LlenarListaActivosDatos(this.cln_clave);
  
        }

        public void LlenarListaActivosDatos(int cln_clave)
        {
            activosDatos = new ObservableCollection<activos_comodatos>();
            activosComoDatosSR oActivosDatos = new activosComoDatosSR();
            var response = oActivosDatos.ObtenerActivosComoDatos(cln_clave);
            for (int i = 0; i < response.Count; i++)
            {

                activosDatos.Add(new activos_comodatos
                {
                    cln_clave = response[i].cln_clave,
                    acn_cantidad = response[i].acn_cantidad,
                    acc_descripcion = response[i].acc_descripcion
                });
            }
        }
    }
}
