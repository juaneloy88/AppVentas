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
	public partial class frmActualizacionDatosClientes : ContentPage
	{
        bool bPagare = false;
        bool bFoco = false;
        bool bCooler = false;

        //Estatus_ClientesVM oEstatus = new Estatus_ClientesVM();
        ClientesVM oEstatus = new ClientesVM();

        public frmActualizacionDatosClientes ()
		{
			InitializeComponent ();
            CargaDatos();

        }

        private void CargaDatos()
        {
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            //lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;
            //int iResultado;
            bPagare = oEstatus.traePagare(VarEntorno.vCliente.cln_clave);
            /*
            switch (iResultado)
            {
                case 1:
                    bPagare = true;
                    break;
                case 0:
                    bPagare = false;
                    break;
                case -1:
                    break;
            }
            */
            

            bFoco = oEstatus.traeClienteFoco(VarEntorno.vCliente.cln_clave);

            bCooler = oEstatus.traeCoolerCliente(VarEntorno.vCliente.cln_clave);

           // sPagare.IsToggled = bPagare;

            sCooler.IsToggled = bCooler;

            sClienteFoco.IsToggled = bFoco;
        }


        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }

        public async void OnClickedGuardar(object sender, EventArgs args)
        {
            bool bRespuesta = await DisplayAlert("Pregunta", "¿Desea guardar cambios?", "Si", "No");

            if (bRespuesta == true)
            {
                bool bResultado;
                /*
                if (sPagare.IsToggled != bPagare)
                {
                    bResultado = oEstatus.ActualizaPagare(VarEntorno.vCliente.cln_clave, sPagare.IsToggled);
                    fnPagare();
                }
                */
                if (sCooler.IsToggled != bCooler)
                {
                    bResultado = oEstatus.ActualizaCooler(VarEntorno.vCliente.cln_clave, sCooler.IsToggled);
                }

                if (sClienteFoco.IsToggled != bFoco)
                {
                    bResultado = oEstatus.ActualizaFoco(VarEntorno.vCliente.cln_clave, sClienteFoco.IsToggled);
                }


                this.Navigation.PopModalAsync();
            }
        }
        /*
        public void fnPagare()
        {            
            VarEntorno.cBuscarCliente.vBuscarCliente.FnMuestraPagare(VarEntorno.vCliente.cln_clave);
        }
        */
        public void OnClickSwitchToggledPagare(object sender, ToggledEventArgs args)
        {
            /*
            if (args.Value)
            {
                bPagare = true;
            }
            else
            {
                bPagare = false;
            }
            */
        }

        public void OnClickSwitchToggledFoco(object sender, ToggledEventArgs args)
        {
          /*  if (args.Value)
            {
                bFoco = true;
            }
            else
            {
                bFoco = false;
            }
            */
        }

        public void OnClickSwitchToggledCooler(object sender, ToggledEventArgs args)
        {
            /*
            if (args.Value)
            {
                bCooler = true;
            }
            else
            {
                bCooler = false;
            }
            */
        }

    }
}
