using PinBuster.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinBuster
{
    class UserAchievements : ContentPage
    {
        Achievement_L achievements;
        HttpClient client;

        public UserAchievements()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            App.IGetCredentials getCredentials = DependencyService.Get<App.IGetCredentials>();
            String userID = getCredentials.IGetCredentials()[0];

            Task.Run(() => getUserAchievements(userID));
        }

        public async void getUserAchievements(String id)
        {
            var achievementsUri = new Uri(string.Format("http://pinbusterapi.azurewebsites.net/api/achievement/" + id, string.Empty));

            try
            {
                var response = await client.GetAsync(achievementsUri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    achievements = (Achievement_L)Newtonsoft.Json.JsonConvert.DeserializeObject(content, typeof(Achievement_L));
                    var achs = achievements.data;
                    var num = achs.Count;

                    var grid = new Grid();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        int x = 0;
                        int y = 0;
                        for (int i = 1; i < num / 3; i++)
                        {
                            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                        }
                        foreach (Achievement ach in achs) {
                            double progress = (double)ach.MessagesFound / ach.MessagesNeeded;
                            var layout = new StackLayout();
                            var logo = new Image { Aspect = Aspect.AspectFit };
                            logo.Source = ImageSource.FromResource("PinBuster.achievement-03.png");
                            layout.Children.Add(logo);
                            layout.Children.Add(new Label { Text = ach.nome, FontSize = 18, HorizontalOptions = LayoutOptions.CenterAndExpand });
                            layout.Children.Add(new ProgressBar { Progress = progress });
                            grid.Children.Add(layout, x, y);
                            x++;
                            if (x == 3) {
                                x = 0;
                                y++;
                            }
                        }

                        ScrollView scrollView = new ScrollView()
                        {
                            Content = grid
                    };
                        Content = scrollView;
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