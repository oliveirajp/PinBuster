using Facebook;
using Newtonsoft.Json.Linq;
using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using static PinBuster.App;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(FacebookFriends))]

namespace PinBuster.UWP
{
    class FacebookFriends : IFacebookFriends
    {
        StackLayout layoutPublic;

        public void IFacebookFriends(StackLayout layout)
        {
            layoutPublic = layout;
            IFacebookLogin();
        }

        private async void IFacebookLogin()
        {
            //Facebook app id
            var clientId = "536841529832251";
            //Facebook permissions
            var scope = "public_profile, email";

            var redirectUri = "http://www.facebook.com/connect/login_success.html";
            var fb = new FacebookClient();
            Uri loginUrl = fb.GetLoginUrl(new
            {
                client_id = clientId,
                redirect_uri = redirectUri,
                response_type = "token",
                scope = scope
            });

            Uri startUri = loginUrl;
            Uri endUri = new Uri(redirectUri, UriKind.Absolute);

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            await ParseAuthenticationResult(result);
        }

        public async Task ParseAuthenticationResult(WebAuthenticationResult result)
        {
            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.ErrorHttp:
                    Debug.WriteLine("Error");
                    break;
                case WebAuthenticationStatus.Success:
                    var pattern = string.Format("{0}#access_token={1}&expires_in={2}", WebAuthenticationBroker.GetCurrentApplicationCallbackUri(), "(?<access_token>.+)", "(?<expires_in>.+)");
                    var match = Regex.Match(result.ResponseData, pattern);
                    Debug.WriteLine(".." + result.ResponseData.ToString());

                    var access_token = match.Groups["access_token"];
                    var expires_in = match.Groups["expires_in"];

                    Debug.WriteLine("token: " + access_token.Value.ToString());
                    // var fb = new FacebookClient(access_token.Value);


                    var match2 = Regex.Match(result.ResponseData, "(?:=)(.*)(?:&)");
                    string token = match2.ToString().Substring(1);
                    token = token.Remove(token.Length - 1);
                    Debug.WriteLine(".." + token);


                    GetData(token, "me?fields=friends");
                    //AccessToken = access_token.Value;
                    //TokenExpiry = DateTime.Now.AddSeconds(double.Parse(expires_in.Value));

                    break;
                case WebAuthenticationStatus.UserCancel:
                    Debug.WriteLine("Operation aborted");
                    break;
                default:
                    break;
            }
        }

        public async void GetData(String AccessToken, String task)
        {
            FacebookClient fbclient = new FacebookClient(AccessToken);
            try
            {

                using (var httpClient = new HttpClient())
                {
                    var json2 = await httpClient.GetStringAsync("https://graph.facebook.com/me/friends?access_token="+ AccessToken);
                    

                    //Debug.WriteLine("datafromjson" + json2.ToString());
                    //var jsonObject = Windows.Data.Json.JsonObject.Parse(json2);
                    JObject friendListJson = JObject.Parse(json2.ToString());
                    List<string> strinArrayList = new List<string>();
/*
                    foreach (var friend in friendListJson["data"].Children())
                    {
                        String id=friend["id"].ToString().Replace("\"", "");
                        String name=friend["name"].ToString().Replace("\"", "");
                        var nameLabel = new Label { Text = name, FontSize = 20, HorizontalOptions = LayoutOptions.CenterAndExpand };
                        layoutPublic.Children.Add(nameLabel);
                    }
                    */

                   // Debug.WriteLine("name:" + jsonName.ToString());
                   // Now parse with JSON.Net
                }

                
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error:" + e.ToString());
            }
        }

        
    }
}
