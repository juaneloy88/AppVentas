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
    public partial class frmResumenAbonoEnvase : ContentPage
    {
        int iIdCliente = 0;

        /*Método constructor de la clase*/
        public frmResumenAbonoEnvase(int iIdClienteParam, string sNombreCliente)
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            lblCliente.Text = iIdClienteParam.ToString().Trim() + "  -  " + sNombreCliente.ToString().Trim();

            iIdCliente = iIdClienteParam;

            FtnCargarEnvasesDeCliente(iIdClienteParam);
        }

        /*Método para conectarse a la vista-modelo que regresa el Resumen de Abono de Envase de un Cliente específico*/
        public async void FtnCargarEnvasesDeCliente(int iIdClienteParam)
        {
            EnvaseVM listaEnvase = new EnvaseVM();

            // Manda llamar a la función que regresa el Resumen de Abono de Envase del Cliente enviado como parámetro.
            var envasesCliente = await listaEnvase.FtnRegresarResumenEnvasePorClienteVM(iIdClienteParam);

            if (envasesCliente == null)
            {
                await DisplayAlert("¡Atención!", "Error al mostrar el Resumen de Abono de Envase.", "OK");
            }
            else
            {
                lsvResumenEnvase.ItemsSource = envasesCliente;
            }
        }

        /*Método para imprimir el Resumen de Envase del Cliente ya con los Abonos de Envase realizados*/
        public async void OnClickedImprimir(object sender, EventArgs args)
        {
            #region Imprime el Ticket de Resumen de Abono de Envase del Cliente las veces que sean necesarias
            if (VarEntorno.sTipoImpresora == "Zebra")
            {
                bool bRespuesta = false;

                ZeImprimeAbonoEnvaseAVM ticketAbonoEnvase = new ZeImprimeAbonoEnvaseAVM();

                do
                {
                    bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Resumen de Abono de Envase?", "Si", "No");

                    if (bRespuesta == true)
                    {
                        ticketAbonoEnvase.FtnImprimirTicketAbonoEnvaseA(iIdCliente, "000000");
                    }
                }
                while (bRespuesta);
            }
            else
            {
                bool bRespuesta = false;

                ImprimeAbonoEnvaseAVM ticketAbonoEnvase = new ImprimeAbonoEnvaseAVM();

                do
                {
                    bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Resumen de Abono de Envase?", "Si", "No");

                    if (bRespuesta == true)
                    {
                        ticketAbonoEnvase.FtnImprimirTicketAbonoEnvaseA(iIdCliente, "000000");
                    }
                }
                while (bRespuesta);
            }
            #endregion
        }

        /*Método para abrir la pantalla de ABONO DE ENVASE*/
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
    }
}
