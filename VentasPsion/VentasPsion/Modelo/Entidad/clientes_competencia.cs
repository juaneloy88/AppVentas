using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("clientes_competencia")]
    public class clientes_competencia
    {
        [MaxLength(100)]
        public string ccc_nombre { get; set; }

        [MaxLength(50)]
        public string ccc_negocio { get; set; }

        public int ccn_tipo { get; set; }

        [MaxLength(5)]
        public string ctp_clave { get; set; }

        public string ccn_latitud { get; set; }

        public string ccn_longitud { get; set; }

        public clientes_competencia()
        { }

    }
}
