using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmCobranzaPagos : ContentPage
	{
        DocumentosVM Doctos = new DocumentosVM();
        bool bAnticipos = false;
        int iFormaPago = 0;
        bool iEspera = true;

        public frmCobranzaPagos ()
		{
			InitializeComponent ();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            if (VarEntorno.cCobranza is null)
                VarEntorno.cCobranza = new CobranzaVM();

            VarEntorno.cCobranza.vCobranzaPagos = this;
            CargaDatosGenerales();

            CargaDocumentos();

        }

        /*****carga Documentos del cliente *****/
        private void CargaDocumentos()
        {            
            if (Doctos.lDoctos())
            {   
                foreach (var Documentos in Doctos.lDoctosCabecera)
                {
                    /*
                            pckDocumentos.Items.Add(
                                i++.ToString().PadLeft(2, '0') + " " +
                                String.Format("{0:N2}", Documentos.vcn_importe).PadLeft(9, ' ') + "  "+
                                String.Format("{0:N2}", Documentos.dcn_saldo).PadLeft(9, ' ') + "  " +
                                Documentos.vcf_movimiento.ToString("dd/MM/yyyy") 
                            );
                            */
                    pckDocumentos.Items.Add(
                        Documentos.vcn_folio.ToString() + "   " +
                        Documentos.vcf_movimiento.ToString("dd/MM/yyyy") + "  " +
                        String.Format("{0:N2}", Documentos.vcn_importe).PadLeft(9, ' ') + "  " +
                        String.Format("{0:N2}", Documentos.dcn_saldo).PadLeft(9, ' ')                         
                    );
                }
            }
            else
                DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
        }

        private void CargaDatosGenerales()
        {
            VarEntorno.iFolio = VarEntorno.TraeFolio();
            VarEntorno.dImporteTotal = 0;           

            //carga de informacion en pantalla
            lblSaldoAnt.Text = String.Format("{0:N2}", VarEntorno.Saldo(VarEntorno.vCliente));            
            //lblClave.Text = VarEntorno.vCliente.cln_clave.ToString();
            
            CargaPagos();

        }

        public void CargaPagos()
        {
            ///inicializa varibles para ls pagos 
            decimal dPTarjeta = 0;
            decimal dPBonificacion = 0;
            decimal dPCheque = 0;
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


            lblTajeta.Text = String.Format("{0:N2}", dPTarjeta);
            lblBonificacion.Text = String.Format("{0:N2}", dPBonificacion);
            lblCheque.Text = String.Format("{0:N2}", dPCheque);
            lblEfectivo.Text = String.Format("{0:N2}", VarEntorno.cCobranza.pEfectivo.vcn_monto//VarEntorno.cCobranza.dEfectivo
                               // + VarEntorno.cCobranza.cTransferencia.dCantidad
                               + dPTransferencia
                                );

            VarEntorno.cCobranza.dPagosTicket = VarEntorno.cCobranza.pEfectivo.vcn_monto//VarEntorno.cCobranza.dEfectivo
                                    + dPTarjeta
                                    + dPCheque
                                    //+ VarEntorno.cCobranza.cTransferencia.dCantidad
                                    + dPTransferencia
                                    ;
            
             VarEntorno.cCobranza.dPagosTicket += dPBonificacion;

            lblSaldoNuevo.Text = String.Format("{0:N2}", Convert.ToDecimal(lblSaldoAnt.Text)
                                             - VarEntorno.cCobranza.dPagosTicket);
            lblSaldoNuevoDocto.Text = String.Format("{0:N2}", Convert.ToDecimal(lblSaldoDocto.Text)
                                             - VarEntorno.cCobranza.dPagosTicket);
            
        }

        private void pckDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pckDocumentos.SelectedIndex != -1)
            {
                var vDocumentos = Doctos.lDoctosCabecera[pckDocumentos.SelectedIndex];
                lblImporteDocto.Text = String.Format("{0:N2}", vDocumentos.vcn_importe);
                lblSaldoDocto.Text = String.Format("{0:N2}", vDocumentos.dcn_saldo);
                lblSaldoNuevoDocto.Text = String.Format("{0:N2}", vDocumentos.dcn_saldo);

                VarEntorno.dEfectivo = vDocumentos.dcn_saldo;

                VarEntorno.cCobranza.dDetalle.vcf_movimiento = DateTime.Now.Date;
                VarEntorno.cCobranza.dDetalle.vcf_movimiento_cabecera = vDocumentos.vcf_movimiento;
                VarEntorno.cCobranza.dDetalle.vcn_cliente = vDocumentos.vcn_cliente;
                VarEntorno.cCobranza.dDetalle.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                VarEntorno.cCobranza.dDetalle.vcn_folio_cabecera = vDocumentos.vcn_folio;
                VarEntorno.cCobranza.dDetalle.ddn_numero_pago = vDocumentos.dcn_numero_pago;
            }
            else
            {
                lblImporteDocto.Text = "0.00";
                lblSaldoDocto.Text = "0.00";
            }
            CargaPagos();
        }

        private async Task<bool> valida_pago()
        {
            try
            {
                bool bresultado = false;
                bool brespuesta = false;
                if (VarEntorno.cCobranza.dPagosTicket >= 0.01M)
                {
                    if (Doctos.lDoctosCabecera.Count > 0)
                    {
                        if (pckDocumentos.SelectedIndex != -1)
                        {
                            if (VarEntorno.cCobranza.dPagosTicket <= Convert.ToDecimal(lblSaldoDocto.Text)
                               // ||(VarEntorno.cCobranza.dPagosTicket <= Convert.ToDecimal(lblSaldoDocto.Text)+1M && VarEntorno.cCobranza.lBonificacion.Count > 0 )
                                )
                            {
                                bresultado = true;
                                VarEntorno.cCobranza.dDetalle.vcn_monto_pago = VarEntorno.cCobranza.dPagosTicket;
                            }
                            else
                            {
                                await DisplayAlert("Aviso", "para abonar mas del saldo de la Documento genere otro ticket con la diferencia ", "OK");
                                bresultado = false;
                            }
                        }
                        else
                        {
                            await DisplayAlert("Aviso", "seleccione una Documento", "OK");
                            bresultado = false;
                        }
                    }
                    else
                    {
                        if (VarEntorno.cCobranza.NumeroPagos() > 1)
                        {
                            await DisplayAlert("Aviso", "Solo se Permite una forma de pago por anticipo ", "OK");
                        }
                        else
                        {
                            brespuesta = await DisplayAlert("Pregunta", "¿Desea generar un anticipo?", "Si", "NO");
                            if (brespuesta)
                            {
                                //if (VarEntorno.cCobranza.lPagos.Where(p => p.vpc_descripcion.Contains("BON")).ToList().Count > 0)
                                if (VarEntorno.cCobranza.lpBonificacion.Count>0)
                                {
                                    await DisplayAlert("Aviso", "Las bonificaciones no pueden ser anticipos", "OK");
                                }
                                else
                                {
                                    Doctos.anticipo.cln_clave = VarEntorno.vCliente.cln_clave;
                                    Doctos.anticipo.vcn_folio = VarEntorno.TraeFolio().ToString().PadLeft(6, '0');
                                    Doctos.anticipo.vcf_movimiento = DateTime.Now.Date;
                                    Doctos.anticipo.ann_monto_pago = VarEntorno.cCobranza.dPagosTicket;
                                    Doctos.anticipo.ann_cantidad_pagos = 1;
                                    Doctos.anticipo.anc_forma_pago = VarEntorno.cCobranza.FormaPago();
                                    Doctos.anticipo.anb_nuevo = true;
                                    Doctos.anticipo.anb_relacionado = false;
                                    bresultado = true;
                                    bAnticipos = true;
                                }
                            }
                            else
                                bresultado =  false;
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
                await DisplayAlert("Aviso", "error al validar pago "+ex.Message, "OK");
                return false;
            }
        }
        
        private async void OnClickedTerminar(object sender, EventArgs args)
        {
            Utilerias oUtilerias = new Utilerias();

            btnTerminar.IsEnabled = false;


            bool r = false;
            if (await valida_pago())
            {
                /*
                if (Doctos.lDoctosCabecera.Count==0)
                {
                    bAnticipos = true;
                }
                */
                if (VarEntorno.cCobranza.GuardaCabecera())
                {
                    if (bAnticipos)
                    {
                        r = Doctos.Guarda_Anticipo();
                    }
                    else
                        r = true;

                    if (r)                    
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
                    {
                        await DisplayAlert("Aviso", "Error al guardar anticipo", "OK");
                    }
                }
                else
                    await DisplayAlert("Aviso", "Error al guardar el Pago.", "OK");                
            }
            btnTerminar.IsEnabled = true;
        }
        
        private void OnClickedEfectivo(object sender, EventArgs args)
        {
            this.Navigation.PushModalAsync(new frmCobranzaEfectivo());
        }

        private async void OnClickedTarjeta(object sender, EventArgs args)
        {
            bool bResult = false;
            iFormaPago = 3;

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

        private async void OnClickedTransferencia(object sender, EventArgs args)
        {
            bool bResult = false;
            iFormaPago = 2;

            if (VarEntorno.cCobranza.lpTransferencia.Count > 0)
            {
                 bResult = await DisplayAlert("Aviso", "Desea generar un nueva transferencia o actualizar  ", "Nuevo", "Actualizar");

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

        private void OnClickedBonificacion(object sender, EventArgs args)
        {
            this.Navigation.PushModalAsync(new frmCobranzaBonificacion());
        }

        private async void OnClickedCheque(object sender, EventArgs args)
        {
            bool bResult = false;
            iFormaPago = 1;
            if (VarEntorno.vCliente.cln_cheque)
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
            else
                await DisplayAlert("Aviso", "Cliente sin derecho a cheque ATTE:credito y cobranza", "OK");
        }

        private void OnClickedRegresar(object sender, EventArgs args)
        {
            VarEntorno.LimpiaVariables();
            VarEntorno.cCobranza = null;
            this.Navigation.PopModalAsync();
        }

        private async void LlenaFormasPagos(int i)
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
                await DisplayAlert("Aviso", "verifica", "OK");
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
