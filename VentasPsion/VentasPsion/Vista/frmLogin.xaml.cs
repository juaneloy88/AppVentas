using VentasPsion.Modelo.Servicio;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using Base;
using VentasPsion.VistaModelo;
using System;

namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmLogin : ContentPage
    {
 

        public frmLogin()
        {
            InitializeComponent();
            this.BackgroundImage = "fondo.png";
            lblVersion.Text = VarEntorno.sVersionApp;

            NavigationPage.SetHasNavigationBar(this, false);

            string Perfil = Plugin.Settings.CrossSettings.Current
            .GetValueOrDefault<string>("Perfil", "");

            string Ruta = Plugin.Settings.CrossSettings.Current
            .GetValueOrDefault<string>("Ruta", "");

            string InicioTurno = Plugin.Settings.CrossSettings.Current
            .GetValueOrDefault<string>("InicioTurno", "");

            string FinTurno = Plugin.Settings.CrossSettings.Current
            .GetValueOrDefault<string>("FinTurno", "");

            if (VarEntorno.iNoRuta == 0)
                if (Ruta != "")
                    VarEntorno.iNoRuta = Convert.ToInt32(Ruta);

            if (VarEntorno.bInicioTurno == false)
                if (InicioTurno != "")
                    VarEntorno.bInicioTurno = true;

            if (VarEntorno.bFinTurno == false)
                if (FinTurno != "")
                    VarEntorno.bFinTurno = true;

            if (VarEntorno.sTipoVenta==null)
                if (Perfil != "")
                {
                    VarEntorno.sTipoVenta = Perfil;
                    if (Convert.ToChar(Perfil.ToUpper().Substring(0, 1)) == 'T' || Convert.ToChar(Perfil.ToUpper().Substring(0, 1)) == 'M')
                    {
                        if (Convert.ToChar(Perfil.ToUpper().Substring(0, 1)) == 'T')
                            VarEntorno.cTipoVenta = 'P';
                        else
                            VarEntorno.cTipoVenta = 'A';

                        VarEntorno.bEsTeleventa = true;
                    }
                    else
                    {
                        VarEntorno.cTipoVenta = Convert.ToChar(Perfil.ToUpper().Substring(0, 1));
                        VarEntorno.bEsTeleventa = false;
                    }
                }

            string sImpresoraConfigurada = Plugin.Settings.CrossSettings.Current.GetValueOrDefault<string>("Impresora", "");

            if (sImpresoraConfigurada.Trim().Contains("XX"))
            {
                
                VarEntorno.sTipoImpresora = "Zebra";
                //utilerias.crearMensajeLargo("¡Atención!\nNo hay Impresora Bluetooth configurada actualmente.");
            }
            else
            {
                VarEntorno.sTipoImpresora = "";
            }

            lblPerfil.Text = VarEntorno.sTipoVenta.ToString();
        }

    
        #region Método en donde se valida el Login usando la clase Login
        public async Task EjecutaLogin()
        {
            Utilerias oUtilerias = new Utilerias();
            var oDialogoCargando = oUtilerias.crearProgressDialog("Cargando", "Espere por favor...");
            oDialogoCargando.Show();

            await Task.Run(() =>
            {
                //Se instancia la clase Login
                empleadosSR login = new empleadosSR();

                //Se obtienen los valores de los Entry Usuario y Contraseña
                string sUsuario = txtUsuario.Text;
                string sContrasenia = txtContrasenia.Text;

                //Validación del Login
                string sRespuesta = login.ValidaLogin(sUsuario, sContrasenia);

                //Respuesta
                oDialogoCargando.Dismiss();
                if (sRespuesta == "Ok")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //Navigation.PushModalAsync(new frmMenuPrincipal());
                        Navigation.PushAsync(new frmMenuPrincipal());
                        VarEntorno.iNUsuario = Convert.ToInt32(sUsuario);
                        VarEntorno.sUriConexionEnvio = "http://192.168.2.23/PublishWebApi/api/";
                        VarEntorno.bVisitaAllCtsReparto = true;
                        VarEntorno.bTipoBaseDatos = true;
                    });      
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DisplayAlert("Login Incorrecto", sRespuesta, "Ok");
                    });
                    
                }
            });
        }
        #endregion Método en donde se valida el Login usando la clase Login
    }
}
