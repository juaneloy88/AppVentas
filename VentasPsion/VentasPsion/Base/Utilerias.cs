using Android;
using Android.App;
using Android.Content;

using Android.Widget;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Text;
using VentasPsion.Modelo;
using VentasPsion.VistaModelo;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Org.Json;
//using Plugin.Geolocator;
using Xamarin.Forms;
using System.IO;
using Android.Graphics.Drawables;
using Android.Locations;
using VentasPsion.Modelo.Servicio;
using VentasPsion.Modelo.ServicioApi;

namespace Base
{
    public class Utilerias : Activity
    {
        Activity oActivity;
        public string camionBase64 = "data:image/png;base64,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";

        public Utilerias()
        {
            // constructor
            oActivity = CrossCurrentActivity.Current.Activity;
        }

        public AlertDialog.Builder crearAlertDialog(String oTitulo,String oMensaje)
        {
            AlertDialog.Builder oAlertDialog = new AlertDialog.Builder(oActivity);
            oAlertDialog.SetTitle(oTitulo);
            oAlertDialog.SetMessage(oMensaje);
            return oAlertDialog;
        }

  

        public ProgressDialog crearProgressDialog(String title, String mensaje)
        {
            ProgressDialog oDialog = new ProgressDialog(oActivity);
            oDialog.SetTitle(title);
            oDialog.SetMessage(mensaje);
            oDialog.SetProgressStyle(ProgressDialogStyle.Spinner);
            oDialog.Indeterminate = true;
            oDialog.SetCancelable(false);
        
           // oDialog.SetIcon(Resource.Mipmap.SymDefAppIcon);
            
            return oDialog;
        }

        public void crearMensaje(String oMensaje)
        {
            Toast.MakeText(oActivity, oMensaje, ToastLength.Short).Show();
        }

        public void crearMensajeLargo(String oMensaje)
        {
            Toast.MakeText(oActivity, oMensaje, ToastLength.Long).Show();
        }



        /*****************************************************/
        /*
            Utilerias oUtilerias = new Utilerias();
            //Task<JSONObject> oJSONObject = oUtilerias.obtenerCoordenadas();
            JSONObject oResultTask = oJSONObject.Result;
            oResultTask.OptString("Latitude");
            oResultTask.OptString("Longitude");     
        */
        /*****************************************************/
        /*  public async Task<JSONObject> obtenerCoordenadas()
          {
              JSONObject oJSONObject = new JSONObject();
              var cached = await CrossGeolocator.Current.GetLastKnownLocationAsync();
              //var cached = CrossGeolocator.Current.GetPositionAsync();
              if (cached == null)
              {
                  oJSONObject.Put("Timestamp", null);
                  oJSONObject.Put("Latitude", null);
                  oJSONObject.Put("Longitude", null);
                  return oJSONObject;
              }
              else
              {
                  oJSONObject.Put("Timestamp", cached.Timestamp.ToString());
                  oJSONObject.Put("Latitude", cached.Latitude.ToString());
                  oJSONObject.Put("Longitude", cached.Longitude.ToString());
                  Console.WriteLine("Position Status: {0}", cached.Timestamp);
                  Console.WriteLine("Position Latitude: {0}", cached.Latitude);
                  Console.WriteLine("Position Longitude: {0}", cached.Longitude);
                  return oJSONObject;
              }
          }*/
        public bool isGPSEnabled(Context mContext)
        {
            LocationManager locationManager = (LocationManager)
                        mContext.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }
        public async void  obtenerCoordenadas(int iProceso)
        {
            try
            {
                JSONObject oJSONObject = new JSONObject();

                ConexionService conexionWifiDatos = new ConexionService();
                StatusRestService statusConexion = new StatusRestService();
                statusConexion = conexionWifiDatos.FtnValidarConexionWifiDatos();

                if (statusConexion.status == true)
                {
               
                        //  var cached = await CrossGeolocator.Current.GetLastKnownLocationAsync();
                        var cached = await CrossGeolocator.Current.GetPositionAsync();

                        if (cached == null)
                        {
                            //   oJSONObject.Put("Timestamp", null);
                            oJSONObject.Put("Latitude", null);
                            oJSONObject.Put("Longitude", null);
                            //bResult = false; ;
                        }
                        else
                        {
                            //  oJSONObject.Put("Timestamp", cached.Timestamp.ToString());
                            oJSONObject.Put("Latitude", cached.Latitude.ToString());
                            oJSONObject.Put("Longitude", cached.Longitude.ToString());

                            //bResult = true;
                        }
                        gpsSR oFnGPS = new gpsSR();
                        //this.oLatitud = oResultTask.OptString("Latitude");
                        //this.oLongitud = oResultTask.OptString("Longitude");
                        oFnGPS.GuardarGPSCliente(VarEntorno.vCliente, cached.Latitude.ToString(), cached.Longitude.ToString(), iProceso);
                        // return bResult;
                }
            }
                catch 
            {

            }

        }
        public double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }     

    } // fin clase


    
}
