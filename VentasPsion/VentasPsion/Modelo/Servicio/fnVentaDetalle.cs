using VentasPsion.Modelo.Entidad;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.VistaModelo;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace VentasPsion.Modelo.Servicio
{
    class fnVentaDetalle
    {
        conexionDB cODBC = new conexionDB();

        private static SemaphoreSlim sl = new SemaphoreSlim(1);

        //* busca la venta de un producto por medio de la clave , el cliente   *//
        public int fnBuscaProductoXCliente(string sProducto)
        {
            try
            {
                int iCantidad = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select vdn_venta from venta_detalle WHERE vdn_producto = ? and vdn_cliente = ? and vdn_folio = ? ";
                    iCantidad = conn.ExecuteScalar<int>(sQuery, sProducto, VarEntorno.vCliente.cln_clave,VarEntorno.iFolio.ToString().PadLeft(6,'0'));
                }

                return iCantidad;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        //* busca la venta de un producto por medio de la clave , el cliente   *//
        public int fnBuscaProductoCliente(string sProducto)
        {
            try
            {
                int iCantidad = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select vdn_venta from venta_detalle where vdn_producto = ? and vdn_cliente = ?  and vdn_folio != ?";
                    iCantidad = conn.ExecuteScalar<int>(sQuery, sProducto, VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));
                }

                return iCantidad;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }
        
        //* guarda la venta de un articulo  *//
        public bool fnGuardarProductoFact(VentaVM vVenta, int esPromo,int iListaPrecio)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    venta_detalle vd = new venta_detalle();
                    List< lista_maestra> lLista = new List<lista_maestra>();

                    vd.vdn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                    vd.vdn_cliente = VarEntorno.vCliente.cln_clave;
                    vd.run_clave = VarEntorno.iNoRuta;
                    vd.vdn_producto = vVenta.pProducto.arc_clave;
                    vd.vdn_venta = vVenta.iVendido;
                    vd.vdn_importe = vVenta.iVendido * vVenta.dPrecio;
                    vd.vdd_descripcion = vVenta.pProducto.ard_corta;
                    vd.vdn_precio = vVenta.dPrecio;
                    vd.vdc_hora = DateTime.Now.ToShortTimeString();

                    vd.vdn_tipo_promo = esPromo;

                    //////////////DATOS DE FACTURACION
                    /*
                    lista_maestra lm = new lista_maestra();

                    lm = conn.Table<lista_maestra>()
                                            .Where(i => i.lmc_producto == vVenta.pProducto.arc_clave && i.lmc_tipo == iListaPrecio)
                                            .FirstOrDefault()  ?? new lista_maestra { } ;
                    lLista.Add(lm);
                    */
                    var sQuery = "select * from lista_maestra where lmc_tipo = ? and lmc_producto = ? ";
                    lLista = conn.Query<lista_maestra>(sQuery, iListaPrecio, vVenta.pProducto.arc_clave);

                    vd.lmn_precioneto = lLista[0].lmn_precioneto;
                    vd.lmn_ieps = lLista[0].lmn_ieps;
                    vd.lmn_preciobruto = lLista[0].lmn_preciobruto;
                    vd.lmn_iva = lLista[0].lmn_iva;
                    vd.lmn_preciofinal = lLista[0].lmn_preciofinal;
                    vd.lmc_porcentaje = lLista[0].lmc_porcentaje;
                    vd.lmn_descuento_neto = lLista[0].lmn_descuento_neto;
                    vd.lmn_iepsdescuento = lLista[0].lmn_iepsdescuento;
                    vd.lmn_descuento_bruto = lLista[0].lmn_descuento_bruto;
                    vd.lmn_ivadescuento = lLista[0].lmn_ivadescuento;
                    vd.lmn_totaldescuento = lLista[0].lmn_totaldescuento;
                    vd.lmn_preciolm_neto = lLista[0].lmn_preciolm_neto;
                    vd.lmn_preciolm_ipes = lLista[0].lmn_preciolm_ipes;
                    vd.lmn_preciolm_bruto = lLista[0].lmn_preciolm_bruto;
                    vd.lmn_preciolm_iva = lLista[0].lmn_preciolm_iva;
                    vd.lmn_preciolm = lLista[0].lmn_preciolm;

                    int iResult =  conn.Insert(vd);
                }

                return true;
            }
            catch 
            {
                VarEntorno.sMensajeError = "Articulo con precio 0; vd_f";
                return false;
            }
        }


        //* guarda la venta de un articulo  *//
        ///prueba
        public bool fnGuardarProductoFactENV(VentaVM vVenta, int esPromo, int iListaPrecio,string abonoEnvase)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    conn.BeginTransaction();

                    bool bResult = false;

                    venta_detalle vd = new venta_detalle();
                    List<lista_maestra> lLista = new List<lista_maestra>();

                    vd.vdn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                    vd.vdn_cliente = VarEntorno.vCliente.cln_clave;
                    vd.run_clave = VarEntorno.iNoRuta;
                    vd.vdn_producto = vVenta.pProducto.arc_clave;
                    vd.vdn_venta = vVenta.iVendido;
                    vd.vdn_importe = vVenta.iVendido * vVenta.dPrecio;
                    vd.vdd_descripcion = vVenta.pProducto.ard_corta;
                    vd.vdn_precio = vVenta.dPrecio;
                    vd.vdc_hora = DateTime.Now.ToShortTimeString();

                    vd.vdn_tipo_promo = esPromo;

                    //////////////DATOS DE FACTURACION
                    /*
                    lista_maestra lm = new lista_maestra();

                    lm = conn.Table<lista_maestra>()
                                            .Where(i => i.lmc_producto == vVenta.pProducto.arc_clave && i.lmc_tipo == iListaPrecio)
                                            .FirstOrDefault()  ?? new lista_maestra { } ;
                    lLista.Add(lm);
                    */
                    var sQuery = "select * from lista_maestra where lmc_tipo = ? and lmc_producto = ? ";
                    lLista = conn.Query<lista_maestra>(sQuery, iListaPrecio, vVenta.pProducto.arc_clave);

                    vd.lmn_precioneto = lLista[0].lmn_precioneto;
                    vd.lmn_ieps = lLista[0].lmn_ieps;
                    vd.lmn_preciobruto = lLista[0].lmn_preciobruto;
                    vd.lmn_iva = lLista[0].lmn_iva;
                    vd.lmn_preciofinal = lLista[0].lmn_preciofinal;
                    vd.lmc_porcentaje = lLista[0].lmc_porcentaje;
                    vd.lmn_descuento_neto = lLista[0].lmn_descuento_neto;
                    vd.lmn_iepsdescuento = lLista[0].lmn_iepsdescuento;
                    vd.lmn_descuento_bruto = lLista[0].lmn_descuento_bruto;
                    vd.lmn_ivadescuento = lLista[0].lmn_ivadescuento;
                    vd.lmn_totaldescuento = lLista[0].lmn_totaldescuento;
                    vd.lmn_preciolm_neto = lLista[0].lmn_preciolm_neto;
                    vd.lmn_preciolm_ipes = lLista[0].lmn_preciolm_ipes;
                    vd.lmn_preciolm_bruto = lLista[0].lmn_preciolm_bruto;
                    vd.lmn_preciolm_iva = lLista[0].lmn_preciolm_iva;
                    vd.lmn_preciolm = lLista[0].lmn_preciolm;


                    if (vVenta.pProducto.arc_produ == "E")
                    {
                        //bResult = vVenta.fnVentaEnvase(vVenta.pProducto.arc_clave);
                        /*
                        EnvFn.fnEnvaseVenta(pProducto.arc_clave
                                                        , VarEntorno.vCliente.cln_clave
                                                        ,this.iVendido
                                                        , VarEntorno.iFolio.ToString().PadLeft(6, '0')
                                                        , sMarca); 
                        */
                        string sValida = "select 1 as existe " +
                                         "from envase_temp " +
                                         "where (mec_envase = '" + vVenta.pProducto.arc_envase + "') " +
                                         "AND (arc_clave = '" + vVenta.pProducto.arc_clave + "')" +
                                         "AND cln_clave = " + VarEntorno.vCliente.cln_clave;
                        int iExiste = conn.ExecuteScalar<int>(sValida);

                        if (iExiste == 1)
                        {
                            sQuery = "UPDATE envase_temp SET " +
                                     "men_venta = " + vVenta.iVendido + ", " +
                                     "men_folio = '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + "' " +
                                     "WHERE (cln_clave = " + VarEntorno.vCliente.cln_clave + ") " +
                                     "AND (mec_envase = '" + vVenta.pProducto.arc_envase + "') " +
                                     "AND (arc_clave = '" + vVenta.pProducto.arc_clave + "')";
                        }
                        else
                        {
                            sQuery = "insert into envase_temp " +
                                     "(cln_clave, men_folio, mec_envase, men_venta, arc_clave) " +
                                     "values " +
                                     "(" + VarEntorno.vCliente.cln_clave + ", '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + "', '" + vVenta.pProducto.arc_envase + "', " + vVenta.iVendido + ", '" + vVenta.pProducto.arc_clave + "')";
                        }

                        SQLiteCommand command = conn.CreateCommand(sQuery);

                        int iResultado = command.ExecuteNonQuery();

                        if (iResultado >= 1)
                            bResult = true;
                        else
                            bResult = false;
                    }
                    else
                    {
                        if (vVenta.pProducto.arc_envase.ToString().Length > 0 && VarEntorno.cTipoVenta == 'A')
                        {
                            int iAbono = 0;

                            if (abonoEnvase == "" || abonoEnvase == "0" || Convert.ToInt32(abonoEnvase) < 0)
                                iAbono = 0;
                            else
                                iAbono = Convert.ToInt32(abonoEnvase);

                            //bResult = vVenta.fnActualizaEnvase(vVenta.pProducto.arc_clave, vVenta.iVendido, iAbono);
                            /*
                              EnvFn.fnEnvaseVenta(pProducto.arc_clave
                                                        , VarEntorno.vCliente.cln_clave
                                                        ,this.iVendido
                                                        , VarEntorno.iFolio.ToString().PadLeft(6, '0')
                                                        , sMarca);  */

                            string sValida = "select 1 as existe " +
                                             "from envase_temp " +
                                             "where (mec_envase = '" + vVenta.pProducto.arc_envase + "') " +
                                             "AND (arc_clave = '" + vVenta.pProducto.arc_clave + "')" +
                                             "AND cln_clave = " + VarEntorno.vCliente.cln_clave;
                            int iExiste = conn.ExecuteScalar<int>(sValida);

                            if (iExiste == 1)
                            {
                                sQuery = "UPDATE envase_temp SET " +
                                         "  men_cargo = " + vVenta.iVendido + " " +
                                         " ,men_abono = " + iAbono + " " +
                                         " ,men_folio =  '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + "' " +
                                         "WHERE (cln_clave = " + VarEntorno.vCliente.cln_clave + ") " +
                                         "AND (mec_envase = '" + vVenta.pProducto.arc_envase + "') " +
                                         "AND (arc_clave = '" + vVenta.pProducto.arc_clave + "')";
                            }
                            else
                            {
                                sQuery = "insert into envase_temp " +
                                         "(cln_clave, men_folio, mec_envase, men_cargo, men_abono, arc_clave) " +
                                         "values " +
                                         "(" + VarEntorno.vCliente.cln_clave + ", '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + "', '" + vVenta.pProducto.arc_envase + "', " + vVenta.iVendido + ", " + iAbono.ToString() + ", '" + vVenta.pProducto.arc_clave + "')";
                            }

                            SQLiteCommand command = conn.CreateCommand(sQuery);

                            int iResultado = command.ExecuteNonQuery();

                            if (iResultado >= 1)
                                bResult = true;
                            else
                                bResult = false;
                        }
                        else
                        {
                            bResult = true;
                        }
                    }

                    if (bResult)
                    {
                        if (conn.Insert(vd) > 0)
                        {
                            conn.Commit();
                            return true;
                        }
                        else
                        {
                            conn.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        conn.Rollback();
                        return false;
                    }
                }
            }
            catch
            {
                VarEntorno.sMensajeError = "Articulo con precio 0; vd_f";
                return false;
            }
        }


        
        
        //*  borra  la venta de articulo en venta detalle *//
        public bool fnBorrarProductoConjunto(VentaVM venta)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    int iRegistros = 0;
                    int iResultado = 1;
                    var sQuery = "SELECT vdn_tipo_promo FROM venta_detalle " +
                        "WHERE (vdn_folio    = '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + @"') and 
                          (vdn_cliente  = " + VarEntorno.vCliente.cln_clave + @") and vdn_producto = '" + venta.pProducto.arc_clave + "'   ";

                    iRegistros = conn.ExecuteScalar<int>(sQuery);
                    if (iRegistros != 0)
                    {
                        sQuery =
                            @"delete from venta_detalle                               
                            WHERE (vdn_folio    = '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + @"') and 
                          (vdn_cliente  = " + VarEntorno.vCliente.cln_clave + @") and 
                          (vdn_tipo_promo = "+ iRegistros + ")";

                        SQLiteCommand command = conn.CreateCommand(sQuery  );

                         iResultado = command.ExecuteNonQuery();
                    }
                    
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
        
        
        //*  se obtien el importe total de una venta realizada con anterioridad por 
        ///  cliente  excluyendo determinado folio*//
        public decimal fnImporteTotalxClienteconExclucion(int iCliente,string sFolio)
        {
            try
            {
                decimal iImporteTotal = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_detalle WHERE vdn_cliente = ? and vdn_folio != ? ";
                    int iRegistros = conn.ExecuteScalar<int>(sQuery, iCliente, sFolio);

                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vdn_importe) from venta_detalle WHERE vdn_cliente = ? and vdn_folio != ? ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, iCliente, sFolio);
                    }
                }

                return iImporteTotal;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        //*  obtiene el folio de venta de preventa para usar en reparto    *//
        public string fnFoliodeCliente(int iCliente)
        {
            try
            {
                string sfolio ="";

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    //var sQuery = "Select distinct vdn_folio from venta_detalle WHERE vdn_cliente = ? limit 1 ";
                    var sQuery = "Select max(vdn_folio) from venta_detalle WHERE vdn_cliente = ?";
                    sfolio = conn.ExecuteScalar<string>(sQuery, iCliente);
                }

                return sfolio;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return "";
            }
        }

        //*   actualiza la venta detalle para poner los articulos devueltos *//
        public bool fnDevolucionProducto(int iCliente,string sFolio)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    if (fnDevoValidaProductos(iCliente)==false)
                    {
                        fnDevolucionProductosTickets(iCliente);
                    }

                    var sQuery = "UPDATE venta_detalle SET " +
                                 " vdn_venta_dev = vdn_venta, " +
                                 " vdn_importe =round(vdn_precio * vdn_venta,2), " +
                                 " vdn_venta = 0, " +
                                 " vdn_folio_devolucion = '" + sFolio + "', " +
                                 " vdn_folio = '" + sFolio + "', " +
                                 " vdc_tipo_precio = 'A', " +                                 
                                 " vdc_hora = '" + DateTime.Now.ToShortTimeString() + "' " +
                                 " WHERE (vdn_cliente = " + iCliente + ")";

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

        //* valida si la devolucion es de varios tickets y si hay productos duplicados *// 
        public bool fnDevoValidaProductos(int iCliente)
        {
            try
            {
                bool bResultado;
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                        var sQuery = " select count(distinct vdn_folio) from  venta_detalle  " +
                                 " WHERE (vdn_cliente = ? )   "+
                                 " group by vdn_producto " +
                                 " having count(distinct vdn_folio) > 1 ";

                        List<int> lArticulos  = conn.Query<int>(sQuery, iCliente);
                       
                        if (lArticulos.Count > 0)
                            bResultado = false;
                        else
                            bResultado = true;
        
                }
                return bResultado;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //* hace la devolucion cuando son varios tickets  *// 
        public bool fnDevolucionProductosTickets(int iCliente)
        {
            try
            {                
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    conn.BeginTransaction();

                    var sQuery = " select * from  venta_detalle  " +
                                 " WHERE (vdn_cliente = ? )   " +
                                 " group by vdn_producto " +
                                 " having count(distinct vdn_folio) > 1 ";
                    List<venta_detalle> lArticulos = conn.Query<venta_detalle>(sQuery, iCliente);

                    List<string> listART = lArticulos.Select(s => "'"+s.vdn_producto+"'").ToList();
                    string strArt = string.Join(",", listART);

                    sQuery = "select * from venta_detalle  " +
                                 "WHERE (vdn_cliente = " + iCliente + ")"+
                                 "  and vdn_producto in ("+ strArt  + ")";
                    int iResultado, x = 0 ;
                    SQLiteCommand command;

                    List<venta_detalle> lVenta_detalle = conn.Query<venta_detalle>(sQuery);

                    //foreach(var detallePK in lVenta_detalle)
                    for (int i=0;i<lVenta_detalle.Count;i++)
                    {
                        //foreach (var detalleFK in lVenta_detalle)
                        for (int p = i; p < lVenta_detalle.Count; p++)
                        {
                            if (lVenta_detalle[i] != lVenta_detalle[p])
                            {
                                if (lVenta_detalle[i].vdn_producto == lVenta_detalle[p].vdn_producto)
                                {
                                    // update original venta + venta de foraneo
                                    // update  foraneo marcar para borrar 
                                    sQuery = "update venta_detalle set vdn_venta = vdn_venta+ " + lVenta_detalle[p].vdn_venta +
                                        
                                        " where vdn_cliente = " + iCliente +
                                        " and vdn_folio =   '" + lVenta_detalle[i].vdn_folio + "'"+
                                        " and vdn_producto = '" + lVenta_detalle[i].vdn_producto + "' ;";

                                    command = conn.CreateCommand(sQuery);
                                    iResultado = command.ExecuteNonQuery();

                                    if (iResultado <= 0)
                                    {
                                        conn.Rollback();
                                        i = p = lVenta_detalle.Count;
                                        x = -1;
                                    }
                                    else
                                    {
                                        sQuery = " update venta_detalle set vdn_folio_devolucion = '-1' " +
                                        " where vdn_cliente = " + iCliente +
                                        " and vdn_folio =  '" + lVenta_detalle[p].vdn_folio +"'"+
                                        " and vdn_producto = '" + lVenta_detalle[p].vdn_producto + "'  ;" +
                                        "";
                                        command = conn.CreateCommand(sQuery);
                                        iResultado = command.ExecuteNonQuery();

                                        if (iResultado <= 0)
                                        {
                                            conn.Rollback();
                                            i = p = lVenta_detalle.Count;
                                            x = -1;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (x == 0)
                    {
                        sQuery = " delete from  venta_detalle  " +
                        " where vdn_cliente = " + iCliente +
                        " and vdn_folio_devolucion = '-1'";

                        command = conn.CreateCommand(sQuery);
                        iResultado = command.ExecuteNonQuery();

                        if (iResultado <= 0)
                        {
                            conn.Rollback();
                            x = -1;
                        }
                    }

                    if (x == 0)
                    {
                        conn.Commit();
                        return true;
                    }
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

        
        public int fnBuscaProductoVendidoxCategoria(string categoria)
        {
            try
            {
                int iCantidad = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = @"Select sum(vdn_venta) from venta_detalle WHERE vdn_producto in (select arc_clave from productos where arc_clasif_estadistica in ("+ categoria + @")) ";
                    iCantidad = conn.ExecuteScalar<int>(sQuery, categoria);
                }

                return iCantidad;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        //* busca como se  vendio un producto por medio venta o promociones  *//
        //desactivar
        public int fnBuscaProductoVendidoxPromo(string sProducto)
        {
            try
            {
                int iCantidad = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select vdn_tipo_promo from venta_detalle WHERE vdn_producto = ? and vdn_cliente = ? and vdn_folio =  ?";
                    iCantidad = conn.ExecuteScalar<int>(sQuery, sProducto,VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));
                }

                return iCantidad;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }
                
        //* busca la devolucion de un producto por medio de la clave , el cliente   *//
        ///Desactivar
        public int fnBuscaProductoXClienteDev(string sProducto)
        {
            try
            {
                int iCantidad = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select vdn_venta_dev from venta_detalle WHERE vdn_producto = ? and vdn_cliente = ? and vdn_folio = ?";
                    iCantidad = conn.ExecuteScalar<int>(sQuery, sProducto, VarEntorno.vCliente.cln_clave,VarEntorno.iFolio.ToString().PadLeft(6,'0'));
                }

                return iCantidad;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        //*  actualiza la venta de articulo en venta detalle *//
        //desactivar
        public bool fnActualizaProductoDev(VentaVM vVenta)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery =
                        @"UPDATE venta_detalle 
                               SET  vdn_venta_dev   = " + vVenta.iVendido + @", 
                                    vdn_importe = " + vVenta.dPrecio * vVenta.iVendido + @"
                    WHERE (vdn_folio    = '" + VarEntorno.iFolio.ToString().PadLeft(6, '0') + @"') and 
                          (vdn_cliente  = " + VarEntorno.vCliente.cln_clave + @") and 
                          (vdn_producto = '" + vVenta.pProducto.arc_clave + @"')";

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
        
        //* guarda la venta de un articulo  *//
        //desactivar
        public bool fnGuardarProductoDev(VentaVM vVenta, int iListaPrecio)
        {
            try
            {
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    venta_detalle vd = new venta_detalle();
                    List<lista_maestra> lLista = new List<lista_maestra>();

                    vd.vdn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                    vd.vdn_cliente = VarEntorno.vCliente.cln_clave;
                    vd.run_clave = VarEntorno.iNoRuta;
                    vd.vdn_producto = vVenta.pProducto.arc_clave;
                    vd.vdn_venta_dev = vVenta.iDevuelto;
                    vd.vdn_importe = vVenta.iDevuelto * vVenta.dPrecio;
                    vd.vdd_descripcion = vVenta.pProducto.ard_corta;
                    vd.vdn_precio = vVenta.dPrecio;
                    vd.vdc_hora = DateTime.Now.ToShortTimeString();

                    //////////////DATOS DE FACTURACION

                    //lLista = conn.Table<lista_maestra>().Where(i => i.lmc_producto == vVenta.pProducto.arc_clave ).FirstOrDefault();                    

                    var sQuery = "select * from lista_maestra where lmc_tipo = ? and lmc_producto = ? ";
                    lLista = conn.Query<lista_maestra>(sQuery, iListaPrecio, vVenta.pProducto.arc_clave);

                    vd.lmn_precioneto = lLista[0].lmn_precioneto;
                    vd.lmn_ieps = lLista[0].lmn_ieps;
                    vd.lmn_preciobruto = lLista[0].lmn_preciobruto;
                    vd.lmn_iva = lLista[0].lmn_iva;
                    vd.lmn_preciofinal = lLista[0].lmn_preciofinal;
                    vd.lmc_porcentaje = lLista[0].lmc_porcentaje;
                    vd.lmn_descuento_neto = lLista[0].lmn_descuento_neto;
                    vd.lmn_iepsdescuento = lLista[0].lmn_iepsdescuento;
                    vd.lmn_descuento_bruto = lLista[0].lmn_descuento_bruto;
                    vd.lmn_ivadescuento = lLista[0].lmn_ivadescuento;
                    vd.lmn_totaldescuento = lLista[0].lmn_totaldescuento;
                    vd.lmn_preciolm_neto = lLista[0].lmn_preciolm_neto;
                    vd.lmn_preciolm_ipes = lLista[0].lmn_preciolm_ipes;
                    vd.lmn_preciolm_bruto = lLista[0].lmn_preciolm_bruto;
                    vd.lmn_preciolm_iva = lLista[0].lmn_preciolm_iva;
                    vd.lmn_preciolm = lLista[0].lmn_preciolm;

                    conn.Insert(vd);                    
                }

                return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
     
        //desactivar
        public int fnProductoDevxEnvase(int iCliente,string sEnvase)
        {
            try
            {
                int iCantidad = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count (*) from venta_detalle   WHERE  vdn_cliente = ?";
                    int iRegistros = conn.ExecuteScalar<int>(sQuery, iCliente);
                    if (iRegistros > 0)
                    {
                         sQuery = "Select sum(vdn_venta_dev) from venta_detalle v join productos p on p.arc_clave = v.vdn_producto   WHERE  vdn_cliente = ? and arc_envase = ?";
                        iCantidad = conn.ExecuteScalar<int>(sQuery, iCliente, sEnvase);
                    }
                }

                return iCantidad;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }
        //desactivar 
        public int fnEsPromoConjunto(string sProducto)
        {
            try
            {

                int iMarca = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select vdn_tipo_promo from venta_detalle WHERE vdn_producto = ? and vdn_cliente = ? and vdn_folio = ?";
                    iMarca = conn.ExecuteScalar<int>(sQuery, sProducto, VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));

                    if (iMarca == 0)
                        return 0;
                    else
                        return 1;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return -1;
            }
        }

        public int fnExisteVenta(int iCliente)
        {           

            try
            {
                int iExiste = -1;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select 1 as existe from venta_detalle WHERE vdn_cliente = ? and vdn_venta <> 0 group by 1;";
                    iExiste = conn.ExecuteScalar<int>(sQuery, VarEntorno.vCliente.cln_clave);
                }

                if (iExiste != 1)
                    iExiste = 0;

                return iExiste;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;                
                return -1;
            }            
        }

        public int fnExisteDevol(int iCliente)
        {
            try
            {
                int iExiste;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select 1 as existe from venta_detalle WHERE vdn_cliente = ? and vdn_venta_dev <> 0 group by 1;";
                    iExiste = conn.ExecuteScalar<int>(sQuery, VarEntorno.vCliente.cln_clave);
                }

                if (iExiste != 1)
                    iExiste = 0;

                return iExiste;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public bool ProductosDeContado()
        {
            try
            {
                int iCantidad = 0;
                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = @"Select count (*) from venta_detalle WHERE vdn_cliente = ? and vdn_folio = ? 
                                            and vdn_producto in (Select arc_clave from productos WHERE arc_contado )";
                    iCantidad = conn.ExecuteScalar<int>(sQuery,  VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));
                }

                if (iCantidad > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        
    }
}
