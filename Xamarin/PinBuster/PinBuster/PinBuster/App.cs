using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using PinBuster.Data;
using PinBuster.Pages;
using PinBuster;
namespace PinBuster
{
    public class App : Application
	{
		private readonly static Locator _locator  = new Locator();

		public static PinsManager PinsManager { get; private set; }

		public static Locator Locator
		{
			get { return _locator; }
		}

        public App()
        {
            // The root page of your application
			PinsManager = new PinsManager();
			MainPage = new MapPage();
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
            // Handle when your app resumes
        }
    }
}
