using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;
using System.Linq;

namespace VentasPsion.Modelo.Servicio
{
    class fnVentaCabecera
    {
        conexionDB cODBC = new conexionDB();
        /// <summary>
        /// FUNCIONES VERIFICADAS   OK
        /// </summary>
        /// <param name="sClave"></param>
        /// <returns></returns>

        public decimal VentasImportes(int sClave)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago!= 'D'";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_importe) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago!= 'D' ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave);
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

        public decimal DevolucionesImportes(int sClave)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago== 'D'";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_importe) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago== 'D' ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave);
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

        public decimal PagosImportes(int sClave)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? ";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_monto_pago) from venta_cabecera WHERE  vcn_cliente = ? ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave);
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

        public decimal SaldoFinal(clientes cCliente)
        {
            try
            {
                decimal dSaldoFinal = 0;
                decimal dVentas = VentasImportes(cCliente.cln_clave);
                decimal dDevoluciones = DevolucionesImportes(cCliente.cln_clave);
                decimal dPagos = PagosImportes(cCliente.cln_clave);

                if (dVentas == -1)                
                    dSaldoFinal = -999;                
                else
                {
                    if (dPagos == -1)                    
                        dSaldoFinal = -999;                    
                    else
                    {
                        if (dVentas == -1)                        
                            dSaldoFinal = -999;                        
                        else
                        {
                            if (VarEntorno.cTipoVenta == 'R')
                            {                                
                                dSaldoFinal = cCliente.cln_saldo - dPagos - dDevoluciones;                                 
                            }
                            else
                            {
                                dSaldoFinal = cCliente.cln_saldo + dVentas - dPagos;
                                if (VarEntorno.cTipoVenta == 'A')
                                    dSaldoFinal = dSaldoFinal - dDevoluciones;
                            }
                        }
                    }
                }

                return Math.Round(dSaldoFinal,2);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -999;
            }
        }


        public decimal DevolucionesImportes(int sClave, int iFolio)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago== 'D' and CAST(vcn_folio as INTEGER) <= ? ";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, iFolio);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_importe) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago== 'D'  and CAST(vcn_folio as INTEGER) <= ? ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave, iFolio);
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

        public decimal VentasImportes(int sClave,int iFolio)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago!= 'D' and CAST(vcn_folio as INTEGER) <= ? ";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, iFolio);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_importe) from venta_cabecera WHERE  vcn_cliente = ? and vcc_tipo_pago!= 'D'  and CAST(vcn_folio as INTEGER) <= ? ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave, iFolio);
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

        public decimal PagosImportes(int sClave,int iFolio)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and CAST(vcn_folio as INTEGER) <= ? ";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, iFolio);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_monto_pago) from venta_cabecera WHERE  vcn_cliente = ? and CAST(vcn_folio as INTEGER) <= ? ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave, iFolio);
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

        public decimal SaldoFinal(clientes cCliente,int iFolio)
        {
            try
            {
                decimal dSaldoFinal = 0;
                decimal dVentas = VentasImportes(cCliente.cln_clave, iFolio);
                decimal dDevoluciones = DevolucionesImportes(cCliente.cln_clave, iFolio);
                decimal dPagos = PagosImportes(cCliente.cln_clave, iFolio);

                if (dVentas == -1)                
                    dSaldoFinal = -999;                
                else
                {
                    if (dPagos == -1)                    
                        dSaldoFinal = -999;                    
                    else
                    {
                        if (dDevoluciones == -1)                        
                            dSaldoFinal = -999;                        
                        else
                        {
                            if (VarEntorno.cTipoVenta == 'R')
                            {                                
                                dSaldoFinal = cCliente.cln_saldo - dPagos - dDevoluciones;                                
                            }
                            else
                            {
                                dSaldoFinal = cCliente.cln_saldo + dVentas - dPagos;
                                if (VarEntorno.cTipoVenta == 'A')
                                    dSaldoFinal = dSaldoFinal - dDevoluciones;
                            }
                        }
                    }
                }

                return Math.Round(dSaldoFinal, 2);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -999;
            }
        }


