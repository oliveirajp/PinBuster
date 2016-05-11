using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinBuster
{
    class UserRecentActivity : ContentPage
    {

        public UserRecentActivity()
        {
            var layout = new StackLayout();

            var title = new Label { Text = "Aqui vai ter entre 3 a 5 mapas com os pins mais recentes" };
            layout.Children.Add(title);

            Content = layout;
        }
    }
}
