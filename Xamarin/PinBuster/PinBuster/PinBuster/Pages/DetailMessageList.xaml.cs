using PinBuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster.Pages
{
    public partial class DetailMessageList : ContentPage
    {
        Pin currentPin;

        public DetailMessageList(Pin temp_Item)
        {
            var layout = new RelativeLayout();

            currentPin = temp_Item;
            Image picture;
            if (temp_Item.Nome == "Admin")
            {
                picture = new Image()
                {
                    Source = temp_Item.Face_id,
                    HeightRequest = 200,
                    WidthRequest = 200
                };
            }
            else
            {
                picture = new Image()
                {
                    Source = "http://graph.facebook.com/" + temp_Item.Face_id + "/picture?width=200&height=200",
                    HeightRequest = 200,
                    WidthRequest = 200
                };
            }


            var name = new Label()
            {
                Text = temp_Item.Nome,
                FontSize = 30,
                TextColor = Color.Black,
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "sans-serif", "")
            };

            var titulo_localizacao = new Label()
            {
                Text = "Localização:",
                FontSize = 15,
                TextColor = Color.FromHex("#B7A19B"),
                FontFamily = Device.OnPlatform("HelveticaNeue-CondensedBlack", "sans-serif-condensed", "")
            };


            var localizacao = new Label() { Text = temp_Item.Localizacao };
            var latitude = new Label() { Text = "Latitude: " + temp_Item.Latitude.ToString(), FontSize = 13 };
            var longitude = new Label() { Text = "Longitude: " + temp_Item.Longitude.ToString(), FontSize = 13 };

            var titulo_conteudo = new Label()
            {
                Text = "Mensagem " + temp_Item.Categoria + " :",
                FontSize = 15,
                TextColor = Color.FromHex("#B7A19B"),
                FontFamily = Device.OnPlatform("HelveticaNeue-CondensedBlack", "sans-serif-condensed", "")
            };

            var conteudo = new Label()
            {
                Text = temp_Item.Conteudo,
                FontSize = 20,
                FontFamily = Device.OnPlatform("HelveticaNeue", "sans-serif", "")
            };
            var viewprofile = new Button
            {
                Text = "View profile",
                TextColor = Color.FromHex("#8B8D8D"),
                BackgroundColor = Color.FromHex("#1B434D"),
                VerticalOptions = LayoutOptions.End
            };
            viewprofile.Clicked += ViewProfileAction;

            var details = new StackLayout()
            {
                Spacing = 10,
                Padding = new Thickness(50, 10),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = {
            name,
            viewprofile,
            titulo_conteudo,
            conteudo,
            titulo_localizacao,
            localizacao,
            latitude,
            longitude
        }
            };

            layout.Children.Add(
                picture,
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width * 0.5 - 100;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * 0.1;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return 200;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return 200;
                })
            );

            layout.Children.Add(
                details,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height * .1 + 300;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height;
                })
            );

            ScrollView scrollView = new ScrollView()
            {
                Content = layout
            };

            Content = scrollView;
        }

        async void ViewProfileAction(object sender, EventArgs args)
        {
            var User_page = new UserPage(currentPin.Face_id); // so the new page shows correct data
            await Navigation.PushAsync(User_page);
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}

