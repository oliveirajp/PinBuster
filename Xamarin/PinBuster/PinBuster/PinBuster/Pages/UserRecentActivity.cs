using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinBuster.Models;
using System.Linq;
using PinBuster.Pages;
using static PinBuster.Data.PinsManager;

namespace PinBuster
{
    class UserRecentActivity : ContentPage
    {
        HttpClient client;

        PinBuster.Data.Utilities u;
        Pins_L lista_mensagens;
        List<Models.Pin> pinList;

        public UserRecentActivity(string userID)
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            u = new PinBuster.Data.Utilities();
            LoadAndDisplayPins(userID);
        }

        private async void LoadAndDisplayPins(string userID)
        {
            pinList = await GetUserRecentActivity(userID);
            pinList = Enumerable.Reverse(pinList).Take(6).Reverse().ToList();

            foreach (PinBuster.Models.Pin x in pinList)
            {
                if (x.Raio != 0)
                {
                    if (Data.CalcDistance.findDistance(PinBuster.App.lat, PinBuster.App.lng, x.Latitude, x.Longitude) * 1000 <= x.Raio)
                        x.Visivel = 1;
                    else
                        x.Visivel = 0;
                }
            }

            var map = new RecentActMapPage(pinList);
            Content = map.layout;
        }

        public async Task<List<Pin>> GetUserRecentActivity(string userID)
        {
            var mensagensUri = new Uri(string.Format("https://pinbusterapi.azurewebsites.net/api/mensagem/" + userID, string.Empty));

            var response = await client.GetAsync(mensagensUri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var content = await response.Content.ReadAsStringAsync();
                    lista_mensagens = (Pins_L)Newtonsoft.Json.JsonConvert.DeserializeObject(content, typeof(Pins_L));

                    List<PinBuster.Models.Pin> myList = new List<PinBuster.Models.Pin>(App.listView._viewModel.All_M);


                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"@@@@@@@@@@@@@@-ERROR {0}", ex.Message);
                }
            }

                return lista_mensagens.data;
        }

        //public async void getUserRecentActivity(String id)
        //{
        //    var mensagensUri = new Uri(string.Format("https://pinbusterapi.azurewebsites.net/api/mensagem/" + id, string.Empty));

        //    try
        //    {
        //        var response = await client.GetAsync(mensagensUri);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
                    
        //            mensagens = (Mensagem_L)Newtonsoft.Json.JsonConvert.DeserializeObject(content, typeof(Mensagem_L));
        //            var lastSix = mensagens.data;
        //            lastSix = Enumerable.Reverse(lastSix).Take(6).Reverse().ToList(); 
                   
                    

        //            Device.BeginInvokeOnMainThread(() =>
        //            {
        //                //foreach(Mensagem msg in lastSix)
        //                //{
        //                //    layout.Children.Add(new Label { Text = msg.data.Substring(3, 12), FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
        //                //    layout.Children.Add(new Label { Text = msg.localizacao, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
        //                //    layout.Children.Add(new Label { Text = msg.conteudo, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand });
        //                //    layout.Children.Add(new BoxView() { Color = Color.FromHex("1b434c"), HeightRequest = 2 });
        //                //}
        //                var map = new RecentActMapPage(lastSix);
        //                Content = map.layout;
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(@"				ERROR {0}", ex.Message);
        //    }
        //}

    }
}
