using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PinBuster.Data;
using PinBuster.Pages;
using PinBuster;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PinBuster
{
    public class App : Application
    {
      

        private readonly static Locator _locator = new Locator();

        public static PinsManager pinsManager { get; private set; }

        public static Locator Locator
        {
            get { return _locator; }
        }
        
        public static IGetCurrentPosition loc;
        public static double lat, lng;
        public static int screenWidth, screenHeight;
        public static ContentPage mapPage;
        public static MessageListView listView;
        public static string town;
        

        public interface ISaveCredentials
        { void ISaveCredentials(string userid, string username);   }

        public interface IGetCredentials
        {String[] IGetCredentials(); }

        public interface IDeleteCredentials
        {void IDeleteCredentials();  }

        public interface IFacebookLogin
        {void IFacebookLogin();}

        public interface IFacebookFriends
        { void IFacebookFriends(Label label); }

        public interface ISaveAndLoad
        { void SaveText(string filename, string tex);
            string LoadText(string filename);
            void DeleteFile(string filename);
        }

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
            pinsManager = new PinsManager();
            App.Current.MainPage = new MasterDetail();
            

           // await App.Current.MainPage.Navigation.PushAsync(new MasterDetail(loctemp));
        }

        IFacebookLogin face;

        public App()
        {
            
            loc = DependencyService.Get<IGetCurrentPosition>();
            loc.locationObtained += (object sender, ILocationEventArgs e) =>
            {
                lat = e.lat;
                lng = e.lng;
                Locator.Map.LoadPins();
            };
            loc.IGetCurrentPosition();

            // The root page of your application
            pinsManager = new PinsManager();
            mapPage = new MapPage();
            listView = new MessageListView();
           

			/*IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userID = null;
            String userName = null;
            if (getCredentials.IGetCredentials() != null)
            {
                userID = getCredentials.IGetCredentials()[0];
                userName = getCredentials.IGetCredentials()[1];
            }*/
          //  Debug.WriteLine("user id after saving:" + userID);

                MainPage = new MasterDetail();
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
