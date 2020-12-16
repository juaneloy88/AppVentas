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
	public partial class frmCobranzaTransferencia : ContentPage
	{
        int iIndex = -1;

        public frmCobranzaTransferencia ()
		{
			InitializeComponent ();
            lblTitulo.Text = "TRANSFERENCIA " + (VarEntorno.cCobranza.lpTransferencia.Count + 1).ToString();
        }

        public frmCobranzaTransferencia(int iNumero)
        {
            InitializeComponent();
            iIndex = iNumero;
            LlenaDatosTransferencia();
            lblTitulo.Text = "TRANSFERENCIA " + (iNumero + 1).ToString();
        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            //  manda la pantalla al fondo de la pila
            this.Navigation.PopModalAsync();
        }

        public async void OnClickedAceptar(object sender, EventArgs args)
        {
            try
            {
                string sMonto = txtCantidad.Text;

                if (string.IsNullOrWhiteSpace(sMonto) || Convert.ToDecimal(sMonto) <= 0M)
                {
                    if (iIndex == -1)
                    {
                        await DisplayAlert("Alert", "Capture un Monto Válido", "OK");///Nada
                    }
                    else
                    {
                        VarEntorno.cCobranza.lpTransferencia.RemoveAt(iIndex);////BORRA 
                        if (VarEntorno.bEsDocumentos)
                            VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
                        else
                            VarEntorno.cCobranza.vCobranza.CargaPagos();

                        this.Navigation.PopModalAsync();
                        
                    }
                }
                else
                {
                    GuardaTransferencia();///GUARDA  o ACTUALIZA
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.Message, "OK");
            }
        }


        private void GuardaTransferencia()
        {
            try
            {                
                if (Convert.ToDecimal(txtCantidad.Text) < 0M)
                {
                    DisplayAlert("Aviso", "No se pueden capturar cantidades negativas", "Ok");
                }
                else
                {      
                    string sRespuesta = string.Empty;
                    if (sRespuesta != txtCantidad.Text || sRespuesta != txtCuenta.Text)
                    {
                        bool bResult = false;
                                
                        if (iIndex == -1)
                        {                                    
                            bResult= VarEntorno.cCobranza.GuardaActualizaPago("TRANSFERENCIA", true, -1, "03", "", "", Convert.ToDecimal(txtCantidad.Text)
                                                                    , "", txtCuenta.Text, "");                                    
                        }
                        else
                        {                                    
                            bResult= VarEntorno.cCobranza.GuardaActualizaPago("TRANSFERENCIA", false, iIndex, "03", "", "", Convert.ToDecimal(txtCantidad.Text)
                                                                    , "", txtCuenta.Text, "");
                        }

                        if (VarEntorno.bEsDocumentos)
                            VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
                        else
                            VarEntorno.cCobranza.vCobranza.CargaPagos();

                        //  manda la pantalla al fondo de la pila
                        if (bResult)
                            this.Navigation.PopModalAsync();
                        else
                            DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
                    }
                    else
                        DisplayAlert("Aviso", "Favor de ingresar una cantidad y/o cuenta", "Ok");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", "Error: " + ex.ToString(), "Ok");
            }
        }

        private void LlenaDatosTransferencia()
        {
            try
            {
                var VP = VarEntorno.cCobranza.lpTransferencia[iIndex];

                txtCantidad.Text = VP.vcn_monto.ToString();
                txtCuenta.Text = VP.vpc_nocuenta;
            }
            catch
            {
                DisplayAlert("Alert", "NO SE PUDO CARGAR LA TRANSFERENCIA", "OK");
            }
        }
    }
}
