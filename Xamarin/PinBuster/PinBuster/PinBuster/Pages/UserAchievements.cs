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
            var achievementsUri = new Uri(string.Format("http://pinbusterapitest.azurewebsites.net/api/achievement/" + id, string.Empty));

            try
            {
                var response = await client.GetAsync(achievementsUri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    achievements = (Achievement_L)Newtonsoft.Json.JsonConvert.DeserializeObject(content, typeof(Achievement_L));
                    var lastSix = achievements.data;
                    lastSix = Enumerable.Reverse(lastSix).Take(6).Reverse().ToList();
                    var num = lastSix.Count;
                    Debug.WriteLine("num: " + num);

                    var grid = new Grid();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        int i = 0;
                        int j = 0;
                        if (num >= 1)
                        {
                            Debug.WriteLine("primeira linha e coluna");
                            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        }
                        if (num >= 2) {
                            Debug.WriteLine("mais uma coluna");
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            i++;
                        }
                        if (num >= 3) {
                            Debug.WriteLine("mais uma coluna");
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                            i++;
                        }
                        if (num >= 4) {
                            Debug.WriteLine("segunda linha");
                            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                            j++;
                        }

                        int x = 0;
                        int y = 0;
                            Debug.WriteLine("dentro do foreach");
                            while (y <= j) {
                                while (x <= i) {
                                foreach (Achievement ach in lastSix)
                                {
                                    double progress = (double)ach.MessagesFound / ach.MessagesNeeded;
                                    var layout = new StackLayout();
                                    var logo = new Image { Aspect = Aspect.AspectFit };
                                    logo.Source = ImageSource.FromResource("PinBuster.achievement-03.png");
                                    layout.Children.Add(logo);
                                    layout.Children.Add(new Label { Text = ach.nome, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
                                    layout.Children.Add(new ProgressBar { Progress = progress });
                                    grid.Children.Add(layout, x, y);
                                    x++;
                                }
                                }
                                x = 0;
                                y++;
                            }

                        Content = grid;
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