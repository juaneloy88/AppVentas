using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("bonificaciones")]
    public class bonificaciones
    {
        public int boc_cliente { get; set; }

        [MaxLength(6)]
        public string boc_folio { get; set; }

        [MaxLength(10)]
        public decimal boi_documento { get; set; }

        [MaxLength(1)]
        public string boc_tipo { get; set; }

        [MaxLength(6)]
        public string boc_folio_venta { get; set; }

        /*Constructor*/
        public bonificaciones()
        { }
    }
}