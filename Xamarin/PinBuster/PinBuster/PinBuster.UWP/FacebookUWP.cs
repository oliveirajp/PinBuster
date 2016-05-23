using Facebook;
using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using static PinBuster.Login;



[assembly: Xamarin.Forms.Dependency(typeof(FacebookUWP))]



namespace PinBuster.UWP
{
    class FacebookUWP : IFacebookLogin
    {
        void IFacebookLogin.IFacebookLogin()
        {
            // throw new NotImplementedException();

            IFacebookLogin();
        }

        private async void IFacebookLogin()
        {
            //Facebook app id
            var clientId = "536841529832251";
            //Facebook permissions
            var scope = "public_profile, email,user_friends";

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


                    GetData(token, "me");
                    GetData(token, "me/friends");

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
                var res = await fbclient.GetTaskAsync(task);//me/feed
                String data = res.ToString();
                Debug.WriteLine("Data:" + data);

                // var bob = JObject.Parse(data);

                // Debug.WriteLine("Data:" + bob["id"]);
                var json = Windows.Data.Json.JsonObject.Parse(data);
                var jsonId = json["id"];
                String id = jsonId.ToString().Substring(1, jsonId.ToString().Length - 2);
                jsonId = json["name"];
                String name = jsonId.ToString().Substring(1, jsonId.ToString().Length - 2);


                Debug.WriteLine("Profile information:" + name + ";" + id);

                var vault = new PasswordVault();
                Login.authFacebook.LoginName = name;
                Login.authFacebook.LoginID = id;
                Login.loginID = id;
                Login.loginName = name;

                Debug.WriteLine("here in Face");

                /*IDictionary<string, object> o3 = (IDictionary<string, object>)await fbclient.GetTaskAsync(task);
                JsonObject o2 = (JsonObject) await fbclient.GetTaskAsync(task);*/
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error:" + e.ToString());
            }
        }


    }
}