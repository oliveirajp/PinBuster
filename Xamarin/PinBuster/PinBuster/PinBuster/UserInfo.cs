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
            var layout = new AbsoluteLayout();

            var name = new Label {Text = "João Cardoso", FontSize = 20 };
            AbsoluteLayout.SetLayoutBounds(name, new Rectangle(1, 0, 200, 25));
            AbsoluteLayout.SetLayoutFlags(name, AbsoluteLayoutFlags.PositionProportional);

            var photo = new Image { Aspect = Aspect.AspectFit };
            photo.Source = ImageSource.FromResource("PinBuster.icon.png");
            AbsoluteLayout.SetLayoutBounds(photo, new Rectangle(0, 0, 130, 130));
            AbsoluteLayout.SetLayoutFlags(photo, AbsoluteLayoutFlags.PositionProportional);
            
            layout.Children.Add(name);
            layout.Children.Add(photo);

            Content = layout;
        }
    }
}