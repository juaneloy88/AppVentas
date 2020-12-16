using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("documentos_detalle")]
    public class documentos_detalle
    {        
        public int vcn_cliente { get; set; }
                
        [MaxLength(6)]
        public string vcn_folio { get; set; }
        
        public DateTime vcf_movimiento { get; set; }

        public string vcn_folio_cabecera { get; set; }

        public DateTime vcf_movimiento_cabecera { get; set; }

        public decimal vcn_monto_pago { get; set; }



        public int ddn_numero_pago { get; set; }
             
        public string vcc_cadena_original { get; set; }

        public int ddn_cantidad_pagos { get; set; }

        [MaxLength(2)]
        public string ddc_forma_pago { get; set; }

        /*Constructor*/
        public documentos_detalle()
        { }
    }
}
