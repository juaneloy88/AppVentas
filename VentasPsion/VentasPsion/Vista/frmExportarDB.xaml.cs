using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.ServicioApi;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmExportarDB : ContentPage
	{
        TransmitirRestService transmitir = new TransmitirRestService();

        public frmExportarDB ()
		{
			InitializeComponent ();

            txtRuta.Text = "Ruta:" + VarEntorno.iNoRuta.ToString();
        }

        #region Botón de Enviar
        public async void OnClickedEnviar(object sender, EventArgs args)
        {
            string sRespuesta = string.Empty;
            string sCorreo = txtCorreo.Text;

            if (string.IsNullOrWhiteSpace(sCorreo))
            {
                await DisplayAlert("Correo Vacío", "Verifique", "Ok");
            }
            else
            {
                Utilerias utilerias = new Utilerias();
                var progressDialogRecepcion = utilerias.crearProgressDialog("Exportando DB", "Enviando archivos JSON...");
                progressDialogRecepcion.Show();                

                sRespuesta = await transmitir.EnviarVentaCabeceraJson(sCorreo);
                sRespuesta = await transmitir.EnviarVentaPagosJson(sCorreo);
                sRespuesta = await transmitir.EnviarVentaDetalleJson(sCorreo);
                sRespuesta = await transmitir.EnviarEnvaseJson(sCorreo);
                sRespuesta = await transmitir.EnviarBonificacionesJson(sCorreo);
                sRespuesta = await transmitir.EnviarClientesEstatusJson(sCorreo);
                sRespuesta = await transmitir.EnviarDevolucionesJson(sCorreo);
                sRespuesta = await transmitir.EnviarRespuestasJson(sCorreo);
                sRespuesta = await transmitir.EnviarSolicitudesJson(sCorreo);
                sRespuesta = await transmitir.EnviarInfoRutaJson(sCorreo);
                sRespuesta = await transmitir.EnviarGpsJson(sCorreo);
                sRespuesta = await transmitir.EnviarPagosProgramadosJson(sCorreo);
                sRespuesta = await transmitir.EnviarEmpleadosJson(sCorreo);
                sRespuesta = await transmitir.EnviarPagareClientesJson(sCorreo);
                sRespuesta = await transmitir.EnviarEnvaseSugeridoJson(sCorreo);
                sRespuesta = await transmitir.EnviarDocumentosCabeceraJson(sCorreo);
                sRespuesta = await transmitir.EnviarDocumentosDetalleJson(sCorreo);
                sRespuesta = await transmitir.EnviarClientesDatosSurtirJson(sCorreo);
                sRespuesta = await transmitir.EnviarClientesCompetenciaJson(sCorreo);
                sRespuesta = await transmitir.EnviarAnticiposJson(sCorreo);
                sRespuesta = await transmitir.EnviarTelefonosClientesJson(sCorreo);

                progressDialogRecepcion.Dismiss();

                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Aviso", "Fin del Envío", "OK");
                });
            }            
        }
        #endregion Botón de Enviar

        #region Botón de Regresar
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar
    }
}