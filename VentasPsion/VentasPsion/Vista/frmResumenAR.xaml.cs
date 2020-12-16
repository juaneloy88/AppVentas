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
	public partial class frmResumenAR : ContentPage
	{
        vmCapturaEnvase vmcapturaEnvase = new vmCapturaEnvase();
        resumenDia resdia = new resumenDia();

		public frmResumenAR ()
		{
			InitializeComponent ();
            txtRuta.Text = "Ruta:" + VarEntorno.iNoRuta.ToString();
            cargaPagos();
            cargaEnvase();

            #region Oculta el botón de Avanzar dependiendo el perfil
            if (VarEntorno.cTipoVenta == 'A')
            { 
                btnAvanzar.IsVisible = true;
                btnImprimir.IsVisible = false;
            }
            else
            {
                btnAvanzar.IsVisible = false;
                btnImprimir.IsVisible = true;
            }
            #endregion Oculta el botón de Avanzar dependiendo el perfil
        }

        #region Método que obtiene el resumen de pagos
        public void cargaPagos()
        {
            resumenDia resDia = new resumenDia();
            double dEfectivo = resDia.vcn_monto_efe();
            double dCheques = resdia.vcn_cheque();
            double dTarjeta = resdia.vcn_tarjeta();
            double dTransferencia = resdia.vcn_transferencia();
            double dtotal = dEfectivo + dCheques + dTarjeta;
            txtEfectivo.Text = String.Format("{0:C2}", dEfectivo);
            txtCheque.Text = String.Format("{0:C2}", dCheques);
            txtTarjeta.Text = String.Format("{0:C2}", dTarjeta);
            txtTransferencia.Text = String.Format("{0:C2}", dTransferencia);
            txtTotal.Text = String.Format("{0:C2}", dtotal);
        }
        #endregion Método que obtiene el resumen de pagos

        #region Método que obtiene el resumen de envase
        public async void cargaEnvase()
        {
            var vCargaEnvase = await vmcapturaEnvase.detEnvaseRuta();
            lsvEnvase.ItemsSource = vCargaEnvase;
        }
        #endregion Método que obtiene el resumen de envase

        #region Botón de Regresar
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar

        #region Botón de Avanzar
        public void OnClickedAvanzar(object sender, EventArgs args)
        {
            this.Navigation.PushModalAsync(new frmResumenDia());                    
        }
        #endregion Botón de Avanzar

        /*Método que imprime el Ticket de Resumen de Reparto de una Ruta*/
        private async void btnImprimir_Clicked(object sender, EventArgs e)
        {
            if (VarEntorno.sTipoImpresora == "Zebra")
            {
                ZeImprimeResumenRVM ticketResumenReparto = new ZeImprimeResumenRVM();

                bool bRespuesta;

                bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Resumen de Reparto?", "Si", "No");

                if (bRespuesta == true)
                {
                    ticketResumenReparto.FtnImprimirResumenR(VarEntorno.iNoRuta);
                }
            }
            else
            { 
                ImprimeResumenRVM ticketResumenReparto = new ImprimeResumenRVM();

                bool bRespuesta;

                bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Resumen de Reparto?", "Si", "No");

                if (bRespuesta == true)
                {
                    ticketResumenReparto.FtnImprimirResumenR(VarEntorno.iNoRuta);
                }
            }
        }
    }
}