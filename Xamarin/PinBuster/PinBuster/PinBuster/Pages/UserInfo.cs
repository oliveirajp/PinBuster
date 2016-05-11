using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinBuster.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;


namespace PinBuster
{
    class UserInfo : ContentPage
    {
        public User user { get; set; }
        HttpClient client;

        public UserInfo()
        {

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Task.Run(() => LoadInfo(6));
        }

        public async void LoadInfo(int id)
        {

            var uri = new Uri(string.Format("http://pinbusterapitest.azurewebsites.net/api/utilizador/" + id, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(content);

                      Device.BeginInvokeOnMainThread(() =>
                    {
                    var layout = new StackLayout();

                    var logo = new Image { Aspect = Aspect.AspectFit };
                    logo.Source = ImageSource.FromResource("PinBuster.microsoft.png");
                    logo.RelScaleTo(0.3);

                    var photo = new Image { Aspect = Aspect.AspectFit };
                    photo.Source = user.imagem;
                    photo.WidthRequest = 150;
                    photo.HeightRequest = 150;

                    var name = new Label { Text = user.nome, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand };

                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    grid.Children.Add(new Label { Text = "5 Followers" }, 1, 0);
                    grid.Children.Add(new Label { Text = "2 Followed" }, 2, 0);
                    grid.Children.Add(new Label { Text = "15 Messages" }, 3, 0);

                    layout.Children.Add(logo);
                    layout.Children.Add(new BoxView() { Color = Color.Gray, HeightRequest = 2 });
                    layout.Children.Add(photo);
                    layout.Children.Add(new BoxView() { Color = Color.Gray, HeightRequest = 2 });
                    layout.Children.Add(name);
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