using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinBuster
{
    class UserStatistics : ContentPage
    {
        public UserStatistics()
        {
            var scroll = new ScrollView();
            var stack = new StackLayout();
            scroll.Content = stack;
            var name = new Label { Text = "Aqui vai ter gráficos", FontSize = 20 };
            stack.Children.Add(new BoxView
            {
                BackgroundColor = Color.Red,
                HeightRequest = 600,
                WidthRequest = 600
            });
            stack.Children.Add(name);
            Content = scroll;
        }
    }
}
