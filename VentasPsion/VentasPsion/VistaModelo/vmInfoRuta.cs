using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.VistaModelo
{
    public class vmInfoRuta
    {
        
    }

    public class InfoVtaCabecera
    {
        public string vcn_folio { get; set; }
        public int vcn_cliente { get; set; }
        public int vcn_estatus { get; set; }
        public double vcn_importe { get; set; }
        public double vcn_saldo_ant { get; set; }
        public double vcn_saldo_nuevo { get; set; }
        public double vcn_monto_pago { get; set; }        
        public string vcc_hora_ini { get; set; }        
        public string vcc_hora_fin { get; set; }
        public string vcc_tipo_pago { get; set; }
        public string vcc_banco { get; set; }
        public string vcf_num_cheque { get; set; }
        public DateTime vcf_fec_cheque { get; set; }
        public double vcn_monto_cheque { get; set; }
        public DateTime vcf_fec_venta { get; set; }
        public int vcn_descuento { get; set; }
        public int vcn_producto_insp { get; set; }
        public double vcn_monto_efe { get; set; }
        public string vcc_banco2 { get; set; }
        public string vcf_num_cheque2 { get; set; }
        public DateTime vcf_fec_cheque2 { get; set; }
        public double vcn_monto_cheque2 { get; set; }
        public DateTime vcf_fec_venta2 { get; set; }
        public int vcn_descuento2 { get; set; }
        public int vcn_producto_insp2 { get; set; }
        public string vcc_banco3 { get; set; }
        public string vcf_num_cheque3 { get; set; }
        public DateTime vcf_fec_cheque3 { get; set; }
        public double vcn_monto_cheque3 { get; set; }
        public DateTime vcf_fec_venta3 { get; set; }
        public int vcn_descuento3 { get; set; }
        public int vcn_producto_insp3 { get; set; }
        public string vcn_saldo_envase { get; set; }
        public int vcn_ruta { get; set; }
        public string vcf_movimiento { get; set; }
        public string vcc_clave { get; set; }
        public string vcc_contado { get; set; }
        public string cmpc_clave_sat { get; set; }
        public string vcc_ctapgosat { get; set; }
        public string cmpc_metodopago { get; set; }
        public string cfpc_formapago { get; set; }
        public string fcd_uuid { get; set; }
        public string sEsTeleventa { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoVtaCabecera()
        { }
    }

    public class InfoVtaDetalle
    {
        public int prn_secuencia { get; set; }
        public string vdn_folio { get; set; }
        public int vdn_cliente { get; set; }
        public int run_clave { get; set; }
        public string vdn_producto { get; set; }
        public int vdn_venta { get; set; }
        public double vdn_importe { get; set; }
        public string vdd_descripcion { get; set; }
        public double vdn_precio { get; set; }
        public string vdc_tipo_precio { get; set; }
        public string vdc_tipo_entrada { get; set; }
        public string vdc_hora { get; set; }
        public int vdn_venta_dev { get; set; }
        public string vdn_folio_devolucion { get; set; }
        public int vdn_tipo_promo { get; set; }
        public double lmn_precioneto { get; set; }
        public double lmn_ieps { get; set; }
        public double lmn_preciobruto { get; set; }
        public double lmn_iva { get; set; }
        public double lmn_preciofinal { get; set; }
        public double lmc_porcentaje { get; set; }
        public double lmn_descuento_neto { get; set; }
        public double lmn_iepsdescuento { get; set; }
        public double lmn_descuento_bruto { get; set; }
        public double lmn_ivadescuento { get; set; }
        public double lmn_totaldescuento { get; set; }
        public double lmn_preciolm_neto { get; set; }
        public double lmn_preciolm_ipes { get; set; }
        public double lmn_preciolm_bruto { get; set; }
        public double lmn_preciolm_iva { get; set; }
        public double lmn_preciolm { get; set; }
        public string sEsTeleventa { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoVtaDetalle()
        { }
    }

    public class infoEnvase
    {
        public int run_clave { get; set; }
        public int cln_clave { get; set; }
        public string men_folio { get; set; }
        public string mec_envase { get; set; }
        public int men_saldo_inicial { get; set; }
        public int men_cargo { get; set; }
        public int men_abono { get; set; }
        public int men_venta { get; set; }
        public int men_saldo_final { get; set; }
        public string mec_es_devolucion { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoEnvase()
        {
            run_clave = 0;
            cln_clave = 0;
            men_folio = "";
            mec_envase = "";
            men_saldo_inicial = 0;
            men_cargo = 0;
            men_abono = 0;
            men_venta = 0;
            men_saldo_final = 0;
            sTipoBD = "false";
            sCorreo = "";
        }
    }

    public class infoBonificaciones
    {
        public int run_clave { get; set; }
        public int boc_cliente { get; set; }
        public string boc_folio { get; set; }
        public double boi_documento { get; set; }
        public string boc_tipo { get; set; }
        public string boc_folio_venta { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoBonificaciones()
        {
            run_clave = 0;
            boc_cliente = 0;
            boc_folio = "";
            boi_documento = 0.0;
            boc_tipo = "";
            boc_folio_venta = "";
            sTipoBD = "false";
            sCorreo = "";
        }
    }

    public class infoClientesStatus
    {
        public int run_clave { get; set; }
        public int cln_clave { get; set; }
        public int cln_visitado { get; set; }
        public bool clb_corregido { get; set; }
        public string clc_programa_pago { get; set; }
        public bool clc_cliente_foco { get; set; }
        public bool clc_cliente_primer_impacto { get; set; }

        public int cln_tipo_no_venta { get; set; }
        public double cln_latitud { get; set; }
        public double cln_longitud { get; set; }

        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoClientesStatus()
        {
            run_clave = 0;
            cln_clave = 0;
            cln_visitado = 0;
            clb_corregido = false;
            clc_programa_pago = "";
            clc_cliente_foco = false;
            clc_cliente_primer_impacto = false;
            sTipoBD = "false";
            sCorreo = "";
        }
    }

    public class infoDevoluciones
    {
        public int run_clave { get; set; }
        public int cln_clave { get; set; }
        public int dev_clave { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoDevoluciones()
        {
            run_clave = 0;
            cln_clave = 0;
            dev_clave = 0;
            sTipoBD = "false";
            sCorreo = "";
        }
    }

    public class InfoRespuestas
    {
        public int cln_clave { get; set; }
        public int run_clave { get; set; }
        public int enn_id { get; set; }
        public string enc_respuesta { get; set; }
        public int enn_tipo_respuesta { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoRespuestas()
        {
        }
    }

    public class InfoSolicitudes
    {
        public int dpn_clave { get; set; }
        public string soc_descripcion { get; set; }
        public int cln_clave { get; set; }
        public int run_clave { get; set; }
        public DateTime sod_fecha { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoSolicitudes()
        { }
    }

    public class infoRuta
    {
        public int run_clave { get; set; }
        public string ruc_inicio { get; set; }
        public string ruc_termino { get; set; }
        public int run_ncamion { get; set; }
        public int run_kminicio { get; set; }
        public int run_kmfinal { get; set; }
        public string ruc_version { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoRuta()
        {
            run_clave = 0;
            ruc_inicio = "";
            ruc_termino = "";
            run_ncamion = 0;
            run_kminicio = 0;
            run_kmfinal = 0;
            ruc_version = "";
            sTipoBD = "false";
        }
    }

    public class InfoGps
    {
        public int run_clave { get; set; }
        public int cln_clave { get; set; }
        public string gpd_latitud { get; set; }
        public string gpd_longitud { get; set; }
        public int ctn_tipo_movimiento { get; set; }
        public string gpd_hora { get; set; }
        public string gpc_folio { get; set; }
        public bool gpb_esBase { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoGps()
        {
        }
    }    

    public class infoEmpleados
    {
        public string sRuta { get; set; }
        public string sEmpleado { get; set; }
        public bool bTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoEmpleados()
        {
            sRuta = "";
            sEmpleado = "";
            bTipoBD = false;
            sCorreo = "";
        }
    }

    public class infoPagareClientes
    {
        public int run_clave { get; set; }
        public int cln_clave { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoPagareClientes()
        {            
        }
    }

    public class infoEnvaseSugerido
    {
        public int run_clave { get; set; }
        public int cln_clave { get; set; }
        public string esc_envase { get; set; }
        public int esn_cantidad_vacio { get; set; }
        public int esn_cantidad_lleno { get; set; }
        public string esc_comentario { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public infoEnvaseSugerido()
        {
        }
    }

    public class InfoVentaPagos
    {
        public int vpn_numpago { get; set; }
        public string vcn_folio { get; set; }
        public int vcn_cliente { get; set; }
        public string vcf_movimiento { get; set; }
        public int vcn_ruta { get; set; }
        public string vpc_descripcion { get; set; }
        public string cfpc_formapago { get; set; }
        public string vcc_banco { get; set; }
        public string vpc_tipotarjeta { get; set; }
        public Decimal vcn_monto { get; set; }
        public string vcc_referencia { get; set; }
        public string vpc_nocuenta { get; set; }
        public string vpc_autorizacion { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoVentaPagos()
        {
        }
    }

    public class InfoDocumentosCabecera
    {
        public int vcn_cliente { get; set; }
        public string vcn_folio { get; set; }
        public DateTime vcf_movimiento { get; set; }
        public double vcn_importe { get; set; }
        public string dcc_tipo { get; set; }
        public string vcc_cadena_original { get; set; }

        public int run_clave { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoDocumentosCabecera()
        {
        }
    }

    public class InfoDocumentosDetalle
    {
        public int vcn_cliente { get; set; }        
        public string vcn_folio { get; set; }
        public DateTime vcf_movimiento { get; set; }
        public string vcn_folio_cabecera { get; set; }
        public DateTime vcf_movimiento_cabecera { get; set; }
        public double vcn_monto_pago { get; set; }
        public int ddn_numero_pago { get; set; }
        public string vcc_cadena_original { get; set; }
        public int ddn_cantidad_pagos { get; set; }        
        public string ddc_forma_pago { get; set; }

        public int run_clave { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoDocumentosDetalle()
        {
        }
    }

    public class InfoClientesDatosSurtir
    {
        public int cln_clave { get; set; }
        public string crc_titular { get; set; }
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

        public int run_clave { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoClientesDatosSurtir()
        {
        }
    }

    public class InfoClientesCompetencia
    {
        public string ccc_nombre { get; set; }        
        public string ccc_negocio { get; set; }
        public int ccn_tipo { get; set; }        
        public string ctp_clave { get; set; }
        public string ccn_latitud { get; set; }
        public string ccn_longitud { get; set; }

        public int run_clave { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoClientesCompetencia()
        { }
    }

    public class InfoAnticipos
    {
        public int cln_clave { get; set; }
        public string vcn_folio { get; set; }
        public DateTime vcf_movimiento { get; set; }
        public decimal ann_monto_pago { get; set; }
        public int ann_cantidad_pagos { get; set; }
        public string anc_forma_pago { get; set; }
        public bool anb_nuevo { get; set; }
        public bool anb_relacionado { get; set; }

        public int run_clave { get; set; }
        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoAnticipos()
        { }
    }

    public class InfoTelefonosClientes
    {
        public int cln_clave { get; set; }        
        public string tcc_telefono { get; set; }
        public string tcc_nombre { get; set; }
        public bool tcb_estatus { get; set; }
        public bool tcb_movil { get; set; }        
        public string tct_horarioini { get; set; }        
        public string tct_horariofin { get; set; }        
        public string tcc_comentario { get; set; }
        public int tcn_rutacaptura { get; set; }
        public DateTime tct_fechacaptura { get; set; }

        public string sTipoBD { get; set; }
        public string sCorreo { get; set; }

        public InfoTelefonosClientes()
        { }
    }
}