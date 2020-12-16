using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("empleados")]
    public class empleados
    {
        public int emn_clave { get; set; }

        [MaxLength(100)]
        public string emc_nombre { get; set; }

        [MaxLength(100)]
        public string emc_usuario { get; set; }

        [MaxLength(100)]
        public string emc_password { get; set; }

        public int emn_ruta { get; set; }

        /*Constructor*/
        public empleados()
        { }
    }
}