        ///se usan para calcular el limite de credito en venta omitiendo los contados
        public decimal PagosImportesdeVentas(int sClave)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcn_importe > 0 ";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave);

                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_monto_pago) from venta_cabecera WHERE vcn_cliente = ? and vcn_importe > 0 ";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave);

                        if (VarEntorno.cTipoVenta == 'P')
                        {
                            List<string> sFolios = FoliosVenta(sClave,"$");
                            //List<venta_cabecera> lCabeceras = new List<venta_cabecera>;
                            string sFolio = "";
                            int i = 0;
                            foreach (var folio in sFolios)
                            //foreach (var folio in lCabeceras)
                            {
                                if (i != 0)
                                    sFolio = sFolio + ",";
                                i++;
                                sFolio = folio;
                            }

                            sQuery = "Select count (*) from bonificaciones WHERE boc_folio_venta in  (" + sFolio + ")";
                            iRegistros = conn.ExecuteScalar<int>(sQuery, sClave);
                            if (iRegistros > 0)
                            {
                                sQuery = "Select sum(boi_documento) from bonificaciones WHERE boc_folio_venta in  (" + sFolio + ")";
                                iImporteTotal = iImporteTotal - conn.ExecuteScalar<decimal>(sQuery);
                            }
                        }
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

