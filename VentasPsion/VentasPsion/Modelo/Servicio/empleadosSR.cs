using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    public class empleadosSR
    {
        //Se instancia la clase Conexion
        conexionDB cODBC = new conexionDB();

        #region Método en donde se valida el Login
        public string ValidaLogin(string usuario, string contrasenia)
        {
            string sRespuesta = string.Empty;

            try
            {
                //Se obtiene la cadena de conexión
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {                    

                    #region Se realiza el query para comparar el usuario y contraseña con la BD, si el login es correcto, la variabla sRespuesta retorna un Ok
                    string sqlQuery = "select * from empleados where emc_usuario = '" + usuario + "' and emc_password = '" + contrasenia + "'";

                    var vExisteIdCliente = conn.Query<empleados>(sqlQuery);

                    if (vExisteIdCliente.Count == 0)
                    {
                        sRespuesta = "Contraseña y/o Password Incorrecto";
                    }
                    else
                    {
                        sRespuesta = "Ok";
                    }
                    #endregion Se realiza el query para comparar el usuario y contraseña, si el login es correcto la variabla sRespuesta retorna un Ok
                }
            }
            catch (Exception ex)
            {
                sRespuesta = "Error: " + ex.ToString();
            }            

            return sRespuesta;
        }
        #endregion Método en donde se valida el Login
    }
}
