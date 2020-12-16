using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;

using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FrmPedidoSugerido : ContentPage
    {
        PedidoSugeridoVM Pedidosugerdiovm = new PedidoSugeridoVM();
        //fnCapturaEnvase fncapturaEnvase = new fnCapturaEnvase();
        List<pedido> vListaPedido = new List<pedido>();
        List<pedido> vListaPedidoMod = new List<pedido>();

        public FrmPedidoSugerido()
        {
            InitializeComponent();

            MuestraPedidoSugerido();
        }

        #region Método para mostrar el envase correspondiente al cliente
        public async void MuestraPedidoSugerido()
        {
            try
            {
                int iCliente = VarEntorno.vCliente.cln_clave;
                vListaPedido = await Pedidosugerdiovm.ObtieneSugerido(iCliente);

                lsvArticulos.ItemsSource = vListaPedido;
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Método para mostrar el envase correspondiente al cliente

        #region Método de selección de un elemento de la lista para capturar un valor en el abono
        private async void OnSelectionEnvase(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var msg = (pedido)e.SelectedItem;
                string sMarca = msg.arc_clave.ToString();

                #region Se invoca el InputBox para capturar la cantidad para el elemento seleccionado
                var vCantidad = await InputBox(this.Navigation);
                int iAbono = Convert.ToInt32(vCantidad);
                #endregion Se invoca el InputBox para capturar la cantidad para el elemento seleccionado

                vListaPedidoMod = Pedidosugerdiovm.actualizandoAbono(sMarca, iAbono);

                lsvArticulos.ItemsSource = null;
                lsvArticulos.ItemsSource = vListaPedidoMod;
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Método de selección de un elemento de la lista para capturar un valor en el abono

        #region Método para crear el InputBox
        public static Task<string> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Capture un Valor", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Existencia:" };
            var txtInput = new Entry { Text = "", Keyboard = Keyboard.Numeric, Placeholder = "0" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;

                if (result == "")
                {
                    result = "0";
                }
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                var result = "0";
                await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(result);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
        #endregion Método para crear el InputBox

        #region Método para mostrar la descripción de algun Error en caso de que exista
        public async void muestraException(string sException)
        {
            await DisplayAlert("Aviso", sException, "Ok");
        }
        #endregion Método para mostrar la descripción de algun Error en caso de que exista

        #region Botón de Regresar
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion  Botón de Regresar

        #region Botón de Avanzar
        public async void OnClickedAvanzar(object sender, EventArgs args)
        {
            try
            {
                //Validación de Abonos Nulos
                /* string sValidaNulos = await fncapturaEnvase.validaAbonoNulos(vListaEnvaseMod);

                 if (sValidaNulos == "Ok")
                 {
                     //Aplica la captura a la tabla de Envase
                     string sRespuesta = await fncapturaEnvase.aplicarCaptura(vListaEnvaseMod);

                     if (sRespuesta == "Captura Guardada Correctamente")
                     {
                         Utilerias oUtilerias = new Utilerias();
                         oUtilerias.crearMensaje(sRespuesta);
                         this.Navigation.PopModalAsync();
                         await this.Navigation.PushModalAsync(new frmCobranza());
                     }
                     else
                     {
                         await DisplayAlert("Aviso", "Error al Guardar la Captura", "Ok");
                     }
                 }
                 else
                 {
                     await DisplayAlert("Aviso", sValidaNulos, "Ok");
                 }*/

                await this.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion  Botón de Avanzar
    }
}
