using System.Collections.Generic;
using VentasPsion.Modelo.Entidad;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Base;
using System;
using VentasPsion.VistaModelo;
using XFShapeView;

namespace VentasPsion.Vista
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MdlGoogleMaps : ContentPage
    {
        Map oMap;
        Utilerias oUtilerias = new Utilerias();
        List<StatusClientes> lLista = new List<StatusClientes>();
        vmStatusClientes vmStatusCtes = new vmStatusClientes();
       
        private List<clientes> clientes;

        public MdlGoogleMaps(List<clientes> _oListaClientes)
        {
            // constructor
            InitializeComponent();

            Device.OpenUri(new Uri("http://maps.google.com/?daddr=" + _oListaClientes[0].cln_latitud + "," + _oListaClientes[0].cln_longitud));

            this.Clientes = _oListaClientes;
            var oListaClientes = this.Clientes;
            oMap = new Map
            {
                HeightRequest = 100,
                WidthRequest = 960,
                IsEnabled = true,
                IsVisible = true,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            Position oPositionPrincipal = new Position(21.8853592, -102.2901165);

            var contadorUbicaciones = 0;
            var cln_secuencia = "";
              lLista = vmStatusCtes.obtieneStatusClientes().Result;
              var oColor = Color.Gray;
              for (int i = 0; i < oListaClientes.Count; i++)
              {

                for (int j = 0; j < lLista.Count; j++)
                {
                    if (Convert.ToInt32(lLista[j].sCliente) == oListaClientes[i].cln_clave)
                    {

                        if (oListaClientes[i].cln_latitud != 0 && oListaClientes[i].cln_longitud != 0)
                        {
                            switch (lLista[j].iVisitado)
                            {
                                case 0:

                                    oColor = Color.Red;
                                    break;
                                case 1:
                                    oColor = Color.Green;
                                    break;
                                case 2:
                                    oColor = Color.Red;
                                    break;
                                case 3:
                                    oColor = Color.Orange;
                                    break;
                            }

                            ShapeView oBox = new ShapeView();
                            oBox.ShapeType = ShapeType.Oval;
                            oBox.HeightRequest = 35;
                            oBox.WidthRequest = 35;
                            oBox.Color = Color.Transparent;
                            oBox.CornerRadius = 1;
                            oBox.BorderWidth = 4f;
                            oBox.BorderColor = oColor;

                            if (oListaClientes[i].cln_secuencia==null)
                            {
                                cln_secuencia = "";
                            }
                            else
                            {
                                cln_secuencia = oListaClientes[i].cln_secuencia;
                            }
                            //   oBox.Margin = 10;
                            oBox.Content =
                           new Label
                            {
                                Text = cln_secuencia,
                                FontSize = Font.SystemFontOfSize(20, FontAttributes.Bold).FontSize,
                                TextColor = Color.Black,
                                HorizontalOptions = LayoutOptions.Fill,
                                VerticalOptions = LayoutOptions.Fill,
                                VerticalTextAlignment = TextAlignment.Center,
                                HorizontalTextAlignment = TextAlignment.Center,
                                Margin = new Thickness(0, 0, 20, 20),


                        };
                          

                            contadorUbicaciones++;

                            var oPin = new Pin()
                            {
                                Label = oListaClientes[i].clc_nombre_comercial,
                                Position = new Position(oListaClientes[i].cln_latitud, oListaClientes[i].cln_longitud),
                                Type = PinType.Place,
                                Address = oListaClientes[i].cln_clave.ToString(),
                                IsVisible = true,
                                Tag = "id_tokyo",
                               // Icon = BitmapDescriptorFactory.DefaultMarker(oColor)
                                Icon = BitmapDescriptorFactory.FromView(oBox)
                            };
                            oBox = null;

                            //oPin.Clicked += Pin_Clicked;
                            oMap.Pins.Add(oPin);
                           // oMap.SelectedPin = oPin;
                        }

                    }

                }

       
              }// fin del for

            oMap.BindingContextChanged += new EventHandler(BindingContext_Changed);
        
            if (contadorUbicaciones==0)
            {
                oUtilerias.crearMensaje("Sin ubicaciones que mostrar.");
            }

            oMap.SelectedPinChanged += (object sender, SelectedPinChangedEventArgs e) =>
            {
                //labelStatus.Text = $"SelectedPin changed - {e?.SelectedPin?.Label ?? "nothing"}";
            };

            oMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
            oPositionPrincipal, Distance.FromKilometers(9)));

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(oMap);


            Content = stack;

        } // fin constructor


        private void BindingContext_Changed(object sender, EventArgs e)
        {
            Console.WriteLine("BindingContext changed");
        }
        void Pin_Clicked(object sender, EventArgs e)
        {
            var pin = (Pin)sender;

            DisplayAlert("Pin Clicked", $"{pin.Label} Clicked.", "Close");
        }

        public List<clientes> Clientes { get => clientes; set => clientes = value; }



    }
}
