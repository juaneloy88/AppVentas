using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmDevolucionAutoventaDocto : ContentPage
	{
        TicketsDevolucionVM ticketsCabecerasDevolucion = null;
       // TicketsDevolucionVM ticketsDetallesDevolucion = null;
        List<tickets_cabeceras> lstTicketsCabecerasDevolucion = null;
        List<venta_detalle> lstTicketsDetallesDevolucion = null;

        Utilerias oUtilerias = new Utilerias();

        string sObservacion = string.Empty;

        public frmDevolucionAutoventaDocto ()
		{
			InitializeComponent ();

            lblRutaPlusTipoVenta.Text = VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            txtNombre.Text = VarEntorno.vCliente.cln_clave+"  "+ VarEntorno.vCliente.clc_nombre;

            VarEntorno.iFolio = VarEntorno.TraeFolio();

            if (VarEntorno.cCobranza is null)
                VarEntorno.cCobranza = new CobranzaVM();
        }


        private async void OnClickedDevolver(object sender, EventArgs args)
        {
            if (sObservacion.Length > 0)
            {
                await DisplayAlert("Aviso", "Atencion  Folio  "+ sObservacion, "OK");
            }
            else
            {
                if (ticketsCabecerasDevolucion != null)
                {
                    if (await ticketsCabecerasDevolucion.DevolucionAutoventa(pckTickets.SelectedIndex))
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
                                   // sConceptoDevol = (pckMotDevoluciones.SelectedIndex + 1).ToString().Trim() + " - " + pckMotDevoluciones.Items[pckMotDevoluciones.SelectedIndex].Trim();

                                    ticketDevolucion.FtnImprimirTicketDevolucionR(VarEntorno.vCliente.cln_clave, VarEntorno.iFolio.ToString().PadLeft(6, '0'), "");
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
                                    //sConceptoDevol = (pckMotDevoluciones.SelectedIndex + 1).ToString().Trim() + " - " + pckMotDevoluciones.Items[pckMotDevoluciones.SelectedIndex].Trim();

                                    ticketDevolucion.FtnImprimirTicketDevolucionR(VarEntorno.vCliente.cln_clave, txtFolio.Text.ToString().PadLeft(6, '0'), "");
                                }
                            }
                            while (bRespuesta);
                        }
                        #endregion
                        this.Navigation.PopModalAsync();
                        VarEntorno.LimpiaVariables();
                        oUtilerias.crearMensaje("¡DEVOLUCION EXITOSA!");
                    }
                    else                    
                        await DisplayAlert("Aviso", VarEntorno.sMensajeError, "OK");                    
                }
                else
                    await DisplayAlert("Aviso", "Sin ticket", "OK");


            }
        }

        private async void OnClickedBuscar(object sender, EventArgs args)
        {
            pckTickets.Items.Clear();
            lblEstatus.Text = "";

            if (txtFolio.Text.Length != 6)
            {
                await DisplayAlert("Aviso", "El Folio del Ticket debe de ser de 6 números.", "OK");
            }
            else
            {
                ticketsCabecerasDevolucion = new TicketsDevolucionVM();
                lstTicketsCabecerasDevolucion = new List<tickets_cabeceras>();

                lstTicketsCabecerasDevolucion = ticketsCabecerasDevolucion.FtnRegresarTicketsDevolucion(VarEntorno.vCliente.cln_clave.ToString(), txtFolio.Text);

                if (lstTicketsCabecerasDevolucion == null)
                {
                    await DisplayAlert("Aviso", "No hay Tickets con ese Folio susceptibles a Devolución.", "OK");
                }
                else
                {
                    
                    lblEstatus.Text = "ENCONTRADO";
                    lblEstatus.TextColor = Color.DarkOliveGreen;
                    lblEstatus.FontAttributes = FontAttributes.Bold;

                    foreach (var item in lstTicketsCabecerasDevolucion)
                    {
                        

                        if (item.tcb_esta_vencido == true)
                        {
                            //sObservacion = "VENCIDO";
                        }
                        else
                            if (item.tcb_tiene_complemento == true)
                        {
                            //sObservacion = "CON PAGO";
                        }

                        pckTickets.Items.Add(
                                item.tcc_folio.ToString().PadLeft(6, '0') + "    " +
                                item.tcf_movimiento.ToString("dd/MM/yyyy") + "    " +
                                item.tcn_importe.ToString("N").PadLeft(9, ' ') + "    " +
                                sObservacion
                            );
                    }
                }
            }
        }

        private async void OnClickedDetalle(object sender, EventArgs args)
        {
            var ticketSelected = ticketsCabecerasDevolucion.lstTicketsCabeceras[pckTickets.SelectedIndex];

            if (ticketSelected.tcb_esta_vencido == true)
            {
                sObservacion = "VENCIDO";
                await DisplayAlert("Aviso", "El Ticket con ese Folio no es susceptible a Devolución porque ya está VENCIDO.", "OK");
            }
            else if (ticketSelected.tcb_tiene_complemento == true)
            {
                sObservacion = "CON PAGO";
                await DisplayAlert("Aviso", "El Ticket con ese Folio no es susceptible a Devolución porque ya tiene PAGOS .", "OK");
            }
            else
            {
                //ticketsCabecerasDevolucion = new TicketsDevolucionVM();
                lstTicketsDetallesDevolucion = new List<venta_detalle>();

                ticketsCabecerasDevolucion.sUUID = ticketSelected.tcc_cadena_original;

                lstTicketsDetallesDevolucion = ticketsCabecerasDevolucion.FtnRegresarTicketsDetallesDevolucion(ticketSelected.tcn_cliente.ToString(), ticketSelected.tcc_folio, ticketSelected.tcf_movimiento);

                pckDetalle.Items.Clear();

                foreach (var item in lstTicketsDetallesDevolucion)
                {
                    pckDetalle.Items.Add(
                            item.vdn_venta.ToString().PadLeft(3, ' ') + "    " +
                            item.vdn_producto.ToString().PadLeft(3, ' ') + "    " +
                            item.vdn_precio.ToString("N").PadLeft(9, ' ') + "    " +
                            item.vdn_importe.ToString("N").PadLeft(9, ' ')
                        );
                }
            }
        }

        private async void OnClickedRegresar(object sender, EventArgs args)
        {
            bool bRespuesta = await DisplayAlert("Pregunta", "¿Desea cancelar el Ticket actual?", "Si", "No");

            if (bRespuesta == true)
            {
                bRespuesta = await DisplayAlert("Pregunta", "¿esta seguro que desea cancelar el Ticket actual?", "Si", "No");
                if (bRespuesta == true)
                {                    
                    VarEntorno.LimpiaVariables();
                    this.Navigation.PopModalAsync();
                }
            }
        }

    }
}