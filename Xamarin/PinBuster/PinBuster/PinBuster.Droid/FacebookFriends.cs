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

[assembly: ExportRenderer(typeof(PinBuster.FacebookFriends), typeof(PinBuster.Droid.FacebookFriends))]

namespace PinBuster.Droid
{
    [Activity(Label = "FacebookFriends")]
    public class FacebookFriends : PageRenderer
    {
        public FacebookFriends()
        {
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

                    var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, eventArgs.Account);
                    var response = await request.GetResponseAsync();
                    var obj = JObject.Parse(response.GetResponseText());

                    var id = obj["id"].ToString().Replace("\"", "");
                    var name = obj["name"].ToString().Replace("\"", "");

                    var request2 = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me/friends"), null, eventArgs.Account);
                    var response2 = await request2.GetResponseAsync();
                    var obj2 = JObject.Parse(response2.GetResponseText());

                    var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                    var filename = Path.Combine(documents, "followers2.txt");
                    File.WriteAllText(filename, obj2.ToString());




                    System.Diagnostics.Debug.WriteLine(obj2.ToString());

                    //activity.Finish();

                    // activity.Finish();

                    //await App.NavigateToProfile(string.Format(name), string.Format(id));

                    //System.Diagnostics.Debug.WriteLine(obj.ToString());

                }
                else
                {
                    await App.NavigateToProfile("Usuário Cancelou o login", "");
                }
            };

            activity.StartActivityForResult(auth.GetUI(activity),1);



        }
    }
}