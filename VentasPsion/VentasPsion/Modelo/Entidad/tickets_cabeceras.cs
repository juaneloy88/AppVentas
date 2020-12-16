using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("tickets_cabeceras")]
    class tickets_cabeceras
    {
        public int tcn_cliente { get; set; }

        [MaxLength(6)]
        public string tcc_folio { get; set; }

        public DateTime tcf_movimiento { get; set; }

        public double tcn_importe { get; set; }

        public string tcc_cadena_original { get; set; }

        public bool tcb_esta_vencido { get; set; }

        public bool tcb_tiene_complemento { get; set; }

        public int tcn_contado { get; set; }

        public string tcc_forma_pago { get; set; }

        /*Constructor*/
        public tickets_cabeceras()
        { }
    }
}
