using Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VentasPsion.Modelo.ServicioApi;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrmSeleccionPerfil : ContentPage
    {
        private bool saltarVista;

        /*Método constructor de la clase*/
        public FrmSeleccionPerfil(bool saltarVista)
        {
            InitializeComponent();
            this.saltarVista = saltarVista;
            var viewModel = new PerfilVM();
            listViewPerfil.ItemsSource = viewModel.oPerfil;
            listViewPerfil.ItemSelected += OnClickPerfilSeleccionado;

            if (this.saltarVista)
            {
                Navigation.PushAsync(new frmLogin());
            }
        }

        /*Método para obtener que Perfil fue seleccionado y mandar llamar a un método que carga los usuarios de dicho perfil*/
        private async void OnClickPerfilSeleccionado(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as PerfilVM.Perfil;

            if (item != null)
            {
                var oPerfil = item.NombrePerfil;
                if (oPerfil.ToCharArray(0, 1)[0] == 'T' || oPerfil.ToCharArray(0, 1)[0] == 'M')
                {
                    VarEntorno.sTipoVenta = oPerfil.ToUpper();                    
                    VarEntorno.bEsTeleventa = true;

                    if (oPerfil.ToCharArray(0, 1)[0] == 'T')
                        VarEntorno.cTipoVenta = 'P';
                    else
                        VarEntorno.cTipoVenta = 'A';
                }
                else
                {
                    VarEntorno.sTipoVenta = oPerfil.ToUpper();
                    VarEntorno.cTipoVenta = oPerfil.ToCharArray(0, 1)[0];
                    VarEntorno.bEsTeleventa = false;
                }

                bool bRespuesta = await DisplayAlert("Pregunta", "¿Desea borrar toda la información y cargar el Perfil?", "Si", "No");

                if (bRespuesta == true)
                {
                    bRespuesta = await DisplayAlert("Pregunta", "¿Seguro que desea reiniciar la información de la Ruta?", "Si", "No");
                    if (bRespuesta == true)
                    {
                        await FtnCargarEmpleados();
                    }
                }
                
            }

            listViewPerfil.SelectedItem = false;
        }

        /*Método para cargar los Usuarios según el Perfil seleccionado, para ello antes manda llamar a un método que valida hay conexión WIFI o de DATOS*/
        public async Task FtnCargarEmpleados()
        {
            ConexionService conexionWifiDatos = new ConexionService();
            StatusRestService statusConexion = new StatusRestService();
            statusConexion = conexionWifiDatos.FtnValidarConexionWifiDatos();

            if (statusConexion.status == true)
            {
                /*Para pruebas*/
                //await DisplayAlert("Aviso", statusConexion.mensaje, "OK");

                string sConexionUri = statusConexion.mensaje;
                string sConexionTipo = statusConexion.valor;

                Utilerias oUtilerias = new Utilerias();
                var oDialogoCargando = oUtilerias.crearProgressDialog("Cargando por " + sConexionTipo, "Descargando información...");
                oDialogoCargando.Show();

                await Task.Run(() =>
                {
                    EmpleadosRestServiceARP empleadosApi = new EmpleadosRestServiceARP();
                    StatusRestService statusEmpleadosApi = new StatusRestService();
                    statusEmpleadosApi = empleadosApi.FtnCargarEmpleados(VarEntorno.cTipoVenta, VarEntorno.sTipoVenta, sConexionUri);

                    oDialogoCargando.Dismiss();

                    if (statusEmpleadosApi.status == true)
                    {
                        VarEntorno.iNoRuta = 0;
                        VarEntorno.bInicioTurno = false;
                        VarEntorno.bFinTurno = false;

                        Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Perfil", VarEntorno.sTipoVenta);
                        Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Ruta", "");
                        Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("InicioTurno", "");
                        Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("FinTurno", "");
                        Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Fecha", DateTime.Now.ToShortDateString());

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            oUtilerias.crearMensaje(statusEmpleadosApi.mensaje);
                            //DisplayAlert("Aviso", statusEmpleadosApi.mensaje, "OK");
                            //this.Navigation.PushModalAsync(new frmLogin());

                            Navigation.PushAsync(new frmLogin());
                        });
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Aviso", statusEmpleadosApi.mensaje, "OK");
                        });
                    }
                });
            }
            else
            {
                await DisplayAlert("¡Atención!", statusConexion.mensaje + " para iniciar el Perfil.", "OK");
            }
        }

        /*Método para cerrar la Aplicación de Ventas*/
        public void OnClickedSalir(object sender, EventArgs args)
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}
