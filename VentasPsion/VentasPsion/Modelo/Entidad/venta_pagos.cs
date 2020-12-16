using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("venta_pagos")]
    public class venta_pagos
    {
        public int vpn_numpago { get; set; }

        [MaxLength(6)]
        public string vcn_folio { get; set; }

        public int vcn_cliente { get; set; }

        public string vcf_movimiento { get; set; }

        public int vcn_ruta { get; set; }

        [MaxLength(50)]
        public string vpc_descripcion { get; set; }

        [MaxLength(2)]
        public string cfpc_formapago { get; set; }

        [MaxLength(100)]
        public string vcc_banco { get; set; }

        [MaxLength(50)]
        public string vpc_tipotarjeta { get; set; }

        public Decimal vcn_monto { get; set; }

        [MaxLength(100)]
        public string vcc_referencia { get; set; }

        [MaxLength(100)]
        public string vpc_nocuenta { get; set; }

        [MaxLength(100)]
        public string vpc_autorizacion { get; set; }

        /*Constructor*/
        public venta_pagos()
        { }
    }
}
