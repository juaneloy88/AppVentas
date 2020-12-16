using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Base;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmReporteActivosDatos : ContentPage
	{
        ActivosComoDatosVM oActivosComoDatosVM;
        private int cln_clave;

        public frmReporteActivosDatos(int cln_clave)
        {
            this.cln_clave = cln_clave;
            InitializeComponent();

            oActivosComoDatosVM = new ActivosComoDatosVM(this.cln_clave); // descomentar cuando se migre a la otra vista
            //oActivosComoDatosVM = new ActivosComoDatosVM(0);

            listViewActivosDatos.ItemsSource = oActivosComoDatosVM.activosDatos;

            if (this.cln_clave != 0)
            {
                Formularios oFormularios = new Formularios();
                var oButtonRegresar = oFormularios.CrearButton(true, true, "REGRESAR", LayoutOptions.FillAndExpand, "ic_arrow_back.png");
                oButtonRegresar.Clicked += OnClickRegresar;
                listViewActivosDatos.Footer = oButtonRegresar;
                
            }
        }


        private void OnClickRegresar(object sender, EventArgs e)
        {
            
            this.Navigation.PopModalAsync();
        } // fin OnClickRegresar
        private void OnRefrescarLista(object sender, TextChangedEventArgs e)
        {
            listViewActivosDatos.BeginRefresh();
            oTxtBuscador.Text = "";
            listViewActivosDatos.ItemsSource = oActivosComoDatosVM.activosDatos;
            listViewActivosDatos.EndRefresh();
        }

        private void SearchBar_OnBuscarActivosDatos(object sender, TextChangedEventArgs e)
        {
            listViewActivosDatos.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                listViewActivosDatos.ItemsSource = oActivosComoDatosVM.activosDatos;
            else
                listViewActivosDatos.ItemsSource = oActivosComoDatosVM.activosDatos.Where(i => i.cln_clave.ToString().Contains(e.NewTextValue));

            listViewActivosDatos.EndRefresh();
        }
      
    }
}