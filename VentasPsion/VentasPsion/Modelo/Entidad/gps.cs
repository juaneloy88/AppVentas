using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("gps")]
    public class gps
    {
        public int run_clave { get; set; }

        public int cln_clave { get; set; }

        public string gpd_latitud { get; set; }

        public string gpd_longitud { get; set; }

        public int ctn_tipo_movimiento { get; set; }

        [MaxLength(20)]
        public string gpd_hora { get; set; }

        public string gpc_folio { get; set; }

        public bool gpb_esBase { get; set; }

        /*Constructor*/
        public gps()
        { }
    }
}
