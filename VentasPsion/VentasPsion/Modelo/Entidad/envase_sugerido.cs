using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{


    [Table("envase_sugerido")]
    public class envase_sugerido
    {
        public int cln_clave { get; set; }

        [MaxLength(1)]
        public string esc_envase { get; set; }

        public int esn_cantidad_vacio { get; set; }

        public int esn_cantidad_lleno { get; set; }

        [MaxLength(100)]
        public string esc_comentario { get; set; }

        /*Constructor*/
        public envase_sugerido()
        { }
    }
}
