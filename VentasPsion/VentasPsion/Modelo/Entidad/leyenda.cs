using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("leyenda")]
    public class leyenda
    {
        public int len_clave { get; set; }

        [MaxLength(100)]
        public string lec_leyenda { get; set; }

        /*Constructor*/
        public leyenda()
        { }
    }
}
