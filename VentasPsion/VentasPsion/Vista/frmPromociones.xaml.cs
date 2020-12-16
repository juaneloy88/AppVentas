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
    public partial class frmPromociones : ContentPage
    {
        vmPromociones vmPromo = new vmPromociones();
        int nCliente = VarEntorno.vCliente.cln_clave;
        int iNumeroPromociones = 1;

        public frmPromociones()
        {
            InitializeComponent();
            txtRuta.Text = "Ruta:" + VarEntorno.iNoRuta.ToString();
            lblCantidad.Text = String.Format("{0:F0}", stepper.Value);

            //Método que carga el Picker con las promociones activas
            cargaPickerPromociones();
        }

        #region OnStepperValueChanged
         void OnStepperValueChanged(object sender, ValueChangedEventArgs args)
        {
            lblCantidad.Text = String.Format("{0:F0}", args.NewValue);
            iNumeroPromociones = Convert.ToInt32(lblCantidad.Text);

            #region Validación para conocer si la lista tiene datos

            if (vmPromo.lMuestraPromociones.Count >= 1)
            {
                cargaList();
            }
            #endregion Validación para conocer si la lista tiene datos
        }
        #endregion OnStepperValueChanged

        #region Carga el Picker con todas las promociones activas
        public async void cargaPickerPromociones()
        {
            //Se Obtiene la lista de las promociones
            List<promociones> promos = await vmPromo.buscaPromociones();

            //Si existen promociones, se agregan al Picker con el Id de la promoción y la descripción
            if (promos.Count >= 1)
            {
                foreach (var promo in promos)
                {
                    string sPromocion = promo.ppn_numero_promocion.ToString().Trim() + ".-" + promo.ppc_descripcion.ToString().Trim();
                    pckPromociones.Items.Add(sPromocion);
                }
            }
        }
        #endregion Carga el Picker con todas las promociones activas

        #region Método que carga los ListView
        public async void cargaList()
        {
            #region Se obtiene el ID de la promoción seleccionada
            string texto = pckPromociones.SelectedItem.ToString();
            int iPosicion = texto.IndexOf(".-");
            string sID = texto.Substring(0, iPosicion);
            #endregion Se obtiene el ID de la promoción seleccionada

            #region Llena los ListView de Venta y Regalo de acuerdo al Id de la promoción seleccionada multiplicada por el número de promociones
            var vClaves = await vmPromo.listaVentaRegalo(sID, iNumeroPromociones);
            lsvCodigosVenta.ItemsSource = null;
            lsvCodigosVenta.ItemsSource = vClaves;

            lsvCodigosRegalo.ItemsSource = null; 
            lsvCodigosRegalo.ItemsSource = vClaves.FindAll(x => x.arn_cantidad_regalo > 0);
            #endregion Llena los ListView de Venta y Regalo de acuerdo al Id de la promoción seleccionada multiplicada por el número de promociones
        }
        #endregion Método que carga los ListView

        #region Método que llena la lista al seleccionar una promoción
        private void pckPromociones_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaList();
        }
        #endregion Método que llena la lista al seleccionar una promoción        

        #region Método para la selección de un elemento de la lista de la parte de Venta
        private async void OnSelectionVenta(object sender, SelectedItemChangedEventArgs e)
        {
            #region Se obtienen valores del elemento seleccionado
            var msg = (MostrarPromociones)e.SelectedItem;
            string sID = msg.ppn_numero_promocion.ToString();
            string sClaveVenta = msg.arc_clave_venta.ToString();
            string sTipoPromo = msg.ptc_tipo.ToString();
            #endregion Se obtienen valores del elemento seleccionado

            if (sTipoPromo == "N")
            {
                await DisplayAlert("Promoción NO Dinámica", "En esta Promo no se pueden Modificar cantidades", "Ok");
            }
            else
            {
                #region Se invoca el InputBox para capturar la cantidad para el elemento seleccionado
                var vCantidad = await InputBox(this.Navigation);
                string sCantidad = vCantidad.ToString();
                #endregion Se invoca el InputBox para capturar la cantidad para el elemento seleccionado

                #region Método para realizar el update con la cantidad capturada para el campo de venta
                var claves = await vmPromo.detallePromocionesVentaUpdate(sClaveVenta, sCantidad);
                lsvCodigosVenta.ItemsSource = null;
                lsvCodigosVenta.ItemsSource = claves;
                #endregion Método para realizar el update con la cantidad capturada para el campo de venta
            }
        }
        #endregion Método para la selección de un elemento de la lista de la parte de Venta

        #region Método para la selección de un elemento de la lista de la parte de Obsequio
        private async void OnSelectionRegalo(object sender, SelectedItemChangedEventArgs e)
        {
            #region Se obtienen valores del elemento seleccionado
            var msg = (MostrarPromociones)e.SelectedItem;
            string sID = msg.ppn_numero_promocion.ToString();
            string sClaveRegalo = msg.arc_clave_regalo.ToString();
            string sTipoPromo = msg.ptc_tipo.ToString();
            #endregion Se obtienen valores del elemento seleccionado

            if (sTipoPromo == "N")
            {
                await DisplayAlert("Promoción NO Dinámica", "En esta Promo no se pueden Modificar cantidades", "Ok");
            }
            else
            {
                #region Se invoca el InputBox para capturar la cantidad para el elemento seleccionado
                var vCantidad = await InputBox(this.Navigation);
                string sCantidad = vCantidad.ToString();
                #endregion Se invoca el InputBox para capturar la cantidad para el elemento seleccionado

                #region Método para realizar el update con la cantidad capturada para el campo de regalo
                var claves = await vmPromo.detallePromocionesRegaloUpdate(sClaveRegalo, sCantidad);
                lsvCodigosRegalo.ItemsSource = null;
                lsvCodigosRegalo.ItemsSource = claves;
                #endregion Método para realizar el update con la cantidad capturada para el campo de regalo
            }
        }
        #endregion Método para la selección de un elemento de la lista de la parte de Obsequio

        #region Método para crear el InputBox
        public static Task<string> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Capture un Valor", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Cantidad:" };
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
                if (Convert.ToInt16(result) <0)
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

        #region Botón del Previo
        private async void btnPrevio(object sender, EventArgs e)
        {
            var vListaPromo = vmPromo.lMuestraPromociones;            

            if (vListaPromo.Count >= 1)
            {
                if (vListaPromo[0].ptc_tipo == "N")
                {
                    var vListaPrevioPromo = await vmPromo.previoPromociones();
                    this.Navigation.PopModalAsync();
                    await Navigation.PushModalAsync(new frmPrevioPromociones(vListaPrevioPromo, vListaPromo));
                }
                else
                {
                    #region Se obtiene el ID de la promoción seleccionada
                    string texto = pckPromociones.SelectedItem.ToString();
                    int iPosicion = texto.IndexOf(".-");
                    string sID = texto.Substring(0, iPosicion);
                    iNumeroPromociones = Convert.ToInt32(lblCantidad.Text);
                    #endregion Se obtiene el ID de la promoción seleccionada

                    string sRespuesta = await vmPromo.validacionPromo(sID, iNumeroPromociones);

                    if (sRespuesta == "Ok")
                    {
                        var vListaPrevioPromo = await vmPromo.previoPromociones();
                        this.Navigation.PopModalAsync();
                        await Navigation.PushModalAsync(new frmPrevioPromociones(vListaPrevioPromo, vListaPromo));
                    }
                    else
                        await DisplayAlert("Captura NO Válida", sRespuesta, "Ok");
                }                           
            }
            else            
                await DisplayAlert("Seleccione una Promoción", "Promoción NO Seleccionada", "Ok");
            
        }
        #endregion Botón del Previo

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
    }
}