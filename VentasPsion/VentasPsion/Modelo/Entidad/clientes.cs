using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Entidad
{
    [Table("clientes")]
    public class clientes
    {
        public int cln_clave { get; set; }

        public int cli_noRutaTvta { get; set; }

        public int run_clave { get; set; }

        [MaxLength(5)]
        public string clc_clasificacion { get; set; }

        [MaxLength(100)]
        public string clc_nombre_comercial { get; set; }

        [MaxLength(100)]
        public string clc_nombre { get; set; }

        [MaxLength(100)]
        public string clc_domicilio { get; set; }

        [MaxLength(15)]
        public string clc_rfc { get; set; }


        public bool clc_devolucion { get; set; }

        [MaxLength(1)]
        public string clc_credito { get; set; }

        [MaxLength(10)]
        public string cln_codigo { get; set; }

        [MaxLength(16)]
        public string clc_rfid { get; set; }

        [MaxLength(1)]
        public string clc_estatus { get; set; }


        public bool clc_impacto { get; set; }

        [MaxLength(1)]
        public string clc_facturacion_linea { get; set; }


        public decimal cln_limite_venta { get; set; }


        public decimal cln_saldo { get; set; }

        [MaxLength(200)]
        public string domiciliocliente { get; set; }


        public int cln_total_punto { get; set; }


        public int cln_cuota { get; set; }


        public decimal ccn_venta { get; set; }

        [MaxLength(20)]
        public string clc_no_licencia { get; set; }

        [MaxLength(5)]
        public string clc_propietario_licencia { get; set; }

        [MaxLength(1)]
        public string clc_estatus_licencia { get; set; }


        public bool clc_cliente_foco { get; set; }


        public bool clc_cliente_primer_impacto { get; set; }


        public int clc_dias_credito { get; set; }

        [MaxLength(20)] 
        public string clc_fecha_movimiento { get; set; }

        [MaxLength(3)]
        public string cln_secuencia { get; set; }


        public int cdn_clave { get; set; }


        public bool cln_cheque { get; set; }


        public decimal clc_pago_diferencia_preventa { get; set; }

        [MaxLength(1)]
        public string vcc_contado { get; set; }


        public int lmc_tipo { get; set; }


        public double cln_latitud { get; set; }

        public double cln_longitud { get; set; }

        [MaxLength(1)]
        public string cld_impuesto_xml { get; set; }

        [MaxLength(4)]
        public string clc_cobrador { get; set; }
        
        public bool pcb_entregado { get; set; }

        public string ctp_clave { get; set; }

        public string clc_mod_nivel1 { get; set; }

        public int clc_dias_credito_cero { get; set; }   /// NUEVO

        public bool clb_ticket_cobranza { get; set; }   /// NUEVO

        /*Constructor*/
        public clientes()
        { }
    }
}
