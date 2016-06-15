using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace PinBuster.Pages
{
    public class CustomRecentActMap : Map
    {
        public List<Models.Pin> CustomPins { get; set; }
    }
}
