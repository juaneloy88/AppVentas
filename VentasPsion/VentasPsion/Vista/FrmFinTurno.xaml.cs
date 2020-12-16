﻿using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FrmFinTurno : ContentPage
    {
        public FrmFinTurno()
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

        /*Método para guardar los datos de Fin de Turno (Fecha, Hora y Kilometraje) de una Ruta específica*/
        public async void OnClickedGuardar(object sender, EventArgs args)
        {
            Utilerias utilerias = new Utilerias();
            bool bClientesVisitados = false;

            //Validación de que todos los clientes sean visitados
            vmStatusClientes vmstatusClientes = new vmStatusClientes();
            List<clientes_estatus> vLista = await vmstatusClientes.obtieneClientesSinVisita();

            //Verifica la bandera de visitar todos los clientes
            if (VarEntorno.cTipoVenta == 'R')
            {   
                if (VarEntorno.bVisitaAllCtsReparto)
                {
                    if (vLista.Count >= 1)
                    {
                        //await DisplayAlert("Aviso", "Faltan Clientes por Visitar", "OK");
                        bClientesVisitados = false;
                    }
                    else
                    {
                        bClientesVisitados = true;
                    }
                }
                else
                {
                    bClientesVisitados = true;
                }
            }
            else
            {
                
                if (VarEntorno.bVisitaAllCtsReparto)
                {
                    if (vLista.Count >= 1)
                    {
                        //await DisplayAlert("Aviso", "Faltan Clientes por Visitar", "OK");
                        bClientesVisitados = false;
                    }
                    else
                    {
                        bClientesVisitados = true;
                    }
                }
                else                
                {
                    bClientesVisitados = true;
                }
            }

            if (bClientesVisitados == false)
            {
                await DisplayAlert("Aviso", "Faltan Clientes por Visitar", "OK");
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
                            bool bRespuesta = await DisplayAlert("Pregunta", "¿Seguro que desea guardar el Fin de Turno?", "Si", "No");

                            if (bRespuesta == true)
                            {
                                Turno turno = new Turno();
                                StatusService statusFinTurno = new StatusService();

                                statusFinTurno = turno.FtnGuardarFinTurno(VarEntorno.iNoRuta, iNoKm);

                                if (statusFinTurno.status == true)
                                {
                                    utilerias.crearMensaje(statusFinTurno.mensaje);
                                    //await DisplayAlert("Aviso", statusFinTurno.mensaje, "OK");
                                    Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("FinTurno", "1");
                                    VarEntorno.bFinTurno = true;
                                    btnGuardar.IsEnabled = false;
                                }
                                else
                                {
                                    await DisplayAlert("¡Atención!", statusFinTurno.mensaje, "OK");
                                    VarEntorno.bFinTurno = false;
                                    entKilometraje.Focus();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
