using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("password")]
    public class password
    {
        public DateTime ppf_fecha { get; set; }

        [MaxLength(10)]
        public string ppc_password { get; set; }

        [MaxLength(10)]
        public string ppc_password_ajustes { get; set; }

        /*Constructor*/
        public password()
        { }
    }
}
