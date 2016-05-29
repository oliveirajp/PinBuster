using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using static PinBuster.App;

namespace PinBuster
{
    public partial class TestPage : ContentPage
    {
        public TestPage(string text, string id)
        {
            Debug.WriteLine("id" + id);
            InitializeComponent();
            NomeUser.Text = "Hello " + text;
            ProfilePic.Source = "http://graph.facebook.com/" + id + "/picture?type=square";

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("nome", text));
            postData.Add(new KeyValuePair<string, string>("imagem", "http://graph.facebook.com/" + id + "/picture?type=square"));
            postData.Add(new KeyValuePair<string, string>("raio", "2"));
            postData.Add(new KeyValuePair<string, string>("face_id", id));


            /*PUT ON THE DATABASE*/

            using (var clientGet = new System.Net.Http.HttpClient())
            {
                clientGet.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                // var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var resultGet = clientGet.GetAsync("api/utilizador/" + id).Result;
                string resultContentGet = resultGet.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Already in the DB");

                if (resultContentGet.Length>0)
                {

                }
                else
                {
                    using (var client = new System.Net.Http.HttpClient())
                    {
                        client.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                        var content = new System.Net.Http.FormUrlEncodedContent(postData);
                        var result = client.PostAsync("api/utilizador", content).Result;
                        string resultContent = result.Content.ReadAsStringAsync().Result;
                        Debug.WriteLine("Saved into the DB ");
                        // NomeUser.Text = resultContent;
                    }
                }
                
            }

            

            var task = App.NavigateToApp();

        }

        public async void NavigateLogout(object sender, EventArgs e)
        {
            //IDeleteCredentials DeleteCredentials = DependencyService.Get<IDeleteCredentials>();
            // DeleteCredentials.IDeleteCredentials();
            
            App.NavigateToApp();
            


            //await App.Current.MainPage.Navigation.PushAsync(new LoginPage());



            // await Navigation.PushAsync(new LoginPage());





        }




    }
}
