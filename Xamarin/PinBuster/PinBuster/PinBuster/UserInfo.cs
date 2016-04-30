using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinBuster
{
    class UserInfo : ContentPage
    {
        public UserInfo()
        {
            var layout = new StackLayout();

            var logo = new Image { Aspect = Aspect.AspectFit };
            logo.Source = ImageSource.FromResource("PinBuster.microsoft.png");
            logo.WidthRequest = 400;

            //var tela = new BoxView { BackgroundColor = Color.FromHex("dddddc"), HeightRequest = 275, WidthRequest = 1000 };

            var photo = new Image { Aspect = Aspect.AspectFit };
            photo.Source = ImageSource.FromResource("PinBuster.icon.png");

            var name = new Label { Text = "João Cardoso", FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand};

            layout.Children.Add(logo);
            //layout.Children.Add(tela);
            layout.Children.Add(photo);
            layout.Children.Add(name);
            
            Content = layout;
        }
    }
}