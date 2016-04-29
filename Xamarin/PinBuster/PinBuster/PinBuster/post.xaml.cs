using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster
{
    public partial class post : ContentPage
    {
        public post()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("latitude", "0"));
            postData.Add(new KeyValuePair<string, string>("longitude", "0"));
            postData.Add(new KeyValuePair<string, string>("data", "20130210 11:11:11 PM"));
            postData.Add(new KeyValuePair<string, string>("tempo_limite", "30"));
            postData.Add(new KeyValuePair<string, string>("raio", "50"));
            postData.Add(new KeyValuePair<string, string>("utilizador_id", "0"));
            postData.Add(new KeyValuePair<string, string>("conteudo", PostMessage.Text));
            postData.Add(new KeyValuePair<string, string>("localizacao", "leiden"));
            postData.Add(new KeyValuePair<string, string>("categoria","xD"));

            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("http://pinbusterapitest.azurewebsites.net");
                var content = new System.Net.Http.FormUrlEncodedContent(postData);


                var result = client.PostAsync("api/mensagem", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                outputpost.Text = resultContent;
            }

            PostMessage.Text = String.Empty;
        }
    }

}

