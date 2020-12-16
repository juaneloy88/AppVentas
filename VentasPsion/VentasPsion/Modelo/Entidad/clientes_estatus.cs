using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("clientes_estatus")]
    public class clientes_estatus
    {
        public int cln_clave { get; set; }


        public int cln_visitado { get; set; }


        public bool clb_corregido { get; set; }

        [MaxLength(1)]
        public string clc_programa_pago { get; set; }


        public bool clc_cliente_foco { get; set; }


        public bool clc_cliente_primer_impacto { get; set; }

        public bool clc_actua_foco_cooler { get; set; }

        public int cln_tipo_no_venta { get; set; }

        public double cln_latitud { get; set; }

        public double cln_longitud { get; set; }

        /*Constructor*/
        public clientes_estatus()
        { }
    }
}