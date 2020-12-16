using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("telefonos_clientes")]
    class telefonos_clientes
    {
        public int cln_clave { get; set; }

        [MaxLength(15)]
        public string tcc_telefono { get; set; }

        [MaxLength(50)]
        public string tcc_nombre { get; set; }

        public bool tcb_estatus { get; set; }

        public bool tcb_movil { get; set; }

        [MaxLength(15)]
        public string tct_horarioini { get; set; }

        [MaxLength(15)]
        public string tct_horariofin { get; set; }

        [MaxLength(50)]
        public string tcc_comentario { get; set; }

        public int tcn_rutacaptura { get; set; }

        public DateTime tct_fechacaptura{ get; set; }

        public bool tcb_revisado { get; set; }
    }
}
