using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class fnCapturaEnvase
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();
        fnVentaDetalle fnventaDetalle = new fnVentaDetalle();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        #region Valida si el cliente ya tiene una captura de envase sugerido
        public bool existeCapturaEnvaseSugerido(string sCliente)
        {
            bool brespuesta = false;

            using (SQLiteConnection conn = cODBC.CadenaConexion())
            {
                string sQuery = "select 1 as existe from envase_sugerido where cln_clave = " + sCliente + " group by 1";
                int iExiste = conn.ExecuteScalar<int>(sQuery);

                if (iExiste == 1)
                    brespuesta = true;
                else
                    brespuesta = false;
            }

            return brespuesta;
        }
        #endregion Valida si el cliente ya tiene una captura de envase sugerido

        #region Borrar información del cliente de la tabla envase_sugerido
        public bool borrarEnvaseSugerido(string sCliente)
        {
            bool bRespuesta = false;

            using (SQLiteConnection conn = cODBC.CadenaConexion())
            {
                string sQuery = "delete from envase_sugerido where cln_clave = " + sCliente;
                SQLiteCommand command = conn.CreateCommand(sQuery);
                command.ExecuteNonQuery();

                bRespuesta = true;
            }

            return bRespuesta;
        }
        #endregion Borrar información del cliente de la tabla envase_sugerido

        #region Cálcula el cargo del envase
        public async Task<List<CapturaEnvase>> calculaCargoEnvaseCliente(string sCliente)
        {
            string sQuery = string.Empty;

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    sQuery = "select e.cln_clave, e.mec_envase, " +
                             "e.men_saldo_inicial, " +
                             "sum(vdn_venta) as men_cargo, " +
                             "null as men_abono, " +
                             "ifnull(s.esn_cantidad_vacio, 0) as esn_cantidad_vacio, " +
                             "ifnull(s.esn_cantidad_lleno, 0) as esn_cantidad_lleno, " +
                             "e.men_venta ," +
                             "case when sum(vdn_venta) >= 1 then " +
                             "((e.men_saldo_inicial + sum(vdn_venta)) - (e.men_abono + e.men_venta)) else " +
                             "((e.men_saldo_inicial) - (e.men_abono + e.men_venta)) end as men_saldo_final " +
                             "from envase e " +
                             "join productos p on p.arc_envase = e.mec_envase " +
                             "left join venta_detalle v on v.vdn_producto = p.arc_clave and v.vdn_cliente = e.cln_clave " +
                             "left join envase_sugerido s on s.cln_clave = e.cln_clave and s.esc_envase = p.arc_envase " +
                             "where e.cln_clave = " + sCliente + " " +
                             "group by e.cln_clave, e.mec_envase, e.men_saldo_inicial, e.men_abono, e.men_venta " +
                             "order by e.mec_envase";
                    var vListaCalculada = conn.Query<CapturaEnvase>(sQuery);

                    return vListaCalculada;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Cálcula el cargo del envase

        #region Válida si el cliente tiene una captura previa de envase_sugerido
        public bool FnValidaExisteEnvSugerido()
        {
            bool bBandera = false;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {                    
                    string sQuery = "select 1 as existe from envase_sugerido where cln_clave = " + VarEntorno.vCliente.cln_clave + " group by 1;";
                    int iExiste = conn.ExecuteScalar<int>(sQuery);

                    if (iExiste == 1)
                        bBandera = true;
                }
            }
            catch
            {
                bBandera = false;
            }

            return bBandera;
        }
        #endregion Válida si el cliente tiene una captura previa de envase_sugerido

        #region Método para obtener el folio de venta_detalle
        public string obtenerFolio(int iCliente)
        {
            string sFolio = fnventaDetalle.fnFoliodeCliente(iCliente);

            return sFolio;
        }
        #endregion Método para obtener el folio de venta_detalle

        #region Método para Válidar si existen valores nulos en el campo del Abono
        public async Task<string> validaAbonoNulos(List<CapturaEnvase> vListaEnvaseMod)
        {
            string sRespuesta = string.Empty;

            await sl.WaitAsync();

            try
            {
                foreach (var capturaEnv in vListaEnvaseMod)
                {
                    if (capturaEnv.men_abono == null)
                    {
                        sRespuesta = "Faltan Abonos por capturar";
                        break;
                    }
                    else
                    {
                        sRespuesta = "Ok";
                    }
                }
            }
            finally
            {
                sl.Release();
            }

            return sRespuesta;
        }
        #endregion Método para Válidar si existen valores nulos en el campo del Abono

        #region Método que Aplica la captura directamente a la tabla de envase
        public async Task<string> aplicarCaptura(List<CapturaEnvase> vListaEnvaseMod, string sComentario)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            string sMarca = string.Empty;
            string sQuery = string.Empty;
            string sFolio = string.Empty;
            int iCliente = 0;
            int iCargo = 0;
            int iAbono = 0;
            int iSaldoFinal = 0;
            #endregion Declaración de Variables

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    foreach (var capturaEnv in vListaEnvaseMod)
                    {
                        iCliente = capturaEnv.cln_clave;
                        sMarca = capturaEnv.mec_envase;
                        iCargo = capturaEnv.men_cargo;

                        if (!String.IsNullOrEmpty(sComentario))
                            iAbono = 0;
                        else
                            iAbono = Convert.ToInt32(capturaEnv.men_abono);

                        iSaldoFinal = capturaEnv.men_saldo_final;
                        sFolio = obtenerFolio(iCliente);
                        
                        sQuery = "update envase set " +
                                 "men_folio = " + VarEntorno.iFolio.ToString().PadLeft(6, '0') + ", " +
                                 "men_cargo = " + iCargo.ToString() + ", " +
                                 "men_abono = " + iAbono.ToString() + ", " +
                                 "men_saldo_final = " + iSaldoFinal.ToString() + " " +
                                 "where cln_clave = " + iCliente.ToString() + " " +
                                 "and mec_envase = '" + sMarca + "'";
                        SQLiteCommand command = conn.CreateCommand(sQuery);
                        command.ExecuteNonQuery();
                    }

                    if (!String.IsNullOrEmpty(sComentario))
                    {
                        sQuery = "update envase_sugerido set " +
                                 "esc_comentario = '" + sComentario + "' " +
                                 "where cln_clave = " + VarEntorno.vCliente.cln_clave + ";";
                        SQLiteCommand command = conn.CreateCommand(sQuery);
                        command.ExecuteNonQuery();
                    }                    

                    sRespuesta = "Captura Guardada Correctamente";
                }

                return sRespuesta;
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método que Aplica la captura directamente a la tabla de envase

        #region Método que Guarda la captura del envase sugerido a la tabla de envase_sugerido
        public async Task<string> GuardarEnvaseSugerido(List<CapturaEnvase> vListaEnvaseMod)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            string sMarca = string.Empty;
            string sQuery = string.Empty;
            string sFolio = string.Empty;
            int iCliente = 0;
            int iCargo = 0;
            int iCapturaVacio = 0;
            int iCapturaLLeno = 0;
            int iSaldoFinal = 0;
            #endregion Declaración de Variables

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    foreach (var capturaEnv in vListaEnvaseMod)
                    {
                        iCliente = capturaEnv.cln_clave;
                        sMarca = capturaEnv.mec_envase;
                        iCargo = capturaEnv.men_cargo;
                        iCapturaVacio = Convert.ToInt32(capturaEnv.esn_cantidad_vacio);
                        iCapturaLLeno = Convert.ToInt32(capturaEnv.esn_cantidad_lleno);
                        iSaldoFinal = capturaEnv.men_saldo_final;
                        sFolio = obtenerFolio(iCliente);

                        //sQuery = "update envase set men_folio = '" + sFolio + "', " +
                        sQuery = "insert into envase_sugerido (cln_clave, esc_envase, esn_cantidad_vacio, esn_cantidad_lleno) " +
                                 "values (" + iCliente.ToString() + ", '" + sMarca + "', " + iCapturaVacio.ToString() + ", " + iCapturaLLeno.ToString() + ")";
                        SQLiteCommand command = conn.CreateCommand(sQuery);
                        command.ExecuteNonQuery();
                    }

                    sRespuesta = "Captura Guardada Correctamente";
                }

                return sRespuesta;
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método que Guarda la captura del envase sugerido a la tabla de envase_sugerido

        #region Método para obtener el detalle de envase del cliente
        public async Task<List<envase_temp>> detEnvaseCliente(string sCliente)
        {
            List<envase_temp> lResumenEnvase = null;
            string sQuery = string.Empty;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                                       

                    sQuery = "select mec_envase, men_saldo_inicial, men_cargo, men_abono, men_venta, men_saldo_final " +
                             "from (" +
                             "select e.mec_envase, " +
                             "ifnull(e.men_saldo_inicial, 0) as men_saldo_inicial, " +
                             "ifnull(e.men_cargo,0) + sum(ifnull(temp.men_cargo,0)) as men_cargo, " +
                             "ifnull(e.men_abono,0) + sum(ifnull(temp.men_abono,0)) as men_abono, " +
                             "ifnull(e.men_venta,0) + sum(ifnull(temp.men_venta,0)) as men_venta, " +
                             "((ifnull(e.men_saldo_inicial, 0) + ifnull(e.men_cargo,0) + sum(ifnull(temp.men_cargo,0))) - (ifnull(e.men_abono,0) + sum(ifnull(temp.men_abono,0)) + ifnull(e.men_venta,0) + sum(ifnull(temp.men_venta,0)))) as men_saldo_final " +
                             "from envase e " +
                             "left join envase_temp temp on e.cln_clave = temp.cln_clave and e.mec_envase = temp.mec_envase " +
                             "where e.cln_clave = " + sCliente + " " +
                             "group by e.mec_envase) as det " +
                             "where (men_saldo_inicial <> 0 " +
                             "or men_cargo <> 0 " +
                             "or men_abono <> 0 " +
                             "or men_venta <> 0 " +
                             "or men_saldo_final <> 0) " +
                             "order by mec_envase";

                    lResumenEnvase = conn.Query<envase_temp>(sQuery);
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                lResumenEnvase = null;
            }

            return lResumenEnvase;
        }
        #endregion Método para obtener el detalle de envase del cliente

        #region Método para obtener el detalle de envase de la ruta
        public async Task<List<CapturaEnvase>> detEnvaseRuta()
        {
            string sQuery = string.Empty;

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    sQuery = "select 0 as cln_clave, mec_envase, sum(men_saldo_inicial) as men_saldo_inicial, " +
                             "sum(men_cargo) as men_cargo, " +
                             "sum(men_abono) as men_abono, " +
                             "sum(men_venta) as men_venta, " +
                             "((sum(men_saldo_inicial) + sum(men_cargo)) - (sum(men_abono) + sum(men_venta))) as men_saldo_final " +
                             "from envase " +
                             "where (men_saldo_inicial <> 0 " +
                             "or men_cargo <> 0 " +
                             "or men_abono <> 0 " +
                             "or men_venta <> 0 " +
                             "or men_saldo_final <> 0) " +
                             "group by mec_envase " +
                             "order by mec_envase";
                    var vDetEnvase = conn.Query<CapturaEnvase>(sQuery);

                    return vDetEnvase;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método para obtener el detalle de envase de la ruta

        
    }
}