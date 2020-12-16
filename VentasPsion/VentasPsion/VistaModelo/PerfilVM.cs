using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VentasPsion.VistaModelo
{
    public class PerfilVM
    {
        //
        public class Perfil
        {
            public string NombrePerfil { get; set; }
            public string TipoVenta { get; set; }
            public string Icon { get; set; }
            public Perfil()
            {

            }
        }
        //
        public ObservableCollection<Perfil> oPerfil { get; set; }

        public PerfilVM()
        {
            // constructor
            oPerfil = new ObservableCollection<Perfil>();
            oPerfil.Add(new Perfil
            {
                NombrePerfil = "PREVENTA",
                TipoVenta = "PREVENTA",
                Icon = "ic_deal.png"
            });

            oPerfil.Add(new Perfil
            {
                NombrePerfil = "REPARTO",
                TipoVenta = "REPARTO",
                Icon = "ic_truck.png"
            });

            oPerfil.Add(new Perfil
            {
                NombrePerfil = "AUTOVENTA",
                TipoVenta = "AUTOVENTA",
                Icon = "ic_coins.png"               
            });

            oPerfil.Add(new Perfil
            {
                NombrePerfil = "TELEVENTA",
                TipoVenta = "PREVENTA",
                Icon = "ic_phone_call.png"
            });

            oPerfil.Add(new Perfil
            {
                NombrePerfil = "MODELORAMAS",
                TipoVenta = "AUTOVENTA",
                Icon = "ic_rama_compact.png"
            });

        }
    }
}
