using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("promociones")]
    public class promociones
    {
        public int ppn_numero_promocion { get; set; }

        [MaxLength(4)]
        public string ppc_codigo_venta { get; set; }

        public int ppn_cantidad_venta { get; set; }

        [MaxLength(4)]
        public string ppc_codigo_regalo { get; set; }

        public int ppn_cantidad_regalo { get; set; }

        public DateTime ppc_inicia { get; set; }

        public DateTime ppc_termina { get; set; }

        [MaxLength(100)]
        public string ppc_descripcion { get; set; }

        [MaxLength(1)]
        public string ppc_tipo { get; set; }

        /*Constructor*/
        public promociones()
        { }
    }
}
