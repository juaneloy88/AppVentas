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
	public partial class frmResumenAutoVenta : ContentPage
	{
        #region Instancias de Clases
        vmResumenVenta resumenVenta = new vmResumenVenta();
        vmCapturaEnvase vmEnvase = new vmCapturaEnvase();
        #endregion Instancias de Clases

        public frmResumenAutoVenta ()
		{
			InitializeComponent ();

            #region Métodos para cargar Líquido y envase
            cargaLiquido();
            cargaEnvase();
            #endregion Métodos para cargar Líquido y envase
        }

        #region Proceso de consulta de liquido
        public async void cargaLiquido()
        {
            decimal dImporte = 0;
            int iVenta = 0;
            var vResumenVentaLiquido = await resumenVenta.cargaLiquido();

            if (vResumenVentaLiquido.Count >= 1)
            {
                lsvClave.ItemsSource = vResumenVentaLiquido;

                foreach (var v in vResumenVentaLiquido)
                {
                    dImporte = dImporte + v.vdn_importe;
                    iVenta = iVenta + v.vdn_venta;
                }

                lblTotal.Text = String.Format("{0:C2}", dImporte);
                lblCartones.Text = String.Format("{0:0}", iVenta);
            }
            else
            {
                Utilerias oUtilerias = new Utilerias();
                oUtilerias.crearMensaje("No existen Registros de Venta");
            }
        }
        #endregion Proceso de consulta de liquido

        #region Proceso de consulta de envase
        public async void cargaEnvase()
        {
            int iCliente = VarEntorno.vCliente.cln_clave;
            var vResumenEnvase = await vmEnvase.detEnvaseCliente(iCliente.ToString());

            if (vResumenEnvase.Count >= 1)
            {
                lsvEnvase.ItemsSource = vResumenEnvase;
            }
        }
        #endregion Proceso de consulta de envase

        #region Botón de Regresar
        public void onClikedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar
    }
}