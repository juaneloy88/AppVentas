using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using VentasCCA.Droid;
using Plugin.Permissions;
using Android.Support.Design.Widget;
using static Android.Resource;

namespace VentasPsion.Droid
{
    //[Activity (Label = "VentasPsion", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [Activity(Label = "VentasCCA", Theme = "@style/MainTheme", Icon = "@drawable/icon", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
          //  base.Window.RequestFeature(WindowFeatures.ActionBar);
            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
           // base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
            //   Xamarin.FormsMaps.Init(this, bundle);
            Xamarin.FormsGoogleMaps.Init(this, bundle); // initialize for Xamarin.Forms.GoogleMaps
            LoadApplication (new VentasPsion.App ());
		}

        /*Método override del botón de Regreso de Android para cancelar que se pueda ir hacia atrás con dicho botón*/
        public override void OnBackPressed()
        {
           // base.OnBackPressed();

            Toast msgToast = Toast.MakeText(this, "- Este botón no funciona en la App de Ventas.", ToastLength.Short);
            msgToast.Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

