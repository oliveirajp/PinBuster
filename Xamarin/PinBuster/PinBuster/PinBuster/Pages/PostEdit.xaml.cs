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


            App.IGetCredentials getCredentials = DependencyService.Get<App.IGetCredentials>();
            String userID = null;
            String userName = null;
            if (getCredentials.IGetCredentials() != null)
            {
                userID = getCredentials.IGetCredentials()[0];
                userName = getCredentials.IGetCredentials()[1];
            }

            updateData.Add(new KeyValuePair<string, string>("mensagem_id", pinId));
            updateData.Add(new KeyValuePair<string, string>("face_id", userID));
            updateData.Add(new KeyValuePair<string, string>("nome", userName));
            updateData.Add(new KeyValuePair<string, string>("latitude", currentLat.ToString()));
            updateData.Add(new KeyValuePair<string, string>("longitude", currentLon.ToString()));
            updateData.Add(new KeyValuePair<string, string>("data", "20130210 11:11:11 PM"));
            updateData.Add(new KeyValuePair<string, string>("tempo_limite", "0"));
            updateData.Add(new KeyValuePair<string, string>("raio", SliderRadius.Value.ToString()));
            updateData.Add(new KeyValuePair<string, string>("utilizador_id", "0"));
            updateData.Add(new KeyValuePair<string, string>("conteudo", PostMessage.Text));
            updateData.Add(new KeyValuePair<string, string>("localizacao", currentTown));
            updateData.Add(new KeyValuePair<string, string>("categoria", CategoryPicker.Items[CategoryPicker.SelectedIndex].ToString()));
            updateData.Add(new KeyValuePair<string, string>("imagem", "dummy"));



            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("http://pinbusterapitest.azurewebsites.net");
                var content = new System.Net.Http.FormUrlEncodedContent(updateData);


                var result = client.PutAsync("api/mensagem", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                outputpost.Text = resultContent;
            }

            PostMessage.Text = String.Empty;
            SliderRadius.Value = 0;
            CategoryPicker.SelectedIndex = -1;

        }

        public void OnSliderValueChanged(Object sender, ValueChangedEventArgs e)
        {
            SliderValue.Text = e.NewValue.ToString("F3");
            SliderValue.Text = "Select the desired radius: " + SliderValue.Text + " m";
        }




    }

}

