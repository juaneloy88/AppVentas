using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("pedido")]
    public class pedido
    {
        public int cln_clave { get; set; }

        public int run_clave { get; set; }

        [MaxLength(4)]
        public string arc_clave { get; set; }

        public int pen_prm_cantidad { get; set; }

        public int pen_ven_cantidad { get; set; }

        public int pen_inv_cantidad { get; set; }

        public int pen_sug_cantidad { get; set; }

        /*Constructor*/
        public pedido()
        { }
    }
}
