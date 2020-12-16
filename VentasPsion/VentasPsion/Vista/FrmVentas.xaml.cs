using Android.Widget;
using Android.App;
using Android.Views;
using Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Base;

using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Platform.Android;
using Newtonsoft.Json.Linq;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FrmVentas : ContentPage
	{
        VentaVM _venta = new VentaVM();
        EnvaseVM _envase = new EnvaseVM();

        Formularios oFormulario = new Formularios();
        ArrayList oArrayFormulario = new ArrayList();

        public  FrmVentas()
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            InicioVenta();
            CargaEnvaseTemp();
            CargaRadioButton();
           
        }

        private void CargaRadioButton()
        {
            var oRadrioGroup = oFormulario.CrearRadioGroup(true, ViewStates.Visible, "1");
            var oRadiobutton1 = oFormulario.CrearRadioButton("Contado", true, ViewStates.Visible, "1", "1");
            var oRadiobutton2 = oFormulario.CrearRadioButton("Crédito", true, ViewStates.Visible, "2", "2");
            
            oRadrioGroup.AddView(oRadiobutton1);
            oRadrioGroup.AddView(oRadiobutton2);

            oArrayFormulario.Add(oRadiobutton1);
            oArrayFormulario.Add(oRadiobutton2);

            grdTipoPago.Children.Add(oRadrioGroup);

            decimal dSaldo = VarEntorno.Saldo(VarEntorno.vCliente);//_venta.Saldo();

            switch (VarEntorno.vCliente.clc_cobrador)
            {
                case "P":
                    if (dSaldo > 0M && VarEntorno.vCliente.clc_credito=="N")
                    {                        
                            oRadiobutton1.Checked = true;  ///contado 

                            oRadiobutton1.Enabled = false;
                            oRadiobutton2.Enabled = false;
                    }
                    else
                    {                          
                        oRadiobutton2.Checked = true; ///credito                           
                    }
                        
                    break;
                case "R":
                    oRadiobutton2.Checked = true;///credito   
                    break;
                case "":
                    oRadiobutton2.Checked = true;///credito   
                    break;
                case "RP":
                    //oRadiobutton2.Checked = true;///credito   
                    if (dSaldo > 0M && VarEntorno.vCliente.clc_credito == "N")
                    {
                        oRadiobutton1.Checked = true;  ///contado 

                        oRadiobutton1.Enabled = false;
                        oRadiobutton2.Enabled = false;
                    }
                    else
                    {
                        oRadiobutton2.Checked = true; ///credito                           
                    }
                    break;
                case "A":
                    if (dSaldo > 0M)
                    {
                        oRadiobutton1.Checked = true;  ///contado 

                        oRadiobutton1.Enabled = false;
                        oRadiobutton2.Enabled = false;
                    }
                    else
                        oRadiobutton2.Checked = true;///credito   
                    break;
                default:                       
                        oRadiobutton1.Checked = true;///contado 
                        oRadiobutton1.Enabled = false;
                        oRadiobutton2.Enabled = false;                 
                    break;
            }
        }

        public int serializarData()
        {
            int tipoRespuesta = 0;           
            
            for (int i = 0; i < oArrayFormulario.Count; i++)
            {
                var oTipoObjeto = oArrayFormulario[i].GetType();
                JObject oJObject = new JObject();
                if (oTipoObjeto == typeof(Android.Widget.RadioButton))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.RadioButton;

                    if (oSender.Checked)
                    {
                        if (i == 0)
                        {
                            tipoRespuesta = 1;
                            break;
                        }
                        else
                        {
                            tipoRespuesta = 2;
                            break;
                        }                            
                    }                    
                }

            } // fin for

            return tipoRespuesta;

        }
        
        /*******  Se inicializa variables de subtotal , total y se obtine folio de venta *****/
        private  void InicioVenta()
        {
            VarEntorno.iFolio = VarEntorno.TraeFolio();
            VarEntorno.dImporteTotal = _venta.fnImporteTotalxFolio(); 
            lblSubtotal.Text = String.Format("{0:N2}", 0);
            lblTotal.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal);
            
        }

        /*******  Se inicializa la tabla temporal de envase *****/
        public void CargaEnvaseTemp()
        {
            int iCliente = VarEntorno.vCliente.cln_clave;
            //bool bRespuesta = _envase.FtnInsertaEnvaseTemp(iCliente);
        }

        /****** al terminar la captura se obtien los datos del articulo para mostrar *****/
        private void Entry_Completed_txtArticulo(object sender, EventArgs e)
        {
            string sArticulo;
            sArticulo = txtArticulo.Text;
            int iTipoPago = serializarData();
            bool bArticuloContado = true;

            _venta = new VentaVM();
            //  se busca la informacion del articulo para llenar pantalla 
            if (txtArticulo.Text != "")
            {
                //validad si el articulo es de venta de contado 
                bArticuloContado = _venta.ArticuloContado(txtArticulo.Text);
                /*
                if ((iTipoPago == 1 && bArticuloContado)
                    || bArticuloContado==false)
                    */
                if (true)
                {
                    if (_venta.fnCargaProducto(sArticulo))
                    {
                        //lblCorta.Text = _venta.pProducto.ard_corta;
                        lblDescripcion.Text = _venta.pProducto.ard_descripcion;
                        lblPrecio.Text = String.Format("{0:N2}", _venta.dPrecio);
                        txtCantidad.Text = _venta.iVendido > 0 ? _venta.iVendido.ToString() : "";
                        lblExistencia.Text = _venta.iExistencia.ToString();

                        if (_venta.iExistencia < 1)
                            lblExistencia.BackgroundColor = Color.Red;
                        else
                            lblExistencia.BackgroundColor = Color.White;

                        if (txtCantidad.Text != "" && _venta.iVendido > 0)
                            lblSubtotal.Text = String.Format("{0:N2}", _venta.iVendido * _venta.dPrecio);
                        else
                            lblSubtotal.Text = String.Format("{0:N2}", 0);

                        txtCantidad.Focus();

                        if (VarEntorno.cTipoVenta == 'A' && _venta.pProducto.arc_envase.Length > 0)
                        {
                            txtCantEnvase.IsVisible = true;
                            lblEnva.IsVisible = true;

                            if (_venta.iAbonoEnvase == 0)
                                txtCantEnvase.Text = "";
                            else
                                txtCantEnvase.Text = _venta.iAbonoEnvase.ToString();
                        }
                        else
                        {
                            txtCantEnvase.IsVisible = false;
                            lblEnva.IsVisible = false;
                        }
                    }
                    else
                    {
                        //  error al cargar la informacion del articulo 
                        DisplayAlert("Aviso", "El Articulo no esta activo o no existe; Verifique", "OK");
                        LimpiarCampos();
                    }
                }
                else
                {
                    DisplayAlert("Alert", "Articulo de venta de contado", "OK");
                }
            }
            else
            {
                // error no ingreso nada
                DisplayAlert("Alert", "Favor de ingresar una clave", "OK");
                LimpiarCampos();
            }

            VarEntorno.sMensajeError = "";
        }
                
        /****** limpia los campos de la pantalla de venta  ******/
        private void LimpiarCampos()
        {            
            //lblCorta.Text = "";
            lblDescripcion.Text = "";
            lblPrecio.Text = String.Format("{0:N2}", 0);
            txtCantidad.Text = "";
            lblExistencia.Text = "";
            lblSubtotal.Text = String.Format("{0:N2}", 0);
            txtArticulo.Text = "";
            lblTotal.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal);
        }

        /********actualiza el subtotal al cambiara la cantidad de cero y precio cero ********/
        private void txtCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {   
            if (Convert.ToDecimal(lblPrecio.Text) > 0 && txtCantidad.Text.Length > 0)            
                lblSubtotal.Text = String.Format("{0:N2}", Convert.ToDecimal(lblPrecio.Text) * Convert.ToDecimal(txtCantidad.Text));
            
        }

        private bool valida_limite(int iTipoPago)
        {
            try
            {
                //double dLimite = VarEntorno.vCliente.cln_limite_venta;
                bool bContado;

                if (iTipoPago == 1)
                    bContado = true;
                else
                    bContado = false;

                if (bContado == true)
                {
                    return true;
                }
                else
                {
                    if (VarEntorno.vCliente.cln_limite_venta == 0M )
                    {
                        return true;
                    }
                    else
                    {
                        decimal dTicketsAnt = _venta.fnImporteTotalAntFolio();
                        decimal dTotalTicket = 0;

                        if (Convert.ToDecimal(lblSubtotal.Text) <= VarEntorno.vCliente.cln_limite_venta)
                        {
                            if (VarEntorno.vCliente.cln_limite_venta < (dTicketsAnt + 700M))
                            {
                                return true;
                            }
                            else
                            {
                                if (_venta.bArtActualizado)                                
                                    dTotalTicket = -(_venta.iVendidoAnt * Convert.ToDecimal(lblPrecio.Text));                                
                                else                                
                                    dTotalTicket = 0;                                

                                dTotalTicket += (Convert.ToDecimal(lblTotal.Text)
                                        + Convert.ToDecimal(lblSubtotal.Text)
                                        + dTicketsAnt);

                                if (dTotalTicket > VarEntorno.vCliente.cln_limite_venta)
                                    return false;
                                else
                                    return true;
                            }
                        }
                        else
                            return false;
                    }                
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", "al validar el limite "+ex.Message, "ok");
                return false;
            }
        }


        /****** se ingresa la venta a venta detalle y se agrega a la sumatoria *****/
        public void OnClickedAgregarNew(object sender, EventArgs args)
        {
            bool bResultado;

            if (txtCantidad.Text == "")
                _venta.iVendido = 0;
            else
                _venta.iVendido = Convert.ToInt16(txtCantidad.Text);

            if (ValidarAgregar() == true)
            {
                ValidaEnvase();
                if (_venta.bArtActualizado == true)
                {
                    if (_venta.iVendido == 0)
                    {
                        switch (_venta.fnEsPromoConjunto())
                        {
                            default:
                                bResultado = false;
                                break;
                            case 0:
                                bResultado = _venta.fnBorrarVenta();
                                break;
                            case 1:
                                bResultado = _venta.fnBorrarVentaConjunto();
                                break;
                        }
                    }
                    else
                        bResultado = _venta.fnActualizaVenta();
                }
                else
                    bResultado = _venta.fnGuardarVenta(0);
            }
            else
                bResultado = false;

            ///   si el proceso es correcto informa y limpia campos
            if (bResultado)
            {
                if (_venta.pProducto.arc_envase.ToString().Length > 0 && VarEntorno.cTipoVenta == 'A')
                {
                    int iAbono = 0;

                    if (txtCantEnvase.Text == "" || txtCantEnvase.Text == "0" || Convert.ToInt32(txtCantEnvase.Text)<0  )
                        iAbono = 0;
                    else
                        iAbono = Convert.ToInt32(txtCantEnvase.Text);
                    

                    if (_venta.fnActualizaEnvase(txtArticulo.Text, Convert.ToInt32(txtCantidad.Text), iAbono) == false)                    
                        DisplayAlert("Aviso", "Envase " + VarEntorno.sMensajeError, "OK");                    
                }
                else
                {
                    if (_venta.pProducto.arc_produ == "E")
                    {
                        if (_venta.fnVentaEnvase(txtArticulo.Text) == false)
                            DisplayAlert("Aviso", "Envase " + VarEntorno.sMensajeError, "OK");
                        else
                            DisplayAlert("Aviso", "Envase vendido", "OK");
                    }
                }

                //oUtilerias.crearMensaje("Articulo Guardado");
                VarEntorno.dImporteTotal = _venta.fnImporteTotalxFolio();
                lblTotal.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal);
                txtCantEnvase.IsVisible = false;
                lblEnva.IsVisible = false;
                LimpiarCampos();
                txtArticulo.Focus();
            }
            else            
                DisplayAlert("Alert", VarEntorno.sMensajeError.ToString(), "OK");
            
        }

        public bool ValidarAgregar()
        {
            int iTipoPago = serializarData();
            bool bResultado;
            bool bArticuloContado = true;

            if (iTipoPago == 0)
            {
                DisplayAlert("Aviso", "Seleccione un tipo de Pago", "OK");
                bResultado =  false;
            }
            else
            {
                //valida que exista un articulo ingresado
                if (txtArticulo.Text != null)
                {
                    //validad si el articulo es de venta de contado
                    bArticuloContado = _venta.ArticuloContado(txtArticulo.Text);
                    /*
                    if ((iTipoPago == 1 && bArticuloContado)
                        || bArticuloContado == false)
                        */
                    if (true)
                    {
                        // se valida que se un producto  tenga precio 
                        if (Convert.ToDecimal(lblPrecio.Text) > 1M)
                        {
                            if (Convert.ToInt32(txtCantidad.Text) >= 0)
                            {
                                //  valida que no supere su limite 
                                if (valida_limite(iTipoPago))
                                {
                                    ///valida candados se cumplan
                                    if (Valida_Candados())
                                    //if (true)
                                    {
                                        ///si ya ingreso anteriormente el articulo 
                                        if (_venta.bArtActualizado == true)
                                        {
                                            ///se desea eliminar el articulo capturado
                                            if (txtCantidad.Text == "" || txtCantidad.Text == "0")
                                                bResultado = true;
                                            else
                                            {
                                                /// si el articulo pertenece a una promocion 
                                                switch (_venta.fnEsPromoConjunto())
                                                {
                                                    default:
                                                        VarEntorno.sMensajeError = "error al buscar tipo prm venta ";
                                                        bResultado = false;
                                                        break;
                                                    case 1:
                                                        /// si la cantidad es menor a la capturada se denega
                                                        if (_venta.iVendidoAnt <= _venta.iVendido)
                                                            bResultado = true;
                                                        else
                                                        {
                                                            VarEntorno.sMensajeError = "Las promociones no se pueden reducir";
                                                            bResultado = false;
                                                        }
                                                        break;
                                                    case 0:
                                                        bResultado = true;
                                                        break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            /// es primer ingreso y es cero de denega 
                                            if (txtCantidad.Text == "" || txtCantidad.Text == "0")
                                            {
                                                VarEntorno.sMensajeError = "No se puedo guardar cantidades  en [ 0 ]";
                                                bResultado = false;
                                            }
                                            else
                                                bResultado = true;
                                        }
                                    }
                                    else
                                        bResultado = false;
                                }
                                else
                                {
                                    VarEntorno.sMensajeError = "Favor de terminar el Ticket y seguir la venta en otro ticket; se superó el límite de crédito que es: $" + VarEntorno.vCliente.cln_limite_venta.ToString();
                                    bResultado = false;
                                }
                            }
                            else
                            {
                                VarEntorno.sMensajeError = "No se pueden guardar cantidades menores a cero";
                                bResultado = false;
                            }
                        }
                        else
                        {
                            VarEntorno.sMensajeError = "No se puedo guardar importes con [ 0 ]";
                            bResultado = false;
                        }
                    }
                    else
                    {
                        VarEntorno.sMensajeError = "Articulo de venta de contado";
                        bResultado = false;
                    }
                }
                else
                {
                    VarEntorno.sMensajeError = "Favor ingresar Clave de producto";
                    bResultado = false;
                } 
            }

            return bResultado;
        }

        private bool Valida_Candados()
        {
            try
            {
                if (txtCantidad.Text == "" || txtCantidad.Text == "0")
                    return true;
                else
                {
                    //// buscar candados
                    if (_venta.fnCandadosArticulos(txtArticulo.Text) == true)
                    {
                        ////validar si cumple los candados
                        string sMensaje = _venta.fnValidadCantidad(txtArticulo.Text, Convert.ToInt16(txtCantidad.Text));
                        if (sMensaje == "")
                            return true;
                        else
                        {
                            VarEntorno.sMensajeError = sMensaje;
                            return false;
                        }
                    }
                    else
                        return true;
                }                
            }
            catch (Exception ex)
            {
                VarEntorno.sMensajeError = ex.Message;
                return false;
            }
        }

        /***** si es autoventa valida que ingrese uan cantidad en los envaes ******/
        private bool ValidaEnvase()
        {
            try
            {
                bool bResultado;
                int iCantEnvase = 0;

                if (lblEnva.IsVisible == true)
                {
                    if (txtCantEnvase.Text == "" )                    
                        iCantEnvase = 0;                    
                    else
                        iCantEnvase = Convert.ToInt16(txtCantEnvase.Text);
                    
                    if ( _venta.bArtActualizado)  
                        _venta.iAbonoEnvase = iCantEnvase * -1;                      
                   else
                       _venta.iAbonoEnvase = iCantEnvase;

                    bResultado = true;
                   
                }
                else
                    bResultado = true;                

                return bResultado;
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", "Error al validar envase"+ex.Message, "OK");
                return false;
            }
        }

        //  manda llamar a la pantalla de Resumen de Venta
        private void onClickedResumen(object sender, EventArgs e)
        {
            if (VarEntorno.cTipoVenta == 'A')
            {
                this.Navigation.PushModalAsync(new frmResumenAutoVenta());
            }
            else
            {
                this.Navigation.PushModalAsync(new frmResumenVenta());                
            }            
        }

        private async void OnClickedRegresar(object sender, EventArgs args)
        {
            bool bRespuesta = await DisplayAlert("Pregunta", "¿Desea regresar y borrar la información del Ticket?", "Si", "No");

            if (bRespuesta == true)
            {
                bRespuesta = await DisplayAlert("Pregunta", "¿Desea borrar la totalidad del Ticket actual?", "Si", "No");
                if (bRespuesta == true)
                {
                    int iCliente = VarEntorno.vCliente.cln_clave;
                    bool bBorrarEnvaseTemp = _envase.FtnBorrarEnvaseTemp(iCliente);
                    await _venta.fnLimpiaTicket(VarEntorno.iFolio);
                    VarEntorno.LimpiaVariables();
                    this.Navigation.PopModalAsync();
                }
            }
        }

        private void OnClickedPromociones(object sender, EventArgs args)
        {
            //  manda llamar a la pantalla de promociones
            this.Navigation.PushModalAsync(new frmPromociones());
        }

        private void OnClickedCobranza(object sender, EventArgs args)
        {
            int iTipoPago = serializarData();

            if (iTipoPago == 2 )
            {
                //validad si el articulo es de venta de contado 
                //if (_venta.ArticulosDeContado())
                if (true)
                {
                    //////valida a credito 
                    if (VarEntorno.dImporteTotal <= VarEntorno.vCliente.cln_limite_venta
                        || VarEntorno.vCliente.clc_cobrador == ""
                        || VarEntorno.vCliente.cln_limite_venta == 0)
                    {
                        VarEntorno.bVentaContado = false;
                        this.Navigation.PopModalAsync();
                        this.Navigation.PushModalAsync(new frmCobranza());
                    }
                    else
                    {
                        DisplayAlert("Aviso", "Reduzca Venta o Cambie a Contado", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Aviso", "Articulos restringidos de contado ", "OK");                    
                }
            }
            else
            {
                VarEntorno.bVentaContado = true;
                this.Navigation.PopModalAsync();
                this.Navigation.PushModalAsync(new frmCobranza());
            }
        }

        private void txtCantidad_Completed(object sender, EventArgs e)
        {
            if (VarEntorno.cTipoVenta == 'A' && txtCantEnvase.IsVisible)
                txtCantEnvase.Focus();
        }

    }
}
