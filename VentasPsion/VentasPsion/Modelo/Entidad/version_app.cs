using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("version_app")]
    class version_app
    {
        public DateTime vpf_fecha { get; set; }

        [MaxLength(10)]
        public string vpc_version { get; set; }

        /*Constructor*/
        public version_app()
        { }
    }
}
