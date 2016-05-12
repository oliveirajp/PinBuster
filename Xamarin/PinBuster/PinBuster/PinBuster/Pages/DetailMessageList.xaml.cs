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
        public DetailMessageList(Message_i temp_Item)
        {
            var layout = new RelativeLayout();
            /*
            var overlay = new Image()
            {
                Aspect = Aspect.AspectFill,
                Source = new FileImageSource() { File = temp_Item.Imagem }
            }; */

            var picture = new Image()
            {
                Aspect = Aspect.AspectFit,
                Source = temp_Item.Imagem
            };

            var name = new Label()
            {
                Text = temp_Item.Nome,
                FontSize = 30,
                TextColor = Color.FromHex("#003d99"),
                FontFamily = Device.OnPlatform("HelveticaNeue-Medium", "sans-serif", "")
            };

            var tagline = new Label() { Text = temp_Item.Localizacao };

            var scovilleLabel = new Label()
            {
                Text = "Conteudo:",
                FontSize = 15,
                TextColor = Color.FromHex("#B7A19B"),
                FontFamily = Device.OnPlatform("HelveticaNeue-CondensedBlack", "sans-serif-condensed", "")
            };

            var scoville = new Label()
            {
                Text = temp_Item.Conteudo,
                FontSize = 20,
                FontFamily = Device.OnPlatform("HelveticaNeue", "sans-serif", "")
            };

            var details = new StackLayout()
            {
                Spacing = 10,
                Padding = new Thickness(50, 10),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = {
            name,
            tagline,
            scovilleLabel,
            scoville,
        }
            };

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

        /*    layout.Children.Add(
                overlay,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => {
                    return parent.Width;
                }),
                Constraint.RelativeToParent((parent) => {
                    return parent.Height;
                })
            ); */

            Content = layout;
        }

        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}

