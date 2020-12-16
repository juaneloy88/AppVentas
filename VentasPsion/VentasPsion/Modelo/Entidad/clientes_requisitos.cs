using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    class clientes_requisitos
    {
        public int cln_clave { get; set; }

        public string crc_titular { get; set; }

        public string crc_horario_apertura { get; set; }

        public string crc_horario_cierre { get; set; }

        public string crc_horario_sugerido { get; set; }

        public int crb_factura { get; set; }

        public int crb_pago_tarjeta { get; set; }

        public int crb_chamuco { get; set; }

        public int crb_escaleras { get; set; }

        public int crb_rampa { get; set; }

        public int crb_espacio_estrecho { get; set; }

        public int crb_asaltos { get; set; }

        public string crc_avisos { get; set; }

        public bool crb_actualizado { get; set; }

        public int tipo { get; set; }

        public List<clientes_requisitos> ListaClientes { get; set; }

        public List<clientes> pendientes { get; set; }

        public clientes_requisitos()
        { }
    }
}
