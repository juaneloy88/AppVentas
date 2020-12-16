using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("clientes_requisitos_surtir")]
    class clientes_requisitos_surtir
    {
        public int cln_clave { get; set; }

        public string  crc_titular { get; set; }

        public string crc_horario_apertura { get; set; }

        public string crc_horario_cierre { get; set; }

        public string crc_horario_sugerido { get; set; }

        public bool crb_factura { get; set; }

        public bool crb_pago_tarjeta { get; set; }

        public bool crb_chamuco { get; set; }

        public bool crb_escaleras { get; set; }

        public bool crb_rampa { get; set; }

        public bool crb_espacio_estrecho { get; set; }

        public bool crb_asaltos { get; set; }

        public string crc_avisos { get; set; }

        public bool crb_actualizado { get; set; }

        /*Constructor*/
        public clientes_requisitos_surtir()
        { }
    }
}
