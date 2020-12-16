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
	public partial class frmCobranzaEfectivo : ContentPage
	{
        int iIndex = -1;
        //venta_pagos cPago = null;
        DocumentosVM Docu = new DocumentosVM();

        public frmCobranzaEfectivo ()
		{
			InitializeComponent ();
            
            if (VarEntorno.cCobranza.pEfectivo.vcn_monto > 0)
                txtCantidad.Text = VarEntorno.cCobranza.pEfectivo.vcn_monto.ToString();

            lblImporte.IsVisible = true;
            lblNimporte.IsVisible = true;

            if (VarEntorno.bSoloCobrar)
            {
                lblNimporte.Text = String.Format("{0:N2}", VarEntorno.dEfectivo);
            }
            else
            {
                if (VarEntorno.cTipoVenta=='R')
                    lblNimporte.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal
                                                                - Docu.Facturas.AbonoPreventa(VarEntorno.vCliente.cln_clave));
                else
                    lblNimporte.Text = String.Format("{0:N2}", VarEntorno.dImporteTotal);                
            }
        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            //  manda la pantalla al fondo de la pila
            this.Navigation.PopModalAsync();
        }

        public void OnClickedAceptar(object sender, EventArgs args)
        {
            try
            {
                string sRespuesta = string.Empty;
                if (sRespuesta != txtCantidad.Text)
                {
                    if (Convert.ToDecimal(txtCantidad.Text) < 0M)
                    {
                        DisplayAlert("Aviso", "No se pueden capturar cantidades negativas", "Ok");
                    }
                    else
                    {                        
                            //VarEntorno.cCobranza.dEfectivo = Convert.ToDecimal(txtCantidad.Text);
                            VarEntorno.cCobranza.pEfectivo.vcn_monto = Convert.ToDecimal(txtCantidad.Text);

                            if (VarEntorno.bEsDocumentos)
                                VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
                            else                                
                                VarEntorno.cCobranza.vCobranza.CargaPagos();

                            //  manda la pantalla al fondo de la pila
                            this.Navigation.PopModalAsync();
                        
                    }
                }
                else
                    DisplayAlert("Aviso", "Favor de ingresar una cantidad", "Ok");
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", "Error: " + ex.ToString(), "Ok");
            }
        }

    }
}
