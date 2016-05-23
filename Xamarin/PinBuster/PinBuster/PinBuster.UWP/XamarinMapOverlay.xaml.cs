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


namespace PinBuster.UWP
{
    public sealed partial class XamarinMapOverlay : UserControl
    {
        Models.Pin pin;

        public XamarinMapOverlay(Models.Pin pin)
        {
            this.InitializeComponent();
            this.pin = pin;
            SetupData();

        }
        void SetupData()
        {
            Label.Text = pin.Nome;
            Address.Text = pin.Conteudo;
        }

        private void OnInfoButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("mostrar info");
        }

        private void OnEditButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Edit");
        }
    }
}
