using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("orden_ticket")]
    public class orden_ticket
    {
        [MaxLength(4)]
        public string arc_clave { get; set; }

        public int oin_orden { get; set; }

        [MaxLength(50)]
        public string cud_descripcion { get; set; }

        /*Constructor*/
        public orden_ticket()
        { }
    }
}
