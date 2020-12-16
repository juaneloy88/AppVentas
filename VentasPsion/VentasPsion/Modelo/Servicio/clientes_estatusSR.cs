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
    class clientes_estatusSR
    {
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        public bool fnActualizaPagoProgramado(int iCliente)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        @"UPDATE clientes_estatus 
                               SET  clc_programa_pago   =  '1'                                
                        WHERE cln_clave    = " + iCliente;

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

        public int ObtieneClientesSinVenta(int iCliente)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select cln_tipo_no_venta from clientes_estatus " +
                                    "where cln_clave =  " + iCliente;
                    var vValor = conn.ExecuteScalar<int>(sQuery);

                    return vValor;
                }
            }

            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public bool fnActualizaEstatus_Noventa(int iCliente, int iEstatusNoventa, int iEstatusCliente)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    conn.BeginTransaction();

                    var sQuery =
                        @"UPDATE clientes_estatus 
                               SET  cln_tipo_no_venta   = " + iEstatusNoventa + @"                               
                        WHERE cln_clave    = " + iCliente;

                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();

                    if (iResultado >= 1)
                    {
                        sQuery =
                        @"UPDATE clientes_estatus 
                               SET  cln_visitado   = " + iEstatusCliente + @"                              
                        WHERE cln_clave    = " + iCliente + " and cln_visitado NOT IN (1,3) ";

                        command = conn.CreateCommand(sQuery);

                        iResultado = command.ExecuteNonQuery();

                        if (iResultado >= 0)
                        {
                            conn.Commit();
                            return true;
                        }
                        else
                        {
                            conn.Rollback();
                            VarEntorno.sMensajeError = "Error de guardar estatus ";
                            return false;
                        }
                    }
                    else
                    {
                        conn.Rollback();
                        VarEntorno.sMensajeError = "Error de guardar no venta ";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        #region Método para conocer si el cliente ya tiene el status de entrega
        public async Task<string> validaStatusCliente(string sCliente)
        {
            string sRespuesta = string.Empty;

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select cln_visitado from clientes_estatus where cln_clave = ?";
                    int iStatus = conn.ExecuteScalar<int>(sQuery, sCliente);

                    switch (iStatus)
                    {
                        case 1:
                            sRespuesta = "Cliente Visitado";
                            break;
                        /* case 2:
                             sRespuesta = "Cliente Visitado";
                             break;*/
                        case 3:
                            sRespuesta = "Cliente Visitado";
                            break;
                        case 0:
                            sRespuesta = "Cliente NO Visitado";
                            break;
                        default:
                            sRespuesta = "Cliente NO Visitado";
                            break;
                    }

                    return sRespuesta;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método para conocer si el cliente ya tiene el status de entrega

        public clientes_estatus clientes_estatusCL(int sClave)
        {
            try
            {
                clientes_estatus cli_status = new clientes_estatus();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    cli_status = conn.Table<clientes_estatus>().Where(i => i.cln_clave == sClave).FirstOrDefault();

                }

                return cli_status;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return new clientes_estatus();
            }
        }


        
        public bool fnFocoCliente(int sClave)
        {
            try
            {
                bool bFoco = false;
                int bActu;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select clc_cliente_foco from clientes_estatus WHERE cln_clave = ? ";
                    bFoco = conn.ExecuteScalar<bool>(sQuery, sClave.ToString());

                    sQuery = "Select clc_actua_foco_cooler from clientes_estatus WHERE cln_clave = ? ";
                    bActu = conn.ExecuteScalar<int>(sQuery, sClave.ToString());
                }

                return bFoco;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public bool fnCoolerCliente(int sClave)
        {
            try
            {
                bool bCooler = false;
                int bActu;
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select clc_cliente_primer_impacto from clientes_estatus WHERE cln_clave = ? ";
                    bCooler = conn.ExecuteScalar<bool>(sQuery, sClave);

                    sQuery = "Select clc_actua_foco_cooler from clientes_estatus WHERE cln_clave = ? ";
                    bActu = conn.ExecuteScalar<int>(sQuery, sClave.ToString());
                }

                return bCooler;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        
        public bool fnActualizaFoco(int iCliente, bool bEstatus)
        {
            try
            {
                string sFoco = "";

                if (bEstatus)
                    sFoco = "1";
                else
                    sFoco = "0";
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "update clientes_estatus set  clc_cliente_foco = '" + sFoco + "' ,clc_actua_foco_cooler = '1'  WHERE cln_clave = " + iCliente.ToString();
                    /*
                     sQuery =
                        @"UPDATE clientes_estatus 
                               SET  clc_cliente_foco   = '" + bEstatus + @"' 
,clc_actua_foco_cooler = 'true'
                    WHERE (cln_clave = '" + iCliente + @"') ";
                    */

                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();

                    //if (conn.Update(sQuery) == -1)
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

        public bool fnActualizaCooler(int iCliente, bool bEstatus)
        {
            try
            {
                string sCooler = "";

                if (bEstatus)
                    sCooler = "1";
                else
                    sCooler = "0";
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "update clientes_estatus set  clc_cliente_primer_impacto = '" + sCooler + "' ,clc_actua_foco_cooler = '1'  WHERE cln_clave = " + iCliente.ToString();
                    //  sQuery = "update clientes_estatus set  clc_programa_pago =  '1'  where cln_clave    = " + iCliente;


                    SQLiteCommand command = conn.CreateCommand(sQuery);

                    int iResultado = command.ExecuteNonQuery();

                    //if (conn.Update(sQuery) == -1)
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

        public bool fnActualizaGPS(int iCliente, double dLatitud, double dLongitud)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "update clientes_estatus set  cln_latitud = '" + dLatitud + "' ,cln_longitud = '" + dLongitud + "'  WHERE cln_clave = " + iCliente.ToString();

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
    }
}
