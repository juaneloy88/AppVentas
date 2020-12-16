using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("candados_productos")]
    public class candados_productos
    {
        [MaxLength(4)]
        public string arc_clave { get; set; }

        public int cpn_tipo { get; set; }

        public int cpn_cantidad { get; set; }

        [MaxLength(10)]
        public string cpc_mod { get; set; }

        [MaxLength(20)]
        public string cpc_segmento { get; set; }

        /*Constructor*/
        public candados_productos()
        { }
    }
}
