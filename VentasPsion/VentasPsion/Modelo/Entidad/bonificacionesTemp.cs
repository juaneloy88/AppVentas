using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    public class bonificacionesTemp
    {
        [MaxLength(6)]
        public string boc_folio { get; set; }

        [MaxLength(10)]
        public double boi_documento { get; set; }

        [MaxLength(1)]
        public string boc_tipo { get; set; }        

        /*Constructor*/
        public bonificacionesTemp()
        { }
    }
}
