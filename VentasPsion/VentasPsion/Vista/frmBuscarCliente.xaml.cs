using Base;
using Org.Json;
using Plugin.Permissions;
using System;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Threading.Tasks;
using VentasPsion.Modelo.Servicio;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Geolocator;
using System.Collections.Generic;
using Android.Content;
using Xamarin.Forms.GoogleMaps;
using Plugin.Permissions.Abstractions;

namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmBuscarCliente : ContentPage
    {
        int iProceso = 0;
        Utilerias oUtilerias = new Utilerias();
        string oLatitud = "", oLongitud = "";
        public Task<JSONObject> oJSONObject;
        public JSONObject oResultTask;
        ClientesVM _cliente = new ClientesVM();
        VentaVM _venta = new VentaVM();
        DocumentosVM Doctos = new DocumentosVM();
        VisitaVM _visita = new VisitaVM();
        //int nDctCero = 0, nDctFac = 0;

        public frmBuscarCliente(int iTipoProceso)
        {
            InitializeComponent();
            VarEntorno.vCliente = null;
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;
            txtPagare.IsVisible = false;
            iProceso = iTipoProceso;

            if (VarEntorno.cBuscarCliente is null)
                VarEntorno.cBuscarCliente = new vmStatusClientes();

            VarEntorno.cBuscarCliente.vBuscarCliente = this;

        }

        public void FnMuestraPagare(int iCliente)
        {
            bool bPagare = new pagare_clientesSR().fnPagareCliente(iCliente);

            if (bPagare)
                txtPagare.IsVisible = true;
            else
                txtPagare.IsVisible = false;
        }

        #region Método para buscar los datos del Id del Cliente Capturado usando la clase BuscarCliente
        public async void MostarDatos()
        {
            //Se instancia la clase BuscarCliente
            //clientesSR buscacliente = new clientesSR();            

            //Se obtiene el ID del cliente capturado y se válida que no esta vacío
            string sIdCliente = txtIdCliente.Text;

            VarEntorno.oOpciones_app = new OpcionesappVM();

            if (string.IsNullOrWhiteSpace(sIdCliente))
            {
                await DisplayAlert("Campo Vacío", "Verifique el Id del Cliente", "Ok");
            }
            else
            {
                //Válida si el cliente existe en la BD
                string sRespuesta = _cliente.BuscaCliente(sIdCliente);// buscacliente.ValidaCliente(sIdCliente);

                //Respuesta
                if (sRespuesta == "Ok")
                {
                    //Busqueda de datos del cliente
                    var vListaCliente = await _cliente.DatosCliente(sIdCliente);//buscacliente.DatosCliente(sIdCliente);
                    VarEntorno.vCliente = vListaCliente;
                    try
                    {

                        VarEntorno.sHoraInicio = DateTime.Now.ToShortTimeString();
                        txtIdCliente.Text = "";
                        txtId.Text = vListaCliente.cln_clave.ToString();

                        FnMuestraPagare(vListaCliente.cln_clave);

                        txtNombre.Text = vListaCliente.clc_nombre.ToString();
                        txtNegocio.Text = vListaCliente.clc_nombre_comercial.ToString();

                        //txtSaldo.Text = vListaCliente.cln_saldo.ToString();
                        txtSaldo.Text = String.Format("{0:N2}", VarEntorno.Saldo(VarEntorno.vCliente));

                        bool bClienteFoco = new clientes_estatusSR().fnFocoCliente(vListaCliente.cln_clave);
                        bool bImpacto = new clientes_estatusSR().fnCoolerCliente(vListaCliente.cln_clave);

                        if (vListaCliente.clc_credito != null)
                        {
                            if (vListaCliente.clc_credito.ToString() == "S") { txtCredito.Text = "SI"; } else { txtCredito.Text = "NO"; }
                        }
                        txtLimitVta.Text = String.Format("{0:N2}", vListaCliente.cln_limite_venta);
                        if (vListaCliente.cln_cheque) { txtCheque.Text = "SI"; } else { txtCheque.Text = "NO"; }
                        //if (vListaCliente.clc_cliente_foco ) { txtCteFoco.Text = "SI"; } else { txtCteFoco.Text = "NO"; }
                        if (bClienteFoco) { txtCteFoco.Text = "SI"; } else { txtCteFoco.Text = "NO"; }
                        //if (vListaCliente.clc_impacto) { txtImpacto.Text = "SI"; } else { txtImpacto.Text = "NO"; }
                        if (bImpacto) { txtImpacto.Text = "SI"; } else { txtImpacto.Text = "NO"; }
                    }
                    catch (Exception ex)
                    {
                        throw new System.ArgumentOutOfRangeException(
                            "Parameter index is out of range." + ex);
                    }
                }
                else
                {
                    await DisplayAlert(sRespuesta, "Verifique el Id del Cliente", "Ok");
                }
            }
        }
        #endregion   Método para buscar los datos del Id del Cliente Capturado

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }

        public async void OnClickVerMapaCliente(object sender, EventArgs args)
        {
            if (VarEntorno.vCliente == null)
            {
                await DisplayAlert("Alerta", "Favor de elegir un cliente ", "OK");
            }
            else
            {
                
                      //  Device.OpenUri(new Uri("tel:038773729"));
                    
                abrirMapa(1);

            }
        }

        public void OnClickActivoComodatados(object sender, EventArgs args)
        {
            if (VarEntorno.vCliente == null)
            {
                DisplayAlert("Alerta", "Favor de elegir un cliente ", "OK");
            }
            else
            {
                this.Navigation.PushModalAsync(new frmReporteActivosDatos(VarEntorno.vCliente.cln_clave));
            }
        }

        public void OnClickDatosCliente(object sender, EventArgs args)
        {
            if (VarEntorno.vCliente == null)
            {
                DisplayAlert("Alerta", "Favor de elegir un cliente ", "OK");
            }
            else
            {
                this.Navigation.PushModalAsync(new frmDatosCliente());
            }
        }

        public async void OnClickEditarUbicacion(object sender, EventArgs args)
        {
            if (VarEntorno.vCliente == null)
            {
                await DisplayAlert("Alerta", "Favor de elegir un cliente ", "OK");
            }
            else
            {
                await Task.Yield();
                abrirMapa(2);
        
            }
        }

        public async void abrirMapa(int iTipo)
        {
            var cached = await CrossGeolocator.Current.GetPositionAsync();
            /***************************************/
            /****FRAGMENTO DE PERMISOS ANDROID *****/
            /***************************************/
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                    // DENEGADO
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                    {
                        oUtilerias.crearMensajeLargo("Es necesario aceptar el permiso.");
                    }
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    // PERMITIRO
                   
                    if(cached==null)
                    {
                        oUtilerias.crearMensajeLargo("Favor de encender su GPS");
                    }
                    else
                    {
                        this.oLatitud = cached.Latitude.ToString();
                        this.oLongitud = cached.Longitude.ToString();
                        if (iTipo!=2)
                            Device.OpenUri(new Uri("http://maps.google.com/?daddr=" + VarEntorno.vCliente.cln_latitud + "," + VarEntorno.vCliente.cln_longitud));
                        else
                            Device.OpenUri(new Uri("geo:0,0?q="+this.oLatitud+","+ this.oLongitud));
                        //await this.Navigation.PushModalAsync(new frmGoogleMapsCliente(VarEntorno.vCliente, iTipo == 1 ? false : true, this.oLatitud, this.oLongitud));
                    }
                   
                }
                else if (status != PermissionStatus.Unknown)
                {
                    oUtilerias.crearMensajeLargo("Es necesario aceptar el permiso, intente de nuevo");
                }
            }
            catch (Exception ex)
            {

                oUtilerias.crearMensajeLargo("Error: " + ex);
            }

            /***************************************/
            /*****FRAGMENTO DE PERMISOS ANDROID ****/
            /***************************************/
        }

        public async void OnClickedAvanzar(object sender, EventArgs args)
        {           

            if (VarEntorno.vCliente == null)
            {
               await DisplayAlert("Alerta", "Favor de elegir un cliente ", "OK");
            }
            else
            {                
                bool bBorrarEnvaseTemp = new EnvaseVM().FtnBorrarEnvaseTemp(VarEntorno.vCliente.cln_clave);
                //fnVentaCabecera cVentaCabecera = new fnVentaCabecera();
                DevolucionVM _devolucion = new DevolucionVM();
                VarEntorno.sMensajeError = "";
                string sVersion = Plugin.Settings.CrossSettings.Current.GetValueOrDefault<string>("Version", "");

                decimal dSaldoSimple = Convert.ToDecimal(txtSaldo.Text);
                if ((sVersion == VarEntorno.sVersionApp && VarEntorno.cTipoVenta=='P')
                    || VarEntorno.cTipoVenta != 'P')
                {
                    switch (iProceso)
                    {
                        #region   Cobranza saldo
                        case 1:
                            {
                                Navigation.PopModalAsync();
                                // gps_cliente();
                                //oUtilerias.obtenerCoordenadas(iProceso);
                                //VarEntorno.bSaldoPendiente = true;                            
                                //await Navigation.PushModalAsync(new frmCobranza());
                            }
                            break;
                        #endregion
                        #region   Cobranza documentos
                        case 50:
                            try
                            {
                                {
                                    if (VarEntorno.cTipoVenta == 'P')
                                    {
                                        //Validación para conocer si el cliente ya tiene una captura de envase a recoger
                                        if (new vmCapturaEnvase().existeCapturaEnvaseSugerido(VarEntorno.vCliente.cln_clave.ToString()))
                                        {
                                            Navigation.PopModalAsync();
                                            //oUtilerias.obtenerCoordenadas(1);
                                            VarEntorno.bEsDocumentos = true;
                                            await this.Navigation.PushModalAsync(new frmCobranzaPagos());

                                        }
                                        else
                                        {
                                            this.Navigation.PopModalAsync();
                                            await this.Navigation.PushModalAsync(new frmEnvaseSugerido());
                                        }

                                    }
                                    else
                                    {
                                        Navigation.PopModalAsync();
                                        //oUtilerias.obtenerCoordenadas(1);
                                        VarEntorno.bEsDocumentos = true;
                                        await this.Navigation.PushModalAsync(new frmCobranzaPagos());
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Alerta", ex.Message, "OK");
                            }
                            break;
                        case 51:
                            try
                            {
                                VarEntorno.bEsDocumentos = true;
                                await this.Navigation.PushModalAsync(new frmConsultaDocumentos());
                            }
                            catch (Exception e)
                            {
                                await DisplayAlert("Alerta", e.Message, "OK");
                            }
                            break;
                        #endregion
                        #region Relacion Anticipos
                        case 52:
                            try
                            {
                                {
                                    Navigation.PopModalAsync();
                                    //oUtilerias.obtenerCoordenadas(1);
                                    //VarEntorno.bEsDocumentos = true;
                                    await this.Navigation.PushModalAsync(new frmRelacionAnticipo(1));
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Alerta", ex.Message, "OK");
                            }
                            break;
                        case 53:
                            try
                            {
                                {
                                    Navigation.PopModalAsync();
                                    //oUtilerias.obtenerCoordenadas(1);
                                    //VarEntorno.bEsDocumentos = true;
                                    await this.Navigation.PushModalAsync(new frmRegistroTelefono());
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Alerta", ex.Message, "OK");
                            }
                            break;
                        #endregion Relacion Anticipos
                        #region VENTA
                        case 2:
                            if (VarEntorno.vCliente.clc_estatus == "I")
                            {
                                await DisplayAlert("Aviso", "Cliente Inactivo ", "OK");
                            }
                            else
                            {
                                if (VarEntorno.bEsDevolucion)
                                {
                                    await DisplayAlert("Aviso", "Favor de cerrar la APP ", "OK");
                                }
                                else
                                {
                                    switch (VarEntorno.cTipoVenta)
                                    {
                                        case 'P':
                                            int i = _cliente.Antiguedad_documentos();
                                            if (i == 0)
                                            // if (true)
                                            {

                                                if (_cliente.candado_saldo_ant() == true)
                                                {
                                                    if (_visita.ValidaEstatusCliente())
                                                    {
                                                        this.Navigation.PopModalAsync();
                                                        //oUtilerias.obtenerCoordenadas(iProceso);
                                                        _venta.fnLimpiaTicket(VarEntorno.TraeFolio());
                                                        await this.Navigation.PushModalAsync(new FrmVentas());

                                                    }
                                                    else
                                                        await DisplayAlert("Aviso", "Cliente con registro de No Venta", "OK");
                                                }
                                                else
                                                    await DisplayAlert("Aviso", "Favor de abonar al saldo anterior primero", "OK");
                                            }
                                            else
                                            {
                                                await DisplayAlert("Aviso", "Cliente con " + i + " Documentos Vencidos ", "OK");
                                            }
                                            break;
                                        case 'A':
                                            //Validación para conocer si el cliente tiene devolución
                                            int iExisteVenta = _venta.fnExisteDevolucion();

                                            switch (iExisteVenta)
                                            {
                                                case 1:
                                                    await DisplayAlert("Alerta", "Cliente tiene Devolución registrada", "OK");
                                                    break;
                                                case -1:
                                                    await DisplayAlert("Alerta", VarEntorno.sMensajeError, "OK");
                                                    break;
                                                case 0:

                                                    int t = await ValidaAutoventa();
                                                    switch (t)
                                                    {
                                                        case -1:
                                                            await DisplayAlert("Aviso", "error validacion de saldo y pedido", "Ok");
                                                            break;
                                                        case 1:
                                                            this.Navigation.PopModalAsync();
                                                            //oUtilerias.obtenerCoordenadas(1);
                                                            //VarEntorno.bSoloCobrar = true;

                                                            //await this.Navigation.PushModalAsync(new frmCobranza());
                                                            VarEntorno.bEsDocumentos = true;
                                                            VarEntorno.bSoloCobrar = true;
                                                            await this.Navigation.PushModalAsync(new frmCobranzaPagos());
                                                            break;
                                                        case 2:
                                                            if (_visita.ValidaEstatusCliente())
                                                            {
                                                                int d = _cliente.Antiguedad_documentos();
                                                                if (d == 0)
                                                                // if (true)
                                                                {
                                                                    this.Navigation.PopModalAsync();
                                                                    //oUtilerias.obtenerCoordenadas(iProceso);
                                                                    _venta.fnLimpiaTicket(VarEntorno.TraeFolio());
                                                                    await this.Navigation.PushModalAsync(new FrmVentas());
                                                                }
                                                                else
                                                                {
                                                                    await DisplayAlert("Aviso", "Cliente con " + d + " Documentos Vencidos ", "OK");
                                                                }
                                                            }
                                                            else
                                                                await DisplayAlert("Aviso", "Cliente con registro de No Venta", "OK");
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                }
                            }
                            break;
                        #endregion
                        #region Pedido Reparto
                        case 3:
                            /// valida que el cliente no tenga otra entrega o devolucion 
                            if (await _devolucion.ValidaEstatusCliente())
                            {
                                int i = _cliente.Antiguedad_documentos();
                                if (i == 0)
                                // if (true)
                                {
                                    switch (await Valida_saldo_credito())
                                    {
                                        case -1:
                                            await DisplayAlert("Aviso", "error validacion de saldo y pedido", "Ok");
                                            break;
                                        case 1:
                                            Navigation.PopModalAsync();
                                            //oUtilerias.obtenerCoordenadas(1);
                                            VarEntorno.bEsDocumentos = true;
                                            VarEntorno.bSoloCobrar = true;
                                            await this.Navigation.PushModalAsync(new frmCobranzaPagos());
                                            break;
                                        case 2:
                                            //oUtilerias.obtenerCoordenadas(iProceso);
                                            this.Navigation.PopModalAsync();
                                            await this.Navigation.PushModalAsync(new frmMostrarPedido());

                                            break;
                                        case 3:
                                            Navigation.PopModalAsync();
                                            // gps_cliente();
                                            //oUtilerias.obtenerCoordenadas(1);
                                            VarEntorno.bSoloCobrar = true;
                                            VarEntorno.bEsDocumentos = true;
                                            await this.Navigation.PushModalAsync(new frmCobranzaPagos());
                                            break;
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Aviso", "Cliente con " + i + " Documentos Vencidos ", "OK");
                                }
                            }
                            else
                                await DisplayAlert("Aviso", "Cliente ya fue Visitado", "Ok");

                            break;
                        #endregion
                        #region Devolucion Reparto
                        case 4:
                            /// valida que el cliente no tenga otra entrega o devolucion 
                            if (await _devolucion.ValidaEstatusCliente())
                            {
                                //gps_cliente();
                                //oUtilerias.obtenerCoordenadas(iProceso);
                                this.Navigation.PopModalAsync();
                                await this.Navigation.PushModalAsync(new frmDevolucionReparto());
                            }
                            else
                                await DisplayAlert("Aviso", "Cliente ya fue Visitado", "Ok");
                            break;
                        #endregion
                        #region Devolucion Autoventa
                        case 5:
                            if (VarEntorno.vCliente.clc_devolucion)
                            {
                                //Validación para conocer si el cliente tiene venta
                                int iExisteVenta = _venta.fnExisteVenta();

                                switch (iExisteVenta)
                                {
                                    case 1:
                                        await DisplayAlert("Alerta", "Cliente tiene movimiento de venta registrado", "OK");
                                        break;
                                    case -1:
                                        await DisplayAlert("Alerta", VarEntorno.sMensajeError, "OK");
                                        break;
                                    case 0:
                                        //gps_cliente();
                                        //oUtilerias.obtenerCoordenadas(iProceso);
                                        this.Navigation.PopModalAsync();
                                        VarEntorno.bEsDevolucion = true;
                                        await this.Navigation.PushModalAsync(new frmDevolucionAutoventaDocto());
                                        break;
                                }
                            }
                            else
                            {
                                await DisplayAlert("Alerta", "Cliente sin derecho a devolucion; Atte: credito y cobranza", "OK");
                            }
                            break;
                        #endregion
                        case 6:
                            // gps_cliente();
                            //oUtilerias.obtenerCoordenadas(iProceso);
                            this.Navigation.PopModalAsync();
                            await this.Navigation.PushModalAsync(new frmQuejasSugerencias());
                            break;
                        case 7:
                            //gps_cliente();
                            //oUtilerias.obtenerCoordenadas(iProceso);
                            this.Navigation.PopModalAsync();
                            await this.Navigation.PushModalAsync(new FrmPedidoSugerido());
                            break;
                        case 8:
                            //gps_cliente();
                            //oUtilerias.obtenerCoordenadas(iProceso);
                            this.Navigation.PopModalAsync();
                            await this.Navigation.PushModalAsync(new frmEncuestas());
                            break;
                        case 9:
                            if (_visita.ValidaEstatusCliente())
                            {
                                //gps_cliente();
                                //oUtilerias.obtenerCoordenadas(iProceso);                            

                                if (VarEntorno.cTipoVenta == 'P')
                                {
                                    //Validación para conocer si el cliente ya tiene una captura de envase a recoger
                                    if (new vmCapturaEnvase().existeCapturaEnvaseSugerido(VarEntorno.vCliente.cln_clave.ToString()))
                                    {
                                        this.Navigation.PopModalAsync();
                                        await this.Navigation.PushModalAsync(new FrmNoVenta());
                                    }
                                    else
                                    {
                                        this.Navigation.PopModalAsync();
                                        await this.Navigation.PushModalAsync(new frmEnvaseSugerido());
                                    }
                                }
                                else
                                {
                                    this.Navigation.PopModalAsync();
                                    await this.Navigation.PushModalAsync(new FrmNoVenta());
                                }
                            }
                            else
                            {
                                await DisplayAlert("Aviso", "Cliente ya fue Visitado", "Ok");
                            }
                            break;
                        case 10:
                            //gps_cliente();
                            //oUtilerias.obtenerCoordenadas(iProceso);
                            this.Navigation.PopModalAsync();
                            await this.Navigation.PushModalAsync(new frmEnvaseSugerido());
                            break;
                    }
                }
                else
                {
                    await DisplayAlert("Alerta", "versión incorrecta", "OK");
                }
            }            
        }
        /*
        public bool candado_saldo_ant()
        {
            try
            {
                bool bAplica = false;
                fnVentaCabecera cVentaCabecera = new fnVentaCabecera();
                
                if (VarEntorno.oOpciones_app.opciones[1].oab_valor == true)
                {
                    switch (VarEntorno.vCliente.clc_cobrador)
                    {
                        default:
                            bAplica = false;
                            break;                       
                        case "":
                            bAplica = true;
                            break;                        
                        case "RP":
                            if (VarEntorno.cTipoVenta == 'R')
                                bAplica = false;
                            else
                                bAplica = true;
                            break;
                        case "A":
                            bAplica = false;
                            break;
                        case "P":                       
                            if (VarEntorno.cTipoVenta == 'P')
                                bAplica = false;
                            else
                                bAplica = true;
                            break;                            
                        case "R":
                            if (VarEntorno.cTipoVenta == 'R')
                                bAplica = false;
                            else
                                bAplica = true;
                            break;
                    }                    
                }
                else                
                {
                    bAplica = true;
                }

                if (bAplica == false)
                {
                    if (VarEntorno.Saldo(VarEntorno.vCliente) > 0 && cVentaCabecera.PagosImportes(VarEntorno.vCliente.cln_clave) < 1)
                    {                       
                        //List<string> sFolios = cVentaCabecera.FoliosVenta(Convert.ToInt32(VarEntorno.vCliente.cln_clave), "$");
                        if (sFolios.Count > 0)
                            bAplica = true;
                        else
                            bAplica = false;
                    }                        
                    else
                    {                        
                            bAplica = true;
                    }
                }


                if (VarEntorno.bEsTeleventa == true)
                    bAplica = true;

                return bAplica;
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso Can_Sal_Ant",ex.Message.ToString(), "OK");
                return false;
            }
        }
        */
        /*
        public bool Antiguedad_documentos()
        {
            try
            {
                int nDiascredito = VarEntorno.vCliente.clc_dias_credito;
                int nDiasCreditoCero = VarEntorno.vCliente.clc_dias_credito_cero;

                nDctCero = 0; nDctFac =0;

                if (Doctos.lDoctos()==false)
                    return false;
                else
                {
                    int x = 0;
                    foreach (var docto in Doctos.lDoctosCabecera)
                    {
                        
                        if (docto.vcn_folio.Length <=5 )
                        {
                            //x = DateTime.Now.ToString("dd/MM/yyyy").CompareTo(docto.vcf_movimiento);
                            x = docto.vcf_movimiento.Date.Subtract(DateTime.Now).Days;
                            if (Math.Abs(x) > nDiasCreditoCero)
                                nDctCero++;
                        }
                        else
                        {
                            //x = DateTime.Now.ToString("dd/MM/yyyy").CompareTo(docto.vcf_movimiento);
                            x = docto.vcf_movimiento.Date.Subtract(DateTime.Now).Days;
                            if (Math.Abs(x) > nDiascredito)
                                nDctFac++;
                        }
                    }
                    if ((nDctCero + nDctFac) > 0)
                    {
                        return false;
                    }
                    else
                        return true;
                }
            }
            catch
            {
                return false;
            }
        }
        */

        public async Task<int> ValidaAutoventa()
        {
            try
            {
                bool respuesta = false;
                decimal dSaldoAnt = VarEntorno.Saldo(VarEntorno.vCliente);             

                if (dSaldoAnt > 0M )
                {
                    if (VarEntorno.vCliente.clc_credito == "S")
                        respuesta = await DisplayAlert("Aviso", "El cliente va a pagar el saldo Ant o la venta ", "Saldo", "Venta");
                    else
                    {
                        respuesta = true;
                        await DisplayAlert("Aviso", "Liquide primero el saldo y despues genere la venta; Cliente sin credito", "OK");
                    }

                    if (respuesta)
                        return 1;        /////cobranza         saldo   
                    else
                        return 2;           /////venta        
                }
                else                
                        return 2; ////venta  
            }
            catch 
            {
                return -1;
            }
        }


        public async Task<int> Valida_saldo_credito()
        {
            try
            {
                bool respuesta = false, bPagoPrev = false, bPagoTikets = false, bPagoRep = false;
                decimal dSaldo = VarEntorno.Saldo(VarEntorno.vCliente);
                decimal dSaldoAnt = 0;
                decimal dImporte = _cliente.VentaCliente(VarEntorno.vCliente.cln_clave);
                int iNFacturas = new facturasventasSR().fnListaFacturas(VarEntorno.vCliente.cln_clave).Count;
                decimal dPagoPreventa = new facturasventasSR().AbonoPreventa(VarEntorno.vCliente.cln_clave);


                dSaldoAnt = dSaldo - dImporte + dPagoPreventa;

                if (VarEntorno.vCliente.clc_pago_diferencia_preventa >= dImporte)
                    bPagoPrev = true;
                else
                    bPagoPrev = false;

                if ((dSaldoAnt + dImporte)<=1M)
                    bPagoTikets = true;
                else
                    bPagoTikets = false;

                if (Doctos.DoctosPendientesR() == 0)
                    bPagoRep = true;
                else
                    bPagoRep = false;

                if ( dSaldoAnt > 1M  &&(bPagoPrev==false && 
                        bPagoRep==false))
                     //bPagoTikets ==false ))
                {

                    if (VarEntorno.vCliente.clc_credito == "S")
                    {
                        respuesta = await DisplayAlert("Aviso", "El cliente va a pagar el saldo Ant o la venta ", "Saldo", "Venta");
                    }
                    else
                    {
                        respuesta = true;
                        await DisplayAlert("Aviso", "Liquide primero el saldo y despues genere la venta; Cliente sin credito", "OK");
                    }

                    if (respuesta)
                        return 1;        /////cobranza         saldo   
                    else
                    {
                        if (iNFacturas > 1 && VarEntorno.vCliente.clc_cobrador!="")
                        {
                            await DisplayAlert("Aviso", "venta de multiples tickets; Liquide los documentos individualmente ", "OK");
                            return 3;
                        }
                        else
                            return 2;           //// pedido  
                    }                               
                }
                else
                {
                    if ((bPagoTikets || bPagoPrev ) && VarEntorno.vCliente.clc_credito != "S" && dSaldoAnt > 1M)
                    {
                        await DisplayAlert("Aviso", "Liquide primero el saldo y despues genere la venta; Cliente sin credito", "OK");
                        return 1;
                    }
                    else
                        return 2; ////venta o pedido 
                    /*

                    if (VarEntorno.cTipoVenta == 'R'
                      //  && VarEntorno.vCliente.clc_credito == "N"
                          && new fnFacturasventas().CuentaFacturas(VarEntorno.vCliente.cln_clave) > 1
                       // &&  ((dImporte + dSaldoAnt )< -2 &&   2 >(dImporte + dSaldoAnt))
                        && bValor==true
                        && bPagoTikets == false)
                    {
                        //await DisplayAlert("Aviso", "Cliente sin credito y venta de multiples tickets; Liquide los documentos ", "OK");
                        return await RepartoTicktes();  /////cobranza   complemntos
                    }
                    else
                        
                        return 2; ////venta o pedido 
                        */
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.Message, "OK");
                return -1;
            }
        }
        
    }
}
