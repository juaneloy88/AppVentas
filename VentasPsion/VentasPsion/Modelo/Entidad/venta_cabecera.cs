using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("venta_cabecera")]
    public class venta_cabecera
    {
        [MaxLength(6)]
        public string vcn_folio { get; set; }

        public int vcn_cliente { get; set; }

        public int vcn_estatus { get; set; }

        public decimal vcn_importe { get; set; }

        public decimal vcn_saldo_ant { get; set; }

        public decimal vcn_saldo_nuevo { get; set; }

        public decimal vcn_monto_pago { get; set; }

        [MaxLength(7)]
        public string vcc_hora_ini { get; set; }

        [MaxLength(7)]
        public string vcc_hora_fin { get; set; }

        [MaxLength(1)]
        public string vcc_tipo_pago { get; set; }

        [MaxLength(11)]
        public string vcc_banco { get; set; }

        [MaxLength(12)]
        public string vcf_num_cheque { get; set; }

        public DateTime vcf_fec_cheque { get; set; }

        public decimal vcn_monto_cheque { get; set; }

        public DateTime vcf_fec_venta { get; set; }

        public int vcn_descuento { get; set; }

        public int vcn_producto_insp { get; set; }

        public decimal vcn_monto_efe { get; set; }


        [MaxLength(11)]
        public string vcc_banco2 { get; set; }

        [MaxLength(12)]
        public string vcf_num_cheque2 { get; set; }

        public DateTime vcf_fec_cheque2 { get; set; }

        public decimal vcn_monto_cheque2 { get; set; }

        public DateTime vcf_fec_venta2 { get; set; }

        public int vcn_descuento2 { get; set; }

        public int vcn_producto_insp2 { get; set; }

        

        [MaxLength(11)]
        public string vcc_banco3 { get; set; }

        [MaxLength(12)]
        public string vcf_num_cheque3 { get; set; }

        public DateTime vcf_fec_cheque3 { get; set; }

        public decimal vcn_monto_cheque3 { get; set; }

        public DateTime vcf_fec_venta3 { get; set; }

        public int vcn_descuento3 { get; set; }

        public int vcn_producto_insp3 { get; set; }

        

        public string vcn_saldo_envase { get; set; }

        [MaxLength(100)]
        public int vcn_ruta { get; set; }

        public string vcf_movimiento { get; set; }

        [MaxLength(2)]
        public string vcc_clave { get; set; }

        [MaxLength(1)]
        public string vcc_contado { get; set; }

        [MaxLength(2)]
        public string cmpc_clave_sat { get; set; }

        [MaxLength(4)]
        public string vcc_ctapgosat { get; set; }
        
        [MaxLength(3)]
        public string cmpc_metodopago { get; set; }

        [MaxLength(2)]
        public string cfpc_formapago { get; set; }

        [MaxLength(40)]
        public string fcd_uuid { get; set; }

        /// <summary>
        /// campo de uso interno 
        /// </summary>
        [MaxLength(40)]
        public string efec_transf { get; set; }

        /*Constructor*/
        public venta_cabecera()
        { }
    }
}
