using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("envase_temp")]
    public class envase_temp
    {
        public int cln_clave { get; set; }

        public string men_folio { get; set; }

        [MaxLength(1)]
        public string mec_envase { get; set; }

        public int men_saldo_inicial { get; set; }

        public int men_cargo { get; set; }

        public int men_abono { get; set; }

        public int men_venta { get; set; }

        public int men_saldo_final { get; set; }

        [MaxLength(4)]
        public string arc_clave { get; set; }

        /*Constructor*/
        public envase_temp()
        { }
    }
}