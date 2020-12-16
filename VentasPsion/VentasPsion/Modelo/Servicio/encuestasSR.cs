using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class encuestasSR
    {
        //Se instancia la clase Conexion
        conexionDB cODBC = new conexionDB();
        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        public encuestasSR()
        {
            // Constructor
        }

        public List<encuesta> obtenerEncuestas()
        {
            try
            {
                List<encuesta> oListaEncuesta = new List<encuesta>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //string sQuery = @"SELECT * FROM encuesta as en JOIN respuestas re ON(en.enn_id=re.enn_id) ORDER BY en.enn_id ";
                    //    string sQuery = @"SELECT * FROM respuestas";
                    //  string sQuery = @"SELECT * FROM encuesta as en JOIN opciones op ON(en.enn_id=op.enn_id) ORDER BY en.enn_id ";
                    string sQuery = @"SELECT * FROM encuesta ";
                    oListaEncuesta = conn.Query<encuesta>(sQuery);
                }

                return oListaEncuesta;

            }
            finally
            {
                sl.Release();
            }
        }


        public List<opciones> obtenerOpciones(int enn_id)
        {
            try
            {
                List<opciones> oListOpciones = new List<opciones>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //string sQuery = @"SELECT * FROM encuesta as en JOIN respuestas re ON(en.enn_id=re.enn_id) ORDER BY en.enn_id ";
                    //    string sQuery = @"SELECT * FROM respuestas";
                    //  string sQuery = @"SELECT * FROM encuesta as en JOIN opciones op ON(en.enn_id=op.enn_id) ORDER BY en.enn_id ";
                    string sQuery = @"SELECT * FROM opciones WHERE enn_id="+ enn_id+" ORDER BY enn_id";
                    oListOpciones = conn.Query<opciones>(sQuery);
                }

                return oListOpciones;

            }
            finally
            {
                sl.Release();
            }
        }

        
        public bool fnGuardarTablaRespuestas(int _cln_clave, string _enc_respuesta, int _enn_tipo_respuesta, int _enn_id)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    respuestas oRespuestas = new respuestas();
                    oRespuestas.cln_clave = _cln_clave;
                    oRespuestas.enc_respuesta = _enc_respuesta;
                    oRespuestas.enn_tipo_respuesta = _enn_tipo_respuesta;
                    oRespuestas.enn_id = _enn_id;
                    conn.Insert(oRespuestas);
                }
                return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
        /****************************/




    }
}
