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

[assembly: Xamarin.Forms.Dependency(typeof(Credentials))]
[assembly: Xamarin.Forms.Dependency(typeof(GetCredentials))]
[assembly: Xamarin.Forms.Dependency(typeof(DeleteCredentials))]




namespace PinBuster.Droid
{
    class Credentials : ISaveCredentials
    {

        public void ISaveCredentials(string userId, string userName)
        {
            if (!string.IsNullOrWhiteSpace(userName))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("UserID", userId);

                System.Diagnostics.Debug.WriteLine(account.Username);

                AccountStore.Create(Forms.Context).Save(account, "PinBuster");
                System.Diagnostics.Debug.WriteLine("Saved");
                String novo = AccountStore.Create(Forms.Context).FindAccountsForService("PinBuster").FirstOrDefault().Username;
                System.Diagnostics.Debug.WriteLine("novo:" + novo);

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
                System.Diagnostics.Debug.WriteLine("in eleseeeeeeeeeeeeeeee");
                String[] array = { UserID, UserName };
                System.Diagnostics.Debug.WriteLine("nameee:" + UserName);
                System.Diagnostics.Debug.WriteLine("idddee:" + UserID);


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

}