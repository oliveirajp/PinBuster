using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace PinBuster.Pages
{
    public class CustomMap : Map
    {
        public List<Models.Pin> CustomPins { get; set; }
        public Models.CustomCircle Circle { get; set; }

    }
}
