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
	public partial class frmQuejasSugerencias : ContentPage
	{
        Utilerias oUtilerias;
        solicitudesSR oFnSolicitudes;
        Dictionary<string, int> dictionary =
           new Dictionary<string, int>();
        //private clientes vCliente;


        public frmQuejasSugerencias()//clientes vCliente)
        {
            //this.vCliente = vCliente;
            oUtilerias = new Utilerias();
            InitializeComponent();
            txtClaveCliente.Text = VarEntorno.vCliente.cln_clave.ToString();
            departamentosSR oDepartementos = new departamentosSR();
            var ListaDepartamentos = oDepartementos.obtenerDepartamentos();

            for(int i = 0; i < ListaDepartamentos.Count; i++)
            {
                dictionary.Add(ListaDepartamentos[i].dpc_descripcion, ListaDepartamentos[i].dpn_clave);
            }
            
            foreach (var oDiccionario in dictionary)
            {
                oPickerDepartamento.Items.Add(oDiccionario.Value.ToString().PadLeft(3,'0')+".-"+oDiccionario.Key);
            }
        }

        /*
        protected async override void OnResume()
        {
            txtClaveCliente.Text = VarEntorno.vCliente.ToString();
            await MainPage.Navigation.PopToRootAsync(true);
        }*/


        public void OnClickedBuscarCliente()
        {
            this.Navigation.PushModalAsync(new frmBuscarCliente(6));
        }

        public void OnClickedRegresar()
        {
            this.Navigation.PopModalAsync();
        }

        public void OnClickedGuardarSolicitud()
        {
            if (txtSolicituQueja.Text == null)
            {
                oUtilerias.crearMensaje("Debe escribir alguna queja o sugerencia.");
            }
            else if (oPickerDepartamento.SelectedIndex < 0)
            {
                oUtilerias.crearMensaje("Debe seleccionar el departamento.");
            }
            else
            {
                oFnSolicitudes = new solicitudesSR();                
                int dpn_clave = Convert.ToInt32( oPickerDepartamento.SelectedItem.ToString().Substring(0,3)); // id departamento seleccionado
                string soc_descripcion = txtSolicituQueja.Text; // texto queja o sugerencia
                int cln_clave = VarEntorno.vCliente.cln_clave;  // clave cliente seleccionado
                int run_clave = VarEntorno.iNoRuta; // ruta
                DateTime sod_fecha = DateTime.Now; // fecha
                if (oFnSolicitudes.GuardarSolicitud(dpn_clave, soc_descripcion, cln_clave, run_clave, sod_fecha))
                {
                    
                    this.Navigation.PopModalAsync();
                    oUtilerias.crearMensaje("Solicitud guardada");
                }
                else
                {
                    DisplayAlert("Aviso", "Error al guardar la solicitud favor de revisar ", "OK");
                }
            }
        }

    }
}