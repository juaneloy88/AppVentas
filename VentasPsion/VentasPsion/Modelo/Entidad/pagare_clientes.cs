using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("pagare_clientes")]
    class pagare_clientes
    {
        public int cln_clave { get; set; }

        public bool pcb_entregado { get; set; }

        public bool pcb_modificado { get; set; }
           

        /*Constructor*/
        public pagare_clientes()
        { }

    }
}
