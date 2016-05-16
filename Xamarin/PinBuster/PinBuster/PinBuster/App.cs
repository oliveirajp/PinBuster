using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PinBuster.Data;
using PinBuster.Pages;
using PinBuster;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PinBuster
{
    public class App : Application
    {
      

        private readonly static Locator _locator = new Locator();

        public static PinsManager PinsManager { get; private set; }

        public static Locator Locator
        {
            get { return _locator; }
        }

        public IGetCurrentPosition loc;

        public interface ISaveCredentials
        { void ISaveCredentials(string userid, string username);   }

        public interface IGetCredentials
        {String[] IGetCredentials(); }

        public interface IDeleteCredentials
        {void IDeleteCredentials();  }

        public interface IFacebookLogin
        {void IFacebookLogin();}

        public async static Task NavigateToProfile(string name, string id)
        {
            
            //saving credentials
            ISaveCredentials saveCredentials = DependencyService.Get<ISaveCredentials>();
            saveCredentials.ISaveCredentials(id, name);

            //how to get credentials
            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userID = getCredentials.IGetCredentials()[0];
            String userName = getCredentials.IGetCredentials()[1];

            await App.Current.MainPage.Navigation.PushAsync(new TestPage(name, id));

           // IGetCurrentPosition loctemp;
            //loctemp = DependencyService.Get<IGetCurrentPosition>();
            //PinsManager = new PinsManager();

            //  await App.Current.MainPage.Navigation.PushAsync(new MasterDetail(loctemp));
        }

        public async static Task NavigateToApp()
        {
            IGetCurrentPosition loctemp;
            loctemp = DependencyService.Get<IGetCurrentPosition>();
            PinsManager = new PinsManager();
            App.Current.MainPage = new MasterDetail(loctemp);
            

           // await App.Current.MainPage.Navigation.PushAsync(new MasterDetail(loctemp));
        }

        IFacebookLogin face;

        public App()
        {
            
            loc = DependencyService.Get<IGetCurrentPosition>();
            // The root page of your application
            PinsManager = new PinsManager();
           

            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userID = null;
            String userName = null;
            if (getCredentials.IGetCredentials() != null)
            {
                userID = getCredentials.IGetCredentials()[0];
                userName = getCredentials.IGetCredentials()[1];
            }
          //  Debug.WriteLine("user id after saving:" + userID);


            if (userID == null)
            {

                MainPage = new NavigationPage(new LoginPage());
            }
            else
                MainPage = new MasterDetail(loc);
        }




        protected override void OnStart()
        {
            // Handle when your app starts
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            
        }
    }
}
