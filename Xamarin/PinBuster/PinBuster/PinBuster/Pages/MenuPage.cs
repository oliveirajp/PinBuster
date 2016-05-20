using PinBuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PinBuster.Pages
{
    public class MenuPage : ContentPage
    {


        public Action<Page> OnMenuSelect
        {
            get;
            set;
        }

        public MenuPage()
        {
            Title = "Menu";

            Padding = new Thickness(10, 20);

            var categories = new List<Menu>() {
            new Menu("Pin list", () => App.listView),
            new Menu("Map", () => App.mapPage),
            new Menu("Profile", () => new UserPage()),
        };

            var dataTemplate = new DataTemplate(typeof(TextCell));
            dataTemplate.SetBinding(TextCell.TextProperty, "Name");

            var listView = new ListView()
            {
                ItemsSource = categories,
                ItemTemplate = dataTemplate
            };


            listView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => {
                if (OnMenuSelect != null)
                {
                    var category = (Menu)e.SelectedItem;
                    var categoryPage = category.PageFn();
                    OnMenuSelect(categoryPage);
                }
            };

            Content = listView;
        }
    }
}
