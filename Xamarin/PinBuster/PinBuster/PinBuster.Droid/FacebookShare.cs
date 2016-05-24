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
using Xamarin.Forms;
using static PinBuster.App;
using Xamarin.Auth;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Platform.Android;
using System.IO;
using System.Json;
using Org.Json;

[assembly: ExportRenderer(typeof(PinBuster.FacebookShare), typeof(PinBuster.Droid.FacebookShare))]

namespace PinBuster.Droid
{
    [Activity(Label = "FacebookShare")]
    public class FacebookShare : PageRenderer
    {
        public FacebookShare()
        {
            String message = "Leiden";
            System.Diagnostics.Debug.WriteLine("android-----------");


            var activity = this.Context as Activity;


            var auth = new OAuth2Authenticator(
                clientId: "536841529832251",
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));



            auth.Completed += async (sender, eventArgs) => {
                if (eventArgs.IsAuthenticated)
                {
                    var accessToken = eventArgs.Account.Properties["access_token"].ToString();
                    var expiresIn = Convert.ToDouble(eventArgs.Account.Properties["expires_in"]);
                    var expiryDate = DateTime.Now + TimeSpan.FromSeconds(expiresIn);
                    String sJson = " {\"og:url\":\"https://www.facebook.com/PinBusterApp/\",\"og:title\":\"PinBuster App\",\"og:type\":\"pinbuster:secret_message\",\"og:description\":\"I just posted a message in "+ message+"\",\"fb:app_id\":536841529832251}";
                    JSONObject json = new JSONObject(sJson);
                    // IDictionary<String, String> dic = new IDictionary<String, String>();
                    var parameters = new Dictionary<string, string>();
                    parameters.Add("object", sJson);

                    var request = new OAuth2Request("Post", new Uri("https://graph.facebook.com/me/objects/pinbuster:secret_message"), parameters, eventArgs.Account);
                    var response = await request.GetResponseAsync();
                    System.Diagnostics.Debug.WriteLine("result:" + response.ToString());

                    var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    var filename = Path.Combine(documents, "facebookshare.txt");
                    File.WriteAllText(filename, "entrou");
                   // await App.Pop();


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