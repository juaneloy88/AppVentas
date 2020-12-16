using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.IO;

namespace VentasPsion.Modelo
{
    public class conexionDB
    {
        
        public SQLiteConnection CadenaConexion()
        {
            #region Se obtiene la ruta de la BD
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            Directory.CreateDirectory(Path.Combine(personalFolder, "databases"));
            var dbPath = Path.Combine(personalFolder, "databases");
            var dbName = Path.Combine(dbPath, "db.db3");
            #endregion Se obtiene la ruta de la BD

            SQLiteConnection conn = new SQLiteConnection(dbName, true);

            return conn;
        }
        

    }
}
