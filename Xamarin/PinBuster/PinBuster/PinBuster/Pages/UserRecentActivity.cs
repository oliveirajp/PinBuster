using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinBuster.Models;
using System.Linq;

namespace PinBuster
{
    class UserRecentActivity : ContentPage
    {
        Mensagem_L mensagens;
        HttpClient client;

        public UserRecentActivity()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

            Task.Run(() => getUserRecentActivity(6));
        }

        public async void getUserRecentActivity(int id)
        {
            var mensagensUri = new Uri(string.Format("http://pinbusterapitest.azurewebsites.net/api/mensagem/" + id, string.Empty));

            try
            {
                var response = await client.GetAsync(mensagensUri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    
                    mensagens = (Mensagem_L)Newtonsoft.Json.JsonConvert.DeserializeObject(content, typeof(Mensagem_L));
                    var lastSix = mensagens.data;
                    lastSix = Enumerable.Reverse(lastSix).Take(6).Reverse().ToList(); 
                   

                    var layout = new StackLayout();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach(Mensagem msg in lastSix)
                        {
                            layout.Children.Add(new Label { Text = msg.data.Substring(3, 12), FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
                            layout.Children.Add(new Label { Text = msg.localizacao, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
                            layout.Children.Add(new Label { Text = msg.conteudo, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
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
