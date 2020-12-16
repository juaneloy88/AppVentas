using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VentasPsion.Modelo.Servicio;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmAbonoEnvase : ContentPage
    {
        /*Método constructor de la clase*/
        public frmAbonoEnvase()
        {
                InitializeComponent();

                lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
                lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
                lblVersionApp.Text = VarEntorno.sVersionApp;

                btnVerResumen.IsEnabled = false;            
        }

        /*Método para guardar los datos del Abono de Envase de una Cliente específico*/
        public async void OnClickedGuardar(object sender, EventArgs args)
        {
            Utilerias utilerias = new Utilerias();
            // Valida que se haya capturado el ID del Cliente.
            if (string.IsNullOrEmpty(entIdCliente.Text))
            {
                await DisplayAlert("Aviso", "Ha olvidado capturar el ID del Cliente.", "OK");
                entIdCliente.Focus();
            }
            else
            {
                // Valida que se haya capturado el ID del Cliente y con ello obtenido y mostrado su Nombre.
                if (string.IsNullOrEmpty(lblNombreCliente.Text))
                {
                    await DisplayAlert("Aviso", "No ha buscado el ID del Cliente para mostrar su Nombre.", "OK");
                    entIdCliente.Focus();
                }
                else
                {
                    // Valida que se haya seleccionado el Tipo de Envase a abonoar.
                    if (pkrEnvases.SelectedIndex == -1)
                    {
                        await DisplayAlert("Aviso", "Ha olvidado seleccionar el Tipo de Envase.", "OK");
                        pkrEnvases.Focus();
                    }
                    else
                    {
                        // Valida que se haya capturado la Cantidad de envase a abonar.
                        if (string.IsNullOrEmpty(entCantidadEnvase.Text))
                        {
                            await DisplayAlert("Aviso", "Ha olvidado capturar la Cantidad de envase a abonar.", "OK");
                            entCantidadEnvase.Focus();
                        }
                        else
                        {
                            // Valida que no se capture un punto en la Cantidad de envase a abonar.
                            if (entCantidadEnvase.Text == ".")
                            {
                                await DisplayAlert("Aviso", "Ha capturado un punto ( . ) en lugar de una Cantidad de envase a abonar.", "OK");
                                entCantidadEnvase.Focus();
                            }
                            else
                            {
                                int iCantidadEnvase;
                                // Valida que no se capture un número decimal en la Cantidad de envase a abonar.
                                if (!int.TryParse(entCantidadEnvase.Text, out iCantidadEnvase) )
                                {
                                    await DisplayAlert("Aviso", "No es válido un decimal en la Cantidad de envase a abonar.", "OK");
                                    entCantidadEnvase.Focus();
                                }
                                else
                                {
                                    bool bRespuesta = await DisplayAlert("Pregunta", "¿Seguro que desea guardar el Abono de Envase?", "Si", "No");

                                    if (bRespuesta == true)
                                    {
                                        StatusService statusServiceValidaAbonos = new StatusService();
                                        EnvaseService envaseService = new EnvaseService();

                                        string sTipoEnvase = (string)pkrEnvases.Items[pkrEnvases.SelectedIndex];
                                        char cTipoEnvase = sTipoEnvase.ToCharArray(0, 1)[0];

                                        // Manda llamar a la función que valida si ya fue guardado el Abono de un cierto Tipo de Envase del Cliente enviado como parámetro.
                                        statusServiceValidaAbonos = envaseService.FtnValidarAbonosGuardados(Convert.ToInt32(entIdCliente.Text), cTipoEnvase);

                                        if (statusServiceValidaAbonos.status == false)
                                        {
                                            bRespuesta = await DisplayAlert("Pregunta", statusServiceValidaAbonos.mensaje + "\n" + 
                                                                            "¿Desea sumarle la cantidad de " + entCantidadEnvase.Text + 
                                                                            " que capturó ahora para un total de " + 
                                                                            (Convert.ToInt32(entCantidadEnvase.Text) + Convert.ToInt32(statusServiceValidaAbonos.valor)) + 
                                                                            "?", "Si", "No");
                                            if (bRespuesta == false)
                                            {
                                                return;
                                            }
                                        }

                                        StatusService statusServiceGuardaAbono = new StatusService();

                                        // Manda llamar a la función que guarda el Abono de un cierto Tipo de Envase del Cliente enviado como parámetro.
                                        int iCantidad = 0;
                                        if (Convert.ToInt32(entCantidadEnvase.Text) < 0)
                                            iCantidad = 0;
                                        else
                                            iCantidad = Convert.ToInt32(entCantidadEnvase.Text);
                                        statusServiceGuardaAbono = envaseService.FtnGuardarAbonoEnvase(Convert.ToInt32(entIdCliente.Text), cTipoEnvase, iCantidad);

                                        if (statusServiceGuardaAbono.status == true)
                                        {
                                            entCantidadEnvase.Text = "";

                                            #region Vuelve a cargar los Saldos de Envase de cada Tipo una vez que se guardó lo que se abonó
                                            pkrEnvases.Items.Clear();

                                            StatusService statusServiceRegresaEnvase = new StatusService();
                                            EnvaseVM listaEnvaseVM = new EnvaseVM();

                                            // Manda llamar a la función que regresa la Lista de Envases a los cuales puede abonar el Cliente enviado como parámetro.
                                            statusServiceRegresaEnvase = listaEnvaseVM.FtnRegresarEnvasesPorClienteVM(Convert.ToInt32(entIdCliente.Text));

                                            if (statusServiceRegresaEnvase.status == true)
                                            {
                                                if (statusServiceRegresaEnvase.listaStrings.Count >= 1)
                                                {
                                                    // Agrega al Picker la Lista de Envases a los cuales puede abonar el Cliente.
                                                    foreach (string promo in statusServiceRegresaEnvase.listaStrings)
                                                    {
                                                        string sPromocion = promo.ToString().Trim();
                                                        pkrEnvases.Items.Add(sPromocion);
                                                    }
                                                }

                                                btnVerResumen.IsEnabled = true;
                                            }
                                            else
                                            {
                                                await DisplayAlert("¡Atención!", statusServiceRegresaEnvase.mensaje, "OK");
                                            }
                                            #endregion

                                            pkrEnvases.SelectedIndex = -1;

                                            #region Imprime el Ticket de Abono de Envase las veces que sean necesarias
                                            //ImprimeAbonoEnvaseAVM ticketAbonoEnvase = new ImprimeAbonoEnvaseAVM();

                                            //do
                                            //{
                                            //    bRespuesta = await DisplayAlert("Pregunta", "¿Desea imprimir el Ticket de Abono de Envase?", "Si", "No");

                                            //    if (bRespuesta == true)
                                            //    {
                                            //        ticketAbonoEnvase.FtnImprimirTicketAbonoEnvaseA(Convert.ToInt32(entIdCliente.Text), statusServiceGuardaAbono.valor.Trim());
                                            //    }

                                            //}
                                            //while (bRespuesta);
                                            #endregion

                                            //utilerias.crearMensaje(statusServiceGuardaAbono.mensaje);
                                            //await DisplayAlert("Aviso", statusServiceGuardaAbono.mensaje, "OK");
                                            utilerias.crearMensaje("¡ABONO EXITOSO!");
                                        }
                                        else
                                        {
                                            await DisplayAlert("¡Atención!", statusServiceGuardaAbono.mensaje, "OK");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /*Método para validar que el Cliente capturado exista según la ruta cargada en Recepción y carga su Lista de Envases a los cuales puede abonar*/
        public async void OnClickedBuscar(object sender, EventArgs args)
        {
            btnVerResumen.IsEnabled = false;
            lblNombreCliente.Text = "";
            VentaVM _venta = new VentaVM();
            
            
                // Valida que se haya capturado el ID del Cliente.
                if (String.IsNullOrEmpty(entIdCliente.Text))
                {
                    await DisplayAlert("Aviso", "Ha olvidado capturar el ID de Cliente.", "OK");
                    entIdCliente.Focus();
                }
                else
                {
                    // Valida que no se capture un punto en el ID del Cliente.
                    if (entIdCliente.Text == ".")
                    {
                        await DisplayAlert("Aviso", "Ha capturado un punto ( . ) en lugar de un ID de Cliente.", "OK");
                        entIdCliente.Focus();
                    }
                    else
                    {
                        int iIdCliente;
                        // Valida que no se capture un número decimal en el ID del Cliente.
                        if (!int.TryParse(entIdCliente.Text, out iIdCliente))
                        {
                            await DisplayAlert("Aviso", "No es válido un decimal en el ID de Cliente.", "OK");
                            entIdCliente.Focus();
                        }
                        else
                        {
                            vmMostrarPedido vmmuestraPedido = new vmMostrarPedido();
                            List<MostrarPedido> vPedido = await vmmuestraPedido.muestraPedido(entIdCliente.Text);

                            if (vPedido.Count > 0 && VarEntorno.cTipoVenta == 'R')
                            {
                                await DisplayAlert("Aviso", "Cliente con pedido ingresar a PEDIDO", "OK");
                                entIdCliente.Focus();
                            }
                            else
                            {
                                clientesSR buscaCliente = new clientesSR();

                                // Manda llamar a la función que valida que el Cliente enviado como parámetro exista en la tabla de la base de datos según la ruta cargada en Recepción.
                                string sRespuesta = buscaCliente.ValidaCliente(entIdCliente.Text);

                                if (sRespuesta == "Ok")
                                {
                                    // Manda llamar a la función que regresa los datos del Cliente enviado como parámetro para mostrar su Nombre en pantalla.
                                    var cliente = await buscaCliente.DatosCliente(entIdCliente.Text);
                                    VarEntorno.vCliente = cliente;
                                    int iExisteVenta = _venta.fnExisteDevolucion();
                                    if (iExisteVenta != 1)
                                    
                                    { 
                                        if (string.IsNullOrEmpty(cliente.clc_nombre))
                                        {
                                            lblNombreCliente.Text = cliente.clc_nombre_comercial.ToString();
                                        }
                                        else
                                        {
                                            lblNombreCliente.Text = cliente.clc_nombre.ToString();
                                        }

                                        if (string.IsNullOrEmpty(cliente.clc_nombre_comercial))
                                        {
                                            lblNegocioCliente.Text = cliente.clc_nombre.ToString();
                                        }
                                        else
                                        {
                                            lblNegocioCliente.Text = cliente.clc_nombre_comercial.ToString();
                                        }

                                        pkrEnvases.Items.Clear();

                                        StatusService statusService = new StatusService();
                                        EnvaseVM listaEnvaseVM = new EnvaseVM();

                                        // Manda llamar a la función que regresa la Lista de Envases a los cuales puede abonar el Cliente enviado como parámetro.
                                        statusService = listaEnvaseVM.FtnRegresarEnvasesPorClienteVM(iIdCliente);

                                        if (statusService.status == true)
                                        {
                                            if (statusService.listaStrings.Count >= 1)
                                            {
                                                // Agrega al Picker la Lista de Envases a los cuales puede abonar el Cliente.
                                                foreach (string promo in statusService.listaStrings)
                                                {
                                                    string sPromocion = promo.ToString().Trim();
                                                    pkrEnvases.Items.Add(sPromocion);
                                                }
                                            }

                                            btnVerResumen.IsEnabled = true;
                                        }
                                        else
                                        {
                                            await DisplayAlert("¡Atención!", statusService.mensaje, "OK");
                                        }
                                    }
                                    else
                                    {
                                        await DisplayAlert("Alerta", "Cliente tiene Devolución registrada", "OK");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("¡Atención!", sRespuesta, "OK");
                                    entIdCliente.Focus();
                                }
                            }
                        }
                    }
                }
            
            
        }

        /*Método para abrir la pantalla de RESUMEN DE ABONO DE ENVASE*/
        public async void OnClickedVerResumen(object sender, EventArgs args)
        {
            // Valida que se haya capturado el ID del Cliente.
            if (String.IsNullOrEmpty(entIdCliente.Text))
            {
                await DisplayAlert("Aviso", "Ha olvidado capturar el ID de Cliente.", "OK");
                entIdCliente.Focus();
            }
            else
            {
                // Valida que no se capture un punto en el ID del Cliente.
                if (entIdCliente.Text == ".")
                {
                    await DisplayAlert("Aviso", "Ha capturado un punto ( . ) en lugar de un ID de Cliente.", "OK");
                    entIdCliente.Focus();
                }
                else
                {
                    int iIdCliente;
                    // Valida que no se capture un número decimal en el ID del Cliente.
                    if (!int.TryParse(entIdCliente.Text, out iIdCliente))
                    {
                        await DisplayAlert("Aviso", "No es válido un decimal en el ID de Cliente.", "OK");
                        entIdCliente.Focus();
                    }
                    else
                    {
                        // Abre la pantalla de Resumen de Abono de Envase del Cliente enviado como parámetro.
                        await this.Navigation.PushModalAsync(new frmResumenAbonoEnvase(Convert.ToInt32(entIdCliente.Text), lblNombreCliente.Text));
                    }
                }
            }
        }

        /*Método para abrir la pantalla de MENÚ PRINCIPAL*/
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }

        /*Método para limpiar y darle el focus al campo de Cantidad*/
        private void pkrEnvases_SelectedIndexChanged(object sender, EventArgs e)
        {
            entCantidadEnvase.Text = "";
            //entCantidadEnvase.Focus();
        }
    }
}
