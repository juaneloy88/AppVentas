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
    public partial class frmResumenVenta : ContentPage
    {
        vmResumenVenta resumenVenta = new vmResumenVenta();
        vmCapturaEnvase vmEnvase = new vmCapturaEnvase();

        public frmResumenVenta()
        {
            InitializeComponent();            

            cargaLiquido();

            txtRuta.Text = "Ruta:" + VarEntorno.iNoRuta.ToString(); 
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

        #region Botón de Regresar
        public void onClikedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar
    }
}