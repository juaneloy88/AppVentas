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
    public partial class frmResumenDia : ContentPage
    {
        resumenDia vwResDia = new resumenDia();

        public frmResumenDia()
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            cargaResumenDía();
        }

        #region Método para consultar las ventas y mostrarlas en la lista
        public async void cargaResumenDía()
        {
            int iVenta = 0;
            int vRegistros =  vwResDia.validaSiExistenRegistrosVenta();

            //Validación de si existen movimientos de ventas
            if (vRegistros == 1)
            {
                //Se obtiene el resumen de ventas del día
                var vResumenDia = await vwResDia.consultaResumenDia();
                lsvResumenDia.ItemsSource = vResumenDia;

                foreach (ResumenDia v in vResumenDia)
                {
                    iVenta = iVenta + v.iVenta;
                }

                lblCartones.Text = iVenta.ToString();
            }
            else
            {
                Utilerias oUtilerias = new Utilerias();
                oUtilerias.crearMensaje("No existen Registros de Venta");
            }
        }
        #endregion Método para consultar las ventas y mostrarlas en la lista

        #region Botón de Regresar
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion   Botón de Regresar

        /*Método que imprime el Ticket de Resumen del Venta de una Ruta*/
        private async void btnImprimirTicket_Clicked(object sender, EventArgs e)
        {
            if (VarEntorno.sTipoImpresora == "Zebra")
            {
                ZeImprimeResumenAPVM ticketResumen = new ZeImprimeResumenAPVM();

                bool bRespuesta;

                bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Resumen de Venta?", "Si", "No");

                if (bRespuesta == true)
                {
                    ticketResumen.FtnImprimirResumenAP(VarEntorno.iNoRuta);
                }
            }
            else
            {
                ImprimeResumenAPVM ticketResumen = new ImprimeResumenAPVM();

                bool bRespuesta;

                bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Resumen de Venta?", "Si", "No");

                if (bRespuesta == true)
                {
                    ticketResumen.FtnImprimirResumenAP(VarEntorno.iNoRuta);
                }
            }
        }
    

    }
}
