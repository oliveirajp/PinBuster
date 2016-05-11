using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinBuster
{
    class UserAchievements : ContentPage
    {
        public UserAchievements()
        {
            var stack = new StackLayout();

            var name = new Label { Text = "Aqui vai ter os premios", FontSize = 20 };
           
            stack.Children.Add(name);
            Content = stack;
        }
    }
}
