

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
using Xamarin.Forms;
using Xamarin.Auth;
using Xamarin.Forms.Platform.Android;
using Newtonsoft.Json.Linq;

[assembly: ExportRenderer(typeof(PinBuster.Login), typeof(PinBuster.Droid.FacebookAndroid))]

namespace PinBuster.Droid
{
    [Activity(Label = "FacebookAndroid")]
    public class FacebookAndroid : PageRenderer
    {
        public FacebookAndroid()
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
                    System.Diagnostics.Debug.WriteLine("expires in:" + expiresIn.ToString());

                    System.Diagnostics.Debug.WriteLine("tempo"+expiryDate.ToString());

                    var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, eventArgs.Account);
                    var response = await request.GetResponseAsync();
                    var obj = JObject.Parse(response.GetResponseText());

                    var id = obj["id"].ToString().Replace("\"", "");
                    var name = obj["name"].ToString().Replace("\"", "");

                    await App.NavigateToProfile(string.Format(name), string.Format(id));

                    System.Diagnostics.Debug.WriteLine(obj.ToString());

                }
                else
                {
                   // await App.NavigateToProfile("Usuário Cancelou o login", "");
                }
            };

            activity.StartActivity(auth.GetUI(activity));
            



        }
    }
}