using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace PinBuster.Pages
{
    public class CustomMap : Map
    {
        public List<Models.Pin> CustomPins { get; set; }
        public Models.CustomCircle Circle { get; set; }
    }
}
