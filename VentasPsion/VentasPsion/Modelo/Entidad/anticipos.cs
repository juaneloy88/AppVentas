using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("anticipos")]
    public class anticipos
    {
        public int cln_clave { get; set; }

        [MaxLength(6)]
        public string vcn_folio { get; set; }

        public DateTime vcf_movimiento { get; set; }
        
        public decimal ann_monto_pago { get; set; }

        public int ann_cantidad_pagos { get; set; }

        [MaxLength(2)]
        public string anc_forma_pago { get; set; }



        public bool anb_nuevo { get; set; }

        public bool anb_relacionado { get; set; }
        

        /*Constructor*/
        public anticipos()
        { }
    }
}
