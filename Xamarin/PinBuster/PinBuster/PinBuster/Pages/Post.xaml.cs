using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster.Pages
{
    public partial class Post : ContentPage
    {
        double currentLat = 0;
        double currentLon = 0;
        String currentTown = "";

        public Post(double lat, double lon, String town)
        {
            currentLat = lat;
            currentLon = lon;
            currentTown = town;
            InitializeComponent();
       
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            /* Check if any input was left blank */ 
            if(PostMessage.Text == null || PostMessage.Text == "" || SliderRadius.Value <= 20 || CategoryPicker.SelectedIndex < 0)
            {
                DisplayAlert("Error", "Please fill in all the fields", "OK");
                return;
            }



            var postData = new List<KeyValuePair<string, string>>();


            App.IGetCredentials getCredentials = DependencyService.Get<App.IGetCredentials>();
            String userID = null;
            String userName = null;
            if (getCredentials.IGetCredentials() != null)
            {
                userID = getCredentials.IGetCredentials()[0];
                userName = getCredentials.IGetCredentials()[1];
            }

           // postData.Add(new KeyValuePair<string, string>("data", DateTime.Now.ToString("ddd MMM dd yyyy HH:mm:ss zzz")));
           // postData.Add(new KeyValuePair<string, string>("data", "20130210 11:11:11 PM"));
            //postData.Add(new KeyValuePair<string, string>("data", DateTime.Now.ToString("yyyymmdd HH:mm:ss tt")));


            int radius = (int)SliderRadius.Value;
            int timeLimit = 0;

            switch (DateUnitPicker.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    timeLimit = (int)TimeLimit.Value;
                    break;
                case 2:
                    timeLimit = (int)TimeLimit.Value*60;
                    break;
                case 3:
                    timeLimit = (int)TimeLimit.Value*60*24;
                    break;
                case 4:
                    timeLimit = (int)TimeLimit.Value*60*24*31;
                    break;
                case 5:
                    timeLimit = (int)TimeLimit.Value*60*24*31*12;
                    break;
            }

            string data = DateTime.Now.ToString("dd-MM-yy HH:mm:ss tt");

            postData.Add(new KeyValuePair<string, string>("face_id", userID));
            postData.Add(new KeyValuePair<string, string>("nome", userName));
            postData.Add(new KeyValuePair<string, string>("latitude", currentLat.ToString()));
            postData.Add(new KeyValuePair<string, string>("longitude", currentLon.ToString()));
            postData.Add(new KeyValuePair<string, string>("data", data));
            postData.Add(new KeyValuePair<string, string>("tempo_limite", timeLimit.ToString()));
            postData.Add(new KeyValuePair<string, string>("raio", radius.ToString()));
            postData.Add(new KeyValuePair<string, string>("utilizador_id", "0"));
            postData.Add(new KeyValuePair<string, string>("conteudo", PostMessage.Text));
            postData.Add(new KeyValuePair<string, string>("localizacao", currentTown));
            postData.Add(new KeyValuePair<string, string>("categoria", CategoryPicker.Items[CategoryPicker.SelectedIndex].ToString()));
            postData.Add(new KeyValuePair<string, string>("imagem", "dummy"));



            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                var content = new System.Net.Http.FormUrlEncodedContent(postData);


                var result = client.PostAsync("api/mensagem", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                outputpost.Text = resultContent;

            }

            PostMessage.Text = String.Empty;
            SliderRadius.Value = 0;
            CategoryPicker.SelectedIndex = -1;
            App.Locator.Map.LoadPins();

        }

        public void OnSliderValueChanged(Object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1);
            SliderRadius.Value = newStep * 1; 

            SliderValue.Text = newStep.ToString("F0");
            SliderValue.Text = "Select the desired radius: " + SliderValue.Text + " m";
         }


        public void OnTimeSliderValueChanged(Object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1);
            TimeLimit.Value = newStep * 1;

            TimeLimitLabel.Text = newStep.ToString("F0");
            TimeLimitLabel.Text = "Time limit: " + TimeLimitLabel.Text ;


        }



    }

}

