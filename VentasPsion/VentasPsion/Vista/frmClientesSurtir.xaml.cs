using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Base;


using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Xaml;
using System.Collections;
using Plugin.CurrentActivity;
using Android.Widget;
using Android.App;
using Android.Views;
using Newtonsoft.Json.Linq;
using Base;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmClientesSurtir : ContentPage
	{

        Formularios oFormulario;
        Activity oActivity = CrossCurrentActivity.Current.Activity;
        ArrayList oArrayFormulario = new ArrayList();
        ArrayList oArraySeleccionMultiple = new ArrayList();
        JArray oArrayJ = new JArray();
        //fnEncuestas oEncuestas = new fnEncuestas();
        clientes_requisitos_surtirVM vmDatosSurtir = new clientes_requisitos_surtirVM();
        Utilerias oUtilerias = new Utilerias();
        //private clientes vCliente;
        public int cln_clave;

        public frmClientesSurtir ()
		{
			InitializeComponent ();

            

            if (vmDatosSurtir.TraeDatosSurtir(VarEntorno.vCliente.cln_clave) == false)
                DisplayAlert("Alerta", "Error en datos del cliente", "OK");
            
                GenerarEncuesta();


        }


        public void GenerarEncuesta()
        {
            var oStack = new StackLayout { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.FillAndExpand };
            try
            {
                //this.vCliente = VarEntorno.vCliente;
                
                cln_clave = VarEntorno.vCliente.cln_clave;

                oFormulario = new Formularios();
                
                //var viewModel = new EncuestaVM();

                var oLabel = new Label() { Margin = 10,  Text= "Titular", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };                
                oStack.Children.Add(oLabel);
                //////   agregar hora de Titular 
                var oEntry = oFormulario.CrearEditor(true, true, null, 60,"1");
                oEntry.Text = vmDatosSurtir.cDatos.crc_titular;
                oStack.Children.Add(oEntry);
                oArrayFormulario.Add(oEntry);

                oLabel = new Label() { Margin = 10, Text = "Horario de Apertura", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                //////   agregar hora de Hora inicio 
                oEntry = oFormulario.CrearEditor(true, true, null, 60, "2");
                oEntry.Text = vmDatosSurtir.cDatos.crc_horario_apertura;
                oStack.Children.Add(oEntry);
                oArrayFormulario.Add(oEntry);

                oLabel = new Label() { Margin = 10, Text = "Horario de Cierre", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                //////   agregar hora de Hora fin 
                oEntry = oFormulario.CrearEditor(true, true, null, 60, "3");
                oEntry.Text = vmDatosSurtir.cDatos.crc_horario_cierre;
                oStack.Children.Add(oEntry);
                oArrayFormulario.Add(oEntry);

                oLabel = new Label() { Margin = 10, Text = "Horario de Sugerido", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                //////   agregar hora de Hora sugerido 
                oEntry = oFormulario.CrearEditor(true, true, null, 60, "4");
                oEntry.Text = vmDatosSurtir.cDatos.crc_horario_sugerido;
                oStack.Children.Add(oEntry);
                oArrayFormulario.Add(oEntry);

                oLabel = new Label() { Margin = 10, Text = "Requiere Factura", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                // Si/ No  
                var oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", "5");
                oSwitch.Checked = vmDatosSurtir.cDatos.crb_factura;
                oStack.Children.Add(oSwitch);
                oArrayFormulario.Add(oSwitch);

                oLabel = new Label() { Margin = 10, Text = "Realiza pago con tarjeta", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                // Si/ No  
                oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", "6");
                oSwitch.Checked = vmDatosSurtir.cDatos.crb_pago_tarjeta;
                oStack.Children.Add(oSwitch);
                oArrayFormulario.Add(oSwitch);

                oLabel = new Label() { Margin = 10, Text = "Requiere Chamuquito", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                // Si/ No  
                oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", "7");
                oSwitch.Checked = vmDatosSurtir.cDatos.crb_chamuco;
                oStack.Children.Add(oSwitch);
                oArrayFormulario.Add(oSwitch);

                oLabel = new Label() { Margin = 10, Text = "Tiene  Escaleras", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                // Si/ No  
                oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", "8");
                oSwitch.Checked = vmDatosSurtir.cDatos.crb_escaleras;
                oStack.Children.Add(oSwitch);
                oArrayFormulario.Add(oSwitch);

                oLabel = new Label() { Margin = 10, Text = "Tiene Rampa", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                // Si/ No  
                oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", "9");
                oSwitch.Checked = vmDatosSurtir.cDatos.crb_rampa;
                oStack.Children.Add(oSwitch);
                oArrayFormulario.Add(oSwitch);

                oLabel = new Label() { Margin = 10, Text = "Tiene Espacio Estrecho", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                // Si/ No  
                oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", "10");
                oSwitch.Checked = vmDatosSurtir.cDatos.crb_espacio_estrecho;
                oStack.Children.Add(oSwitch);
                oArrayFormulario.Add(oSwitch);

                oLabel = new Label() { Margin = 10, Text = "Peligro de Asalto", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                // Si/ No  
                oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", "11");
                oSwitch.Checked = vmDatosSurtir.cDatos.crb_asaltos;
                oStack.Children.Add(oSwitch);
                oArrayFormulario.Add(oSwitch);

                oLabel = new Label() { Margin = 10, Text = "Comentarios", FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes = FontAttributes.Bold, TextColor = Color.FromHex("#3a1310") };
                oStack.Children.Add(oLabel);
                //////   agregar comentario
                oEntry = oFormulario.CrearEditor(true, true, null, 60, "12");
                oEntry.Text = vmDatosSurtir.cDatos.crc_avisos;
                oStack.Children.Add(oEntry);
                oArrayFormulario.Add(oEntry);

                

                ////botones de fin 
                var oButtonTerminar = oFormulario.CrearButton(true, true, "TERMINAR", LayoutOptions.FillAndExpand, "ic_checked.png");
                oButtonTerminar.Clicked += OnClickTerminar;
                oStack.Children.Add(oButtonTerminar);
                
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", ex.Message, "OK");
            }
            finally
            {
                var oButtonRegresar = oFormulario.CrearButton(true, true, "REGRESAR", LayoutOptions.FillAndExpand, "ic_arrow_back.png");
                oButtonRegresar.Clicked += OnClickRegresar;
                oStack.Children.Add(oButtonRegresar);

                var scrollView = new Xamarin.Forms.ScrollView
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Content = oStack
                };
                Content = scrollView;
            }

        }

        private void OnClickRegresar(object sender, EventArgs e)
        {

            var oAlertDialog = oUtilerias.crearAlertDialog("Aviso", "¿Está seguro de salir?");
            oAlertDialog.Create();
            oAlertDialog.SetPositiveButton("SALIR", (senderAlert, args) =>
            {
                this.Navigation.PopModalAsync();
            });
            oAlertDialog.SetNegativeButton("CANCELAR", (senderAlert, args) =>
            {

            });
            oAlertDialog.Show();



        } // fin OnClickRegresar

        private void OnClickTerminar(object sender, EventArgs e)
        {
            try
            {
                
                string sMensaje = "";
                ActualizaDatos();

                if (vmDatosSurtir.cDatos.crc_horario_apertura.Length > 0 &&
                    vmDatosSurtir.cDatos.crc_horario_cierre.Length > 0 &&
                    vmDatosSurtir.cDatos.crc_horario_sugerido.Length > 0 &&
                    vmDatosSurtir.cDatos.crc_titular.Length > 0)
                {
                    if (vmDatosSurtir.GuardaDatosSurtir(cln_clave))
                        sMensaje = "Censo Terminado.";
                    else
                        sMensaje = VarEntorno.sMensajeError;

                    var oAlertDialog = oUtilerias.crearAlertDialog("Aviso", sMensaje);
                    oAlertDialog.Create();
                    oAlertDialog.SetPositiveButton("SALIR", (senderAlert, args) =>
                    {
                        this.Navigation.PopModalAsync();
                    });

                    oAlertDialog.Show();
                }
                else
                {
                    DisplayAlert("Alerta", "Debe completar los campos de Titular, Horario de apertura, cierre y sugerido ", "OK");
                }



            }
            catch //(Exception ex)
            {
               
            }
        } // fin OnClickTerminar

        public JArray serializarData()
        {
            int tipoRespuesta = 0, idPregunta = 0;
            var respuesta = "";

            if (oArrayJ.Count > 0)
            {
                oArrayJ.Clear();
            }

            for (int i = 0; i < oArrayFormulario.Count; i++)
            {
                var oTipoObjeto = oArrayFormulario[i].GetType();
                JObject oJObject = new JObject();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    idPregunta = Convert.ToInt32(oSender.ClassId.ToString());
                    tipoRespuesta = 1;
                    respuesta = oSender.Text;
                }
                else if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;
                    idPregunta = Convert.ToInt32(oSender.Tag.ToString());
                    tipoRespuesta = 2;
                    if (!oSender.Checked) 
                    {
                        respuesta = "false";
                    }
                    else
                    {
                        respuesta = "true";
                    }
                }
                

                oJObject.Add("cln_clave", cln_clave);
                oJObject.Add("tipoRespuesta", tipoRespuesta);
                oJObject.Add("idPregunta", idPregunta);
                oJObject.Add("respuesta", respuesta);
                oArrayJ.Add(oJObject);

            } // fin for

            return RemoveDuplicateTasks(oArrayJ);


        }

        public void CargaDatos()
        {
            try
            {
                

                int i = 0;                
                var oTipoObjeto = oArrayFormulario[i].GetType();                
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    oSender.Text= vmDatosSurtir.cDatos.crc_titular;
                }

                i = 1;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    oSender.Text = vmDatosSurtir.cDatos.crc_horario_apertura;
                }

                i = 2;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    oSender.Text = vmDatosSurtir.cDatos.crc_horario_cierre;
                }

                i = 3;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    oSender.Text = vmDatosSurtir.cDatos.crc_horario_sugerido;
                }

                i = 4;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;                    
                    
                    if (vmDatosSurtir.cDatos.crb_factura)                    
                        oSender.Checked = true;                    
                    else                    
                        oSender.Checked = true;                    
                }

                i = 5;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_pago_tarjeta)
                        oSender.Checked = true;
                    else
                        oSender.Checked = true;
                }

                i = 6;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_chamuco)
                        oSender.Checked = true;
                    else
                        oSender.Checked = true;
                }

                i = 7;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_escaleras)
                        oSender.Checked = true;
                    else
                        oSender.Checked = true;
                }

                i = 8;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_rampa)
                        oSender.Checked = true;
                    else
                        oSender.Checked = true;
                }

                i = 9;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_espacio_estrecho)
                        oSender.Checked = true;
                    else
                        oSender.Checked = true;
                }

                i = 10;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_asaltos)
                        oSender.Checked = true;
                    else
                        oSender.Checked = true;
                }

                i = 11;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    oSender.Text = vmDatosSurtir.cDatos.crc_avisos;
                }
            }
            catch
            {

            }
        }


        public void ActualizaDatos()
        {
            try
            {
                int i = 0;
                var oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    if (vmDatosSurtir.cDatos.crc_titular != oSender.Text.Trim())
                    {
                        vmDatosSurtir.cDatos.crc_titular = oSender.Text.Trim();
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 1;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    if (vmDatosSurtir.cDatos.crc_horario_apertura != oSender.Text)
                    {
                        vmDatosSurtir.cDatos.crc_horario_apertura = oSender.Text;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 2;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    if (vmDatosSurtir.cDatos.crc_horario_cierre != oSender.Text)
                    {
                        vmDatosSurtir.cDatos.crc_horario_cierre = oSender.Text;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 3;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    if (vmDatosSurtir.cDatos.crc_horario_sugerido != oSender.Text)
                    {
                        vmDatosSurtir.cDatos.crc_horario_sugerido = oSender.Text;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 4;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_factura != oSender.Checked)
                    {
                        vmDatosSurtir.cDatos.crb_factura = oSender.Checked;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 5;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_pago_tarjeta != oSender.Checked)
                    {
                        vmDatosSurtir.cDatos.crb_pago_tarjeta = oSender.Checked;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 6;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_chamuco != oSender.Checked)
                    {
                        vmDatosSurtir.cDatos.crb_chamuco = oSender.Checked;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 7;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_escaleras != oSender.Checked)
                    {
                        vmDatosSurtir.cDatos.crb_escaleras = oSender.Checked;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 8;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_rampa != oSender.Checked)
                    {
                        vmDatosSurtir.cDatos.crb_rampa = oSender.Checked;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 9;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_espacio_estrecho != oSender.Checked)
                    {
                        vmDatosSurtir.cDatos.crb_espacio_estrecho = oSender.Checked;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 10;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Android.Widget.Switch))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.Switch;

                    if (vmDatosSurtir.cDatos.crb_asaltos != oSender.Checked)
                    {
                        vmDatosSurtir.cDatos.crb_asaltos = oSender.Checked;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }

                i = 11;
                oTipoObjeto = oArrayFormulario[i].GetType();
                if (oTipoObjeto == typeof(Editor))
                {
                    var oSender = oArrayFormulario[i] as Editor;
                    if (vmDatosSurtir.cDatos.crc_avisos != oSender.Text)
                    { 
                        vmDatosSurtir.cDatos.crc_avisos = oSender.Text;
                        vmDatosSurtir.cDatos.crb_actualizado = true;
                    }
                }
            }
            catch
            {

            }
        }

        private static JArray RemoveDuplicateTasks(JArray strArray)
        {

            JArray uniqueArray = new JArray();
            StringBuilder jsonResponse = new StringBuilder();

            //Loop through each array and find for array value with a specific column name. (ex: ID)
            foreach (JObject jObject in strArray)
            {
                //Verify if the ID column value exist in the uniqueArray
                JObject rowObject = uniqueArray.Children<JObject>().FirstOrDefault(o => o["idPregunta"] != null && o["idPregunta"].ToString() == jObject.Property("idPregunta").Value.ToString());

                //rowObject will be null if these is no match for the value in ID column
                if (rowObject == null)
                {
                    uniqueArray.Add(jObject);
                }
            }

            //Remove the curly braces { }
            int strLength = uniqueArray.ToString().Length;
            string strValue = uniqueArray.ToString().Substring(1, (strLength - 2));
            jsonResponse.Append((jsonResponse.Length > 0 ? "," : "") + strValue);
            // return "@[" + jsonResponse.ToString() + "]";
            return uniqueArray;
        }


    }
}
