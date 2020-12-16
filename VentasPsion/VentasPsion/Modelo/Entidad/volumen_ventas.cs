using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("volumen_ventas")]
    public class volumen_ventas
    {
        public int run_clave { get; set; }

        [MaxLength(20)]
        public string vvc_tipo { get; set; }

        public decimal vvn_cuota_dia { get; set; }

        public decimal vvn_tendencia_cartones { get; set; }

        public decimal vvn_faltantes { get; set; }

        public decimal vvn_porcentaje { get; set; }

        public decimal vvn_tendencia_porcentaje { get; set; }

        public DateTime vvd_fecha { get; set; }

        /*Constructor*/
        public volumen_ventas()
        { }
    }
}