        public decimal PagosImportesdePagos (int sClave)
        {
            try
            {
                decimal iImporteTotal = 0;
                int iRegistros = 0;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ? and vcn_importe =0";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select sum(vcn_monto_pago) from venta_cabecera WHERE  vcn_cliente = ?  and vcn_importe =0";
                        iImporteTotal = conn.ExecuteScalar<decimal>(sQuery, sClave);
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

        //obtener los cabeceras de ticket por Tipo mov para reimpresion de ticket de venta o pago
        public List<venta_cabecera> VentasCabeceras (int sClave,string sTipoMov)
        {
            try
            {
               
                int iRegistros = 0;
                
                List<string> sFolios = new List<string>();
                List<venta_cabecera> vLista = new List<venta_cabecera>();

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ?  and vcc_tipo_pago= ?";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, sTipoMov);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select * from venta_cabecera WHERE  vcn_cliente = ?   and vcc_tipo_pago= ?";
                         vLista = conn.Query<venta_cabecera>(sQuery, sClave, sTipoMov);
                        /*
                        foreach (var cabecera in vLista)
                            sFolios.Add(cabecera.vcn_folio);
                            */
                    }
                   
                }

                return vLista;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        //obtener los folios de ticket por Tipo mov 
        public List<string> FoliosVenta(int sClave, string sTipoMov)
        {
            try
            {

                int iRegistros = 0;

                List<string> sFolios = new List<string>();
                List<venta_cabecera> vLista = null;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    var sQuery = "Select count(*) from venta_cabecera WHERE  vcn_cliente = ?  and vcc_tipo_pago= ?";
                    iRegistros = conn.ExecuteScalar<int>(sQuery, sClave, sTipoMov);
                    if (iRegistros > 0)
                    {
                        sQuery = "Select vcn_folio from venta_cabecera WHERE  vcn_cliente = ?   and vcc_tipo_pago= ?";
                        vLista = conn.Query<venta_cabecera>(sQuery, sClave, sTipoMov);
                       
                        foreach (var cabecera in vLista)
                            sFolios.Add(cabecera.vcn_folio);
                            
                    }

                }

                return sFolios;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return null;
            }
        }

        public bool GuardaNormalCompleta(venta_cabecera vCabecera,int iEstatus,bool bDocCab,bool bDocDet)
        {
            try
            {
                bool bResult , bResultado;
                decimal dPgYAnt = 0M;
                string sFolioCab = "";
                //VarEntorno.cCobranza.dCabecera

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    if (VarEntorno.bSoloCobrar == false)
                    {
                         //bResult = new fnCapturaEnvase().GuardaEnvase(vCabecera.vcn_cliente);

                        #region Declaración de Variables
                        string sqlTipoMov = "";
                        string sqlMovEnvase = "";
                        string sTipoMov = "";
                        string sqlUpdate = "";
                        string sEnvase = string.Empty;
                        string sFolio = string.Empty;
                        string sCargo = string.Empty;
                        string sAbono = string.Empty;
                        string sVenta = string.Empty;
                        string sQuery = "";
                        int iResultado = 0;
                        SQLiteCommand command;
                        #endregion Declaración de Variables

                        #region obtener envase temp
                        sqlMovEnvase = "select mec_envase, men_folio, sum(ifnull(men_cargo,0)) as men_cargo, " +
                                  "sum(ifnull(men_abono,0)) as men_abono, sum(ifnull(men_venta,0)) as men_venta " +
                                  "from envase_temp " +
                                  "where cln_clave = " + vCabecera.vcn_cliente +
                                  " and mec_envase!=''  group by mec_envase " +
                                  "order by mec_envase";
                        List<envase_temp> vDetEnvase = conn.Query<envase_temp>(sqlMovEnvase);
                                                
                        List<string> lstEnvacestmp = new List<string>();
                        foreach (var str in vDetEnvase)
                            lstEnvacestmp.Add(str.mec_envase);
                        #endregion

                        string Envase = " select * from envase " +
                                        " where cln_clave = " + vCabecera.vcn_cliente ;
                        List<envase> lEnvase = conn.Query<envase>(Envase);

                        List<string> lstEnvaces = new List<string>();
                        foreach (var str in lEnvase)
                            lstEnvaces.Add(str.mec_envase);

                        List<string>  NoExisten = (from p in lstEnvacestmp
                                                   where !(from ex in  lstEnvaces
                                                           select ex.ToString())
                                                            .Contains(p.ToString())
                                                    select p).ToList();


                        if ((NoExisten.Count==0 && lstEnvacestmp.Count>0)
                            || lstEnvacestmp.Count ==0)
                        {
                            iResultado=1;
                        }
                        else
                        {
                            foreach (var env1 in NoExisten)
                            sQuery += " insert into envase " +
                                     " (cln_clave, men_folio, mec_envase,men_saldo_inicial, men_cargo, men_abono " +
                                     "      , men_venta,men_saldo_final,mec_es_devolucion) " +
                                     " values " +
                                     "("+vCabecera.vcn_cliente +",'"+vCabecera.vcn_folio+"','"+env1+"',0,0,0,0,0,''); ";

                            command = conn.CreateCommand(sQuery);

                            iResultado = command.ExecuteNonQuery();
                        }

                        if (iResultado>0)
                        {
                            iResultado = 0;
                            int i = 0;

                            foreach (envase_temp movEnvase in vDetEnvase)
                            {
                                sEnvase = movEnvase.mec_envase;
                                sFolio = movEnvase.men_folio;
                                sCargo = movEnvase.men_cargo.ToString();
                                sAbono = movEnvase.men_abono.ToString();
                                sVenta = movEnvase.men_venta.ToString();
                                
                                if (VarEntorno.bEsDevolucion)
                                    sTipoMov = "D";
                                else
                                    sTipoMov = "V";

                                sqlUpdate = "update envase set " +
                                            "men_folio = '" + sFolio + "', " +
                                            "men_cargo = men_cargo + " + sCargo + ", " +
                                            "men_abono = men_abono + " + sAbono + ", " +
                                            "men_venta = men_venta + " + sVenta + ", " +
                                            "mec_es_devolucion = '" + sTipoMov + "' " +
                                            "where cln_clave = " + vCabecera.vcn_cliente + " " +
                                            "and mec_envase = '" + sEnvase + "'";
                                command = conn.CreateCommand(sqlUpdate);

                                iResultado = command.ExecuteNonQuery();

                                i = i + iResultado;
                            }

                            if (i== vDetEnvase.Count)
                            {
                                sqlUpdate = "update envase set " +
                                            "men_saldo_final = (men_saldo_inicial + men_cargo) - (men_abono + men_venta) " +
                                            "where cln_clave = " + vCabecera.vcn_cliente;
                                           
                                SQLiteCommand command2 = conn.CreateCommand(sqlUpdate);

                                iResultado = command2.ExecuteNonQuery();

                                if (iResultado >= 1)
                                {
                                    string sInsert = "delete from envase_temp where cln_clave = " + vCabecera.vcn_cliente;
                                    command = conn.CreateCommand(sInsert);

                                    iResultado = command.ExecuteNonQuery();

                                    if (  (iResultado == 0 && vDetEnvase.Count == 0)
                                        || (iResultado >= 1 && vDetEnvase.Count > 0))
                                        bResult = true;
                                    else
                                        bResult = false;
                                }
                                else
                                {
                                    bResult = false;
                                }
                            }
                            else
                            {
                                bResult = false;
                            }
                        }
                        else
                            bResult = true;
                    }
                    else
                        bResult = true;

                    if (bResult)
                    {
                        conn.BeginTransaction();
                        ///GUARDA LA VENTA CABECERA
                        int iResultado = conn.Insert(vCabecera);

                        if (iResultado > 0)
                        {
                            ///ACTUALIZA EL ESTATUS DEL CLIENTE
                            var sQuery =
                            @"UPDATE clientes_estatus 
                               SET  cln_visitado   = " + iEstatus + @"                              
                                WHERE cln_clave    = " + vCabecera.vcn_cliente + " and cln_visitado NOT IN (1,3) ";

                            SQLiteCommand command = conn.CreateCommand(sQuery);

                            iResultado = command.ExecuteNonQuery();

                            if (iResultado >= 0)
                            {
                                //SI HAY BONIFICACIONES LA MARCA SINO SIGUE 
                                //if (VarEntorno.cCobranza.lBonificacion.Count > 0)
                                if (VarEntorno.cCobranza.lpBonificacion.Count > 0)
                                {
                                    //foreach (var bonificacion in VarEntorno.cCobranza.lBonificacion)    /////listado de bonificaciones
                                    foreach (var bonificacion in VarEntorno.cCobranza.lpBonificacion)
                                    {
                                        sQuery =                                                            
                                            @"UPDATE bonificaciones 
                                            SET  boc_folio_venta  = '" + vCabecera.vcn_folio + @"'                                  
                                            WHERE (boc_folio = '" + bonificacion.vcc_referencia + @"')
                                            and (boc_cliente = " + vCabecera.vcn_cliente + @" )";

                                        command = conn.CreateCommand(sQuery);

                                        iResultado = command.ExecuteNonQuery();

                                        if (iResultado >= 1 && bResult)
                                            bResult = true;
                                        else
                                            bResult = false;
                                    }
                                }
                                else
                                    bResult = true;


                                if (bResult)
                                {
                                    ///INCREMENTA EL FOLIO  CONSECUTIVO
                                    sQuery =
                                    @"UPDATE ruta 
                                           SET run_folio = run_folio + 1";

                                    command = conn.CreateCommand(sQuery);

                                    iResultado = command.ExecuteNonQuery();

                                    if (iResultado >= 1)
                                    {
                                        // SI HAY PAGOS LOS GUARDA DE LO CONTRARIO AVANZA
                                        if (VarEntorno.cCobranza.lPagos.Count > 0)
                                        {
                                            int d = 0;
                                            //int nNpago = 1;
                                            int nNpago = 0;

                                            foreach (var pago in VarEntorno.cCobranza.lPagos)
                                            {
                                                if (pago.vcn_monto > 0)
                                                {
                                                    nNpago++;
                                                    pago.vpn_numpago = nNpago;
                                                    d = conn.Insert(pago);
                                                }
                                                else
                                                    d = 1;

                                                if (d <= 0)
                                                    break;
                                            }

                                            if (d > 0)
                                            {
                                                bResult = true;
                                                VarEntorno.cCobranza.dDetalle.ddn_cantidad_pagos = nNpago;
                                            }
                                            else
                                                bResult = false;
                                        }
                                        else
                                            bResult = true;

                                        if (bResult == true)
                                        {
                                            ///GUARDA LOS MOVIMIENTO CORRESPONDIENTES A LOS DOCUMENTOS                                             
                                            iResultado = 0;

                                            if (bDocCab == true 
                                                && VarEntorno.bEsDevolucion==false)                                            
                                                iResultado = conn.Insert(VarEntorno.cCobranza.dCabecera);                                            
                                            else
                                                iResultado = 1;

                                            if (iResultado==1)
                                            {
                                                if (bDocDet == true )  
                                                    iResultado = conn.Insert(VarEntorno.cCobranza.dDetalle);
                                                else
                                                    iResultado = 1;

                                                if (iResultado == 1 
                                                    && bDocDet == true 
                                                    && VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera != null)
                                                {
                                                    dPgYAnt = VarEntorno.cCobranza.dDetalle.vcn_monto_pago;
                                                    sFolioCab = VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera;
                                                    /*
                                                    sQuery = "update documentos_cabecera set dcn_numero_pago=coalesce(dcn_numero_pago,0)+1,dPagosVendedor = dPagosVendedor+? where vcn_folio = ? and vcn_cliente =? ";

                                                    command = conn.CreateCommand(sQuery, VarEntorno.cCobranza.dDetalle.vcn_monto_pago
                                                                                        , VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera
                                                                                        , VarEntorno.cCobranza.dDetalle.vcn_cliente);
                                                    iResultado = 0;
                                                    iResultado = command.ExecuteNonQuery();
                                                    */
                                                }
                                                else
                                                    iResultado = 1;
                                                
                                                if (iResultado > 0)
                                                {
                                                    if (VarEntorno.bAnticipoRelacionado)
                                                    {                                                        
                                                        sQuery = "update anticipos " +
                                                                    "set anb_relacionado = '1' " +
                                                                    "where cln_clave = " + VarEntorno.vCliente.cln_clave + " " +
                                                                    "and vcn_folio = '" + VarEntorno.cAnticipos.vcn_folio + "'";

                                                        SQLiteCommand command1 = conn.CreateCommand(sQuery);
                                                        iResultado = command1.ExecuteNonQuery();

                                                        if (iResultado > 0)
                                                        {
                                                            iResultado = conn.Insert(VarEntorno.cCobranza.dAnticipo);

                                                            if (iResultado > 0)
                                                            {
                                                                if (VarEntorno.cCobranza.dPagosTicket == 0)
                                                                    dPgYAnt =  VarEntorno.cCobranza.dAnticipo.vcn_monto_pago;
                                                                else
                                                                    dPgYAnt = dPgYAnt+ VarEntorno.cCobranza.dAnticipo.vcn_monto_pago;

                                                                sFolioCab = VarEntorno.cCobranza.dCabecera.vcn_folio;
                                                            }
                                                            else
                                                            {
                                                                bResultado = false;
                                                                VarEntorno.sMensajeError = "error guardar docto detalle anticipo ";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            bResultado = false;
                                                            VarEntorno.sMensajeError = "No se pudieron relacionar los anticipos ";
                                                        }

                                                        #region anticipo version 1
                                                        /*    
                                                        if (iResultado > 0 && VarEntorno.cCobranza.dDetalle.vcn_folio != null)
                                                        {
                                                            dPgYAnt = dPgYAnt + VarEntorno.cCobranza.dAnticipo.vcn_monto_pago;
                                                            sFolioCab = VarEntorno.cCobranza.dCabecera.vcn_folio;
                                                            
                                                            sQuery = "update documentos_cabecera set dcn_numero_pago=coalesce(dcn_numero_pago,0)+1,dPagosVendedor = dPagosVendedor+? where vcn_folio = ? and vcn_cliente =? ";

                                                            SQLiteCommand command2 = conn.CreateCommand(sQuery, VarEntorno.cCobranza.dAnticipo.vcn_monto_pago
                                                                                                , VarEntorno.cCobranza.dCabecera.vcn_folio
                                                                                                , VarEntorno.vCliente.cln_clave);
                                                            iResultado = command2.ExecuteNonQuery();
                                                            
                                                            if (iResultado > 0)
                                                            {
                                                                conn.Commit();
                                                                bResultado = true;
                                                                VarEntorno.sMensajeError = "";
                                                            }
                                                            else
                                                            {
                                                                conn.Rollback();
                                                                bResultado = false;
                                                                VarEntorno.sMensajeError = "No se pudo Guardar el detalle del Anticipo ";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            conn.Rollback();
                                                            
                                                        }   */
                                                        #endregion
                                                    }
                                                    else                                                    
                                                        iResultado = 1;
                                                        
                                                    
                                                    if (iResultado>0)
                                                    {
                                                        if (dPgYAnt > 0M)
                                                        {
                                                            sQuery = "update documentos_cabecera set dcn_numero_pago=coalesce(dcn_numero_pago,0)+1,dPagosVendedor = dPagosVendedor+? where vcn_folio = ? and vcn_cliente =? ";

                                                            command = conn.CreateCommand(sQuery, dPgYAnt
                                                                                                , sFolioCab
                                                                                                , VarEntorno.vCliente.cln_clave);
                                                            iResultado = 0;
                                                            iResultado = command.ExecuteNonQuery();
                                                        }
                                                        else
                                                            iResultado = 1;

                                                        if (iResultado > 0)
                                                        {
                                                            conn.Commit();
                                                            bResultado = true;
                                                            VarEntorno.sMensajeError = "";
                                                        }
                                                        else
                                                        {
                                                            conn.Rollback();
                                                            bResultado = false;
                                                            VarEntorno.sMensajeError = "Error de pagos en cabecera ";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        conn.Rollback();
                                                        bResultado = false;
                                                        // mensaje de error desde ANTICIPOS
                                                        //VarEntorno.sMensajeError = "Error en anticipos ";
                                                    }
                                                }
                                                else
                                                {
                                                    conn.Rollback();
                                                    bResultado = false;
                                                    VarEntorno.sMensajeError = "Error en guardar documentos  Det ";
                                                }
                                            }
                                            else
                                            {
                                                conn.Rollback();
                                                bResultado = false;
                                                VarEntorno.sMensajeError = "Error en guardar Doctos Cab  ";
                                            }                                            
                                        }
                                        else
                                        {
                                            conn.Rollback();
                                            bResultado = false;
                                            VarEntorno.sMensajeError = "Error en guardar pagos  ";
                                        }
                                    }
                                    else
                                    {
                                        conn.Rollback();
                                        bResultado = false;
                                        VarEntorno.sMensajeError = "Error al incrementar el folio ";
                                    }
                                }
                                else
                                {
                                    conn.Rollback();
                                    bResultado = false;
                                    VarEntorno.sMensajeError = "Error al guardar el bonificaciones del cliente ";
                                }
                            }
                            else
                            {
                                conn.Rollback();
                                bResultado = false;
                                VarEntorno.sMensajeError = "Error al guardar el estatus del cliente ";
                            }
                        }
                        else
                        {
                            conn.Rollback();
                            bResultado = false;
                            VarEntorno.sMensajeError = "Error al guardar la cabecera ";
                        }
                    }
                    else
                    {
                        conn.Rollback();
                        VarEntorno.sMensajeError = "Error al guardar el envase ";
                        bResultado = false;                        
                    }
                }

                return bResultado;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public bool GuardaDevolucionCompleta(venta_cabecera vCabecera,int iMotiDev)
        {
            try
            {
                bool  bResultado;

                using (SQLiteConnection conn = cODBC.CadenaConexion())
                {
                    if (new fnVentaDetalle().fnDevolucionProducto(vCabecera.vcn_cliente, vCabecera.vcn_folio) == false)
                    {
                        bResultado = false;
                        VarEntorno.sMensajeError = "Error al devolver los productos ";
                    }
                    else
                    {
                        conn.BeginTransaction();

                        int iResultado = conn.Insert(vCabecera);

                        if (iResultado > 0)
                        {
                            devoluciones dv = new devoluciones();
                            dv.dev_clave = iMotiDev;
                            dv.cln_clave = vCabecera.vcn_cliente;

                            iResultado = conn.Insert(dv);

                            if (iResultado > 0)
                            {
                                var sQuery =
                                    @"UPDATE clientes_estatus 
                                           SET  cln_visitado   = 3                              
                                    WHERE cln_clave    = " + vCabecera.vcn_cliente + " and cln_visitado NOT IN (1,3) ";

                                SQLiteCommand command = conn.CreateCommand(sQuery);

                                iResultado = command.ExecuteNonQuery();

                                if (iResultado >= 0)
                                {
                                    sQuery =
                                     @"update envase set  men_cargo = 0, men_abono = 0 "+
                                     ",men_saldo_final = ifnull(men_saldo_inicial,0) + ifnull(men_cargo,0) - ifnull(men_abono,0) - ifnull(men_venta,0)  where cln_clave = ? ";

                                    command = conn.CreateCommand(sQuery, vCabecera.vcn_cliente);

                                    iResultado = command.ExecuteNonQuery();

                                    if (iResultado >= 1)
                                    {
                                        sQuery =
                                        @"UPDATE ruta 
                                               SET run_folio = run_folio + 1";

                                        command = conn.CreateCommand(sQuery);

                                        iResultado = command.ExecuteNonQuery();

                                        if (iResultado >= 1)
                                        {
                                            sQuery = "update documentos_cabecera set  dcc_tipo = 'D' where vcn_cliente = ? and vcf_movimiento in (select max(vcf_movimiento) from FacturasVenta)";

                                            command = conn.CreateCommand(sQuery, vCabecera.vcn_cliente);

                                            iResultado = command.ExecuteNonQuery();

                                            if (iResultado >= 1)
                                            {
                                                conn.Commit();
                                                bResultado = true;
                                                VarEntorno.sMensajeError = "";
                                            }
                                            else
                                            {
                                                conn.Rollback();
                                                bResultado = false;
                                                VarEntorno.sMensajeError = "Error al actualizar el documento  ";
                                            }
                                        }
                                        else
                                        {
                                            conn.Rollback();
                                            bResultado = false;
                                            VarEntorno.sMensajeError = "Error al incrementar el folio ";
                                        }
                                    }
                                    else
                                    {
                                        conn.Rollback();
                                        bResultado = false;
                                        VarEntorno.sMensajeError = "Error al quitar movimientos de envase ";
                                    }
                                }
                                else
                                {
                                    conn.Rollback();
                                    bResultado = false;
                                    VarEntorno.sMensajeError = "Error al guardar el estatus ";
                                }
                            }
                            else
                            {
                                conn.Rollback();
                                bResultado = false;
                                VarEntorno.sMensajeError = "Error al guardar el motivos ";
                            }
                        }
                        else
                        {
                            conn.Rollback();
                            bResultado = false;
                            VarEntorno.sMensajeError = "Error al guardar la cabecera ";
                        }
                    }
                }

                if (bResultado == false)
                {
                    //recargar el producto del detalle 
                }

                return bResultado;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }
    }
}
