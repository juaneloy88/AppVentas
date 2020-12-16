using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class gpsSR
    {
        conexionDB cODBC = new conexionDB();
        public gpsSR()
        {
            // constructor
        }

        /****************************/
        public bool GuardarGPSCliente(clientes vCliente, string _latitud, string _longitud,int _tipo_movimiento)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    conn.BeginTransaction();
                    int iResult = 0;
                    bool bResultado = false;

                    string sQuery = "select * from gps where cln_clave = ? and gpb_esBase  ";
                    List<gps> vLista = conn.Query<gps>(sQuery, vCliente.cln_clave);

                    gps oGps = new gps();

                    gps oGpsBase = new gps();

                    oGps.cln_clave = vCliente.cln_clave;
                    oGps.gpd_latitud = _latitud;
                    oGps.gpd_longitud = _longitud;                                   
                    oGps.ctn_tipo_movimiento = _tipo_movimiento;
                    oGps.run_clave = VarEntorno.iNoRuta;
                    oGps.gpd_hora= DateTime.Now.ToShortTimeString();
                    oGps.gpc_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                    oGps.gpb_esBase = false;

                    if (vLista.Count > 0)
                    {
                        oGps.gpd_latitud = vLista[0].gpd_latitud;
                        oGps.gpd_longitud = vLista[0].gpd_longitud;
                        
                    }

                    iResult = conn.Insert(oGps);

                    if (iResult >= 1)
                    {
                        iResult = 0;
                        if (vLista.Count > 0)
                        {
                            iResult = 1;
                        }
                        else
                        {
                            oGpsBase = oGps;
                            oGpsBase.ctn_tipo_movimiento = 4;
                            oGpsBase.gpc_folio = "";
                            oGpsBase.gpb_esBase = true;
                            iResult = conn.Insert(oGpsBase);
                        }

                        if (iResult >= 1)
                        {
                            conn.Commit();
                            bResultado = true;
                            VarEntorno.sMensajeError = "";
                        }
                        else
                        {
                            conn.Rollback();
                            bResultado = false;
                            VarEntorno.sMensajeError = "Error en guardar GPS  ";
                        }
                    }
                    else
                    {
                        conn.Rollback();
                        bResultado = false;
                        VarEntorno.sMensajeError = "Error en guardar GPS base  ";
                    }
                    return bResultado;
                }
                
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
