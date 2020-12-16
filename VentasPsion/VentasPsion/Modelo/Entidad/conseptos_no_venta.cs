using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("conseptos_no_venta")]
    class conseptos_no_venta
    {
        public int svn_clave { get; set; }

        [MaxLength(100)]
        public string svc_descripcion { get; set; }

        /*Constructor*/
        public conseptos_no_venta()
        { }
    }
}
