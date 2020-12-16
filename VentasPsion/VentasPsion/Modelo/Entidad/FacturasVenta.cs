using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("FacturasVenta")]
    class FacturasVenta
    {
        public int vcn_cliente { get; set; }

        public string vcn_folio { get; set; }

        public string vcc_cadena_original { get; set; }

        public int vcn_ruta { get; set; }

        public DateTime vcf_movimiento { get; set; }

        public string fvc_tipo { get; set; }

        public decimal vcn_importe { get; set; }

        public decimal fvn_pagos { get; set; }

        /*Constructor*/
        public FacturasVenta()
        { }
    }
}
