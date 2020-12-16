using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmMenuPrincipal : ContentPage
    {
        private static bool banderaClick;
        List<MenuPrincipalVM.MenuPrincipal> oMenuPrincipal = new List<MenuPrincipalVM.MenuPrincipal>();
        Utilerias oUtilerias = new Utilerias();
        public frmMenuPrincipal()
        {           
            InitializeComponent();
            banderaClick = true;
            Title = VarEntorno.sTipoVenta;
        }
        public  void LlenarMenu()
        {
            MenuPrincipalVM oMenuPrincipalVM = new MenuPrincipalVM();
            listViewMenuPrincipal.ItemsSource = null;
            listViewMenuPrincipal.ItemsSource = oMenuPrincipalVM.ObtenerMenu();
        }
        public async void OnClickMenuSeleccionado(object sender, SelectedItemChangedEventArgs e)
        {
            listViewMenuPrincipal.SelectedItem = true;
            if (banderaClick)
            {
               
                var item = e.SelectedItem as MenuPrincipalVM.MenuPrincipal;
                if ((item != null) && (item.Habilitado))
                {
                    var oPerfil = item.Opcion;
                    banderaClick = false;
                    switch (oPerfil)
                    {
                        case "RECEPCION":
                            await this.Navigation.PushModalAsync(new FrmRecepcion());
                            break;
                        case "INICIO DE TURNO":
                            await this.Navigation.PushModalAsync(new FrmInicioTurno());
                            break;
                        case "OPERACIONES":
                            try
                            {
                                switch (VarEntorno.cTipoVenta)
                                {
                                    case 'A':
                                        await this.Navigation.PushAsync(new frmMenuAutoventa());
                                        //this.Navigation.PushModalAsync(new frmMenuAutoventa());
                                        break;
                                    case 'P':
                                        await this.Navigation.PushAsync(new frmMenuPreventa());
                                        //this.Navigation.PushModalAsync(new frmMenuPreventa());
                                        break;
                                    case 'R':
                                        await this.Navigation.PushAsync(new frmMenuReparto());
                                        //  this.Navigation.PushModalAsync(new frmMenuReparto());
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new System.ArgumentOutOfRangeException(
                                    "Parameter index is out of range." + ex);
                            }
                            break;
                        case "FIN DE TURNO":
                            await this.Navigation.PushModalAsync(new FrmFinTurno());
                            break;
                        case "TRANSMISION":
                            await this.Navigation.PushModalAsync(new frmTransmitirRest());
                            break;
                        case "UTILERIAS":
                            OnClickedUtileria();
                            break;
                        case "SALIR":
                            OnClickedRegresar();
                            break;

                    } // fin switch
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        banderaClick = true;
                    });
                }
                else
                {
                    oUtilerias.crearMensaje("Sin acceso a la opción seleccionada.");
                }
            } // fin if bandera
        }

        protected override async void OnAppearing()
        {
            LlenarMenu();
            await Task.Yield();
        }
        public void OnClickedRegresar()
        {
            Utilerias oUtilerias = new Utilerias();
            var oAlertDialog = oUtilerias.crearAlertDialog("Aviso", "¿Está seguro de salir?");
            oAlertDialog.Create();
            oAlertDialog.SetPositiveButton("SALIR", (senderAlert, args) =>
            {
                VarEntorno.vCliente = null;
                this.Navigation.PushAsync(new FrmSeleccionPerfil(false));
            });
            oAlertDialog.SetNegativeButton("CANCELAR", (senderAlert, args) =>
            {

            });
            oAlertDialog.Show();
        }
        /*Método para abrir la pantalla de carga de perfil*/
        public void OnClickedUtileria()
        {
            //this.Navigation.PushModalAsync(new frmUtilerias());
             this.Navigation.PushAsync(new frmMenuUtilerias());
        }
    }
}
