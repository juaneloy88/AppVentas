using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Servicio;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrmInicioTurno : ContentPage
    {
        public FrmInicioTurno()
        {
            InitializeComponent();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblHora.Text = DateTime.Now.ToString("hh:mm tt");
        }

        /*Método para abrir la pantalla de MENÚ PRINCIPAL*/
        public void OnClickedRegresar(object sender, EventArgs args)
        {
             this.Navigation.PopModalAsync();
           
        }

        /*Método para guardar los datos de Inicio de Turno (Fecha, Hora, Unidad y Kilometraje) de una Ruta específica*/
        public async void OnClickedGuardar(object sender, EventArgs args)
        {
            Utilerias utilerias = new Utilerias();

            if (String.IsNullOrEmpty(entUnidad.Text))
            {
                await DisplayAlert("Aviso", "Ha olvidado capturar la UNIDAD.", "OK");
                entUnidad.Focus();
            }
            else
            {
                if (entUnidad.Text == ".")
                {
                    await DisplayAlert("Aviso", "Ha capturado un punto ( . ) en lugar de una UNIDAD.", "OK");
                    entUnidad.Focus();
                }
                else
                {
                    int iNoUnidad;
                    if (!int.TryParse(entUnidad.Text, out iNoUnidad))
                    {
                        await DisplayAlert("Aviso", "No es válido un decimal en la UNIDAD.", "OK");
                        entUnidad.Focus();
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(entKilometraje.Text))
                        {
                            await DisplayAlert("Aviso", "Ha olvidado capturar el KILOMETRAJE.", "OK");
                            entKilometraje.Focus();
                        }
                        else
                        {
                            if (entKilometraje.Text == ".")
                            {
                                await DisplayAlert("Aviso", "Ha capturado un punto ( . ) en lugar del KILOMETRAJE.", "OK");
                                entKilometraje.Focus();
                            }
                            else
                            {
                                int iNoKm;
                                if (!int.TryParse(entKilometraje.Text, out iNoKm))
                                {
                                    await DisplayAlert("Aviso", "No es válido un decimal en el KILOMETRAJE.", "OK");
                                    entKilometraje.Focus();
                                }
                                else
                                {
                                    bool bRespuesta = await DisplayAlert("Pregunta", "¿Seguro que desea guardar el Inicio de Turno?", "Si", "No");

                                    if (bRespuesta == true)
                                    {
                                        Turno turno = new Turno();
                                        StatusService statusInicioTurno = new StatusService();

                                        statusInicioTurno = turno.FtnGuardarInicioTurno(VarEntorno.iNoRuta, iNoUnidad, iNoKm);

                                        if (statusInicioTurno.status == true)
                                        {
                                            utilerias.crearMensaje(statusInicioTurno.mensaje);
                                            //await DisplayAlert("Aviso", statusInicioTurno.mensaje, "OK");
                                            Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("InicioTurno", "1");
                                            VarEntorno.bInicioTurno = true;
                                            btnGuardar.IsEnabled = false;
                                        }
                                        else
                                        {
                                            await DisplayAlert("¡Atención!", statusInicioTurno.mensaje, "OK");
                                            VarEntorno.bInicioTurno = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
