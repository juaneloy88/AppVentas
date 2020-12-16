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
	public partial class frmCobranzaCheque : ContentPage
    {
        //buscarCliente buscacliente = new buscarCliente();
        int iIndex = -1;

        public frmCobranzaCheque()
        {
            InitializeComponent();
            validaDerechoACheque();
            //lblTitulo.Text = "CHEQUE " + (VarEntorno.cCobranza.lCheque.Count + VarEntorno.cCobranza.lTarjeta.Count + 1).ToString();
            lblTitulo.Text = "CHEQUE " + (VarEntorno.cCobranza.lpCheques.Count + 1).ToString();
        }

        public frmCobranzaCheque(int iNumber)
        {
            InitializeComponent();
            validaDerechoACheque();
            iIndex = iNumber;
            LlenaDatosCheque();
            lblTitulo.Text = "CHEQUE " + (iIndex+1).ToString();
        }

        #region Válida si el cliente tiene derecho a pago mediante cheque
        public async void validaDerechoACheque()
        {
            int nCliente = VarEntorno.vCliente.cln_clave;

            //var vListaCliente = await buscacliente.DatosCliente(nCliente.ToString());

            if (VarEntorno.vCliente.cln_cheque)
            {
                pckBancos.Items.Add("Banamex");
                pckBancos.Items.Add("Bancomer");
                pckBancos.Items.Add("Banorte");
                pckBancos.Items.Add("Santander");
                pckBancos.Items.Add("Banregio");
                pckBancos.Items.Add("Banbajio");
                pckBancos.Items.Add("Inbursa");
                pckBancos.Items.Add("Scotia");
                pckBancos.Items.Add("HSBC");
                pckBancos.Items.Add("Banco multiva");
                pckBancos.Items.Add("BancaAfirme");
            }
            else
            {
                await DisplayAlert("Alert", "El Cliente no tiene derecho a PAGO por medio de CHEQUE", "OK");
                await this.Navigation.PopModalAsync();
            }
        }
        #endregion Válida si el cliente tiene derecho a pago mediante cheque

        #region Botón donde guarda la información en la clase de Cobranza.Cheque
        private async void GuardaCheque()
        {
            try
            {
                int iBancos = pckBancos.SelectedIndex;

                if (iBancos < 0)
                {
                    await DisplayAlert("Alert", "Seleccione un Banco", "OK");
                }
                else
                {
                    string sBanco = pckBancos.SelectedItem.ToString();
                    string sNoCheque = txtNoCheque.Text;
                    string sMonto = txtMonto.Text;
                    string sCuenta = txtNoCuenta.Text;
                    bool bResult = false;

                    if (string.IsNullOrWhiteSpace(sNoCheque))
                    {
                        await DisplayAlert("Alert", "Capture un Número de Cheque", "OK");
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(sMonto) || Convert.ToDecimal(sMonto) <= 0M)
                        {
                            await DisplayAlert("Alert", "Capture un Monto Válido", "OK");
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(sCuenta))
                            {
                                await DisplayAlert("Alert", "Capture una Cuenta", "OK");
                            }
                            else
                            {                                
                                if (iIndex == -1)
                                {   
                                            bResult = VarEntorno.cCobranza.GuardaActualizaPago("CHEQUE", true, -1, "02", pckBancos.SelectedItem.ToString(), ""
                                                                                    , Convert.ToDecimal(txtMonto.Text)
                                                                                    , txtNoCheque.Text, txtNoCuenta.Text, "");
                                }
                                else
                                {                                    
                                    bResult = VarEntorno.cCobranza.GuardaActualizaPago("CHEQUE", false, iIndex, "02", pckBancos.SelectedItem.ToString(), ""
                                                                            , Convert.ToDecimal(txtMonto.Text), txtNoCheque.Text, txtNoCuenta.Text, "");
                                                                        
                                }

                                if (VarEntorno.bEsDocumentos)
                                    VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
                                else
                                    VarEntorno.cCobranza.vCobranza.CargaPagos();

                                if (bResult)
                                {
                                    //await DisplayAlert("Aviso", "Captura de Cheque Guardado CORRECTAMENTE", "Ok");
                                    await this.Navigation.PopModalAsync();
                                }
                                else
                                    await DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.Message, "Ok");
            }

        }

        public async void onClikedAceptar(object sender, EventArgs args)
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
                    VarEntorno.cCobranza.lpCheques.RemoveAt(iIndex);////BORRA CHEQUE
                    if (VarEntorno.bEsDocumentos)
                        VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
                    else
                        VarEntorno.cCobranza.vCobranza.CargaPagos();

                    this.Navigation.PopModalAsync();
                }
            }
            else
            {
                GuardaCheque();///GUARDA  o ACTUALIZA
            }
        }
        #endregion Botón donde guarda la información en la clase de Cobranza.Cheque

        private void LlenaDatosCheque()
        {
            try
            {
                var VP = VarEntorno.cCobranza.lpCheques[iIndex];  

                pckBancos.SelectedIndex = pckBancos.Items.IndexOf(VP.vcc_banco); 
                txtNoCheque.Text = VP.vcc_referencia;
                txtMonto.Text= VP.vcn_monto.ToString();
                txtNoCuenta.Text= VP.vpc_nocuenta;
            }
            catch
            {
                DisplayAlert("Alert", "NO SE PUDO CARGAR EL CHEQUE", "OK");
            }
        }

        #region Botón de Regresar
        public void onClikedRegresar(object sender, EventArgs args)
        {
            Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar
    }
}
