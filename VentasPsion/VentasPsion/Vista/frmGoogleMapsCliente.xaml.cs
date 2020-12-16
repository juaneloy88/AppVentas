using System.Collections.Generic;
using VentasPsion.Modelo.Entidad;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Base;
using System;
using VentasPsion.VistaModelo;
using System.Threading.Tasks;
using Java.Util;
using VentasPsion.Base;
using VentasPsion.Modelo.Servicio;
using System.IO;
using Android.Util;
using Android.Graphics;

namespace VentasPsion.Vista
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmGoogleMapsCliente : ContentPage
    {
        Map oMap;
        Utilerias oUtilerias;
        Stream oStream;
        //private List<clientes> clientes;
        private bool esEdicion;
        private clientes vCliente;
        private string oLatitud, oLongitud;
        Formularios oFormularios = new Formularios();


        public frmGoogleMapsCliente(clientes vCliente, bool _esEdicion, string _oLatitud , string _oLongitud)
        {
            // constructor
            InitializeComponent();
            if (vCliente is null)
                MapaCompetencia( _esEdicion,  _oLatitud,  _oLongitud);
            else
                MapaCliente(vCliente, _esEdicion, _oLatitud, _oLongitud);

        } // fin constructor

        private void MapaCompetencia(bool _esEdicion, string _oLatitud, string _oLongitud)
        {
            try
            {
                this.vCliente = vCliente;
                this.esEdicion = _esEdicion;
                this.oLatitud = _oLatitud;
                this.oLongitud = _oLongitud;
                var oCliente = vCliente;

                oUtilerias = new Utilerias();
                oMap = new Map
                {
                    HeightRequest = 100,
                    WidthRequest = 960,
                    IsEnabled = true,
                    IsVisible = true,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Position oPositionPrincipal = new Position(21.8853592, -102.2901165);
                /*
                if (oCliente.cln_latitud != 0 && oCliente.cln_longitud != 0)
                {
                    var oDistancia = String.Format("{0:0.00}", oUtilerias.distance(Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud), oCliente.cln_latitud, oCliente.cln_longitud, 'K'));
                    var oPin = new Pin()
                    {
                        Label = oCliente.clc_nombre_comercial,
                        Position = new Position(oCliente.cln_latitud, oCliente.cln_longitud),
                        Type = PinType.Place,
                        //Address = oCliente.cln_clave.ToString() + "A: " + oDistancia + "km.",
                        IsVisible = true,
                    };
                    */
                    oStream = new MemoryStream(Convert.FromBase64String(oUtilerias.camionBase64.Split(",".ToCharArray(), 2)[1]));
                    var oPinUbicActual = new Pin()
                    {
                        Label = "UBICACIÓN ACTUAL",
                        Position = new Position(Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud)),
                        Type = PinType.Place,
                        Address = "Información obtenida por Google Maps",
                        IsVisible = true,
                        IsDraggable = true,
                        Icon = BitmapDescriptorFactory.FromStream(oStream)
                    };
                    oMap.Pins.Add(oPinUbicActual);
                /*
                    oPositionPrincipal = new Position(oCliente.cln_latitud, oCliente.cln_longitud);
                
                    oMap.Pins.Add(oPin);
                    oMap.SelectedPin = oPin;                
                }
                else
                {
                    oUtilerias.crearMensaje("Sin ubicación que mostrar.");
                }
                */
                oMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                oPositionPrincipal, Distance.FromKilometers(9)));

                var stack = new StackLayout { Spacing = 0 };
                stack.Children.Add(oMap);
                var oButtonRegresar = oFormularios.CrearButton(true, true, "REGRESAR", LayoutOptions.FillAndExpand, "ic_arrow_back.png");
                if (this.esEdicion)
                {
                    var oButtonEditar = oFormularios.CrearButton(true, true, "Marcar Ubicación", LayoutOptions.FillAndExpand, "ic_checked.png");
                    oButtonEditar.Clicked += OnClickUbicacion;
                    oButtonRegresar.Margin = new Thickness(0, 10, 0, 0);
                    stack.Children.Add(oButtonEditar);
                }

                oButtonRegresar.Clicked += OnClickRegresar;
                stack.Children.Add(oButtonRegresar);



                Content = stack;
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message.ToString(), "ok");
            }
        }

        private void OnClickUbicarComp(object sender, EventArgs e)
        {
            var oAlertDialog = oUtilerias.crearAlertDialog("Aviso", "Esta apunto de marcar la ubicacion GPS");
            oAlertDialog.Create();
            oAlertDialog.SetPositiveButton("ACEPTAR", (senderAlert, args) =>
            {
                /*
                fnGPS ofnGPS = new fnGPS();
                ofnGPS.fnActualizaLatituLongitud(this.vCliente.cln_clave, Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud));
                */
                /*
                fnClientes_Estatus cGPS = new fnClientes_Estatus();
                bool bREsult = cGPS.fnActualizaGPS(this.vCliente.cln_clave, Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud));
                */
                oUtilerias.crearMensaje("Cliente ubicado con éxito.");
                Regresar();
            });
            oAlertDialog.SetNegativeButton("CANCELAR", (senderAlert, args) =>
            {

            });
            oAlertDialog.Show();

        }

        private void MapaCliente(clientes vCliente, bool _esEdicion, string _oLatitud, string _oLongitud)
        {
            try
            {

                this.vCliente = vCliente;
                this.esEdicion = _esEdicion;
                this.oLatitud = _oLatitud;
                this.oLongitud = _oLongitud;
                var oCliente = vCliente;

                oUtilerias = new Utilerias();
                oMap = new Map
                {
                    HeightRequest = 100,
                    WidthRequest = 960,
                    IsEnabled = true,
                    IsVisible = true,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Position oPositionPrincipal = new Position(21.8853592, -102.2901165);

                if (oCliente.cln_latitud != 0 && oCliente.cln_longitud != 0)
                {
                    var oDistancia = String.Format("{0:0.00}", oUtilerias.distance(Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud), oCliente.cln_latitud, oCliente.cln_longitud, 'K'));
                    var oPin = new Pin()
                    {
                        Label = oCliente.clc_nombre_comercial,
                        Position = new Position(oCliente.cln_latitud, oCliente.cln_longitud),
                        Type = PinType.Place,
                        Address = oCliente.cln_clave.ToString() + "A: " + oDistancia + "km.",
                        IsVisible = true,
                    };

                    oStream = new MemoryStream(Convert.FromBase64String(oUtilerias.camionBase64.Split(",".ToCharArray(), 2)[1]));
                    var oPinUbicActual = new Pin()
                    {
                        Label = "UBICACIÓN ACTUAL",
                        Position = new Position(Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud)),
                        Type = PinType.Place,
                        Address = "Información obtenida por Google Maps",
                        IsVisible = true,
                        IsDraggable = true,
                        Icon = BitmapDescriptorFactory.FromStream(oStream)
                    };
                    oMap.Pins.Add(oPinUbicActual);
                    oPositionPrincipal = new Position(oCliente.cln_latitud, oCliente.cln_longitud);
                    oMap.Pins.Add(oPin);
                    oMap.SelectedPin = oPin;
                }
                else
                {
                    oUtilerias.crearMensaje("Sin ubicación que mostrar.");
                }
                oMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                oPositionPrincipal, Distance.FromKilometers(9)));

                var stack = new StackLayout { Spacing = 0 };
                stack.Children.Add(oMap);
                var oButtonRegresar = oFormularios.CrearButton(true, true, "REGRESAR", LayoutOptions.FillAndExpand, "ic_arrow_back.png");
                if (this.esEdicion)
                {
                    var oButtonEditar = oFormularios.CrearButton(true, true, "Editar Ubicación", LayoutOptions.FillAndExpand, "ic_checked.png");
                    oButtonEditar.Clicked += OnClickUbicacion;
                    oButtonRegresar.Margin = new Thickness(0, 10, 0, 0);
                    stack.Children.Add(oButtonEditar);
                }

                oButtonRegresar.Clicked += OnClickRegresar;
                stack.Children.Add(oButtonRegresar);



                Content = stack;
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message.ToString(), "ok");
            }
        }

        private void OnClickEditarUbicacion(object sender, EventArgs e)
        {
            var oAlertDialog = oUtilerias.crearAlertDialog("Aviso", "¿Está seguro de reubicar al cliente: "+this.vCliente.clc_nombre_comercial+"?");
            oAlertDialog.Create();
            oAlertDialog.SetPositiveButton("ACEPTAR", (senderAlert, args) =>
            {
                /*fnGPS ofnGPS = new fnGPS();
                ofnGPS.fnActualizaLatituLongitud(this.vCliente.cln_clave, Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud));
                */

                clientes_estatusSR cGPS = new clientes_estatusSR();
                bool bREsult = cGPS.fnActualizaGPS(this.vCliente.cln_clave , Convert.ToDouble(this.oLatitud), Convert.ToDouble(this.oLongitud));
                oUtilerias.crearMensaje("Cliente reubicado con éxito.");
                Regresar();
            });
            oAlertDialog.SetNegativeButton("CANCELAR", (senderAlert, args) =>
            {

            });
            oAlertDialog.Show();
           
        }

        private void OnClickUbicacion(object sender, EventArgs e)
        {
            if (vCliente is null)
                OnClickUbicarComp(sender, e);
            else
                OnClickEditarUbicacion(sender, e);
        }

        public void Regresar()
        {
            this.Navigation.PopModalAsync();
        }

        private void OnClickRegresar(object sender, EventArgs e)
        {

            Regresar();
        } // fin OnClickRegresar

        void Pin_Clicked(object sender, EventArgs e)
        {
            var pin = (Pin)sender;

            DisplayAlert("Pin Clicked", $"{pin.Label} Clicked.", "Close");
        }

        public Bitmap Base64ToBitmap(String base64String)
        {
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
    }
}
