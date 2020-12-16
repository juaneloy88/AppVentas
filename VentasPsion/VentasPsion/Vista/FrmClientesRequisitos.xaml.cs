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
	public partial class FrmClientesRequisitos : ContentPage
	{
		public FrmClientesRequisitos ()
		{
			InitializeComponent ();
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            GetEncuesta();
        }

        private void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }

        public void GetEncuesta()
        {
            clientes_requisitos_surtirVM VMClientes = new clientes_requisitos_surtirVM();
            var vDatosReqs = VMClientes.ClientesReqs();


            lblFactn.Text = vDatosReqs.crb_factura.ToString();
            lblChan.Text = vDatosReqs.crb_chamuco.ToString();
            lblEscn.Text = vDatosReqs.crb_escaleras.ToString();
            lblRampn.Text = vDatosReqs.crb_rampa.ToString();
            lblEspn.Text = vDatosReqs.crb_espacio_estrecho.ToString();
            lblAsalton.Text = vDatosReqs.crb_asaltos.ToString();

            if (vDatosReqs.crb_factura > 0)
               lblFactrue.IsVisible = true;
            else
               lblFacfalse.IsVisible = true;

            if (vDatosReqs.crb_chamuco > 0)
                lblChantrue.IsVisible = true;
            else
                lblChanfalse.IsVisible = true;

            if (vDatosReqs.crb_escaleras > 0)
                lblEsctrue.IsVisible = true;
            else
                lblEscfalse.IsVisible = true;

            if (vDatosReqs.crb_rampa > 0)
                lblRamptrue.IsVisible = true;
            else
                lblRampfalse.IsVisible = true;

            if (vDatosReqs.crb_espacio_estrecho > 0)
                lblEsptrue.IsVisible = true;
            else
                lblEspfalse.IsVisible = true;

            if (vDatosReqs.crb_asaltos > 0)
                lblAsaltotrue.IsVisible = true;
            else
                lblAsaltofalse.IsVisible = true;
        }
    }
}