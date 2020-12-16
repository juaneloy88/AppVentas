using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class vmCapturaEnvase
    {
        fnCapturaEnvase fncapturaEnvase = new fnCapturaEnvase();

        public List<CapturaEnvase> lEnvaseCliente = new List<CapturaEnvase>();

        #region Valida si el cliente ya tiene una captura de envase sugerido
        public bool existeCapturaEnvaseSugerido(string sCliente)
        {
            bool bRespuesta = fncapturaEnvase.existeCapturaEnvaseSugerido(sCliente);

            return bRespuesta;
        }
        #endregion Valida si el cliente ya tiene una captura de envase sugerido

        #region Borrar información del cliente de la tabla envase_sugerido
        public bool borrarEnvaseSugerido(string sCliente)
        {
            bool bRespuesta = fncapturaEnvase.borrarEnvaseSugerido(sCliente);

            return bRespuesta;
        }
        #endregion Borrar información del cliente de la tabla envase_sugerido

        #region Método para obtener el listado de envase del cliente con el cargo calculado
        public async Task<List<CapturaEnvase>> obtieneEnvase(string sCliente)
        {
            var vListaCargo = await fncapturaEnvase.calculaCargoEnvaseCliente(sCliente);

            //if (fncapturaEnvase.FnValidaExisteEnvSugerido())
            //{
            //    foreach (CapturaEnvase capturaEnv in vListaCargo)
            //    {
            //        CapturaEnvase capturaEnvase = new CapturaEnvase();
            //        capturaEnvase.cln_clave = capturaEnv.cln_clave;
            //        capturaEnvase.mec_envase = capturaEnv.mec_envase;
            //        capturaEnvase.men_saldo_inicial = capturaEnv.men_saldo_inicial;
            //        capturaEnvase.men_cargo = capturaEnv.men_cargo;


            //        capturaEnvase.men_abono = capturaEnv.esn_cantidad_vacio;

            //        capturaEnvase.men_saldo_final =
            //                (capturaEnv.men_saldo_inicial + capturaEnv.men_cargo) -
            //                (Convert.ToInt32(capturaEnv.esn_cantidad_vacio) + Convert.ToInt32(capturaEnv.men_venta));

            //        capturaEnvase.esn_cantidad_vacio = capturaEnv.esn_cantidad_vacio;
            //        capturaEnvase.esn_cantidad_lleno = capturaEnv.esn_cantidad_lleno;

            //        capturaEnvase.men_venta = capturaEnv.men_venta;

            //        this.lEnvaseCliente.Add(capturaEnvase);
            //    }
            //}
            //else
            {
                foreach (CapturaEnvase capturaEnv in vListaCargo)
                {
                    CapturaEnvase capturaEnvase = new CapturaEnvase();
                    capturaEnvase.cln_clave = capturaEnv.cln_clave;
                    capturaEnvase.mec_envase = capturaEnv.mec_envase;
                    capturaEnvase.men_saldo_inicial = capturaEnv.men_saldo_inicial;
                    capturaEnvase.men_cargo = capturaEnv.men_cargo;

                    if (capturaEnv.men_saldo_inicial == 0 & capturaEnv.men_cargo == 0)
                        capturaEnvase.men_abono = "0";
                    else
                        capturaEnvase.men_abono = capturaEnv.men_abono;

                    capturaEnvase.men_saldo_final = capturaEnv.men_saldo_final;

                    capturaEnvase.esn_cantidad_vacio = capturaEnv.esn_cantidad_vacio;
                    capturaEnvase.esn_cantidad_lleno = capturaEnv.esn_cantidad_lleno;

                    capturaEnvase.men_venta = capturaEnv.men_venta;

                    this.lEnvaseCliente.Add(capturaEnvase);
                }                
            }            

            return this.lEnvaseCliente;

        }
        #endregion Método para obtener el listado de envase del cliente con el cargo calculado

        #region Método que actualiza el campo del abono a la lista de CapturaEnvase
        public List<CapturaEnvase> actualizandoAbono(string sMarca, int iAbono)
        {
            var vValida = this.lEnvaseCliente.Find(x => x.mec_envase.Contains(sMarca));

            if (vValida != null)
            {
                this.lEnvaseCliente.Remove(vValida);

                CapturaEnvase capturaEnvase = new CapturaEnvase();
                capturaEnvase.cln_clave = vValida.cln_clave;
                capturaEnvase.mec_envase = vValida.mec_envase;
                capturaEnvase.men_saldo_inicial = vValida.men_saldo_inicial;
                capturaEnvase.men_cargo = vValida.men_cargo;
                capturaEnvase.men_abono = iAbono.ToString();
                capturaEnvase.men_venta = vValida.men_venta;
                capturaEnvase.men_saldo_final = (vValida.men_saldo_inicial + vValida.men_cargo) - (iAbono + vValida.men_venta);
                this.lEnvaseCliente.Add(capturaEnvase);
            }

            return this.lEnvaseCliente.OrderBy(x => x.mec_envase).ToList();
        }
        #endregion Método que actualiza el campo del abono a la lista de CapturaEnvase

        #region Método que actualiza los campos de Vacio y Lleno en la captura del Inventario de Envase
        public List<CapturaEnvase> actualizaCapturaVacioLleno(string sMarca, int iCapturaVacio, int iCapturaLleno)
        {
            var vValida = this.lEnvaseCliente.Find(x => x.mec_envase.Contains(sMarca));

            if (vValida != null)
            {
                this.lEnvaseCliente.Remove(vValida);

                CapturaEnvase capturaEnvase = new CapturaEnvase();
                capturaEnvase.cln_clave = vValida.cln_clave;
                capturaEnvase.mec_envase = vValida.mec_envase;
                capturaEnvase.men_saldo_inicial = vValida.men_saldo_inicial;
                capturaEnvase.men_cargo = vValida.men_cargo;
                capturaEnvase.esn_cantidad_vacio = iCapturaVacio.ToString();
                capturaEnvase.esn_cantidad_lleno = iCapturaLleno.ToString();

                this.lEnvaseCliente.Add(capturaEnvase);
            }

            return this.lEnvaseCliente.OrderBy(x => x.mec_envase).ToList();
        }
        #endregion Método que actualiza los campos de Vacio y Lleno en la captura del Inventario de Envase

        #region Método para obtener el detalle de envase del cliente
        public async Task<List<envase_temp>> detEnvaseCliente(string sCliente)
        {
            return await fncapturaEnvase.detEnvaseCliente(sCliente);
        }
        #endregion  para obtener el detalle de envase del cliente

        #region Método para obtener el detalle de envase de la ruta
        public async Task<List<CapturaEnvase>> detEnvaseRuta()
        {
            return await fncapturaEnvase.detEnvaseRuta();
        }
        #endregion Método para obtener el detalle de envase de la ruta
    }

    public class CapturaEnvase
    {
        public int cln_clave { get; set; }
        public string mec_envase { get; set; }
        public int men_saldo_inicial { get; set; }
        public int men_cargo { get; set; }
        public string men_abono { get; set; }
        public string esn_cantidad_vacio { get; set; }
        public string esn_cantidad_lleno { get; set; }
        public int men_venta { get; set; }
        public int men_saldo_final { get; set; }

        public CapturaEnvase()
        {
            cln_clave = 0;
            mec_envase = "";
            men_saldo_inicial = 0;
            men_cargo = 0;
            men_abono = null;
            esn_cantidad_vacio = null;
            esn_cantidad_lleno = null;
            men_venta = 0;
            men_saldo_final = 0;
        }
    }
}