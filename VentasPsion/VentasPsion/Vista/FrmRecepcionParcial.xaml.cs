using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.ServicioApi;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FrmRecepcionParcial : ContentPage
	{
        #region Declaración de Labels para Status de Recepción Parcial
        Label lblStatusPermisosApp, lblTituloPermisosApp = null;
        Label lblStatusClientes, lblTituloClientes = null;
        Label lblStatusEnvases, lblTituloEnvases = null;
        Label lblStatusListasPrecios, lblTituloListasPrecios = null;
        Label lblStatusBonificaciones, lblTituloBonificaciones = null;
        Label lblStatusPedidosSugeridos, lblTituloPedidosSugeridos = null;
        //Label lblStatusComplementos, lblTituloComplementos = null;
        Label lblStatusDocumentos, lblTituloDocumentos = null;
        Label lblStatusTelefonosClientes, lblTituloTelefonosClientes = null;

        Label lblStatusCandadosProductos, lblTituloCandadosProductos = null;
        Label lblStatusPromociones, lblTituloPromociones = null;
        Label lblStatusProductos, lblTituloProductos = null;
        Label lblStatusExistencias, lblTituloExistencias = null;
        #endregion

        public FrmRecepcionParcial ()
		{
			InitializeComponent ();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            lblRuta.Text = VarEntorno.iNoRuta.ToString();
            

            #region Inicialización de Labels con sus propiedades para el Status de Recepción Parcial
            lblStatusPermisosApp = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloPermisosApp = new Label { Text = "Permisos de App", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloClientes = new Label { Text = "Clientes", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusEnvases = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloEnvases = new Label { Text = "Envases", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusListasPrecios = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloListasPrecios = new Label { Text = "Listas de Precios", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusBonificaciones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloBonificaciones = new Label { Text = "Bonificaciones", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusPedidosSugeridos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloPedidosSugeridos = new Label { Text = "Pedidos Sugeridos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            //lblStatusComplementos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            //lblTituloComplementos = new Label { Text = "Complementos de Pago", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusDocumentos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloDocumentos = new Label { Text = "Documentos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusTelefonosClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloTelefonosClientes = new Label { Text = "Telefonos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusCandadosProductos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloCandadosProductos = new Label { Text = "Candados de Productos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusPromociones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloPromociones = new Label { Text = "Promociones", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusProductos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloProductos = new Label { Text = "Productos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusExistencias = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloExistencias = new Label { Text = "Existencias", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            #endregion

            #region Agrega los Labels para el Status de Recepción Parcial al Grid, según el Tipo de Venta o Perfil (AUTOVENTA, REPARTO ó PREVENTA)
            switch (VarEntorno.cTipoVenta)
            {
                case 'A': /*AUTOVENTA*/
                    grdRecepcion.Children.Add(lblStatusPermisosApp, 1, 2);
                    grdRecepcion.Children.Add(lblTituloPermisosApp, 2, 2);
                    Grid.SetColumnSpan(lblTituloPermisosApp, 2);
                    grdRecepcion.Children.Add(lblStatusClientes, 1, 3);
                    grdRecepcion.Children.Add(lblTituloClientes, 2, 3);
                    Grid.SetColumnSpan(lblTituloClientes, 2);
                    grdRecepcion.Children.Add(lblStatusEnvases, 1, 4);
                    grdRecepcion.Children.Add(lblTituloEnvases, 2, 4);
                    Grid.SetColumnSpan(lblTituloEnvases, 2);
                    
                    grdRecepcion.Children.Add(lblStatusBonificaciones, 1, 5);
                    grdRecepcion.Children.Add(lblTituloBonificaciones, 2, 5);
                    Grid.SetColumnSpan(lblTituloBonificaciones, 2);
                    grdRecepcion.Children.Add(lblStatusPedidosSugeridos, 1, 6);
                    grdRecepcion.Children.Add(lblTituloPedidosSugeridos, 2, 6);
                    Grid.SetColumnSpan(lblTituloPedidosSugeridos, 2);
                    //grdRecepcion.Children.Add(lblStatusComplementos, 1, 8);
                    //grdRecepcion.Children.Add(lblTituloComplementos, 2, 8);
                    //Grid.SetColumnSpan(lblTituloComplementos, 2);
                    grdRecepcion.Children.Add(lblStatusDocumentos, 1, 7);
                    grdRecepcion.Children.Add(lblTituloDocumentos, 2, 7);
                    Grid.SetColumnSpan(lblTituloDocumentos, 2);
                    grdRecepcion.Children.Add(lblStatusTelefonosClientes, 1, 8);
                    grdRecepcion.Children.Add(lblTituloTelefonosClientes, 2, 8);
                    Grid.SetColumnSpan(lblTituloTelefonosClientes, 2);

                    grdRecepcion.Children.Add(lblStatusListasPrecios, 1, 9);
                    grdRecepcion.Children.Add(lblTituloListasPrecios, 2, 9);
                    Grid.SetColumnSpan(lblTituloListasPrecios, 2);

                    grdRecepcion.Children.Add(lblStatusCandadosProductos, 1, 10);
                    grdRecepcion.Children.Add(lblTituloCandadosProductos, 2, 10);
                    Grid.SetColumnSpan(lblTituloCandadosProductos, 2);
                    grdRecepcion.Children.Add(lblStatusPromociones, 1, 11);
                    grdRecepcion.Children.Add(lblTituloPromociones, 2, 11);
                    Grid.SetColumnSpan(lblTituloPromociones, 2);
                    grdRecepcion.Children.Add(lblStatusProductos, 1, 12);
                    grdRecepcion.Children.Add(lblTituloProductos, 2, 12);
                    Grid.SetColumnSpan(lblTituloProductos, 2);
                    grdRecepcion.Children.Add(lblStatusExistencias, 1, 13);
                    grdRecepcion.Children.Add(lblTituloExistencias, 2, 13);
                    Grid.SetColumnSpan(lblTituloExistencias, 2);
                    break;
                case 'R': /*REPARTO*/
                    grdRecepcion.Children.Add(lblStatusPermisosApp, 1, 2);
                    grdRecepcion.Children.Add(lblTituloPermisosApp, 2, 2);
                    Grid.SetColumnSpan(lblTituloPermisosApp, 2);
                    grdRecepcion.Children.Add(lblStatusClientes, 1, 3);
                    grdRecepcion.Children.Add(lblTituloClientes, 2, 3);
                    Grid.SetColumnSpan(lblTituloClientes, 2);
                    break;
                case 'P': /*PREVENTA*/
                    grdRecepcion.Children.Add(lblStatusPermisosApp, 1, 2);
                    grdRecepcion.Children.Add(lblTituloPermisosApp, 2, 2);
                    Grid.SetColumnSpan(lblTituloPermisosApp, 2);
                    grdRecepcion.Children.Add(lblStatusClientes, 1, 3);
                    grdRecepcion.Children.Add(lblTituloClientes, 2, 3);
                    Grid.SetColumnSpan(lblTituloClientes, 2);
                    grdRecepcion.Children.Add(lblStatusEnvases, 1, 4);
                    grdRecepcion.Children.Add(lblTituloEnvases, 2, 4);
                    Grid.SetColumnSpan(lblTituloEnvases, 2);                    
                    grdRecepcion.Children.Add(lblStatusBonificaciones, 1, 5);
                    grdRecepcion.Children.Add(lblTituloBonificaciones, 2, 5);
                    Grid.SetColumnSpan(lblTituloBonificaciones, 2);
                    grdRecepcion.Children.Add(lblStatusPedidosSugeridos, 1, 6);
                    grdRecepcion.Children.Add(lblTituloPedidosSugeridos, 2, 6);
                    Grid.SetColumnSpan(lblTituloPedidosSugeridos, 2);
                    //grdRecepcion.Children.Add(lblStatusComplementos, 1, 8);
                    //grdRecepcion.Children.Add(lblTituloComplementos, 2, 8);
                    //Grid.SetColumnSpan(lblTituloComplementos, 2);
                    grdRecepcion.Children.Add(lblStatusDocumentos, 1, 7);
                    grdRecepcion.Children.Add(lblTituloDocumentos, 2, 7);
                    Grid.SetColumnSpan(lblTituloDocumentos, 2);
                    grdRecepcion.Children.Add(lblStatusTelefonosClientes, 1, 8);
                    grdRecepcion.Children.Add(lblTituloTelefonosClientes, 2, 8);
                    Grid.SetColumnSpan(lblTituloTelefonosClientes, 2);

                    grdRecepcion.Children.Add(lblStatusListasPrecios, 1, 9);
                    grdRecepcion.Children.Add(lblTituloListasPrecios, 2, 9);
                    Grid.SetColumnSpan(lblTituloListasPrecios, 2);

                    grdRecepcion.Children.Add(lblStatusCandadosProductos, 1, 10);
                    grdRecepcion.Children.Add(lblTituloCandadosProductos, 2, 10);
                    Grid.SetColumnSpan(lblTituloCandadosProductos, 2);
                    grdRecepcion.Children.Add(lblStatusPromociones, 1, 11);
                    grdRecepcion.Children.Add(lblTituloPromociones, 2, 11);
                    Grid.SetColumnSpan(lblTituloPromociones, 2);
                    grdRecepcion.Children.Add(lblStatusProductos, 1, 12);
                    grdRecepcion.Children.Add(lblTituloProductos, 2, 12);
                    Grid.SetColumnSpan(lblTituloProductos, 2);
                    grdRecepcion.Children.Add(lblStatusExistencias, 1, 13);
                    grdRecepcion.Children.Add(lblTituloExistencias, 2, 13);
                    Grid.SetColumnSpan(lblTituloExistencias, 2);
                    break;
                default:
                    break;
            }
            #endregion
        }

        /*Método para realizar la Recepción Parcial de datos de una Ruta específica de AUTOVENTA, REPARTO ó PREVENTA*/
        private async Task btnRecepcion_Clicked(object sender, EventArgs e)
        {
            string sIdRuta = lblRuta.Text;
            bool bClientes = false;

            FtnLimpiarStatusRecepcion();

            if (String.IsNullOrEmpty(sIdRuta))
            {
                await DisplayAlert("Aviso", "Ha olvidado capturar la RUTA.", "OK");
            }
            else
            {
                if (sIdRuta == ".")
                {
                    await DisplayAlert("Aviso", "Ha capturado un punto ( . ) en lugar de una RUTA.", "OK");
                }
                else
                {
                    int iIdRuta;
                    if (!int.TryParse(sIdRuta, out iIdRuta))
                    {
                        await DisplayAlert("Aviso", "No es válido un decimal en la RUTA.", "OK");
                    }
                    else
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

                            Utilerias utilerias = new Utilerias();
                            var progressDialogRecepcion = utilerias.crearProgressDialog("Recepción Parcial por " + sConexionTipo, "Cargando información...");
                            progressDialogRecepcion.Show();

                            await Task.Run(() =>
                            {
                                DateTime dFechaReparto = new DateTime();

                                /*Carga datos de los PERMISOS de la App de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                   (VarEntorno.cTipoVenta == 'R') ||
                                   (VarEntorno.cTipoVenta == 'P'))
                                {
                                    PermisosAppRestServiceARP permisosAppApi = new PermisosAppRestServiceARP();
                                    StatusRestService statusPermisosAppApi = new StatusRestService();
                                    statusPermisosAppApi = permisosAppApi.FtnCargarPermisosAppARP(VarEntorno.cTipoVenta, sConexionUri);

                                    if (statusPermisosAppApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPermisosApp.Text = "OK";
                                            lblStatusPermisosApp.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPermisosApp.Text = "ERROR";
                                            lblStatusPermisosApp.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusPermisosAppApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los CLIENTES de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ClientesRestServiceAP clientesApi = new ClientesRestServiceAP();
                                    StatusRestService statusClientesApi = new StatusRestService();
                                    statusClientesApi = clientesApi.FtnCargarClientes(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL", VarEntorno.bEsTeleventa);

                                    if (statusClientesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            /*Valida que si no hay nuevos Clientes lo especifique durante la Recepción Parcial*/
                                            if (statusClientesApi.valor == "0")
                                            {
                                                lblStatusClientes.Text = "OK";
                                                //lblStatusClientes.TextColor = Color.Red;
                                                lblStatusClientes.TextColor = Color.LimeGreen;
                                                //lblTituloClientes.Text = "Clientes   (No hay nuevos clientes)";
                                                lblTituloClientes.Text = "Clientes   (No hay nuevos pero se actualizaron las Condiciones de Crédito de los existentes)";
                                            }
                                            else
                                            {
                                                lblStatusClientes.Text = "OK";
                                                lblStatusClientes.TextColor = Color.LimeGreen;
                                                lblTituloClientes.Text = "Clientes   (Nuevos clientes:  " + statusClientesApi.valor + ")";
                                            }
                                        });

                                        if (statusClientesApi.valor == "0")
                                        {
                                            bClientes = false;
                                        //    progressDialogRecepcion.Dismiss();
                                        //    return;
                                        }
                                        else
                                            bClientes = true;
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientes.Text = "ERROR";
                                            lblStatusClientes.TextColor = Color.Red;
                                            lblTituloClientes.Text = "Clientes";
                                            DisplayAlert("Aviso", statusClientesApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los CLIENTES de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    FechaRepartoRestServiceR fechaRepartoApi = new FechaRepartoRestServiceR();
                                    StatusRestService statusFechaRepartoApi = new StatusRestService();
                                    statusFechaRepartoApi = fechaRepartoApi.FtnObtenerFechaReparto(VarEntorno.dFechaVenta, sConexionUri);

                                    if (statusFechaRepartoApi.status == true)
                                    {
                                        dFechaReparto = statusFechaRepartoApi.fecha;
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientes.Text = "ERROR";
                                            lblStatusClientes.TextColor = Color.Red;
                                            lblTituloClientes.Text = "Clientes";
                                            DisplayAlert("Aviso", statusFechaRepartoApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    ClientesRestServiceR clientesApi = new ClientesRestServiceR();
                                    StatusRestService statusClientesApi = new StatusRestService();
                                    statusClientesApi = clientesApi.FtnCargarClientes(iIdRuta, dFechaReparto, sConexionUri, "PARCIAL");

                                    if (statusClientesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientes.Text = "OK";
                                            lblStatusClientes.TextColor = Color.LimeGreen;
                                            lblTituloClientes.Text = "Clientes   (Fecha " + dFechaReparto.ToShortDateString() + ")   (Se actualizaron sus Condiciones de Crédito)";
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientes.Text = "ERROR";
                                            lblStatusClientes.TextColor = Color.Red;
                                            lblTituloClientes.Text = "Clientes   (Fecha " + dFechaReparto.ToShortDateString() + ")";
                                            DisplayAlert("Aviso", statusClientesApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los ENVASES de AUTOVENTA ó PREVENTA*/
                                if (((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P')) && bClientes)
                                {
                                    EnvasesRestServiceAP envasesApi = new EnvasesRestServiceAP();
                                    StatusRestService statusEnvasesApi = new StatusRestService();
                                    statusEnvasesApi = envasesApi.FtnCargarEnvases(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL", VarEntorno.bEsTeleventa);

                                    if (statusEnvasesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEnvases.Text = "OK";
                                            lblStatusEnvases.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEnvases.Text = "ERROR";
                                            lblStatusEnvases.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusEnvasesApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                
                                /*Carga datos de las BONIFICACIONES de AUTOVENTA ó PREVENTA*/
                                if (((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P')) && bClientes)
                                {
                                    BonificacionesRestServiceAP bonificacionesApi = new BonificacionesRestServiceAP();
                                    StatusRestService statusBonificacionesApi = new StatusRestService();
                                    statusBonificacionesApi = bonificacionesApi.FtnCargarBonificaciones(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL", VarEntorno.bEsTeleventa);

                                    if (statusBonificacionesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusBonificaciones.Text = "OK";
                                            lblStatusBonificaciones.TextColor = Color.LimeGreen;

                                            /*Valida que si no hay Bonificaciones lo especifique durante la Recepción*/
                                            if (statusBonificacionesApi.valor == "NO")
                                            {
                                                lblTituloBonificaciones.Text = "Bonificaciones   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloBonificaciones.Text = "Bonificaciones";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusBonificaciones.Text = "ERROR";
                                            lblStatusBonificaciones.TextColor = Color.Red;
                                            lblTituloBonificaciones.Text = "Bonificaciones";
                                            DisplayAlert("Aviso", statusBonificacionesApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los PEDIDOS SUGERIDOS de AUTOVENTA ó PREVENTA*/
                                if (((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P')) && bClientes)
                                {
                                    PedidosSugeridosRestServiceAP pedidosSugeridosApi = new PedidosSugeridosRestServiceAP();
                                    StatusRestService statusPedidosSugeridosApi = new StatusRestService();
                                    statusPedidosSugeridosApi = pedidosSugeridosApi.FtnCargarPedidosSugeridosAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL");

                                    if (statusPedidosSugeridosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPedidosSugeridos.Text = "OK";
                                            lblStatusPedidosSugeridos.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Bonificaciones lo especifique durante la Recepción*/
                                            if (statusPedidosSugeridosApi.valor == "NO")
                                            {
                                                lblTituloPedidosSugeridos.Text = "Pedidos Sugeridos   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloPedidosSugeridos.Text = "Pedidos Sugeridos";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPedidosSugeridos.Text = "ERROR";
                                            lblStatusPedidosSugeridos.TextColor = Color.Red;
                                            lblTituloPedidosSugeridos.Text = "Pedidos Sugeridos";
                                            DisplayAlert("Aviso", statusPedidosSugeridosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los COMPLEMENTOS DE PAGO de AUTOVENTA ó PREVENTA*/
                                /*OBSOLETA*/
                                if (((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P')) && bClientes)
                                {
                                    //ComplementosRestServiceAP complementosApi = new ComplementosRestServiceAP();
                                    //StatusRestService statusComplementosApi = new StatusRestService();
                                    //statusComplementosApi = complementosApi.FtnCargarComplementosPagoAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL", VarEntorno.bEsTeleventa);

                                    //if (statusComplementosApi.status == true)
                                    //{
                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusComplementos.Text = "OK";
                                    //        lblStatusComplementos.TextColor = Color.LimeGreen;
                                    //        /*Valida que si no hay Complementos de Pago lo especifique durante la Recepción*/
                                    //        if (statusComplementosApi.valor == "NO")
                                    //        {
                                    //            lblTituloComplementos.Text = "Complementos de Pago   (Sin datos)";
                                    //        }
                                    //        else
                                    //        {
                                    //            lblTituloComplementos.Text = "Complementos de Pago";
                                    //        }
                                    //    });
                                    //}
                                    //else
                                    //{
                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusComplementos.Text = "ERROR";
                                    //        lblStatusComplementos.TextColor = Color.Red;
                                    //        lblTituloComplementos.Text = "Complementos de Pago";
                                    //        DisplayAlert("Aviso", statusComplementosApi.mensaje, "OK");
                                    //    });

                                    //    progressDialogRecepcion.Dismiss();
                                    //    return;
                                    //}
                                }

                                /*Carga datos de los DOCUMENTOS de AUTOVENTA ó PREVENTA*/
                                if (((VarEntorno.cTipoVenta == 'A') ||
                                   (VarEntorno.cTipoVenta == 'P')) && bClientes)
                                {
                                    DocumentosRestServiceAP documentosApi = new DocumentosRestServiceAP();
                                    StatusRestService statusDocumentosApi = new StatusRestService();

                                    statusDocumentosApi = documentosApi.FtnCargarDocumentosAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL", VarEntorno.bEsTeleventa);

                                    if (statusDocumentosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentos.Text = "OK";
                                            lblStatusDocumentos.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Documentos lo especifique durante la Recepción*/
                                            if (statusDocumentosApi.valor == "NO")
                                            {
                                                lblTituloDocumentos.Text = "Documentos   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloDocumentos.Text = "Documentos";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentos.Text = "ERROR";
                                            lblStatusDocumentos.TextColor = Color.Red;
                                            lblTituloDocumentos.Text = "Documentos";
                                            DisplayAlert("Aviso", statusDocumentosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de la Telefonos de AUTOVENTA ó PREVENTA*/
                                if (((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P')) && bClientes)
                                {
                                    /*Carga datos de las OPCIONES DE Telefonos de AUTOVENTA ó PREVENTA*/
                                    TelefonosClientesRestServiceAP opcionesTelefonosApi = new TelefonosClientesRestServiceAP();
                                    StatusRestService statusTelefonosApi = new StatusRestService();
                                    statusTelefonosApi = opcionesTelefonosApi.FtnCargarTelefonosClientesAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL", VarEntorno.bEsTeleventa);

                                    if (statusTelefonosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusTelefonosClientes.Text = "OK";
                                            lblStatusTelefonosClientes.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Opciones de Respuestas lo especifique durante la Recepción*/
                                            if (statusTelefonosApi.valor == "NO")
                                            {
                                                lblTituloTelefonosClientes.Text = lblTituloTelefonosClientes.Text + "   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloTelefonosClientes.Text = lblTituloTelefonosClientes.Text + " ";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusTelefonosClientes.Text = "ERROR";
                                            lblStatusTelefonosClientes.TextColor = Color.Red;
                                            lblTituloTelefonosClientes.Text = "Telefonos (Con Error)";
                                            DisplayAlert("Aviso", statusTelefonosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }


                                /*Carga datos de las LISTAS DE PRECIOS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ListasPreciosRestServiceAP listasPreciosApi = new ListasPreciosRestServiceAP();
                                    StatusRestService statusListasPreciosApi = new StatusRestService();
                                    statusListasPreciosApi = listasPreciosApi.FtnCargarListasPrecios(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "PARCIAL", VarEntorno.bEsTeleventa);

                                    if (statusListasPreciosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusListasPrecios.Text = "OK";
                                            lblStatusListasPrecios.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusListasPrecios.Text = "ERROR";
                                            lblStatusListasPrecios.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusListasPreciosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los CANDADOS DE PRODUCTOS VERSION 2 de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {

                                    CandadosProductosRestServiceAPv2 candadosProductosV2Api = new CandadosProductosRestServiceAPv2();
                                    StatusRestService statusCandadosProductosV2Api = new StatusRestService();
                                    statusCandadosProductosV2Api = candadosProductosV2Api.FtnCargarCandadosProductosAPv2(VarEntorno.dFechaVenta, VarEntorno.dFechaVenta,"PARCIAL", sConexionUri);

                                    if (statusCandadosProductosV2Api.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusCandadosProductos.Text = "OK";
                                            lblStatusCandadosProductos.TextColor = Color.LimeGreen;
                                            //Valida que si no hay Candados de Productos lo especifique durante la Recepción
                                            if (statusCandadosProductosV2Api.valor == "NO")
                                            {
                                                lblTituloCandadosProductos.Text = "Candados de Productos   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloCandadosProductos.Text = "Candados de Productos";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusCandadosProductos.Text = "ERROR";
                                            lblStatusCandadosProductos.TextColor = Color.Red;
                                            lblTituloCandadosProductos.Text = "Candados de Productos";
                                            DisplayAlert("Aviso", statusCandadosProductosV2Api.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                }

                                /*Carga datos de las PROMOCIONES de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    PromocionesRestServiceAP promocionesApi = new PromocionesRestServiceAP();
                                    StatusRestService statusPromocionesApi = new StatusRestService();
                                    statusPromocionesApi = promocionesApi.FtnCargarPromociones(VarEntorno.dFechaVenta, "PARCIAL", sConexionUri);

                                    if (statusPromocionesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPromociones.Text = "OK";
                                            lblStatusPromociones.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Promociones lo especifique durante la Recepción*/
                                            if (statusPromocionesApi.valor == "NO")
                                            {
                                                lblTituloPromociones.Text = "Promociones   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloPromociones.Text = "Promociones";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPromociones.Text = "ERROR";
                                            lblStatusPromociones.TextColor = Color.Red;
                                            lblTituloPromociones.Text = "Promociones";
                                            DisplayAlert("Aviso", statusPromocionesApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los PRODUCTOS de AUTOVENTA, REPARTO ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ProductosRestServiceARP productosApi = new ProductosRestServiceARP();
                                    StatusRestService statusProductosApi = new StatusRestService();
                                    statusProductosApi = productosApi.FtnCargarProductos("PARCIAL", sConexionUri);

                                    if (statusProductosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusProductos.Text = "OK";
                                            lblStatusProductos.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusProductos.Text = "ERROR";
                                            lblStatusProductos.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusProductosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de las EXISTENCIAS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ExistenciasRestServiceAP existenciasApi = new ExistenciasRestServiceAP();
                                    StatusRestService statusExistenciasApi = new StatusRestService();
                                    statusExistenciasApi = existenciasApi.FtnCargarExistencias(VarEntorno.Almacen(), "PARCIAL", sConexionUri);

                                    if (statusExistenciasApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusExistencias.Text = "OK";
                                            lblStatusExistencias.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusExistencias.Text = "ERROR";
                                            lblStatusExistencias.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusExistenciasApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }
                                

                                progressDialogRecepcion.Dismiss();

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("Aviso", "La Recepción Parcial de la Ruta " + iIdRuta.ToString() + " ha terminado correctamente.", "OK");
                                });
                            });
                        }
                        else
                        {
                            await DisplayAlert("¡Atención!", statusConexion.mensaje + " para realizar la Recepción Parcial.", "OK");
                        }
                    }
                }
            }
        }

        /*Método para limpiar las etiquetas para una nueva Recepción Parcial de datos*/
        public void FtnLimpiarStatusRecepcion()
        {
            lblStatusPermisosApp.Text = "";
            lblStatusClientes.Text = "";
            lblTituloClientes.Text = "Clientes";
            lblStatusEnvases.Text = "";
            lblStatusListasPrecios.Text = "";
            lblStatusBonificaciones.Text = "";
            lblTituloBonificaciones.Text = "Bonificaciones";
            lblStatusPedidosSugeridos.Text = "";
            lblTituloPedidosSugeridos.Text = "Pedidos Sugeridos";
            //lblStatusComplementos.Text = "";
            //lblTituloComplementos.Text = "Complementos de Pago";
            lblStatusDocumentos.Text = "";
            lblTituloDocumentos.Text = "Documentos";
        }

        /*Método para regresar a la pantalla de MENÚ UTILERIAS*/
        private void btnRegresar_Clicked(object sender, EventArgs e)
        {
            //this.Navigation.PushModalAsync(new frmMenuPrincipal());
            this.Navigation.PopModalAsync();
        }
    }
}