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
    public partial class frmCapturaEnvase : ContentPage
    {
        vmCapturaEnvase vmcapturaEnvase = new vmCapturaEnvase();
        fnCapturaEnvase fncapturaEnvase = new fnCapturaEnvase();
        List<CapturaEnvase> vListaEnvase = new List<CapturaEnvase>();
        List<CapturaEnvase> vListaEnvaseMod = new List<CapturaEnvase>();

        string sComentario = string.Empty;

        public frmCapturaEnvase()
        {
            InitializeComponent();

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
                vListaEnvase = await vmcapturaEnvase.obtieneEnvase(sCliente);

                lsvEnvase.ItemsSource = null;
                lsvEnvase.ItemsSource = vListaEnvase;                
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
                //if (fncapturaEnvase.FnValidaExisteEnvSugerido())
                //{
                //    var vComentario = await InputBox(this.Navigation, true);
                //    sComentario = vComentario.ToString().ToUpper();

                //    if (String.IsNullOrEmpty(sComentario))
                //        await DisplayAlert("Aviso", "Debe de colocar un comentario de porque no se va a recoger el envase", "Ok");
                //}
                //else
                {
                    var msg = (CapturaEnvase)e.SelectedItem;
                    string sMarca = msg.mec_envase.ToString();

                    var vCantidad = await InputBox(this.Navigation, false);
                    int iAbono = Convert.ToInt32(vCantidad);

                    vListaEnvaseMod = vmcapturaEnvase.actualizandoAbono(sMarca, iAbono);

                    lsvEnvase.ItemsSource = null;
                    lsvEnvase.ItemsSource = vListaEnvaseMod;
                }                
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Método de selección de un elemento de la lista para capturar un valor en el abono

        #region Método para crear el InputBox
        public static Task<string> InputBox(INavigation navigation, bool bBandera)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Capture un Valor", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Abono:" };
            var txtInput = new Entry { Text = "", Keyboard = Keyboard.Numeric, Placeholder = "0" };

            if (bBandera)
            {
                lblTitle = new Label { Text = "Capture porque NO se va a Recoger el Envase", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
                lblMessage = new Label { Text = "Comentario:" };
                txtInput = new Entry { Text = "", Placeholder = "" };
            }

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

                if (bBandera == false)
                    if (0>Convert.ToInt16(result))
                        result = "0";

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
                //Validación de Abonos Nulos
                if (vListaEnvaseMod.Count == 0)
                    vListaEnvaseMod = vListaEnvase;

                string sValidaNulos = sValidaNulos = await fncapturaEnvase.validaAbonoNulos(vListaEnvaseMod);

                if (sValidaNulos == "Ok")
                {
                    //Aplica la captura a la tabla de Envase                    
                    string sRespuesta = await fncapturaEnvase.aplicarCaptura(vListaEnvaseMod, sComentario);

                    if (sRespuesta == "Captura Guardada Correctamente")
                    {
                        //if (fncapturaEnvase.FnValidaExisteEnvSugerido())
                        //{
                        //    if (String.IsNullOrEmpty(sComentario))
                        //    {
                        //        foreach (var item in vListaEnvaseMod)
                        //        {
                        //            sEnvase = item.mec_envase;
                        //            sEnvaseVacio = item.esn_cantidad_vacio.ToString();

                        //            if (sEnvaseVacio != "0")
                        //                sMensaje = sMensaje + " " + sEnvase + " - " + sEnvaseVacio;
                        //        }

                        //        await DisplayAlert("Aviso", "El envase Recolectado es: " + sMensaje, "Ok");
                        //    }
                        //}

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
                    if (sValidaNulos == "")
                        sValidaNulos = "Capture abono de envase ";
                    await DisplayAlert("Aviso", sValidaNulos, "Ok");
                }
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion  Botón de Avanzar
    }
}