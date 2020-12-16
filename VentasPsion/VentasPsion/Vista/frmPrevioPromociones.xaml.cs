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
    public partial class frmPrevioPromociones : ContentPage
    {
        vmPromociones vmPromo = new vmPromociones();
        List<MostrarPromociones> lPromociones = new List<MostrarPromociones>();
        int nCliente = VarEntorno.vCliente.cln_clave;

        public frmPrevioPromociones(List<MostrarPrevioPromocion> lMuestraPrevioPromo, List<MostrarPromociones> lMuestraPromociones)
        {
            InitializeComponent();

            lPromociones = lMuestraPromociones;
            lsvPrevio.ItemsSource = lMuestraPrevioPromo;
        }

        #region Botón que Guarda los registros de venta y de regalo en la tabla de venta_detalle
        private async void OnClickedAvanzar(object sender, EventArgs e)
        {
            string sRespuesta = await vmPromo.GuardaVenta(lPromociones);

            if (sRespuesta == "Venta Guardada" || sRespuesta == "Venta Actualizada")
            {
                VarEntorno.dImporteTotal = new VentaVM().fnImporteTotalxFolio();
                DisplayAlert("Alert", sRespuesta, "OK");
                await this.Navigation.PopModalAsync();
            }
            else
            {
                DisplayAlert("Alert", sRespuesta, "OK");
                await this.Navigation.PopModalAsync();
            }
        }
        #endregion Botón que Guarda los registros de venta y de regalo en la tabla de venta_detalle
    }
}