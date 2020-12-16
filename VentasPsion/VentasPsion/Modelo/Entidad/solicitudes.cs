using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

 // SOLICITUDES Y QUEJAS
namespace VentasPsion.Modelo.Entidad
{
    [Table("solicitudes")]
    public class solicitudes
    {
        public int dpn_clave { get; set; }

        [MaxLength(100)]
        public string soc_descripcion { get; set; }

        public int cln_clave { get; set; }

        public int run_clave { get; set; }

        public DateTime sod_fecha { get; set; }

        /*Constructor*/
        public solicitudes()
        { }
    }
}
