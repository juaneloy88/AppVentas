using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("kpis")]
    public class kpis
    {      
        public int pkn_orden { get; set; }

        public int pkn_ruta { get; set; }

        [MaxLength(50)]
        public string pkc_descripcion  { get; set; }

        public int pkn_cuota { get; set; }

        public int pkn_venta { get; set; }

        public int pkn_diferencia { get; set; }

        public int pkn_porcentaje { get; set; }

        /*Constructor*/
        public kpis()
        { }
        
    }
}
