using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("devoluciones")]
    public class devoluciones
    {
        public int cln_clave { get; set; }

        public int dev_clave { get; set; }

        /*Constructor*/
        public devoluciones()
        { }
    }
}
