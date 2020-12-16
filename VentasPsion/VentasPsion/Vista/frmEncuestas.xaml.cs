using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.Base;
using VentasPsion.Modelo.Entidad;
using VentasPsion.Modelo.Servicio;
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
    public partial class frmEncuestas : ContentPage
    {
        Formularios oFormulario;
        Activity oActivity = CrossCurrentActivity.Current.Activity;
        ArrayList oArrayFormulario = new ArrayList();
        ArrayList oArraySeleccionMultiple = new ArrayList();
        JArray oArrayJ = new JArray();
        encuestasSR oEncuestas = new encuestasSR();
        Utilerias oUtilerias = new Utilerias();
       // private clientes vCliente;
        public int cln_clave;

        public frmEncuestas() // constructor
        {
            //this.vCliente = vCliente;
            InitializeComponent();

            cln_clave = VarEntorno.vCliente.cln_clave;//this.vCliente.cln_clave;
             
            oFormulario = new Formularios();
            var oStack = new StackLayout { Orientation = StackOrientation.Vertical , HorizontalOptions = LayoutOptions.FillAndExpand };  
            var viewModel = new EncuestaVM();
            foreach (var oModel in viewModel.oListaFormularioEncuesta)
            {
                var oLabel = new Label() { Margin = 10 , BindingContext = oModel , FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), FontAttributes =  FontAttributes.Bold , TextColor = Color.FromHex("#3a1310") };
                oLabel.SetBinding(Label.TextProperty, "enc_pregunta", BindingMode.TwoWay);
                oStack.Children.Add(oLabel);
                List<opciones> oListaOpciones = null;
                switch (oModel.enn_tipo_respuesta)
                {
                    case 1: // Abierta
                        var oEntry = oFormulario.CrearEditor(true, true, null, 60, oModel.enn_id.ToString());
                        oStack.Children.Add(oEntry);
                        oArrayFormulario.Add(oEntry);
                        break;
                    case 2: // Si/ No
                        var oSwitch = oFormulario.CrearSwitch(true, "SI", "NO", oModel.enn_id.ToString());
                        oStack.Children.Add(oSwitch);
                        oArrayFormulario.Add(oSwitch);
                        break;
                    case 3: // MultiOpción CHECKBOX
                         oListaOpciones = oEncuestas.obtenerOpciones(oModel.enn_id);
                        for (int j = 0; j < oListaOpciones.Count; j++)
                        {
                            var oCheckBox = oFormulario.CrearCheckBox(oListaOpciones[j].opn_descripcion, true, ViewStates.Visible,oModel.enn_id.ToString(), oListaOpciones[j].opn_id.ToString());
                         //   oCheckBox.Click += OCheckBox_Click;
                            oStack.Children.Add(oCheckBox);
                            oArrayFormulario.Add(oCheckBox);
                            oArraySeleccionMultiple.Add(oCheckBox);
                        }
                       
                        break;
                    case 4: // MultiOpción RADIOBUTTON
                         oListaOpciones = oEncuestas.obtenerOpciones(oModel.enn_id);
                        var oRadrioGroup = oFormulario.CrearRadioGroup(true,ViewStates.Visible, oModel.enn_id.ToString());
                        for (int j = 0; j < oListaOpciones.Count; j++)
                        {
                            var oRadiobutton = oFormulario.CrearRadioButton(oListaOpciones[j].opn_descripcion, true, ViewStates.Visible, oModel.enn_id.ToString(), oListaOpciones[j].opn_id.ToString());
                            oRadrioGroup.AddView(oRadiobutton);
                         
                            oArrayFormulario.Add(oRadiobutton);
                            oArraySeleccionMultiple.Add(oRadiobutton);
                        }
                        oStack.Children.Add(oRadrioGroup);

                        break;
                }
                var lblDetail = new Label
                {
                    BackgroundColor = Color.OrangeRed,
                    LineBreakMode = LineBreakMode.TailTruncation
                };
                oStack.Children.Add(lblDetail);
            }
            var oButtonTerminar = oFormulario.CrearButton(true, true, "TERMINAR", LayoutOptions.FillAndExpand, "ic_checked.png");
            oButtonTerminar.Clicked += OnClickTerminar;
            oStack.Children.Add(oButtonTerminar);
            var oButtonRegresar = oFormulario.CrearButton(true, true, "REGRESAR", LayoutOptions.FillAndExpand, "ic_arrow_back.png");
            oButtonRegresar.Clicked += OnClickRegresar;
            oStack.Children.Add(oButtonRegresar);

            var scrollView = new Xamarin.Forms.ScrollView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = oStack
            };
            Content = scrollView;
            
        } // fin constructor

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

        private void OnClickTerminar(object sender, EventArgs e) {
            var oData = serializarData();

            var viewModel = new EncuestaVM();

            foreach (JObject item in oData)
            {
                
                var cln_clave = item.GetValue("cln_clave").ToString();
                var tipoRespuesta = item.GetValue("tipoRespuesta").ToString();
                var idPregunta = item.GetValue("idPregunta").ToString();
                var respuesta = item.GetValue("respuesta").ToString();

                oEncuestas.fnGuardarTablaRespuestas( Convert.ToInt32(cln_clave), respuesta, Convert.ToInt32(tipoRespuesta), Convert.ToInt32(idPregunta));
            }

            
            var oAlertDialog = oUtilerias.crearAlertDialog("Aviso", "Encuesta Terminada.");
            oAlertDialog.Create();
            oAlertDialog.SetPositiveButton("SALIR", (senderAlert, args) =>
            {
                this.Navigation.PopModalAsync();
            });

            oAlertDialog.Show();

        } // fin OnClickTerminar

        public JArray serializarData() {
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
                else if (oTipoObjeto == typeof(Android.Widget.CheckBox))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.CheckBox;
                    idPregunta = Convert.ToInt32(oSender.Tag.ToString());
                    tipoRespuesta = 3;
                    respuesta = formarRespuestaCheckBox(oSender.Tag.ToString());
                }
                else if (oTipoObjeto == typeof(Android.Widget.RadioButton))
                {
                    var oSender = oArrayFormulario[i] as Android.Widget.RadioButton;
                    idPregunta = Convert.ToInt32(oSender.Tag.ToString());
                    tipoRespuesta = 4;
                    respuesta = formarRespuestaRadioButton(oSender.Tag.ToString());
                }

                oJObject.Add("cln_clave", cln_clave);
                oJObject.Add("tipoRespuesta", tipoRespuesta);
                oJObject.Add("idPregunta", idPregunta);
                oJObject.Add("respuesta", respuesta);
                oArrayJ.Add(oJObject);

            } // fin for

             return  RemoveDuplicateTasks(oArrayJ);


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
        public string formarRespuestaCheckBox(string _Tag)
        {
            var oRespuestaFinal = "";
            for (int j = 0; j < oArraySeleccionMultiple.Count; j++)
            {
                var oSelf = oArraySeleccionMultiple[j] as Android.Widget.CheckBox;
                if ((oSelf.Tag.ToString() == _Tag) && (oSelf.Checked))
                {
                    oRespuestaFinal = oRespuestaFinal + oSelf.ContentDescription + ",";
                }
            }

            return oRespuestaFinal = oRespuestaFinal.Remove(oRespuestaFinal.Length - 1); ;
        }
        public string formarRespuestaRadioButton(string _Tag)
        {
            var oRespuestaFinal = "";
            for (int j = 0; j < oArraySeleccionMultiple.Count; j++)
            {
                var oSelf = oArraySeleccionMultiple[j] as Android.Widget.RadioButton;
                if ((oSelf.Tag.ToString() == _Tag) && (oSelf.Checked))
                {
                    oRespuestaFinal = oRespuestaFinal + oSelf.ContentDescription + ",";
                }
            }

            return oRespuestaFinal = oRespuestaFinal.Remove(oRespuestaFinal.Length - 1); ;
        }
    }
}