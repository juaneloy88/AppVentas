using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    class clientes_requisitos_surtirSR
    {
        conexionDB cODBC = new conexionDB();

        public clientes_requisitos_surtir TraeDatos(int nCliente)
        {
            clientes_requisitos_surtir lLista = null;
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    lLista = conn.Table<clientes_requisitos_surtir>().Where(i => i.cln_clave == nCliente).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                lLista = null;
                VarEntorno.sMensajeError = ex.Message;
            }

            return lLista;

        }

        public bool ActualizaDatos(int iCliente, clientes_requisitos_surtir cDatos)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        @"UPDATE clientes_requisitos_surtir 
                               SET            
            crc_titular = '" + cDatos.crc_titular + @"'
          , crc_horario_apertura = '" + cDatos.crc_horario_apertura + @"'
          , crc_horario_cierre = '" + cDatos.crc_horario_cierre + @"'
          , crc_horario_sugerido = '" + cDatos.crc_horario_sugerido + @"'
          , crb_factura = '" + (cDatos.crb_factura ? 1 : 0) + @"'
          , crb_pago_tarjeta = '" + (cDatos.crb_pago_tarjeta ? 1 : 0) + @"'
          , crb_chamuco = '" + (cDatos.crb_chamuco ? 1 : 0) + @"'
          , crb_escaleras = '" + (cDatos.crb_escaleras ? 1 : 0) + @"'
          , crb_rampa = '" + (cDatos.crb_rampa ? 1 : 0) + @"'
          , crb_espacio_estrecho = '" + (cDatos.crb_espacio_estrecho ? 1 : 0) + @"'
          , crb_asaltos = '" + (cDatos.crb_asaltos ? 1 : 0) + @"'
        , crb_actualizado = '" + (cDatos.crb_actualizado ? 1 : 0) + @"'
          , crc_avisos = '" + cDatos.crc_avisos + @"'
                        WHERE cln_clave    = " + iCliente + "  ";

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
        
        public clientes_requisitos RequisitosClientes()
        {
            List<clientes_requisitos> ListaReq = new List<clientes_requisitos>();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "SELECT " +
                                 "sum(CASE WHEN crb_factura  THEN 1 ELSE 0 END) AS crb_factura, " +
                                 "sum(CASE WHEN crb_pago_tarjeta THEN 1 ELSE 0 END) AS crb_pago_tarjeta, " +
                                 "sum(CASE WHEN crb_chamuco = 1 THEN 1 ELSE 0 END) AS crb_chamuco, " +
                                 "sum(CASE WHEN crb_escaleras = 1 THEN 1 ELSE 0 END) AS crb_escaleras, " +
                                 "sum(CASE WHEN crb_rampa = 1 THEN 1 ELSE 0 END) AS crb_rampa, " +
                                 "sum(CASE WHEN crb_espacio_estrecho = 1 THEN 1 ELSE 0 END) AS crb_espacio_estrecho," +
                                 "sum(CASE WHEN crb_asaltos = 1 THEN 1 ELSE 0 END) AS crb_asaltos " +
                                 "FROM clientes_requisitos_surtir ";

                    ListaReq = conn.Query<clientes_requisitos>(sQuery);

                    int _cfactura = ListaReq[0].crb_factura;
                    int _tarjeta = ListaReq[0].crb_pago_tarjeta;

                    if (_cfactura >= 1 || _tarjeta >= 1)
                    {

                        ListaReq[0].ListaClientes = (from cfactura in conn.Table<clientes_requisitos_surtir>()
                                                     from clientes in conn.Table<clientes>()
                                                     where cfactura.cln_clave == clientes.cln_clave
                                                      && (cfactura.crb_factura == true ||
                                                     cfactura.crb_pago_tarjeta == true)
                                                     select new clientes_requisitos()
                                                     {
                                                         cln_clave = cfactura.cln_clave,
                                                         crc_titular = clientes.clc_nombre_comercial == "" ?
                                                        clientes.clc_nombre : clientes.clc_nombre_comercial,
                                                         tipo = cfactura.crb_factura == true ? 1 : 0
                                                     }).ToList();
                    }

                    List<clientes_requisitos_surtir> a = conn.Table<clientes_requisitos_surtir>().ToList();

                    string pendientes = string.Join(",", conn.Table<clientes_requisitos_surtir>().ToList().Select(e => e.cln_clave).ToArray());

                    ListaReq[0].pendientes = conn.Table<clientes>().ToList()
                                            .Where(e => !pendientes.Contains(e.cln_clave.ToString()))
                                            .Select(e => new clientes { cln_clave = e.cln_clave, clc_nombre_comercial = e?.clc_nombre_comercial ?? e.clc_nombre_comercial })
                                            .ToList();

                }
            }

            catch (Exception ex)
            {
                ListaReq = null;
                VarEntorno.sMensajeError = ex.Message;
            }

            return ListaReq[0];
        }
    }
}
