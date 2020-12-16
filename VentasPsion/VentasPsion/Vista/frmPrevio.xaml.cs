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
	public partial class frmPrevio : ContentPage
	{
		public frmPrevio ()
		{
			InitializeComponent ();
		}


        public void OnClickedRegresar(object sender, EventArgs args)
        {
            VarEntorno.LimpiaVariables();
            this.Navigation.PopModalAsync();
        }

        private async void txtClave_Completed(object sender, EventArgs e)
        {
            //Se instancia la clase BuscarCliente
            //clientesSR buscacliente = new clientesSR();
            ClientesVM _cliente = new ClientesVM();

            //Se obtiene el ID del cliente capturado y se válida que no esta vacío
            string sIdCliente = txtClave.Text;

            if (string.IsNullOrWhiteSpace(sIdCliente))
            {
                await DisplayAlert("Campo Vacío", "Verifique el Id del Cliente", "Ok");
            }
            else
            {
                //Válida si el cliente existe en la BD
                string sRespuesta = _cliente.BuscaCliente(sIdCliente);//buscacliente.ValidaCliente(sIdCliente);

                //Respuesta
                if (sRespuesta == "Ok")
                {
                    //Busqueda de datos del cliente
                    var vListaCliente = await _cliente.DatosCliente(sIdCliente);
                    VarEntorno.vCliente = vListaCliente;

                    VarEntorno.dImporteTotal =  _cliente.VentaCliente(VarEntorno.vCliente.cln_clave);

                    lblNombre.Text = vListaCliente.clc_nombre;
                    lblSaldoAnt.Text = String.Format("{0:N2}", VarEntorno.Saldo(VarEntorno.vCliente));
                    lblPedido.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal);
                   
                    lblPagoPrev.Text = String.Format("{0:N2}", new DocumentosVM().Facturas.AbonoPreventa(VarEntorno.vCliente.cln_clave));
                    lblSaldoFin.Text = String.Format("{0:N2}", VarEntorno.vCliente.cln_saldo);

                    switch (VarEntorno.vCliente.clc_credito)
                    {
                        case "S":
                            lblTipoCredito.Text = "Credito";
                            lblTipoCredito.TextColor = Color.Green;
                            break;
                        case "N":
                            lblTipoCredito.Text = "No Tiene";
                            lblTipoCredito.TextColor = Color.Red;
                            break;
                        case "M":
                            lblTipoCredito.Text = "Saldo Ant";
                            lblTipoCredito.TextColor = Color.Blue;
                            break;
                    }

                    if (VarEntorno.vCliente.cln_cheque)
                    {
                        lblCheque.Text = "Si";
                        lblCheque.TextColor = Color.Green;
                    }
                    else
                    {
                        lblCheque.Text = "No";
                        lblCheque.TextColor = Color.Red;
                    }
                }
                else
                {
                    await DisplayAlert(sRespuesta, "Verifique el Id del Cliente", "Ok");
                }
            }
        }
    }
}
