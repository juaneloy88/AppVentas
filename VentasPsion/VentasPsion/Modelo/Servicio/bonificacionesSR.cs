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
    public class bonificacionesSR
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        #region Método que busca la bonificación del cliente
        public async Task<List<bonificaciones>> buscaBonificaciones(string sFolio, string sCliente)
        {
            await sl.WaitAsync();

            try
            {
                #region Búsqueda de Bonificaciones del cliente
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    List<bonificaciones> lListaBonis = conn.Query<bonificaciones>("select * from bonificaciones where boc_folio = '"+ sFolio + "' and boc_cliente = " + sCliente+";");

                    return lListaBonis;
                }
                #endregion Búsqueda de Bonificaciones del cliente
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método que busca las bonificaciones del cliente

        #region Valida si ya se encuentra aplicada la Bonificación
        public async Task<string> validaSiEstaAplicadaBoni(string sfolio)
        {
            string sRespuesta = "";

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select 1 as existe from bonificaciones WHERE boc_folio_venta is not null and boc_folio = ? ";
                    int iExiste = conn.ExecuteScalar<int>(sQuery, sfolio);

                    if (iExiste == 1)
                    {
                        sRespuesta = "Ya se encuentra aplicada la Bonificación";
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
        #endregion Valida si ya se encuentra aplicada la Bonificación

        
    }
}