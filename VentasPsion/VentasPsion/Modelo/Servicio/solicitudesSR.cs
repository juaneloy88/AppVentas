using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class solicitudesSR
    {
        conexionDB cODBC = new conexionDB();
        public solicitudesSR()
        {
            // constructor
        }

        /****************************/
        /*********METODOS************/
        /****************************/
        public bool GuardarSolicitud(int _dpn_clave, string _soc_descripcion, int _cln_clave, int _run_clave , DateTime _sod_fecha)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    solicitudes oSolicitudes = new solicitudes();
                    oSolicitudes.dpn_clave = _dpn_clave;
                    oSolicitudes.soc_descripcion = _soc_descripcion;
                    oSolicitudes.cln_clave = _cln_clave;
                    oSolicitudes.run_clave = _run_clave;
                    oSolicitudes.sod_fecha = _sod_fecha;
                    conn.Insert(oSolicitudes);
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
        /*********METODOS************/
        /****************************/
    }
}
