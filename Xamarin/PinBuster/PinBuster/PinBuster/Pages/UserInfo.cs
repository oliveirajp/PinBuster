using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinBuster.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using static PinBuster.App;
using Newtonsoft.Json.Linq;


namespace PinBuster
{

    class UserInfo : ContentPage
    {
        public User user { get; set; }
        HttpClient client;
        public static StackLayout layoutPublic;
        public static Label labelPublic;

        public static String resultPublicString;
        public static UserInfo userinfoPublic;
        public UserInfo()
        {
            labelPublic = new Label { IsVisible = false, Text = "" };

            resultPublicString = "";
            userinfoPublic = this;
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            Task.Run(() => LoadInfo(6));
        }

        public async void LoadInfo(int id)
        {


            var uri = new Uri(string.Format("http://pinbusterapitest.azurewebsites.net/api/utilizador/" + id, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(content);

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var layout = new StackLayout() { VerticalOptions = LayoutOptions.FillAndExpand };

                        //var logo = new Image { Aspect = Aspect.AspectFit };
                        //logo.Source = ImageSource.FromResource("PinBuster.microsoft.png");
                        //logo.RelScaleTo(0.3);

                        var bLogout = new Button { Text = "Logout", TextColor = Color.White, BackgroundColor = Color.FromHex("#FF464D"), VerticalOptions = LayoutOptions.End };
                        var bFollowers = new Button { Text = "Followers from Facebook", TextColor = Color.White, BackgroundColor = Color.FromHex("#3b5998") };
                        var bShare = new Button { Text = "Share", TextColor = Color.White, BackgroundColor = Color.FromHex("#3b5998") };

                        bShare.Clicked += delegate
                        {
                            IFacebookShare FacebookShare = DependencyService.Get<IFacebookShare>();
                            FacebookShare.IFacebookShare("secret message", "Porto");


                        };

                            bLogout.Clicked += async delegate
                        {
                            // CrossShare.Current.ShareLink("http://motzcod.es", "I just posted a scret message on Pinbuster", "Pinbuster");


                            IDeleteCredentials DeleteCredentials = DependencyService.Get<IDeleteCredentials>();
                            DeleteCredentials.IDeleteCredentials();
                            DisplayAlert("Alert", "Reinicia a app para acabar o logout", "OK");

                            // await App.Current.MainPage.Navigation.PushAsync(new LoginPage());

                        };


                        //how to get credentials
                        IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
                        String userID = getCredentials.IGetCredentials()[0];
                        String userName = getCredentials.IGetCredentials()[1];

                        var photo = new Image { Aspect = Aspect.AspectFit };
                        photo.Source = "http://graph.facebook.com/" + userID + "/picture?width=200&height=200";
                        photo.WidthRequest = 150;
                        photo.HeightRequest = 150;

                        var name = new Label { Text = userName, FontSize = 30, HorizontalOptions = LayoutOptions.CenterAndExpand };

                        var grid = new Grid();
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        grid.Children.Add(new Label { Text = "5 Followers" }, 1, 0);
                        grid.Children.Add(new Label { Text = "2 Followed" }, 2, 0);
                        grid.Children.Add(new Label { Text = "15 Messages" }, 3, 0);

                        // layout.Children.Add(logo);
                        layout.Children.Add(new BoxView() { Color = Color.Gray, HeightRequest = 2 });
                        layout.Children.Add(photo);
                        layout.Children.Add(new BoxView() { Color = Color.Gray, HeightRequest = 2 });
                        layout.Children.Add(name);
                        layout.Children.Add(grid);

                        layout.Children.Add(bFollowers);
                        layout.Children.Add(bLogout);
                        layout.Children.Add(bShare);




                        Content = layout;


                        bFollowers.Clicked += async delegate
                        {


                            if (Device.OS == TargetPlatform.Windows)
                            {
                                IFacebookFriends FacebookFriends = DependencyService.Get<IFacebookFriends>();
                                FacebookFriends.IFacebookFriends(labelPublic);
                                Task t = new Task(InsertFollowersWindowsPhone);

                                t.Start();
                            }
                            else
                            {
                                layoutPublic = layout;

                                Task t = new Task(InsertFollowers);
                                await Navigation.PushModalAsync(new FacebookFriends(layout));
                                t.Start();


                            }
                        };
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
            // Debug.WriteLine("result:" + result);



            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(() =>
            {
                userinfoPublic.Navigation.PushAsync(new ListFollowers(result));

                /*
                try
                {
                    
                   
                    JObject friendListJson = JObject.Parse(result);
                    List<string> strinArrayList = new List<string>();

                    foreach (var friend in friendListJson["data"].Children())
                    {
                        String id = friend["id"].ToString().Replace("\"", "");
                        String name = friend["name"].ToString().Replace("\"", "");
                        var nameLabel = new Label { Text = name, FontSize = 20, HorizontalOptions = LayoutOptions.CenterAndExpand };
                        Button button=new Button { Text="Follow", HorizontalOptions = LayoutOptions.CenterAndExpand};
                        button.Clicked += delegate
                        {
                            var postData = new List<KeyValuePair<string, string>>();
                            postData.Add(new KeyValuePair<string, string>("follower", id));
                            postData.Add(new KeyValuePair<string, string>("followed", userid));

                
                                    using (var client = new System.Net.Http.HttpClient())
                                    {
                                        client.BaseAddress = new Uri("http://pinbusterapitest.azurewebsites.net");
                                        var content = new System.Net.Http.FormUrlEncodedContent(postData);
                                        var result2 = client.PostAsync("api/follow", content).Result;
                                        string resultContent = result2.Content.ReadAsStringAsync().Result;
                                        Debug.WriteLine(resultContent);
                                        // NomeUser.Text = resultContent;
                                    }
                                

                            
                        };
                        layoutPublic.Children.Add(nameLabel);
                        layoutPublic.Children.Add(button);
                    }
                    tcs.SetResult(null);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                */
            });


        }
    }
}