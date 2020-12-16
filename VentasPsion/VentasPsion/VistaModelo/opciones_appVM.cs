using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class OpcionesappVM
    {
        public List<opciones_app> opciones = new List<opciones_app>();

        public   OpcionesappVM()
        {
            opciones = new opcionesAppSR().ObtenerListaOpciones();
        }
    }
}
