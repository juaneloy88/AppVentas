using Base;
using System;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmRelacionAnticipo : ContentPage
	{
        DocumentosVM Doctos = new DocumentosVM();
        AnticiposVM Anticipos = new AnticiposVM();
        Utilerias oUtilerias = new Utilerias();

        string sFolioCabecera = string.Empty;
        string sFolioAnticipo = string.Empty;
        decimal dImporte = 0;
        decimal dMontoPago = 0;
        int iTipoPantalla = 0;
        DateTime fMovimiento = DateTime.Now;

        public frmRelacionAnticipo (int iTipo)
		{
			InitializeComponent ();
            Infoapp();

            iTipoPantalla = iTipo;

            if (iTipoPantalla == 1)
            {
                CargaDocumentos();
                CargaAnticipos();

                if (VarEntorno.cCobranza is null)
                    VarEntorno.cCobranza = new CobranzaVM();
            }
            else
            {
                //CargaDocumentos();
                lblDocumentos.IsVisible = false;
                pckDocumentos.IsVisible = false;
                CargaAnticipos();

                if (VarEntorno.cAnticipos is null)
                    VarEntorno.cAnticipos = new AnticiposVM();
            }

            //VarEntorno.cCobranza = new CobranzaVM();
        }

        #region Infoapp
        private void Infoapp()
        {            
            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;
        }
        #endregion Infoapp

        #region Carga Documentos del cliente
        private void CargaDocumentos()
        {
            if (Doctos.lDoctos())
            {
                int i = 0;

                foreach (var Documentos in Doctos.lDoctosCabecera)
                {
                    pckDocumentos.Items.Add(
                        i++.ToString().PadLeft(2, '0') + " " +
                        String.Format("{0:N2}", Documentos.vcn_importe).PadLeft(9, ' ') + "  " +
                        String.Format("{0:N2}", Documentos.dcn_saldo).PadLeft(9, ' ') + "  " +
                        Documentos.vcf_movimiento.ToString("dd/MM/yyyy")
                    );
                }
            }
            else
                DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
        }
        #endregion Carga Documentos del cliente

        #region Carga Anticipos del Cliente
        private void CargaAnticipos()
        {
            if (Anticipos.lAnt())
            {
                int i = 0;

                foreach (var Anticipos in Anticipos.lAnticipos)
                {
                    pckAnticipos.Items.Add(
                        i++.ToString().PadLeft(2, '0') + " " +
                        Anticipos.vcn_folio.PadLeft(9, ' ') + " " +
                        String.Format("{0:N2}", Anticipos.ann_monto_pago).PadLeft(9, ' ') + " " +
                        Anticipos.vcf_movimiento.ToString("dd/MM/yyyy")
                        );
                }
            }
            else
                DisplayAlert("Aviso", VarEntorno.sMensajeError, "Ok");
        }
        #endregion Carga Anticipos del Cliente

        private void pckDocumentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pckDocumentos.SelectedIndex != -1)
            {
                var _docSelectedCab = Doctos.lDoctosCabecera[pckDocumentos.SelectedIndex];
                sFolioCabecera = _docSelectedCab.vcn_folio;
                dImporte = _docSelectedCab.vcn_importe;

                //if (iTipoPantalla == 2)
                {
                    VarEntorno.cCobranza.dAnticipo.vcn_cliente = _docSelectedCab.vcn_cliente;
                    VarEntorno.cCobranza.dAnticipo.vcn_folio_cabecera = _docSelectedCab.vcn_folio.ToString().PadLeft(6, '0');
                    VarEntorno.cCobranza.dAnticipo.vcf_movimiento_cabecera = _docSelectedCab.vcf_movimiento;
                    VarEntorno.cCobranza.dAnticipo.ddn_numero_pago = _docSelectedCab.dcn_numero_pago;
                }
                
            }                
        }

        private void pckAnticipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pckAnticipos.SelectedIndex != -1)
            {
                var _docSelectedAnt = Anticipos.lAnticipos[pckAnticipos.SelectedIndex];
                sFolioAnticipo = _docSelectedAnt.vcn_folio;
                dMontoPago = _docSelectedAnt.ann_monto_pago;
                fMovimiento = _docSelectedAnt.vcf_movimiento;

                //if (iTipoPantalla == 2)
                {
                    VarEntorno.cCobranza.dAnticipo.vcn_folio = _docSelectedAnt.vcn_folio.ToString().PadLeft(6, '0');
                    VarEntorno.cCobranza.dAnticipo.vcf_movimiento = _docSelectedAnt.vcf_movimiento;
                    VarEntorno.cCobranza.dAnticipo.vcn_monto_pago = _docSelectedAnt.ann_monto_pago;
                    VarEntorno.cCobranza.dAnticipo.vcc_cadena_original = "";
                    VarEntorno.cCobranza.dAnticipo.ddn_cantidad_pagos = 1;
                    VarEntorno.cCobranza.dAnticipo.ddc_forma_pago = _docSelectedAnt.anc_forma_pago;
                }
            }                
        }

        public void OnClickedGuardar(object sender, EventArgs args)
        {
            try
            {
                switch (iTipoPantalla)
                {
                    //Caso cuando la relación del anticipo se realiza desde la pantalla de "relación anticipos"
                    case 1:
                        if (String.IsNullOrEmpty(sFolioCabecera) || String.IsNullOrEmpty(sFolioAnticipo))
                        {
                            DisplayAlert("Error", "Seleccione un Documento y un Anticipo", "OK");
                        }
                        else
                        {
                            if (dImporte >= dMontoPago)
                            {
                                if (Anticipos.RelacionaAnticipo(sFolioAnticipo, sFolioCabecera))
                                {
                                    this.Navigation.PopModalAsync();
                                    oUtilerias.crearMensaje("¡Guardado exitoso!");
                                }
                                else
                                    DisplayAlert("Error", VarEntorno.sMensajeError, "OK");
                            }
                            else
                                DisplayAlert("Error", "El Anticipo no puede ser Mayor al Saldo", "OK");
                        }

                        break;
                    //Caso cuando la relación del anticipo se realiza desde la pantalla de "cobranza"
                    case 2:
                        if (String.IsNullOrEmpty(sFolioAnticipo))
                        {
                            DisplayAlert("Error", "Seleccione un Anticipo", "OK");
                        }
                        else
                        {
                            if (VarEntorno.dImporteTotal >= dMontoPago)
                            {
                                VarEntorno.cAnticipos.vcn_folio = sFolioAnticipo;
                                VarEntorno.cAnticipos.dMontoPago = dMontoPago;
                                VarEntorno.cAnticipos.anfMovimiento = fMovimiento;
                                VarEntorno.cCobranza.vCobranza.CargaPagos();
                                VarEntorno.bAnticipoRelacionado = true;
                                this.Navigation.PopModalAsync();
                            }                                
                            else
                                DisplayAlert("Error", "El Anticipo no puede ser Mayor al Saldo", "OK");
                        }
                        break;
                }                
                
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "OK");
            }

        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            if (iTipoPantalla == 2)
            {
                VarEntorno.cCobranza.dAnticipo = new Modelo.Entidad.documentos_detalle();
            }
                this.Navigation.PopModalAsync();
        }
    }
}