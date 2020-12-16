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
	public partial class frmDevolucionReparto : ContentPage
	{
        DevolucionVM devVM = new DevolucionVM();

        public frmDevolucionReparto ()
		{
			InitializeComponent ();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            string sIdCliente = VarEntorno.vCliente.cln_clave.ToString().Trim();
            string sNombreCliente = VarEntorno.vCliente.clc_nombre == null ? "" : VarEntorno.vCliente.clc_nombre.Trim();
            if (sNombreCliente.Trim() == "")
                sNombreCliente = VarEntorno.vCliente.clc_nombre_comercial == null ? "" : VarEntorno.vCliente.clc_nombre_comercial.Trim();
            lblClienteDevol.Text = sIdCliente + " - " + sNombreCliente;

            CargaMotivos();
            /*
            Device.BeginInvokeOnMainThread(() =>
            {
                if (pckMotDevoluciones.IsFocused)
                    pckMotDevoluciones.Unfocus();

                pckMotDevoluciones.Focus();
            });
            */
            
        }

        /*****carga motivos de devolucion *****/
        private void CargaMotivos()
        {
            

            if (devVM.lConsepDev())
            {
                foreach (var Consepto in devVM.ListaConseptos)
                    pckMotDevoluciones.Items.Add(Consepto.cdc_descripcion);

                VarEntorno.iFolio = VarEntorno.TraeFolio();
                
            }
            else
                DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
        }

        /***** Mandar a llamar la funcion del vistamodelo que genere la devolucion ***///
        public async void OnClickedAceptar(object sender, EventArgs args)
        {
            try
            {
                string sConceptoDevol = "";

                Utilerias oUtilerias = new Utilerias();
                //DevolucionVM _devolucion = new DevolucionVM();
                ////verifica la seleccion de un motivo
                if (pckMotDevoluciones.SelectedIndex >= 0 )
                {
                    /// valida que el cliente no tenga otra entrega o devolucion 
                    if (await devVM.ValidaEstatusCliente())
                    {
                        if (devVM.FacConPagos()  || VarEntorno.vCliente.cln_clave == 32507)
                        {
                            ////señala si se genero la devolucion del cliente 
                            if (devVM.DevolucionReparto(pckMotDevoluciones.SelectedIndex + 1))
                            {
                                oUtilerias.obtenerCoordenadas(4);
                                #region Imprime el Ticket de Devolucion las veces que sean necesarias
                                if (VarEntorno.sTipoImpresora == "Zebra")
                                {
                                    ZeImprimeDevolucionRVM ticketDevolucion = new ZeImprimeDevolucionRVM();

                                    bool bRespuesta;

                                    do
                                    {
                                        bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Devolución?", "Si", "No");

                                        if (bRespuesta == true)
                                        {
                                            sConceptoDevol = (pckMotDevoluciones.SelectedIndex + 1).ToString().Trim() + " - " + pckMotDevoluciones.Items[pckMotDevoluciones.SelectedIndex].Trim();

                                            ticketDevolucion.FtnImprimirTicketDevolucionR(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'), sConceptoDevol);
                                        }
                                    }
                                    while (bRespuesta);
                                }
                                else
                                {
                                    ImprimeDevolucionRVM ticketDevolucion = new ImprimeDevolucionRVM();

                                    bool bRespuesta;

                                    do
                                    {
                                        bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Devolución?", "Si", "No");

                                        if (bRespuesta == true)
                                        {
                                            sConceptoDevol = (pckMotDevoluciones.SelectedIndex + 1).ToString().Trim() + " - " + pckMotDevoluciones.Items[pckMotDevoluciones.SelectedIndex].Trim();

                                            ticketDevolucion.FtnImprimirTicketDevolucionR(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'), sConceptoDevol);
                                        }
                                    }
                                    while (bRespuesta);
                                }
                                #endregion

                                VarEntorno.LimpiaVariables();
                                oUtilerias.crearMensaje("¡DEVOLUCION EXITOSA!");
                                await this.Navigation.PopModalAsync();
                            }
                            else
                                await DisplayAlert("Aviso", "Error: " + VarEntorno.sMensajeError, "OK");
                        }
                        else
                        {
                            await DisplayAlert("Aviso", " Cliente con complementos se denega devolucion ATTE:SAT ", "OK");
                            await this.Navigation.PopModalAsync();
                        }
                    }
                    else
                        await DisplayAlert("Aviso", "Cliente ya fue Visitado", "Ok");
                }
                else
                {
                    await DisplayAlert("Aviso", "Seleccione el Motivo de Devolución.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", "Error: " + ex.ToString(), "OK");
            }           
        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
    }
}
