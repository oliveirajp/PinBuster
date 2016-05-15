using System;
using System.Collections.Generic;
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
            InitializeComponent();
            NomeUser.Text = "Hello " + text;
            ProfilePic.Source = "http://graph.facebook.com/" + id + "/picture?type=square";

            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("nome", text));
            postData.Add(new KeyValuePair<string, string>("imagem", "http://graph.facebook.com/" + id + "/picture?type=square"));
            postData.Add(new KeyValuePair<string, string>("raio", "2"));


            var task = App.NavigateToApp();
            



            /*PUT ON THE DATABASE*/

            /*
            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("http://pinbusterapitest.azurewebsites.net");
                var content = new System.Net.Http.FormUrlEncodedContent(postData);


                var result = client.PostAsync("api/utilizador", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

               // NomeUser.Text = resultContent;
            }

            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("http://pinbusterapitest.azurewebsites.net");
               // var content = new System.Net.Http.FormUrlEncodedContent(postData);


                var result = client.GetAsync("api/utilizador/6").Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

              //  NomeUser.Text = resultContent;
            }

    */

        }

        public async void NavigateLogout(object sender, EventArgs e)
        {
            //IDeleteCredentials DeleteCredentials = DependencyService.Get<IDeleteCredentials>();
            // DeleteCredentials.IDeleteCredentials();
            App.NavigateToApp();

            NavigationPage.PopToRootAsync();


            //await App.Current.MainPage.Navigation.PushAsync(new LoginPage());



            // await Navigation.PushAsync(new LoginPage());





        }




    }
}
