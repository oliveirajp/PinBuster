using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PinBuster.Droid;
using static PinBuster.App;
using Xamarin.Auth;
using Xamarin.Forms;
using System.IO;
using Org.Json;
using System.Net.Http;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(Credentials))]
[assembly: Xamarin.Forms.Dependency(typeof(GetCredentials))]
[assembly: Xamarin.Forms.Dependency(typeof(DeleteCredentials))]
[assembly: Xamarin.Forms.Dependency(typeof(FacebookShareInterface))]



[assembly: Dependency(typeof(SaveAndLoad))]


namespace PinBuster.Droid
{
    class Credentials : ISaveCredentials
    {

        public void ISaveCredentials(string userId, string userName, string accessToken)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("UserID", userId);
                account.Properties.Add("accessToken", accessToken);


                System.Diagnostics.Debug.WriteLine(account.Username);

                AccountStore.Create(Forms.Context).Save(account, "PinBuster");

                System.Diagnostics.Debug.WriteLine("Saved");
                String novo = AccountStore.Create(Forms.Context).FindAccountsForService("PinBuster").FirstOrDefault().Username;
                //System.Diagnostics.Debug.WriteLine("novo:" + novo);

            }
            else
                System.Diagnostics.Debug.WriteLine("else");

        }

        
    }

    class GetCredentials : IGetCredentials
    {


        string[] IGetCredentials.IGetCredentials()
        {
            if (UserName == null)
                return null;
            else
            {
                //System.Diagnostics.Debug.WriteLine("in eleseeeeeeeeeeeeeeee");
                String[] array = { UserID, UserName, AccessToken };
              //  System.Diagnostics.Debug.WriteLine("nameee:" + UserName);
               // System.Diagnostics.Debug.WriteLine("idddee:" + UserID);


                return array;
            }
        }

        public string UserName
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService("PinBuster").FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }
        public string UserID
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService("PinBuster").FirstOrDefault();
                return (account != null) ? account.Properties["UserID"] : null;
            }
        }

        public string AccessToken
        {
            get
            {
                var account = AccountStore.Create(Forms.Context).FindAccountsForService("PinBuster").FirstOrDefault();
                return (account != null) ? account.Properties["accessToken"] : null;
            }
        }
    }

    class DeleteCredentials : IDeleteCredentials
    {
        public void IDeleteCredentials()
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService("PinBuster").FirstOrDefault();
            System.Diagnostics.Debug.WriteLine("here");

            if (account != null)
            {
                AccountStore.Create(Forms.Context).Delete(account, "PinBuster");
            }
        }

    }



    
        public class SaveAndLoad : ISaveAndLoad
        {
            public void SaveText(string filename, string text)
            {
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                System.IO.File.WriteAllText(filePath, text);
          
            }
            public string LoadText(string filename)
            {
                var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, filename);
                return System.IO.File.ReadAllText(filePath);
            }

        public void DeleteFile(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, filename);
            System.IO.File.Delete(filePath);
        }
    }



    class FacebookShareInterface : IFacebookShare
    {
        public void IFacebookShare(String place, String message)
        {
            IFacebookShareAsync(place, message);
        }
        private async void IFacebookShareAsync(String place, String message)
        {
            var activity = Xamarin.Forms.Forms.Context as Activity;
            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();

            String accessTokenSaves = getCredentials.IGetCredentials()[2];

            // String urlString = "https://graph.facebook.com/me/objects/pinbuster:secret_message?object= {\"og:url\":\"https:%2F%2Fwww.facebook.com%2FPinBusterApp%2F\"%2C\"og:title\":\"PinBuster App\"%2C\"og:type\":\"pinbuster:secret_message\"%2C\"og:description\":\"I just posted a secret message in Porto\"%2C\"fb:app_id\":536841529832251}&access_token=" + accessTokenSaves;

            String urlString = "https://graph.facebook.com/me/objects/pinbuster:secret_message";
            HttpClient httpClient = new HttpClient();
            String streamAsync = "";
            String sJson = " {\"og:url\":\"https://www.facebook.com/PinBusterApp/\",\"og:title\":\"PinBuster App\",\"og:type\":\"pinbuster:secret_message\",\"og:description\":\"I just posted a " + place + " in " + message + "\",\"fb:app_id\":536841529832251}";
            JSONObject json = new JSONObject(sJson);
            var parameters = new Dictionary<string, string>();
            parameters.Add("object", sJson);
            try
            {
                Account a = AccountStore.Create().FindAccountsForService("Facebook").First();


                // IDictionary<String, String> dic = new IDictionary<String, String>();
            
                
                var request = new OAuth2Request("Post", new Uri("https://graph.facebook.com/me/objects/pinbuster:secret_message"), parameters, a);
                var response = await request.GetResponseAsync();

                System.Diagnostics.Debug.WriteLine("result:" + response.ToString());
                


            }
            catch
            {

                var auth = new OAuth2Authenticator(
               clientId: "536841529832251",
               scope: "",
               authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
               redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));
                auth.Completed += async (sender, eventArgs) =>
                {
                    if (eventArgs.IsAuthenticated)
                    {
                        AccountStore.Create().Save(eventArgs.Account, "Facebook");
                    
                        var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                        var expiresIn = Convert.ToDouble(eventArgs.Account.Properties["expires_in"]);
                        var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);
                        parameters.Add("object", sJson);

                        var request = new OAuth2Request("Post", new Uri("https://graph.facebook.com/me/objects/pinbuster:secret_message"), parameters, eventArgs.Account);
                        var response = await request.GetResponseAsync();
                        System.Diagnostics.Debug.WriteLine("result:" + response.ToString());

                    }
                    else
                    {
                        await App.NavigateToApp();
                    }
                };

                activity.StartActivityForResult(auth.GetUI(activity), 1);

            }

        }


    }
}

