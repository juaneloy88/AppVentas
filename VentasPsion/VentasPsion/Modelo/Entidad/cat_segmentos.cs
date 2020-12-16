using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("cat_segmentos")]
    public class cat_segmentos
    {
        [MaxLength(2)]
        public string csc_clave { get; set; }

        [MaxLength(50)]
        public string csc_nombre { get; set; }

    }
}
