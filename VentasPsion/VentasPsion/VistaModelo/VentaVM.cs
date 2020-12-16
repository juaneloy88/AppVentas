using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;
using VentasPsion.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VentasPsion.VistaModelo
{
    class VentaVM
    {
        public productos pProducto { get; set; }
        public int iExistencia { get; set; }
        public int iVendido { get; set; }
        public int iAbonoEnvase { get; set; }
        public decimal dPrecio { get; set; }
        public bool bArtActualizado { get; set; }

        public bool bVentaxPromo { get; set; }

        public int iVendidoAnt { get; set; }
        private int iAbnEnvAnt { get; set; }
        private string sArticulo { get; set; }

        //desactivar
        public int iDevuelto { get; set; }
        public int iSaldoEnvase { get; set; }

        private fnVentaDetalle VenDetFN = new fnVentaDetalle();
        private venta_detalleSR VenDetSR = new venta_detalleSR();
        private EnvaseService EnvFn =  new EnvaseService();
        private candados_productosSR CandFN = new candados_productosSR();

        //****** busca los atributos del articulo por medio de otras funciones ******//
        public bool fnCargaProducto(string sClave)
        {
            try
            {
                sArticulo = sClave;
                //* busca los atributos del producto *//
                if (fnCargaArticulo())
                {
                    //* busca el precio del producto del cliente *//
                    if (fnCargaPrecio())
                    {
                        //*   valida que tipo de proceso a realizar  *//
                        if (VarEntorno.bEsDevolucion)
                        {
                            //*  busca la cantidad devuelta con anterioridad  *// 
                            if (fnCargaProductoDevuelto())
                            {
                                //*  carga saldo de envase solo se usa para autoventa  *//
                                if (fnCargaSaldoEnvase())
                                {
                                    return true;
                                }
                                else
                                {
                                    VarEntorno.sMensajeError = "Error al saldo env" + VarEntorno.sMensajeError;
                                    return false;
                                }
                            }
                            else
                            {
                                VarEntorno.sMensajeError = "Error al dev articulo" + VarEntorno.sMensajeError;
                                return false;
                            }
                        }
                        else
                        {
                            //* busca la existencia del producto  *//
                            if (fnCargaExistencia())
                            {
                                //* busca si el producto ya habia sido vendido *//
                                if (fnCargaProductoVendido())
                                {
                                    if (fnCargaProductoVendidoxPromo())
                                    {
                                        return true;
                                        //*  carga abono de envase solo se usa para autoventa  *//
                                        /*
                                        if (fnCargaAbonoEnvase())
                                        {
                                            return true;
                                        }
                                        else
                                        {
                                            VarEntorno.sMensajeError = "Error al abonos env" + VarEntorno.sMensajeError;
                                            return false;
                                        }*/
                                    }
                                    else
                                    {
                                        VarEntorno.sMensajeError = "Error al tipo de ingreso " + VarEntorno.sMensajeError;
                                        return false;
                                    }
                                }
                                else
                                {
                                    VarEntorno.sMensajeError = "Error al venta articulo" + VarEntorno.sMensajeError;
                                    return false;
                                }
                            }
                            else
                            {
                                VarEntorno.sMensajeError = "Error al cargar existencia" + VarEntorno.sMensajeError;
                                return false;
                            }
                        }                            
                    }
                    else
                    {
                        VarEntorno.sMensajeError = "Error al cargar precio" + VarEntorno.sMensajeError;
                        return false;
                    }                    
                }
                else
                {
                    VarEntorno.sMensajeError = "Error al cargar Articulo" + VarEntorno.sMensajeError;
                    return false;
                }                
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //***** busca los atributos del producto ********//
        private bool fnCargaArticulo()
        {
            try
            {   
                pProducto = new productoSR().BuscarProducto(sArticulo);
                if (pProducto == null )
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

        //****** busca la existencia del producto  ******//
        private bool fnCargaExistencia()
        {
            try
            {
                bool bResultado = false;
                iExistencia = new existenciaSR().BuscarExistencia(pProducto.arc_afecta);
                //int iVendidoDia = VenDetFN.fnBuscaProductoVendido(pProducto.arc_afecta);
                                
                    if (iExistencia == -999)
                        bResultado = false;
                    else
                        bResultado = true;
                
                return bResultado;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******* busca el precio del producto del cliente ******//
        private bool fnCargaPrecio()
        {
            try
            {
                dPrecio = new lista_maestraSR(). ProductoxListaPrecio(sArticulo,VarEntorno.vCliente.lmc_tipo);
             
                if (dPrecio < 0M)
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

        //******** busca si el producto ya habia sido vendido *******//
        private bool fnCargaProductoVendido()
        {
            try
            {
                iVendido = VenDetSR.VentasxClientexFolioxArt(VarEntorno.vCliente.cln_clave
                                                            ,VarEntorno.iFolio.ToString().PadLeft(6, '0')
                                                            ,this.sArticulo
                                                            ).vdn_venta;
                                                            //VenDetFN.fnBuscaProductoXCliente(sArticulo);

                if (iVendido == -1)
                    return false;
                else
                {
                    if (iVendido > 0)
                        bArtActualizado = true;
                    else
                        bArtActualizado = false;
                    iVendidoAnt = iVendido;
                    return true;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******** busca si el producto ya habia sido vendido  por promocion *******//
        private bool fnCargaProductoVendidoxPromo()
        {
            try
            {
                //int iResultado = VenDetFN.fnBuscaProductoVendidoxPromo(sArticulo);
                int iResultado = VenDetSR.VentasxClientexFolioxArt(VarEntorno.vCliente.cln_clave
                                                            , VarEntorno.iFolio.ToString().PadLeft(6, '0')
                                                            , this.sArticulo).vdn_tipo_promo;
                bool bResultado ;

                switch (iResultado)
                {
                    case -1:
                        bResultado = false;
                        bVentaxPromo = false;
                        break;
                    case 0:
                        bResultado = true;
                        bVentaxPromo = false;
                        break;
                    default:
                        bResultado = true;
                        bVentaxPromo = true;
                        break;
                }

                return bResultado;

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******** guarda la venta detalle *********//
        public bool fnGuardarVenta(int stipo)
        {
            try
            {
                return VenDetFN.fnGuardarProductoFact(this, stipo,VarEntorno.vCliente.lmc_tipo);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******* actualiza la venta detalle *******//
        public bool fnActualizaVenta()
        {
            try
            {
                return VenDetSR.fnActualizaProducto(this);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******* borrar la venta detalle *******//
        public bool fnBorrarVenta()
        {
            try
            {
                return VenDetSR.fnBorrarProducto(this);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //*********valida si la clave es parte de una promocion en conjunto
        public int fnEsPromoConjunto()
        {
            try
            {
                //return VenDetFN.fnEsPromoConjunto(pProducto.arc_clave);
                var ven_det = VenDetSR.VentasxClientexFolioxArt(VarEntorno.vCliente.cln_clave
                                                            , VarEntorno.iFolio.ToString().PadLeft(6, '0')
                                                            , this.sArticulo);

                if (ven_det.vdn_tipo_promo==0)
                    return 0;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return -1;
            }
        }

        //******* borrar la venta detalle *******//
        public bool fnBorrarVentaConjunto()
        {
            try
            {
                return VenDetFN.fnBorrarProductoConjunto(this);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******* actualiza la movimientos de envase *******//
        public bool fnActualizaEnvase(string sMarca, int iCargo, int iAbono)
        {
            try
            {
                //return EnvFn.fnEnvasexClientexCargo_Abono(VarEntorno.vCliente.cln_clave
                //                                                        ,this.pProducto.arc_envase
                //                                                        ,-iVendidoAnt+this.iVendido
                //                                                        ,-iAbnEnvAnt + this.iAbonoEnvase
                //                                                        ,VarEntorno.iFolio.ToString().PadLeft(6,'0'));

                return EnvFn.fnEnvaseClienteCargo_Abono(VarEntorno.vCliente.cln_clave, this.pProducto.arc_envase,
                                                                      iCargo, iAbono,
                                                                      VarEntorno.iFolio.ToString().PadLeft(6, '0'), sMarca);
                
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }


        public bool fnCandadosArticulos(string sProducto)
        {
            try
            {
                return CandFN.fnProductoconRestriccionV(sProducto
                     , VarEntorno.vCliente.clc_mod_nivel1.Substring(0,1), VarEntorno.vCliente.ctp_clave);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }            
        }

        //*****  valida si la clave no tiene restriccion de cantidad de venta  ******//
        public string fnValidadCantidad(string sArticulo,int iCantidad)
        {
            try
            {
                /////ciclo de los candados de la clave
                //revisa las cantidades de tickets del dia
                ///verifica si la cantida ant mas la actual cumple con la validacion
                int iCan = VenDetFN.fnBuscaProductoCliente(sArticulo) + iCantidad;

                return CandFN.fnValidaRestriccion( sArticulo, iCan
                                ,VarEntorno.vCliente.clc_mod_nivel1.Substring(0, 1), VarEntorno.vCliente.ctp_clave);
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return "Error";
            }
        }



        //*****  trae el importe de los artoculos del ticket  ****/
        public decimal fnImporteTotalxFolio()
        {
            try
            {
                return VenDetSR.fnImporteTotalxFolio(VarEntorno.iFolio);

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        //*****  trae el importe de los artoculos del ticket  ****/
        public decimal fnImporteTotalAntFolio()
        {
            try
            {
                return VenDetFN.fnImporteTotalxClienteconExclucion(VarEntorno.vCliente.cln_clave,  VarEntorno.iFolio.ToString().PadLeft(6, '0'));

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        //*****   obtiene el abono de envase del articulo correspondiente *****// 
        private bool fnCargaAbonoEnvase()
        {
            try
            {
                iAbnEnvAnt = EnvFn.fnEnvaseAbonoCliente(pProducto.arc_envase, VarEntorno.vCliente.cln_clave);
                iAbonoEnvase = iAbnEnvAnt;
                if (iAbnEnvAnt == -999)
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

        public bool fnVentaEnvase(string sMarca)
        {
            try
            {
                return EnvFn.fnEnvaseVenta(pProducto.arc_clave
                                                        , VarEntorno.vCliente.cln_clave
                                                        ,this.iVendido
                                                        , VarEntorno.iFolio.ToString().PadLeft(6, '0')
                                                        , sMarca);                
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public async Task< bool> fnLimpiaTicket(int iFolio)
        {
            string sResultado = await VenDetSR.BorrarVenta(iFolio.ToString().PadLeft(6,'0'));
            if ("Ticket Eliminado" == sResultado)
                return true;
            else
                return false;
        }

        #region Devolucion 
        //******** busca si el producto ya habia sido devuelto *******//
        private bool fnCargaProductoDevuelto()
        {
            try
            {
                iDevuelto = VenDetFN.fnBuscaProductoXClienteDev(sArticulo);

                if (iDevuelto == -1)
                    return false;
                else
                {
                    if (iDevuelto > 0)
                        bArtActualizado = true;
                    else
                        bArtActualizado = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //********  carga el saldo inicla del envase   *********///
        private bool fnCargaSaldoEnvase()
        {
            try
            {
                iSaldoEnvase = EnvFn.fnEnvaseSaldoCliente(pProducto.arc_envase,VarEntorno.vCliente.cln_clave) ;

                if (iDevuelto == -999)
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

        //******** guarda la venta detalle *********//
        public bool fnGuardarDevolucion()
        {
            try
            {
                 return VenDetFN.fnGuardarProductoDev(this, VarEntorno.vCliente.lmc_tipo);
           
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******* actualiza la venta detalle *******//
        public bool fnActualizaDevolucion()
        {
            try
            {
                 return VenDetFN.fnActualizaProductoDev(this);
                
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        //******* actualiza la movimientos de envase en devolucion*******//
        public bool fnActualizaEnvaseDev()
        {
            try
            {
                return EnvFn.fnEnvasexClientexCargo_Abono(VarEntorno.vCliente.cln_clave
                                                                        , this.pProducto.arc_envase
                                                                        , 0
                                                                        , this.iVendido
                                                                        , VarEntorno.iFolio.ToString().PadLeft(6, '0'));
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        public int fnArticulosDevxEnvase(int iCliente, string sEnvase)
        {
            try
            {
                return VenDetFN.fnProductoDevxEnvase(iCliente, sEnvase);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return -1;
            }
        }
        #endregion

        public int fnExisteVenta()
        {
            try
            {                
                int iExiste = VenDetFN.fnExisteVenta(VarEntorno.vCliente.cln_clave);

                return iExiste;

            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }

        public int fnExisteDevolucion()
        {
            try
            {
                int iExiste = VenDetFN.fnExisteDevol(VarEntorno.vCliente.cln_clave);

                return iExiste;
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return -1;
            }
        }



        public bool ArticuloContado(string sArticulo)
        {
            try
            {
                
                return false;///pProducto.arc_contado
            }
            catch
            {
                return false;
            }
        }

        public bool ArticulosDeContado()
        {
            try
            {
                return true;///VenDetFN.ProductosDeContado();
            }
            catch
            {
                return false;
            }
        }

    }
}
