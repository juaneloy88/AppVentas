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
	public partial class frmCobranzaBonificacion : ContentPage
    {
        vwBonificaciones vwBonis = new vwBonificaciones();
        int nCliente = VarEntorno.vCliente.cln_clave;
        decimal dTotal = 0;

        public frmCobranzaBonificacion()
        {
            InitializeComponent();
            txtRuta.Text = "Ruta:" + VarEntorno.iNoRuta.ToString();
        }

        #region Botón de Regresar
        private void OnClickedRegresar(object sender, EventArgs args)
        {
            // VarEntorno.cCobranza.lBonificacion.Clear();
            VarEntorno.cCobranza.lpBonificacion.Clear();
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar        

        #region Busqueda del número de bonificación
        private async void Entry_Completed_txtNoBonificacion(object sender, EventArgs e)
        {
            #region Declaración de Variables
            string sTipo = string.Empty;
            string sImporte = string.Empty;
            string sNoBoni = txtNoBonificacion.Text;
            #endregion Declaración de Variables

            //if (VarEntorno.cTipoVenta =='P' && VarEntorno.bEsTeleventa)
            { }
            //else
            {
                if (string.IsNullOrWhiteSpace(sNoBoni))
                {
                    await DisplayAlert("Alert", "Favor de ingresar un Número de Bonificación", "OK");
                }
                else
                {
                    var vLista = await vwBonis.BonificacionesCliente(sNoBoni, nCliente.ToString());

                    if (vLista.Count >= 1)
                    {
                        foreach (var bonis in vLista)
                        {
                            sTipo = bonis.boc_tipo.ToString();
                            sImporte = bonis.boi_documento.ToString();
                        }

                        switch (sTipo)
                        {
                            case "C":
                                sTipo = "Cerveza";
                                break;
                            case "L":
                                sTipo = "Licencia";
                                break;
                            case "Z":
                                sTipo = "Mensual";
                                break;
                            case "R":
                                sTipo = "Refresco";
                                break;
                            case "K":
                                sTipo = "Kermato";
                                break;
                            case "E":
                                sTipo = "Envase";
                                break;
                        }

                        txtTipo.Text = sTipo;
                        txtImporte.Text = String.Format("{0:0.00}", sImporte);

                    }
                    else
                    {
                        await DisplayAlert("Alert", "NO se encontro ningún Doc con ese Número de Bonificación", "OK");
                    }
                }
            }
        }
        #endregion Busqueda del número de bonificación

        #region Método que inserta la Boni en la lista y tambien para su uso posterior
        private async void OnClickedAplicar(object sender, EventArgs args)
        {
            string sNoBoni = txtNoBonificacion.Text;
            

            if (string.IsNullOrWhiteSpace(sNoBoni))
            {
                await DisplayAlert("Alert", "Favor de ingresar un Número de Bonificación", "OK");
            }
            else
            {
                string sFolio = txtNoBonificacion.Text.Trim();
                string sTipo = txtTipo.Text;

                if (string.IsNullOrWhiteSpace(sTipo))
                {
                    await DisplayAlert("Alert", "Capture el Número de Bonificación y presiona ENTER", "OK");
                }
                else
                {
                    decimal dImporte = Convert.ToDecimal(txtImporte.Text);

                    //Validación de si la Boni ya se encuentra aplicada
                    string sRespuesta = await vwBonis.validaSiEstaAplicadaBoni(sFolio);

                    if (sRespuesta == "Ya se encuentra aplicada la Bonificación")
                    {
                        await DisplayAlert("Alert", sRespuesta, "OK");
                    }
                    else
                    {
                        if (sRespuesta == "Ok")
                        {
                            //Válida si la bonificación ya existe en la lista
                            //var vValida = VarEntorno.cCobranza.lBonificacion.Find(x => x.boc_folio.Contains(sNoBoni));
                            var vValida = VarEntorno.cCobranza.lpBonificacion.Find(x => x.vcc_referencia.Contains(sNoBoni));
                            if (vValida != null)
                            {
                                await DisplayAlert("Alert", "Hoy ya capturaste esta Bonificación", "OK");

                                txtNoBonificacion.Text = "";
                                txtTipo.Text = "";
                                txtImporte.Text = "";
                            }
                            else
                            {
                                //Se Agrega la boni a la lista para ser mostrada
                                var vBonis = await vwBonis.listaBonificaciones(sFolio, sTipo, dImporte);

                                if (vBonis.Count >= 1)
                                {
                                    lsvDocumentos.ItemsSource = vBonis;                                    
                                    
                                    dTotal = dTotal + dImporte;

                                    bool bResult = false;
                                    /*
                                    Bonificacion boni = new Bonificacion();
                                    boni.boc_folio = sFolio;
                                    boni.boi_documento = dImporte;
                                    boni.boc_tipo = sTipo;
                                        */
                                    //VarEntorno.cCobranza.lBonificacion.Add(boni);
                                    /*
                                    venta_pagos cPago = new venta_pagos();
                                    cPago.vpc_descripcion = "BONIFICACION";
                                    cPago.vcn_monto = Convert.ToDecimal(txtImporte.Text);
                                    cPago.vcf_movimiento = DateTime.Now.ToShortDateString();
                                    cPago.cfpc_formapago = "27";
                                    cPago.vcn_cliente = VarEntorno.vCliente.cln_clave;
                                    cPago.vcn_folio = VarEntorno.iFolio.ToString().PadLeft(6, '0');
                                   // cPago.vpn_numpago = VarEntorno.cCobranza.iNPagos;
                                    cPago.vcc_referencia = txtNoBonificacion.Text;
                                    */
                                    //VarEntorno.cCobranza.lPagos.Add(cPago);
                                    //VarEntorno.cCobranza.lpBonificacion.Add(cPago);

                                    bResult=VarEntorno.cCobranza.GuardaActualizaPago("BONIFICACION", true, -1, "27", "", "", Convert.ToDecimal(txtImporte.Text)
                                                                            , txtNoBonificacion.Text, "", "");

                                    //VarEntorno.cCobranza.iNPagos++;
                                    if (bResult)
                                        await DisplayAlert("Alert", "Bonificación Capturada Correctamente", "OK");
                                    else
                                        await DisplayAlert("Alert", "No se guardo la bonificacion", "OK");

                                    txtTotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(dTotal));

                                    txtNoBonificacion.Text = "";
                                    txtTipo.Text = "";
                                    txtImporte.Text = "";
                                }
                                else
                                {
                                    await DisplayAlert("Alert", "Error al cargar la lista", "OK");
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion Método que inserta la Boni en la lista y tambien para su uso posterior

        #region Botón de Aceptar
        private async void OnClickedAceptar(object sender, EventArgs args)
        {  
            await this.Navigation.PopModalAsync();
            if (VarEntorno.bEsDocumentos)
                VarEntorno.cCobranza.vCobranzaPagos.CargaPagos();
            else
                VarEntorno.cCobranza.vCobranza.CargaPagos();

        }
        #endregion Botón de Aceptar
    }
}