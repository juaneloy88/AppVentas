using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class conseptoDevolucionesSR
    {
        conexionDB cODBC = new conexionDB();

        public List<concepto_devoluciones> ListaConseptosDevoluciones()
        {
            try
            {
                List<concepto_devoluciones> LConsDev = new List<concepto_devoluciones>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = @"SELECT *
                                    FROM concepto_devoluciones                
                                    ORDER BY cdn_clave";
                    

                    LConsDev = conn.Query<concepto_devoluciones>(sQuery);
                }

                return LConsDev;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        public string ConseptoDevolucion(int iCliente)

        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "  select cdc_descripcion from concepto_devoluciones where cdn_clave = (Select dev_clave from devoluciones WHERE  cln_clave = ? )";
                    string sRegistro = conn.ExecuteScalar<string>(sQuery, iCliente);

                    return sRegistro;
                }


            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return "";
            }
        }

    }
}
