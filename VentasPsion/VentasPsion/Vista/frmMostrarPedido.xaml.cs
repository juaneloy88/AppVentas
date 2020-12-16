using Base;
using System;
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
    public partial class frmMostrarPedido : ContentPage
    {
        vmMostrarPedido vmmuestraPedido = new vmMostrarPedido();
        List<MostrarPedido> vPedido = new List<MostrarPedido>();

        public frmMostrarPedido()
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            VarEntorno.iFolio = VarEntorno.TraeFolio();//new rutaSR().GetFolio();
            muestraPedido();

            //VarEntorno.bVentaContado = false;
        }

        #region Validación para conocer si el cliente ya tiene el status de entrega
        public async void validaStatusCliente()
        {
            string sCliente = VarEntorno.vCliente.cln_clave.ToString();

            string sRespuesta = await vmmuestraPedido.validaStatusCliente(sCliente);

            if (sRespuesta == "Cliente NO Visitado")
            {
                muestraPedido();
            }
            else
            {
                Utilerias oUtilerias = new Utilerias();
                oUtilerias.crearMensaje(sRespuesta);

                await this.Navigation.PopModalAsync();
            }
        }
        #endregion Validación para conocer si el cliente ya tiene el status de entrega

        #region Método que muestra el pedido
        public async void muestraPedido()
        {
            try
            {

                #region Declaración de Variables
                int iVenta = 0;
                decimal dImporte = 0;
                int iTotVenta = 0;
                decimal dTotImporte = 0;
                VarEntorno.dImporteTotal = 0;
                string sCliente = VarEntorno.vCliente.cln_clave.ToString();
                lsvMostrarPedido.ItemsSource = null;
                #endregion Declaración de Variables

                vPedido = await vmmuestraPedido.muestraPedido(sCliente);

                if (vPedido.Count >= 1)
                {
                    foreach (MostrarPedido muestraPedido in vPedido)
                    {
                        iVenta = muestraPedido.iVenta;
                        dImporte = muestraPedido.dImporte;

                        iTotVenta = iTotVenta + iVenta;
                        dTotImporte = dTotImporte + dImporte;
                    }

                    txtTotCartones.Text = iTotVenta.ToString();
                    txtTotImporte.Text = "$" + String.Format("{0:#,0.00}", dTotImporte);

                    VarEntorno.dImporteTotal = dTotImporte;

                    lsvMostrarPedido.ItemsSource = vPedido;
                }
                else
                {
                    Utilerias oUtilerias = new Utilerias();
                    oUtilerias.crearMensaje("No existen Registros");
                }
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion Método que muestra el pedido

        #region Botón de Mostrar Pedido
        public void OnClickedMostrar(object sender, EventArgs args)
        {
            validaStatusCliente();
        }
        #endregion Botón de Mostrar Pedido

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
                }
            }
        }
        #endregion  Botón de Regresar

        #region Botón de Avanzar
        public void OnClickedAvanzar(object sender, EventArgs args)
        {
            try
            {
                /* if (vPedido.Count >= 1)
                 {
                     this.Navigation.PopModalAsync();
                     this.Navigation.PushModalAsync(new frmCapturaEnvase());
                 }
                 else
                 {
                     Utilerias oUtilerias = new Utilerias();
                     oUtilerias.crearMensaje("No existen Registros");
                 }*/
                this.Navigation.PopModalAsync();
                this.Navigation.PushModalAsync(new frmCapturaEnvase());
            }
            catch (Exception ex)
            {
                muestraException(ex.ToString());
            }
        }
        #endregion  Botón de Avanzar
    }
}