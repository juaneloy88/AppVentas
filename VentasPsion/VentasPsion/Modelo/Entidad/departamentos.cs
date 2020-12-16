using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("departamentos")]
    public class departamentos
    {
        public int dpn_clave { get; set; }

        [MaxLength(100)]
        public string dpc_descripcion { get; set; }

        public bool dpb_estatus { get; set; }


        /*Constructor*/
        public departamentos()
        { }
    }
}
