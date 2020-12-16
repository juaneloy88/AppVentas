using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("cuota")]
    public class cuota
    {
        public int cliente { get; set; }

        public int cartonesAnt { get; set; }

        public int cartonesAct { get; set; }

        public int cartonesFal { get; set; }

        /*Constructor*/
        public cuota()
        { }
    }
}
