using System;
using Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmDevolucionAutoventa : ContentPage
	{
        VentaVM _venta = new VentaVM();

        public frmDevolucionAutoventa()
        {
            InitializeComponent();
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            InicioVenta();
        }        

        /*******  Se inicializa variables de subtotal , total y se obtine folio de venta *****/
        private void InicioVenta()
        {            
            VarEntorno.iFolio = VarEntorno.TraeFolio();
            VarEntorno.dImporteTotal = _venta.fnImporteTotalxFolio();
            lblSubtotal.Text = String.Format("{0:N2}", 0);
            lblTotal.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal);
        }

        /****** al terminar la captura se obtien los datos del articulo para mostrar *****/
        private void Entry_Completed_txtArticulo(object sender, EventArgs e)
        {
            _venta = new VentaVM();
            //  se busca la informacion del articulo para llenar pantalla 
            if (txtArticulo.Text.Trim().Length > 0)
            {
                if (_venta.fnCargaProducto(txtArticulo.Text))
                {
                    lblCorta.Text = _venta.pProducto.ard_corta;
                    lblDescripcion.Text = _venta.pProducto.ard_descripcion;
                    lblPrecio.Text = String.Format("{0:N2}", _venta.dPrecio);
                    txtCantidad.Text = _venta.iDevuelto > 0 ? _venta.iDevuelto.ToString():"";

                    if (txtCantidad.Text.Trim().Length > 0 && _venta.iVendido > 0)
                        lblSubtotal.Text = String.Format("{0:N2}", _venta.iVendido * _venta.dPrecio);
                    else
                        lblSubtotal.Text = String.Format("{0:N2}", 0);

                    txtCantidad.Focus();
                }
                else
                {
                    //  error al cargar la informacion del articulo 
                    DisplayAlert("Aviso", VarEntorno.sMensajeError, "OK");
                    LimpiarCampos();
                }
            }
            else
            {
                // error no ingreso nada
                DisplayAlert("Alerta", "Favor de ingresar una clave", "OK");
                LimpiarCampos();
            }

            VarEntorno.sMensajeError = "";
        }

        /****** limpia los campos de la pantalla de venta  ******/
        private void LimpiarCampos()
        {
            lblCorta.Text = "";
            lblDescripcion.Text = "";
            lblPrecio.Text = String.Format("{0:N2}", 0);
            txtCantidad.Text = "";            
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

        //  manda llamar a la pantalla de Resumen de Venta
        private void onClickedResumen(object sender, EventArgs e)
        {
            this.Navigation.PushModalAsync(new frmResumenVenta());
        }

        /****** se ingresa la venta a venta_detalle y se agrega a la sumatoria *****/
        public void OnClickedAgregar(object sender, EventArgs args)
        {
            bool bResultado;
            Utilerias oUtilerias = new Utilerias();

            if (txtArticulo.Text != null)
            {
                // se valida que se un producto  con precio 
                if (Convert.ToDecimal(lblPrecio.Text) > 0M)
                {
                    // se valida que exista cantidad de venta 
                    if (txtCantidad.Text == "0" && txtCantidad.Text == "")
                    {
                        if (_venta.bArtActualizado == true)
                        {
                            ////Borrar devolucion
                            bResultado = _venta.fnBorrarVenta();
                        }
                        else
                        {
                            DisplayAlert("Alert",   "No se puedo guardar cantidades en [ 0 ]", "OK");
                            bResultado = false;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt16(txtCantidad.Text) >= 0)
                        {
                            bResultado = ValidaEnvase();
                            if (bResultado)
                            {
                                _venta.iDevuelto = Convert.ToInt16(txtCantidad.Text);

                                /// se verifica si es una venta nueva o actualizacion de producto 
                                if (_venta.bArtActualizado)
                                    bResultado = _venta.fnActualizaDevolucion();
                                else
                                    bResultado = _venta.fnGuardarDevolucion();

                                if (_venta.fnActualizaEnvase(txtArticulo.Text, 0, Convert.ToInt32(txtCantidad.Text)) == false)
                                    DisplayAlert("Aviso", "Envase " + VarEntorno.sMensajeError, "OK");

                            }
                        }
                        else
                        {
                            DisplayAlert("Alert", "No se puedo guardar cantidades en [ 0 ]", "OK");
                            bResultado = false;
                        }
                    }
                }
                else
                {
                    DisplayAlert("Alert", "No se puedo guardar importes con [ 0 ]", "OK");
                    bResultado = false;
                }
            }
            else
            {
                DisplayAlert("Aviso", "Favor ingresar Clave de producto", "OK");
                bResultado = false;
            }


            ///   si el proceso es correcto informa y limpia campos
            if (bResultado)
            {
                if (_venta.pProducto.arc_envase.ToString().Length > 0 &&
                    _venta.fnActualizaEnvaseDev() == false)
                    DisplayAlert("Aviso", "Envase " + VarEntorno.sMensajeError, "OK");
                else
                {
                    oUtilerias.crearMensaje("Articulo Guardado");
                    VarEntorno.dImporteTotal = Convert.ToDecimal(lblTotal.Text) + Convert.ToDecimal(lblSubtotal.Text);
                    lblTotal.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal);
                    txtArticulo.Focus();
                    LimpiarCampos();
                }
            }
            else
                if (VarEntorno.sMensajeError!="")
                    DisplayAlert("Aviso", VarEntorno.sMensajeError, "OK");
        }

        /***** si es autoventa valida que ingrese uan cantidad en los envaces ******/
        private bool ValidaEnvase()
        {
            try
            {
                bool bResultado;

                if (_venta.pProducto.arc_clave != _venta.pProducto.arc_envase)
                {
                    /*
                    if (!(_venta.pProducto.arc_envase == null || _venta.pProducto.arc_envase == ""))
                    {
                        int iCant = Convert.ToInt16(txtCantidad.Text)+_venta.fnArticulosDevxEnvase(VarEntorno.vCliente.cln_clave, _venta.pProducto.arc_envase);
                        if (iCant  <= _venta.iSaldoEnvase)
                            bResultado = true;
                        else
                        {
                            bResultado = false;
                            DisplayAlert("Aviso", "No puede devolver mas de que lo se debe de envase ", "OK");
                        }
                    }
                    else
                    */
                        bResultado = true;
                }
                else
                    bResultado = false;

                return bResultado;
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", "Error al validar envase" + ex.Message, "OK");
                return false;
            }
        }

        private async void  OnClickedRegresar(object sender, EventArgs args)
        {
            bool bRespuesta = await DisplayAlert("Pregunta", "¿Desea regresar y borrar la información del Ticket?", "Si", "No");

            if (bRespuesta == true)
            {
                bRespuesta = await DisplayAlert("Pregunta", "¿Desea borrar la totalidad del Ticket actual?", "Si", "No");
                if (bRespuesta == true)
                {                                     
                    await _venta.fnLimpiaTicket(VarEntorno.iFolio);
                    VarEntorno.LimpiaVariables();
                    this.Navigation.PopModalAsync();
                }
            }
        }

        public void OnClickedCobranza(object sender, EventArgs args)
        {

                this.Navigation.PopModalAsync();
                this.Navigation.PushModalAsync(new frmCobranza());
            
        }

    }
}
