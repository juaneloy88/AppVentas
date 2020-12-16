using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("venta_detalle")]
    public class venta_detalle
    {
        public int prn_secuencia { get; set; }

        [MaxLength(6)]//,PrimaryKey]
        public string vdn_folio { get; set; }

        //[ PrimaryKey]
        public int vdn_cliente { get; set; }

        public int run_clave { get; set; }

        [MaxLength(4)]//, PrimaryKey]
        public string vdn_producto { get; set; }

        public int vdn_venta { get; set; }

        public decimal vdn_importe { get; set; }

        [MaxLength(20)]
        public string vdd_descripcion { get; set; }

        public decimal vdn_precio { get; set; }

        [MaxLength(2)]
        public string vdc_tipo_precio { get; set; }

        [MaxLength(1)]
        public string vdc_tipo_entrada { get; set; }

        [MaxLength(7)]
        public string vdc_hora { get; set; }

        public int vdn_venta_dev { get; set; }

        [MaxLength(6)]
        public string vdn_folio_devolucion { get; set; }

        public int vdn_tipo_promo { get; set; }

        [NotNull]
        public decimal lmn_precioneto { get; set; }
        public decimal lmn_ieps { get; set; }
        public decimal lmn_preciobruto { get; set; }
        public decimal lmn_iva { get; set; }
        public decimal lmn_preciofinal { get; set; } 
        public decimal lmc_porcentaje { get; set; }
        public decimal lmn_descuento_neto { get; set; }
        public decimal lmn_iepsdescuento { get; set; }
        public decimal lmn_descuento_bruto { get; set; }
        public decimal lmn_ivadescuento { get; set; }
        public decimal lmn_totaldescuento { get; set; }
        public decimal lmn_preciolm_neto { get; set; }
        public decimal lmn_preciolm_ipes { get; set; }
        public decimal lmn_preciolm_bruto { get; set; }
        public decimal lmn_preciolm_iva { get; set; }
        public decimal lmn_preciolm { get; set; }

        /*Constructor*/
        public venta_detalle()
        { }
    }
}