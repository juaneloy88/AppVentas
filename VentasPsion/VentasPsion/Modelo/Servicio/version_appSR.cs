using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace VentasPsion.Modelo.Servicio
{
    public class version_appSR
    {
        conexionDB cODBC = new conexionDB();

        /*Método para validar si ya fue guardado el Abono de un cierto Tipo de Envase de un Cliente específico*/
        public StatusService FtnDevuelveVersionApp()
        {
            var dbConexion = new conexionDB().CadenaConexion();

            using (SQLiteConnection conn = dbConexion)
            { 
                try
                {
                    string sSql = "SELECT COALESCE(vpc_version, '') " +
                                  "FROM version_app";

                    string sVersionApp = conn.ExecuteScalar<string>(sSql);

                    if ((sVersionApp != "") && (sVersionApp != null))
                    {
                        return new StatusService
                        {
                            status = true,
                            mensaje = "La Versión de la App de Ventas CCA de la Base de Datos se obtuvo correctamente.",
                            valor = sVersionApp
                        };
                    }
                    else
                    {
                        return new StatusService
                        {
                            status = true,
                            mensaje = "No se ha podido obtener la Versión de la App de Ventas CCA de la Base de Datos.",
                            valor = ""
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
    }
}
