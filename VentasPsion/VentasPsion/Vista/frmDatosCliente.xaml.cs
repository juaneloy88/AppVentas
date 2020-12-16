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
	public partial class frmDatosCliente : ContentPage
	{
        TelefonosVM tel = new TelefonosVM();
        

        public frmDatosCliente ()
		{
			InitializeComponent ();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblVersionApp.Text = VarEntorno.sVersionApp;

            CargaDatosCliente();

        }

        #region Método que obtiene los datos del cliente
        public void CargaDatosCliente()
        {
            var vDatosCliente = VarEntorno.vCliente;//await new clientesSR().DatosCliente(VarEntorno.vCliente.cln_clave.ToString());

            txtClave.Text = vDatosCliente.cln_clave.ToString();
            txtNombreComercial.Text = vDatosCliente.clc_nombre_comercial;
            txtNombre.Text = vDatosCliente.clc_nombre;
            txtDomicilio.Text = vDatosCliente.clc_domicilio;
            txtCP.Text = vDatosCliente.cln_codigo;
            txtRfc.Text = vDatosCliente.clc_rfc;
            txtDomicilioCliente.Text = vDatosCliente.domiciliocliente;

            if (tel.ListaTelefonos())
            {
                foreach (var registro in tel.Telefonos)
                    pckTelefonos.Items.Add(registro.tcc_nombre+" "+registro.tcc_telefono+" "+registro.tcc_comentario);
            }
            else
                DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
        }
        #endregion Método que obtiene los datos del cliente

        #region Botón de Regresar
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar

        public void OnClickedMas(object sender, EventArgs args)
        {

            if (VarEntorno.cTipoVenta == 'R')
            {
                this.Navigation.PushModalAsync(new frmClientesSurtir());
            }
            else
                this.Navigation.PushModalAsync(new frmActualizacionDatosClientes());
        }

        private void SelectedIndexChanged(object sender, EventArgs args)
        {
            try
            {
                Device.OpenUri(new Uri("tel:"+tel.Telefonos[pckTelefonos.SelectedIndex].tcc_telefono));

            }
            catch
            { }
        }
    }
}