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
     class venta_detalleSR
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        ///  cliente   OK       folio  OK      articulo   OK
        #region  ventas detalle filtrando por cliente , folio y articulo 
        public venta_detalle VentasxClientexFolioxArt(int iCliente,string sFolio,string sArt)
        {
            // await sl.WaitAsync();

            try
            {
                venta_detalle ven = new venta_detalle();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //string sqlQuery = "select * from venta_detalle"; 

                    ven =  conn.Table<venta_detalle>().Where(i => i.vdn_cliente == iCliente
                                                               && i.vdn_folio == sFolio
                                                               && i.vdn_producto == sArt).FirstOrDefault();
                }

                if (ven is null)
                    ven = new venta_detalle();

                return ven;
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return new venta_detalle();
            }
            finally
            {
                // sl.Release();
            }
        }
        #endregion

        ///  cliente   OK       folio  OK      articulo   X
        #region Búsqueda de la venta actual del cliente por folio
        public async Task<List<venta_detalle>> cargaLiquido(string sIdCliente)
        {
            string sQuery = string.Empty;
            string sFolio = VarEntorno.iFolio.ToString().PadLeft(6, '0');

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    sQuery = "select v.prn_secuencia," +
                             "v.vdn_folio, " +
                             "v.vdn_cliente, " +
                             "v.run_clave, " +
                             "v.vdn_producto, " +
                             "v.vdn_venta, " +
                             "v.vdn_importe, " +
                             "p.ard_descripcion as vdd_descripcion, " +
                             "v.vdn_precio, " +
                             "v.vdc_tipo_precio, " +
                             "v.vdc_tipo_entrada, " +
                             "v.vdc_hora, " +
                             "v.vdn_venta_dev, " +
                             "v.vdn_folio_devolucion " +
                             "from venta_detalle v " +
                             "join productos p on p.arc_clave = v.vdn_producto " +
                             "where v.vdn_cliente = " + sIdCliente + " " +
                             "and v.vdn_folio = '" + sFolio + "'";
                    return conn.Query<venta_detalle>(sQuery);
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Búsqueda de la venta actual del cliente

        ///  cliente   OK       folio  X       articulo   OK
        ///-------------------------------------------------

        ///  cliente   X        folio  OK      articulo   OK
        ///-------------------------------------------------

        ///  cliente   X        folio  X       articulo   OK
        ///-------------------------------------------------

        ///  cliente   X        folio  OK      articulo   X
        #region  se obtien el importe total de una venta realizada por folio  *//
        public decimal fnImporteTotalxFolio(int iFolio)
        {
            try
            {
                decimal iImporteTotal = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select  ifnull(sum(vdn_importe),0) from venta_detalle WHERE vdn_folio = ?  ";
                    iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, iFolio.ToString().PadLeft(6, '0'));
                }

                return iImporteTotal;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }
        #endregion

        ///  cliente   OK       folio  X       articulo   X
        #region  se obtien el importe total de una venta realizada con anterioridad por cliente  *//
        public decimal fnImporteTotalxCliente(int iCliente)
        {
            try
            {
                decimal iImporteTotal = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select sum(vdn_importe) from venta_detalle WHERE vdn_cliente = ?  ";
                    iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, iCliente);
                }

                return iImporteTotal;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }
        #endregion

        ///  cliente   X        folio  X       articulo   X
        #region Se obtiene el Resumen de Venta del Día
        public List<venta_detalle> ConsultaVentasDia()
        {
            // await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //string sqlQuery = "select * from venta_detalle";
                    string sqlQuery = "select v.prn_secuencia," +
                             "v.vdn_folio, " +
                             "v.vdn_cliente, " +
                             "v.run_clave, " +
                             "p.arc_afecta as vdn_producto, " +
                             "v.vdn_venta, " +
                             "v.vdn_importe, " +
                             "p.ard_descripcion as vdd_descripcion, " +
                             "v.vdn_precio, " +
                             "v.vdc_tipo_precio, " +
                             "v.vdc_tipo_entrada, " +
                             "v.vdc_hora, " +
                             "v.vdn_venta_dev, " +
                             "v.vdn_folio_devolucion, " +
                             "v.vdn_tipo_promo "+
                             "from venta_detalle v " +
                             "join productos p on p.arc_clave = v.vdn_producto ";

                    return conn.Query<venta_detalle>(sqlQuery);

                }
            }
            finally
            {
                // sl.Release();
            }
        }
        #endregion Se obtiene el Resumen de Venta del Día
        

        #region Método para obtener el pedido y mostrarlo
        public async Task<List<MostrarPedido>> muestraPedido(string sCliente)
        {
            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select v.vdn_producto as sClaveAfecta, p.ard_descripcion as sDescripcion, " +
                                    "sum(v.vdn_venta) as iVenta, sum(v.vdn_importe) as dImporte " +
                                    "from venta_detalle v " +
                                    "join productos p on p.arc_clave = v.vdn_producto " +
                                    "where v.vdn_cliente = " + sCliente + " " +
                                    "GROUP BY v.vdn_producto, p.ard_descripcion";

                    var lLista = conn.Query<MostrarPedido>(sQuery);

                    return lLista;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método para obtener el pedido y mostrarlo

        //*  actualiza la venta de articulo en venta detalle *//
        public bool fnActualizaProducto(VentaVM vVenta)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        @"UPDATE venta_detalle 
                               SET  vdn_venta   = " + vVenta.iVendido + @", 
                                    vdn_importe = " + vVenta.dPrecio * vVenta.iVendido + @"
                    WHERE (vdn_folio    = '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + @"') and 
                          (vdn_cliente  = " + VarEntorno.vCliente.cln_clave + @") and 
                          (vdn_producto = '" + vVenta.pProducto.arc_clave + @"')";

                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();


                    if (iResultado >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }


        //*  borra  la venta de articulo en venta detalle *//
        public bool fnBorrarProducto(VentaVM vVenta)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        @"delete from venta_detalle                               
                        WHERE (vdn_folio    = '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + @"') and 
                              (vdn_cliente  = " + VarEntorno.vCliente.cln_clave + @") and 
                              (vdn_producto = '" + vVenta.pProducto.arc_clave + @"')";

                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();


                    if (iResultado >= 1)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //* limpia le folio de venta de articulos **/
        public async Task<string> BorrarVenta(string sFolio)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sTipoTicket = string.Empty;
            int iRespuesta;
            //bool bRespuesta;
            #endregion Declaración de Variables

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    conn.BeginTransaction();

                    //borra los registros de la tabla venta cabecera
                    sQuery = "delete from venta_cabecera where vcn_folio = '" + sFolio + "'";
                    SQLiteCommand command = conn.CreateCommand(sQuery);
                    iRespuesta = command.ExecuteNonQuery();

                    if (iRespuesta < 0)
                    {
                        sRespuesta = "No se pudo borrar de la tabla venta cabecera";
                        conn.Rollback();
                    }
                    else
                    {
                        //borra los registros de la tabla venta_detalle
                        sQuery = "delete from venta_detalle where vdn_folio =  '" + sFolio + "'";
                        SQLiteCommand command2 = conn.CreateCommand(sQuery);
                        iRespuesta = command2.ExecuteNonQuery();

                        if (iRespuesta < 0)
                        {
                            sRespuesta = "No se pudo borrar de la tabla venta cabecera";
                            conn.Rollback();
                        }
                        else
                        {
                            conn.Commit();
                            sRespuesta = "Ticket Eliminado";
                        }
                    }
                }
                return sRespuesta;
            }
            finally
            {
                sl.Release();
            }
        }


        public bool GuardaDetalleDEV(List<venta_detalle> lDetalle)
        {
            try
            {
                int i = 1;
                bool o = true;
                productos pProducto = null;
                string sQuery = "";

                EnvaseService EnvFn = new EnvaseService();
                productoSR ProductoFN = new productoSR();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    conn.BeginTransaction();

                    foreach (var det in lDetalle)
                    {
                        if (i > 0 && o == true)
                        {
                            det.vdn_venta_dev = det.vdn_venta;
                            det.vdn_venta = 0;
                            det.run_clave = VarEntorno.iNoRuta;
                            det.vdc_hora = DateTime.Now.ToShortTimeString();
                            det.vdn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                            i = conn.Insert(det);

                            pProducto = null;
                            pProducto = ProductoFN.BuscarProducto(det.vdn_producto);

                            if (pProducto != null)
                            {
                                if (pProducto.arc_envase != "")
                                {
                                    /*
                                    o = EnvFn.fnEnvaseClienteCargo_Abono(VarEntorno.vCliente.cln_clave, pProducto.arc_envase,
                                                                        0, det.vdn_venta_dev,
                                                                        VarEntorno.iFolio.ToString().PadLeft(6, '0'), det.vdn_producto);
                                                                        */
                                    string sValida = "select 1 as existe " +
                                     "from envase_temp " +
                                     "where (mec_envase = '" + pProducto.arc_envase + "') " +
                                     "AND (arc_clave = '" + det.vdn_producto + "')" +
                                     "AND cln_clave = " + VarEntorno.vCliente.cln_clave.ToString();
                                    int iExiste = conn.ExecuteScalar<int>(sValida);

                                    if (iExiste == 1)
                                    {
                                        sQuery = "UPDATE envase_temp SET " +
                                                 "  men_cargo = " + 0 + " " +
                                                 " ,men_abono = " + det.vdn_venta_dev + " " +
                                                 " ,men_folio =  '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + "' " +
                                                 "WHERE (cln_clave = " + VarEntorno.vCliente.cln_clave + ") " +
                                                 "AND (mec_envase = '" + pProducto.arc_envase + "') " +
                                                 "AND (arc_clave = '" + det.vdn_producto + "')";
                                    }
                                    else
                                    {
                                        sQuery = "insert into envase_temp " +
                                                 "(cln_clave, men_folio, mec_envase, men_cargo, men_abono, arc_clave) " +
                                                 "values " +
                                                 "(" + VarEntorno.vCliente.cln_clave.ToString() + ", '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') +
                                                 "', '" + pProducto.arc_envase + "', " + 0 + ", " + det.vdn_venta_dev + ", '" + det.vdn_producto + "')";
                                    }

                                    SQLiteCommand command = conn.CreateCommand(sQuery);

                                    int iResultado = command.ExecuteNonQuery();
                                }

                                else
                                    o = true;
                            }
                            else
                                o = false;

                        }
                        else
                            break;
                    }

                    if (i == 0 || o == false)
                        conn.Rollback();
                    else
                        conn.Commit();
                }
                if (i == 0)
                {
                    VarEntorno.sMensajeError = " Error en guardar un detalle y envase ";
                    return false;
                }
                else
                    return true;
            }
            catch
            {
                VarEntorno.sMensajeError = "Error en guardar detalle ";
                return false;
            }
        }


    }
}
