using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class frmRegistroTelefono : ContentPage
    {
        TelefonosVM telVM = new TelefonosVM();

        public frmRegistroTelefono()
        {
            InitializeComponent();
            telVM = new TelefonosVM();

            pktHorarioIni.Time = DateTime.Now.TimeOfDay;
            pktHorarioFin.Time = DateTime.Now.TimeOfDay;
        }

        private bool ValidaCampos()
        {
            try
            {
                bool bresult = false;
                //string regularexpre = @"^(\(?\+?[0-9]*\)?)?[0-9_\- \(\)]*$";
                string regexpTel = @"^[0-9]{7}$";
                string regexpCel = @"^[0-9]{10}$";

                Match m ;
                if (sCelular.IsToggled)
                    m = Regex.Match(txtTelefono.Text, regexpCel, RegexOptions.IgnoreCase);
                else
                    m = Regex.Match(txtTelefono.Text, regexpTel, RegexOptions.IgnoreCase);

                if (txtNombre.Text.Length > 1)
                {
                    if (m.Success)
                    {
                        telVM.Tel.cln_clave = VarEntorno.vCliente.cln_clave;
                        telVM.Tel.tcc_telefono = txtTelefono.Text;
                        telVM.Tel.tcc_nombre = txtNombre.Text;
                        telVM.Tel.tcb_estatus = true;
                        telVM.Tel.tcb_movil = sCelular.IsToggled;

                        DateTime time = DateTime.Today.Add(pktHorarioIni.Time);                        
                        telVM.Tel.tct_horarioini = time.ToString("hh:mm tt");
                        time = DateTime.Today.Add(pktHorarioFin.Time);
                        telVM.Tel.tct_horariofin = time.ToString("hh:mm tt");

                        telVM.Tel.tcc_comentario = txtComentario.Text;
                        telVM.Tel.tcn_rutacaptura = VarEntorno.iNoRuta;
                        telVM.Tel.tct_fechacaptura = DateTime.Now.Date;

                        bresult = true;
                    }
                    else                    
                        DisplayAlert("Aviso", "ingrese un numero valido ", "OK");
                    
                }
                else                
                    DisplayAlert("Aviso","ingrese el nombre de la persona", "OK");
                
                return bresult;
            }
            catch (Exception ex)
            {
                DisplayAlert("Aviso", ex.Message, "OK");
                return false;
            }
        }

        public async void OnClickedGuardar(object sender, EventArgs args)
        {
            try
            {
                if (ValidaCampos())
                {
                    if (telVM.GuardaRegistro())
                    {
                        if (await DisplayAlert("Aviso", "Guardado con exito ¿Desea Guardar otro Telefono?", "Si", "NO"))
                            LimpiaCampos();
                        else
                            await this.Navigation.PopModalAsync();
                    }
                    else
                        await DisplayAlert("Aviso", "Revice la informacion ", "OK");
                }                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", ex.Message, "OK");
            }
        }

        void LimpiaCampos()
        {
            txtNombre.Text = "";
            txtTelefono.Text = "";
            txtComentario.Text = "";
            sCelular.IsToggled = false;
            pktHorarioIni.Time = new TimeSpan(12, 0, 0);
            pktHorarioFin.Time = new TimeSpan(12, 0, 0);
            telVM.limpia();
        }

        public void OnClickedRegresar(object sender, EventArgs args)
        {
            this.Navigation.PopModalAsync();
        }
    }
}