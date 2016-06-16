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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace PinBuster.UWP
{
    public sealed partial class XamarinMapOverlay : UserControl
    {
        Models.Pin pin;

        public XamarinMapOverlay(Models.Pin pin)
        {
            this.InitializeComponent();
            if (pin != null)
                this.pin = pin;
            SetupData();

        }
        void SetupData()
        {
            if (pin != null)
            {
                Label.Text = pin.Categoria;
                Address.Text = pin.Conteudo;
                Uri uri = new Uri(pin.Imagem);
                BitmapImage bmi = new BitmapImage();
                bmi.UriSource = uri;

                UserImage.Source = bmi;
            }
            else
            {
                Label.Text = "Warning!";
                Address.Text = "Get closer to read the pin";
                InfoButton.Visibility = Visibility.Collapsed;
                EditButton.Visibility = Visibility.Collapsed;
            }
        }

        async private void OnInfoButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            await PinBuster.App.NavigateToEditPost(pin);
        }


    }
}
