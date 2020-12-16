using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VentasPsion.VistaModelo
{
    public class MenuUtileriasVM
    {
        //
        public class UtileriasMenu
        {
            public string Opcion { get; set; }
            public bool Habilitado { get; set; }
            public string Icon { get; set; }
            public UtileriasMenu()
            {// constructor

            }
        }
        //
        public MenuUtileriasVM()
        {
            // constructor
        }

        public ObservableCollection<UtileriasMenu> obtenerListaUtilerias()
        {
            ObservableCollection<UtileriasMenu> oMenuUtilerias = new ObservableCollection<UtileriasMenu>();
            oMenuUtilerias.Add(new UtileriasMenu
            {
                Opcion = "IMPRESORA BLUETOOTH",
                Habilitado = true,
                Icon = "ic_bluetooth.png"
            });

            oMenuUtilerias.Add(new UtileriasMenu
            {
                Opcion = "AJUSTES",
                Habilitado = true,
                Icon = "ic_info.png"
            });

            oMenuUtilerias.Add(new UtileriasMenu
            {
                Opcion = "EXPORTAR DB",
                Habilitado = true,
                Icon = "ic_info.png"
            });
            return oMenuUtilerias;
        }
    }
}
