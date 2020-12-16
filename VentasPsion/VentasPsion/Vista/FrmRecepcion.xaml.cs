using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VentasPsion.Modelo;
using VentasPsion.Modelo.ServicioApi;
using VentasPsion.VistaModelo;
using VentasPsion.Modelo.Servicio;
using Base;

namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrmRecepcion : ContentPage
    {
        #region Declaración de Labels para Status de Recepción
        /*Esta información es OBLIGATORIA de cargar, si alguno de ellos falla, no se realiza la Recepción correctamente*/
        Label lblStatusPasswords, lblTituloPasswords = null;
        Label lblStatusRutas, lblTituloRutas = null;
        Label lblStatusClientes, lblTituloClientes = null;
        Label lblStatusProductos, lblTituloProductos = null;
        Label lblStatusExistencias, lblTituloExistencias = null;
        Label lblStatusEnvases, lblTituloEnvases = null;
        Label lblStatusListasPrecios, lblTituloListasPrecios = null;
        Label lblStatusDetalleVentas, lblTituloDetalleVentas = null;
        Label lblStatusOrdenTickets, lblTituloOrdenTickets = null;
        Label lblStatusDepartamentos, lblTituloDepartamentos = null;
        Label lblStatusConceptosDev, lblTituloConceptosDev = null;
        Label lblStatusConceptosNoVenta, lblTituloConceptosNoVenta = null;
        Label lblStatusEnvasesSugeridos, lblTituloEnvasesSugeridos = null;

        /*Esta información es OPCIONAL de cargar, si no hay datos o falla alguno de ellos, la Recepción continúa*/
        Label lblStatusFacturasVentas, lblTituloFacturasVentas = null;
        Label lblStatusBonificaciones, lblTituloBonificaciones = null;
        Label lblStatusPromociones, lblTituloPromociones = null;
        //Label lblStatusComplementos, lblTituloComplementos = null;
        Label lblStatusDocumentos, lblTituloDocumentos = null;
        Label lblStatusRequisitosEntrega, lblTituloRequisitosEntrega = null;
        Label lblStatusKpis, lblTituloKpis = null;
        //Label lblStatusRetosDiarios, lblTituloRetosDiarios = null;
        Label lblStatusPedidosSugeridos, lblTituloPedidosSugeridos = null;
        Label lblStatusVolumenVentas, lblTituloVolumenVentas = null;
        Label lblStatusActivosComodatos, lblTituloActivosComodatos = null;
        Label lblStatusCandadosProductos, lblTituloCandadosProductos = null;
        Label lblStatusEncuestasClientes, lblTituloEncuestasClientes = null;
        Label lblStatusAnticiposClientes, lblTituloAnticiposClientes = null;
        Label lblStatusTelefonosClientes, lblTituloTelefonosClientes = null;
        //Label lblStatusOpcionesEncuestas, lblTituloOpcionesEncuestas = null;
        #endregion

        public FrmRecepcion()
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            #region Inicialización de Labels con sus propiedades para el Status de Recepción
            /*Esta información es OBLIGATORIA de cargar, si alguno de ellos falla, no se realiza la Recepción correctamente*/
            lblStatusPasswords = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloPasswords = new Label { Text = "Versión, Permisos, Passwords de App", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusRutas = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloRutas = new Label { Text = "Ruta", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloClientes = new Label { Text = "Clientes", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusProductos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloProductos = new Label { Text = "Productos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusExistencias = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloExistencias = new Label { Text = "Existencias", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusEnvases = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloEnvases = new Label { Text = "Envases", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusListasPrecios = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloListasPrecios = new Label { Text = "Listas de Precios", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusDetalleVentas = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloDetalleVentas = new Label { Text = "Detalles de Ventas", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusOrdenTickets = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloOrdenTickets = new Label { Text = "Orden de Tickets", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusDepartamentos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloDepartamentos = new Label { Text = "Departamentos y Segmentos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusConceptosDev = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloConceptosDev = new Label { Text = "Conceptos de Devolución", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusConceptosNoVenta = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloConceptosNoVenta = new Label { Text = "Conceptos de No Venta", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusEnvasesSugeridos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloEnvasesSugeridos = new Label { Text = "Envases Sugeridos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            /*Esta información es OPCIONAL de cargar, si no hay datos o falla alguno de ellos, la Recepción continúa*/
            lblStatusFacturasVentas = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloFacturasVentas = new Label { Text = "Facturas de Ventas", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusBonificaciones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloBonificaciones = new Label { Text = "Bonificaciones", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusPromociones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloPromociones = new Label { Text = "Promociones", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            //lblStatusComplementos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            //lblTituloComplementos = new Label { Text = "Complementos de Pago", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusDocumentos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloDocumentos = new Label { Text = "Documentos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusRequisitosEntrega = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloRequisitosEntrega = new Label { Text = "Requisitos de Entrega", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusKpis = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloKpis = new Label { Text = "KPI's  y  Retos Diarios", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };
            
            //lblStatusRetosDiarios = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            //lblTituloRetosDiarios = new Label { Text = "Retos Diarios", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusPedidosSugeridos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloPedidosSugeridos = new Label { Text = "Pedidos Sugeridos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusVolumenVentas = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloVolumenVentas = new Label { Text = "Volumen de Ventas", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusActivosComodatos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloActivosComodatos = new Label { Text = "Activos Comodatados", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusCandadosProductos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloCandadosProductos = new Label { Text = "Candados de Productos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusEncuestasClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloEncuestasClientes = new Label { Text = "Encuesta  y  Opciones de Respuestas", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusAnticiposClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloAnticiposClientes = new Label { Text = "Anticipos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            lblStatusTelefonosClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            lblTituloTelefonosClientes = new Label { Text = "Telefonos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };

            //lblStatusOpcionesEncuestas = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)) };
            //lblTituloOpcionesEncuestas = new Label { Text = "Opciones de Encuestas", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), TextColor = Color.Black };
            #endregion

            #region Agrega los Labels para el Status de Recepción al Grid, según el Tipo de Venta o Perfil (AUTOVENTA, REPARTO ó PREVENTA)
            switch (VarEntorno.cTipoVenta)
            {
                case 'A': /*AUTOVENTA*/
                    /*Esta información es OBLIGATORIA de cargar, si alguno de ellos falla, no se realiza la Recepción correctamente*/
                    grdRecepcion.Children.Add(lblStatusPasswords, 1, 2);
                    grdRecepcion.Children.Add(lblTituloPasswords, 2, 2);
                    Grid.SetColumnSpan(lblTituloPasswords, 2);
                    grdRecepcion.Children.Add(lblStatusRutas, 1, 3);
                    grdRecepcion.Children.Add(lblTituloRutas, 2, 3);
                    Grid.SetColumnSpan(lblTituloRutas, 2);
                    grdRecepcion.Children.Add(lblStatusClientes, 1, 4);
                    grdRecepcion.Children.Add(lblTituloClientes, 2, 4);
                    Grid.SetColumnSpan(lblTituloClientes, 2);
                    grdRecepcion.Children.Add(lblStatusProductos, 1, 5);
                    grdRecepcion.Children.Add(lblTituloProductos, 2, 5);
                    Grid.SetColumnSpan(lblTituloProductos, 2);
                    grdRecepcion.Children.Add(lblStatusExistencias, 1, 6);
                    grdRecepcion.Children.Add(lblTituloExistencias, 2, 6);
                    Grid.SetColumnSpan(lblTituloExistencias, 2);
                    grdRecepcion.Children.Add(lblStatusEnvases, 1, 7);
                    grdRecepcion.Children.Add(lblTituloEnvases, 2, 7);
                    Grid.SetColumnSpan(lblTituloEnvases, 2);
                    grdRecepcion.Children.Add(lblStatusListasPrecios, 1, 8);
                    grdRecepcion.Children.Add(lblTituloListasPrecios, 2, 8);
                    Grid.SetColumnSpan(lblTituloListasPrecios, 2);
                    grdRecepcion.Children.Add(lblStatusOrdenTickets, 1, 9);
                    grdRecepcion.Children.Add(lblTituloOrdenTickets, 2, 9);
                    Grid.SetColumnSpan(lblTituloOrdenTickets, 2);
                    grdRecepcion.Children.Add(lblStatusDepartamentos, 1, 10);
                    grdRecepcion.Children.Add(lblTituloDepartamentos, 2, 10);
                    Grid.SetColumnSpan(lblTituloDepartamentos, 2);
                    grdRecepcion.Children.Add(lblStatusConceptosNoVenta, 1, 11);
                    grdRecepcion.Children.Add(lblTituloConceptosNoVenta, 2, 11);
                    Grid.SetColumnSpan(lblTituloConceptosNoVenta, 2);


                    /*Esta información es OPCIONAL de cargar, si no hay datos o falla alguno de ellos, la Recepción continúa*/
                    grdRecepcion.Children.Add(lblStatusBonificaciones, 1, 12);
                    grdRecepcion.Children.Add(lblTituloBonificaciones, 2, 12);
                    Grid.SetColumnSpan(lblTituloBonificaciones, 2);
                    grdRecepcion.Children.Add(lblStatusPromociones, 1, 13);
                    grdRecepcion.Children.Add(lblTituloPromociones, 2, 13);
                    Grid.SetColumnSpan(lblTituloPromociones, 2);
                    //grdRecepcion.Children.Add(lblStatusComplementos, 1, 14);
                    //grdRecepcion.Children.Add(lblTituloComplementos, 2, 14);
                    //Grid.SetColumnSpan(lblTituloComplementos, 2);
                    grdRecepcion.Children.Add(lblStatusDocumentos, 1, 14);
                    grdRecepcion.Children.Add(lblTituloDocumentos, 2, 14);
                    Grid.SetColumnSpan(lblTituloDocumentos, 2);
                    grdRecepcion.Children.Add(lblStatusKpis, 1, 15);
                    grdRecepcion.Children.Add(lblTituloKpis, 2, 15);
                    Grid.SetColumnSpan(lblTituloKpis, 2);
                    //grdRecepcion.Children.Add(lblStatusRetosDiarios, 1, 15);
                    //grdRecepcion.Children.Add(lblTituloRetosDiarios, 2, 15);
                    //Grid.SetColumnSpan(lblTituloRetosDiarios, 2);
                    grdRecepcion.Children.Add(lblStatusPedidosSugeridos, 1, 16);
                    grdRecepcion.Children.Add(lblTituloPedidosSugeridos, 2, 16);
                    Grid.SetColumnSpan(lblTituloPedidosSugeridos, 2);
                    grdRecepcion.Children.Add(lblStatusVolumenVentas, 1, 17);
                    grdRecepcion.Children.Add(lblTituloVolumenVentas, 2, 17);
                    Grid.SetColumnSpan(lblTituloVolumenVentas, 2);
                    grdRecepcion.Children.Add(lblStatusActivosComodatos, 1, 18);
                    grdRecepcion.Children.Add(lblTituloActivosComodatos, 2, 18);
                    Grid.SetColumnSpan(lblTituloActivosComodatos, 2);
                    //grdRecepcion.Children.Add(lblStatusCandadosProductos, 1, 19);
                    //grdRecepcion.Children.Add(lblTituloCandadosProductos, 2, 19);
                    //Grid.SetColumnSpan(lblTituloCandadosProductos, 2);
                    grdRecepcion.Children.Add(lblStatusEncuestasClientes, 1, 20);
                    grdRecepcion.Children.Add(lblTituloEncuestasClientes, 2, 20);
                    Grid.SetColumnSpan(lblTituloEncuestasClientes, 2);
                    grdRecepcion.Children.Add(lblStatusAnticiposClientes, 1, 21);
                    grdRecepcion.Children.Add(lblTituloAnticiposClientes, 2, 21);
                    Grid.SetColumnSpan(lblTituloAnticiposClientes, 2);
                    grdRecepcion.Children.Add(lblStatusTelefonosClientes, 1, 22);
                    grdRecepcion.Children.Add(lblTituloTelefonosClientes, 2, 22);
                    Grid.SetColumnSpan(lblTituloTelefonosClientes, 2);
                    //grdRecepcion.Children.Add(lblStatusOpcionesEncuestas, 1, 21);
                    //grdRecepcion.Children.Add(lblTituloOpcionesEncuestas, 2, 21);
                    //Grid.SetColumnSpan(lblTituloOpcionesEncuestas, 2);
                    break;
                case 'R': /*REPARTO*/
                    /*Esta información es OBLIGATORIA de cargar, si alguno de ellos falla, no se realiza la Recepción correctamente*/
                    grdRecepcion.Children.Add(lblStatusPasswords, 1, 2);
                    grdRecepcion.Children.Add(lblTituloPasswords, 2, 2);
                    Grid.SetColumnSpan(lblTituloPasswords, 2);
                    grdRecepcion.Children.Add(lblStatusRutas, 1, 3);
                    grdRecepcion.Children.Add(lblTituloRutas, 2, 3);
                    Grid.SetColumnSpan(lblTituloRutas, 2);
                    grdRecepcion.Children.Add(lblStatusClientes, 1, 4);
                    grdRecepcion.Children.Add(lblTituloClientes, 2, 4);
                    Grid.SetColumnSpan(lblTituloClientes, 2);
                    grdRecepcion.Children.Add(lblStatusProductos, 1, 5);
                    grdRecepcion.Children.Add(lblTituloProductos, 2, 5);
                    Grid.SetColumnSpan(lblTituloProductos, 2);
                    grdRecepcion.Children.Add(lblStatusEnvases, 1, 6);
                    grdRecepcion.Children.Add(lblTituloEnvases, 2, 6);
                    Grid.SetColumnSpan(lblTituloEnvases, 2);
                    grdRecepcion.Children.Add(lblStatusListasPrecios, 1, 7);
                    grdRecepcion.Children.Add(lblTituloListasPrecios, 2, 7);
                    Grid.SetColumnSpan(lblTituloListasPrecios, 2);
                    grdRecepcion.Children.Add(lblStatusDetalleVentas, 1, 8);
                    grdRecepcion.Children.Add(lblTituloDetalleVentas, 2, 8);
                    Grid.SetColumnSpan(lblTituloDetalleVentas, 2);
                    grdRecepcion.Children.Add(lblStatusOrdenTickets, 1, 9);
                    grdRecepcion.Children.Add(lblTituloOrdenTickets, 2, 9);
                    Grid.SetColumnSpan(lblTituloOrdenTickets, 2);
                    grdRecepcion.Children.Add(lblStatusDepartamentos, 1, 10);
                    grdRecepcion.Children.Add(lblTituloDepartamentos, 2, 10);
                    Grid.SetColumnSpan(lblTituloDepartamentos, 2);
                    grdRecepcion.Children.Add(lblStatusConceptosDev, 1, 11);
                    grdRecepcion.Children.Add(lblTituloConceptosDev, 2, 11);
                    Grid.SetColumnSpan(lblTituloConceptosDev, 2);
                    grdRecepcion.Children.Add(lblStatusEnvasesSugeridos, 1, 12);
                    grdRecepcion.Children.Add(lblTituloEnvasesSugeridos, 2, 12);
                    Grid.SetColumnSpan(lblTituloEnvasesSugeridos, 2);

                    /*Esta información es OPCIONAL de cargar, si no hay datos o falla alguno de ellos, la Recepción continúa*/
                    grdRecepcion.Children.Add(lblStatusFacturasVentas, 1, 13);
                    grdRecepcion.Children.Add(lblTituloFacturasVentas, 2, 13);
                    Grid.SetColumnSpan(lblTituloFacturasVentas, 2);
                    //grdRecepcion.Children.Add(lblStatusComplementos, 1, 13);
                    //grdRecepcion.Children.Add(lblTituloComplementos, 2, 13);
                    //Grid.SetColumnSpan(lblTituloComplementos, 2);
                    grdRecepcion.Children.Add(lblStatusDocumentos, 1, 14);
                    grdRecepcion.Children.Add(lblTituloDocumentos, 2, 14);
                    Grid.SetColumnSpan(lblTituloDocumentos, 2);
                    grdRecepcion.Children.Add(lblStatusRequisitosEntrega, 1, 15);
                    grdRecepcion.Children.Add(lblTituloRequisitosEntrega, 2, 15);
                    Grid.SetColumnSpan(lblTituloRequisitosEntrega, 2);
                    grdRecepcion.Children.Add(lblStatusTelefonosClientes, 1, 16);
                    grdRecepcion.Children.Add(lblTituloTelefonosClientes, 2, 16);
                    Grid.SetColumnSpan(lblTituloTelefonosClientes, 2);
                    break;
                case 'P': /*PREVENTA*/
                    /*Esta información es OBLIGATORIA de cargar, si alguno de ellos falla, no se realiza la Recepción correctamente*/
                    grdRecepcion.Children.Add(lblStatusPasswords, 1, 2);
                    grdRecepcion.Children.Add(lblTituloPasswords, 2, 2);
                    Grid.SetColumnSpan(lblTituloPasswords, 2);
                    grdRecepcion.Children.Add(lblStatusRutas, 1, 3);
                    grdRecepcion.Children.Add(lblTituloRutas, 2, 3);
                    Grid.SetColumnSpan(lblTituloRutas, 2);
                    grdRecepcion.Children.Add(lblStatusClientes, 1, 4);
                    grdRecepcion.Children.Add(lblTituloClientes, 2, 4);
                    Grid.SetColumnSpan(lblTituloClientes, 2);
                    grdRecepcion.Children.Add(lblStatusProductos, 1, 5);
                    grdRecepcion.Children.Add(lblTituloProductos, 2, 5);
                    Grid.SetColumnSpan(lblTituloProductos, 2);
                    grdRecepcion.Children.Add(lblStatusExistencias, 1, 6);
                    grdRecepcion.Children.Add(lblTituloExistencias, 2, 6);
                    Grid.SetColumnSpan(lblTituloExistencias, 2);
                    grdRecepcion.Children.Add(lblStatusEnvases, 1, 7);
                    grdRecepcion.Children.Add(lblTituloEnvases, 2, 7);
                    Grid.SetColumnSpan(lblTituloEnvases, 2);
                    grdRecepcion.Children.Add(lblStatusListasPrecios, 1, 8);
                    grdRecepcion.Children.Add(lblTituloListasPrecios, 2, 8);
                    Grid.SetColumnSpan(lblTituloListasPrecios, 2);
                    grdRecepcion.Children.Add(lblStatusOrdenTickets, 1, 9);
                    grdRecepcion.Children.Add(lblTituloOrdenTickets, 2, 9);
                    Grid.SetColumnSpan(lblTituloOrdenTickets, 2);
                    grdRecepcion.Children.Add(lblStatusDepartamentos, 1, 10);
                    grdRecepcion.Children.Add(lblTituloDepartamentos, 2, 10);
                    Grid.SetColumnSpan(lblTituloDepartamentos, 2);
                    grdRecepcion.Children.Add(lblStatusConceptosNoVenta, 1, 11);
                    grdRecepcion.Children.Add(lblTituloConceptosNoVenta, 2, 11);
                    Grid.SetColumnSpan(lblTituloConceptosNoVenta, 2);


                    /*Esta información es OPCIONAL de cargar, si no hay datos o falla alguno de ellos, la Recepción continúa*/
                    grdRecepcion.Children.Add(lblStatusBonificaciones, 1, 12);
                    grdRecepcion.Children.Add(lblTituloBonificaciones, 2, 12);
                    Grid.SetColumnSpan(lblTituloBonificaciones, 2);
                    grdRecepcion.Children.Add(lblStatusPromociones, 1, 13);
                    grdRecepcion.Children.Add(lblTituloPromociones, 2, 13);
                    Grid.SetColumnSpan(lblTituloPromociones, 2);
                    //grdRecepcion.Children.Add(lblStatusComplementos, 1, 14);
                    //grdRecepcion.Children.Add(lblTituloComplementos, 2, 14);
                    //Grid.SetColumnSpan(lblTituloComplementos, 2);
                    grdRecepcion.Children.Add(lblStatusDocumentos, 1, 14);
                    grdRecepcion.Children.Add(lblTituloDocumentos, 2, 14);
                    Grid.SetColumnSpan(lblTituloDocumentos, 2);
                    grdRecepcion.Children.Add(lblStatusKpis, 1, 15);
                    grdRecepcion.Children.Add(lblTituloKpis, 2, 15);
                    Grid.SetColumnSpan(lblTituloKpis, 2);
                    //grdRecepcion.Children.Add(lblStatusRetosDiarios, 1, 15);
                    //grdRecepcion.Children.Add(lblTituloRetosDiarios, 2, 15);
                    //Grid.SetColumnSpan(lblTituloRetosDiarios, 2);
                    grdRecepcion.Children.Add(lblStatusPedidosSugeridos, 1, 16);
                    grdRecepcion.Children.Add(lblTituloPedidosSugeridos, 2, 16);
                    Grid.SetColumnSpan(lblTituloPedidosSugeridos, 2);
                    grdRecepcion.Children.Add(lblStatusVolumenVentas, 1, 17);
                    grdRecepcion.Children.Add(lblTituloVolumenVentas, 2, 17);
                    Grid.SetColumnSpan(lblTituloVolumenVentas, 2);
                    grdRecepcion.Children.Add(lblStatusActivosComodatos, 1, 18);
                    grdRecepcion.Children.Add(lblTituloActivosComodatos, 2, 18);
                    Grid.SetColumnSpan(lblTituloActivosComodatos, 2);
                    //grdRecepcion.Children.Add(lblStatusCandadosProductos, 1, 19);
                    //grdRecepcion.Children.Add(lblTituloCandadosProductos, 2, 19);
                    //Grid.SetColumnSpan(lblTituloCandadosProductos, 2);
                    grdRecepcion.Children.Add(lblStatusEncuestasClientes, 1, 20);
                    grdRecepcion.Children.Add(lblTituloEncuestasClientes, 2, 20);
                    Grid.SetColumnSpan(lblTituloEncuestasClientes, 2);
                    grdRecepcion.Children.Add(lblStatusAnticiposClientes, 1, 21);
                    grdRecepcion.Children.Add(lblTituloAnticiposClientes, 2, 21);
                    Grid.SetColumnSpan(lblTituloAnticiposClientes, 2);
                    grdRecepcion.Children.Add(lblStatusTelefonosClientes, 1, 22);
                    grdRecepcion.Children.Add(lblTituloTelefonosClientes, 2, 22);
                    Grid.SetColumnSpan(lblTituloTelefonosClientes, 2);
                    //grdRecepcion.Children.Add(lblStatusOpcionesEncuestas, 1, 21);
                    //grdRecepcion.Children.Add(lblTituloOpcionesEncuestas, 2, 21);
                    //Grid.SetColumnSpan(lblTituloOpcionesEncuestas, 2);
                    break;
                default:
                    break;
            }
            #endregion
        }

        /*Método para realizar la Recepción de datos de una Ruta específica de AUTOVENTA, REPARTO ó PREVENTA*/
        public async Task OnClickedRecepcion(object sender, EventArgs args)
        {
            string sIdRuta = entRuta.Text;

            FtnLimpiarStatusRecepcion();

            if (String.IsNullOrEmpty(sIdRuta))
            {
                await DisplayAlert("Aviso", "Ha olvidado capturar la RUTA.", "OK");
                entRuta.Focus();
            }
            else
            {
                if (sIdRuta == ".")
                {
                    await DisplayAlert("Aviso", "Ha capturado un punto ( . ) en lugar de una RUTA.", "OK");
                    entRuta.Focus();
                }
                else
                {
                    int iIdRuta;
                    if (!int.TryParse(sIdRuta, out iIdRuta))
                    {
                        await DisplayAlert("Aviso", "No es válido un decimal en la RUTA.", "OK");
                        entRuta.Focus();
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

                            //Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Ruta", sIdRuta);

                            Utilerias utilerias = new Utilerias();
                            var progressDialogRecepcion = utilerias.crearProgressDialog("Recepción por " + sConexionTipo, "Cargando información...");
                            progressDialogRecepcion.Show();

                            await Task.Run(() =>
                            {
                                string sAlmacen = "";
                                DateTime dFechaReparto = new DateTime();

                                /*Borra datos de todas las tablas de la BD del dispositivo para cargarlos posteriormente*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'R') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    BorraDatosTablasService BorraDatosTablas = new BorraDatosTablasService();
                                    StatusService statusBorraDatosTablas = new StatusService();
                                    statusBorraDatosTablas = BorraDatosTablas.FtnBorrarDatosTablas();

                                    if (statusBorraDatosTablas.status == true)
                                    {
                                        //lblStatusRutas.Text = "OK";
                                        //lblStatusRutas.TextColor = Color.LimeGreen;
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            //lblStatusRutas.Text = "ERROR";
                                            //lblStatusRutas.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusBorraDatosTablas.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Esta información es OBLIGATORIA de cargar, si alguno de ellos falla, no se realiza la Recepción correctamente*/

                                /*Carga datos de la VERSION, PERMISOS y PASSWORDS de la App de AUTOVENTA, REPARTO ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'R') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    /*Carga datos de la VERSION de la App de AUTOVENTA, REPARTO ó PREVENTA*/
                                    VersionAppRestServiceARP versionAppApi = new VersionAppRestServiceARP();
                                    StatusRestService statusVersionAppApi = new StatusRestService();
                                    statusVersionAppApi = versionAppApi.FtnCargarVersionAppARP(sConexionUri);

                                    if (statusVersionAppApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPasswords.Text = "OK";
                                            lblStatusPasswords.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPasswords.Text = "ERROR";
                                            lblStatusPasswords.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusVersionAppApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    /*Valida que la Versión de la App de Ventas CCA esté actualizada a la última versión según la BD del ERP*/
                                    version_appSR versionApp = new version_appSR();
                                    StatusService statusVersionApp = new StatusService();
                                    statusVersionApp = versionApp.FtnDevuelveVersionApp();

                                    if (statusVersionApp.status == true)
                                    {
                                        if (statusVersionApp.valor == "")
                                        {
                                            VarEntorno.iNoRuta = 0;

                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                lblStatusPasswords.Text = "ERROR";
                                                lblStatusPasswords.TextColor = Color.Red;
                                                DisplayAlert("Aviso", statusVersionApp.mensaje, "OK");
                                            });

                                            progressDialogRecepcion.Dismiss();
                                            return;
                                        }
                                        else 
                                            if (statusVersionApp.valor == VarEntorno.sVersionApp.Replace("Versión ", ""))
                                            {
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPasswords.Text = "OK";
                                                    lblStatusPasswords.TextColor = Color.LimeGreen;
                                                });
                                            }
                                            else
                                            {
                                                VarEntorno.iNoRuta = 0;

                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPasswords.Text = "ERROR";
                                                    lblStatusPasswords.TextColor = Color.Red;
                                                    DisplayAlert("Aviso", "-No es posible realizar la Recepción porque la Versión de la App de Ventas CCA no está actualizada.\n" +
                                                                          "-Favor de pasar a Sistemas a actualizarla.", "OK");
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                            }
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPasswords.Text = "ERROR";
                                            lblStatusPasswords.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusVersionApp.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    /*Carga datos de los PERMISOS de la App de AUTOVENTA, REPARTO ó PREVENTA*/
                                    PermisosAppRestServiceARP permisosAppApi = new PermisosAppRestServiceARP();
                                    StatusRestService statusPermisosAppApi = new StatusRestService();
                                    statusPermisosAppApi = permisosAppApi.FtnCargarPermisosAppARP(VarEntorno.cTipoVenta, sConexionUri);

                                    if (statusPermisosAppApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPasswords.Text = "OK";
                                            lblStatusPasswords.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPasswords.Text = "ERROR";
                                            lblStatusPasswords.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusPermisosAppApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    /*Carga datos de los PASSWORDS de la App de AUTOVENTA, REPARTO ó PREVENTA*/
                                    PasswordRestServiceARP passwordsApi = new PasswordRestServiceARP();
                                    StatusRestService statusPasswordsApi = new StatusRestService();
                                    statusPasswordsApi = passwordsApi.FtnCargarPasswordARP(VarEntorno.cTipoVenta, VarEntorno.dFechaVenta, sConexionUri);

                                    if (statusPasswordsApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPasswords.Text = "OK";
                                            lblStatusPasswords.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusPasswords.Text = "ERROR";
                                            lblStatusPasswords.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusPasswordsApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de la RUTA de AUTOVENTA, REPARTO ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'R') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    RutaRestServiceARP rutasApi = new RutaRestServiceARP();
                                    StatusRestService statusRutasApi = new StatusRestService();
                                    statusRutasApi = rutasApi.FtnCargarRuta(iIdRuta, VarEntorno.cTipoVenta, VarEntorno.sTipoVenta, sConexionUri);

                                    if (statusRutasApi.status == true)
                                    {
                                        VarEntorno.iNoRuta = iIdRuta;
                                        sAlmacen = statusRutasApi.valor;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRutas.Text = "OK";
                                            lblStatusRutas.TextColor = Color.LimeGreen;
                                            lblTituloRutas.Text = "Ruta " + iIdRuta.ToString() + "   (Almacén " + statusRutasApi.valor + ")";
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRutas.Text = "ERROR";
                                            lblStatusRutas.TextColor = Color.Red;
                                            lblTituloRutas.Text = "Ruta";
                                            DisplayAlert("Aviso", statusRutasApi.mensaje, "OK");
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
                                    statusClientesApi = clientesApi.FtnCargarClientes(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

                                    if (statusClientesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientes.Text = "OK";
                                            lblStatusClientes.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientes.Text = "ERROR";
                                            lblStatusClientes.TextColor = Color.Red;
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
                                        VarEntorno.iNoRuta = 0;

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
                                    statusClientesApi = clientesApi.FtnCargarClientes(iIdRuta, dFechaReparto, sConexionUri, "TOTAL");

                                    if (statusClientesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientes.Text = "OK";
                                            lblStatusClientes.TextColor = Color.LimeGreen;
                                            lblTituloClientes.Text = "Clientes   (Fecha " + dFechaReparto.ToShortDateString() + ")";
                                            //await DisplayAlert("Aviso", statusClientesApi.mensaje, "OK");
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de los PRODUCTOS de AUTOVENTA, REPARTO ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'R') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ProductosRestServiceARP productosApi = new ProductosRestServiceARP();
                                    StatusRestService statusProductosApi = new StatusRestService();
                                    statusProductosApi = productosApi.FtnCargarProductos("TOTAL",sConexionUri);

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
                                    statusExistenciasApi = existenciasApi.FtnCargarExistencias(sAlmacen, "TOTAL", sConexionUri);

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

                                /*Carga datos de los ENVASES de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    EnvasesRestServiceAP envasesApi = new EnvasesRestServiceAP();
                                    StatusRestService statusEnvasesApi = new StatusRestService();
                                    statusEnvasesApi = envasesApi.FtnCargarEnvases(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de los ENVASES de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    EnvasesRestServiceR envasesApi = new EnvasesRestServiceR();
                                    StatusRestService statusEnvasesApi = new StatusRestService();
                                    statusEnvasesApi = envasesApi.FtnCargarEnvasesR(iIdRuta, dFechaReparto, sConexionUri);

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de las LISTAS DE PRECIOS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ListasPreciosRestServiceAP listasPreciosApi = new ListasPreciosRestServiceAP();
                                    StatusRestService statusListasPreciosApi = new StatusRestService();
                                    statusListasPreciosApi = listasPreciosApi.FtnCargarListasPrecios(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de las LISTAS DE PRECIOS de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    ListasPreciosRestServiceR listasPreciosApi = new ListasPreciosRestServiceR();
                                    StatusRestService statusListasPreciosApi = new StatusRestService();
                                    statusListasPreciosApi = listasPreciosApi.FtnCargarListasPreciosR(iIdRuta, dFechaReparto, sConexionUri);

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de los DETALLES DE VENTAS de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    DetallesVentasRestServiceR DetallesVentasApi = new DetallesVentasRestServiceR();
                                    StatusRestService statusDetallesVentasApi = new StatusRestService();
                                    statusDetallesVentasApi = DetallesVentasApi.FtnCargarDetallesVentasR(iIdRuta, dFechaReparto, sConexionUri);

                                    if (statusDetallesVentasApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDetalleVentas.Text = "OK";
                                            lblStatusDetalleVentas.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDetalleVentas.Text = "ERROR";
                                            lblStatusDetalleVentas.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusDetallesVentasApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos del ORDEN DE TICKETS de AUTOVENTA, REPARTO ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'R') ||
                                    (VarEntorno.cTipoVenta == 'P'))

                                {
                                    OrdenTicketsRestServiceARP ordenTicketsApi = new OrdenTicketsRestServiceARP();
                                    StatusRestService statusOrdenTicketsApi = new StatusRestService();
                                    statusOrdenTicketsApi = ordenTicketsApi.FtnCargarOrdenTickets(sConexionUri);

                                    if (statusOrdenTicketsApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusOrdenTickets.Text = "OK";
                                            lblStatusOrdenTickets.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusOrdenTickets.Text = "ERROR";
                                            lblStatusOrdenTickets.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusOrdenTicketsApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de DEPARTAMENTOS y SEGMENTOS de AUTOVENTA, REPARTO ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'R') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    /*Carga datos de DEPARTAMENTOS de AUTOVENTA, REPARTO ó PREVENTA*/
                                    DepartamentosRestServiceARP departamentosApi = new DepartamentosRestServiceARP();
                                    StatusRestService statusDepartamentosApi = new StatusRestService();
                                    statusDepartamentosApi = departamentosApi.FtnCargarDepartamentos(sConexionUri);

                                    if (statusDepartamentosApi.status == true)
                                    {
                                        /*Valida que si no hay Departamentos lo especifique durante la Recepción*/
                                        if (statusDepartamentosApi.valor == "NO")
                                        {
                                            VarEntorno.iNoRuta = 0;

                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                lblStatusDepartamentos.Text = "ERROR";
                                                lblStatusDepartamentos.TextColor = Color.Red;
                                                lblTituloDepartamentos.Text = "Departamentos  (Sin datos)  y  Segmentos";
                                                //DisplayAlert("Aviso", statusDepartamentosApi.mensaje, "OK");
                                            });

                                            progressDialogRecepcion.Dismiss();
                                            return;
                                        }
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                lblStatusDepartamentos.Text = "OK";
                                                lblStatusDepartamentos.TextColor = Color.LimeGreen;
                                                lblTituloDepartamentos.Text = "Departamentos";
                                            });
                                        }
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDepartamentos.Text = "ERROR";
                                            lblStatusDepartamentos.TextColor = Color.Red;
                                            lblTituloDepartamentos.Text = "Departamentos  (Con Error)  y  Segmentos";
                                            DisplayAlert("Aviso", statusDepartamentosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    /*Carga datos de SEGMENTOS de AUTOVENTA, REPARTO ó PREVENTA*/
                                    SegmentosRestServiceAP segmentosApi = new SegmentosRestServiceAP();
                                    StatusRestService statusSegmentosApi = new StatusRestService();
                                    statusSegmentosApi = segmentosApi.FtnCargarSegmentosAP(sConexionUri);

                                    if (statusSegmentosApi.status == true)
                                    {
                                        /*Valida que si no hay Segmentos lo especifique durante la Recepción*/
                                        if (statusSegmentosApi.valor == "NO")
                                        {
                                            VarEntorno.iNoRuta = 0;

                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                lblStatusDepartamentos.Text = "ERROR";
                                                lblStatusDepartamentos.TextColor = Color.Red;
                                                lblTituloDepartamentos.Text = lblTituloDepartamentos.Text + "  y  Segmentos  (Sin datos)";
                                                //DisplayAlert("Aviso", statusSegmentosApi.mensaje, "OK");
                                            });

                                            progressDialogRecepcion.Dismiss();
                                            return;
                                        }
                                        else
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                lblStatusDepartamentos.Text = "OK";
                                                lblStatusDepartamentos.TextColor = Color.LimeGreen;
                                                lblTituloDepartamentos.Text = lblTituloDepartamentos.Text + "  y  Segmentos";
                                            });
                                        }
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDepartamentos.Text = "ERROR";
                                            lblStatusDepartamentos.TextColor = Color.Red;
                                            lblTituloDepartamentos.Text = lblTituloDepartamentos.Text + "  y  Segmentos  (Con Error)";
                                            DisplayAlert("Aviso", statusSegmentosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los CONCEPTOS DE DEVOLUCION de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    ConceptosDevolRestServiceR ConceptosDevolApi = new ConceptosDevolRestServiceR();
                                    StatusRestService statusConceptosDevolApi = new StatusRestService();
                                    statusConceptosDevolApi = ConceptosDevolApi.FtnCargarConceptosDevol(sConexionUri);

                                    if (statusConceptosDevolApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusConceptosDev.Text = "OK";
                                            lblStatusConceptosDev.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusConceptosDev.Text = "ERROR";
                                            lblStatusConceptosDev.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusConceptosDevolApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los CONCEPTOS DE NO VENTA de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ConceptosNoVentaRestServiceAP ConceptosNoVentaApi = new ConceptosNoVentaRestServiceAP();
                                    StatusRestService statusConceptosNoVentaApi = new StatusRestService();
                                    statusConceptosNoVentaApi = ConceptosNoVentaApi.FtnCargarConceptosNoVenta(sConexionUri);

                                    if (statusConceptosNoVentaApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusConceptosNoVenta.Text = "OK";
                                            lblStatusConceptosNoVenta.TextColor = Color.LimeGreen;
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusConceptosNoVenta.Text = "ERROR";
                                            lblStatusConceptosNoVenta.TextColor = Color.Red;
                                            DisplayAlert("Aviso", statusConceptosNoVentaApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }
                                #region ENVASE SUGERIDO
                                /*Carga datos de los ENVASES SUGERIDOS de REPARTO*/
                                /*
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    EnvasesSugeridosRestServiceR envasesSugeridosApi = new EnvasesSugeridosRestServiceR();
                                    StatusRestService statusEnvasesSugeridosApi = new StatusRestService();
                                    statusEnvasesSugeridosApi = envasesSugeridosApi.FtnCargarEnvasesSugeridosR(iIdRuta, dFechaReparto, sConexionUri);

                                    if (statusEnvasesSugeridosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEnvasesSugeridos.Text = "OK";
                                            lblStatusEnvasesSugeridos.TextColor = Color.LimeGreen;

                                            ///Líneas de código para que la carga sea OPCIONAL
                                            ///Valida que si no hay Envases Sugeridos lo especifique durante la Recepción
                                            //if (statusEnvasesSugeridosApi.valor == "NO")
                                            //{
                                            //    lblTituloEnvasesSugeridos.Text = "Envases Sugeridos   (Sin datos)";
                                            //}
                                            //else
                                            //{
                                            //    lblTituloEnvasesSugeridos.Text = "Envases Sugeridos";
                                            //}
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEnvasesSugeridos.Text = "ERROR";
                                            lblStatusEnvasesSugeridos.TextColor = Color.Red;
                                            lblTituloEnvasesSugeridos.Text = "Envases Sugeridos";
                                            DisplayAlert("Aviso", statusEnvasesSugeridosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }
                                */
                                #endregion
                                /*Esta información es OPCIONAL de cargar, si no hay datos o falla alguno de ellos, la Recepción continúa*/

                                /*Carga datos de las FACTURAS DE VENTAS de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    FacturasVentasRestServiceR FacturasVentasApi = new FacturasVentasRestServiceR();
                                    StatusRestService statusFacturasVentasApi = new StatusRestService();
                                    statusFacturasVentasApi = FacturasVentasApi.FtnCargarFacturasVentasR(iIdRuta, dFechaReparto, sConexionUri);

                                    if (statusFacturasVentasApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusFacturasVentas.Text = "OK";
                                            lblStatusFacturasVentas.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Facturas de Venta lo especifique durante la Recepción*/
                                            if (statusFacturasVentasApi.valor == "NO")
                                            {
                                                lblTituloFacturasVentas.Text = "Facturas de Ventas   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloFacturasVentas.Text = "Facturas de Ventas";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusFacturasVentas.Text = "ERROR";
                                            lblStatusFacturasVentas.TextColor = Color.Red;
                                            lblTituloFacturasVentas.Text = "Facturas de Ventas";
                                            DisplayAlert("Aviso", statusFacturasVentasApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de las BONIFICACIONES de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    BonificacionesRestServiceAP bonificacionesApi = new BonificacionesRestServiceAP();
                                    StatusRestService statusBonificacionesApi = new StatusRestService();
                                    statusBonificacionesApi = bonificacionesApi.FtnCargarBonificaciones(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de las PROMOCIONES de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    PromocionesRestServiceAP promocionesApi = new PromocionesRestServiceAP();
                                    StatusRestService statusPromocionesApi = new StatusRestService();
                                    statusPromocionesApi = promocionesApi.FtnCargarPromociones(VarEntorno.dFechaVenta, "TOTAL", sConexionUri);

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

                                /*Carga datos de los COMPLEMENTOS DE PAGO de AUTOVENTA ó PREVENTA*/
                                /*OBSOLETA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                   (VarEntorno.cTipoVenta == 'P'))
                                {
                                    //ComplementosRestServiceAP complementosApi = new ComplementosRestServiceAP();
                                    //StatusRestService statusComplementosApi = new StatusRestService();
                                    //statusComplementosApi = complementosApi.FtnCargarComplementosPagoAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

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
                                    //    VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de los COMPLEMENTOS DE PAGO de REPARTO*/
                                /*OBSOLETA*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    //ComplementosRestServiceR complementosApi = new ComplementosRestServiceR();
                                    //StatusRestService statusComplementosApi = new StatusRestService();
                                    //statusComplementosApi = complementosApi.FtnCargarComplementosPagoR(iIdRuta, dFechaReparto, sConexionUri);

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
                                    //    VarEntorno.iNoRuta = 0;

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
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                   (VarEntorno.cTipoVenta == 'P'))
                                {
                                    DocumentosRestServiceAP documentosApi = new DocumentosRestServiceAP();
                                    StatusRestService statusDocumentosApi = new StatusRestService();

                                    statusDocumentosApi = documentosApi.FtnCargarDocumentosAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de los DOCUMENTOS de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    DocumentosRestServiceR documentosApi = new DocumentosRestServiceR();
                                    StatusRestService statusDocumentosApi = new StatusRestService();

                                    statusDocumentosApi = documentosApi.FtnCargarDocumentosR(iIdRuta, dFechaReparto, sConexionUri);

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de los REQUISITOS DE ENTREGA de REPARTO*/
                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    RequisitosEntregaRestServiceR requisitoEntregaApi = new RequisitosEntregaRestServiceR();
                                    StatusRestService statusRequisitoEntregaApi = new StatusRestService();

                                    statusRequisitoEntregaApi = requisitoEntregaApi.FtnCargarRequisitosEntregaR(iIdRuta, dFechaReparto, sConexionUri);

                                    if (statusRequisitoEntregaApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRequisitosEntrega.Text = "OK";
                                            lblStatusRequisitosEntrega.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Requisitos de Entrega lo especifique durante la Recepción*/
                                            if (statusRequisitoEntregaApi.valor == "NO")
                                            {
                                                lblTituloRequisitosEntrega.Text = "Requisitos de Entrega   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloRequisitosEntrega.Text = "Requisitos de Entrega";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRequisitosEntrega.Text = "ERROR";
                                            lblStatusRequisitosEntrega.TextColor = Color.Red;
                                            lblTituloRequisitosEntrega.Text = "Requisitos de Entrega";
                                            DisplayAlert("Aviso", statusRequisitoEntregaApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los KPI's y RETOS DIARIOS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    /*Carga datos de los KPI's de AUTOVENTA ó PREVENTA*/
                                    KpisRestServiceAP kpisApi = new KpisRestServiceAP();
                                    StatusRestService statusKpisApi = new StatusRestService();
                                    statusKpisApi = kpisApi.FtnCargarKpisAP(VarEntorno.iNoRuta, VarEntorno.dFechaVenta, sConexionUri);

                                    if (statusKpisApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusKpis.Text = "OK";
                                            lblStatusKpis.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay KIP's lo especifique durante la Recepción*/
                                            if (statusKpisApi.valor == "NO")
                                            {
                                                lblTituloKpis.Text = "KPI's  (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloKpis.Text = "KPI's";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusKpis.Text = "ERROR";
                                            lblStatusKpis.TextColor = Color.Red;
                                            lblTituloKpis.Text = "KPI's  (Con Error)  y  Retos Diarios";
                                            DisplayAlert("Aviso", statusKpisApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    /*Carga datos de los RETOS DIARIOS de AUTOVENTA ó PREVENTA*/
                                    RetosDiariosRestServiceAP retosDiariosApi = new RetosDiariosRestServiceAP();
                                    StatusRestService statusRetosDiariosApi = new StatusRestService();
                                    statusRetosDiariosApi = retosDiariosApi.FtnCargarRetosDiariosAP(VarEntorno.iNoRuta, VarEntorno.dFechaVenta, sConexionUri);

                                    if (statusRetosDiariosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusKpis.Text = "OK";
                                            lblStatusKpis.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Retos Diarios lo especifique durante la Recepción*/
                                            if (statusRetosDiariosApi.valor == "NO")
                                            {
                                                lblTituloKpis.Text = lblTituloKpis.Text + "  y  Retos Diarios  (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloKpis.Text = lblTituloKpis.Text + "  y  Retos Diarios";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusKpis.Text = "ERROR";
                                            lblStatusKpis.TextColor = Color.Red;
                                            lblTituloKpis.Text = "KPI's  y  Retos Diarios  (Con Error)";
                                            DisplayAlert("Aviso", statusRetosDiariosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los RETOS DIARIOS de AUTOVENTA ó PREVENTA*/
                                /*OBSOLETA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    //RetosDiariosRestServiceAP retosDiariosApi = new RetosDiariosRestServiceAP();
                                    //StatusRestService statusRetosDiariosApi = new StatusRestService();
                                    //statusRetosDiariosApi = retosDiariosApi.FtnCargarRetosDiariosAP(VarEntorno.iNoRuta, VarEntorno.dFechaVenta, sConexionUri);

                                    //if (statusRetosDiariosApi.status == true)
                                    //{
                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusRetosDiarios.Text = "OK";
                                    //        lblStatusRetosDiarios.TextColor = Color.LimeGreen;
                                    //        /*Valida que si no hay Retos Diarios lo especifique durante la Recepción*/
                                    //        if (statusRetosDiariosApi.valor == "NO")
                                    //        {
                                    //            lblTituloRetosDiarios.Text = "Retos Diarios   (Sin datos)";
                                    //        }
                                    //        else
                                    //        {
                                    //            lblTituloRetosDiarios.Text = "Retos Diarios";
                                    //        }
                                    //    });
                                    //}
                                    //else
                                    //{
                                    //    VarEntorno.iNoRuta = 0;

                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusRetosDiarios.Text = "ERROR";
                                    //        lblStatusRetosDiarios.TextColor = Color.Red;
                                    //        lblTituloRetosDiarios.Text = "Retos Diarios";
                                    //        DisplayAlert("Aviso", statusRetosDiariosApi.mensaje, "OK");
                                    //    });

                                    //    progressDialogRecepcion.Dismiss();
                                    //    return;
                                    //}
                                }

                                /*Carga datos de los PEDIDOS SUGERIDOS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    PedidosSugeridosRestServiceAP pedidosSugeridosApi = new PedidosSugeridosRestServiceAP();
                                    StatusRestService statusPedidosSugeridosApi = new StatusRestService();
                                    statusPedidosSugeridosApi = pedidosSugeridosApi.FtnCargarPedidosSugeridosAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL");

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
                                        VarEntorno.iNoRuta = 0;

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

                                /*Carga datos de los VOLUMEN DE VENTAS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
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
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVolumenVentas.Text = "ERROR";
                                            lblStatusVolumenVentas.TextColor = Color.Red;
                                            lblTituloVolumenVentas.Text = "Volumen de Ventas";
                                            DisplayAlert("Aviso", statusFechaRepartoApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    VolumenVentasRestServiceAP volumenVentasApi = new VolumenVentasRestServiceAP();
                                    StatusRestService statusVolumenVentasApi = new StatusRestService();
                                    statusVolumenVentasApi = volumenVentasApi.FtnCargarVolumenVentasAP(VarEntorno.iNoRuta, dFechaReparto, sConexionUri);

                                    if (statusVolumenVentasApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVolumenVentas.Text = "OK";
                                            lblStatusVolumenVentas.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Volumen de Ventas lo especifique durante la Recepción*/
                                            if (statusVolumenVentasApi.valor == "NO")
                                            {
                                                lblTituloVolumenVentas.Text = "Volumen de Ventas   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloVolumenVentas.Text = "Volumen de Ventas   (Fecha " + dFechaReparto.ToShortDateString() + ")";
                                            }
                                            //await DisplayAlert("Aviso", statusVolumenVentasApi.mensaje, "OK");
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVolumenVentas.Text = "ERROR";
                                            lblStatusVolumenVentas.TextColor = Color.Red;
                                            lblTituloVolumenVentas.Text = "Volumen de Ventas   (Fecha " + dFechaReparto.ToShortDateString() + ")";
                                            DisplayAlert("Aviso", statusVolumenVentasApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los ACTIVOS COMODATADOS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    ActivosComodatosRestServiceAP activosComodatosApi = new ActivosComodatosRestServiceAP();
                                    StatusRestService statusActivosComodatosApi = new StatusRestService();
                                    statusActivosComodatosApi = activosComodatosApi.FtnCargarActivosComodatosAP(VarEntorno.iNoRuta, VarEntorno.cDiaVisita, sConexionUri, VarEntorno.bEsTeleventa);

                                    if (statusActivosComodatosApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusActivosComodatos.Text = "OK";
                                            lblStatusActivosComodatos.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Activos Comodatados lo especifique durante la Recepción*/
                                            if (statusActivosComodatosApi.valor == "NO")
                                            {
                                                lblTituloActivosComodatos.Text = "Activos Comodatados   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloActivosComodatos.Text = "Activos Comodatados";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusActivosComodatos.Text = "ERROR";
                                            lblStatusActivosComodatos.TextColor = Color.Red;
                                            lblTituloActivosComodatos.Text = "Activos Comodatados";
                                            DisplayAlert("Aviso", statusActivosComodatosApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de los CANDADOS DE PRODUCTOS de AUTOVENTA ó PREVENTA*/
                                /*OBSOLETA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    //CandadosProductosRestServiceAP  candadosProductosApi = new CandadosProductosRestServiceAP();
                                    //StatusRestService statusCandadosProductosApi = new StatusRestService();
                                    //statusCandadosProductosApi = candadosProductosApi.FtnCargarCandadosProductosAP(sConexionUri);

                                    //if (statusCandadosProductosApi.status == true)
                                    //{
                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusCandadosProductos.Text = "OK";
                                    //        lblStatusCandadosProductos.TextColor = Color.LimeGreen;
                                    //        /*Valida que si no hay Candados de Productos lo especifique durante la Recepción*/
                                    //        if (statusCandadosProductosApi.valor == "NO")
                                    //        {
                                    //            lblTituloCandadosProductos.Text = "Candados de Productos   (Sin datos)";
                                    //        }
                                    //        else
                                    //        {
                                    //            lblTituloCandadosProductos.Text = "Candados de Productos";
                                    //        }
                                    //    });
                                    //}
                                    //else
                                    //{
                                    //    VarEntorno.iNoRuta = 0;

                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusCandadosProductos.Text = "ERROR";
                                    //        lblStatusCandadosProductos.TextColor = Color.Red;
                                    //        lblTituloCandadosProductos.Text = "Candados de Productos";
                                    //        DisplayAlert("Aviso", statusCandadosProductosApi.mensaje, "OK");
                                    //    });

                                    //    progressDialogRecepcion.Dismiss();
                                    //    return;
                                    //}
                                }

                                /*Carga datos de los CANDADOS DE PRODUCTOS VERSION 2 de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    
                                    CandadosProductosRestServiceAPv2 candadosProductosV2Api = new CandadosProductosRestServiceAPv2();
                                    StatusRestService statusCandadosProductosV2Api = new StatusRestService();
                                    statusCandadosProductosV2Api = candadosProductosV2Api.FtnCargarCandadosProductosAPv2(VarEntorno.dFechaVenta, VarEntorno.dFechaVenta,"TOTAL", sConexionUri);

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

                                /*Carga datos de la ENCUESTA a Clientes y sus OPCIONES DE RESPUESTA de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    /*Carga datos de las ENCUESTA a Clientes de AUTOVENTA ó PREVENTA*/
                                    EncuestasClientesRestServiceAP encuestasClientesApi = new EncuestasClientesRestServiceAP();
                                    StatusRestService statusEncuestasClientesApi = new StatusRestService();
                                    statusEncuestasClientesApi = encuestasClientesApi.FtnCargarEncuestasClientesAP(sConexionUri);

                                    if (statusEncuestasClientesApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEncuestasClientes.Text = "OK";
                                            lblStatusEncuestasClientes.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Encuesta a Clientes lo especifique durante la Recepción*/
                                            if (statusEncuestasClientesApi.valor == "NO")
                                            {
                                                lblTituloEncuestasClientes.Text = "Encuesta  (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloEncuestasClientes.Text = "Encuesta";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEncuestasClientes.Text = "ERROR";
                                            lblStatusEncuestasClientes.TextColor = Color.Red;
                                            lblTituloEncuestasClientes.Text = "Encuesta  (Con Error)  y  Opciones de Respuestas";
                                            DisplayAlert("Aviso", statusEncuestasClientesApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }

                                    /*Carga datos de las OPCIONES DE RESPUESTAS de AUTOVENTA ó PREVENTA*/
                                    OpcionesEncuestasRestServiceAP opcionesEncuestasApi = new OpcionesEncuestasRestServiceAP();
                                    StatusRestService statusOpcionesEncuestasApi = new StatusRestService();
                                    statusOpcionesEncuestasApi = opcionesEncuestasApi.FtnCargarOpcionesEncuestas(sConexionUri);

                                    if (statusOpcionesEncuestasApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEncuestasClientes.Text = "OK";
                                            lblStatusEncuestasClientes.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Opciones de Respuestas lo especifique durante la Recepción*/
                                            if (statusEncuestasClientesApi.valor == "NO")
                                            {
                                                lblTituloEncuestasClientes.Text = lblTituloEncuestasClientes.Text + "  y  Opciones de Respuestas  (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloEncuestasClientes.Text = lblTituloEncuestasClientes.Text + "  y  Opciones de Respuestas";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEncuestasClientes.Text = "ERROR";
                                            lblStatusEncuestasClientes.TextColor = Color.Red;
                                            lblTituloEncuestasClientes.Text = "Encuesta  y  Opciones de Respuestas  (Con Error)";
                                            DisplayAlert("Aviso", statusEncuestasClientesApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                #region ENCUESTA  COMENTADA 
                                /*Carga datos de la ENCUESTA a Clientes y sus OPCIONES DE RESPUESTA de AUTOVENTA ó PREVENTA*/
                                //if ((VarEntorno.cTipoVenta == 'A') ||
                                //    (VarEntorno.cTipoVenta == 'P'))
                                //{
                                //    /*Carga datos de las ENCUESTA a Clientes de AUTOVENTA ó PREVENTA*/
                                //    EncuestasClientesRestServiceAP encuestasClientesApi = new EncuestasClientesRestServiceAP();
                                //    StatusRestService statusEncuestasClientesApi = new StatusRestService();
                                //    statusEncuestasClientesApi = encuestasClientesApi.FtnCargarEncuestasClientesAP(sConexionUri);

                                //    if (statusEncuestasClientesApi.status == true)
                                //    {
                                //        Device.BeginInvokeOnMainThread(() =>
                                //        {
                                //            lblStatusEncuestasClientes.Text = "OK";
                                //            lblStatusEncuestasClientes.TextColor = Color.LimeGreen;
                                //            /*Valida que si no hay Encuesta a Clientes lo especifique durante la Recepción*/
                                //            if (statusEncuestasClientesApi.valor == "NO")
                                //            {
                                //                lblTituloEncuestasClientes.Text = "Encuesta  (Sin datos)";
                                //            }
                                //            else
                                //            {
                                //                lblTituloEncuestasClientes.Text = "Encuesta";
                                //            }
                                //        });
                                //    }
                                //    else
                                //    {
                                //        VarEntorno.iNoRuta = 0;

                                //        Device.BeginInvokeOnMainThread(() =>
                                //        {
                                //            lblStatusEncuestasClientes.Text = "ERROR";
                                //            lblStatusEncuestasClientes.TextColor = Color.Red;
                                //            lblTituloEncuestasClientes.Text = "Encuesta  (Con Error)  y  Opciones de Respuestas";
                                //            DisplayAlert("Aviso", statusEncuestasClientesApi.mensaje, "OK");
                                //        });

                                //        progressDialogRecepcion.Dismiss();
                                //        return;
                                //    }
                                //}
#endregion
                                /*Carga datos de la ANTICIPOS de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    /*Carga datos de las OPCIONES DE Anticipos de AUTOVENTA ó PREVENTA*/
                                    AnticiposClientesRestServiceAP opcionesAnticiposApi = new AnticiposClientesRestServiceAP();
                                    StatusRestService statusAnticiposApi = new StatusRestService();
                                    statusAnticiposApi = opcionesAnticiposApi.FtnCargarAnticiposClientesAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

                                    if (statusAnticiposApi.status == true)
                                    {
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusAnticiposClientes.Text = "OK";
                                            lblStatusAnticiposClientes.TextColor = Color.LimeGreen;
                                            /*Valida que si no hay Opciones de Respuestas lo especifique durante la Recepción*/
                                            if (statusAnticiposApi.valor == "NO")
                                            {
                                                lblTituloAnticiposClientes.Text = lblTituloAnticiposClientes.Text + "   (Sin datos)";
                                            }
                                            else
                                            {
                                                lblTituloAnticiposClientes.Text = lblTituloAnticiposClientes.Text + " ";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        VarEntorno.iNoRuta = 0;

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusAnticiposClientes.Text = "ERROR";
                                            lblStatusAnticiposClientes.TextColor = Color.Red;
                                            lblTituloAnticiposClientes.Text = "Anticipos (Con Error)";
                                            DisplayAlert("Aviso", statusAnticiposApi.mensaje, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                    }
                                }

                                /*Carga datos de la Telefonos de AUTOVENTA ó PREVENTA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    /*Carga datos de las OPCIONES DE Telefonos de AUTOVENTA ó PREVENTA*/
                                    TelefonosClientesRestServiceAP opcionesTelefonosApi = new TelefonosClientesRestServiceAP();
                                    StatusRestService statusTelefonosApi = new StatusRestService();
                                    statusTelefonosApi = opcionesTelefonosApi.FtnCargarTelefonosClientesAP(iIdRuta, VarEntorno.cDiaVisita, sConexionUri, "TOTAL", VarEntorno.bEsTeleventa);

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

                                if (VarEntorno.cTipoVenta == 'R')
                                {
                                    TelefonosClientesRestServiceR opcionesTelefonosApi = new TelefonosClientesRestServiceR();
                                    StatusRestService statusTelefonosApi = new StatusRestService();
                                    statusTelefonosApi = opcionesTelefonosApi.FtnCargarTelefonosClientesR(iIdRuta, dFechaReparto, sConexionUri);

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

                                /*Carga datos de las OPCIONES DE RESPUESTA de AUTOVENTA ó PREVENTA*/
                                /*OBSOLETA*/
                                if ((VarEntorno.cTipoVenta == 'A') ||
                                    (VarEntorno.cTipoVenta == 'P'))
                                {
                                    //OpcionesEncuestasRestServiceAP opcionesEncuestasApi = new OpcionesEncuestasRestServiceAP();
                                    //StatusRestService statusOpcionesEncuestasApi = new StatusRestService();
                                    //statusOpcionesEncuestasApi = opcionesEncuestasApi.FtnCargarOpcionesEncuestas(sConexionUri);

                                    //if (statusOpcionesEncuestasApi.status == true)
                                    //{
                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusOpcionesEncuestas.Text = "OK";
                                    //        lblStatusOpcionesEncuestas.TextColor = Color.LimeGreen;
                                    //        /*Valida que si no hay Opciones de Encuestas lo especifique durante la Recepción*/
                                    //        if (statusOpcionesEncuestasApi.valor == "NO")
                                    //        {
                                    //            lblTituloOpcionesEncuestas.Text = "Opciones de Encuestas   (Sin datos)";
                                    //        }
                                    //        else
                                    //        {
                                    //            lblTituloOpcionesEncuestas.Text = "Opciones de Encuestas";
                                    //        }
                                    //    });
                                    //}
                                    //else
                                    //{
                                    //    VarEntorno.iNoRuta = 0;

                                    //    Device.BeginInvokeOnMainThread(() =>
                                    //    {
                                    //        lblStatusOpcionesEncuestas.Text = "ERROR";
                                    //        lblStatusOpcionesEncuestas.TextColor = Color.Red;
                                    //        lblTituloOpcionesEncuestas.Text = "Opciones de Encuestas";
                                    //        DisplayAlert("Aviso", statusOpcionesEncuestasApi.mensaje, "OK");
                                    //    });

                                    //    progressDialogRecepcion.Dismiss();
                                    //    return;
                                    //}
                                }

                                progressDialogRecepcion.Dismiss();

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("Aviso", "La Recepción de la Ruta " + iIdRuta.ToString() + " ha terminado correctamente.", "OK");
                                    Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Ruta", iIdRuta.ToString());
                                    Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Version", VarEntorno.sVersionApp);
                                    btnRecepcion.IsEnabled = false;
                                });
                            });
                        }
                        else
                        {
                            await DisplayAlert("¡Atención!", statusConexion.mensaje + " para realizar la Recepción.", "OK");
                        }
                    }
                }
            }
        }

        /*Método para limpiar las etiquetas para una nueva Recepción de datos*/
        public void FtnLimpiarStatusRecepcion()
        {
            /*Esta información es OBLIGATORIA de cargar, si alguno de ellos falla, no se realiza la Recepción correctamente*/
            lblStatusPasswords.Text = "";
            lblStatusRutas.Text = "";
            lblTituloRutas.Text = "Ruta";
            lblStatusClientes.Text = "";
            lblTituloClientes.Text = "Clientes";
            lblStatusProductos.Text = "";
            lblStatusExistencias.Text = "";
            lblStatusEnvases.Text = "";
            lblStatusListasPrecios.Text = "";
            lblStatusDetalleVentas.Text = "";
            lblStatusOrdenTickets.Text = "";
            lblStatusDepartamentos.Text = "";
            lblTituloDepartamentos.Text = "Departamentos y Segmentos";
            lblStatusConceptosDev.Text = "";
            lblStatusConceptosNoVenta.Text = "";


            /*Esta información es OPCIONAL de cargar, si no hay datos o falla alguno de ellos, la Recepción continúa*/
            lblStatusFacturasVentas.Text = "";
            lblTituloFacturasVentas.Text = "Facturas de Ventas";
            lblStatusBonificaciones.Text = "";
            lblTituloBonificaciones.Text = "Bonificaciones";
            lblStatusPromociones.Text = "";
            lblTituloPromociones.Text = "Promociones";
            //lblStatusComplementos.Text = "";
            //lblTituloComplementos.Text = "Complementos de Pago";
            lblStatusDocumentos.Text = "";
            lblTituloDocumentos.Text = "Documentos";
            lblStatusEnvasesSugeridos.Text = "";
            lblTituloEnvasesSugeridos.Text = "Envases Sugeridos";
            lblStatusRequisitosEntrega.Text = "";
            lblTituloRequisitosEntrega.Text = "Requisitos de Entrega";
            lblStatusKpis.Text = "";
            lblTituloKpis.Text = "KPI's  y  Retos Diarios";
            //lblStatusRetosDiarios.Text = "";
            //lblTituloRetosDiarios.Text = "Retos Diarios";
            lblStatusPedidosSugeridos.Text = "";
            lblTituloPedidosSugeridos.Text = "Pedidos Sugeridos";
            lblStatusVolumenVentas.Text = "";
            lblTituloVolumenVentas.Text = "Volumen de Ventas";
            lblStatusActivosComodatos.Text = "";
            lblTituloActivosComodatos.Text = "Activos Comodatados";
            //lblStatusCandadosProductos.Text = "";
            //lblTituloCandadosProductos.Text = "Candados de Productos";
            lblStatusEncuestasClientes.Text = "";
            lblTituloEncuestasClientes.Text = "Encuesta  y  Opciones de Respuestas";
            //lblTituloEncuestasClientes.Text = "Encuestas a Clientes y sus Opciones";
            //lblStatusOpcionesEncuestas.Text = "";
            //lblTituloOpcionesEncuestas.Text = "Opciones de Encuestas";
        }

        /*Método para abrir la pantalla de MENÚ PRINCIPAL*/
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            //this.Navigation.PushModalAsync(new frmMenuPrincipal());
            this.Navigation.PopModalAsync();
        }
    }
}