using Base;
using System;
using System.Threading.Tasks;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmCobranza : ContentPage
    {
        DocumentosVM Docu = new DocumentosVM();
        AnticiposVM Anti = new AnticiposVM();
        bool bDoc = false;
        bool bAnticipo = false;
        int iFormaPago = 0;
        bool iEspera = true;

        public frmCobranza()
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            if (VarEntorno.cCobranza is null)
                VarEntorno.cCobranza = new CobranzaVM();

            try
            {
                if (VarEntorno.cTipoVenta=='R')
                    bDoc = Docu.UltimoDoc();
            }
            catch
            { }
            VarEntorno.cCobranza.vCobranza = this;
            CargaDatosGenerales();
        }

        /***  Carga basica de infromacion para cobranza invocando del tipo de perfil  ****/
        private void CargaDatosGenerales()
        {
            if (VarEntorno.bSoloCobrar)
            {
                VarEntorno.iFolio = VarEntorno.TraeFolio();
                VarEntorno.dImporteTotal = 0;
                //lblsincom.IsVisible = false;
            }
            else
                VarEntorno.dImporteTotal = VarEntorno.bEsDevolucion ? (-1 * VarEntorno.dImporteTotal) : VarEntorno.dImporteTotal;
                        
            ///carga informacion especifica de perfil
            switch (VarEntorno.cTipoVenta)
            {
                default:
                    CargaDatos_AutoventaPreventa();
                    lblSaldoAnt.Text = String.Format("{0:N2}", VarEntorno.Saldo(VarEntorno.vCliente));
                    break;
                case 'R':
                    CargaDatos_Reparto();
                    lblSaldoAnt.Text = String.Format("{0:N2}", VarEntorno.Saldo(VarEntorno.vCliente)
                                                                - VarEntorno.dImporteTotal);
                    break;
            }

                       
            lblClave.Text       = VarEntorno.vCliente.cln_clave.ToString();
            lblLimite.Text      = String.Format("{0:N2}", VarEntorno.vCliente.cln_limite_venta);            
            
            lblSaldoTotal.Text = String.Format("{0:N2}", /*Convert.ToDecimal(lblSaldoAnt.Text)
                                                         +*/ Convert.ToDecimal(lblVenta.Text));
            CargaPagos();

        }

        public void CargaPagos()
        {
            ///inicializa varibles para ls pagos 
            decimal dPTarjeta = 0;
            decimal dPBonificacion = 0;
            decimal dPCheque = 0;
            decimal dAnticipos = 0;
            decimal dPTransferencia = 0;

            /*
            foreach (var tarjeta in VarEntorno.cCobranza.lTarjeta)
                dPTarjeta += tarjeta.dCantidad;
                */
            foreach (var tarjeta in VarEntorno.cCobranza.lpTarjeta)
                dPTarjeta += tarjeta.vcn_monto;
            /*
            foreach (var bonificacion in VarEntorno.cCobranza.lBonificacion)
                dPBonificacion += bonificacion.boi_documento;
                */

            foreach (var bonificacion in VarEntorno.cCobranza.lpBonificacion)
                dPBonificacion += bonificacion.vcn_monto;

            /*
            foreach (var cheque in VarEntorno.cCobranza.lCheque)
                dPCheque += cheque.dMonto;
                */
            foreach (var cheque in VarEntorno.cCobranza.lpCheques)
                dPCheque += cheque.vcn_monto;

            foreach (var transferencia in VarEntorno.cCobranza.lpTransferencia)
                dPTransferencia += transferencia.vcn_monto;

            if (VarEntorno.bAnticipoRelacionado)            
                if (VarEntorno.cAnticipos.dMontoPago > 0M)
                    dAnticipos = VarEntorno.cAnticipos.dMontoPago;

            lblTajeta.Text = String.Format("{0:N2}", dPTarjeta);
            lblBonificacion.Text = String.Format("{0:N2}", dPBonificacion);
            lblCheque.Text = String.Format("{0:N2}", dPCheque);
            lblEfectivo.Text = String.Format("{0:N2}", VarEntorno.cCobranza.pEfectivo.vcn_monto//VarEntorno.cCobranza.dEfectivo
                                //+ VarEntorno.cCobranza.cTransferencia.dCantidad
                                + dPTransferencia
                                );
            lblAnticipo.Text = String.Format("{0:N2}", dAnticipos);

            VarEntorno.cCobranza.dPagosTicket = VarEntorno.cCobranza.pEfectivo.vcn_monto//VarEntorno.cCobranza.dEfectivo
                                    + dPTarjeta                                  
                                    + dPCheque
                                    //+ VarEntorno.cCobranza.cTransferencia.dCantidad
                                    + dPTransferencia
                                    + dPBonificacion
                                    ;

            lblSaldoNuevo.Text = String.Format("{0:N2}", Convert.ToDecimal(lblSaldoTotal.Text)
                                                        - VarEntorno.cCobranza.dPagosTicket
                                                        - dAnticipos);

            if (VarEntorno.cCobranza.dPagosTicket != 0M)
            {
                VarEntorno.cCobranza.dPagosTicket = VarEntorno.cCobranza.dPagosTicket + dAnticipos;
                bAnticipo = true;
            }
        }

        /*****   Carga ventas y pago previos para el calculo final del limite de venta   ******/
        private void CargaDatos_AutoventaPreventa()
        {
            lblPagoPrev.IsVisible = false;
            lblPPreventa.IsVisible = false;            

            //if (VarEntorno.bSoloCobrar)
            //    btnBonifcacion.IsVisible = true;
            //else
            {
                lblVenta.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal - VarEntorno.vCliente.clc_pago_diferencia_preventa);

                if (VarEntorno.cCobranza.PagosVentasClienteHoy() == false)
                    DisplayAlert("Aviso", "Error al buscar pagos ant", "OK");

                if (VarEntorno.cCobranza.VentasClienteHoy() == false)
                    DisplayAlert("Aviso", "Error al buscar ventas ant", "OK");

                if (VarEntorno.vCliente.clc_cobrador == "R" && VarEntorno.bVentaContado == false)
                {
                    btnEfectivo.IsEnabled = false;
                    btnCheque.IsEnabled = false;
                    btnTarjeta.IsEnabled = false;
                    btnTrasnferencia.IsEnabled = false;
                }

                lblPagoPrev.IsVisible = true;
                lblPPreventa.IsVisible = true;

                lblPagoPrev.Text = VarEntorno.bVentaContado ? "Contado" : "Credito"; 
                lblPPreventa.Text = "T. Venta";

                //btnBonifcacion.IsVisible = false;
            }
        }
        
        /****  carga aldo dependoiendo de la venta y pago de preventa   ****/
        private void CargaDatos_Reparto()
        {
            if (VarEntorno.bSoloCobrar == true)
            {
                lblPagoPrev.IsVisible = false;
                lblPPreventa.IsVisible = false;
            }
            else            
            {
                
                //lblVenta.Text = String.Format("{0:N2}", Facturas.SaldoPedidoReparto(VarEntorno.vCliente.cln_clave));
                lblPagoPrev.IsVisible = true;
                lblPPreventa.IsVisible = true;
                //lblPagoPrev.Text = String.Format("{0:N2}", VarEntorno.vCliente.clc_pago_diferencia_preventa);
                //lblVenta.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal - VarEntorno.vCliente.clc_pago_diferencia_preventa);
                VarEntorno.cCobranza.dImporteNeto = VarEntorno.dImporteTotal - Docu.Facturas.AbonoPreventa(VarEntorno.vCliente.cln_clave);
                lblPagoPrev.Text = String.Format("{0:N2}", Docu.Facturas.AbonoPreventa(VarEntorno.vCliente.cln_clave));
                lblVenta.Text = String.Format("{0:N2}", VarEntorno.cCobranza.dImporteNeto);

                if (VarEntorno.vCliente.clb_ticket_cobranza)
                {
                    lblMsn1.IsVisible = true;
                    lblMsn2.IsVisible = true;
                    lblMsn3.IsVisible = true;
                }
            }

        }

        /****   valida el limite de venta y credito junto con los demas tickets del cliente y sus pagos ****/
        private async Task<bool> ValidaVenta()
        {
            decimal dSubtotal = 0;
            
            bool bResultado = false;
            bool bHayAntiPend = false;
            bool bEstatus = false;

            switch (VarEntorno.cTipoVenta)
            {
                #region REPARTO
                case 'R':
                    ///valida el credito                   
                    if (VarEntorno.vCliente.clc_cobrador != "")
                    {
                        if ((await ValidaCredito()))
                            bResultado = true;
                        else
                            bResultado = false;
                    }
                    else
                    {
                        if (VarEntorno.cCobranza.dPagosTicket > 0M)
                        {
                            if (await ValidaCredito() == false)
                                bResultado = false;
                            else
                                bResultado = true;
                        }
                        else
                            bResultado = true;
                    }                    
                        break;
                #endregion
                default:
                    if (VarEntorno.dImporteTotal != 0M)
                    {
                        if (VarEntorno.bEsDevolucion == false)
                        {
                            // obtiene el total de todas las ventas y todos los pagos

                            dSubtotal = (VarEntorno.dImporteTotal + VarEntorno.cCobranza.dVentasCliente)
                                - (VarEntorno.cCobranza.dPagosTicket + VarEntorno.cCobranza.dPagosCliente);

                            if (VarEntorno.vCliente.clc_cobrador != "")
                            {
                                /*if ((VarEntorno.cTipoVenta == 'A' && VarEntorno.bVentaContado == false)
                                    || (VarEntorno.cTipoVenta == 'P' && VarEntorno.bVentaContado == false ))*/
                                if (VarEntorno.vCliente.cln_limite_venta != 0M && VarEntorno.bVentaContado == false)
                                {
                                    ////verifica el limite de venta 
                                    if (dSubtotal > VarEntorno.vCliente.cln_limite_venta)
                                    {
                                        decimal dDif = 0;                                        

                                        if (valida_limite_total_venta())                                        
                                            dDif = VarEntorno.dImporteTotal;                                        
                                        else
                                            dDif = (dSubtotal - VarEntorno.vCliente.cln_limite_venta);

                                        //var respuesta = await DisplayAlert("Aviso", "Sobrepasa el limite de credito desea pagar la diferencia en efectivo " + dDif, "Si", "No");
                                         await DisplayAlert("Aviso", "Sobrepasa el limite de credito debe pagar la diferencia  " + dDif, "Ok");

                                        bResultado = false;

                                        #region pago de diferencia directo 
                                        ///verifica respuesta de si desea pagar limite de venta 
                                        
                                        /*
                                        if (respuesta)
                                        {
                                            ///verifica el pago de transferencia en vacio 
                                            if (VarEntorno.cCobranza.cTransferencia.dCantidad > 0M)
                                            {
                                                respuesta = await DisplayAlert("Aviso", "Desea eliminar el pago  de trasferencia por pago de diferencia ", "Si", "No");
                                                ///elimina transferencia y guarda diferencia
                                                if (respuesta)
                                                {
                                                    VarEntorno.cCobranza.cTransferencia = new Transferencia();
                                                    VarEntorno.cCobranza.dEfectivo = dDif;
                                                    VarEntorno.cCobranza.EstatusClientePagoPro();
                                                    bResultado = true;
                                                }
                                                else                                                
                                                    bResultado = false;                                                
                                            }
                                            else
                                            {
                                                ///guarda pago de diferencia 
                                                VarEntorno.cCobranza.dEfectivo += dDif;
                                                VarEntorno.cCobranza.EstatusClientePagoPro();
                                                CargaPagos();
                                                bResultado = false;
                                            }
                                        }
                                        else                                        
                                            bResultado = false;  
                                        */
                                        #endregion
                                    }
                                    else                                    
                                        bResultado = true;                                    
                                }
                                else                                
                                    bResultado = true;                                
                            }
                            else                            
                                bResultado = true;                            
                        }
                        else                        
                            bResultado = true;                        
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Ticket de venta sin productos ", "OK");
                        bResultado = false;
                    }

                    if (VarEntorno.bEsDevolucion == false)
                        if (bResultado)
                        {
                            if (Anti.lAnt())
                            {                                
                                foreach (var Anticipos in Anti.lAnticipos)
                                {
                                    if (Anticipos.ann_monto_pago < VarEntorno.dImporteTotal
                                        && VarEntorno.bAnticipoRelacionado == false
                                        && bEstatus == false)
                                            bEstatus = true;                                    
                                }
                                bHayAntiPend = bEstatus;
                            }
                            else                            
                                bHayAntiPend = false;
                            

                            if (bHayAntiPend)
                            {
                                await DisplayAlert("Aviso", "favor de aplicar el anticipo", "OK");
                                bResultado = false;
                            }
                            else
                            {
                                if (VarEntorno.vCliente.clc_cobrador != "")
                                {
                                    if (await ValidaCredito() == false)
                                        bResultado = false;
                                    else
                                        bResultado = true;
                                }
                                else
                                {
                                    if (VarEntorno.bVentaContado
                                            || VarEntorno.cCobranza.dPagosTicket > 0)
                                    {
                                        if (await ValidaCredito() == false)
                                            bResultado = false;
                                        else
                                            bResultado = true;
                                    }
                                    else
                                        bResultado = true;
                                }
                            }
                        }

                        break;
                    }
            
            return bResultado;
        }
        
        /****   valida el limite de venta y credito junto con los demas tickets del cliente y sus pagos ****/
        private async Task<bool> ValidaVentaNew()
        {
            decimal dSubtotal = 0;
            bool bResultado = false;

            if (VarEntorno.cTipoVenta != 'R')
            {
                if (VarEntorno.dImporteTotal != 0M)
                {
                    if (VarEntorno.bEsDevolucion == false)
                    {
                        if (VarEntorno.vCliente.clc_cobrador != "")
                        {
                            if (VarEntorno.vCliente.cln_limite_venta != 0M && VarEntorno.bVentaContado == false)
                            {
                                ////verifica el limite de venta 
                                if (dSubtotal > VarEntorno.vCliente.cln_limite_venta)
                                {
                                    decimal dDif = 0;
                                    // obtiene el total de todas las ventas y todos los pagos
                                    dSubtotal = (VarEntorno.dImporteTotal + VarEntorno.cCobranza.dVentasCliente)
                                        - (VarEntorno.cCobranza.dPagosTicket + VarEntorno.cCobranza.dPagosCliente);

                                    if (valida_limite_total_venta())                                    
                                        dDif = VarEntorno.dImporteTotal;
                                    else
                                        dDif = (dSubtotal - VarEntorno.vCliente.cln_limite_venta);

                                    await DisplayAlert("Aviso", "Sobrepasa el limite de credito debe pagar la diferencia  " + dDif, "Ok");

                                    bResultado = false;
                                }
                                else
                                    bResultado = true;
                            }
                            else
                                bResultado = true;
                        }
                        else
                            bResultado = true;                           
                    }
                    else
                        bResultado = true;
                }
                else
                {
                    await DisplayAlert("Aviso", "Ticket de venta sin productos ", "OK");
                    bResultado = false;
                }
            }
            else
                bResultado = true;

            if (VarEntorno.vCliente.clc_cobrador != "")
            {
                if (VarEntorno.bEsDevolucion == false)
                {
                    if (bResultado)
                        bResultado = await ValidaCredito();
                }
            }
            else
            {
                if (VarEntorno.bVentaContado || VarEntorno.cCobranza.dPagosTicket > 0)
                {
                    if (await ValidaCredito() == false)
                        bResultado = false;
                    else
                        bResultado = true;
                }
                else
                    bResultado = true;
            }

            return bResultado;
        }

        /*****  valida el credito *******/
        private async Task<bool> ValidaCredito()
        {
            bool bresultado = false;
            decimal dVal = 0;
            decimal dAnticipo = 0M;

            if (bAnticipo)
                dAnticipo = Convert.ToDecimal(lblAnticipo.Text);
            else
                dAnticipo = 0M;

            if (VarEntorno.bVentaContado
                 || (VarEntorno.cCobranza.dPagosTicket > 0 && VarEntorno.cTipoVenta != 'R')
                )
            {

                //if (Convert.ToDecimal(lblSaldoAnt.Text) < 0M)
                //    dVal = Convert.ToDecimal(lblSaldoTotal.Text);
                //else      

                dVal = Convert.ToDecimal(lblVenta.Text);
                
                if (VarEntorno.bVentaContado)
                {                
                    if (VarEntorno.cCobranza.dPagosTicket == dVal)
                    {
                        VarEntorno.cCobranza.dDetalle.vcf_movimiento = DateTime.Now.Date;
                        VarEntorno.cCobranza.dDetalle.vcf_movimiento_cabecera = DateTime.Now.Date;// Docu.lDoctosCabecera[0].vcf_movimiento;
                        VarEntorno.cCobranza.dDetalle.vcn_cliente = VarEntorno.vCliente.cln_clave;
                        VarEntorno.cCobranza.dDetalle.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                        VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera = VarEntorno.iFolio.ToString().PadLeft(6, '0');//Docu.lDoctosCabecera[0].vcn_folio;                                
                        VarEntorno.cCobranza.dDetalle.vcn_monto_pago = VarEntorno.cCobranza.dPagosTicket
                                                                       - dAnticipo;
                        VarEntorno.cCobranza.dDetalle.ddn_numero_pago = 1;
                        bresultado = true;
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "debe liquidar la venta exactamente", "OK");
                        bresultado = false;
                    }                    
                }
                else
                {
                    await DisplayAlert("Aviso", "venta  de CREDITO para pagos cambie a CONTADO ", "OK");
                    bresultado = false;
                }
                
            }
            else
            {
                switch (VarEntorno.vCliente.clc_credito)
                {
                    default:
                        dVal = Convert.ToDecimal(lblVenta.Text);

                        if (VarEntorno.cCobranza.dPagosTicket == dVal
                             || (dVal <= 0M && VarEntorno.cCobranza.dPagosTicket == 0M)
                             || (VarEntorno.cTipoVenta == 'P' && 0M == VarEntorno.cCobranza.dPagosTicket)
                             || (VarEntorno.cTipoVenta == 'R' && 1M == dVal)
                             // || (VarEntorno.cTipoVenta == 'R' && Docu.DoctosPendientesR() == 0 && Convert.ToDecimal(lblSaldoAnt.Text) <= 0)
                             || (VarEntorno.cTipoVenta == 'R' && Docu.DoctosPendientesR() == 0)
                             )
                        {
                            bresultado = true;

                            if (VarEntorno.cCobranza.dPagosTicket == Convert.ToDecimal(lblVenta.Text)
                                && (VarEntorno.cTipoVenta == 'R' || VarEntorno.cTipoVenta == 'A')
                                )
                            {
                                VarEntorno.cCobranza.dDetalle.vcf_movimiento = DateTime.Now.Date;
                                VarEntorno.cCobranza.dDetalle.vcn_cliente = VarEntorno.vCliente.cln_clave;
                                VarEntorno.cCobranza.dDetalle.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                                VarEntorno.cCobranza.dDetalle.vcn_monto_pago = VarEntorno.cCobranza.dPagosTicket
                                                                                - dAnticipo;

                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    VarEntorno.cCobranza.dDetalle.vcf_movimiento_cabecera = Docu.Facturas.ListaDoctos[0].vcf_movimiento;// Docu.lDoctosCabecera[0].vcf_movimiento;
                                    VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera = Docu.Facturas.ListaDoctos[0].vcn_folio;//Docu.lDoctosCabecera[0].vcn_folio;                                
                                    VarEntorno.cCobranza.dDetalle.ddn_numero_pago = Docu.N_Pago_Factura(Docu.Facturas.ListaDoctos[0].vcn_folio); ;
                                }
                                else
                                {
                                    VarEntorno.cCobranza.dDetalle.vcf_movimiento_cabecera = DateTime.Now.Date;// Docu.lDoctosCabecera[0].vcf_movimiento;
                                    VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera = VarEntorno.iFolio.ToString().PadLeft(6, '0'); //Docu.lDoctosCabecera[0].vcn_folio;                                
                                    VarEntorno.cCobranza.dDetalle.ddn_numero_pago = 1;
                                }
                            }
                        }
                        else
                        {
                            if (VarEntorno.cCobranza.dPagosTicket > dVal)
                                await DisplayAlert("Aviso", "debe liquidar el total del saldo y venta exactamente", "OK");
                            else
                                await DisplayAlert("Aviso", "debe liquidar el total del saldo y venta ", "OK");
                            bresultado = false;
                        }
                        break;
                    case "S":
                        if (Convert.ToDecimal(VarEntorno.cCobranza.dPagosTicket) == Convert.ToDecimal(lblVenta.Text)
                            // || Convert.ToDecimal(VarEntorno.cCobranza.dPagosTicket) == Convert.ToDecimal(lblSaldoAnt.Text)
                            // || Convert.ToDecimal(VarEntorno.cCobranza.dPagosTicket) == Convert.ToDecimal(lblSaldoTotal.Text)
                            || (Convert.ToDecimal(lblSaldoAnt.Text) <= 0M && 0M == VarEntorno.cCobranza.dPagosTicket)
                            || (Convert.ToDecimal(lblVenta.Text) <= 0M && 0M == VarEntorno.cCobranza.dPagosTicket)
                            || (VarEntorno.cTipoVenta == 'P' && 0M == VarEntorno.cCobranza.dPagosTicket)
                            || (VarEntorno.cTipoVenta == 'R' && 0M == Convert.ToDecimal(lblVenta.Text))
                            || (VarEntorno.cTipoVenta == 'R' && Docu.DoctosPendientesR() == 0 && (VarEntorno.cCobranza.dPagosTicket) == 0M)
                            )
                        {
                            bresultado = true;
                            bool valor = bDoc;

                            if (Convert.ToDecimal(VarEntorno.cCobranza.dPagosTicket) == Convert.ToDecimal(lblVenta.Text)
                                && (VarEntorno.cTipoVenta == 'R' || VarEntorno.cTipoVenta == 'A')
                                && Convert.ToDecimal(VarEntorno.cCobranza.dPagosTicket) > 0M)
                            {
                                VarEntorno.cCobranza.dDetalle.vcf_movimiento = DateTime.Now.Date;
                                VarEntorno.cCobranza.dDetalle.vcn_cliente = VarEntorno.vCliente.cln_clave;
                                VarEntorno.cCobranza.dDetalle.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                                VarEntorno.cCobranza.dDetalle.vcn_monto_pago = VarEntorno.cCobranza.dPagosTicket
                                                                                - dAnticipo;

                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    VarEntorno.cCobranza.dDetalle.vcf_movimiento_cabecera = Docu.Facturas.ListaDoctos[0].vcf_movimiento;// Docu.lDoctosCabecera[0].vcf_movimiento;
                                    VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera = Docu.Facturas.ListaDoctos[0].vcn_folio;//Docu.lDoctosCabecera[0].vcn_folio;                                
                                    VarEntorno.cCobranza.dDetalle.ddn_numero_pago = Docu.N_Pago_Factura(Docu.Facturas.ListaDoctos[0].vcn_folio);
                                }
                                else
                                {
                                    VarEntorno.cCobranza.dDetalle.vcf_movimiento_cabecera = DateTime.Now.Date;// Docu.lDoctosCabecera[0].vcf_movimiento;
                                    VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera = VarEntorno.iFolio.ToString().PadLeft(6, '0'); //Docu.lDoctosCabecera[0].vcn_folio;                                
                                    VarEntorno.cCobranza.dDetalle.ddn_numero_pago = 1;
                                }
                            }
                        }
                        else
                        {
                            if (VarEntorno.cTipoVenta == 'R' && Docu.DoctosPendientesR() == 0 && (VarEntorno.cCobranza.dPagosTicket) > 0M)
                            {
                                await DisplayAlert("Aviso", " Este Documento(s) ya se liquidaron ", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Aviso", " debe liquidar  la venta exactamente", "OK");
                                //await DisplayAlert("Aviso", "debe liquidar el  saldo o la venta exactamente", "OK");
                                //await DisplayAlert("Aviso", "debe liquidar la venta exactamente; para abonar  genere un ticket en cobranza", "OK");
                            }
                            bresultado = false;
                        }
                        break;
                }
            }
            return bresultado;
            
        }
        
        private bool valida_limite_total_venta()
        {
            try
            {
                // Double dLimite = VarEntorno.vCliente.cln_limite_venta;
                decimal dTicketsAnt = 0M;

                if (VarEntorno.vCliente.cln_limite_venta == 0M )                
                    return false;                
                else
                {
                    dTicketsAnt = VarEntorno.cCobranza.fnImporteTotalAntFolio();

                    if (VarEntorno.vCliente.cln_limite_venta < (dTicketsAnt + 700M))                    
                        return true;                    
                    else                    
                        return false;    
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", "al validar el limite " + ex.Message, "ok");
                return true;
            }
        }

        private async Task<bool> valida_pago()
        {
            try
            {
                bool bresultado = false;

                if (VarEntorno.cCobranza.dPagosTicket > 0M)
                {
                    if (Convert.ToDecimal(lblSaldoAnt.Text) <= 0M)
                        bresultado = true;
                    else
                    {                        
                        if (VarEntorno.cCobranza.dPagosTicket <= (Convert.ToDecimal(lblSaldoAnt.Text))  )
                            bresultado = true;
                        else
                        {
                            await DisplayAlert("Aviso", "para abonar mas del saldo genere otro ticket con la diferencia ", "OK");
                            bresultado = false;
                        }                          
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "no pueden realizar pagos con 0", "OK");
                    bresultado = false;
                }

                return bresultado;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.Message, "OK");
                return false;
            }
        }

        /****  termina la venta o pago  y cierra la ventana ***/
        private async void OnClickedTerminar(object sender, EventArgs args)
        {
            Utilerias oUtilerias = new Utilerias();            
            
            btnTerminar.IsEnabled = false;

            if (VarEntorno.bSoloCobrar)
            {
                if (await valida_pago())
                {
                    if (VarEntorno.cCobranza.GuardaCabecera())
                    {
                        oUtilerias.obtenerCoordenadas(1);
                        if (VarEntorno.sTipoImpresora == "Zebra")
                        {
                            ZeImprimePagoARPVM ticketPago = new ZeImprimePagoARPVM();

                            bool bRespuesta;

                            do
                            {
                                bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Pago?", "Si", "No");

                                if (bRespuesta == true)
                                    ticketPago.FtnImprimirTicketPagoARP(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));

                            }
                            while (bRespuesta);
                        }
                        else
                        {
                            #region Imprime el Ticket de Pago las veces que sean necesarias
                            ImprimePagoARPVM ticketPago = new ImprimePagoARPVM();

                            bool bRespuesta;

                            do
                            {
                                bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Pago?", "Si", "No");

                                if (bRespuesta == true)
                                    ticketPago.FtnImprimirTicketPagoARP(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));

                            }
                            while (bRespuesta);
                            #endregion
                        }
                                               
                        VarEntorno.LimpiaVariables();
                        oUtilerias.crearMensaje("¡PAGO EXITOSO!");
                        await this.Navigation.PopModalAsync();
                        
                    }
                    else
                        await DisplayAlert("Aviso", "Error al guardar el Pago.", "OK");
                }
            }
            else
            {
                if (await ValidaVenta())
                {
                    if (VarEntorno.cCobranza.GuardaCabecera())
                    {
                        
                        /*Manda imprimir el Ticket de Entrega si el perfil de REPARTO*/
                        if (VarEntorno.cTipoVenta == 'R')
                        {
                            oUtilerias.obtenerCoordenadas(3);
                            #region Imprime el Ticket de Entrega las veces que sean necesarias
                            if (VarEntorno.sTipoImpresora == "Zebra")
                            {
                                ZeImprimeEntregaRVM ticketEntrega = new ZeImprimeEntregaRVM();

                                bool bRespuesta;
                                var oDialogoCargando = oUtilerias.crearProgressDialog("Imprimiendo", "Espere por favor...");

                                do
                                {
                                    bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Entrega?", "Si", "No");

                                    if (bRespuesta == true)
                                    {
                                        oDialogoCargando.Show();
                                        await Task.Run(() =>
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                ticketEntrega.FtnImprimirTicketEntregaR(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));
                                            });
                                        });
                                        oDialogoCargando.Dismiss();
                                    }
                                }
                                while (bRespuesta);
                            }
                            else
                            {
                                ImprimeEntregaRVM ticketEntrega = new ImprimeEntregaRVM();

                                bool bRespuesta;
                                var oDialogoCargando = oUtilerias.crearProgressDialog("Imprimiendo", "Espere por favor...");

                                do
                                {
                                    bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Entrega?", "Si", "No");

                                    if (bRespuesta == true)
                                    {
                                        oDialogoCargando.Show();
                                        await Task.Run(() =>
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                ticketEntrega.FtnImprimirTicketEntregaR(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));
                                            });
                                        });
                                        oDialogoCargando.Dismiss();
                                    }
                                }
                                while (bRespuesta);
                            }
                            #endregion
                        }
                        /*Manda imprimir el Ticket de Venta si el perfil de AUTOVENTA O PREVENTA*/
                        else
                        {
                            oUtilerias.obtenerCoordenadas(2);
                            #region Imprime el Ticket de Venta las veces que sean necesarias
                            if (VarEntorno.bEsDevolucion)
                            {
                                #region Imprime el Ticket de Devolucion las veces que sean necesarias
                                if (VarEntorno.sTipoImpresora == "Zebra")
                                {
                                    ZeImprimeDevolucionRVM ticketDevolucion = new ZeImprimeDevolucionRVM();

                                    bool bRespuesta;

                                    do
                                    {
                                        bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Devolución?", "Si", "No");

                                        if (bRespuesta == true)
                                        {

                                            ticketDevolucion.FtnImprimirTicketDevolucionR(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'), "");
                                        }
                                    }
                                    while (bRespuesta);
                                }
                                else
                                {
                                    ImprimeDevolucionRVM ticketDevolucion = new ImprimeDevolucionRVM();

                                    bool bRespuesta;

                                    do
                                    {
                                        bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Devolución?", "Si", "No");

                                        if (bRespuesta == true)
                                        {
                                            ticketDevolucion.FtnImprimirTicketDevolucionR(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'), "");
                                        }
                                    }
                                    while (bRespuesta);
                                }
                                #endregion

                            }
                            else
                            {
                                if (VarEntorno.sTipoImpresora == "Zebra")
                                {
                                    ZeImprimeVentaAPVM ticket = new ZeImprimeVentaAPVM();

                                    bool bRespuesta;

                                    do
                                    {
                                        bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Venta?", "Si", "No");

                                        if (bRespuesta == true)
                                        {
                                            ticket.FtnImprimirTicketVentaAP(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));
                                        }
                                    }
                                    while (bRespuesta);

                                }
                                else
                                {
                                    ImprimeVentaAPVM ticket = new ImprimeVentaAPVM();

                                    bool bRespuesta;

                                    do
                                    {
                                        bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Venta?", "Si", "No");

                                        if (bRespuesta == true)
                                        {
                                            ticket.FtnImprimirTicketVentaAP(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'));
                                        }
                                    }
                                    while (bRespuesta);

                                }                                
                            }
                            #endregion
                        }
                        if (VarEntorno.cTipoVenta!='R')
                            await DisplayAlert("Aviso", VarEntorno.cCobranza.LogroDesafio(), "OK");

                        if (VarEntorno.cTipoVenta == 'P')
                        {
                            #region Envase Sugerido
                            //Validación para conocer si el cliente ya tiene una captura de envase a recoger
                            //if (new vmCapturaEnvase().existeCapturaEnvaseSugerido(VarEntorno.vCliente.cln_clave.ToString()))
                            //{
                            //    VarEntorno.LimpiaVariables();
                            //    oUtilerias.crearMensaje("¡VENTA EXITOSA!");
                            //    await this.Navigation.PopModalAsync();
                            //}
                            //else
                            //{
                                this.Navigation.PopModalAsync();
                                await this.Navigation.PushModalAsync(new frmEnvaseSugerido());
                            //}
                            #endregion Envase Sugerido
                        }
                        else
                        {
                            VarEntorno.LimpiaVariables();
                            oUtilerias.crearMensaje("¡VENTA EXITOSA!");
                            await this.Navigation.PopModalAsync();
                        }
                    }
                    else
                        await DisplayAlert("Aviso", "Error al guardar la Venta. " + VarEntorno.sMensajeError, "OK");
                }
                ///fin cuadro
            }
            btnTerminar.IsEnabled = true;
        }
        
        private void OnClickedEfectivo(object sender, EventArgs args)
        {
            
            if (VarEntorno.cCobranza.NumeroTickets()> 1 
                && VarEntorno.cTipoVenta == 'R' 
                && VarEntorno.bSoloCobrar==false
                && Convert.ToDecimal(lblSaldoTotal.Text)>2M)
                DisplayAlert("Aviso", " venta de multiples ticket para pagar ingrese a cobranza documentos ", "OK");
            else
            
                this.Navigation.PushModalAsync(new frmCobranzaEfectivo());
        }

        private async void OnClickedTarjeta(object sender, EventArgs args)
        {
            bool bResult = false;
            iFormaPago = 3;

            if (VarEntorno.cCobranza.NumeroTickets() > 1
                && VarEntorno.cTipoVenta == 'R'
                && VarEntorno.bSoloCobrar == false)
                await DisplayAlert("Aviso", " venta de multiples ticket para pagar ingrese a cobranza documentos ", "OK");
            else
            {
                if (VarEntorno.cCobranza.lpTarjeta.Count > 0)
                {
                    bResult = await DisplayAlert("Aviso", "Desea generar un nueva tarjeta o actualizar  ", "Nuevo", "Actualizar");

                    if (bResult)
                        await this.Navigation.PushModalAsync(new frmCobranzaTarjeta());
                    else
                    {
                        LlenaFormasPagos(iFormaPago);
                    }
                }
                else
                    await Navigation.PushModalAsync(new frmCobranzaTarjeta());
            }
        }

        private async void OnClickedTransferencia(object sender, EventArgs args)
        {
            bool bResult = false;
            iFormaPago = 2;

            if (VarEntorno.cCobranza.NumeroTickets() > 1
                && VarEntorno.cTipoVenta == 'R'
                && VarEntorno.bSoloCobrar == false)
                await DisplayAlert("Aviso", " venta de multiples ticket para pagar ingrese a cobranza documentos ", "OK");
            else
            {
                if (VarEntorno.cCobranza.lpTransferencia.Count > 0)
                {
                        bResult = await  DisplayAlert("Aviso", "Desea generar un nueva transferencia o actualizar  ", "Nuevo", "Actualizar");

                    if (bResult)
                        await Navigation.PushModalAsync(new frmCobranzaTransferencia());
                    else
                    {
                        LlenaFormasPagos(iFormaPago);
                    }
                }
                else
                     await Navigation.PushModalAsync(new frmCobranzaTransferencia());
            }
                
        }

        private void OnClickedBonificacion(object sender, EventArgs args)
        {
            if (VarEntorno.cCobranza.NumeroTickets() > 1
                    && VarEntorno.cTipoVenta == 'R'
                    && VarEntorno.bSoloCobrar == false)
                DisplayAlert("Aviso", " venta de multiples ticket para pagar ingrese a cobranza documentos ", "OK");
            else
                this.Navigation.PushModalAsync(new frmCobranzaBonificacion());
        }

        private void OnClickedAnticipos(object sender, EventArgs args)
        {
            if (VarEntorno.cCobranza.NumeroTickets() > 1
                    && VarEntorno.cTipoVenta == 'R'
                    && VarEntorno.bSoloCobrar == false)
                DisplayAlert("Aviso", " venta de multiples ticket para pagar ingrese a cobranza documentos ", "OK");
            else
                this.Navigation.PushModalAsync(new frmRelacionAnticipo(2));
        }

        private async void OnClickedCheque(object sender, EventArgs args)
        {
            bool bResult = false;
            iFormaPago = 1;
            if (VarEntorno.vCliente.cln_cheque)
            {
                if (VarEntorno.cCobranza.NumeroTickets() > 1
                    && VarEntorno.cTipoVenta == 'R'
                    && VarEntorno.bSoloCobrar == false)
                    await DisplayAlert("Aviso", " venta de multiples ticket para pagar ingrese a cobranza documentos ", "OK");
                else
                {
                    if (VarEntorno.cCobranza.lpCheques.Count > 0)
                    {
                        bResult = await DisplayAlert("Aviso", "Desea generar un nuevo cheque o actualizar  ", "Nuevo", "Actualizar");

                        if (bResult)
                            await Navigation.PushModalAsync(new frmCobranzaCheque());
                        else
                        {
                            LlenaFormasPagos(iFormaPago);
                        }
                    }
                    else
                        await Navigation.PushModalAsync(new frmCobranzaCheque());
                }
            }
            else
               await DisplayAlert("Aviso", "Cliente sin derecho a cheque ATTE: credito y cobranza", "OK");
        }

        private void OnClickedRegresar(object sender, EventArgs args)
        {
            if (VarEntorno.bSoloCobrar)
            {
                VarEntorno.LimpiaVariables();
                this.Navigation.PopModalAsync();
            }
            else
            {
                this.Navigation.PopModalAsync();
                switch (VarEntorno.cTipoVenta)
                {
                    case 'R':
                        VarEntorno.cCobranza = null;
                        break;
                    default:
                        if (VarEntorno.bEsDevolucion)
                            this.Navigation.PushModalAsync(new frmDevolucionAutoventa());
                        else
                            this.Navigation.PushModalAsync(new FrmVentas());
                        break;
                }

            }
        }

        private void LlenaFormasPagos(int i)
        {
            try
            {
                iEspera = true;
                pckPagos.Items.Clear();
                switch (i)
                {
                    case 1:
                        foreach (var pago in VarEntorno.cCobranza.lpCheques)
                            pckPagos.Items.Add("Cheque de $" + String.Format("{0:N2}", pago.vcn_monto));
                        break;
                    case 2:
                        foreach (var pago in VarEntorno.cCobranza.lpTransferencia)
                            pckPagos.Items.Add("Transferencia de $" + String.Format("{0:N2}", pago.vcn_monto));
                        break;
                    case 3:
                        foreach (var pago in VarEntorno.cCobranza.lpTarjeta)
                            pckPagos.Items.Add("Tarjeta de $" + String.Format("{0:N2}", pago.vcn_monto));
                        break;
                }
                iEspera = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (pckPagos.IsFocused)
                        pckPagos.Unfocus();

                    pckPagos.Focus();
                });
            }
            catch
            {
            }
        }

        private void SelectedIndexChanged(object sender, EventArgs args)
        {
            try
            {
                if (iEspera)
                {

                }
                else
                {
                    switch (iFormaPago)
                    {
                        case 1:
                            Navigation.PushModalAsync(new frmCobranzaCheque(pckPagos.SelectedIndex));
                            break;
                        case 2:
                            Navigation.PushModalAsync(new frmCobranzaTransferencia(pckPagos.SelectedIndex));
                            break;
                        case 3:
                            Navigation.PushModalAsync(new frmCobranzaTarjeta(pckPagos.SelectedIndex));
                            break;
                    }
                }
            }
            catch
            { }
        }
    }
}
