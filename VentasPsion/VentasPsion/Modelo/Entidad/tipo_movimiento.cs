using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("tipo_movimiento")]
    class tipo_movimiento
    {
        public int tmn_tipo_movimiento { get; set; }

        public string tmc_descripcion { get; set; }


    }
}
