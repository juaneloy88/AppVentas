using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("concepto_devoluciones")]
    public class concepto_devoluciones
    {
        public int cdn_clave { get; set; }

        [MaxLength(100)]
        public string cdc_descripcion { get; set; }

        /*Constructor*/
        public concepto_devoluciones()
        { }
    }
}
