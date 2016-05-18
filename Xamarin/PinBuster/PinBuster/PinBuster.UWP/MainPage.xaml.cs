using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms.Maps;

namespace PinBuster.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Xamarin.FormsMaps.Init("q1gj2POVAQsGUs2DWuuZ~OBfu7hWRe8C8EuJUHiGRDg~AtSM77YwjyMP0x3438i1lyZ1h5SU3B_nVhAoMEE1hbJPtqVXtiOlM5Aj7rOLmBsp");
            PinBuster.App.screenHeight = (int)Window.Current.Bounds.Height;
            PinBuster.App.screenWidth = (int)Window.Current.Bounds.Width;
            LoadApplication(new PinBuster.App());
        }
    }
}
