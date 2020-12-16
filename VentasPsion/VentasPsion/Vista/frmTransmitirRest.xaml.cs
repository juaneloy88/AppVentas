using Base;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.ServicioApi;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmTransmitirRest : ContentPage
	{
        TransmitirRestService transmitir = new TransmitirRestService();
        vmStatusClientes vmstatusClientes = new vmStatusClientes();

        #region Declaración de Labels para Status de Recepción
        Label lblStatusVentaCabecera, lblVentaCabecera, lblResumenVentaCabecera = null;
        Label lblStatusVentaPagos, lblVentaPagos, lblResumenVentaPagos = null;
        Label lblStatusVentaDetalle, lblVentaDetalle, lblResumenVentaDetalle = null;
        Label lblStatusEnvase, lblEnvase, lblResumenEnvase = null;        
        Label lblStatusBonificaciones, lblBonificaciones, lblResumenBonificaciones = null;
        Label lblStatusClientesStatus, lblClientesStatus, lblResumenClientesStatus = null;
        Label lblStatusDevoluciones, lblDevoluciones, lblResumenDevoluciones = null;
        Label lblStatusGps, lblGps, lblResumenGps = null;
        Label lblStatusRespuesta, lblRespuesta, lblResumenRespuesta = null;
        //Label lblStatusSolicitudes, lblSolicitudes, lblResumenSolicitudes = null;
        Label lblStatusInfoRuta, lblInfoRuta, lblResumenInfoRuta = null;
        //Label lblStatusPagosProgramados, lblPagosProgramados, lblResumenPagosProgramados = null;
        Label lblStatusEmpleados, lblEmpleados, lblResumenEmpleados = null;
        Label lblStatusPagareClientes, lblPagareClientes, lblResumenPagareClientes = null;
        Label lblStatusEnvaseSugerido, lblEnvaseSugerido, lblResumenEnvaseSugerido = null;
        Label lblStatusDocumentosCabecera, lblDocumentosCabecera, lblResumenDocumentosCabecera = null;
        Label lblStatusDocumentosDetalle, lblDocumentosDetalle, lblResumenDocumentosDetalle = null;
        Label lblStatusRequisitos, lblRequisitos, lblResumenRequisitos = null;
        Label lblStatusClientesCompetencia, lblClientesCompetencia, lblResumenClientesCompetencia = null;
        Label lblStatusAnticipos, lblAnticipos, lblResumenAnticipos = null;

        Label lblStatusTelefonosClientes, lblTelefonosClientes, lblResumenTelefonosClientes = null;

        #endregion

        public frmTransmitirRest ()
		{
			InitializeComponent ();

            txtRuta.Text = "Ruta:" + VarEntorno.iNoRuta.ToString();

            #region Inicialización de Labels con sus propiedades para el Status de Recepción
            lblStatusVentaCabecera = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblVentaCabecera = new Label { Text = "Venta Cabecera", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };            
            lblResumenVentaCabecera = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            //lblStatusVentaPagos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            //lblVentaPagos = new Label { Text = "Venta Pagos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            //lblResumenVentaPagos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusVentaPagos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblVentaPagos = new Label { Text = "Pagos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenVentaPagos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusVentaDetalle = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblVentaDetalle = new Label { Text = "Venta Detalle", FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenVentaDetalle = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusEnvase = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblEnvase = new Label { Text = "Envase", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenEnvase = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusBonificaciones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblBonificaciones = new Label { Text = "Bonificaciones", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenBonificaciones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusClientesStatus = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblClientesStatus = new Label { Text = "Clientes Status", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenClientesStatus = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusDevoluciones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblDevoluciones = new Label { Text = "Devoluciones", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenDevoluciones = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusGps = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblGps = new Label { Text = "GPS", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenGps = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            //lblStatusRespuesta = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            //lblRespuesta = new Label { Text = "Respuesta", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            //lblResumenRespuesta = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusRespuesta = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblRespuesta = new Label { Text = "Resp y Solicitudes", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenRespuesta = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            //lblStatusSolicitudes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            //lblSolicitudes = new Label { Text = "Solicitudes", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            //lblResumenSolicitudes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusInfoRuta = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblInfoRuta = new Label { Text = "Info Ruta", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenInfoRuta = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            //lblStatusPagosProgramados = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            //lblPagosProgramados = new Label { Text = "Pagos Programados", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            //lblResumenPagosProgramados = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusEmpleados = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblEmpleados = new Label { Text = "Empleados", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenEmpleados = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusPagareClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblPagareClientes = new Label { Text = "Pagare Clientes", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenPagareClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusEnvaseSugerido = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblEnvaseSugerido = new Label { Text = "Envase Sugerido", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenEnvaseSugerido = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };            

            lblStatusDocumentosCabecera = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblDocumentosCabecera = new Label { Text = "Doctos Cabecera", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenDocumentosCabecera = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };            

            lblStatusDocumentosDetalle = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblDocumentosDetalle = new Label { Text = "Doctos Detalle", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenDocumentosDetalle = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusRequisitos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblRequisitos = new Label { Text = "Requisitos para Surtir", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenRequisitos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusClientesCompetencia = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblClientesCompetencia = new Label { Text = "Ctes Competencia", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenClientesCompetencia = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusAnticipos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblAnticipos = new Label { Text = "Anticipos", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenAnticipos = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            lblStatusTelefonosClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };
            lblTelefonosClientes = new Label { Text = "Telefonos Clientes", VerticalTextAlignment = TextAlignment.Center, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.Black };
            lblResumenTelefonosClientes = new Label { Text = "", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) };

            #endregion Inicialización de Labels con sus propiedades para el Status de Recepción

            #region Agrega los Labels para el Status de Recepción al Grid, según el Tipo de Venta o Perfil (AUTOVENTA, REPARTO ó PREVENTA)
            switch (VarEntorno.cTipoVenta)
            {
                case 'A': /*AUTOVENTA*/
                    #region 
                    grdTransmitir.Children.Add(lblStatusVentaCabecera, 0, 0);
                    grdTransmitir.Children.Add(lblVentaCabecera, 1, 0);
                    grdTransmitir.Children.Add(lblResumenVentaCabecera, 2, 0);
                    grdTransmitir.Children.Add(lblStatusVentaPagos, 0, 1);
                    grdTransmitir.Children.Add(lblVentaPagos, 1, 1);
                    grdTransmitir.Children.Add(lblResumenVentaPagos, 2, 1);
                    grdTransmitir.Children.Add(lblStatusVentaDetalle, 0, 2);
                    grdTransmitir.Children.Add(lblVentaDetalle, 1, 2);
                    grdTransmitir.Children.Add(lblResumenVentaDetalle, 2, 2);
                    grdTransmitir.Children.Add(lblStatusEnvase, 0, 3);
                    grdTransmitir.Children.Add(lblEnvase, 1, 3);
                    grdTransmitir.Children.Add(lblResumenEnvase, 2, 3);
                    grdTransmitir.Children.Add(lblStatusBonificaciones, 0, 4);
                    grdTransmitir.Children.Add(lblBonificaciones, 1, 4);
                    grdTransmitir.Children.Add(lblResumenBonificaciones, 2, 4);
                    grdTransmitir.Children.Add(lblStatusClientesStatus, 0, 5);
                    grdTransmitir.Children.Add(lblClientesStatus, 1, 5);
                    grdTransmitir.Children.Add(lblResumenClientesStatus, 2, 5);
                    grdTransmitir.Children.Add(lblStatusInfoRuta, 0, 6);
                    grdTransmitir.Children.Add(lblInfoRuta, 1, 6);
                    grdTransmitir.Children.Add(lblResumenInfoRuta, 2, 6);
                    grdTransmitir.Children.Add(lblStatusGps, 0, 7);
                    grdTransmitir.Children.Add(lblGps, 1, 7);
                    grdTransmitir.Children.Add(lblResumenGps, 2, 7);
                    grdTransmitir.Children.Add(lblStatusPagareClientes, 0, 8);
                    grdTransmitir.Children.Add(lblPagareClientes, 1, 8);
                    grdTransmitir.Children.Add(lblResumenPagareClientes, 2, 8);
                    //grdTransmitir.Children.Add(lblStatusRespuesta, 0, 9);
                    //grdTransmitir.Children.Add(lblRespuesta, 1, 9);
                    //grdTransmitir.Children.Add(lblResumenRespuesta, 2, 9);
                    //grdTransmitir.Children.Add(lblStatusSolicitudes, 0, 10);
                    //grdTransmitir.Children.Add(lblSolicitudes, 1, 10);
                    //grdTransmitir.Children.Add(lblResumenSolicitudes, 2, 10);                    
                    grdTransmitir.Children.Add(lblStatusDocumentosCabecera, 0, 9);
                    grdTransmitir.Children.Add(lblDocumentosCabecera, 1, 9);
                    grdTransmitir.Children.Add(lblResumenDocumentosCabecera, 2, 9);
                    grdTransmitir.Children.Add(lblStatusDocumentosDetalle, 0, 10);
                    grdTransmitir.Children.Add(lblDocumentosDetalle, 1, 10);
                    grdTransmitir.Children.Add(lblResumenDocumentosDetalle, 2, 10);
                    grdTransmitir.Children.Add(lblStatusAnticipos, 0, 11);
                    grdTransmitir.Children.Add(lblAnticipos, 1, 11);
                    grdTransmitir.Children.Add(lblResumenAnticipos, 2, 11);

                    grdTransmitir.Children.Add(lblStatusTelefonosClientes, 0, 12);
                    grdTransmitir.Children.Add(lblTelefonosClientes, 1, 12);
                    grdTransmitir.Children.Add(lblResumenTelefonosClientes, 2, 12);

                    break;
                    
                    #endregion
                case 'R': /*REPARTO*/
                    #region
                    grdTransmitir.Children.Add(lblStatusVentaCabecera, 0, 0);
                    grdTransmitir.Children.Add(lblVentaCabecera, 1, 0);
                    grdTransmitir.Children.Add(lblResumenVentaCabecera, 2, 0);

                    grdTransmitir.Children.Add(lblStatusVentaPagos, 0, 1);
                    grdTransmitir.Children.Add(lblVentaPagos, 1, 1);
                    grdTransmitir.Children.Add(lblResumenVentaPagos, 2, 1);

                    grdTransmitir.Children.Add(lblStatusVentaDetalle, 0, 2);
                    grdTransmitir.Children.Add(lblVentaDetalle, 1, 2);
                    grdTransmitir.Children.Add(lblResumenVentaDetalle, 2, 2);
                    grdTransmitir.Children.Add(lblStatusEnvase, 0, 3);
                    grdTransmitir.Children.Add(lblEnvase, 1, 3);
                    grdTransmitir.Children.Add(lblResumenEnvase, 2, 3);
                    grdTransmitir.Children.Add(lblStatusClientesStatus, 0, 4);
                    grdTransmitir.Children.Add(lblClientesStatus, 1, 4);
                    grdTransmitir.Children.Add(lblResumenClientesStatus, 2, 4);
                    grdTransmitir.Children.Add(lblStatusDevoluciones, 0, 5);
                    grdTransmitir.Children.Add(lblDevoluciones, 1, 5);
                    grdTransmitir.Children.Add(lblResumenDevoluciones, 2, 5);
                    grdTransmitir.Children.Add(lblStatusInfoRuta, 0, 6);
                    grdTransmitir.Children.Add(lblInfoRuta, 1, 6);
                    grdTransmitir.Children.Add(lblResumenInfoRuta, 2, 6);
                    grdTransmitir.Children.Add(lblStatusGps, 0, 7);
                    grdTransmitir.Children.Add(lblGps, 1, 7);
                    grdTransmitir.Children.Add(lblResumenGps, 2, 7);
                    //grdTransmitir.Children.Add(lblStatusSolicitudes, 0, 8);
                    //grdTransmitir.Children.Add(lblSolicitudes, 1, 8);
                    //grdTransmitir.Children.Add(lblResumenSolicitudes, 2, 8);
                    
                    grdTransmitir.Children.Add(lblStatusDocumentosCabecera, 0, 9);
                    grdTransmitir.Children.Add(lblDocumentosCabecera, 1, 9);
                    grdTransmitir.Children.Add(lblResumenDocumentosCabecera, 2, 9);

                    grdTransmitir.Children.Add(lblStatusDocumentosDetalle, 0, 10);
                    grdTransmitir.Children.Add(lblDocumentosDetalle, 1, 10);
                    grdTransmitir.Children.Add(lblResumenDocumentosDetalle, 2, 10);
                    
                    grdTransmitir.Children.Add(lblStatusRequisitos, 0, 8);
                    grdTransmitir.Children.Add(lblRequisitos, 1, 8);
                    grdTransmitir.Children.Add(lblResumenRequisitos, 2, 8);

                    grdTransmitir.Children.Add(lblStatusTelefonosClientes, 0, 11);
                    grdTransmitir.Children.Add(lblTelefonosClientes, 1, 11);
                    grdTransmitir.Children.Add(lblResumenTelefonosClientes, 2, 11);
                    break;
                #endregion
                case 'P': /*PREVENTA*/
                    #region
                    grdTransmitir.Children.Add(lblStatusVentaCabecera, 0, 0);
                    grdTransmitir.Children.Add(lblVentaCabecera, 1, 0);
                    grdTransmitir.Children.Add(lblResumenVentaCabecera, 2, 0);
                    grdTransmitir.Children.Add(lblStatusVentaPagos, 0, 1);
                    grdTransmitir.Children.Add(lblVentaPagos, 1, 1);
                    grdTransmitir.Children.Add(lblResumenVentaPagos, 2, 1);
                    grdTransmitir.Children.Add(lblStatusVentaDetalle, 0, 2);
                    grdTransmitir.Children.Add(lblVentaDetalle, 1, 2);
                    grdTransmitir.Children.Add(lblResumenVentaDetalle, 2, 2);
                    grdTransmitir.Children.Add(lblStatusEnvase, 0, 3);
                    grdTransmitir.Children.Add(lblEnvase, 1, 3);
                    grdTransmitir.Children.Add(lblResumenEnvase, 2, 3);
                    grdTransmitir.Children.Add(lblStatusBonificaciones, 0, 4);
                    grdTransmitir.Children.Add(lblBonificaciones, 1, 4);
                    grdTransmitir.Children.Add(lblResumenBonificaciones, 2, 4);
                    grdTransmitir.Children.Add(lblStatusInfoRuta, 0, 5);
                    grdTransmitir.Children.Add(lblInfoRuta, 1, 5);
                    grdTransmitir.Children.Add(lblResumenInfoRuta, 2, 5);
                    grdTransmitir.Children.Add(lblStatusEmpleados, 0, 6);
                    grdTransmitir.Children.Add(lblEmpleados, 1, 6);
                    grdTransmitir.Children.Add(lblResumenEmpleados, 2, 6);
                    //grdTransmitir.Children.Add(lblStatusPagosProgramados, 0, 7);
                    //grdTransmitir.Children.Add(lblPagosProgramados, 1, 7);
                    //grdTransmitir.Children.Add(lblResumenPagosProgramados, 2, 7);                    
                    grdTransmitir.Children.Add(lblStatusClientesCompetencia, 0, 7);
                    grdTransmitir.Children.Add(lblClientesCompetencia, 1, 7);
                    grdTransmitir.Children.Add(lblResumenClientesCompetencia, 2, 7);
                    grdTransmitir.Children.Add(lblStatusGps, 0, 8);
                    grdTransmitir.Children.Add(lblGps, 1, 8);
                    grdTransmitir.Children.Add(lblResumenGps, 2, 8);
                    grdTransmitir.Children.Add(lblStatusPagareClientes, 0, 9);
                    grdTransmitir.Children.Add(lblPagareClientes, 1, 9);
                    grdTransmitir.Children.Add(lblResumenPagareClientes, 2, 9);
                    //grdTransmitir.Children.Add(lblStatusRespuesta, 0, 10);
                    //grdTransmitir.Children.Add(lblRespuesta, 1, 10);
                    //grdTransmitir.Children.Add(lblResumenRespuesta, 2, 10);
                    //grdTransmitir.Children.Add(lblStatusSolicitudes, 0, 11);
                    //grdTransmitir.Children.Add(lblSolicitudes, 1, 11);
                    //grdTransmitir.Children.Add(lblResumenSolicitudes, 2, 11);
                    grdTransmitir.Children.Add(lblStatusEnvaseSugerido, 0, 10);
                    grdTransmitir.Children.Add(lblEnvaseSugerido, 1, 10);
                    grdTransmitir.Children.Add(lblResumenEnvaseSugerido, 2, 10);                    
                    grdTransmitir.Children.Add(lblStatusDocumentosCabecera, 0, 11);
                    grdTransmitir.Children.Add(lblDocumentosCabecera, 1, 11);
                    grdTransmitir.Children.Add(lblResumenDocumentosCabecera, 2, 11);
                    grdTransmitir.Children.Add(lblStatusDocumentosDetalle, 0, 12);
                    grdTransmitir.Children.Add(lblDocumentosDetalle, 1, 12);
                    grdTransmitir.Children.Add(lblResumenDocumentosDetalle, 2, 12);
                    grdTransmitir.Children.Add(lblStatusAnticipos, 0, 13);
                    grdTransmitir.Children.Add(lblAnticipos, 1, 13);
                    grdTransmitir.Children.Add(lblResumenAnticipos, 2, 13);

                    grdTransmitir.Children.Add(lblStatusTelefonosClientes, 0, 14);
                    grdTransmitir.Children.Add(lblTelefonosClientes, 1, 14);
                    grdTransmitir.Children.Add(lblResumenTelefonosClientes, 2, 14);

                    break;
                    #endregion
                default:
                    break;
            }
            #endregion
        }

        #region Botón de Enviar
        public async void OnClickedEnviar(object sender, EventArgs args)
        {
            limpiarGrid();
            string sRespuesta = string.Empty;
            int iRuta = VarEntorno.iNoRuta;
            string sUriConexion = string.Empty;

            try
            {
                //Obtiene el la URI para poder transmitir la información
                sUriConexion = validaConexion();

                if (sUriConexion == "Ok")
                {
                    Utilerias utilerias = new Utilerias();
                    var progressDialogRecepcion = utilerias.crearProgressDialog("Transmisión", "Enviando información...");
                    progressDialogRecepcion.Show();

                    #region Procedimiento para el envío de Autoventa y Preventa
                    if (VarEntorno.cTipoVenta == 'A' || VarEntorno.cTipoVenta == 'P')
                    {
                        /*
                        //Validación de que todos los clientes sean visitados
                        List<clientes_estatus> vLista = await vmstatusClientes.obtieneClientesSinVisita();

                        //Verifica la bandera de visitar todos los clientes
                        if (VarEntorno.bVisitaAllCtsReparto)
                        {
                            if (vLista.Count >= 1)
                            {
                                await DisplayAlert("Aviso", "Faltan Clientes por Visitar", "OK");
                                progressDialogRecepcion.Dismiss();
                                btnEnviar.IsEnabled = false;
                            }
                            else
                            {
                                btnEnviar.IsEnabled = true;
                            }
                        }
                        else
                        {
                            btnEnviar.IsEnabled = true;
                        }
                        */
                        if (btnEnviar.IsEnabled)
                        {
                            //Válida si la Ruta ya fue cerrada
                            sRespuesta = await transmitir.validaCierreRuta(iRuta);

                            if (sRespuesta == "\"1\"")
                            {
                                await DisplayAlert("Aviso", "La Ruta ya se encuentra CERRADA", "OK");
                                progressDialogRecepcion.Dismiss();
                                btnEnviar.IsEnabled = false;
                            }
                            else
                            {
                                #region Envío de Autoventa
                                if (VarEntorno.cTipoVenta == 'A')
                                {
                                    //Validación para saber si los anticipos se encuentran relacionados
                                    sRespuesta = transmitir.ValidaRelacionAnticipos();

                                    if (sRespuesta == "Ok")
                                    {
                                        #region Procedimiento para enviar venta_cabecera
                                        sRespuesta = await transmitir.enviarVentaCabecera();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaCabecera.Text = "Ok";
                                                    lblStatusVentaCabecera.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaCabecera.Text = "Ok";
                                                    lblStatusVentaCabecera.TextColor = Color.LimeGreen;
                                                    lblResumenVentaCabecera.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaCabecera.Text = "Error";
                                                    lblStatusVentaCabecera.TextColor = Color.Red;
                                                    DisplayAlert("Aviso", sRespuesta, "OK");
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar venta_cabecera

                                        #region Procedimiento para enviar venta_pagos
                                        sRespuesta = await transmitir.enviarVentaPagos();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Ok";
                                                    lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Ok";
                                                    lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                                    lblResumenVentaPagos.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Error";
                                                    lblStatusVentaPagos.TextColor = Color.Red;
                                                    DisplayAlert("Aviso", sRespuesta, "OK");
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar venta_pagos

                                        #region Procedimiento para enviar venta_detalle
                                        sRespuesta = await transmitir.enviarVentaDetalle();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaDetalle.Text = "Ok";
                                                    lblStatusVentaDetalle.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaDetalle.Text = "Ok";
                                                    lblStatusVentaDetalle.TextColor = Color.LimeGreen;
                                                    lblResumenVentaDetalle.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaDetalle.Text = "Error";
                                                    lblStatusVentaDetalle.TextColor = Color.Red;
                                                    DisplayAlert("Aviso", sRespuesta, "OK");
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar venta_detalle

                                        #region Procedimiento para enviar envase
                                        sRespuesta = await transmitir.enviarEnvase();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvase.Text = "Ok";
                                                    lblStatusEnvase.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvase.Text = "Ok";
                                                    lblStatusEnvase.TextColor = Color.LimeGreen;
                                                    lblResumenEnvase.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvase.Text = "Error";
                                                    lblStatusEnvase.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar envase

                                        #region Procedimiento para enviar bonificaciones
                                        sRespuesta = await transmitir.enviarBonificaciones();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusBonificaciones.Text = "Ok";
                                                    lblStatusBonificaciones.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusBonificaciones.Text = "Ok";
                                                    lblStatusBonificaciones.TextColor = Color.LimeGreen;
                                                    lblResumenBonificaciones.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusBonificaciones.Text = "Error";
                                                    lblStatusBonificaciones.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar bonificaciones

                                        #region Procedimiento para enviar clientes_estatus
                                        sRespuesta = await transmitir.enviarClientesEstatus();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesStatus.Text = "Ok";
                                                    lblStatusClientesStatus.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesStatus.Text = "Ok";
                                                    lblStatusClientesStatus.TextColor = Color.LimeGreen;
                                                    lblResumenClientesStatus.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesStatus.Text = "Error";
                                                    lblStatusClientesStatus.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar clientes_estatus                                

                                        #region Procedimiento para enviar gps
                                        sRespuesta = await transmitir.enviarGps();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusGps.Text = "Ok";
                                                    lblStatusGps.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusGps.Text = "Ok";
                                                    lblStatusGps.TextColor = Color.LimeGreen;
                                                    lblResumenGps.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusGps.Text = "Error";
                                                    lblStatusGps.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar gps

                                        #region Procedimiento para enviar Anticipos
                                        sRespuesta = await transmitir.EnviarAnticipos();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusAnticipos.Text = "Ok";
                                                    lblStatusAnticipos.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusAnticipos.Text = "Ok";
                                                    lblStatusAnticipos.TextColor = Color.LimeGreen;
                                                    lblResumenAnticipos.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusAnticipos.Text = "Error";
                                                    lblStatusAnticipos.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }

                                        #endregion Procedimiento para enviar Anticipos

                                        #region Procedimiento para enviar documentos_cabecera
                                        sRespuesta = await transmitir.enviarDocumentosCabecera();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosCabecera.Text = "Ok";
                                                    lblStatusDocumentosCabecera.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosCabecera.Text = "Ok";
                                                    lblStatusDocumentosCabecera.TextColor = Color.LimeGreen;
                                                    lblResumenDocumentosCabecera.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosCabecera.Text = "Error";
                                                    lblStatusDocumentosCabecera.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar documentos_cabecera

                                        #region Procedimiento para enviar documentos_detalle
                                        sRespuesta = await transmitir.enviarDocumentosDetalle();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosDetalle.Text = "Ok";
                                                    lblStatusDocumentosDetalle.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosDetalle.Text = "Ok";
                                                    lblStatusDocumentosDetalle.TextColor = Color.LimeGreen;
                                                    lblResumenDocumentosDetalle.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosDetalle.Text = "Error";
                                                    lblStatusDocumentosDetalle.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar documentos_detalle

                                        #region Procedimiento para enviar horario, kilometraje y versión de la ruta
                                        sRespuesta = await transmitir.enviarInfoRuta();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusInfoRuta.Text = "Ok";
                                                    lblStatusInfoRuta.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusInfoRuta.Text = "Ok";
                                                    lblStatusInfoRuta.TextColor = Color.LimeGreen;
                                                    lblResumenInfoRuta.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusInfoRuta.Text = "Error";
                                                    lblStatusInfoRuta.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar horario, kilometraje y versión de la ruta

                                        #region Procedimiento para enviar clientes_competencia
                                        sRespuesta = await transmitir.enviarClientesCompetencia();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesCompetencia.Text = "Ok";
                                                    lblStatusClientesCompetencia.TextColor = Color.LimeGreen;
                                                    lblClientesCompetencia.Text = "Ctes Competencia";
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesCompetencia.Text = "Ok";
                                                    lblStatusClientesCompetencia.TextColor = Color.LimeGreen;
                                                    lblResumenClientesCompetencia.Text = sRespuesta;
                                                    lblClientesCompetencia.Text = "Ctes Competencia";
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesCompetencia.Text = "Error";
                                                    lblStatusClientesCompetencia.TextColor = Color.Red;
                                                    lblClientesCompetencia.Text = "Ctes Competencia";
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar documentos_cabecera
                                     
                                        #region Procedimiento para enviar pagare_clientes
                                        sRespuesta = await transmitir.enviarPagareClientes();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPagareClientes.Text = "Ok";
                                                    lblStatusPagareClientes.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPagareClientes.Text = "Ok";
                                                    lblStatusPagareClientes.TextColor = Color.LimeGreen;
                                                    lblResumenPagareClientes.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPagareClientes.Text = "Error";
                                                    lblStatusPagareClientes.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar pagare_clientes

                                        #region Procedimiento para enviar respuestas
                                        sRespuesta = await transmitir.enviarRespuestas();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                    lblResumenRespuesta.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Error";
                                                    lblStatusRespuesta.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar respuestas

                                        #region Procedimiento para enviar solicitudes
                                        sRespuesta = await transmitir.enviarSolicitudes();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                    lblResumenRespuesta.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Error";
                                                    lblStatusRespuesta.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar solicitudes                                                                                                                

                                        #region Procedimiento para enviar Telefonos_Clientes
                                        sRespuesta = await transmitir.EnviarTelefonosClientes();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusTelefonosClientes.Text = "Ok";
                                                    lblStatusTelefonosClientes.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusTelefonosClientes.Text = "Ok";
                                                    lblStatusTelefonosClientes.TextColor = Color.LimeGreen;
                                                    lblResumenTelefonosClientes.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusTelefonosClientes.Text = "Error";
                                                    lblStatusTelefonosClientes.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar Telefonos_Clientes
                                    }
                                    else
                                    {
                                        DisplayAlert("Aviso", sRespuesta, "OK");
                                    }
                                }
                                #endregion Envío de Autoventa

                                #region Envío de Preventa
                                if (VarEntorno.cTipoVenta == 'P')
                                {
                                    //Validación para saber si los anticipos se encuentran relacionados
                                    sRespuesta = transmitir.ValidaRelacionAnticipos();

                                    if (sRespuesta == "Ok")
                                    {
                                        #region Procedimiento para enviar venta_cabecera
                                        sRespuesta = await transmitir.enviarVentaCabecera();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaCabecera.Text = "Ok";
                                                    lblStatusVentaCabecera.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaCabecera.Text = "Ok";
                                                    lblStatusVentaCabecera.TextColor = Color.LimeGreen;
                                                    lblResumenVentaCabecera.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaCabecera.Text = "Error";
                                                    lblStatusVentaCabecera.TextColor = Color.Red;
                                                    DisplayAlert("Aviso", sRespuesta, "OK");
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar venta_cabecera

                                        #region Procedimiento para enviar venta_pagos
                                        sRespuesta = await transmitir.enviarVentaPagos();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Ok";
                                                    lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Ok";
                                                    lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                                    lblResumenVentaPagos.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Error";
                                                    lblStatusVentaPagos.TextColor = Color.Red;
                                                    DisplayAlert("Aviso", sRespuesta, "OK");
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar venta_pagos

                                        #region Procedimiento para enviar venta_detalle
                                        sRespuesta = await transmitir.enviarVentaDetalle();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaDetalle.Text = "Ok";
                                                    lblStatusVentaDetalle.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaDetalle.Text = "Ok";
                                                    lblStatusVentaDetalle.TextColor = Color.LimeGreen;
                                                    lblResumenVentaDetalle.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaDetalle.Text = "Error";
                                                    lblStatusVentaDetalle.TextColor = Color.Red;
                                                    DisplayAlert("Aviso", sRespuesta, "OK");
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar venta_detalle

                                        #region Procedimiento para enviar envase
                                        sRespuesta = await transmitir.enviarEnvase();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvase.Text = "Ok";
                                                    lblStatusEnvase.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvase.Text = "Ok";
                                                    lblStatusEnvase.TextColor = Color.LimeGreen;
                                                    lblResumenEnvase.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvase.Text = "Error";
                                                    lblStatusEnvase.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar envase

                                        #region Procedimiento para enviar bonificaciones
                                        sRespuesta = await transmitir.enviarBonificaciones();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusBonificaciones.Text = "Ok";
                                                    lblStatusBonificaciones.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusBonificaciones.Text = "Ok";
                                                    lblStatusBonificaciones.TextColor = Color.LimeGreen;
                                                    lblResumenBonificaciones.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusBonificaciones.Text = "Error";
                                                    lblStatusBonificaciones.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar bonificaciones

                                        #region Procedimiento para enviar clientes_estatus
                                        sRespuesta = await transmitir.enviarClientesEstatus();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesStatus.Text = "Ok";
                                                    lblStatusClientesStatus.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesStatus.Text = "Ok";
                                                    lblStatusClientesStatus.TextColor = Color.LimeGreen;
                                                    lblResumenClientesStatus.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesStatus.Text = "Error";
                                                    lblStatusClientesStatus.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar clientes_estatus

                                        #region Procedimiento para enviar gps
                                        sRespuesta = await transmitir.enviarGps();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusGps.Text = "Ok";
                                                    lblStatusGps.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusGps.Text = "Ok";
                                                    lblStatusGps.TextColor = Color.LimeGreen;
                                                    lblResumenGps.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusGps.Text = "Error";
                                                    lblStatusGps.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar gps

                                        #region Procedimiento para enviar Anticipos
                                        sRespuesta = await transmitir.EnviarAnticipos();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusAnticipos.Text = "Ok";
                                                    lblAnticipos.Text = "Anticipos";
                                                    lblStatusAnticipos.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusAnticipos.Text = "Ok";
                                                    lblAnticipos.Text = "Anticipos";
                                                    lblStatusAnticipos.TextColor = Color.LimeGreen;
                                                    lblResumenAnticipos.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusAnticipos.Text = "Error";
                                                    lblAnticipos.Text = "Anticipos";
                                                    lblStatusAnticipos.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }

                                        #endregion Procedimiento para enviar Anticipos

                                        #region Procedimiento para enviar documentos_cabecera
                                        sRespuesta = await transmitir.enviarDocumentosCabecera();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosCabecera.Text = "Ok";
                                                    lblStatusDocumentosCabecera.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosCabecera.Text = "Ok";
                                                    lblStatusDocumentosCabecera.TextColor = Color.LimeGreen;
                                                    lblResumenDocumentosCabecera.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosCabecera.Text = "Error";
                                                    lblStatusDocumentosCabecera.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar documentos_cabecera

                                        #region Procedimiento para envair documentos_detalle
                                        sRespuesta = await transmitir.enviarDocumentosDetalle();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosDetalle.Text = "Ok";
                                                    lblStatusDocumentosDetalle.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosDetalle.Text = "Ok";
                                                    lblStatusDocumentosDetalle.TextColor = Color.LimeGreen;
                                                    lblResumenDocumentosDetalle.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusDocumentosDetalle.Text = "Error";
                                                    lblStatusDocumentosDetalle.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para envair documentos_detalle

                                        #region Procedimiento para enviar horario, kilometraje y versión de la ruta
                                        sRespuesta = await transmitir.enviarInfoRuta();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusInfoRuta.Text = "Ok";
                                                    lblStatusInfoRuta.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusInfoRuta.Text = "Ok";
                                                    lblStatusInfoRuta.TextColor = Color.LimeGreen;
                                                    lblResumenInfoRuta.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusInfoRuta.Text = "Error";
                                                    lblStatusInfoRuta.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar horario, kilometraje y versión de la ruta

                                        #region Procedimiento para enviar empleados
                                        sRespuesta = await transmitir.enviarEmpleados();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEmpleados.Text = "Ok";
                                                    lblStatusEmpleados.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEmpleados.Text = "Ok";
                                                    lblStatusEmpleados.TextColor = Color.LimeGreen;
                                                    lblResumenEmpleados.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEmpleados.Text = "Error";
                                                    lblStatusEmpleados.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion #region Procedimiento para enviar empleados

                                        #region Procedimiento para enviar clientes_competencia
                                        sRespuesta = await transmitir.enviarClientesCompetencia();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesCompetencia.Text = "Ok";
                                                    lblStatusClientesCompetencia.TextColor = Color.LimeGreen;
                                                    lblClientesCompetencia.Text = "Ctes Competencia";
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesCompetencia.Text = "Ok";
                                                    lblStatusClientesCompetencia.TextColor = Color.LimeGreen;
                                                    lblResumenClientesCompetencia.Text = sRespuesta;
                                                    lblClientesCompetencia.Text = "Ctes Competencia";
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusClientesCompetencia.Text = "Error";
                                                    lblStatusClientesCompetencia.TextColor = Color.Red;
                                                    lblClientesCompetencia.Text = "Ctes Competencia";
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar documentos_cabecera

                                        #region Procedimiento para enviar pagos programados
                                        sRespuesta = await transmitir.enviarPagosProgramados();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Ok";
                                                    lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Ok";
                                                    lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                                    lblResumenVentaPagos.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusVentaPagos.Text = "Error";
                                                    lblStatusVentaPagos.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar pagos programados                                        

                                        #region Procedimiento para enviar pagare_clientes
                                        sRespuesta = await transmitir.enviarPagareClientes();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPagareClientes.Text = "Ok";
                                                    lblStatusPagareClientes.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPagareClientes.Text = "Ok";
                                                    lblStatusPagareClientes.TextColor = Color.LimeGreen;
                                                    lblResumenPagareClientes.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusPagareClientes.Text = "Error";
                                                    lblStatusPagareClientes.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar pagare_clientes

                                        #region Procedimiento para enviar envase_sugerido
                                        sRespuesta = await transmitir.enviarInfoEnvaseSugerido();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvaseSugerido.Text = "Ok";
                                                    lblStatusEnvaseSugerido.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvaseSugerido.Text = "Ok";
                                                    lblStatusEnvaseSugerido.TextColor = Color.LimeGreen;
                                                    lblResumenEnvaseSugerido.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusEnvaseSugerido.Text = "Error";
                                                    lblStatusEnvaseSugerido.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }

                                        #endregion Procedimiento para enviar envase_sugerido

                                        #region Procedimiento para enviar respuestas
                                        sRespuesta = await transmitir.enviarRespuestas();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                    lblResumenRespuesta.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Error";
                                                    lblStatusRespuesta.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar respuestas

                                        #region Procedimiento para enviar solicitudes
                                        sRespuesta = await transmitir.enviarSolicitudes();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Ok";
                                                    lblStatusRespuesta.TextColor = Color.LimeGreen;
                                                    lblResumenRespuesta.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusRespuesta.Text = "Error";
                                                    lblStatusRespuesta.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar solicitudes                                                                        

                                        #region Procedimiento para enviar Telefonos_Clientes
                                        sRespuesta = await transmitir.EnviarTelefonosClientes();

                                        switch (sRespuesta)
                                        {
                                            case "Ok":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusTelefonosClientes.Text = "Ok";
                                                    lblStatusTelefonosClientes.TextColor = Color.LimeGreen;
                                                });
                                                break;
                                            case "No existen datos":
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusTelefonosClientes.Text = "Ok";
                                                    lblStatusTelefonosClientes.TextColor = Color.LimeGreen;
                                                    lblResumenTelefonosClientes.Text = sRespuesta;
                                                });
                                                break;
                                            default:
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    lblStatusTelefonosClientes.Text = "Error";
                                                    lblStatusTelefonosClientes.TextColor = Color.Red;
                                                });

                                                progressDialogRecepcion.Dismiss();
                                                return;
                                        }
                                        #endregion Procedimiento para enviar Telefonos_Clientes

                                    }
                                    else
                                    {
                                        DisplayAlert("Aviso", sRespuesta, "OK");
                                    }                                        
                                }
                                #endregion Envío de Preventa

                                #region Mensaje Final de Envío
                                progressDialogRecepcion.Dismiss();

                                Device.BeginInvokeOnMainThread(() =>
                                {                                    
                                    if (sRespuesta == "Ok" || sRespuesta == "No existen datos")
                                        DisplayAlert("Aviso", "La Transmición de la Ruta " + iRuta.ToString() + " ha terminado.", "OK");
                                    else
                                        DisplayAlert("Aviso", "La Transmición No fue Completada", "OK");

                                    btnEnviar.IsEnabled = false;
                                });
                                #endregion Mensaje Final de Envío
                            }
                        }
                    }
                    #endregion Procedimiento para el envío de Autoventa y Preventa

                    #region Procedimiento para el envío de Reparto
                    if (VarEntorno.cTipoVenta == 'R')
                    {
                        //Validación de que todos los clientes sean visitados
                        List<clientes_estatus> vLista = await vmstatusClientes.obtieneClientesSinVisita();

                        //Verifica la bandera de visitar todos los clientes
                        if (VarEntorno.bVisitaAllCtsReparto)
                        {
                            if (vLista.Count >= 1)
                            {
                                await DisplayAlert("Aviso", "Faltan Clientes por Visitar", "OK");
                                progressDialogRecepcion.Dismiss();
                                btnEnviar.IsEnabled = false;
                            }
                            else
                            {
                                btnEnviar.IsEnabled = true;
                            }
                        }
                        else
                        {
                            btnEnviar.IsEnabled = true;
                        }


                        if (btnEnviar.IsEnabled)                        
                        {
                            //Válida si la Ruta ya fue cerrada
                            sRespuesta = await transmitir.validaCierreRuta(iRuta);                            

                            if (sRespuesta == "\"1\"")
                            {
                                await DisplayAlert("Aviso", "La Ruta ya se encuentra CERRADA", "OK");
                                progressDialogRecepcion.Dismiss();
                                btnEnviar.IsEnabled = false;
                            }
                            else
                            {
                                #region Procedimiento para enviar venta_cabecera
                                sRespuesta = await transmitir.enviarVentaCabecera();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaCabecera.Text = "Ok";
                                            lblStatusVentaCabecera.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaCabecera.Text = "Ok";
                                            lblStatusVentaCabecera.TextColor = Color.LimeGreen;
                                            lblResumenVentaCabecera.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaCabecera.Text = "Error";
                                            lblStatusVentaCabecera.TextColor = Color.Red;
                                            DisplayAlert("Aviso", sRespuesta, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar venta_cabecera

                                #region Procedimiento para enviar venta_pagos
                                sRespuesta = await transmitir.enviarVentaPagos();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaPagos.Text = "Ok";
                                            lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaPagos.Text = "Ok";
                                            lblStatusVentaPagos.TextColor = Color.LimeGreen;
                                            lblResumenVentaPagos.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaPagos.Text = "Error";
                                            lblStatusVentaPagos.TextColor = Color.Red;
                                            DisplayAlert("Aviso", sRespuesta, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar venta_pagos

                                #region Procedimiento para enviar venta_detalle
                                sRespuesta = await transmitir.enviarVentaDetalle();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaDetalle.Text = "Ok";
                                            lblStatusVentaDetalle.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaDetalle.Text = "Ok";
                                            lblStatusVentaDetalle.TextColor = Color.LimeGreen;
                                            lblResumenVentaDetalle.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusVentaDetalle.Text = "Error";
                                            lblStatusVentaDetalle.TextColor = Color.Red;
                                            DisplayAlert("Aviso", sRespuesta, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar venta_detalle

                                #region Procedimiento para enviar envase
                                sRespuesta = await transmitir.enviarEnvase();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEnvase.Text = "Ok";
                                            lblStatusEnvase.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEnvase.Text = "Ok";
                                            lblStatusEnvase.TextColor = Color.LimeGreen;
                                            lblResumenEnvase.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusEnvase.Text = "Error";
                                            lblStatusEnvase.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar envase                            

                                #region Procedimiento para enviar clientes_estatus
                                sRespuesta = await transmitir.enviarClientesEstatus();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientesStatus.Text = "Ok";
                                            lblStatusClientesStatus.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientesStatus.Text = "Ok";
                                            lblStatusClientesStatus.TextColor = Color.LimeGreen;
                                            lblResumenClientesStatus.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusClientesStatus.Text = "Error";
                                            lblStatusClientesStatus.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar clientes_estatus

                                #region Procedimiento para enviar devoluciones
                                sRespuesta = await transmitir.enviarDevoluciones();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDevoluciones.Text = "Ok";
                                            lblStatusDevoluciones.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDevoluciones.Text = "Ok";
                                            lblStatusDevoluciones.TextColor = Color.LimeGreen;
                                            lblResumenDevoluciones.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDevoluciones.Text = "Error";
                                            lblStatusDevoluciones.TextColor = Color.Red;
                                            DisplayAlert("Aviso", sRespuesta, "OK");
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar devoluciones                            

                                #region Procedimiento para enviar gps
                                sRespuesta = await transmitir.enviarGps();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusGps.Text = "Ok";
                                            lblStatusGps.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusGps.Text = "Ok";
                                            lblStatusGps.TextColor = Color.LimeGreen;
                                            lblResumenGps.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusGps.Text = "Error";
                                            lblStatusGps.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar gps

                                #region Procedimiento para enviar documentos_cabecera
                                sRespuesta = await transmitir.enviarDocumentosCabecera();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentosCabecera.Text = "Ok";
                                            lblStatusDocumentosCabecera.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentosCabecera.Text = "Ok";
                                            lblStatusDocumentosCabecera.TextColor = Color.LimeGreen;
                                            lblResumenDocumentosCabecera.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentosCabecera.Text = "Error";
                                            lblStatusDocumentosCabecera.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar documentos_cabecera

                                #region Procedimiento para enviar documentos_detalle
                                sRespuesta = await transmitir.enviarDocumentosDetalle();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentosDetalle.Text = "Ok";
                                            lblStatusDocumentosDetalle.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentosDetalle.Text = "Ok";
                                            lblStatusDocumentosDetalle.TextColor = Color.LimeGreen;
                                            lblResumenDocumentosDetalle.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusDocumentosDetalle.Text = "Error";
                                            lblStatusDocumentosDetalle.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar documentos_detalle

                                #region Procedimiento para enviar horario, kilometraje y versión de la ruta
                                sRespuesta = await transmitir.enviarInfoRuta();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusInfoRuta.Text = "Ok";
                                            lblStatusInfoRuta.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusInfoRuta.Text = "Ok";
                                            lblStatusInfoRuta.TextColor = Color.LimeGreen;
                                            lblResumenInfoRuta.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusInfoRuta.Text = "Error";
                                            lblStatusInfoRuta.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar horario, kilometraje y versión de la ruta

                                //#region Procedimiento para enviar envase_sugerido
                                //sRespuesta = await transmitir.enviarInfoEnvaseSugerido();

                                //switch (sRespuesta)
                                //{
                                //    case "Ok":
                                //        Device.BeginInvokeOnMainThread(() =>
                                //        {
                                //            lblStatusEnvaseSugerido.Text = "Ok";
                                //            lblStatusEnvaseSugerido.TextColor = Color.LimeGreen;
                                //        });
                                //        break;
                                //    case "No existen datos":
                                //        Device.BeginInvokeOnMainThread(() =>
                                //        {
                                //            lblStatusEnvaseSugerido.Text = "Ok";
                                //            lblStatusEnvaseSugerido.TextColor = Color.LimeGreen;
                                //            lblResumenEnvaseSugerido.Text = sRespuesta;
                                //        });
                                //        break;
                                //    default:
                                //        Device.BeginInvokeOnMainThread(() =>
                                //        {
                                //            lblStatusEnvaseSugerido.Text = "Error";
                                //            lblStatusEnvaseSugerido.TextColor = Color.Red;
                                //        });

                                //        progressDialogRecepcion.Dismiss();
                                //        return;
                                //}

                                //#endregion Procedimiento para enviar envase_sugerido

                                #region Procedimiento para enviar solicitudes
                                sRespuesta = await transmitir.enviarSolicitudes();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRespuesta.Text = "Ok";
                                            lblStatusRespuesta.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRespuesta.Text = "Ok";
                                            lblStatusRespuesta.TextColor = Color.LimeGreen;
                                            lblResumenRespuesta.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRespuesta.Text = "Error";
                                            lblStatusRespuesta.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar solicitudes                                                                                                

                                #region Procedimiento para enviar Requisitos
                                sRespuesta = await transmitir.enviarClientesDatosSurtir();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRequisitos.Text = "Ok";
                                            lblStatusRequisitos.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRequisitos.Text = "Ok";
                                            lblStatusRequisitos.TextColor = Color.LimeGreen;
                                            lblResumenRequisitos.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusRequisitos.Text = "Error";
                                            lblStatusRequisitos.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar Requisitos

                                #region Procedimiento para enviar Telefonos_Clientes
                                sRespuesta = await transmitir.EnviarTelefonosClientes();

                                switch (sRespuesta)
                                {
                                    case "Ok":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusTelefonosClientes.Text = "Ok";
                                            lblStatusTelefonosClientes.TextColor = Color.LimeGreen;
                                        });
                                        break;
                                    case "No existen datos":
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusTelefonosClientes.Text = "Ok";
                                            lblStatusTelefonosClientes.TextColor = Color.LimeGreen;
                                            lblResumenTelefonosClientes.Text = sRespuesta;
                                        });
                                        break;
                                    default:
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            lblStatusTelefonosClientes.Text = "Error";
                                            lblStatusTelefonosClientes.TextColor = Color.Red;
                                        });

                                        progressDialogRecepcion.Dismiss();
                                        return;
                                }
                                #endregion Procedimiento para enviar Telefonos_Clientes

                                #region Mensaje Final de Envío
                                progressDialogRecepcion.Dismiss();

                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    DisplayAlert("Aviso", "La Transmición de la Ruta " + iRuta.ToString() + " ha terminado.", "OK");

                                    btnEnviar.IsEnabled = false;
                                });

                                #endregion Mensaje Final de Envío                                
                            }
                        }
                    }
                    #endregion Procedimiento para el envío de Reparto
                }
                else
                {
                    await DisplayAlert("Verifica tu Conexión", sUriConexion, "OK");                    
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.ToString(), "OK");
            }
        }
        #endregion Botón de Enviar

        #region Método que limpia los text del Grid
        public void limpiarGrid()
        {
            lblStatusVentaDetalle.Text = "";
            lblVentaDetalle.Text = "Venta Detalle";
            lblResumenVentaDetalle.Text = "";
            lblStatusVentaPagos.Text = "";
            lblVentaPagos.Text = "Pagos";
            lblResumenVentaPagos.Text = "";
            lblStatusEnvase.Text = "";
            lblEnvase.Text = "Envase";
            lblResumenEnvase.Text = "";
            lblStatusVentaCabecera.Text = "";
            lblVentaCabecera.Text = "Venta Cabecera";
            lblResumenVentaCabecera.Text = "";
            lblStatusBonificaciones.Text = "";
            lblBonificaciones.Text = "Bonificaciones";
            lblResumenBonificaciones.Text = "";
            lblStatusClientesStatus.Text = "";
            lblClientesStatus.Text = "Clientes Status";
            lblResumenClientesStatus.Text = "";
            lblStatusDevoluciones.Text = "";
            lblDevoluciones.Text = "Devoluciones";
            lblResumenDevoluciones.Text = "";
            lblStatusGps.Text = "";
            lblGps.Text = "GPS";
            lblResumenGps.Text = "";
            lblStatusRespuesta.Text = "";
            lblRespuesta.Text = "Respuesta";
            lblResumenRespuesta.Text = "";

            //lblStatusSolicitudes.Text = "";
            //lblSolicitudes.Text = "Solicitudes";
            //lblResumenSolicitudes.Text = "";

            lblStatusInfoRuta.Text = "";
            lblInfoRuta.Text = "Info Ruta";
            lblResumenInfoRuta.Text = "";

            //lblStatusPagosProgramados.Text = "";
            //lblPagosProgramados.Text = "Pagos Programados";
            //lblResumenPagosProgramados.Text = "";

            lblStatusClientesCompetencia.Text = "";
            lblClientesCompetencia.Text = "";
            lblResumenClientesCompetencia.Text = "";

            lblStatusEmpleados.Text = "";
            lblEmpleados.Text = "Empleados";
            lblResumenEmpleados.Text = "";
            lblStatusPagareClientes.Text = "";
            lblPagareClientes.Text = "Pagare Clientes";
            lblResumenPagareClientes.Text = "";
            lblStatusEnvaseSugerido.Text = "";
            lblEnvaseSugerido.Text = "Envase Sugerido";
            lblResumenEnvaseSugerido.Text = "";

            lblStatusDocumentosCabecera.Text = "";
            lblDocumentosCabecera.Text = "Doctos Cabecera";
            lblResumenDocumentosCabecera.Text = "";

            lblStatusDocumentosDetalle.Text = "";
            lblDocumentosDetalle.Text = "Doctos Detalle";
            lblResumenDocumentosDetalle.Text = "";

            lblStatusRequisitos.Text = "";
            lblRequisitos.Text = "Requisitos para Surtir";
            lblResumenRequisitos.Text = "";

            lblStatusAnticipos.Text = "";
            lblAnticipos.Text = "";
            lblResumenAnticipos.Text = "";

        }
        #endregion Método que limpia los text del Grid

        #region Método para válidar si el dispositivo tiene algun tipo de conexión (Wifi o Datos)
        public string validaConexion()
        {
            string sValidaConexion = string.Empty;

            if (CrossConnectivity.Current.IsConnected == true)
            {
                IEnumerable<ConnectionType> connectionTypes;
                connectionTypes = CrossConnectivity.Current.ConnectionTypes;

                if (connectionTypes.Contains(ConnectionType.WiFi))
                {
                    sValidaConexion = "Ok";
                }
                else
                {
                    if (connectionTypes.Contains(ConnectionType.Cellular))
                    {
                        sValidaConexion = "Ok";
                    }
                    else
                    {
                        sValidaConexion = "No tienes ningún tipo de conexión para poder Transmitir";
                    }
                }
            }
            else
            {
                sValidaConexion = "No tienes ningún tipo de conexión para poder Transmitir";
            }

            return sValidaConexion;
        }
        #endregion Método para válidar si el dispositivo tiene algun tipo de conexión (Wifi o Datos)

        #region Botón de Regresar
        private void OnClickedRegresar(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar
    }
}