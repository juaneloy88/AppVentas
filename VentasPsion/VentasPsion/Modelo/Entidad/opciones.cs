using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("opciones")]
    public class opciones
    {
        public int opn_id { get; set; }

        public int enn_id { get; set; } // id pregunta - encuesta

        [MaxLength(100)]
        public string opn_descripcion { get; set; }


        /*Constructor*/
        public opciones()
        { }
    }
}
