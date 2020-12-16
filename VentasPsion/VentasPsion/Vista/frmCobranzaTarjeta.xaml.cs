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
	public partial class frmCobranzaTarjeta : ContentPage
    {
        int iIndex = -1;

        public frmCobranzaTarjeta ()
		{
			InitializeComponent ();            

            lblFecha.Text = VarEntorno.dFechaVenta.ToShortDateString();
            //lblTitulo.Text = "TARJETA " + (VarEntorno.cCobranza.lCheque.Count + VarEntorno.cCobranza.lTarjeta.Count + 1).ToString();
            lblTitulo.Text = "TARJETA " + (VarEntorno.cCobranza.lpTarjeta.Count + 1).ToString();
            pckTipoTarjeta.Items.Add("DEBITO");
            pckTipoTarjeta.Items.Add("CREDITO");           

        }

        public frmCobranzaTarjeta(int iNumero)
        {
            InitializeComponent();

            lblFecha.Text = VarEntorno.dFechaVenta.ToShortDateString();
            //lblTitulo.Text = "TARJETA " + (VarEntorno.cCobranza.lCheque.Count + VarEntorno.cCobranza.lTarjeta.Count + 1).ToString();
            lblTitulo.Text = "TARJETA " + (iNumero + 1).ToString();
            pckTipoTarjeta.Items.Add("DEBITO");
            pckTipoTarjeta.Items.Add("CREDITO");
            iIndex = iNumero;
            LlenaDatosTarjeta();

        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            //  manda la pantalla al fondo de la pila
            this.Navigation.PopModalAsync();
        }

        private bool valida_campos()
        {
            try
            {
                string sRespuesta = "";
                if (txtMonto.Text != sRespuesta)
                {
                    if (txtComision.Text != sRespuesta && Convert.ToDecimal(txtComision.Text) >= 0M)
                    {
                        if (txtImporte.Text != sRespuesta && Convert.ToDecimal (txtImporte.Text)>0M)
                        {
                            if (txtAutorizacion.Text!= sRespuesta)
                            {
                                if (txtCuenta.Text != sRespuesta && txtCuenta.Text.Length<17 )
                                {
                                    if (pckTipoTarjeta.SelectedIndex != -1)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        DisplayAlert("Aviso", "seleccione un tipo de tarjeta", "Ok");
                                        return false;
                                    }
                                }
                                else
                                {
                                    DisplayAlert("Aviso", "ingrese una cuenta o la cuenta es demasiado grande", "Ok");
                                    return false;
                                }
                            }
                            else
                            {
                                DisplayAlert("Aviso", "ingrese una autorizacion ", "Ok");
                                return false;
                            }
                        }
                        else
                        {
                            DisplayAlert("Aviso", "ingrese una importe permitido", "Ok");
                            return false;
                        }
                    }
                    else
                    {
                        DisplayAlert("Aviso", "ingrese una comision ", "Ok");
                        return false;
                    }
                }
                else
                {
                    DisplayAlert("Aviso", "ingrese una monto ", "Ok");
                    return false;
                }
            }
            catch (Exception ex)
            {
                 DisplayAlert("Aviso", "Error: " + ex.ToString(), "Ok");
                return false;
            }
    
        }

        private async void GuardaTarjeta()
        {
            try
            {
                bool bResult = false;
                if (valida_campos())
                {  
                    if (iIndex == -1)
                    {                                
                        bResult=VarEntorno.cCobranza.GuardaActualizaPago("TARJETA", true, -1, pckTipoTarjeta.SelectedIndex == 0 ? "28" : "04", ""
                                                                , pckTipoTarjeta.SelectedItem.ToString(), Convert.ToDecimal(txtMonto.Text)
                                                                , "", txtCuenta.Text, txtAutorizacion.Text);
                    }
                    else
                    {                                
                        bResult=VarEntorno.cCobranza.GuardaActualizaPago("TARJETA", false, iIndex, pckTipoTarjeta.SelectedIndex == 0 ? "28" : "04", ""
                                                                , pckTipoTarjeta.SelectedItem.ToString(), Convert.ToDecimal(txtMonto.Text)
                                                                , "", txtCuenta.Text, txtAutorizacion.Text);                                
                    }

                    //VarEntorno.cCobranza.lTarjeta.Add(cTarjeta);
                    if (VarEntorno.bEsDocumentos)
                        VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
                    else
                        VarEntorno.cCobranza.vCobranza.CargaPagos();

                    //  manda la pantalla al fondo de la pila
                    if (bResult)
                        await this.Navigation.PopModalAsync();
                    else
                        DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", "Error: " + ex.ToString(), "Ok");
            }
        }

        public async void OnClickedAceptar(object sender, EventArgs args)
        {
            string sMonto = txtMonto.Text;

            if (string.IsNullOrWhiteSpace(sMonto) || Convert.ToDecimal(sMonto) <= 0M)
            {
                if (iIndex == -1)
                {
                    await DisplayAlert("Alert", "Capture un Monto Válido", "OK");///Nada
                }
                else
                {
                    VarEntorno.cCobranza.lpTarjeta.RemoveAt(iIndex);////BORRA 

                    if (VarEntorno.bEsDocumentos)
                        VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
                    else
                        VarEntorno.cCobranza.vCobranza.CargaPagos();

                    this.Navigation.PopModalAsync();
                }
            }
            else
            {
                GuardaTarjeta();///GUARDA  o ACTUALIZA
            }
        }

        private void LlenaDatosTarjeta()
        {
            try
            {
                var VP = VarEntorno.cCobranza.lpTarjeta[iIndex];

                txtMonto.Text = VP.vcn_monto.ToString();
                txtImporte.Text = VP.vcn_monto.ToString();
                txtCuenta.Text = VP.vpc_nocuenta;
                txtAutorizacion.Text = VP.vpc_autorizacion;
                pckTipoTarjeta.SelectedIndex = VP.cfpc_formapago == "28" ? 0 : 1;

            }
            catch
            {
                DisplayAlert("Alert", "NO SE PUDO CARGAR LA TARJETA", "OK");
            }
        }

        private void txtComision_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularMonto();
        }

        private void txtImporte_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcularMonto();
        }

        private void CalcularMonto()
        {
            try
            {
                decimal dComision = 0, dImporte = 0;

                if (txtComision.Text != "")
                    dComision = Convert.ToDecimal(txtComision.Text);

                if (txtImporte.Text != "")
                    dImporte = Convert.ToDecimal(txtImporte.Text);

                txtMonto.Text = String.Format("{0:0.00}", dImporte + dComision);
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", ex.Message, "OK");
            }
        }
    }
}
