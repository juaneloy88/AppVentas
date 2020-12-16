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
	public partial class FrmNoVenta : ContentPage
	{
        VisitaVM VisVM = new VisitaVM();

        public FrmNoVenta ()
		{
			InitializeComponent ();
            CargaMotivos();
        }

        /*****carga motivos de devolucion *****/
        private void CargaMotivos()
        {
            
            if (VisVM.lConseptosNoventa())
            {
                foreach (var Consepto in VisVM.ListaConseptos)
                    pckMotNoventa.Items.Add(Consepto.svc_descripcion);
               
            }
            else
                DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
        }

        /***** Mandar a llamar la funcion del vistamodelo que genere la devolucion ***///
        public async void OnClickedAceptar(object sender, EventArgs args)
        {
            try
            {
                Utilerias oUtilerias = new Utilerias();
                VisitaVM NoVenta = new VisitaVM();
                ////verifica la seleccion de un motivo
                if (pckMotNoventa.SelectedIndex >= 0)
                {
                    /// valida que el cliente no tenga otra entrega o devolucion 
                    if ( NoVenta.ValidaEstatusCliente())
                    {
                        ////señala si se genero la devolucion del cliente 
                        if (NoVenta.GuardaMotivo(pckMotNoventa.SelectedIndex + 1))
                        {
                            oUtilerias.obtenerCoordenadas(9);
                            /*
                            if (VarEntorno.sTipoImpresora == "Zebra")
                            {
                                ZeImprimeVisitaAPVM ticketVisita = new ZeImprimeVisitaAPVM();

                                bool bRespuesta;
                                var oDialogoCargando = oUtilerias.crearProgressDialog("Imprimiendo", "Espere por favor...");

                                do
                                {
                                    bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Visita?", "Si", "No");

                                    if (bRespuesta == true)
                                    {
                                        oDialogoCargando.Show();
                                        await Task.Run(() =>
                                        {
                                            Device.BeginInvokeOnMainThread(() =>
                                            {
                                                ticketVisita.FtnImprimirTicketVisitaAP(VarEntorno.vCliente.cln_clave
                                                                                    , VarEntorno.iFolio.ToString().PadLeft(6, '0')
                                                                                    , VisVM.ListaConseptos[pckMotNoventa.SelectedIndex].svc_descripcion);
                                            });
                                        });
                                        oDialogoCargando.Dismiss();
                                    }
                                }
                                while (bRespuesta);
                            }*/
                            oUtilerias.crearMensaje("Visita EXITOSA!!");
                            //se imprime ticket                        
                            VarEntorno.LimpiaVariables();
                            await this.Navigation.PopModalAsync();
                        }
                        else
                            await DisplayAlert("Aviso", "Error: " + VarEntorno.sMensajeError, "Ok");
                    }
                    else
                        await DisplayAlert("Aviso", "Cliente ya fue Visitado", "Ok");
                }
                else
                {
                    await DisplayAlert("Aviso", "Seleccione un Motivo", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", "Error: " + ex.ToString(), "Ok");
            }
        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
    }
}
