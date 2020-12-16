using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("productos")]
    public class productos
    {
        [MaxLength(4)]
        public string arc_clave { get; set; }

        [MaxLength(4)]
        public string arc_envase { get; set; }

        public int arn_puntos { get; set; }

        [MaxLength(30)]
        public string ard_corta { get; set; }

        [MaxLength(50)]
        public string ard_descripcion { get; set; }

        [MaxLength(1)]
        public string arc_estatus { get; set; }

        //[MaxLength(100)]
        //public string arc_producto { get; set; }

        [MaxLength(1)]
        public string arc_produ { get; set; }

        public int pdc_prom_envase { get; set; }

        [MaxLength(4)]
        public string arc_afecta { get; set; }

        [MaxLength(3)]
        public string arc_clasif_estadistica { get; set; }

        [MaxLength(3)]
        public string cmc_clave { get; set; }

        [MaxLength(3)]
        public string cuc_clave { get; set; }
                
        public bool arc_contado { get; set; }

        /*Constructor*/
        public productos()
        { }
    }
}
