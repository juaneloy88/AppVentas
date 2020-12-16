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
	public partial class frmMenuReparto : ContentPage
	{
        private static bool banderaClick = true;
        public frmMenuReparto ()
		{
			InitializeComponent ();
            if (VarEntorno.iNoRuta.Equals(0))
            {
                Title = VarEntorno.sTipoVenta;
            }
            else
            {
                Title = "RUTA: " + VarEntorno.iNoRuta.ToString();
            }
            var viewModel = new MenuUsuarioVM("REPARTO");
            listViewMenuPrincipal.ItemsSource = viewModel.oMenuUsuario;
            listViewMenuPrincipal.ItemSelected += OnClickOpcionMenuSeleccionado;
        }



        private async void OnClickOpcionMenuSeleccionado(object sender, SelectedItemChangedEventArgs e)
        {
            listViewMenuPrincipal.SelectedItem = true;
            if (banderaClick)
            {
                banderaClick = false;

                var item = e.SelectedItem as MenuUsuarioVM.MenuPrincipal;
                if ((item != null) && (item.Habilitado))
                {
                    var OpcionSeleccionada = item.Opcion;
                    switch (OpcionSeleccionada)
                    {
                        case "REPARTO":
                            OnClickedReparto();
                            break;
                        case "DEVOLUCIONES":
                            OnClickedDevolucion();
                            break;
                        case "COBRANZA Saldo":
                            OnClickedCobranza();
                            break;
                        case "COBRANZA Documentos":
                            OnClickedCobranzaPagos();
                            break;
                        case "PREVIO":
                            OnClickedPrevio();
                            break;
                        case "CONSULTA":
                            OnClickedConsulta();
                            break;
                        case "CONSULTA DOCUMENTOS":
                            OnClickedConsultaDocs();
                            break;
                        case "RESUMEN":
                            OnClickedResumenDia();
                            break;
                        case "MAPA":
                            OnClickedVerMapa();
                            break;
                        case "ENCUESTA":
                            OnClickedVerEncuesta();
                            break;
                        case "CLIENTES REQUISITOS":
                            OnClickedRequisitosClientes();
                            break;
                        case "RECEPCION PARCIAL":
                            OnClickedRecepcionParcial();
                            break;
                        case "REGISTRO TELEFONICO":
                            OnClickedRegistroTelefonico();
                            break;
                    }// fin del switch

                    //FrmClientesRequisitos
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        banderaClick = true;

                    });

                } // fin if  item

            } // fin if banderaCLick
        }
        public void OnClickedRegistroTelefonico()
        {
             this.Navigation.PushModalAsync(new frmBuscarCliente(53));
            //this.Navigation.PushModalAsync(new frmRegistroTelefono());
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedRequisitosClientes()
        {
            
            this.Navigation.PushModalAsync(new frmResumenClientesRequisitos());
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedConsultaDocs()
        {
            this.Navigation.PushModalAsync(new frmBuscarCliente(51));
            listViewMenuPrincipal.SelectedItem = false;
        }


        public void OnClickedVerEncuesta()
        {
            
            this.Navigation.PushModalAsync(new frmBuscarCliente(8));
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedVerMapa()
        {
            /*
           JSONArray oJSONArray = new JSONArray();
           buscarCliente oBuscarCliente = new buscarCliente();
           var clientes = oBuscarCliente.obtenerclientes();
           JsonConvert.SerializeObject(clientes);
           oJSONArray.Put(JsonConvert.SerializeObject(clientes));
           return oJSONArray;*/
            
            //Navigation.PushAsync(new MdlGoogleMaps(clientes));
            //listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedReparto()
        {
            VarEntorno.bSoloCobrar = false;
            //  this.Navigation.PushModalAsync(new frmMostrarPedido());
            this.Navigation.PushModalAsync(new frmBuscarCliente(3));
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedCobranza()
        {
            VarEntorno.bSoloCobrar = true;
         //   this.Navigation.PushModalAsync(new frmCobranza());
            this.Navigation.PushModalAsync(new frmBuscarCliente(1));
            listViewMenuPrincipal.SelectedItem = false;
        }
        
        public void OnClickedCobranzaPagos()
        {
            VarEntorno.bSoloCobrar = true;
            // this.Navigation.PushModalAsync(new frmCobranza());
            this.Navigation.PushModalAsync(new frmBuscarCliente(50));
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedDevolucion()
        {
            //this.Navigation.PushModalAsync(new frmDevolucionReparto());
            this.Navigation.PushModalAsync(new frmBuscarCliente(4));
            listViewMenuPrincipal.SelectedItem = false;
        }

        private void OnClickedPrevio()
        {
            this.Navigation.PushModalAsync(new frmPrevio());
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedResumenDia()
        {
            //this.Navigation.PushModalAsync(new frmResumenDia());
            this.Navigation.PushModalAsync(new frmResumenAR());
            listViewMenuPrincipal.SelectedItem = false;
        }


        public void OnClickedRegresar()
        {
            this.Navigation.PopModalAsync();
        }

        public void OnClickedConsulta()
        {
            this.Navigation.PushModalAsync(new frmEstatusCliente());
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedRecepcionParcial()
        {
            this.Navigation.PushModalAsync(new FrmRecepcionParcial());
            listViewMenuPrincipal.SelectedItem = false;
        }
    }
}
