using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Base;

using VentasPsion.VistaModelo;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmUtilerias : ContentPage
	{
        string sUriConexionEnvio = "http://192.168.2.23/PublishWebApi/api/";
        bool bClientes;
        bool bTipoBD;
        bool bOperaciones;
       // int iRuta;

        public frmUtilerias ()
		{
			InitializeComponent ();

            txtPerfil.Text = VarEntorno.sTipoVenta;

            #region Inicializa el switch dependiendo del valor de la Uri de Conexión de envío
            if (VarEntorno.sUriConexionEnvio == "http://192.168.2.14/PublishWebApi/api/")
            {
                sSocketEnvio.IsToggled = true;
            }
            else
            {
                sSocketEnvio.IsToggled = false;
            }
            #endregion Inicializa el switch dependiendo del valor de la Uri de Conexión de envío

            #region Inicializa el switch dependiendo del valor de visitar todos los clientes de reparto
            if (VarEntorno.bVisitaAllCtsReparto)
            {
                sVistaClientesReparto.IsToggled = true;
            }
            else
            {
                sVistaClientesReparto.IsToggled = false;
            }
            #endregion Inicializa el switch dependiendo del valor de visitar todos los clientes de reparto

            #region Inicializa el switch dependiendo del valor del tipo de BD a utilizar en la transmisión
            if (VarEntorno.bTipoBaseDatos)
            {
                sTipoBD.IsToggled = true;
            }
            else
            {
                sTipoBD.IsToggled = false;
            }
            #endregion Inicializa el switch dependiendo del valor del tipo de BD a utilizar en la transmisión

            #region Inicializa el switch dependiendo del valor de mostrar menu de Operaciones
            if (VarEntorno.bOperaciones)
            {

                sValidarOperaciones.IsToggled = true;
            }
            else
            {
                sValidarOperaciones.IsToggled = false;
            }
            #endregion Inicializa el switch dependiendo del valor de mostrar menu de Operaciones

           //if (VarEntorno.cTipoVenta == 'R')
                sVistaClientesReparto.IsEnabled = true;
           // txtRuta.Text = VarEntorno.iNoRuta.ToString();
        }

        #region Botón de Regresar
        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
        #endregion Botón de Regresar

        #region Establece el valor de la Uri de Conexión
        public  void OnClickSwitchToggledPruebas(object sender, ToggledEventArgs args)
        {
            if (args.Value)
            {
                sUriConexionEnvio = "http://192.168.2.14/PublishWebApi/api/";
            }
            else
            {
                sUriConexionEnvio = "http://192.168.2.23/PublishWebApi/api/";
            }
        }
        #endregion Establece el valor de la Uri de Conexión

        #region Establece el valor de la bandera de visitar todos los clientes en Reparto
        public  void OnClickSwitchToggledClientes(object sender, ToggledEventArgs args)
        {
            if (args.Value)
            {
                bClientes = true;                
            }
            else
            {
                bClientes = false;                
            }
        }
        #endregion Establece el valor de la bandera de visitar todos los clientes en Reparto

        #region Establece en que BD se va a utilizar al momento de Transmitir información
        public void OnClickSwitchToggledBD(object sender, ToggledEventArgs args)
        {
            if (args.Value)
            {
                bTipoBD = true;
            }
            else
            {
                bTipoBD = false;
            }
        }
        #endregion Establece en que BD se va a utilizar al momento de Transmitir información

        #region Método para Guardar los cambios realizados
        public void OnClickedCambiarUri(object sender, EventArgs args)
        {
            VarEntorno.sUriConexionEnvio = sUriConexionEnvio;
            VarEntorno.bVisitaAllCtsReparto = bClientes;
            VarEntorno.bTipoBaseDatos = bTipoBD;
            VarEntorno.bOperaciones = bOperaciones;
            //if (iRuta!=0)
            //VarEntorno.iNoRuta = iRuta;

            Utilerias oUtilerias = new Utilerias();
            oUtilerias.crearMensaje("Cambios Guardados");

            this.Navigation.PopModalAsync();
        }
        #endregion Método para Guardar los cambios realizados

        public void OnClickSwitchToggledOperaciones(object sender, ToggledEventArgs args)
        {
            if (args.Value)
            {
                bOperaciones = true;
            }
            else
            {
                bOperaciones = false;
            }
        }

        //public void OnClickSwitchToggledRuta(object sender, ToggledEventArgs args)
        //{
        //    if (args.Value)
        //    {
        //        // bOperaciones = true;
        //        iRuta = Convert.ToInt32(txtRuta.Text);
        //    }
        //    else
        //    {
        //       // bOperaciones = false;
        //    }
        //}
    }
}