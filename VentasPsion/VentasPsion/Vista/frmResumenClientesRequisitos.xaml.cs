using Android.App;
using Android.Content;
using Android.Text;
using Android.Widget;
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
using static Android.App.AlertDialog;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmResumenClientesRequisitos : ContentPage
	{
        List<clientes_requisitos> Clientes = new List<clientes_requisitos>();
        List<clientes> _pendientes = new List<clientes>();

        public frmResumenClientesRequisitos ()
		{
			InitializeComponent ();
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;
            GetEncuesta();
        }


        private void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }

        private void OnClickTarjeta(object sender, EventArgs args)
        {
            List<clientes_requisitos> _tarjeta = Clientes?.Where(e => e.tipo == 0).ToList() ?? null;
            buildAlert(_tarjeta, "con terminal");
        }

        private void OnClickClientes(object sender, EventArgs args)
        {
            List<clientes_requisitos> _factura = Clientes?.Where(e => e.tipo == 1).ToList() ?? null;
            buildAlert(_factura, "con factura");
        }

        private void OnClickedPendientes(object sender, EventArgs args)
        {
            Utilerias oUtilerias = new Utilerias();
            string _alert = "<ul style='overflow-x: scroll'>";

            if (_pendientes != null)
            {
                if (_pendientes.Count() > 0)
                {
                    foreach (var item in _pendientes)
                    {
                        _alert += "<li>" + item.cln_clave + " - " + item.clc_nombre_comercial + "</li></hr>";
                    }
                }
            }
            else
            {
                _alert = "<li><i class='&#xf071;'><i> No hay clientes pendientes </li>";
            }
            _alert += "</ul>";

            var oDialogoCargando = oUtilerias.crearAlertDialog("Clientes Pendientes", "");
            oDialogoCargando.SetMessage(Html.FromHtml(_alert));
            oDialogoCargando.SetCancelable(true);
            oDialogoCargando.SetNegativeButton("OK", (c, ev) =>
            {
                oDialogoCargando.Dispose();
            });
            oDialogoCargando.Show();
        }

        private void buildAlert(List<clientes_requisitos> _Clientes, string type)
        {
            Utilerias oUtilerias = new Utilerias();
            string clientes = "<ul style='overflow-x: scroll'>";


            if (_Clientes != null)
            {
                if (_Clientes.Count() > 0)
                {
                    foreach (var item in _Clientes)
                    {
                        clientes = clientes + "<li>" + item.cln_clave + "-" + item.crc_titular + "</li></hr>";
                    }
                }
            }
            else
            {
                clientes =  "<li><i class='fa fa-ban'><i> No hay clientes  " + type + "</li>";
            }
            clientes = clientes + "</ul>";


            var oDialogoCargando = oUtilerias.crearAlertDialog("Clientes", "");
            oDialogoCargando.SetMessage(Html.FromHtml(clientes));
            oDialogoCargando.SetCancelable(true);
            oDialogoCargando.SetNegativeButton("OK", (c, ev) =>
            {
                oDialogoCargando.Dispose();
            });
            oDialogoCargando.Show();
        }

        public void GetEncuesta()
        {
            clientes_requisitos_surtirVM VMClientes = new clientes_requisitos_surtirVM();
            var vDatosReqs = VMClientes.ClientesReqs();


            lblFactn.Text = vDatosReqs.crb_factura.ToString();
            lblChan.Text = vDatosReqs.crb_chamuco.ToString();
            lblEscn.Text = vDatosReqs.crb_escaleras.ToString();
            lblRampn.Text = vDatosReqs.crb_rampa.ToString();
            lblEspn.Text = vDatosReqs.crb_espacio_estrecho.ToString();
            lblAsalton.Text = vDatosReqs.crb_asaltos.ToString();
            lblTer.Text = vDatosReqs.crb_pago_tarjeta.ToString();

            if (vDatosReqs.crb_factura > 0)
                lblFactrue.IsVisible = true;
            else
                lblFacfalse.IsVisible = true;

            if (vDatosReqs.crb_pago_tarjeta > 0)
                lblTertrue.IsVisible = true;
            else
                lblTerfalse.IsVisible = true;

            if (vDatosReqs.crb_chamuco > 0)
                lblChantrue.IsVisible = true;
            else
                lblChanfalse.IsVisible = true;

            if (vDatosReqs.crb_escaleras > 0)
                lblEsctrue.IsVisible = true;
            else
                lblEscfalse.IsVisible = true;

            if (vDatosReqs.crb_rampa > 0)
                lblRamptrue.IsVisible = true;
            else
                lblRampfalse.IsVisible = true;

            if (vDatosReqs.crb_espacio_estrecho > 0)
                lblEsptrue.IsVisible = true;
            else
                lblEspfalse.IsVisible = true;

            if (vDatosReqs.crb_asaltos > 0)
                lblAsaltotrue.IsVisible = true;
            else
                lblAsaltofalse.IsVisible = true;

            Clientes = vDatosReqs.ListaClientes;
            _pendientes = vDatosReqs.pendientes;
        }
    }
}
