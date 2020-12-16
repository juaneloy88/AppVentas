using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("respuestas")]
    public class respuestas
    {
        public int cln_clave { get; set; }

        public int enn_id { get; set; }

        [MaxLength(100)]
        public string enc_respuesta { get; set; }

        public int enn_tipo_respuesta { get; set; }

        /*Constructor*/
        public respuestas()
        { }
    }
}