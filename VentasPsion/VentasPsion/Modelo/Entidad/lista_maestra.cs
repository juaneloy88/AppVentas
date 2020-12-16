using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("lista_maestra")]
    public class lista_maestra
    {
        public int lmc_tipo { get; set; }

        [MaxLength(4)]
        public string lmc_producto { get; set; }

        public decimal lmc_precio { get; set; }

        public bool lmd_estatus { get; set; }

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
        public lista_maestra()
        { }
    }
}
