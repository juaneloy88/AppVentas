using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Servicio;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmEnvaseSugerido : ContentPage
	{
        vmCapturaEnvase vmcapturaEnvase = new vmCapturaEnvase();
        fnCapturaEnvase fncapturaEnvase = new fnCapturaEnvase();
        List<CapturaEnvase> vListaEnvase = new List<CapturaEnvase>();
        List<CapturaEnvase> vListaEnvaseMod = new List<CapturaEnvase>();        

        public frmEnvaseSugerido ()
		{
			InitializeComponent ();            

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            muestraEnvase();
        }

        #region Método para mostrar el envase correspondiente al cliente
        public async void muestraEnvase()
        {
            try
            {
                string sCliente = VarEntorno.vCliente.cln_clave.ToString();

                if (vmcapturaEnvase.existeCapturaEnvaseSugerido(sCliente)) //Validación para saber si ya existe una captura de envase para el cliente
                {                       
                    await DisplayAlert("Aviso", "Existe una Captura de Envase Sugerido para este Cliente", "Si");
                    bool bRespuesta = await DisplayAlert("Pregunta", "¿Desea BORRAR y volver a Capturar TODA la información?", "Si", "No");

                    if (bRespuesta)
                    {
                        if (vmcapturaEnvase.borrarEnvaseSugerido(sCliente))
                        {
                            DisplayAlert("Aviso", "Nuevamente Capture el Envase a Recoger", "Si");

                            vListaEnvase = await vmcapturaEnvase.obtieneEnvase(sCliente);

                            lsvEnvase.ItemsSource = null;
                            lsvEnvase.ItemsSource = vListaEnvase;
                        }
                        else
                        {
                            muestraException("No se pudo Borrar el Envase Sugerido del Cliente:" + sCliente);
                        }
                    }
                    else
                    {
                        this.Navigation.PopModalAsync();
                        this.Navigation.PopModalAsync();
                    }
                }
                else
                {
                    vListaEnvase = await vmcapturaEnvase.obtieneEnvase(sCliente);

                    lsvEnvase.ItemsSource = null;
                    lsvEnvase.ItemsSource = vListaEnvase;
                }
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
                var vCantidad = "";
                int iCapturaVacio = 0;                

                var msg = (CapturaEnvase)e.SelectedItem;
                string sMarca = msg.mec_envase.ToString();

                vCantidad = await InputBox(this.Navigation, "Capture un Valor para Envase", "VACIO");
                iCapturaVacio = Convert.ToInt32(vCantidad);

                vListaEnvaseMod = vmcapturaEnvase.actualizaCapturaVacioLleno(sMarca, iCapturaVacio, 0);

                lsvEnvase.ItemsSource = null;
                lsvEnvase.ItemsSource = vListaEnvaseMod;

            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Método de selección de un elemento de la lista para capturar un valor en el abono

        #region Método para crear el InputBox
        public static Task<string> InputBox(INavigation navigation, string sTitulo, string sMensaje)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = sTitulo, HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = sMensaje, FontAttributes = FontAttributes.Bold };
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

                if (result == "." || result == "_" || result == "," || result == "-")
                {
                    result = "0";
                }
                else
                {
                    if (result == "")
                    {
                        result = "0";
                    }
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

        #region Botón de Regresar
        public async void OnClickedRegresar(object sender, EventArgs args)
        {
            bool bRespuesta = await DisplayAlert("Pregunta", "¿Desea regresar y borrar la informacion del ticket ?", "Si", "No");

            if (bRespuesta == true)
            {
                bRespuesta = await DisplayAlert("Pregunta", "¿Desea borrar la totalidad del ticket actual ?", "Si", "No");
                if (bRespuesta == true)
                {
                    this.Navigation.PopModalAsync();
                    this.Navigation.PopModalAsync();
                }
            }
            //this.Navigation.PushModalAsync(new frmMenuReparto());
        }
        #endregion  Botón de Regresar

        #region Botón de Avanzar
        public async void OnClickedAvanzar(object sender, EventArgs args)
        {
            string sEnvase = string.Empty;
            string sEnvaseVacio = string.Empty;
            string sMensaje = string.Empty;

            try
            {
                if (vListaEnvaseMod.Count == 0)
                    vListaEnvaseMod = vListaEnvase;

                //Aplica la captura a la tabla de Envase
                string sRespuesta = await fncapturaEnvase.GuardarEnvaseSugerido(vListaEnvaseMod);

                if (sRespuesta == "Captura Guardada Correctamente")
                {
                    VarEntorno.LimpiaVariables();
                    Utilerias oUtilerias = new Utilerias();
                    oUtilerias.crearMensaje(sRespuesta);
                    this.Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Aviso", "Error al Guardar la Captura", "Ok");
                }
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion  Botón de Avanzar

        #region Método para mostrar la descripción de algun Error en caso de que exista
        public async void muestraException(string sException)
        {
            await DisplayAlert("Aviso", sException, "Ok");
        }
        #endregion Método para mostrar la descripción de algun Error en caso de que exista
    }
}