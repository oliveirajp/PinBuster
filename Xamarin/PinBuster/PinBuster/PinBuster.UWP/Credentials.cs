using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PinBuster.App;

[assembly: Xamarin.Forms.Dependency(typeof(SaveCredentials))]
[assembly: Xamarin.Forms.Dependency(typeof(GetCredentials))]
[assembly: Xamarin.Forms.Dependency(typeof(DeleteCredentials))]





namespace PinBuster.UWP
{
    class SaveCredentials : ISaveCredentials
    {
        public void ISaveCredentials(string userid, string username)
        {
            /* var vault = new Windows.Security.Credentials.PasswordVault();
             vault.Add(new Windows.Security.Credentials.PasswordCredential(
                 "My App", username, ""));
                 */
            Debug.WriteLine("here:");

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["userID"] = userid;
            localSettings.Values["userName"] = username;

            Object value = localSettings.Values["userID"];
            Debug.WriteLine("userIDStored:" + value.ToString());


        }
    }
    class GetCredentials : IGetCredentials
    {
        public string[] IGetCredentials()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            Object id = localSettings.Values["userID"];
            Object name = localSettings.Values["userName"];
            if (id == null)
                return null;
            String userID = id.ToString();
            String userName = name.ToString();
            Debug.WriteLine("userIDStored:" + userID);
            String[] array = { userID, userName };
            return array;
        }
    }
    class DeleteCredentials : IDeleteCredentials
    {
        public void IDeleteCredentials()
        {

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["userID"] = null;

        }
    }
}

