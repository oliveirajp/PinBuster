using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigation))]
namespace PinBuster.UWP
{
    class CustomNavigation : Xamarin.Forms.Platform.UWP.NavigationPageRenderer
    {
        //protected override void OnElementChanged()
        //{
        //    base.OnElementChanged(e);

        //    ChangeAppIconInActionBar();
        //}

        //void ChangeAppIconInActionBar()
        //{
        //    var actionBar = ((Activity)Context).ActionBar;
        //    actionBar.SetCustomView(Resource.Layout.ActionBar);
        //    actionBar.SetIcon(new ColorDrawable(Color.Transparent.ToAndroid()));
        //    actionBar.SetDisplayShowTitleEnabled(false);
        //    actionBar.SetDisplayShowCustomEnabled(true);
        //}
    }
}
