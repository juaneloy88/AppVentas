using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("encuesta")]
    public class encuesta
    {
        public int enn_id { get; set; }

        [MaxLength(100)]
        public string enc_pregunta { get; set; }

        public int enn_tipo_respuesta { get; set; }

        public DateTime end_fecha_alta { get; set; }

        public DateTime end_fecha_baja { get; set; }

        public bool enb_estatus { get; set; }

        public int enn_tipo_encuesta { get; set; }

        /*Constructor*/
        public encuesta()
        { }
    }
}
