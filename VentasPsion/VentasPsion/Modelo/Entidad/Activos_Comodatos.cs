using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("activos_comodatos")]
    public class activos_comodatos
    {

        public int cln_clave { get; set; }

        public int acn_cantidad { get; set; }

        [MaxLength(100)]
        public string  acc_descripcion { get; set; }

        /*Constructor*/
        public activos_comodatos()
        { }
        
    }
    
}
