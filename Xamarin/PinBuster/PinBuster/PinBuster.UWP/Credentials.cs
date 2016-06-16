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
[assembly: Xamarin.Forms.Dependency(typeof(SaveAndLoad))]




namespace PinBuster.UWP
{
    class SaveCredentials : ISaveCredentials
    {
        public void ISaveCredentials(string userid, string username, string accessToken)
        {
            /* var vault = new Windows.Security.Credentials.PasswordVault();
             vault.Add(new Windows.Security.Credentials.PasswordCredential(
                 "My App", username, ""));
                 */
            Debug.WriteLine("here:");

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["userID"] = userid;
            localSettings.Values["userName"] = username;
            Debug.WriteLine("acessToken:" + accessToken);
            localSettings.Values["accessToken"] = accessToken;


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
            Object access = localSettings.Values["accessToken"];

            if (id == null)
                return null;
            String userID = id.ToString();
            String userName = name.ToString();
            String accessToken = access.ToString();

            Debug.WriteLine("userIDStored:" + userID);
            String[] array = { userID, userName, accessToken };
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

    class SaveAndLoad : ISaveAndLoad
    {
        public void DeleteFile(string filename)
        {
            throw new NotImplementedException();
        }

        public string LoadText(string filename)
        {

            Windows.Storage.StorageFolder storageFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =   storageFolder.GetFileAsync(filename).GetResults();
            string text =  Windows.Storage.FileIO.ReadTextAsync(sampleFile).GetResults();
            return text;


        }

        public void SaveText(string filename, string text)
        {
            Windows.Storage.StorageFolder storageFolder =
    Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =
                 storageFolder.GetFileAsync(filename).GetResults();
             Windows.Storage.FileIO.WriteTextAsync(sampleFile, text).GetResults();

        }
    }
}

