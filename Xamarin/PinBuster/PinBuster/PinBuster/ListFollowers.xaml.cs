using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using static PinBuster.App;

namespace PinBuster
{
    public partial class ListFollowers : ContentPage
    {
        private string result;


        public ListFollowers(string result)
        {
            InitializeComponent();
            this.result = result;


            JObject friendListJson = JObject.Parse(result);
            List<string> strinArrayList = new List<string>();


            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userID = getCredentials.IGetCredentials()[0];
            {
                userID = getCredentials.IGetCredentials()[0];

                foreach (var friend in friendListJson["data"].Children())
                {
                    String id = friend["id"].ToString().Replace("\"", "");
                    String name = friend["name"].ToString().Replace("\"", "");
                    var nameLabel = new Label { Text = name, FontSize = 20, HorizontalOptions = LayoutOptions.CenterAndExpand };
                    Button button = new Button { Text = "Follow", HorizontalOptions = LayoutOptions.CenterAndExpand };
                    button.Clicked += delegate
                    {
                        var postData = new List<KeyValuePair<string, string>>();
                        postData.Add(new KeyValuePair<string, string>("follower", id));
                        postData.Add(new KeyValuePair<string, string>("followed", userID));


                        using (var client = new System.Net.Http.HttpClient())
                        {
                            client.BaseAddress = new Uri("http://pinbusterapitest.azurewebsites.net");
                            var content = new System.Net.Http.FormUrlEncodedContent(postData);
                            var result2 = client.PostAsync("api/follow", content).Result;
                            string resultContent = result2.Content.ReadAsStringAsync().Result;
                        //  Debug.WriteLine(resultContent);
                        // NomeUser.Text = resultContent;
                    }
                    };
                    Image image = new Image { Source = "http://graph.facebook.com/" + id + "/picture?type=square" };
                    image.WidthRequest = 30;
                    image.HeightRequest = 30;

                    StackLayoutObj.Children.Add(image);
                    StackLayoutObj.Children.Add(nameLabel);
                    StackLayoutObj.Children.Add(button);

                }

            }
        }
    }
}
