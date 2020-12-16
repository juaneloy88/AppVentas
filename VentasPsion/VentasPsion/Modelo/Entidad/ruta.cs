using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("ruta")]
    public class ruta
    {
        public int run_clave { get; set; }

        [MaxLength(50)]
        public string ruc_descripcion { get; set; }

        public int run_rfid { get; set; }

        public int run_folio { get; set; }

        [MaxLength(2)]
        public string alc_clave { get; set; }

        [MaxLength(30)]
        public string ruc_inicio { get; set; }

        [MaxLength(30)]
        public string ruc_termino { get; set; }

        public int run_ncamion { get; set; }

        public int run_kminicio { get; set; }

        public int run_kmfinal { get; set; }

        /*Constructor*/
        public ruta()
        { }
    }
}
