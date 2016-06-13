using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PinBuster.iOS;
using static PinBuster.App;
using Xamarin.Auth;
using Xamarin.Forms;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(Credentials))]
[assembly: Xamarin.Forms.Dependency(typeof(GetCredentials))]
[assembly: Xamarin.Forms.Dependency(typeof(DeleteCredentials))]


[assembly: Dependency(typeof(SaveAndLoad))]
namespace PinBuster.iOS
{
	class Credentials : ISaveCredentials
	{

		public void ISaveCredentials(string userId, string userName)
		{
			if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(userName))
			{
				Account account = new Account
				{
					Username = userName
				};
				account.Properties.Add("UserID", userId);
				AccountStore.Create().Save(account, "PinBuster");
			}
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
				String[] array = { UserID, UserName };
				//  System.Diagnostics.Debug.WriteLine("nameee:" + UserName);
				// System.Diagnostics.Debug.WriteLine("idddee:" + UserID);


				return array;
			}
		}

		public string UserName
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService("PinBuster").FirstOrDefault();
				return (account != null) ? account.Username : null;
			}
		}

		public string UserID
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService("PinBuster").FirstOrDefault();
				return (account != null) ? account.Properties["UserID"] : null;
			}
		}
	}

	class DeleteCredentials : IDeleteCredentials
	{
		public void IDeleteCredentials()
		{
			var account = AccountStore.Create().FindAccountsForService("PinBuster").FirstOrDefault();
			if (account != null)
			{
				AccountStore.Create().Delete(account, "PinBuster");
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

}

