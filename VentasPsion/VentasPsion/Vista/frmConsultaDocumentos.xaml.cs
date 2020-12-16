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
	public partial class frmConsultaDocumentos : ContentPage
	{
        DocumentosVM lstDocs = new DocumentosVM();

        public frmConsultaDocumentos ()
		{
			InitializeComponent ();
            infoapp();
            fillDoctos();
        }

        private void infoapp()
        {
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;
        }

        private void fillDoctos()
        {
            if (lstDocs.lDoctos())
            {
                int i = 0;
                foreach (var item in lstDocs.GetDoctActivos())
                {
                    pckDocumentos.Items.Add(
                        i++.ToString().PadLeft(2, '0') + " " +
                        item.vcn_importe.ToString("N").PadLeft(9, ' ') + " " +
                        item.dcn_saldo.ToString("N").PadLeft(9, ' ') + " " +
                        item.vcf_movimiento.ToString("dd/MM/yyyy")
                    );
                }
            }
        }

        private void pckDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var _docSelected = lstDocs.lDoctosCabecera[pckDocumentos.SelectedIndex];
            txtCliente.Text = _docSelected.vcn_cliente.ToString() + " " + VarEntorno.vCliente.clc_nombre?.ToString() ?? VarEntorno.vCliente.clc_nombre_comercial.ToString();
            txtdMov.Text = _docSelected.vcf_movimiento.ToString("dd/MM/yyy");
            txtImporte.Text = _docSelected.vcn_importe.ToString("N");
            txtruta.Text = VarEntorno.iNoRuta.ToString();
            txtticket.Text = _docSelected.vcn_folio;
            txtSaldo.Text = _docSelected.dcn_saldo.ToString("N");
        }

        private void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
    }
}