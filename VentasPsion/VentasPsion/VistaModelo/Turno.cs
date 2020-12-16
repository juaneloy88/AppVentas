using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo;
using VentasPsion.Modelo.Servicio;

namespace VentasPsion.VistaModelo
{
    public class Turno
    {
        /*Método para guardar los datos de Inicio de Turno (Fecha, Hora, Unidad y Kilometraje) de una Ruta específica*/
        public StatusService FtnGuardarInicioTurno(int iRuta, int iUnidad, int iKmInicial)
        {
            var dbConexion = new conexionDB().CadenaConexion();

            using (SQLiteConnection conn = dbConexion)
            {
                try
                {
                    string sSql = "UPDATE ruta SET " +
                                  "ruc_inicio = '" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "', " +
                                  "run_ncamion = " + iUnidad + ", " +
                                  "run_kminicio = " + iKmInicial + " " +
                                  "WHERE (run_clave = " + iRuta + ");";

                    SQLiteCommand command = conn.CreateCommand(sSql);

                    int iResultado = command.ExecuteNonQuery();

                    if (iResultado >= 1)
                    {
                        return new StatusService
                        {
                            status = true,
                            mensaje = "Los datos del Inicio de Turno fueron guardados correctamente."
                        };
                    }
                    else
                    {
                        return new StatusService
                        {
                            status = false,
                            mensaje = "Los datos del Inicio de Turno no pudieron ser guardados."
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

        /*Método para guardar los datos de Fin de Turno (Fecha, Hora y Kilometraje) de una Ruta específica*/
        public StatusService FtnGuardarFinTurno(int iRuta, int iKmFinal)
        {
            var dbConexion = new conexionDB().CadenaConexion();

            using (SQLiteConnection conn = dbConexion)
            {
                try
                {
                    string sSql = "SELECT run_kminicio " +
                                  "FROM ruta " +
                                  "WHERE (run_clave = " + iRuta + ");";

                    int iKmInicial = conn.ExecuteScalar<int>(sSql);

                    if (iKmInicial > 0)
                    {
                        if (iKmFinal > iKmInicial)
                        {
                            sSql = "UPDATE ruta SET " +
                                   "ruc_termino = '" + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + "', " +
                                   "run_kmfinal = " + iKmFinal + " " +
                                   "WHERE (run_clave = " + iRuta + ");";

                            SQLiteCommand command = conn.CreateCommand(sSql);

                            int iRespuesta = command.ExecuteNonQuery();

                            if (iRespuesta >= 1)
                            {
                                return new StatusService
                                {
                                    status = true,
                                    mensaje = "Los datos del Fin de Turno fueron guardados correctamente."
                                };
                            }
                            else
                            {
                                return new StatusService
                                {
                                    status = false,
                                    mensaje = "Los datos del Fin de Turno no pudieron ser guardados."
                                };
                            }

                        }
                        else
                        {
                            return new StatusService
                            {
                                status = false,
                                mensaje = "El Kilometraje de Fin de Turno (" + iKmFinal.ToString() + ") no puede ser menor o igual al Kilometraje de Inicio de Turno (" + iKmInicial.ToString() + ")."
                            };
                        }
                    }
                    else
                    {
                        return new StatusService
                        {
                            status = false,
                            mensaje = "No se pudo obtener el Kilometraje de Inicio de Turno."
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
