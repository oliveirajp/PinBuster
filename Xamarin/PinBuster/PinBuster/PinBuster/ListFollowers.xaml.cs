using Newtonsoft.Json.Linq;
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
    public partial class ListFollowers : ContentPage
    {
        private string result;


        public ListFollowers(string result)
        {
            NavigationPage.SetHasNavigationBar(this, false);


            InitializeComponent();
            Debug.WriteLine("recieved" + result);
            this.result = result;
            BackButtonFacebook.HorizontalOptions = LayoutOptions.End;
            BackButtonFacebook.Clicked += delegate
            {
                App.NavigateToApp();
            };

            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userID = getCredentials.IGetCredentials()[0];
            Boolean noUsers = false;
            List<string> followsList = new List<string>();

            using (var clientGet = new System.Net.Http.HttpClient())
            {
                clientGet.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                // var content = new System.Net.Http.FormUrlEncodedContent(postData);
                var resultGet = clientGet.GetAsync("api/follow/" + userID + "?f=follower").Result;
                string resultContentGet = resultGet.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("REcieved from DB" + resultContentGet);
                JObject followersJson = JObject.Parse(resultContentGet);
                if (followersJson["data"].ToString() == "false")
                {
                    noUsers = true;
                }


                foreach (var friend in followersJson["data"].Children())
                {
                    followsList.Add(friend["face_id"].ToString());
                    Debug.WriteLine(friend["face_id"].ToString());
                }
            }
            Debug.WriteLine("followers" + followsList.ToArray().ToString());
            ;

            JObject friendListJson = JObject.Parse(result);
            List<string> strinArrayList = new List<string>();


            {
                userID = getCredentials.IGetCredentials()[0];

                foreach (var friend in friendListJson["data"].Children())
                {
                    String id = friend["id"].ToString().Replace("\"", "");
                    String name = friend["name"].ToString().Replace("\"", "");
                    Debug.WriteLine(name);
                    var nameLabel = new Label
                    {
                        Text = name,
                        FontSize = 20,
                        TextColor = Color.Black
                    };
                    Button button = new Button { Text = "Follow", HorizontalOptions = LayoutOptions.End, HeightRequest = 30 };

                    button.BackgroundColor = Color.FromHex("#8ADD97");

                    Boolean following = false;
                    if (!noUsers)
                    {
                        foreach (String user in followsList)
                        {
                            Debug.WriteLine(user + "-" + id);
                            if (user == id)
                            {
                                button.BackgroundColor = Color.FromHex("#FF464D");

                                button.Text = "Unfollow";
                                following = true;
                            }
                        }
                    }
                    button.Clicked += delegate
                    {
                        var postData = new List<KeyValuePair<string, string>>();
                        postData.Add(new KeyValuePair<string, string>("follower", userID));
                        postData.Add(new KeyValuePair<string, string>("followed", id));

                        if (following)
                        {
                            using (var client = new System.Net.Http.HttpClient())
                            {

                                Debug.WriteLine("here");
                                Debug.WriteLine("treying to delete");

                                client.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                                var result2 = client.DeleteAsync("api/follow/" + userID + "?unfollow=" + id).Result;
                                string resultContent = result2.Content.ReadAsStringAsync().Result;
                                JObject resultContentJson = JObject.Parse(resultContent);
                                Debug.WriteLine("DELETE RESULT:" + resultContent);
                                Debug.WriteLine("api/follow/" + userID + "?unfollow=" + id);
                                Debug.WriteLine("PostResult:" + resultContentJson["data"].ToString());


                                if (resultContentJson["data"].ToString() == "done")
                                {
                                    button.BackgroundColor = Color.FromHex("#8ADD97");

                                    button.Text = "Follow";
                                    following = false;
                                }


                                // NomeUser.Text = resultContent;
                            }
                        }
                        else
                        {
                            using (var client = new System.Net.Http.HttpClient())
                            {
                                Debug.WriteLine("here");
                                client.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                                var content = new System.Net.Http.FormUrlEncodedContent(postData);
                                var result2 = client.PostAsync("api/follow", content).Result;
                                string resultContent = result2.Content.ReadAsStringAsync().Result;
                                Debug.WriteLine("POST RESULT:" + resultContent);

                                if (resultContent == "\"done\"")
                                {
                                    button.BackgroundColor = Color.FromHex("#FF464D");
                                    button.Text = "Unfollow";
                                    following = true;
                                }


                                // NomeUser.Text = resultContent;
                            }
                        }

                    };
                    Image image = new Image { Source = "http://graph.facebook.com/" + id + "/picture?type=square" };
                    image.WidthRequest = 30;
                    image.HeightRequest = 30;


                    StackLayout s = new StackLayout() { HeightRequest = 40, Orientation = StackOrientation.Horizontal, Padding = new Thickness(5, 8, 5, 5) };
                    s.Children.Add(image);
                    s.Children.Add(nameLabel);
                    s.Children.Add(button);
                    Debug.WriteLine("here");
                    // StackLayoutObj.Children.Add(nameLabel);

                    StackLayoutObj.Children.Add(s);
                    // StackLayoutObj.Children.Add(new Label() { Text = "novo", FontSize = 20 });




                    /*  StackLayoutObj.Children.Add(relativeLayout);
                      StackLayoutObj.Children.Add(relativeLayout);
                      StackLayoutObj.Children.Add(relativeLayout);
                      StackLayoutObj.Children.Add(relativeLayout);
                      StackLayoutObj.Children.Add(relativeLayout);
                      StackLayoutObj.Children.Add(relativeLayout);
                      */

                }

            }
        }

    }
}