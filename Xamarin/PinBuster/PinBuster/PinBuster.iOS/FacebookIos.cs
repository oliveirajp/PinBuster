using System;
using Xamarin.Auth;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using PinBuster;
using PinBuster.iOS;
using Newtonsoft.Json.Linq;

[assembly: ExportRenderer(typeof(PinBuster.Login), typeof(FacebookIos))]

namespace PinBuster.iOS
{
	public class FacebookIos : PageRenderer
	{

		bool IsShown;

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);

			// Fixed the issue that on iOS 8, the modal wouldn't be popped.
			// url : http://stackoverflow.com/questions/24105390/how-to-login-to-facebook-in-xamarin-forms
			if (!IsShown)
			{

				IsShown = true;

				var auth = new OAuth2Authenticator(
					clientId: "536841529832251",
					scope: "",
					authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
					redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")
				); // the redirect URL for the service




				auth.Completed += async (sender, eventArgs) =>
				{
					// We presented the UI, so it's up to us to dimiss it on iOS.
					//App.Instance.SuccessfulLoginAction.Invoke();

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

						await App.NavigateToProfile(string.Format(name), string.Format(id));
					}
					else {
						// The user cancelled
					}
				};

				PresentViewController(auth.GetUI(), true, null);

			}

		}
	}
}