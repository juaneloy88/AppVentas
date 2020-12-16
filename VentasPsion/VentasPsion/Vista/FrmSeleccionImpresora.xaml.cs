using Java.Util;
using System;
using Android.Bluetooth;
using Java.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Base;
using VentasPsion.VistaModelo;

namespace VentasPsion.Vista
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FrmSeleccionImpresora : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<string> ListaDispositivos { get; set; } = new ObservableCollection<string>();
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        public FrmSeleccionImpresora ()
		{
			InitializeComponent ();

            lblRutaPlusTipoVenta.Text = VarEntorno.iNoRuta.ToString() + " - " + VarEntorno.sTipoVenta;
            lblFechaVenta.Text = VarEntorno.dFechaVenta.ToShortDateString();
            lblVersionApp.Text = VarEntorno.sVersionApp;

            string sImpresoraConfigurada = Plugin.Settings.CrossSettings.Current.GetValueOrDefault<string>("Impresora", "");

            lblImpresoraConfigurada.Text = sImpresoraConfigurada.Trim();

            // Valida que ya haya una Impresora configurada.
            if (string.IsNullOrEmpty(sImpresoraConfigurada.Trim()))
                btnImprimirPrueba.IsEnabled = false;
            else
                btnImprimirPrueba.IsEnabled = true;

            this.BindingContext = this;

            FtnObtenerDispositivosConectados();
        }

        /*Método para encontrar las Impresoras Bluetooth vinculadas*/
        public void FtnObtenerDispositivosConectados()
        {
            try
            {
                bthAdapter = BluetoothAdapter.DefaultAdapter;
                utilerias = new Utilerias();

                if (bthAdapter == null)
                {
                    DisplayAlert("¡Atención!", "No se ha encontrado Adaptador Bluetooth, por lo que no se pudieron encontrar Impresoras Bluetooth vinculadas.", "OK");
                }
                else
                {
                    if (bthAdapter.IsEnabled == false)
                    {
                        DisplayAlert("¡Atención!", "El Adaptador Bluetooth no está habilitado, por lo que no se pudieron encontrar Impresoras Bluetooth vinculadas.", "OK");
                    }
                    else
                    {
                        foreach (var dispositivo in bthAdapter.BondedDevices)
                            ListaDispositivos.Add(dispositivo.Name + " - " + dispositivo.Address);
                    }
                }
            }
            catch (Exception exc)
            {
                DisplayAlert("¡Atención!", "Error al encontrar Impresoras Bluetooth vinculadas.\nDetalle: " + exc.Message, "OK");
            }
            finally
            {
                bthAdapter = null;
            }
        }

        /*Método para realizar una Impresión de Prueba en la Impresora configurada*/
        private void btnImprimirPrueba_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lblImpresoraConfigurada.Text.Trim()))
            {
                DisplayAlert("¡Aviso!", "No hay Impresora configurada actualmente.", "OK");
            }
            else
            {
                try
                {
                    string sImpresoraConfigurada = lblImpresoraConfigurada.Text.Trim();

                    bthAdapter = BluetoothAdapter.DefaultAdapter;
                    utilerias = new Utilerias();

                    if (bthAdapter == null)
                    {
                        DisplayAlert("¡Atención!", "No se ha encontrado Adaptador Bluetooth, por lo que no se pudo efectuar la Impresión de Prueba.", "OK");
                    }
                    else
                    {
                        if (bthAdapter.IsEnabled == false)
                        {
                            DisplayAlert("¡Atención!", "El Adaptador Bluetooth no está habilitado, por lo que no se pudo efectuar la Impresión de Prueba.", "OK");
                        }
                        else
                        {
                            foreach (var dispositivo in bthAdapter.BondedDevices)
                            {
                                if ((dispositivo.Name.ToUpper().Trim() + " - " + dispositivo.Address.ToUpper().Trim()) == sImpresoraConfigurada.ToUpper().Trim())
                                {
                                    bthDevice = dispositivo;
                                    break;
                                }
                            }

                            if (bthDevice == null)
                            {
                                DisplayAlert("¡Atención!", "No se ha encontrado Impresora Bluetooth a la cual conectarse, por lo que no se pudo efectuar la Impresión de Prueba.", "OK");
                            }
                            else
                            {
                                UUID uuid = UUID.FromString("00001101-0000-1000-8000-00805f9b34fb");
                                if ((int)Android.OS.Build.VERSION.SdkInt >= 10) // Gingerbread 2.3.3 2.3.4
                                    bthSocket = bthDevice.CreateInsecureRfcommSocketToServiceRecord(uuid);
                                else
                                    bthSocket = bthDevice.CreateRfcommSocketToServiceRecord(uuid);

                                if (bthSocket == null)
                                {
                                    DisplayAlert("¡Atención!", "No se pudo conectar con el Socket Bluetooth, por lo que no se pudo efectuar la Impresión de Prueba.", "OK");
                                }
                                else
                                {
                                    bthSocket.Connect();

                                    if (bthSocket.IsConnected == false)
                                    {
                                        DisplayAlert("¡Atención!", "No se pudo conectar a la Impresora Bluetooth, por lo que no se pudo efectuar la Impresión de Prueba.", "OK");
                                    }
                                    else
                                    {
                                        utilerias.crearMensaje("Se ha conectado con la Impresora Bluetooth: '" + sImpresoraConfigurada + "'.");

                                        bufWriter = new BufferedWriter(new OutputStreamWriter(bthSocket.OutputStream));

                                        if (VarEntorno.sTipoImpresora == "Zebra")
                                        {
                                            FtnImprimirTextoZebra("                                                ");
                                            FtnImprimirTextoZebra("------------------------------------------------");
                                            FtnImprimirTextoZebra("               CERVEZA CORONA EN                ");
                                            FtnImprimirTextoZebra("          AGUASCALIENTES S.A. DE C.V.           ");
                                            FtnImprimirTextoZebra("                                                ");
                                            FtnImprimirTextoZebra("    *** Esta es una impresion de prueba ***     ");
                                            FtnImprimirTextoZebra("                                                ");
                                            FtnImprimirTextoZebra("Impresora configurada:                          ");
                                            FtnImprimirTextoZebra(("'" + sImpresoraConfigurada.Trim() + "'").PadRight(48).Substring(0, 48));
                                            FtnImprimirTextoZebra("                                                ");
                                            FtnImprimirTextoZebra("------------------------------------------------");
                                            FtnImprimirTextoZebra("                                                ");
                                            FtnImprimirTextoZebra("                                                ");
                                        }
                                        else
                                        {
                                            FtnImprimirTexto((char)29 + "");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto((char)27 + "F" + (char)1 + (char)27 + "k4" + (char)28);
                                            FtnImprimirTexto("            CERVEZA CORONA EN");
                                            FtnImprimirTexto("       AGUASCALIENTES S.A. DE C.V.");
                                            FtnImprimirTexto((char)29 + "");
                                            FtnImprimirTexto(" *** Esta es una impresion de prueba ***");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("Impresora configurada:");
                                            FtnImprimirTexto("'" + sImpresoraConfigurada.Trim() + "'");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                            FtnImprimirTexto(" ");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    DisplayAlert("¡Atención!", "Error al efectuar la Impresión de Prueba.\nDetalle: " + exc.Message, "OK");
                }
                finally
                {
                    if (bthSocket != null)
                        bthSocket.Close();

                    bthDevice = null;
                    bthAdapter = null;
                }
            }
        }

        /*Método para imprime el texto recibido como parámetro*/
        public void FtnImprimirTexto(string sTextoPorImprimir)
        {
            bufWriter.Write(sTextoPorImprimir + "\n");
            bufWriter.Flush();
        }

        /*Método para imprime el texto recibido como parámetro*/
        public void FtnImprimirTextoZebra(string sTextoPorImprimir)
        {
            sTextoPorImprimir = sTextoPorImprimir.Replace("Ñ", "N");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Á", "A");
            sTextoPorImprimir = sTextoPorImprimir.Replace("É", "E");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Í", "I");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Ó", "O");
            sTextoPorImprimir = sTextoPorImprimir.Replace("Ú", "U");
            bufWriter.Write(sTextoPorImprimir + "\n");
            bufWriter.Flush();
        }

        /*Método para seleccionar una Impresora y configurarla para la impresión*/
        private void btnConectar_Clicked(object sender, EventArgs e)
        {
            if (pkrEligeImpresora.SelectedIndex < 0)
            {
                DisplayAlert("¡Aviso!", "No ha seleccionado otra Impresora.", "OK");
            }
            else
            {
                if (lblImpresoraConfigurada.Text.Trim() == pkrEligeImpresora.Items[pkrEligeImpresora.SelectedIndex].Trim())
                {
                    DisplayAlert("¡Aviso!", "La Impresora seleccionada es la que está actualmente configurada.", "OK");
                    pkrEligeImpresora.SelectedIndex = -1;
                }
                else
                {
                    Plugin.Settings.CrossSettings.Current.AddOrUpdateValue<string>("Impresora", pkrEligeImpresora.Items[pkrEligeImpresora.SelectedIndex].Trim());
                    lblImpresoraConfigurada.Text = pkrEligeImpresora.Items[pkrEligeImpresora.SelectedIndex].Trim();
                    btnImprimirPrueba.IsEnabled = true;
                    utilerias.crearMensaje("La Impresora Bluetooth '" + pkrEligeImpresora.Items[pkrEligeImpresora.SelectedIndex].Trim() + "' quedó configurada.");
                    pkrEligeImpresora.SelectedIndex = -1;
                }
            }

            if (lblImpresoraConfigurada.Text.Trim().Contains("XX"))
            {
                VarEntorno.sTipoImpresora = "Zebra";
                
            }
            else
            {
                VarEntorno.sTipoImpresora = "";
            }
        }

        /*Método para abrir la pantalla de MENÚ PRINCIPAL*/
        private void btnRegresar_Clicked(object sender, EventArgs e)
        {
            //this.Navigation.PushModalAsync(new frmMenuPrincipal());
            this.Navigation.PopModalAsync();
        }
    }
}