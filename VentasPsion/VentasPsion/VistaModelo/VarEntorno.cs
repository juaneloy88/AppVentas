using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class VarEntorno
    {
        public static string sTipoVenta { get; set; }
        public static char cTipoVenta { get; set; }
        public static DateTime dFechaVenta { get; set; }
        public static char cDiaVisita { get; set; }
        public static string sVersionApp { get; set; }
        public static int iNoRuta { get; set; }
        public static string sAlmacen { get; set; }
        public static bool bInicioTurno { get; set; }
        public static bool bFinTurno { get; set; }

        public static int iNUsuario { get; set; }

        /*variables para venta */
        public static clientes vCliente { get; set; }
        public static string sMensajeError { get; set; }
        public static int iFolio { get; set; }
        public static decimal dImporteTotal { get; set; }

        public static bool bEsDevolucion { get; set; }
        public static bool bSoloCobrar { get; set; }

        public static CobranzaVM cCobranza { get; set; }

        public static AnticiposVM cAnticipos { get; set; }
        public static bool  bAnticipoRelacionado { get; set; }

        public static vmStatusClientes cBuscarCliente { get; set; }

        public static string sHoraInicio { get; set; }

        public static bool bOperaciones { get; set; }

        public static string sUriConexionEnvio { get; set; }

        public static bool bVisitaAllCtsReparto { get; set; }

        public static bool bTipoBaseDatos { get; set; }

        public static bool bEsTeleventa { get; set; }

        public static OpcionesappVM oOpciones_app { get; set; }

        public static bool bVentaContado { get; set; } //Variable para determinar si la venta es al contado o a crédito

        //public static bool bSaldoPendiente { get; set; } // variable para descuadre de saldo y documentos 

        public static bool bEsDocumentos { get; set; } // variable para de saldo y documentos 

        public static decimal dEfectivo { get; set; }

        public static string sTipoImpresora { get; set; }

        /// <summary>
        /// FUNCIONES GENERALES
        /// </summary>
        public static void LimpiaVariables()
        {
            cCobranza = null;
            cAnticipos = null;
            vCliente = null;
            sMensajeError = "";
            iFolio = 0;
            dImporteTotal = 0;
            dEfectivo =0;
            bTipoBaseDatos = true;

            bVentaContado = false;
            bAnticipoRelacionado = false;
            bEsDevolucion = false;
            //bSaldoPendiente = false;
            bEsDocumentos = false;
        }

        public static int TraeFolio()
        {
            try
            {
                return new rutaSR().GetFolio();
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public static decimal Saldo(clientes Cliente)
        {
            try
            {
                return new fnVentaCabecera().SaldoFinal(Cliente);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -9999;
            }
        }

        public static string Almacen()
        {
            return new rutaSR().GetAlmacen();
        }
    }
}
