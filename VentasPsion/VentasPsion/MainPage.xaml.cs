using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using SQLite;
using System.IO;

using VentasPsion.Modelo;
using VentasPsion.Vista;

namespace VentasPsion
{
	public partial class MainPage : ContentPage
	{
        
        

        public MainPage()
        {
            InitializeComponent();
        }


         async void OnButtonClicked1(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new frmLogin());
        }


            void OnButtonClicked(object sender, EventArgs args)
        {
            fnMuestra();
        }

        private void fnMuestra()
        {
            conexionDB cODBC = new conexionDB();
            SQLiteConnection conn;
            conn = cODBC.CadenaConexion();

            string sQuery = " select count(*) from clientes ";

            int iValor = conn.ExecuteScalar<int>(sQuery);

            DisplayAlert("Message", iValor.ToString(), "OK");
        }

        //private void fnCargaDatos()
        //    {
        //    string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        //    Directory.CreateDirectory(Path.Combine(personalFolder, "databases"));
        //    string strPath = Path.Combine(Path.Combine(personalFolder, "databases"), "db.db3");
        //    // SQLiteConnection conn = new SQLiteConnection (strPath,true);

        //    conn = new SQLiteConnection(strPath, true);

        //    conn.CreateTable<bonificaciones>();
        //    conn.CreateTable<clientes>();
        //    conn.CreateTable<clientes_estatus>();
        //    conn.CreateTable<consepto_devoluciones>();
        //    conn.CreateTable<empleados>();

        //    conn.CreateTable<envase>();
        //    conn.CreateTable<Existencia>();
        //    conn.CreateTable<gps>();
        //    conn.CreateTable<orden_ticket>();
        //    conn.CreateTable<password>();

        //    conn.CreateTable<pedido>();
        //    conn.CreateTable<productos>();
        //    conn.CreateTable<ruta>();
        //    conn.CreateTable<candados_productos>();
        //    conn.CreateTable<cuota>();

        //    conn.CreateTable<devoluciones>();
        //    conn.CreateTable<encuesta>();
        //    conn.CreateTable<leyenda>();
        //    conn.CreateTable<promociones>();
        //    conn.CreateTable<respuestas>();

        //    conn.CreateTable<venta_cabecera>();
        //    conn.CreateTable<venta_detalle>();
        //    conn.CreateTable<lista_maestra>();

        //    new CargaDatos().carga(conn);
        //}
    }
}
