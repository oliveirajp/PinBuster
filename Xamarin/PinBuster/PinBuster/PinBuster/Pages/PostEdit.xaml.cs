using PinBuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster.Pages
{
    public partial class PostEdit : ContentPage
    {
        double currentLat = 0;
        double currentLon = 0;
        string pinId = "";
        String currentTown = "";


        public PostEdit(Pin pin)
        {
            if (pin == null)
            {
                System.Diagnostics.Debug.WriteLine("Pin is null on postEdit");
                throw new ArgumentNullException();
            }
            else
            {
                InitializeComponent();
                currentLat = pin.Latitude;
                currentLon = pin.Longitude;
                PostMessage.Text = pin.Conteudo;
                CategoryPicker.SelectedIndex = 0;
                SliderRadius.Value = pin.Raio;
                pinId = pin.Mensagem_id;
                currentTown = pin.Localizacao;

            }
       
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {

            var updateData = new List<KeyValuePair<string, string>>();

            int timeLimit = 0;

            switch (DateUnitPicker.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    timeLimit = (int)TimeLimit.Value;
                    break;
                case 2:
                    timeLimit = (int)TimeLimit.Value * 60;
                    break;
                case 3:
                    timeLimit = (int)TimeLimit.Value * 60 * 24;
                    break;
                case 4:
                    timeLimit = (int)TimeLimit.Value * 60 * 24 * 31;
                    break;
                case 5:
                    timeLimit = (int)TimeLimit.Value * 60 * 24 * 31 * 12;
                    break;
            }


            App.IGetCredentials getCredentials = DependencyService.Get<App.IGetCredentials>();
            String userID = null;
            String userName = null;
            if (getCredentials.IGetCredentials() != null)
            {
                userID = getCredentials.IGetCredentials()[0];
                userName = getCredentials.IGetCredentials()[1];
            }

            int radius = (int)SliderRadius.Value;

            updateData.Add(new KeyValuePair<string, string>("mensagem_id", pinId));
            updateData.Add(new KeyValuePair<string, string>("face_id", userID));
            updateData.Add(new KeyValuePair<string, string>("nome", userName));
            updateData.Add(new KeyValuePair<string, string>("latitude", currentLat.ToString()));
            updateData.Add(new KeyValuePair<string, string>("longitude", currentLon.ToString()));
            updateData.Add(new KeyValuePair<string, string>("data", "20130210 11:11:11 PM"));
            updateData.Add(new KeyValuePair<string, string>("tempo_limite", timeLimit.ToString()));
            updateData.Add(new KeyValuePair<string, string>("raio", radius.ToString()));
            updateData.Add(new KeyValuePair<string, string>("utilizador_id", "0"));
            updateData.Add(new KeyValuePair<string, string>("conteudo", PostMessage.Text));
            updateData.Add(new KeyValuePair<string, string>("localizacao", currentTown));
            updateData.Add(new KeyValuePair<string, string>("categoria", CategoryPicker.Items[CategoryPicker.SelectedIndex].ToString()));
            updateData.Add(new KeyValuePair<string, string>("imagem", "dummy"));



            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                var content = new System.Net.Http.FormUrlEncodedContent(updateData);


                var result = client.PutAsync("api/mensagem", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;
                System.Diagnostics.Debug.WriteLine("Resposta ao edit post message: " + resultContent);
            }

            PostMessage.Text = String.Empty;
            SliderRadius.Value = 0;
            CategoryPicker.SelectedIndex = -1;

        }

        public void OnSliderValueChanged(Object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1);
            SliderRadius.Value = newStep * 1;

            SliderValue.Text = e.NewValue.ToString("F3");
            SliderValue.Text = "Select the desired radius: " + SliderValue.Text + " m";
        }


        public void OnTimeSliderValueChanged(Object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 1);
            TimeLimit.Value = newStep * 1;

            TimeLimitLabel.Text = newStep.ToString("F0");
            TimeLimitLabel.Text = "Time limit: " + TimeLimitLabel.Text;
        }



    }

}

