using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("existencia")]
    public class existencia
    {
        public string alc_clave { get; set; }

        [MaxLength(4)]
        public string arc_clave { get; set; }

        public int exn_existencia { get; set; }

        public int exn_vendido { get; set; }

        /*Constructor*/
        public existencia()
        { }
    }
}
