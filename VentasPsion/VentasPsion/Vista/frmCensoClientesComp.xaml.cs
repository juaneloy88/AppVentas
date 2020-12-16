using System;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Base;

using Plugin.Permissions;


using VentasPsion.VistaModelo;

using Plugin.Permissions.Abstractions;
using Plugin.Geolocator;


namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmCensoClientesComp : ContentPage
    {
        Utilerias oUtilerias = new Utilerias();
        string oLatitud = "", oLongitud = "";

        CensoVM censo = new CensoVM();

        public frmCensoClientesComp()
        {
            InitializeComponent();
            infoapp();
            pckSegmento.Items.Add("C -TRADICIONAL");
            pckSegmento.Items.Add("A -ON PREMISE");
            pckSegmento.Items.Add("NP-NEGOCIOS PROPIOS");
            pckSegmento.Items.Add("CM-CANAL MODERNO");
            pckSegmento.Items.Add("E -AUTOSERVICIO");
            cargaCoordenadas();
        }


        private void infoapp()
        {
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;
        }


        public void OnClickEditarUbicacion(object sender, EventArgs args)
        {
            abrirMapa(2);
        }


        public async void cargaCoordenadas()
        {
            var cached = await CrossGeolocator.Current.GetPositionAsync();
            /***************************************/
            /****FRAGMENTO DE PERMISOS ANDROID *****/
            /***************************************/
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    // DENEGADO
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        oUtilerias.crearMensajeLargo("Es necesario aceptar el permiso.");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    // PERMITIRO

                    if (cached == null)
                    {
                        oUtilerias.crearMensajeLargo("Favor de encender su GPS");
                    }
                    else
                    {
                        this.oLatitud = cached.Latitude.ToString();
                        this.oLongitud = cached.Longitude.ToString();
         
                        //await this.Navigation.PushModalAsync(new frmGoogleMapsCliente(VarEntorno.vCliente,  true, this.oLatitud, this.oLongitud));
                    }

                }
                else if (status != PermissionStatus.Unknown)
                {
                    oUtilerias.crearMensajeLargo("Es necesario aceptar el permiso, intente de nuevo");
                }
            }
            catch (Exception ex)
            {

                oUtilerias.crearMensajeLargo("Error: " + ex);
            }

            /***************************************/
            /*****FRAGMENTO DE PERMISOS ANDROID ****/
            /***************************************/
        }

        public async void abrirMapa(int iTipo)
        {
            var cached = await CrossGeolocator.Current.GetPositionAsync();
            /***************************************/
            /****FRAGMENTO DE PERMISOS ANDROID *****/
            /***************************************/
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    // DENEGADO
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        oUtilerias.crearMensajeLargo("Es necesario aceptar el permiso.");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    // PERMITIRO

                    if (cached == null)
                    {
                        oUtilerias.crearMensajeLargo("Favor de encender su GPS");
                    }
                    else
                    {
                        this.oLatitud = cached.Latitude.ToString();
                        this.oLongitud = cached.Longitude.ToString();
                        Device.OpenUri(new Uri("geo:0,0?q=" + this.oLatitud + "," + this.oLongitud));
                        //await this.Navigation.PushModalAsync(new frmGoogleMapsCliente(VarEntorno.vCliente,  true, this.oLatitud, this.oLongitud));
                    }

                }
                else if (status != PermissionStatus.Unknown)
                {
                    oUtilerias.crearMensajeLargo("Es necesario aceptar el permiso, intente de nuevo");
                }
            }
            catch (Exception ex)
            {

                oUtilerias.crearMensajeLargo("Error: " + ex);
            }

            /***************************************/
            /*****FRAGMENTO DE PERMISOS ANDROID ****/
            /***************************************/
        }

        public void OnClickedGuardar(object sender, EventArgs args)
        {
            try
            {
                if (valida_registro())
                {
                    censo.ClienteComp.ccc_nombre = txtNombre.Text.Trim();
                    censo.ClienteComp.ccc_negocio = txtNegocio.Text.Trim();
                    censo.ClienteComp.ctp_clave = pckSegmento.SelectedItem.ToString().Substring(0, 2).Trim();
                    censo.ClienteComp.ccn_latitud = oLatitud;
                    censo.ClienteComp.ccn_longitud = oLongitud;

                    if (censo.GuardarComp())
                    {
                        this.Navigation.PopModalAsync();
                        oUtilerias.crearMensaje("¡Guardado exitoso!");
                    }
                    else
                        DisplayAlert("Aviso", VarEntorno.sMensajeError, "OK");
                }
                else
                {
                    DisplayAlert("Aviso", "Datos incompletos o incorrectos", "OK");
                }
            } catch (Exception ex)
            {
                DisplayAlert("Aviso", ex.Message, "OK");
            }
            
        }

        private bool valida_registro()
        {
            try
            {
                bool bResultado = false;
                if (txtNombre.Text.Trim() != "")
                {
                    if (txtNegocio.Text.Trim() != "")
                    {
                        if (pckSegmento.SelectedIndex != -1)
                        {
                            bResultado = true;
                        }
                    }
                }

                return bResultado;
            }
            catch
            {
                return false;
            }
        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }

    }
}