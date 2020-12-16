using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VentasPsion.Modelo.ServicioApi
{
    public class ConexionService
    {
        //private static string sUriPrefijoConexion = "http://192.168.2.49/api/Recibe/";
        //private static string sUriPrefijoConexion = "http://192.168.2.23/VentasWebApi/api/Recibe/";
        //private static string sUriPrefijoConexion = "http://200.56.117.142/VentasWebApi/api/Recibe/";

        //public static string FtnRegresarUriPrefijoConexion
        //{
        //    get
        //    {
        //        return sUriPrefijoConexion;
        //    }
        //}

        /*Método para validar si hay conexión WIFI o de DATOS*/
        public StatusRestService FtnValidarConexionWifiDatos()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected == true)
                {
                    IEnumerable<ConnectionType> connectionTypes;
                    connectionTypes = CrossConnectivity.Current.ConnectionTypes;

                    if (connectionTypes.Contains(ConnectionType.WiFi))
                    {
                        return new StatusRestService
                        {
                            status = true,
                            //mensaje = "Conexión de WIFI activa"
                        mensaje = "http://192.168.2.23/VentasWebApi/api/Recibe/",
                        //mensaje = "http://192.168.2.49/VentasWebApi/api/Recibe/",
                            valor = "WIFI"
                        };
                    }
                    else
                    {
                        if (connectionTypes.Contains(ConnectionType.Cellular))
                        {
                            return new StatusRestService
                            {
                                status = true,
                                //mensaje = "Conexión de DATOS activa"
                                mensaje = "http://200.56.117.142/VentasWebApi/api/Recibe/",
                                valor = "DATOS"
                            };
                        }
                        else
                        {
                            return new StatusRestService
                            {
                                status = false,
                                mensaje = "No tienes conexión WIFI ni de DATOS"
                            };
                        }
                    }
                }
                else
                {
                    return new StatusRestService
                    {
                        status = false,
                        mensaje = "No tienes conexión WIFI ni de DATOS"
                    };
                }
            }
            catch (Exception ex)
            {

                return new StatusRestService
                {
                    status = false,
                    mensaje = "No tienes conexión WIFI ni de DATOS"
                };
            }
            
        }
    }
}
