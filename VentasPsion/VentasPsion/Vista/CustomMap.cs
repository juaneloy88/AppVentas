using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace VentasPsion.Vista
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
        /******************************************************/
        /************CLASE PERSONALIZAR PIN********************/
        public class CustomPin
        {
            public Pin Pin { get; set; }
            public string Id { get; set; }
            public string Url { get; set; }
        }
        /******************************************************/
        /******************************************************/
        public CustomMap()
        {
            // constructor
        }
       
      
    }


}
