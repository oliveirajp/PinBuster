using PinBuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using static PinBuster.App;

namespace PinBuster.Pages
{
    public partial class DetailMessageList : ContentPage
    {
        public DetailMessageList(Pin temp_Item)
        {
            var layout = new RelativeLayout();

     
            var picture = new Image()
            {
                
                Source = "http://graph.facebook.com/" + temp_Item.Face_id + "/picture?width=400&height=400"
        };


            var name = new Label()
            {
                Text = temp_Item.Nome,
                FontSize = 30,
                TextColor = Color.FromHex("#003d99"),
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
            var longitude = new Label() { Text = "Longitude: "+ temp_Item.Longitude.ToString(), FontSize = 13};

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

            var bShare = new Button { Text = "Post to Facebook", TextColor = Color.White, BackgroundColor = Color.FromHex("#3b5998") };

            bShare.Clicked += delegate
            {


                IFacebookShare FacebookShare = DependencyService.Get<IFacebookShare>();
                FacebookShare.IFacebookShare("secret message", "Porto");

                DisplayAlert("Facebook", "Your post has been shared to Facebook", "OK");


            };


            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String  userID = getCredentials.IGetCredentials()[0];

               

            var details = new StackLayout()
            {
                Spacing = 10,
                Padding = new Thickness(50, 10),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = {
            name,
            titulo_conteudo,
            conteudo,
            titulo_localizacao,
            localizacao,
            latitude,
            longitude
            }
            };

            if (temp_Item.Face_id == userID)
            {
                details.Children.Add(bShare);
            }

            layout.Children.Add(
                picture,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height * .5;
                })
            );

            layout.Children.Add(
                details,
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height * .5;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height;
                })
            );
        


            ScrollView scrollView = new ScrollView()
            {
                Content = layout
            };

            Content = scrollView;
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}

