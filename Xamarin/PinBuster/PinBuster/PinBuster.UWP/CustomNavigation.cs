using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(MasterDetailPageRenderer), typeof(CustomNavigation))]
namespace PinBuster.UWP
{
    class CustomNavigation : Xamarin.Forms.Platform.UWP.MasterDetailPageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<MasterDetailPage> e)
        {
            base.OnElementChanged(e);

            //var app = Application.Current.
            //var applicationBar = app.ApplicationBar;
            //applicationBar.Buttons.Clear();
            //base.OnElementPropertyChanged(sender, e);

        }
    }
}
