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
            Icon = "hamburger.png";
            Padding = new Thickness(10, 20);

            var categories = new List<Menu>() {
            new Menu("Pin list","vista_lista.png", () => App.listView),
            new Menu("Map","vista_mapa.png",() => App.mapPage),
            new Menu("Profile","perfil.png", () => new UserPage()),
            new Menu("Search","search.png", () => new SearchPage())
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
