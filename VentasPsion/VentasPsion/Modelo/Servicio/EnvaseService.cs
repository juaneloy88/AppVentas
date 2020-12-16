using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class EnvaseService
    {
        conexionDB cODBC = new conexionDB();

        /*Clase interna usada para obtener los datos de los Envases que podría abonar un Cliente específico */
        class envaseCliente
        {
            public int cln_clave { get; set; }
            [MaxLength(1)]
            public string mec_envase { get; set; }
            [MaxLength(50)]
            public string ard_descripcion { get; set; }
            public int men_saldo_inicial { get; set; }
            public int men_saldo_final { get; set; }
        }

        /*Método para guardar el Abono de Envase de un Cliente específico*/
        public StatusService FtnGuardarAbonoEnvase(int iIdClienteParam, char cTipoEnvaseParam, int iCantEnvaseParam)
        {
            var dbConexion = new conexionDB().CadenaConexion();

            using (SQLiteConnection conn = dbConexion)
            {
                try
                {
                    /*Obtiene el Folio del Ticket para asignarle al Abono de Envase*/
                    int iFolio = new rutaSR().GetFolio();

                    if (iFolio == -999)
                    {
                        return new StatusService
                        {
                            status = false,
                            mensaje = "El Folio no pudo ser obtenido correctamente."
                        };
                    }

                    string sFolio = iFolio.ToString().PadLeft(6, '0');

                    /*Incrementa el Folio en 1 para el siguiente Ticket*/
                    if (new rutaSR().IncFolio() == false)
                    {
                        return new StatusService
                        {
                            status = false,
                            mensaje = "El Folio no pudo ser incrementado correctamente."
                        };
                    }
                    
                    string sSql = "UPDATE envase SET " +
                                  "men_folio = '" + sFolio + "', " +
                                  "men_saldo_final = IFNULL(men_saldo_inicial, 0) + (IFNULL(men_cargo, 0) + " + iCantEnvaseParam + ") - IFNULL(men_abono, 0) - IFNULL(men_venta, 0), " +
                                  "men_abono = (men_abono + " + iCantEnvaseParam + ") " +
                                  "WHERE (cln_clave = " + iIdClienteParam + ") " +
                                  "AND (mec_envase = '" + cTipoEnvaseParam.ToString() + "');";

                    SQLiteCommand command = conn.CreateCommand(sSql);

                    int iResultado = command.ExecuteNonQuery();

                    if (iResultado >= 1)
                    {
                        return new StatusService
                        {
                            status = true,
                            mensaje = "El Abono de Envase del Cliente fue guardado correctamente.",
                            valor = sFolio
                        };
                    }
                    else
                    {
                        return new StatusService
                        {
                            status = false,
                            mensaje = "El Abono de Envase del Cliente no pudo ser guardado."
                        };
                    }
                }
                catch (SQLiteException exc)
                {
                    return new StatusService
                    {
                        status = false,
                        mensaje = "Error: " + exc.Message
                    };
                }
                catch (Exception exc)
                {
                    return new StatusService
                    {
                        status = false,
                        mensaje = "Error: " + exc.Message
                    };
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }

        /*Método para validar si ya fue guardado el Abono de un cierto Tipo de Envase de un Cliente específico*/
        public StatusService FtnValidarAbonosGuardados(int iIdCliente, char cTipoEnvase)
        {
            var dbConexion = new conexionDB().CadenaConexion();

            using (SQLiteConnection conn = dbConexion)
            {
                try
                {
                    string sSql = "SELECT men_abono " +
                                  "FROM envase " +
                                  "WHERE (cln_clave = " + iIdCliente + ") " +
                                  "AND (mec_envase = '" + cTipoEnvase.ToString() + "');";

                    int iCantAbono = conn.ExecuteScalar<int>(sSql);

                    if (iCantAbono == 0)
                    {
                        return new StatusService
                        {
                            status = true,
                            mensaje = "Aún no se ha abonado envase de este Tipo de Envase."
                        };
                    }
                    else
                    {
                        return new StatusService
                        {
                            status = false,
                            mensaje = "Ya se han abonado previamente la cantidad de " + iCantAbono.ToString() + " a este Tipo de Envase.",
                            valor = iCantAbono.ToString().Trim()
                        };
                    }
                }
                catch (SQLiteException exc)
                {
                    return new StatusService
                    {
                        status = false,
                        mensaje = "Error: " + exc.Message
                    };
                }
                catch (Exception exc)
                {
                    return new StatusService
                    {
                        status = false,
                        mensaje = "Error: " + exc.Message
                    };
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }

        /*Método para regresar la Lista de Envases a los cuales puede abonar un Cliente específico*/
        public StatusService FtnRegresarEnvasesPorCliente(int iIdCliente)
        {
            var dbConexion = new conexionDB().CadenaConexion();

            using (SQLiteConnection conn = dbConexion)
            {
                try
                {
                    List<string> lsListaEnvases = new List<string>();

                    


                    string sSql = "SELECT " +
                                  " en.cln_clave, " +
                                  " en.mec_envase, " +
                                  " pr.ard_descripcion, " +
                                  " en.men_saldo_inicial, " +
                                  " (((men_saldo_inicial + men_cargo) - men_abono) - men_venta) AS men_saldo_final " + 
                                  "FROM envase en " +
                                  "JOIN productos pr ON (pr.arc_clave = en.mec_envase) " +
                                  "WHERE (en.cln_clave = " + iIdCliente + ") " +
                                  "ORDER BY en.mec_envase";

                    List<envaseCliente> lEnvases = conn.Query<envaseCliente>(sSql);

                    foreach (envaseCliente envase in lEnvases)
                    {
                        lsListaEnvases.Add(envase.mec_envase.ToString() + " - " + envase.ard_descripcion.Trim() + " - Saldo: " + envase.men_saldo_final.ToString());
                    }

                    //List<dynamic> lListaPromo = (dynamic)conn.Query<string>(sSql);
                    //List<dynamic> lListaPromo = (dynamic)conn.Query<envase>(sSql);
                    //List<dynamic> dss = (dynamic)lListaPromo;
                    //List<dynamic> sdss = new List<dynamic>();

                    if (lsListaEnvases.Count > 0)
                    {
                        return new StatusService
                        {
                            status = true,
                            mensaje = "Los Tipos de Envase del Cliente fueron obtenidos correctamente.",
                            listaStrings = lsListaEnvases
                        };
                    }
                    else
                    {
                        return new StatusService
                        {
                            status = false,
                            mensaje = "Los Tipos de Envase del Cliente no pudieron ser obtenidos."
                        };
                    }
                }
                catch (SQLiteException exc)
                {
                    return new StatusService
                    {
                        status = false,
                        mensaje = "Error: " + exc.Message
                    };
                }
                catch (Exception exc)
                {
                    return new StatusService
                    {
                        status = false,
                        mensaje = "Error: " + exc.Message
                    };
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
        }

        /*Método para regresar el Resumen de Abono de Envase de un Cliente específico*/
        public async Task< List<envase>> FtnRegresarResumenEnvasePorCliente(int iIdCliente)
        {
            List<envase> listaEnvases = null;

            var dbConexion = new conexionDB().CadenaConexion();

            using (SQLiteConnection conn = dbConexion)
            {
                try
                {
                    string sSql = "SELECT " +
                                  " cln_clave, " +
                                  " mec_envase, " +
                                  " men_saldo_inicial, " +
                                  " men_cargo, " +
                                  " men_abono, " +
                                  " men_venta, " +
                                  " (((men_saldo_inicial + men_cargo) - men_abono) - men_venta)  AS men_saldo_final " +
                                  "FROM envase " +
                                  "WHERE (cln_clave = " + iIdCliente + ") " +
                                  "ORDER BY mec_envase";

                    listaEnvases = new List<envase>();

                    listaEnvases = conn.Query<envase>(sSql);

                    if (listaEnvases.Count > 0)
                    {
                        Console.WriteLine("Lleno");
                    }
                    else
                    {
                        listaEnvases = null;
                    }
                }
                catch (SQLiteException exc)
                {
                    exc.ToString();
                    listaEnvases = null;
                }
                catch (Exception exc)
                {
                    exc.ToString();
                    listaEnvases = null;
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }

            return listaEnvases;
        }

        public bool fnCorregirEnvase(int iClave)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        @"update envase set  men_cargo = 0, men_abono = 0 ,men_saldo_final = ifnull(men_saldo_inicial,0) + ifnull(men_cargo,0) - ifnull(men_abono,0) - ifnull(men_venta,0),mec_es_devolucion='V'  where cln_clave = ? ";

                    SQLiteCommand command = conn.CreateCommand(sQuery, iClave);

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
        //desactivar
        public bool fnEnvasexClientexCargo_Abono(int iCliente, string sEnvase, int iCargo, int iAbono,string sFolio)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {

                    var sQuery =
                     "UPDATE envase SET " +
                                  "  men_cargo = ifnull(men_cargo,0) + " + iCargo + " " +
                                  " ,men_abono = ifnull(men_abono,0) + " + iAbono + " " +
                                  " ,men_saldo_final = ifnull(men_saldo_inicial,0) + ifnull(men_cargo,0) - ifnull(men_abono,0) - ifnull(men_venta,0) " +
                                  " ,men_folio =  '" + sFolio + "' " +
                                  "WHERE (cln_clave = " + iCliente + ") " +
                                  "AND (mec_envase = '" + sEnvase + "');";

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

        /*** Método para insertar el envase del cliente a la tabla temporal ***/
        public bool FtnInsertaEnvaseTemp(int iCliente)
        {
            bool bRespuesta = false;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sValida = "select 1 as existe from envase_temp where cln_clave = " + iCliente.ToString();
                    int iExiste = conn.ExecuteScalar<int>(sValida);

                    if (iExiste == 1)
                    {
                        bRespuesta = true;
                    }
                    else
                    {
                        string sInsert = "insert into envase_temp " +
                                         "(cln_clave, men_folio, mec_envase, men_saldo_inicial, men_cargo, men_abono, men_venta, men_saldo_final) " +
                                         "select * from envase where cln_clave = " + iCliente.ToString();
                        SQLiteCommand command = conn.CreateCommand(sInsert);

                        int iResultado = command.ExecuteNonQuery();

                        if (iResultado >= 1)
                            bRespuesta = true;
                        else
                            bRespuesta = false;
                    }
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                bRespuesta = false;
            }

            return bRespuesta;
        }

        /*** Método para borrar la tabla temporal envase_temp ***/
        public bool FtnBorrarEnvaseTemp(int iCliente)
        {
            bool bRespuesta = false;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sInsert = "delete from envase_temp where cln_clave = " + iCliente.ToString();
                    SQLiteCommand command = conn.CreateCommand(sInsert);

                    int iResultado = command.ExecuteNonQuery();

                    if (iResultado >= 1)
                        bRespuesta = true;
                    else
                        bRespuesta = false;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                bRespuesta = false;
            }

            return bRespuesta;
        }

        public bool fnEnvaseClienteCargo_Abono(int iCliente, string sEnvase, int iCargo, int iAbono, string sFolio, string sMarca)
        {
            var sQuery = "";

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sValida = "select 1 as existe " +
                                     "from envase_temp " +
                                     "where (mec_envase = '" + sEnvase + "') " +
                                     "AND (arc_clave = '" + sMarca + "')" +
                                     "AND cln_clave = " + iCliente.ToString();
                    int iExiste = conn.ExecuteScalar<int>(sValida);

                    if (iExiste == 1)
                    {
                        sQuery = "UPDATE envase_temp SET " +
                                 "  men_cargo = " + iCargo + " " +
                                 " ,men_abono = " + iAbono + " " +
                                 " ,men_folio =  '" + sFolio + "' " +
                                 "WHERE (cln_clave = " + iCliente + ") " +
                                 "AND (mec_envase = '" + sEnvase + "') " +
                                 "AND (arc_clave = '" + sMarca + "')";
                    }
                    else
                    {
                        sQuery = "insert into envase_temp " +
                                 "(cln_clave, men_folio, mec_envase, men_cargo, men_abono, arc_clave) " +
                                 "values " +
                                 "(" + iCliente.ToString() + ", '" + sFolio + "', '" + sEnvase + "', " + iCargo.ToString() + ", " + iAbono.ToString() + ", '"+ sMarca + "')";
                    }

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

        //desactivar
        public int fnEnvaseSaldoCliente(string sEnvase, int iCliente)
        {
            int iEnvase = 0;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "SELECT " +
                                 /* " cln_clave, " +
                                  " mec_envase, " +*/
                                  " men_saldo_inicial " +
                                /*  ", men_cargo, " +
                                  " men_abono, " +
                                  " men_venta, " +
                                  " (men_saldo_inicial + men_cargo - men_abono - men_venta)  AS men_saldo_final " +*/
                                  "FROM envase " +
                                  "WHERE (cln_clave = " + iCliente + ") " +
                                  "and  (mec_envase = '" + sEnvase + "') " +
                                  "ORDER BY mec_envase";

                    iEnvase = conn.ExecuteScalar<int>(sQuery);
                }
                return iEnvase;

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -999;
            }
        }

        //desactivar
        public int fnEnvaseAbonoCliente(string sEnvase, int iCliente)
        {
            int iEnvase = 0;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "SELECT " +
                                  /* " cln_clave, " +
                                   " mec_envase, " +
                                  " men_saldo_inicial, " +
                                    ", men_cargo, " +*/
                                    " men_abono " +
                                  /*  " men_venta, " +
                                    " (men_saldo_inicial + men_cargo - men_abono - men_venta)  AS men_saldo_final " +*/
                                  "FROM envase " +
                                  "WHERE (cln_clave = " + iCliente + ") " +
                                  "and  (mec_envase = '" + sEnvase + "') " +
                                  "ORDER BY mec_envase";

                    iEnvase = conn.ExecuteScalar<int>(sQuery);
                }
                return iEnvase;

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -999;
            }
        }

        public bool fnEnvaseVenta(string sEnvase, int iCliente, int iCantidad,string sFolio, string sMarca)
        {
            var sQuery = "";

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //var sQuery =
                    // "UPDATE envase SET " +
                    //              "  men_venta = ifnull(men_venta,0) + " + iCantidad + " " +
                    //              " ,men_folio =  '" + sFolio + "' " +
                    //               " ,men_saldo_final = ifnull(men_saldo_inicial,0) + ifnull(men_cargo,0) - ifnull(men_abono,0) - ifnull(men_venta,0) " +
                    //              //   " ,men_saldo_final = men_saldo_inicial + men_cargo - men_abono - men_venta " +
                    //              "WHERE (cln_clave = " + iCliente + ") " +
                    //              "AND (mec_envase = '" + sEnvase + "');";

                    //var sQuery =
                    // "UPDATE envase_temp SET " +
                    //              "men_venta = " + iCantidad + " " +                                  
                    //              "WHERE (cln_clave = " + iCliente + ") " +
                    //              "AND (mec_envase = '" + sEnvase + "') " +
                    //              "AND (arc_clave = '" + sMarca + "')";


                    string sValida = "select 1 as existe " +
                                     "from envase_temp " +
                                     "where (mec_envase = '" + sEnvase + "') " +
                                     "AND (arc_clave = '" + sMarca + "')" +
                                     "AND cln_clave = " + iCliente.ToString();
                    int iExiste = conn.ExecuteScalar<int>(sValida);

                    if (iExiste == 1)
                    {
                        sQuery = "UPDATE envase_temp SET " +
                                 "men_venta = " + iCantidad + ", " +
                                 "men_folio = '"+ sFolio + "' " +
                                 "WHERE (cln_clave = " + iCliente + ") " +
                                 "AND (mec_envase = '" + sEnvase + "') " +
                                 "AND (arc_clave = '" + sMarca + "')";
                    }
                    else
                    {
                        sQuery = "insert into envase_temp " +
                                 "(cln_clave, men_folio, mec_envase, men_venta, arc_clave) " +
                                 "values " +
                                 "(" + iCliente.ToString() + ", '" + sFolio + "', '" + sEnvase + "', " + iCantidad.ToString() + ", '" + sMarca + "')";
                    }

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

        /* Método para aplicar la devolución de envase */


    }
}
