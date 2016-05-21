using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Net.Http;
using Newtonsoft.Json;
using PinBuster.Models;

namespace PinBuster
{
    class UserInfo : ContentPage
    {
        HttpClient client;
        public User user { get; set; }
        public ProfileInfo info { get; set; }

        public UserInfo()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            Task.Run(() => getUser(6));
        }

        public async void getUser(int id)
        {
            var uri_user = new Uri(string.Format("http://pinbusterapitest.azurewebsites.net/api/utilizador/" + id, string.Empty));
            var uri_achievements = new Uri(string.Format("http://pinbusterapitest.azurewebsites.net/api/perfil_info/" + id, string.Empty));

            try
            {
                var response = await client.GetAsync(uri_user);
                var response2 = await client.GetAsync(uri_achievements);
       
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var content2 = await response2.Content.ReadAsStringAsync();

                    user = JsonConvert.DeserializeObject<User>(content);
                    info = JsonConvert.DeserializeObject<ProfileInfo>(content2);

                    var layout = new StackLayout();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var logo = new Image { Aspect = Aspect.AspectFit };
                        logo.Source = ImageSource.FromResource("PinBuster.microsoft.png");
                        logo.RelScaleTo(-0.3);

                        var photo = new Image { Aspect = Aspect.AspectFit };
                        photo.Source = user.imagem;
                        photo.WidthRequest = 200;
                        photo.HeightRequest = 200;

                        Grid grid = new Grid
                        {
                            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } },
                            ColumnDefinitions = {
                                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                new ColumnDefinition { Width = GridLength.Auto },
                                new ColumnDefinition { Width = GridLength.Auto },
                                new ColumnDefinition { Width = GridLength.Auto },
                                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                            }
                        };

                        grid.Children.Add(new Label {Text = info.nr_followers + " Followers", FontSize = 15, HorizontalOptions = LayoutOptions.Center}, 1, 0);
                        grid.Children.Add(new Label {Text = info.nr_followed + " Followed", FontSize = 15, HorizontalOptions = LayoutOptions.Center}, 2, 0);
                        grid.Children.Add(new Label {Text = info.nr_mensagens + " Messages", FontSize = 15, HorizontalOptions = LayoutOptions.Center}, 3, 0);


                        this.Content = grid;

                        layout.Children.Add(logo);
                        layout.Children.Add(new BoxView() { Color = Color.FromHex("1b434c"), HeightRequest = 2 });
                        layout.Children.Add(photo);
                        layout.Children.Add(new BoxView() { Color = Color.FromHex("1b434c"), HeightRequest = 2 });
                        layout.Children.Add(new Label { Text = user.nome, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
                        layout.Children.Add(grid);

                        Content = layout;
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }

    }
}
