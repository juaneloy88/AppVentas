using Base;
using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Org.Json;
using System.Threading.Tasks;
using VentasPsion.Modelo.Servicio;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmEstatusCliente : ContentPage
    {
        vmStatusClientes vmStatusCtes = new vmStatusClientes();
        fnStatusClientes fnStatusCtes = new fnStatusClientes();
        List<StatusClientes> lLista = new List<StatusClientes>();

        string scliente = "";
        string sException = string.Empty;

        Utilerias oUtilerias;


        public frmEstatusCliente()
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;            

            #region Habilita o deshabilita botones de acuerdo al perfil de la ruta
            switch (VarEntorno.cTipoVenta)
            {
                case 'R':
                    btnPagos.Text = "Devol";
                    btnBorrarVta.IsEnabled = false;
                    btnBorrarDev.IsEnabled = false;
                    break;
                case 'P':
                    btnReImpDev.IsEnabled = false;
                    btnBorrarDev.IsEnabled = false;
                    break;
                case 'A':
                    btnReImpDev.IsEnabled = true;
                    btnBorrarDev.IsEnabled = false;
                    btnBorrarVta.IsEnabled = false;
                    break;
            }
            #endregion Habilita o deshabilita botones de acuerdo al perfil de la ruta

            //Carga el Listado de Clientes con sus estatus correspondientes
            cargaClientesStatus();
        }

        #region Método que carga los clientes del día
        public async void cargaClientesStatus()
        {
            try
            {
                scliente = "";

                lLista = await vmStatusCtes.obtieneStatusClientes();
                txtTotClientes.Text = "Clientes:" + lLista.Count.ToString();

                var vTotalClientesVentaPago = fnStatusCtes.obtieneClientesVisitados(lLista, 1);
                txtTotVisitados.Text = "Vta/Pgo:" + vTotalClientesVentaPago.Count.ToString();

                var vTotalClientesNoVisitados = fnStatusCtes.obtieneClientesVisitados(lLista, 0);
                txtTotNoVisitados.Text = "NoVisita:" + vTotalClientesNoVisitados.Count.ToString();

                if (VarEntorno.cTipoVenta == 'R')
                {
                    var vTotClientesDevol = fnStatusCtes.obtieneClientesVisitados(lLista, 3);
                    txtTotPagos.Text = "Devol:" + vTotClientesDevol.Count.ToString();
                }
                else
                {
                    var vTotClientesPagos = fnStatusCtes.obtieneClientesVisitados(lLista, 4);
                    txtTotPagos.Text = "Visitados:" + vTotClientesPagos.Count.ToString();
                }                

                lsvStatusClientes.ItemsSource = lLista;
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Método que carga los clientes del día        

        #region Botón para filtrar los clientes Visitados
        private void OnClickedVentaPagos(object sender, EventArgs e)
        {
            try
            {
                var vTotalClientesVistados = fnStatusCtes.obtieneClientesVisitados(lLista, 1);
                lsvStatusClientes.ItemsSource = vTotalClientesVistados;
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Botón para filtrar los clientes Visitados

        #region Botón para filtrar los clientes No Visitados 
        private void OnClickedNoVisitados(object sender, EventArgs e)
        {
            try
            {
                var vTotalClientesNoVisitados = fnStatusCtes.obtieneClientesVisitados(lLista, 0);
                lsvStatusClientes.ItemsSource = vTotalClientesNoVisitados;
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Botón para filtrar los clientes No Visitados 

        //#region Botón para filtrar los clientes con Pago
        //private void OnClickedPagos(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var vTotClientesPagos = fnStatusCtes.obtieneClientesVisitados(lLista, 2);
        //        lsvStatusClientes.ItemsSource = vTotClientesPagos;
        //    }
        //    catch (Exception ex)
        //    {
        //        muestraException(ex.ToString());
        //    }

        //}
        //#endregion Botón para filtrar los clientes con Pago

        #region Botón para filtrar los clientes Visitados
        private void OnClickedVisitados(object sender, EventArgs e)
        {
            try
            {
                if (VarEntorno.cTipoVenta == 'R')
                {
                    var vTotClientesDevol = fnStatusCtes.obtieneClientesVisitados(lLista, 3);                    
                    lsvStatusClientes.ItemsSource = vTotClientesDevol;
                }
                else
                {
                    var vTotClientesPagos = fnStatusCtes.obtieneClientesVisitados(lLista, 4);
                    lsvStatusClientes.ItemsSource = vTotClientesPagos;
                }
                
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }

        }
        #endregion Botón para filtrar los clientes con Pago

        #region Botón de busqueda por Id del Cliente
        private async void OnClickedBuscarCte(object sender, EventArgs e)
        {
            try
            {
                #region Se invoca el InputBox para capturar el ID del Cliente
                var vIdCte = await InputBox(this.Navigation);
                string sCliente = vIdCte.ToString();
                #endregion Se invoca el InputBox para capturar el ID del Cliente

                if (sCliente == "0")
                {
                    //Carga el Listado de Clientes con sus estatus correspondientes
                    cargaClientesStatus();
                }
                else
                {
                    var vClientesID = fnStatusCtes.obtieneClientesId(lLista, sCliente);
                    lsvStatusClientes.ItemsSource = vClientesID;
                }
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Botón de busqueda por Id del Cliente

        #region Método para crear el InputBox
        public static Task<string> InputBox(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Capture un ID", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Cliente:" };
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

        #region Botón de regresar
        private void OnClickedRegresar(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de regresar

        #region Botón de Liberar Ruta para Borrar Tickets
        private async void OnClickedLiberar(object sender, EventArgs e)
        {
            try
            {
                var vPassword = await InputBoxLiberarRuta(this.Navigation);
                string sPassword = vPassword.ToString();
                string sRespuesta = await vmStatusCtes.liberaReparto(sPassword);

                if (sRespuesta == "Ruta Liberada")
                {
                    if (VarEntorno.cTipoVenta == 'A')
                    {
                        btnBorrarVta.IsEnabled = true;
                        btnBorrarDev.IsEnabled = true;
                    }
                    else
                    {
                        btnBorrarVta.IsEnabled = true;
                        btnBorrarDev.IsEnabled = true;
                    }                    

                    Utilerias oUtilerias = new Utilerias();
                    oUtilerias.crearMensaje(sRespuesta);
                    
                }
                else
                {
                    await DisplayAlert("Alerta", sRespuesta, "Ok");
                }
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Botón de Liberar Ruta para Borrar Tickets

        #region Método para crear el InputBox de Liberación de Ruta
        public static Task<string> InputBoxLiberarRuta(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Capture un Valor", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Contraseña:" };
            var txtInput = new Entry { Text = "", Keyboard = Keyboard.Numeric, IsPassword = true, Placeholder = "0" };

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
        #endregion Método para crear el InputBox de Liberación de Ruta

        #region Método para mostrar la descripción de algun Error en caso de que exista
        public async void muestraException(string sException)
        {
            await DisplayAlert("Aviso", sException, "Ok");
        }
        #endregion Método para mostrar la descripción de algun Error en caso de que exista

        #region Botón de Borrar Venta(Preventa y Autoventa) / Borrar Entrega (Reparto)
        private async void OnClickedBorrarVenta(object sender, EventArgs e)
        {
            string sRespuesta = string.Empty;

            try
            {
                if (scliente == "")
                {
                    await DisplayAlert("Aviso", "Seleccione un Cliente de la Lista", "Ok");
                }
                else
                {
                    if (VarEntorno.cTipoVenta == 'R')
                    {
                        sRespuesta = await vmStatusCtes.borrarEntrega(scliente);
                    }
                    else
                    {
                        sRespuesta = await vmStatusCtes.borrarVenta(scliente);
                    }
/*
                    oUtilerias = new Utilerias();
                    //oUtilerias.obtenerCoordenadas(0);
                     //Task<JSONObject> oJSONObject = oUtilerias.obtenerCoordenadas(100);
                     JSONObject oResultTask = oJSONObject.Result;
                     fnGPS oFnGPS = new fnGPS();
                     oFnGPS.fnGuardarGPSCliente(VarEntorno.vCliente, oResultTask.OptString("Latitude"), oResultTask.OptString("Longitude"), 0);
                     */
                    await DisplayAlert("Aviso", sRespuesta, "Ok");

                    //Carga el Listado de Clientes con sus estatus correspondientes
                    cargaClientesStatus();
                }
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Botón de Borrar Venta

        #region Botón para Borrar la Devolución
        private async void OnClickedBorrarDevol(object sender, EventArgs e)
        {
            try
            {
                if (scliente == "")
                {
                    await DisplayAlert("Aviso", "Seleccione un Cliente de la Lista", "Ok");
                }
                else
                {
                    string sRespuesta = await vmStatusCtes.borrarDevol(scliente);

/*
                    oUtilerias = new Utilerias();
                    //oUtilerias.obtenerCoordenadas(0);
                    // Task<JSONObject> oJSONObject = oUtilerias.obtenerCoordenadas(100);
                     JSONObject oResultTask = oJSONObject.Result;
                     fnGPS oFnGPS = new fnGPS();
                     oFnGPS.fnGuardarGPSCliente(VarEntorno.vCliente, oResultTask.OptString("Latitude"), oResultTask.OptString("Longitude"), 0);
                     */
                    await DisplayAlert("Aviso", sRespuesta, "Ok");

                    //Carga el Listado de Clientes con sus estatus correspondientes
                    cargaClientesStatus();
                }
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Botón para Borrar la Devolución

        #region Método para obtener el Id del cliente seleccionado de la lista 
        private void OnSelectionCliente(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var vVar = (StatusClientes)e.SelectedItem;
                scliente = vVar.sCliente;
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Método para obtener el Id del cliente seleccionado de la lista 

        private void OnClickedReimprimirVenta(object sender, EventArgs e)
        {
            if (scliente != "")
            {
                //List<venta_cabecera> sFolios = null;
               
                switch (VarEntorno.cTipoVenta)
                {
                    case 'R':
                        vmStatusCtes.sFolios = new fnVentaCabecera().VentasCabeceras(Convert.ToInt32(scliente),"$");

                        if (vmStatusCtes.sFolios.Count> 0)
                            foreach (var folio in vmStatusCtes.sFolios)
                            {
                                if (folio.vcn_importe > 0)
                                {
                                    #region Imprime el Ticket de Entrega de Reparto
                                    if (VarEntorno.sTipoImpresora == "Zebra")
                                    {
                                        ZeImprimeEntregaRVM ticketEntrega = new ZeImprimeEntregaRVM();
                                        ticketEntrega.FtnImprimirTicketEntregaR(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    else
                                    {
                                        ImprimeEntregaRVM ticketEntrega = new ImprimeEntregaRVM();
                                        ticketEntrega.FtnImprimirTicketEntregaR(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Imprime el Ticket de Pago en Reparto
                                    if (VarEntorno.sTipoImpresora == "Zebra")
                                    {
                                        ZeImprimePagoARPVM ticketPagor = new ZeImprimePagoARPVM();
                                        ticketPagor.FtnImprimirTicketPagoARP(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    else
                                    {
                                        ImprimePagoARPVM ticketPagor = new ImprimePagoARPVM();
                                        ticketPagor.FtnImprimirTicketPagoARP(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    #endregion
                                }
                            }                        
                        break;
                    default:
                        vmStatusCtes.sFolios = new fnVentaCabecera().VentasCabeceras(Convert.ToInt32(scliente),"$");

                        if (vmStatusCtes.sFolios.Count > 0)                        
                            foreach (var folio in vmStatusCtes.sFolios)
                            {
                                if (folio.vcn_importe > 0)
                                {
                                    #region Imprime el Ticket de Ventas en Preventa y Autoventa
                                    if (VarEntorno.sTipoImpresora == "Zebra")
                                    {
                                        ZeImprimeVentaAPVM ticket = new ZeImprimeVentaAPVM();
                                        ticket.FtnImprimirTicketVentaAP(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    else
                                    {
                                        ImprimeVentaAPVM ticket = new ImprimeVentaAPVM();
                                        ticket.FtnImprimirTicketVentaAP(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region Imprime el Ticket de Pago en Preventa y Autoventa
                                    if (VarEntorno.sTipoImpresora == "Zebra")
                                    {
                                        ZeImprimePagoARPVM ticketPago = new ZeImprimePagoARPVM();
                                        ticketPago.FtnImprimirTicketPagoARP(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    else
                                    {
                                       
                                        ImprimePagoARPVM ticketPago = new ImprimePagoARPVM();
                                        ticketPago.FtnImprimirTicketPagoARP(Convert.ToInt32(scliente), folio.vcn_folio);
                                    }
                                    #endregion
                                }
                            }     
                        break;
                }
            }
        }

        private void OnClickedReimprimirDevolucion(object sender, EventArgs e)
        {
            if (scliente != "")
            {
                //List<venta_cabecera> sFolios = null;
                string sDevolucion;
                vmStatusCtes.sFolios = new fnVentaCabecera().VentasCabeceras(Convert.ToInt32(scliente), "D");

                switch (VarEntorno.cTipoVenta)
                {
                    case 'R':                        
                        sDevolucion = new conseptoDevolucionesSR().ConseptoDevolucion(Convert.ToInt32(scliente));
                        if (vmStatusCtes.sFolios.Count > 0)
                        {

                            #region Imprime el Ticket de Devolucion de Reparto
                            if (VarEntorno.sTipoImpresora == "Zebra")
                            {
                                ZeImprimeDevolucionRVM ticketDevolucion = new ZeImprimeDevolucionRVM();
                                ticketDevolucion.FtnImprimirTicketDevolucionR(Convert.ToInt32(scliente), vmStatusCtes.sFolios[0].vcn_folio, sDevolucion);
                            }
                            else
                            {
                                ImprimeDevolucionRVM ticketDevolucion = new ImprimeDevolucionRVM();
                                ticketDevolucion.FtnImprimirTicketDevolucionR(Convert.ToInt32(scliente), vmStatusCtes.sFolios[0].vcn_folio, sDevolucion);
                            }
                            #endregion
                        }
                        break;
                    default:                       
                        sDevolucion = "";
                        if (vmStatusCtes.sFolios.Count > 0)
                        {
                            #region Imprime el Ticket de Devolucion de Reparto
                            foreach (var folio in vmStatusCtes.sFolios)
                            {
                                if (VarEntorno.sTipoImpresora == "Zebra")
                                {
                                    ZeImprimeDevolucionRVM ticketDevolucion = new ZeImprimeDevolucionRVM();
                                    ticketDevolucion.FtnImprimirTicketDevolucionR(Convert.ToInt32(scliente), vmStatusCtes.sFolios[0].vcn_folio, sDevolucion);
                                }
                                else
                                {
                                    ImprimeDevolucionRVM ticketDevolucion = new ImprimeDevolucionRVM();
                                    ticketDevolucion.FtnImprimirTicketDevolucionR(Convert.ToInt32(scliente), vmStatusCtes.sFolios[0].vcn_folio, sDevolucion);
                                }
                            }
                            #endregion
                        }
                        break;
                }

            }
        }
    }
}