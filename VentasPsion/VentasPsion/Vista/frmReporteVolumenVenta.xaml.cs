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
	public partial class frmReporteVolumenVenta : ContentPage
	{
		public frmReporteVolumenVenta ()
		{
			InitializeComponent ();
            //


            //



            var viewModel = new VolumenVentaVM();
            listViewVolumenVenta.ItemsSource = viewModel.volumenventas;
        }
	}
}