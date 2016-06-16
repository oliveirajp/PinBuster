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
        private static double oldLat, oldLng;
        public static int screenWidth, screenHeight;
        public static ContentPage mapPage;
        public static MessageListView listView;
        public static string town;
        public static int radius = 10;
        public static bool updatedPins;

        public interface ISaveCredentials
        { void ISaveCredentials(string userid, string username); }

        public interface IGetCredentials
        { String[] IGetCredentials(); }

        public interface IDeleteCredentials
        { void IDeleteCredentials(); }

        public interface IFacebookLogin
        { void IFacebookLogin(); }

        public interface IFacebookFriends
        { void IFacebookFriends(Label label); }

        public interface ISaveAndLoad
        {
            void SaveText(string filename, string tex);
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

        }

        public async static Task NavigateToApp()
        {
            pinsManager = new PinsManager();
            App.Current.MainPage = new MasterDetail();


        }

        public async static Task NavigateToEditPost(Models.Pin pin)
        {
            Debug.WriteLine("ESTA NO APP NAVIGATE EDIT POST:");
            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userID = getCredentials.IGetCredentials()[0];

            if (pin.Face_id == userID)
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new PostEdit(pin));
            }
            else
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new DetailMessageList(pin));
            }
        }

        IFacebookLogin face;

        public App()
        {
            oldLat = 0;
            oldLng = 0;
            updatedPins = true;

            loc = DependencyService.Get<IGetCurrentPosition>();
            loc.locationObtained += async (object sender, ILocationEventArgs e) =>
           {
               List<PinBuster.Models.Pin> myList = Locator.Map.Pins.ToList();

               foreach (PinBuster.Models.Pin x in myList)
               {
                   if (x.Raio != 0)
                   {
                       if (CalcDistance.findDistance(e.lat, e.lng, x.Latitude, x.Longitude) * 1000 <= x.Raio)
                           x.Visivel = 1;
                       else
                           x.Visivel = 0;
                   }
                   
               }
               lat = e.lat;
               lng = e.lng;

               if (CalcDistance.findDistance(oldLat, oldLng, e.lat, e.lng) > 0.010)
               {
                   updatedPins = true;
                   oldLat = e.lat;
                   oldLng = e.lng;
                   await Locator.Map.LoadPins();
                   Device.StartTimer(new TimeSpan(0, 0, 7), () => { updatedPins = false; return false; });
               }

           };
            loc.IGetCurrentPosition();

            // The root page of your application
            pinsManager = new PinsManager();
            mapPage = new MapPage();
            listView = new MessageListView();


            IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
            String userID = null;
            String userName = null;
            if (getCredentials.IGetCredentials() != null)
            {
                userID = getCredentials.IGetCredentials()[0];
                userName = getCredentials.IGetCredentials()[1];
            }

            if (userID == null)
            {

                MainPage = new NavigationPage(new LoginPage());
            }
            else
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
