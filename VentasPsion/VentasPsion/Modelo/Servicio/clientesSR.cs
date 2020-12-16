using SQLite;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;

namespace VentasPsion.Modelo.Servicio
{
    class clientesSR
    {
        //Se instancia la clase Conexion
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        #region Obtiene todos los clientes de la tabla de clientes
        public List<clientes> obtenerclientes()
        {
            try
            {
                List<clientes> oListaClientes = new List<clientes>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT *
                                    FROM clientes";
                    oListaClientes = conn.Query<clientes>(sQuery);
                }

                return oListaClientes;

            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Obtiene todos los clientes de la tabla de clientes

        #region Válida si el Id del cliente existe en la BD
        public string ValidaCliente(string cIdCliente)
        {
            string sRespuesta = string.Empty;

            try
            {
                //Se obtiene la cadena de conexión
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //Se convierte la variable cIdCliente a Integer
                    int nIdCliente = Convert.ToInt32(cIdCliente);

                    #region Se realiza la consulta para saber si el cliente existe en la BD, si existe, la variabla sRespuesta retorna un Ok
                    var vExisteIdCliente = conn.Table<clientes>().Where(x => x.cln_clave == nIdCliente).FirstOrDefault();

                    if (vExisteIdCliente == null)
                    {
                        sRespuesta = "Cliente No Válido";
                    }
                    else
                    {
                        sRespuesta = "Ok";
                    }
                    #endregion Se realiza la consulta para saber si el cliente existe en la BD, si existe, la variabla sRespuesta retorna un Ok
                }
            }
            catch (Exception ex)
            {
                sRespuesta = "Error: " + ex.ToString();
            }           

            return sRespuesta;
        }
        #endregion Válida si el Id del cliente existe en la BD

        #region Método que devuelve la clase clientes con el ID del Cliente a buscar
        public async Task<clientes> DatosCliente(string cIdCliente)
        {
            await sl.WaitAsync();

            try
            {
                //Se obtiene la cadena de conexión
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //Se convierte la variable cIdCliente a Integer
                    int nIdCliente = Convert.ToInt32(cIdCliente);

                    //Se realiza la consulta para obtener los datos del cliente capturado
                    return conn.Table<clientes>().Where(i => i.cln_clave == nIdCliente).FirstOrDefault();
                }
                
            }
            finally
            {
                sl.Release();
            }            
        }
        #endregion Método que devuelve la clase clientes con el ID del Cliente a buscar
    }
}
