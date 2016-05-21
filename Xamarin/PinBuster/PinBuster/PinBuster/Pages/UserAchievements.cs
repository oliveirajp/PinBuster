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

            Task.Run(() => getUserAchievements(6));
        }

        public async void getUserAchievements(int id)
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

                    var layout = new StackLayout();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (Achievement ach in lastSix)
                        {
                            layout.Children.Add(new Label { Text = ach.nome, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
                            layout.Children.Add(new Label { Text = "MessagesNeeded: " + ach.MessagesNeeded, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
                            layout.Children.Add(new Label { Text = "MessagesFound: " + ach.MessagesFound, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
                            layout.Children.Add(new BoxView() { Color = Color.FromHex("1b434c"), HeightRequest = 2 });
                        }

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