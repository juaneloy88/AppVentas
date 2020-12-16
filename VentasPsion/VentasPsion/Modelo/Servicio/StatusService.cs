using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Servicio
{
    public class StatusService
    {
        public bool status { get; set; }
        public string mensaje { get; set; }
        public string valor { get; set; }
        public DateTime fecha { get; set; }
        public List<string> listaStrings { get; set; }
    }
}
