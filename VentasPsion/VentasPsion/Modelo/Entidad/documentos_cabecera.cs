using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("documentos_cabecera")]
    public class documentos_cabecera
    {
        public int vcn_cliente { get; set; }

        [MaxLength(6)]
        public string vcn_folio { get; set; }

        public DateTime vcf_movimiento { get; set; }

        public decimal vcn_importe { get; set; }              

        [MaxLength(1)]
        public string dcc_tipo { get; set; }        

        public string vcc_cadena_original { get; set; }

        //public int dcn_clienteBase { get; set; }   /// NUEVO   13 04 2019

        public int dcn_numero_pago_base { get; set; }

        public decimal dcn_saldo { get; set; }

        public int  dcn_dias_antiguedad { get; set; }  /// NUEVO   21 08 2018


        public int dcn_numero_pago { get; set; }

        public bool dcb_nuevo_documento { get; set; }   

        public decimal dPagosVendedor { get; set; }


        /*Constructor*/
        public documentos_cabecera()
        { }
    }
}
