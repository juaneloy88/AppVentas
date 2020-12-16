using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("reto_diario")]
    public class reto_diario
    {
        public int run_clave { get; set; }

        [MaxLength(30)]
        public string rdc_nombre { get; set; }  

        public int rdn_cantidad { get; set; }

        public DateTime rdd_fecha { get; set; }

        /*Constructor*/
        public reto_diario()
        { }
    }
}
