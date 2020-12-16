using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("opciones_app")]
    public class opciones_app
    {        
        public int oan_id { get; set; }

        [MaxLength(100)]
        public string opn_descripcion { get; set; }

        public bool oab_valor { get; set; }

        /*Constructor*/
        public opciones_app()
        { }
    }
}
