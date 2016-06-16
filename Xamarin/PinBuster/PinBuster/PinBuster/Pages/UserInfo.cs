using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinBuster.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using static PinBuster.App;
using Newtonsoft.Json.Linq;
using PinBuster.Data;

using System.Net;
using System.IO;
using Java.Net;
using Org.Apache.Http.Client.Methods;
using Org.Apache.Http;
using System.Threading.Tasks;
using System.Net.Http;

namespace PinBuster
{

    class UserInfo : ContentPage
    {
        public ProfileInfo info { get; set; }
        public User user { get; set; }
        HttpClient client;
        public static StackLayout layoutPublic;
        public static Label labelPublic;
        String userID;
        private bool follow;

        public static String resultPublicString;
        public static UserInfo userinfoPublic;
        public UserInfo(string id)
        {
            labelPublic = new Label { IsVisible = false, Text = "" };
            resultPublicString = "";
            userinfoPublic = this;
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Task.Run(() => LoadInfo(id));
        }

        public async void LoadInfo(string id)
        {


            var uri = new Uri(string.Format("http://pinbusterapi.azurewebsites.net/api/perfil_info/" + id, string.Empty));
            var uriUser = new Uri(string.Format("https://pinbusterapi.azurewebsites.net/api/utilizador/" + id, string.Empty));
            var followUri = new Uri(string.Format("https://pinbusterapi.azurewebsites.net/api/follow", string.Empty));

            JArray followArray = null;
            try
            {
                var response = await client.GetAsync(uriUser);
                var responseFollow = await client.GetAsync(followUri);
                if (response.IsSuccessStatusCode && responseFollow.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var followContent = await responseFollow.Content.ReadAsStringAsync();
                    JObject jsonFollow = JObject.Parse(followContent);
                    followArray = JArray.Parse(jsonFollow["data"].ToString());
                    this.user = JsonConvert.DeserializeObject<User>(content);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    info = JsonConvert.DeserializeObject<ProfileInfo>(content);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //how to get credentials
                        IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
                        userID = getCredentials.IGetCredentials()[0];
                        String userName = getCredentials.IGetCredentials()[1];

                        var layout = new StackLayout() { VerticalOptions = LayoutOptions.FillAndExpand };

                        //var logo = new Image { Aspect = Aspect.AspectFit };
                        //logo.Source = ImageSource.FromResource("PinBuster.microsoft.png");
                        //logo.RelScaleTo(0.3);

                        var photo = new Image { Aspect = Aspect.AspectFit };
                        photo.Source = "http://graph.facebook.com/" + user.face_id + "/picture?width=200&height=200";
                        photo.WidthRequest = 150;
                        photo.HeightRequest = 150;

                        var name = new Label { Text = user.nome, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand };

                        var grid = new Grid();
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        grid.Children.Add(new Label { Text = info.nr_followers + " Followers" }, 1, 0);
                        grid.Children.Add(new Label { Text = info.nr_followed + " Followed" }, 2, 0);
                        grid.Children.Add(new Label { Text = info.nr_mensagens + " Pins" }, 3, 0);

                        // layout.Children.Add(logo);
                        layout.Children.Add(new BoxView() { Color = Color.Gray, HeightRequest = 2 });
                        layout.Children.Add(photo);
                        layout.Children.Add(new BoxView() { Color = Color.Gray, HeightRequest = 2 });
                        layout.Children.Add(name);
                        layout.Children.Add(grid);

                        if (userID == user.face_id)
                        {

                            var bLogout = new Button { Text = "Logout", TextColor = Color.White, BackgroundColor = Color.FromHex("#FF464D"), VerticalOptions = LayoutOptions.End };
                            var bFollowers = new Button { Text = "Followers from Facebook", TextColor = Color.White, BackgroundColor = Color.FromHex("#3b5998") };
                            bLogout.Clicked += async delegate
                            {
                                // CrossShare.Current.ShareLink("http://motzcod.es", "I just posted a scret message on Pinbuster", "Pinbuster");


                                IDeleteCredentials DeleteCredentials = DependencyService.Get<IDeleteCredentials>();
                                DeleteCredentials.IDeleteCredentials();
                                await DisplayAlert("Alert", "Restart the app to complete logout", "OK");

                                await App.Current.MainPage.Navigation.PushAsync(new LoginPage());

                            };

                            bFollowers.Clicked += async delegate
                            {


                                if (Device.OS == TargetPlatform.Windows)
                                {
                                    String accessTokenSaves = getCredentials.IGetCredentials()[2];
                                    String urlString = "https://graph.facebook.com/me/friends?access_token=" + accessTokenSaves;
                                    HttpClient httpClient = new HttpClient();
                                    Debug.WriteLine(urlString);
                                    String streamAsync = "";
                                    try
                                    {

                                        streamAsync = httpClient.GetStringAsync(urlString).Result;
                                        Debug.WriteLine(streamAsync);
                                        Task t = new Task(InsertFollowers);
                                        await Navigation.PushModalAsync(new ListFollowers(streamAsync));
                                        t.Start();
                                    }
                                    catch
                                    {
                                        IFacebookFriends FacebookFriends = DependencyService.Get<IFacebookFriends>();
                                        FacebookFriends.IFacebookFriends(labelPublic);
                                        Task t = new Task(InsertFollowersWindowsPhone);
                                        Debug.WriteLine("falied");

                                        t.Start();
                                    }
                                }
                                else
                                {
                                    layoutPublic = layout;
                                    String accessTokenSaves = getCredentials.IGetCredentials()[2];
                                    String urlString = "https://graph.facebook.com/me/friends?access_token=" + accessTokenSaves;
                                    System.Diagnostics.Debug.WriteLine(urlString);

                                    System.Diagnostics.Debug.WriteLine("hereeee");

                                    HttpClient httpClient = new HttpClient();
                                    String streamAsync = "";
                                    try
                                    {
                                        streamAsync = httpClient.GetStringAsync(urlString).Result;
                                        Task t = new Task(InsertFollowers);
                                        await Navigation.PushModalAsync(new ListFollowers(streamAsync));
                                        t.Start();
                                    }
                                    catch
                                    {
                                        Task t = new Task(InsertFollowers);
                                        await Navigation.PushModalAsync(new FacebookFriends(layout));
                                        t.Start();

                                    }
                                }
                            };


                            layout.Children.Add(bFollowers);
                            layout.Children.Add(bLogout);

                        }
                        else
                        {
                            follow = isFollow(followArray);

                            Button buttonFollow = new Button { Text = "Follow", BackgroundColor = Color.FromHex("#8ADD97"), VerticalOptions = LayoutOptions.End };
                            Button buttonUnfollow = new Button { Text = "Unfollow", BackgroundColor = Color.FromHex("#FF464D"), VerticalOptions = LayoutOptions.End };

                            Button bFollow = buttonFollow;
                            if (follow)
                            {
                                bFollow = buttonUnfollow;
                            }
                            
                            bFollow.Clicked += delegate
                            {
                                using (var client2 = new HttpClient())
                                {
                                    client2.BaseAddress = new Uri("https://pinbusterapi.azurewebsites.net");
                                    if (!follow)
                                    {

                                        var utilities = new Utilities();
                                        var values = new Dictionary<string, string>
                                    {
                                        {"follower", userID },
                                        {"followed", user.face_id}
                                    };

                                        var contentFollow = new FormUrlEncodedContent(values);
                                        var result2 = client2.PostAsync("api/follow", contentFollow).Result;
                                        string resultContent = result2.Content.ReadAsStringAsync().Result;
                                        Debug.WriteLine("POST RESULT:" + resultContent);
                                        
                                        follow = true;
                                        bFollow = buttonUnfollow;

                                    }
                                    else
                                    {

                                        var result2 = client2.DeleteAsync("api/follow/" + userID + "?unfollow=" + user.face_id).Result;
                                        string resultContent = result2.Content.ReadAsStringAsync().Result;
                                        JObject resultContentJson = JObject.Parse(resultContent);
                                        
                                        follow = false;
                                        bFollow = buttonFollow;
                                    }
                                }
                            };

                            layout.Children.Add(bFollow);
                        }
                        Content = layout;
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
        }


        static void InsertFollowersWindowsPhone()
        {
            Debug.WriteLine("in insert");

            while (labelPublic.Text == "")
            {
                Debug.WriteLine("result no ciclo" + labelPublic.Text);
                int milliseconds = 100;
                Task.Delay(milliseconds).Wait();
            }
            Debug.WriteLine("666666666666666666666666666666");
            Debug.WriteLine(labelPublic.Text);

            // Debug.WriteLine("result:" + result);



            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(() =>
            {
                // userinfoPublic.DisplayAlert("Alert", "entrou", "OK");
                userinfoPublic.Navigation.PushAsync(new ListFollowers(labelPublic.Text));

            });


        }

        static void InsertFollowers()
        {
            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userid = getCredentials.IGetCredentials()[0];
            ISaveAndLoad FacebookFriends = DependencyService.Get<ISaveAndLoad>();
            //  FacebookFriends.DeleteFile("followers2.txt");
            FacebookFriends.SaveText("followers2.txt", "");
            String result = FacebookFriends.LoadText("followers2.txt");

            while (result == "")
            {
                result = FacebookFriends.LoadText("followers2.txt");
                // Debug.WriteLine("result no ciclo" + result);
                int milliseconds = 100;
                Task.Delay(milliseconds).Wait();
            }
            Debug.WriteLine("666666666666666666666666666666");


            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(() =>
            {
                int milliseconds = 100;
                Task.Delay(milliseconds).Wait();
                userinfoPublic.Navigation.PushAsync(new ListFollowers(result));
            });


        }
        private bool isFollow(JArray array)
        {
            foreach (JObject o in array)
            {
                if (o["follower"].ToString().Equals(userID) && o["followed"].ToString().Equals(user.face_id))
                {
                    return true;
                }
            }
            return false;

        }
    }
}