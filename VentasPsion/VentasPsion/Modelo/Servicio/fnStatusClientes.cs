using Xamarin.Forms;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion.Modelo.Servicio
{
    public class fnStatusClientes
    {
        //Instancia de la clase Conexion
        conexionDB cODBC = new conexionDB();

        EnvaseService envaseService = new EnvaseService();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);        

        #region Método que devuelve los clientes sin visitar de reparto
        public async Task<List<clientes_estatus>> obtieneClientesSinVisita()
        {
            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from clientes_estatus " +
                                    "where cln_visitado not in (1,  3,4) ";
                    var lLista = conn.Query<clientes_estatus>(sQuery);                    

                    return lLista;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método que devuelve los clientes sin visitar

        #region Método que devuelve los clientes sin visitar de reparto
        public async Task<List<clientes_estatus>> obtieneClientesSinVisitaAP()
        {
            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from clientes_estatus " +
                                    "where cln_visitado not in (1,2,3,4) ";
                    var lLista = conn.Query<clientes_estatus>(sQuery);

                    return lLista;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método que devuelve los clientes sin visitar

        #region Método para obtener el listado de Clientes con sus status
        public async Task<List<StatusClientes>> obtieneStatusClientes()
        {
            vmStatusClientes vmStatusCtes = new vmStatusClientes();

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    
                    //string sQuery = "select c.cln_clave as sCliente, ctes.clc_nombre_comercial as sNombreComercial, c.cln_visitado as iVisitado " +
                    //                "from clientes_estatus c " +
                    //                "join clientes ctes on ctes.cln_clave = c.cln_clave " +
                    //                "order by c.cln_clave";
                    string sQuery = "select c.cln_clave as sCliente, ctes.clc_nombre as sNombreComercial, c.cln_visitado as iVisitado " +
                                    "from clientes_estatus c " +
                                    "join clientes ctes on ctes.cln_clave = c.cln_clave " +
                                    "order by c.cln_clave";
                    var lLista = conn.Query<StatusClientes>(sQuery);

                    foreach (StatusClientes statusCtes in lLista)
                    {
                        switch (statusCtes.iVisitado)
                        {
                            case 1:
                                StatusClientes statCtes1 = new StatusClientes();
                                statCtes1.sCliente = statusCtes.sCliente;
                                statCtes1.sNombreComercial = statusCtes.sNombreComercial;
                                statCtes1.iVisitado = statusCtes.iVisitado;
                                statCtes1.cstatusColor = Color.Green;
                                vmStatusCtes.lEstatusClientes.Add(statCtes1);
                                break;
                            case 2:
                                StatusClientes statCtes2 = new StatusClientes();

                                statCtes2.sCliente = statusCtes.sCliente;
                                statCtes2.sNombreComercial = statusCtes.sNombreComercial;
                                statCtes2.iVisitado = 1;
                                statCtes2.cstatusColor = Color.Green;
                                vmStatusCtes.lEstatusClientes.Add(statCtes2);

                                //if (VarEntorno.cTipoVenta == 'R')
                                //{
                                //    statCtes2.sCliente = statusCtes.sCliente;
                                //    statCtes2.sNombreComercial = statusCtes.sNombreComercial;
                                //    statCtes2.iVisitado = 1;
                                //    statCtes2.cstatusColor = Color.Green;
                                //    vmStatusCtes.lEstatusClientes.Add(statCtes2);
                                //}
                                //else
                                //{
                                //    statCtes2.sCliente = statusCtes.sCliente;
                                //    statCtes2.sNombreComercial = statusCtes.sNombreComercial;
                                //    statCtes2.iVisitado = statusCtes.iVisitado;
                                //    statCtes2.cstatusColor = Color.Orange;
                                //    vmStatusCtes.lEstatusClientes.Add(statCtes2);
                                //}
                                break;
                            case 3:
                                StatusClientes statCtes3 = new StatusClientes();
                                statCtes3.sCliente = statusCtes.sCliente;
                                statCtes3.sNombreComercial = statusCtes.sNombreComercial;
                                statCtes3.iVisitado = statusCtes.iVisitado;
                                statCtes3.cstatusColor = Color.Orange;                                
                                vmStatusCtes.lEstatusClientes.Add(statCtes3);
                                break;
                            case 0:
                                StatusClientes statCtes4 = new StatusClientes();
                                statCtes4.sCliente = statusCtes.sCliente;
                                statCtes4.sNombreComercial = statusCtes.sNombreComercial;
                                statCtes4.iVisitado = statusCtes.iVisitado;
                                statCtes4.cstatusColor = Color.Red;
                                vmStatusCtes.lEstatusClientes.Add(statCtes4);
                                break;
                            case 4:
                                StatusClientes statCtes5 = new StatusClientes();
                                statCtes5.sCliente = statusCtes.sCliente;
                                statCtes5.sNombreComercial = statusCtes.sNombreComercial;
                                statCtes5.iVisitado = statusCtes.iVisitado;
                                statCtes5.cstatusColor = Color.Orange;
                                vmStatusCtes.lEstatusClientes.Add(statCtes5);
                                break;
                            default:
                                StatusClientes statCtes6 = new StatusClientes();
                                statCtes6.sCliente = statusCtes.sCliente;
                                statCtes6.sNombreComercial = statusCtes.sNombreComercial;
                                statCtes6.iVisitado = statusCtes.iVisitado;
                                statCtes6.cstatusColor = Color.Red;
                                vmStatusCtes.lEstatusClientes.Add(statCtes6);
                                break;
                        }
                    }

                    var lClientesStatus = vmStatusCtes.lEstatusClientes;

                    //lClientesStatus = lClientesStatus.OrderBy(o => o.sCliente).ToList();

                    return lClientesStatus;
                }
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método para obtener el listado de Clientes con sus status

        #region Método que obtiene los clientes dependiendo del status
        public List<StatusClientes> obtieneClientesVisitados(List<StatusClientes> lListaClientes, int iVisitado)
        {
            var vLista = new List<StatusClientes>();

            if (VarEntorno.cTipoVenta == 'R')
            {
                switch (iVisitado)
                {
                    case 1:
                        vLista = lListaClientes.Where(x => x.iVisitado == iVisitado || x.iVisitado == 2).ToList<StatusClientes>();                        
                        break;
                    case 3:
                        vLista = lListaClientes.Where(x => x.iVisitado == 3).ToList<StatusClientes>();
                        break;
                    case 0:
                        vLista = lListaClientes.Where(x => x.iVisitado == iVisitado).ToList<StatusClientes>();
                        break;
                    default:
                        vLista = lListaClientes.Where(x => x.iVisitado == iVisitado).ToList<StatusClientes>();
                        break;
                }
            }
            else
            {
                if (iVisitado == 1)
                    vLista = lListaClientes.Where(x => x.iVisitado == iVisitado || x.iVisitado == 2).ToList<StatusClientes>();
                else
                    vLista = lListaClientes.Where(x => x.iVisitado == iVisitado).ToList<StatusClientes>();
            }

            return vLista;
        }
        #endregion Método que obtiene los clientes dependiendo del status

        #region Método que devuelve los clientes que coincidan con el ID usando un like
        public List<StatusClientes> obtieneClientesId(List<StatusClientes> lListaClientes, string sCliente)
        {
            return lListaClientes.Where(x => x.sCliente.Contains(sCliente)).ToList<StatusClientes>();
        }
        #endregion Método que devuelve los clientes que coincidan con el ID usando un like

        #region Método para Liberar los botones de borrar entrega y devolución de rutas de Reparto
        public async Task<string> liberaReparto(string sPassword)
        {
            string sRespuesta = string.Empty;

            await sl.WaitAsync();

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery = "select * from password where ppc_password = '" + sPassword + "'";

                    var lLista = conn.Query<password>(sQuery);

                    if (lLista.Count == 1)
                    {
                        sRespuesta = "Ruta Liberada";
                    }
                    else
                    {
                        sRespuesta = "Contraseña Incorrecta";
                    }
                }

                return sRespuesta;
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método para Liberar los botones de borrar entrega y devolución de rutas de Reparto

        #region Método para Borrar Venta
        public async Task<string> borrarVenta(string sCliente)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sTipoTicket = string.Empty;
            int iRespuesta;

            fnVentaCabecera FNvenC = new fnVentaCabecera();
            //bool bRespuesta;
            #endregion Declaración de Variables

            await sl.WaitAsync();

            try
            {
                //Válida el tipo de Ticket
                //sTipoTicket = verificaTipoTicket(sCliente);
                List<venta_cabecera> lVenCab = FNvenC.VentasCabeceras(Convert.ToInt32(sCliente), "D");

                if (lVenCab.Count > 0)
                    sTipoTicket = "D";
                else
                    sTipoTicket = "V";

                if (sTipoTicket == "D")
                {
                    sRespuesta = "El Ticket del Cliente es una Devolución";
                }
                else
                {   
                    using (SQLiteConnection conn = cODBC.CadenaConexion())
                    {
                        conn.BeginTransaction();

                        //Realiza Update de la tabla de clientes_estatus
                        sQuery = "update clientes_estatus set cln_visitado = 0, clb_corregido = '1', cln_tipo_no_venta = 0 where cln_clave = " + sCliente;
                        SQLiteCommand commandCE = conn.CreateCommand(sQuery);
                        iRespuesta = commandCE.ExecuteNonQuery();

                        if (iRespuesta < 0)
                        {
                            sRespuesta = "No se pudo actualizar la tabla clientes_estatus";
                            conn.Rollback();
                        }
                        else
                        {
                            //Actualiza los registros de la tabla de envase
                            sQuery = @"update envase set men_cargo = 0, men_abono = 0, men_venta = 0, men_saldo_final = men_saldo_inicial where cln_clave = " + sCliente;
                            SQLiteCommand command0 = conn.CreateCommand(sQuery);
                            iRespuesta = command0.ExecuteNonQuery();

                            if (iRespuesta < 0)
                            {
                                sRespuesta = "No se pudo Actualizar de la tabla envase";
                                conn.Rollback();
                            }
                            else
                            {
                                //borra los registros de la tabla venta_cabecera
                                sQuery = "delete from venta_cabecera where vcn_cliente = " + sCliente;
                                SQLiteCommand command = conn.CreateCommand(sQuery);
                                iRespuesta = command.ExecuteNonQuery();

                                if (iRespuesta < 0)
                                {
                                    sRespuesta = "No se pudo borrar de la tabla venta_cabecera";
                                    conn.Rollback();
                                }
                                else
                                {
                                    //borra los registros de la tabla venta_detalle
                                    sQuery = "delete from venta_detalle where vdn_cliente = " + sCliente;
                                    SQLiteCommand command2 = conn.CreateCommand(sQuery);
                                    iRespuesta = command2.ExecuteNonQuery();

                                    if (iRespuesta < 0)
                                    {
                                        sRespuesta = "No se pudo borrar de la tabla venta_cabecera";
                                        conn.Rollback();
                                    }
                                    else
                                    {
                                        //Realiza Update de la tabla de bonificaciones
                                        //sQuery = "update bonificaciones set boc_folio_venta = '' where boc_cliente = " + sCliente;
                                        sQuery = "update bonificaciones set boc_folio_venta = null where boc_cliente = " + sCliente;
                                        SQLiteCommand command3 = conn.CreateCommand(sQuery);
                                        iRespuesta = command3.ExecuteNonQuery();

                                        if (iRespuesta < 0)
                                        {
                                            sRespuesta = "No se pudo actualizar la tabla bonificaciones";
                                            conn.Rollback();
                                        }
                                        else
                                        {

                                            //Tratamiento a la tabla de documentos cabecera
                                            //Actualización del campo dPagosVendedor a cero
                                            sQuery = "update documentos_cabecera set dPagosVendedor = 0, dcn_numero_pago = dcn_numero_pago_base where vcn_cliente = " + sCliente;
                                            SQLiteCommand commandDC1 = conn.CreateCommand(sQuery);
                                            iRespuesta = commandDC1.ExecuteNonQuery();

                                            if (iRespuesta < 0)
                                            {
                                                sRespuesta = "No se pudo actualizar la tabla documentos cabecera";
                                                conn.Rollback();
                                            }
                                            else
                                            {
                                                //Borrado de los nuevos registros de la tabla documentos cabecera
                                                sQuery = "delete from documentos_cabecera where vcn_cliente = " + sCliente + " and dcb_nuevo_documento";
                                                SQLiteCommand commandDC2 = conn.CreateCommand(sQuery);
                                                iRespuesta = commandDC2.ExecuteNonQuery();

                                                if (iRespuesta < 0)
                                                {
                                                    sRespuesta = "No se pudieron borrar los registros de la tabla documentos cabecera";
                                                    conn.Rollback();
                                                }
                                                else
                                                {
                                                    //Borrado de la tabla documentos_detalle
                                                    sQuery = "delete from documentos_detalle where vcn_cliente = " + sCliente + ";";
                                                    SQLiteCommand commandDD = conn.CreateCommand(sQuery);
                                                    iRespuesta = commandDD.ExecuteNonQuery();

                                                    if (iRespuesta < 0)
                                                    {
                                                        sRespuesta = "No se pudieron borrar los registros de la tabla documentos_detalle";
                                                        conn.Rollback();
                                                    }
                                                    else
                                                    {
                                                        //Borrado de la tabla venta_pagos
                                                        sQuery = "delete from venta_pagos where vcn_cliente = " + sCliente + ";";
                                                        SQLiteCommand commandVP = conn.CreateCommand(sQuery);
                                                        iRespuesta = commandVP.ExecuteNonQuery();

                                                        if (iRespuesta < 0)
                                                        {
                                                            sRespuesta = "No se pudieron borrar los registros de la tabla venta_pagos";
                                                            conn.Rollback();
                                                        }
                                                        else
                                                        {
                                                            //Borrado de la tabla de anticipos
                                                            sQuery = "delete from anticipos where anb_nuevo and cln_clave = " + sCliente + ";";
                                                            SQLiteCommand commandAnt = conn.CreateCommand(sQuery);
                                                            iRespuesta = commandAnt.ExecuteNonQuery();

                                                            if (iRespuesta < 0)
                                                            {
                                                                sRespuesta = "No se pudieron borrar los registros de la tabla anticipos";
                                                                conn.Rollback();
                                                            }
                                                            else
                                                            {
                                                                //Borrado de la tabla de envase_sugerido
                                                                sQuery = "delete from envase_sugerido where cln_clave = " + sCliente + ";";
                                                                SQLiteCommand commandEnvSug = conn.CreateCommand(sQuery);
                                                                iRespuesta = commandEnvSug.ExecuteNonQuery();

                                                                if (iRespuesta < 0)
                                                                {
                                                                    sRespuesta = "No se pudieron borrar los registros de la tabla envase_sugerido";
                                                                    conn.Rollback();
                                                                }
                                                                else
                                                                {
                                                                    //Quita la relación de Anticipo
                                                                    sQuery = "update anticipos set anb_relacionado = '0' where cln_clave = " + sCliente + ";";
                                                                    SQLiteCommand commandRelAnt = conn.CreateCommand(sQuery);
                                                                    iRespuesta = commandRelAnt.ExecuteNonQuery();

                                                                    if (iRespuesta < 0)
                                                                    {
                                                                        sRespuesta = "No se pudo quitar la relación del anticipo";
                                                                        conn.Rollback();
                                                                    }
                                                                    else
                                                                    {
                                                                        //Borrado de la tabla de GPS exceptuando los movimientos base
                                                                        sQuery = "delete from gps where gpb_esBase = 0 and cln_clave = " + sCliente + ";";
                                                                        SQLiteCommand commandGps = conn.CreateCommand(sQuery);
                                                                        iRespuesta = commandGps.ExecuteNonQuery();

                                                                        if (iRespuesta < 0)
                                                                        {
                                                                            sRespuesta = "No se pudieron borrar los movimientos de la tabla GPS";
                                                                            conn.Rollback();
                                                                        }
                                                                        else
                                                                        {
                                                                            conn.Commit();
                                                                            sRespuesta = "Venta Eliminada";
                                                                        }
                                                                    }                                                                    
                                                                }                                                                    
                                                            }                                                            
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }                        
                    }                    
                }                

                return sRespuesta;
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método para Borrar Venta

        #region Método para borrar la entrega de Reparto
        public async Task<string> borrarEntrega(string sCliente)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sTipoTicket = string.Empty;
            int iRespuesta = 0;
            fnVentaCabecera FNvenC = new fnVentaCabecera();
            bool bRespuesta;
            #endregion Declaración de Variables

            await sl.WaitAsync();
            
            try
            {
                //Válida el tipo de Ticket
                //sTipoTicket = verificaTipoTicket(sCliente);
                List<venta_cabecera> lVenCab = FNvenC.VentasCabeceras(Convert.ToInt32( sCliente),"D");

                if (lVenCab.Count > 0)
                    sTipoTicket = "D";
                else
                    sTipoTicket = "V";

                if (sTipoTicket == "D")
                {
                    sRespuesta = "El Ticket del Cliente es una Devolución";
                }
                else
                {
                    //actualiza los registros de la tabla de envase
                    bRespuesta = envaseService.fnCorregirEnvase(Convert.ToInt32(sCliente));

                    if (bRespuesta == false)
                    {
                        sRespuesta = "No se pudo actualizar la tabla envase";
                    }
                    else
                    {
                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                        {
                            conn.BeginTransaction();

                            //borra los registros de la tabla venta_cabecera
                            sQuery = @"delete from venta_cabecera where vcn_cliente = " + sCliente + @"";
                            SQLiteCommand command = conn.CreateCommand(sQuery);
                            iRespuesta = command.ExecuteNonQuery();

                            if (iRespuesta < 0)
                            {
                                sRespuesta = "No se pudo borrar de la tabla venta_cabecera";
                                conn.Rollback();
                            }
                            else
                            {
                                //Realiza Update de la tabla de clientes_estatus                            
                                sQuery = @"update clientes_estatus set cln_visitado = 0, clb_corregido = '1' where cln_clave = " + sCliente + @"";
                                SQLiteCommand command4 = conn.CreateCommand(sQuery);
                                iRespuesta = command4.ExecuteNonQuery();

                                if (iRespuesta < 0)
                                {
                                    sRespuesta = "No se pudo actualizar la tabla clientes_estatus";
                                    conn.Rollback();
                                }
                                else
                                {
                                    //Borrado de la tabla documentos_detalle
                                    sQuery = "delete from documentos_detalle where vcn_cliente = " + sCliente + ";";
                                    SQLiteCommand commandDD = conn.CreateCommand(sQuery);
                                    iRespuesta = commandDD.ExecuteNonQuery();

                                    if (iRespuesta < 0)
                                    {
                                        sRespuesta = "No se pudieron borrar los registros de la tabla documentos_detalle";
                                        conn.Rollback();
                                    }
                                    else
                                    {
                                        //Borrado de la tabla documentos cabecera
                                        sQuery = "update documentos_cabecera set dPagosVendedor = 0, dcn_numero_pago = dcn_numero_pago_base where vcn_cliente = " + sCliente;
                                        SQLiteCommand commandDC = conn.CreateCommand(sQuery);
                                        iRespuesta = commandDC.ExecuteNonQuery();

                                        if (iRespuesta < 0)
                                        {
                                            sRespuesta = "No se pudieron actualizar los registros de la tabla documentos cabecera";
                                            conn.Rollback();
                                        }
                                        else
                                        {
                                            //Borrado de la tabla venta_pagos
                                            sQuery = "delete from venta_pagos where vcn_cliente = " + sCliente + ";";
                                            SQLiteCommand commandVP = conn.CreateCommand(sQuery);
                                            iRespuesta = commandVP.ExecuteNonQuery();

                                            if (iRespuesta < 0)
                                            {
                                                sRespuesta = "No se pudieron borrar los registros de la tabla venta_pagos";
                                                conn.Rollback();
                                            }
                                            else
                                            {
                                                //Borrado de la tabla de GPS exceptuando los movimientos base
                                                sQuery = "delete from gps where gpb_esBase = 0 and cln_clave = " + sCliente + ";";
                                                SQLiteCommand commandGps = conn.CreateCommand(sQuery);
                                                iRespuesta = commandGps.ExecuteNonQuery();

                                                if (iRespuesta < 0)
                                                {
                                                    sRespuesta = "No se pudieron borrar los movimientos de la tabla GPS";
                                                    conn.Rollback();
                                                }
                                                else
                                                {
                                                    conn.Commit();
                                                    sRespuesta = "Venta Eliminada";
                                                }
                                            }
                                        }
                                    }                                    
                                }
                            }
                        }
                    }
                }                
            }
            finally
            {
                sl.Release();
            }

            return sRespuesta;
        }
        #endregion Método para borrar la entrega de Reparto

        #region Método para Borrar la Devolución
        public async Task<string> borrarDevol(string sCliente)
        {
            #region Declaración de Variables
            string sRespuesta = string.Empty;
            string sQuery = string.Empty;
            string sTipoTicket = string.Empty;
            int iRespuesta = 0;
            bool bRespuesta;

            fnVentaCabecera FNvenC = new fnVentaCabecera();
            #endregion Declaración de Variables

            await sl.WaitAsync();

            try
            {
                //Válida el tipo de Ticket
                //sTipoTicket = verificaTipoTicket(sCliente);
                List<venta_cabecera> lVenCab = FNvenC.VentasCabeceras(Convert.ToInt32(sCliente), "D");

                if (lVenCab.Count > 0)
                    sTipoTicket = "D";
                else
                    sTipoTicket = "V";

                if (sTipoTicket != "D")
                {
                    sRespuesta = "El Ticket del Cliente NO es una Devolución";
                }
                else
                {
                    //Actualiza los registros de la tabla de envase
                    bRespuesta = envaseService.fnCorregirEnvase(Convert.ToInt32(sCliente));

                    if (bRespuesta == false)
                    {
                        sRespuesta = "No se pudo actualizar la tabla envase";
                    }
                    else
                    {
                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                        {
                            conn.BeginTransaction();

                            //borra los registros de la tabla venta_cabecera                        
                            sQuery = @"delete from venta_cabecera where vcn_cliente = " + sCliente + @"";
                            SQLiteCommand command = conn.CreateCommand(sQuery);
                            iRespuesta = command.ExecuteNonQuery();

                            if (iRespuesta < 0)
                            {
                                sRespuesta = "No se pudo borrar de la tabla venta_cabecera";
                                conn.Rollback();
                            }
                            else
                            {
                                //borrar los registros de la tabla de devoluciones
                                sQuery = "delete from devoluciones where cln_clave = " + sCliente + @"";
                                SQLiteCommand command2 = conn.CreateCommand(sQuery);
                                iRespuesta = command2.ExecuteNonQuery();

                                if (iRespuesta < 0)
                                {
                                    sRespuesta = "No se pudo borrar de la tabla devoluciones";
                                    conn.Rollback();
                                }
                                else
                                {
                                    if (VarEntorno.cTipoVenta == 'A')
                                    {
                                        //borra los registros de la tabla venta_detalle                        
                                        sQuery = @"delete from venta_detalle where vdn_cliente = " + sCliente + @"";
                                    }
                                    else
                                    {
                                        //actualiza los registros de la tabla venta_detalle                                
                                        sQuery = @"update venta_detalle set vdn_venta = vdn_venta_dev, " +
                                                  "vdc_tipo_precio = vdn_precio, " +
                                                  "vdn_venta_dev = 0 " +
                                                  "where vdn_cliente = " + sCliente + @"";
                                    }
                                    SQLiteCommand command3 = conn.CreateCommand(sQuery);
                                    iRespuesta = command3.ExecuteNonQuery();

                                    if (iRespuesta < 0)
                                    {
                                        sRespuesta = "No se pudo actualizar la tabla venta_detalle";
                                        conn.Rollback();
                                    }
                                    else
                                    {
                                        //Realiza Update de la tabla de clientes_estatus                                    
                                        sQuery = @"update clientes_estatus set cln_visitado = 0, clb_corregido = '1' where cln_clave = " + sCliente + @"";
                                        SQLiteCommand command4 = conn.CreateCommand(sQuery);
                                        iRespuesta = command4.ExecuteNonQuery();

                                        if (iRespuesta < 0)
                                        {
                                            sRespuesta = "No se pudo actualizar la tabla clientes_estatus";
                                            conn.Rollback();
                                        }
                                        else
                                        {
                                            //Realiza Update a los documentos  para activarlos                                 
                                            sQuery = @"update documentos_cabecera set  dcc_tipo = 'V' where vcn_cliente = " + sCliente + @" and dcc_tipo = 'D' ";
                                            SQLiteCommand command5 = conn.CreateCommand(sQuery);
                                            iRespuesta = command5.ExecuteNonQuery();

                                            if (iRespuesta < 0)
                                            {
                                                sRespuesta = "No se pudo actualizar la tabla documentos cab";
                                                conn.Rollback();
                                            }
                                            else
                                            {
                                                //Borrado de la tabla documentos_detalle
                                                sQuery = "delete from documentos_detalle where vcn_cliente = " + sCliente + ";";
                                                SQLiteCommand commandDD = conn.CreateCommand(sQuery);
                                                iRespuesta = commandDD.ExecuteNonQuery();

                                                if (iRespuesta < 0)
                                                {
                                                    sRespuesta = "No se pudieron borrar los registros de la tabla documentos_detalle";
                                                    conn.Rollback();
                                                }
                                                else
                                                {
                                                    //Borrado de la tabla venta_pagos
                                                    sQuery = "delete from venta_pagos where vcn_cliente = " + sCliente + ";";
                                                    SQLiteCommand commandVP = conn.CreateCommand(sQuery);
                                                    iRespuesta = commandVP.ExecuteNonQuery();

                                                    if (iRespuesta < 0)
                                                    {
                                                        sRespuesta = "No se pudieron borrar los registros de la tabla venta_pagos";
                                                        conn.Rollback();
                                                    }
                                                    else
                                                    {
                                                        //conn.Commit();
                                                        //sRespuesta = "Devolución Borrada";

                                                        //Borrado de la tabla de GPS exceptuando los movimientos base
                                                        sQuery = "delete from gps where gpb_esBase = 0 and cln_clave = " + sCliente + ";";
                                                        SQLiteCommand commandGps = conn.CreateCommand(sQuery);
                                                        iRespuesta = commandGps.ExecuteNonQuery();

                                                        if (iRespuesta < 0)
                                                        {
                                                            sRespuesta = "No se pudieron borrar los movimientos de la tabla GPS";
                                                            conn.Rollback();
                                                        }
                                                        else
                                                        {
                                                            if (VarEntorno.cTipoVenta == 'A')
                                                            {
                                                                //Realiza Update de la tabla de bonificaciones
                                                                //sQuery = "update bonificaciones set boc_folio_venta = '' where boc_cliente = " + sCliente;
                                                                sQuery = "update bonificaciones set boc_folio_venta = null where boc_cliente = " + sCliente;
                                                                SQLiteCommand commandBN = conn.CreateCommand(sQuery);
                                                                iRespuesta = commandBN.ExecuteNonQuery();

                                                                if (iRespuesta < 0)
                                                                {
                                                                    sRespuesta = "No se pudo actualizar la tabla bonificaciones";
                                                                    conn.Rollback();
                                                                }
                                                                else
                                                                {
                                                                    //Borrado de la tabla de anticipos
                                                                    sQuery = "delete from anticipos where anb_nuevo and cln_clave = " + sCliente + ";";
                                                                    SQLiteCommand commandAnt = conn.CreateCommand(sQuery);
                                                                    iRespuesta = commandAnt.ExecuteNonQuery();

                                                                    if (iRespuesta < 0)
                                                                    {
                                                                        sRespuesta = "No se pudieron borrar los registros de la tabla anticipos";
                                                                        conn.Rollback();
                                                                    }
                                                                    else
                                                                    {
                                                                        //Quita la relación de Anticipo
                                                                        sQuery = "update anticipos set anb_relacionado = '0' where cln_clave = " + sCliente + ";";
                                                                        SQLiteCommand commandRelAnt = conn.CreateCommand(sQuery);
                                                                        iRespuesta = commandRelAnt.ExecuteNonQuery();

                                                                        if (iRespuesta < 0)
                                                                        {
                                                                            sRespuesta = "No se pudo quitar la relación del anticipo";
                                                                            conn.Rollback();
                                                                        }
                                                                        else
                                                                        {
                                                                            conn.Commit();
                                                                            sRespuesta = "Devolución Borrada";
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                conn.Commit();
                                                                sRespuesta = "Devolución Borrada";
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return sRespuesta;
            }
            finally
            {
                sl.Release();
            }
        }
        #endregion Método para Borrar la Devolución

        #region Método que verifica el Tipo de Ticket
        public string verificaTipoTicket(string sCliente)
        {
            string sTipoPago = string.Empty;
            string sRespuesta = string.Empty;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {                    
                    string sQuery = "select vcc_tipo_pago from venta_cabecera where vcn_cliente = " + sCliente + ";";
                    sTipoPago = conn.ExecuteScalar<string>(sQuery);                    
                    sRespuesta = Convert.ToChar(sTipoPago).ToString();
                }
            }
            catch (Exception ex)
            {
                sRespuesta = ex.ToString();
            }            

            return sRespuesta;
        }
        #endregion Método que verifica el Tipo de Ticket

        #region Método para contar clientes con venta y/o cobranza y/o visita 
        public int ClientesSinVisita()
        {
            int iFaltantes = 1;
            int iClientes = 0;
            int iTotales = 0;

            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    string sQuery =
                        "select count(distinct clave) as cli from " +
                        "(" +
                                "select cln_clave as clave from clientes_estatus " +
                                "where cln_visitado  in (1,  3,4) " +
                                "union "+
                                "select distinct vcn_cliente::int as clave from venta_cabecera " +
                        ")";
                     iClientes = conn.ExecuteScalar<int>(sQuery);
                     sQuery =
                        "select count(distinct clave) as cli from " +
                        "(" +
                                "select cln_clave as clave from clientes_estatus " +
                        ")";
                     iTotales = conn.ExecuteScalar<int>(sQuery);
                }
                
                iFaltantes = iTotales - iClientes;
               
            }
            catch (Exception ex)
            {
                iFaltantes = -1;
            }

            return iFaltantes;
        }
        #endregion Método que verifica el Tipo de Ticket

    }
}