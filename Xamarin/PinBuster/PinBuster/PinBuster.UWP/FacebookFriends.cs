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
        Label labelPublic;

        public void IFacebookFriends(Label label)
        {
            labelPublic = label;
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

                    Debug.WriteLine("token000" + access_token.Value.ToString());
                    Debug.WriteLine("token000" + access_token.Value.ToString());

                    // var fb = new FacebookClient(access_token.Value);


                    //how to get credentials
                    IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
                    String userID = getCredentials.IGetCredentials()[0];
                    String userName = getCredentials.IGetCredentials()[1];

                    ISaveCredentials saveCredentials = DependencyService.Get<ISaveCredentials>();
                    saveCredentials.ISaveCredentials(userID, userName, access_token.Value.ToString());







                    var match2 = Regex.Match(result.ResponseData, "(?:=)(.*)(?:&)");
                    string token = match2.ToString().Substring(1);
                    token = token.Remove(token.Length - 1);
                    Debug.WriteLine("..." + token);
                    Debug.WriteLine("CREDENTIALS:" + token);


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

                    labelPublic.Text = json2.ToString();
                   
                }

                
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error:" + e.ToString());
            }
        }

        
    }
}
