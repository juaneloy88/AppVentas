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
	public partial class frmMenuAutoventa : ContentPage
	{
        private static bool banderaClick = true;
        public frmMenuAutoventa ()
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
            var viewModel = new MenuUsuarioVM("AUTOVENTA");
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
                        case "VENTA":
                            OnClickedVenta();
                            break;
                        case "DEVOLUCIONES":
                            OnClickedDevolucion();
                            break;
                        case "VISITA":
                            OnClickeNoVenta();
                            break;
                        case "COBRANZA Saldo":
                            OnClickedCobranza();
                            break;
                        case "COBRANZA Documentos":
                            OnClickedCobranzaPagos();
                            break;
                        case "CONSULTA":
                            OnClickedConsulta();
                            break;
                        case "ABONO ENVASE":
                            OnClickedEnvase();
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
                        case "RETO DEL DIA":
                            OnClickedRetoDia();
                            break;
                        case "VOLUMEN DE VENTA":
                            OnClickedVolumenVenta();
                            break;
                        case "ACTIVOS COMODATADOS":
                            OnClickeActivosDatos();
                            break;
                        case "REPORTE KPIs":
                            OnClickeReporteKpis();
                            break;
                        case "QUEJAS Y SUGERENCIAS":
                            OnClickeQuejasSugerencias();
                            break;
                        case "PEDIDO SUGERIDO":
                            OnClickePedidoSugerido();
                            break;
                        case "ENCUESTA":
                            OnClickedVerEncuesta();
                            break;
                        case "RECEPCION PARCIAL":
                            OnClickedRecepcionParcial();
                            break;
                        case "CENSO COMPETENCIA":
                            OnClickedCensoCompentencia();
                            break;
                        case "RELACION ANTICIPO":
                            OnClickedRelacionAnticipo();
                            break;
                        case "REGISTRO TELEFONICO":
                            OnClickedRegistroTelefonico();
                            break;
                    }// fin del switch

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

        public void OnClickedRelacionAnticipo()
        {
            this.Navigation.PushModalAsync(new frmBuscarCliente(52));
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedCensoCompentencia()
        {
            this.Navigation.PushModalAsync(new frmCensoClientesComp());
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

        public void OnClickeQuejasSugerencias()
        {
            //  Navigation.PushAsync(new frmQuejasSugerencias());
            this.Navigation.PushModalAsync(new frmBuscarCliente(6));
            listViewMenuPrincipal.SelectedItem = false;
        }

        /*Método para abrir la pantalla de ABONO DE ENVASE*/
        public void OnClickedEnvase()
        {
            this.Navigation.PushModalAsync(new frmAbonoEnvase());
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickeNoVenta()
        {
            //  Navigation.PushAsync(new frmQuejasSugerencias());
            this.Navigation.PushModalAsync(new frmBuscarCliente(9));
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickePedidoSugerido()
        {

            this.Navigation.PushModalAsync(new frmBuscarCliente(7));
            listViewMenuPrincipal.SelectedItem = false;
        }
        /// Funcion manda a llamar forma de venta 
        public void OnClickedVenta()
        {
            VarEntorno.bSoloCobrar = false;
           // this.Navigation.PushModalAsync(new FrmVentas());
            this.Navigation.PushModalAsync(new frmBuscarCliente(2));
            listViewMenuPrincipal.SelectedItem = false;
        }

  
        public void OnClickedDevolucion()
        {
            
            //this.Navigation.PushModalAsync(new frmDevolucionAutoventa());
            this.Navigation.PushModalAsync(new frmBuscarCliente(5));
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

        public void OnClickedResumenDia()
        {
            //this.Navigation.PushModalAsync(new frmResumenDia());
            this.Navigation.PushModalAsync(new frmResumenAR());
            listViewMenuPrincipal.SelectedItem = false;
        }


        public void OnClickedRegresar()
        {
            this.Navigation.PopModalAsync();
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedConsulta()
        {
            this.Navigation.PushModalAsync(new frmEstatusCliente());
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedVolumenVenta()
        {
            Navigation.PushAsync(new frmReporteVolumenVenta());
            listViewMenuPrincipal.SelectedItem = false;
        }
        public void OnClickedRetoDia()
        {
            Navigation.PushAsync(new frmReporteCuotaDia());
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
           return oJSONArray;
            buscarCliente oBuscarCliente = new buscarCliente();
            var clientes = oBuscarCliente.obtenerclientes();
            Navigation.PushAsync(new MdlGoogleMaps(clientes));
            listViewMenuPrincipal.SelectedItem = false;
            */
        }

        public void OnClickeActivosDatos()
        {
            Navigation.PushAsync(new frmReporteActivosDatos(0));
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickeReporteKpis()
        {
            Navigation.PushAsync(new frmReporteKpis());
            listViewMenuPrincipal.SelectedItem = false;
        }

        public void OnClickedRecepcionParcial()
        {
            this.Navigation.PushModalAsync(new FrmRecepcionParcial());
            listViewMenuPrincipal.SelectedItem = false;
        }
    }
}
