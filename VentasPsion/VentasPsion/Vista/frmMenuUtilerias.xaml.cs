using Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VentasPsion.VistaModelo;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class frmMenuUtilerias : ContentPage
    {
        private static bool banderaClick;
        Utilerias oUtilerias = new Utilerias();
        public frmMenuUtilerias ()
		{
			InitializeComponent ();
            banderaClick = true;
            Title = "UTILERIAS";
		}




        protected override async void OnAppearing()
        {
            // Set up activity indicator
            LlenarMenu();
            await Task.Yield();
            // Remove activity indicator and set up real views
        }
        public  void LlenarMenu()
        {
            MenuUtileriasVM oMenuUtileriasVM = new MenuUtileriasVM();

            listViewMenuUtilerias.ItemsSource = null;
            //  await Task.Delay(500);
            listViewMenuUtilerias.ItemsSource = oMenuUtileriasVM.obtenerListaUtilerias();

        }

        public async void OnClickMenuSeleccionado(object sender, SelectedItemChangedEventArgs e)
        {
            listViewMenuUtilerias.SelectedItem = true;
            if (banderaClick)
            {
                var item = e.SelectedItem as MenuUtileriasVM.UtileriasMenu;
                if ((item != null) && (item.Habilitado))
                {
                    var opcion = item.Opcion;
                    banderaClick = false;
                    switch (opcion)
                    {
                        case "IMPRESORA BLUETOOTH":
                            await this.Navigation.PushModalAsync(new FrmSeleccionImpresora());
                            break;
                        case "AJUSTES":
                            var vPassword = await InputBoxLiberarRuta(this.Navigation);
                            string sPassword = vPassword.ToString();
                            if (await validaPassword(sPassword))
                                await this.Navigation.PushModalAsync(new frmUtilerias());
                            else
                               await  DisplayAlert("AVISO", "Sin acceso a la opción seleccionada", "OK");
                            break;
                        case "EXPORTAR DB":
                            backupdDatabase();
                            await this.Navigation.PushModalAsync(new frmExportarDB());
                            break;
                    }
                    await Task.Run(async () =>
                    {
                        await Task.Delay(500);
                        banderaClick = true;

                    });
                }
                else
                {
                    oUtilerias.crearMensaje("Sin acceso a la opción seleccionada.");
                }
            }
        } // fin OnClickMenuSeleccionado

        public void backupdDatabase()
        {
            try
            {
               /* string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                Directory.CreateDirectory(Path.Combine(personalFolder, "databases"));
                var dbPath = Path.Combine(personalFolder, "databases");
                var dbName = Path.Combine(dbPath, "db.db3");


                //String inFileName = "/data/data/<your.app.package>/databases/foo.db";
                String inFileName = dbName;
                Java.IO.File dbFile = new File(inFileName);
                FileInputStream fis = new FileInputStream(dbFile);

                String outFileName = Environment.getExternalStorageDirectory() + "/database_copy.db";

                // Open the empty db as the output stream
                OutputStream output = new FileOutputStream(outFileName);

                // Transfer bytes from the inputfile to the outputfile
                byte[] buffer = new byte[1024];
                int length;
                while ((length = fis.read(buffer)) > 0)
                {
                    output.write(buffer, 0, length);
                }

                // Close the streams
                output.flush();
                output.close();
                fis.close();
            */
            }
            catch (Exception e)
            {
                //Log.i("Backup", e.toString());
            }
        }

        private async Task<bool> validaPassword(string sPass)
        {
            try
            {
                return await  PasswordVM.PassAjustes(sPass);
                /*
                bool bResult;
                switch (sPass)
                {
                    default:
                        bResult = false;
                        break;
                    case "2486":
                    case "1793":
                    case "357":
                    case "159":
                    case "789":
                        bResult = true;
                        break;
                }
                return bResult;*/
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region Método para crear el InputBox de Liberación de Ruta
        public static Task<string> InputBoxLiberarRuta(INavigation navigation)
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblTitle = new Label { Text = "Capture un Valor", HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold };
            var lblMessage = new Label { Text = "Contraseña:" };
            var txtInput = new Entry { Text = "", Keyboard = Keyboard.Numeric, IsPassword = true, Placeholder = "0" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;

                if (result == "")
                {
                    result = "0";
                }
                await navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                var result = "0";
                await navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(result);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblTitle, lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }
        #endregion Método para crear el InputBox de Liberación de Ruta

    }
}