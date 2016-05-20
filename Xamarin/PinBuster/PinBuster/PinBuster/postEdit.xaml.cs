using PinBuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster
{
    public partial class postEdit : ContentPage
    {
        double currentLat = 0;
        double currentLon = 0;


        public postEdit(Pin pin)
        {
            if (pin == null)
            {
                System.Diagnostics.Debug.WriteLine("Pin is null on postEdit");
                throw new ArgumentNullException();
            }
            else
            {
                InitializeComponent();
                currentLat = pin.latitude;
                currentLon = pin.longitude;
                System.Diagnostics.Debug.WriteLine(pin.mensagem);
                PostMessage.Text = pin.mensagem;
                CategoryPicker.SelectedIndex = 0;
                SliderRadius.Value = pin.raio;

            }
       
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("face_id", "1"));
            postData.Add(new KeyValuePair<string, string>("nome", "dummy"));
            postData.Add(new KeyValuePair<string, string>("latitude", currentLat.ToString()));
            postData.Add(new KeyValuePair<string, string>("longitude", currentLon.ToString()));
            postData.Add(new KeyValuePair<string, string>("data", "20130210 11:11:11 PM"));
            postData.Add(new KeyValuePair<string, string>("tempo_limite", "30"));
            postData.Add(new KeyValuePair<string, string>("raio", SliderRadius.Value.ToString()));
            postData.Add(new KeyValuePair<string, string>("utilizador_id", "0"));
            postData.Add(new KeyValuePair<string, string>("conteudo", PostMessage.Text));
            postData.Add(new KeyValuePair<string, string>("localizacao", "leiden"));
            postData.Add(new KeyValuePair<string, string>("categoria", CategoryPicker.SelectedIndex.ToString()));
            postData.Add(new KeyValuePair<string, string>("imagem", "dummy"));



            using (var client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri("http://pinbusterapitest.azurewebsites.net");
                var content = new System.Net.Http.FormUrlEncodedContent(postData);


                var result = client.PostAsync("api/mensagem", content).Result;
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

