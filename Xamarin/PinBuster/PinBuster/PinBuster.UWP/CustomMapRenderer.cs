using PinBuster.Pages;
using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PinBuster.UWP
{
    class CustomMapRenderer : MapRenderer
    {
        MapControl nativeMap;

        bool xamarinOverlayShown = false;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
           
            base.OnElementChanged(e);
            if (e.OldElement != null)
            {
                nativeMap.ManipulationCompleted-= OnMapElementOut;
                nativeMap.Children.Clear();
                nativeMap = null;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;
                nativeMap.Children.Clear();
                nativeMap.ManipulationCompleted += OnMapElementOut;

            }

        }

        private void OnMapElementOut(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("testetas");
        }
    }
}

