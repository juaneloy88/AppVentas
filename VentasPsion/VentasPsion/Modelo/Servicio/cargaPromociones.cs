//using Android.Content;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class cargaPromociones
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        VentaVM _venta = new VentaVM();
        existenciaSR _existencia = new existenciaSR();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        #region Método que busca las promociones activas
        public async Task<List<promociones>> buscaPromociones()
        {
            await sl.WaitAsync();

            try
            {
                #region Se obtiene la fecha actual
                DateTime dFechaActual = DateTime.Now;
                dFechaActual.Date.ToShortDateString();
                #endregion Se obtiene la fecha actual

                #region Búsqueda de promociones vigentes de acuerdo a la fecha actual
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    List<promociones> lListaPromo = conn.Query<promociones>("select DISTINCT ppn_numero_promocion, ppc_descripcion from promociones where  ppc_inicia <= ?", new DateTime(dFechaActual.Date.Year, dFechaActual.Date.Month, dFechaActual.Date.Day).Ticks, new DateTime(dFechaActual.Date.Year, dFechaActual.Date.Month, dFechaActual.Date.Day).Ticks);

                    return lListaPromo;
                }
                #endregion Búsqueda de promociones vigentes de acuerdo a la fecha actual
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método que busca las promociones activas

        #region Consulta para obtener el detalle de la promoción
        public async Task<List<MostrarPromociones>> listaVentaRegalo(string sID)
        {
            await sl.WaitAsync();

            try
            {   

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    #region Se Obtiene la lista de marcas de acuerdo al ID de promoción selecciondo
                    string sqlQuery = "select DISTINCT p.ppn_numero_promocion, " +
                                      "p.ppc_codigo_venta as arc_clave_venta, " +
                                      "p.ppn_cantidad_venta as arn_cantidad_venta, " +
                                      "a.ard_corta as ard_corta_vta, " +
                                      "p.ppc_codigo_regalo as arc_clave_regalo, " +
                                      "p.ppn_cantidad_regalo as arn_cantidad_regalo, " +
                                      "aa.ard_corta as ard_corta_regalo, " +
                                      "p.ppc_tipo as ptc_tipo " +
                                      "from promociones p " +
                                      "join productos a on a.arc_clave = p.ppc_codigo_venta " +
                                      "join productos aa on aa.arc_clave = p.ppc_codigo_regalo " +
                                      "where ppn_numero_promocion = " + sID;

                    List<MostrarPromociones> lClavesPromociones = conn.Query<MostrarPromociones>(sqlQuery);
                    #endregion Se Obtiene la lista de marcas de acuerdo al ID de promoción selecciondo

                    return lClavesPromociones;

                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Consulta para obtener el detalle de la promoción

        #region Método que válida que las cantidades de venta y de regalo de la promociones esten correctas
        public async Task<List<promociones>> validacionPromo(string sID)
        {
            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {                    
                    string sQuery = "select DISTINCT ppn_cantidad_venta, ppn_cantidad_regalo " +
                                    "from promociones " +
                                    "where ppn_numero_promocion = " + sID;
                    var vCantidades = conn.Query<promociones>(sQuery);

                    return vCantidades;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método que válida que las cantidades de venta y de regalo de la promociones esten correctas

        #region Método que Guarda los registros tanto de Venta como de Regalo en la tabla de venta_detalle
        public async Task<string> GuardaVenta(List<MostrarPromociones> lMuestraPromociones)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            string sClaveVenta, sClaveRegalo, sExistencia = string.Empty;
            int iNumPromo = 0;
            int iVenta, iRegalo;
            bool bResultado;
            #endregion Declaración de Variables

            //Se obtiene la lista de los articulos de la promoción
            var lListaPromociones = lMuestraPromociones;

            #region Se barre la lista de artículos de venta para saber si ya se ha capturado un producto en otra promoción
            foreach (MostrarPromociones pVenta in lListaPromociones)
            {
                //Se obtiene la clave y cantidad de venta                
                sClaveVenta = pVenta.arc_clave_venta.ToString().Trim();
                iVenta = pVenta.arn_venta;

                //Se Válida que la venta no este en Cero(0)
                if (iVenta != 0)
                {
                    //Se inserta el producto en la clase de Venta
                    if (_venta.fnCargaProducto(sClaveVenta))
                    {                        
                        //Se válida si el registro ya existe para ser actualizado o insertado
                        if (_venta.bArtActualizado)
                        {
                            sRespuesta = "Ya existe una captura con artículo en promoción";
                            break;
                        }
                        else
                        {                                
                            sRespuesta = "Ok";
                        }
                        
                    }
                }
            }
            #endregion Se barre la lista de artículos de venta para saber si ya se ha capturado un producto en otra promoción

            if (sRespuesta == "Ok")
            {
                #region Se barre la lista de artículos de Regalo para saber si ya se ha capturado un producto en otra promoción
                foreach (MostrarPromociones pRegalo in lListaPromociones)
                {
                    //Se obtiene la clave y cantidad de Regalo                    
                    sClaveRegalo = pRegalo.arc_clave_regalo.ToString().Trim();
                    iRegalo = pRegalo.arn_regalo;

                    //Se Válida que el regalo no este en Cero(0)
                    if (iRegalo != 0)
                    {
                        //Se inserta el producto en la clase de Venta
                        if (_venta.fnCargaProducto(sClaveRegalo))
                        {
                            //Se válida si el registro ya existe para ser actualizado o insertado
                            if (_venta.bArtActualizado)
                            {
                                sRespuesta = "Ya existe una captura con artículo en promoción";
                                break;
                            }
                            else
                            {                                
                                    sRespuesta = "Ok";
                            }
                        }
                    }
                }
                #endregion Se barre la lista de artículos de Regalo para saber si ya se ha capturado un producto en otra promoción

                if (sRespuesta == "Ok")
                {
                    #region Se barre la lista de articulos de Venta
                    foreach (MostrarPromociones pVenta in lListaPromociones)
                    {
                        //Se obtiene la clave y cantidad de venta
                        iNumPromo = pVenta.ppn_numero_promocion;
                        sClaveVenta = pVenta.arc_clave_venta.ToString().Trim();
                        iVenta = pVenta.arn_venta;

                        //Se Válida que la venta no este en Cero(0)
                        if (iVenta != 0)
                        {
                            //Se inserta el producto en la clase de Venta
                            if (_venta.fnCargaProducto(sClaveVenta))
                            {
                                //Se obtiene la existencia
                                sExistencia = _existencia.BuscarExistencia(sClaveVenta).ToString();

                                //Se compara la venta con la existencia
                                _venta.iVendido = iVenta;

                                //if (Convert.ToInt32(_venta.iVendido) <= Convert.ToInt32(sExistencia))
                                {
                                    //Se válida si el registro ya existe para ser actualizado o insertado
                                    if (_venta.bArtActualizado)
                                    {
                                        sRespuesta = "Ya existe una captura con artículo en promoción";
                                        break;
                                        //Se actualiza el registro
                                        //bResultado = _venta.fnActualizaVenta();

                                        //if (bResultado)
                                        //{
                                        //    sRespuesta = "Venta Actualizada";
                                        //}
                                        //else
                                        //{
                                        //    sRespuesta = VarEntorno.sMensajeError;
                                        //}
                                    }
                                    else
                                    {
                                        //Se inserta el registro
                                        bResultado = _venta.fnGuardarVenta(iNumPromo);

                                        if (bResultado)
                                        {
                                            if (_venta.pProducto.arc_envase.ToString().Length > 0 && VarEntorno.cTipoVenta == 'A')
                                            {
                                                int iAbono = 0;

                                                if (_venta.fnActualizaEnvase(sClaveVenta, iVenta, iAbono) == false)
                                                    sRespuesta = VarEntorno.sMensajeError;
                                                else
                                                    sRespuesta = "Venta Guardada";
                                            }
                                            else
                                                sRespuesta = "Venta Guardada";
                                        }
                                        else
                                        {
                                            sRespuesta = VarEntorno.sMensajeError;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion Se barre la lista de articulos de Venta

                    #region Se barre la lista de articulos de Regalo
                    foreach (MostrarPromociones pRegalo in lListaPromociones)
                    {
                        //Se obtiene la clave y cantidad de Regalo
                        iNumPromo = pRegalo.ppn_numero_promocion;
                        sClaveRegalo = pRegalo.arc_clave_regalo.ToString().Trim();
                        iRegalo = pRegalo.arn_regalo;

                        //Se Válida que el regalo no este en Cero(0)
                        if (iRegalo != 0)
                        {
                            //Se inserta el producto en la clase de Venta
                            if (_venta.fnCargaProducto(sClaveRegalo))
                            {
                                _venta.iVendido = pRegalo.arn_regalo;

                                //Se válida si el registro ya existe para ser actualizado o insertado
                                if (_venta.bArtActualizado)
                                {
                                    sRespuesta = "Ya existe una captura con artículo en promoción";
                                    break;
                                    //bResultado = _venta.fnActualizaVenta();

                                    //if (bResultado)
                                    //{
                                    //    sRespuesta = "Venta Actualizada";
                                    //}
                                    //else
                                    //{
                                    //    sRespuesta = VarEntorno.sMensajeError;
                                    //}
                                }
                                else
                                {
                                    bResultado = _venta.fnGuardarVenta(iNumPromo);

                                    if (bResultado)
                                    {
                                        if (_venta.pProducto.arc_envase.ToString().Length > 0 && VarEntorno.cTipoVenta == 'A')
                                        {
                                            int iAbono = 0;

                                            if (_venta.fnActualizaEnvase(sClaveRegalo, iRegalo, iAbono) == false)
                                                sRespuesta = VarEntorno.sMensajeError;
                                            else
                                                sRespuesta = "Venta Guardada";
                                        }
                                        else
                                            sRespuesta = "Venta Guardada";
                                    }
                                    else
                                    {
                                        sRespuesta = VarEntorno.sMensajeError;
                                    }
                                }
                            }
                        }
                    }
                    #endregion Se barre la lista de articulos de Regalo
                }
            }

            return sRespuesta;
        }
        #endregion Método que Guarda los registros tanto de Venta como de Regalo en la tabla de venta_detalle


    }
}
