using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using PinBuster.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]
namespace PinBuster.Droid
{
    class CustomNavigationRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);

            ChangeAppIconInActionBar();
        }

        void ChangeAppIconInActionBar()
        {
            var actionBar = ((Activity)Context).ActionBar;
            actionBar.SetCustomView(Resource.Layout.ActionBar);
            actionBar.SetIcon(new ColorDrawable(Color.Transparent.ToAndroid()));
            actionBar.SetDisplayShowTitleEnabled(false);
            actionBar.SetDisplayShowCustomEnabled(true);
        }
    }
}