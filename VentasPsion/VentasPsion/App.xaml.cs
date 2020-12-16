using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using VentasPsion.Modelo;
using VentasPsion.Modelo.Entidad;
using VentasPsion.VistaModelo;

namespace VentasPsion
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            #region "Setea la variables estáticas que serán usadas en todas las pantallas"
            VarEntorno.sVersionApp = "Versión 1.3.9";
            VarEntorno.dFechaVenta = DateTime.Now;

            DayOfWeek diaSemana = DateTime.Now.DayOfWeek;
            int iDiaSemana = (int)diaSemana;

            switch (iDiaSemana)
            {
                case 0:
                    VarEntorno.cDiaVisita = 'D';
                    break;
                case 1:
                    VarEntorno.cDiaVisita = 'L';
                    break;
                case 2:
                    VarEntorno.cDiaVisita = 'M';
                    break;
                case 3:
                    VarEntorno.cDiaVisita = 'R';
                    break;
                case 4:
                    VarEntorno.cDiaVisita = 'J';
                    break;
                case 5:
                    VarEntorno.cDiaVisita = 'V';
                    break;
                case 6:
                    VarEntorno.cDiaVisita = 'S';
                    break;
                default:
                    break;
            }

            //VarEntorno.sTipoImpresora = "Zebra";
            #endregion


            string Fecha = Plugin.Settings.CrossSettings.Current
            .GetValueOrDefault<string>("Fecha", "");

            

            if (Fecha == DateTime.Now.ToShortDateString())
            {
                //  MainPage = new NavigationPage(new VentasPsion.Vista.frmLogin());
                MainPage = new NavigationPage(new VentasPsion.Vista.FrmSeleccionPerfil(true));
                // MainPage = new VentasPsion.Vista.frmLogin();

            }
             else
            {                
                
                //  MainPage = new VentasPsion.Vista.FrmSeleccionPerfil();
                MainPage = new NavigationPage(new VentasPsion.Vista.FrmSeleccionPerfil(false));

                new CreaDB();
            }

            

            
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
        }
	}
}
