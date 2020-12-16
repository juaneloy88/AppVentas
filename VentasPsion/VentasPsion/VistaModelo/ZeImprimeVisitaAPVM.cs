using Android.Bluetooth;
using Base;
using Java.IO;
using Java.Util;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo;

namespace VentasPsion.VistaModelo
{
    class ZeImprimeVisitaAPVM
    {
        BluetoothDevice bthDevice = null;
        BluetoothAdapter bthAdapter = null;
        BluetoothSocket bthSocket = null;
        BufferedWriter bufWriter = null;
        Utilerias utilerias = null;

        /*Método para realizar la Impresión de un Ticket de Venta en la Impresora configurada*/
        public async void FtnImprimirTicketVisitaAP(int iIdClienteParam, string sNoTicketParam,string sRazonVoventa)
        {
            try
            {
                utilerias = new Utilerias();

                string sImpresoraConfigurada = Plugin.Settings.CrossSettings.Current.GetValueOrDefault<string>("Impresora", "");

                if (string.IsNullOrEmpty(sImpresoraConfigurada.Trim()))
                {
                    utilerias.crearMensajeLargo("¡Atención!\nNo hay Impresora Bluetooth configurada actualmente.");
                }
                else
                {
                    bthAdapter = BluetoothAdapter.DefaultAdapter;

                    if (bthAdapter == null)
                    {
                        utilerias.crearMensajeLargo("¡Atención!\nNo se ha encontrado Adaptador Bluetooth.");
                    }
                    else
                    {
                        if (bthAdapter.IsEnabled == false)
                        {
                            utilerias.crearMensajeLargo("¡Atención!\nEl Adaptador Bluetooth no está habilitado.");
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
                                utilerias.crearMensajeLargo("¡Atención!\nNo se ha encontrado Impresora Bluetooth a la cual conectarse.");
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
                                    utilerias.crearMensajeLargo("¡Atención!\nNo se pudo conectar con el Socket Bluetooth.");
                                }
                                else
                                {
                                    bthSocket.Connect();

                                    if (bthSocket.IsConnected == false)
                                    {
                                        utilerias.crearMensajeLargo("¡Atención!\nNo se pudo conectar a la Impresora Bluetooth.");
                                    }
                                    else
                                    {
                                        utilerias.crearMensajeLargo("Se ha conectado con la Impresora Bluetooth: '" + sImpresoraConfigurada + "'.");

                                        bufWriter = new BufferedWriter(new OutputStreamWriter(bthSocket.OutputStream));

                                        /*Variables para datos generales de un Cliente*/
                                        int iIdCliente = 0;
                                        int iIdRuta = 0;
                                        string sNombreCliente = "";
                                        string sRfc = "";
                                        string sDomicilio = "";
                                                    
                                        string sSql = "";

                                        List<ClienteAbonoEnvase> listaClienteAbonoEnvase = null;

                                        conexionDB cODBC = new conexionDB();

                                        using (SQLiteConnection conn = cODBC.CadenaConexion())
                                        {
                                            /*Query que obtiene los datos generales de un Cliente*/
                                            sSql = "SELECT " +
                                                    "    cl.cln_clave AS idCliente, " +
                                                    "    cl.run_clave AS idRuta, " +
                                                    "    cl.clc_nombre AS nombreCliente, " +
                                                    "    cl.clc_nombre_comercial AS nombreComercial, " +
                                                    "    cl.clc_rfc AS rfc, " +
                                                    "    cl.clc_domicilio AS domicilio " +
                                                    "FROM clientes AS cl " +
                                                    "WHERE (cl.cln_clave = " + iIdClienteParam + ")";

                                            listaClienteAbonoEnvase = conn.Query<ClienteAbonoEnvase>(sSql);
                                                   
                                            foreach (var clienteAbonoEnvase in listaClienteAbonoEnvase)
                                            {
                                                iIdCliente = clienteAbonoEnvase.idCliente;
                                                iIdRuta = clienteAbonoEnvase.idRuta;
                                                sNombreCliente = clienteAbonoEnvase.nombreCliente == null ? "" : clienteAbonoEnvase.nombreCliente.Trim();
                                                if (sNombreCliente.Trim() == "")
                                                    sNombreCliente = clienteAbonoEnvase.nombreComercial == null ? "" : clienteAbonoEnvase.nombreComercial.Trim();
                                                sRfc = clienteAbonoEnvase.rfc == null ? "" : clienteAbonoEnvase.rfc.Trim();
                                                sDomicilio = clienteAbonoEnvase.domicilio == null ? "" : clienteAbonoEnvase.domicilio.Trim();
                                            }

                                            FtnImprimirTexto("               TICKET DE VISITA                 ");
                                            FtnImprimirTexto("------------------------------------------------");
                                            //FtnImprimirTexto(" ");
                                            //FtnImprimirTexto((char)27 + "F" + (char)1 + (char)27 + "k4" + (char)28);
                                            FtnImprimirTexto("            CERVEZA CORONA EN                   ");
                                            FtnImprimirTexto("       AGUASCALIENTES S.A. DE C.V.              ");
                                            //FtnImprimirTexto((char)29 + "");
                                            FtnImprimirTexto("Reg. Fed. de Contribuyentes CCA-810221-N7A      ");
                                            FtnImprimirTexto("Direccion: Blvd. Jose Maria Chavez #2906        ");
                                            FtnImprimirTexto("Cd. Industrial, C.P. 20290, Ags., Ags.          ");
                                            //FtnImprimirTexto(" ");
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("Ruta: " + iIdRuta.ToString().PadRight(42));
                                            //FtnImprimirTexto("Ticket: " + sNoTicketParam);
                                            //FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToLongDateString().Trim());
                                            FtnImprimirTexto("Fecha: " + System.DateTime.Now.ToShortDateString().Trim().PadRight(41));
                                            FtnImprimirTexto("Hora: " + System.DateTime.Now.ToLongTimeString().Trim().PadRight(42));
                                            FtnImprimirTexto("------------------------------------------------");
                                            FtnImprimirTexto("Cliente: " + (iIdCliente + " - " + sNombreCliente).PadRight(39).Substring(0, 39));
                                            FtnImprimirTexto("RFC: " + sRfc.Trim().PadRight(43));
                                            FtnImprimirTexto("Direccion: " + sDomicilio.Trim().PadRight(133));
                                            FtnImprimirTexto("------------------------------------------------");

                                            FtnImprimirTexto(sRazonVoventa.ToUpper().Trim().PadRight(48));
                                                                                        
                                            FtnImprimirTexto("                                                ");
                                            
                                            FtnImprimirTexto("                                                ");
                                            FtnImprimirTexto("                                                ");
                                         
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                utilerias.crearMensajeLargo("¡Atención!\nError al imprimir el Ticket de Visita \nDetalle: " + exc.Message);
            }
            finally
            {
                if (bthSocket != null)
                    bthSocket.Close();

                bthDevice = null;
                bthAdapter = null;
            }
        }

        /*Método para imprime el texto recibido como parámetro*/
        public void FtnImprimirTexto(string sTextoPorImprimir)
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

    }
}
