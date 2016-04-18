using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using PinBuster.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System.Diagnostics;
using PinBuster.Pages;

namespace PinBuster.ViewModels
{
	public class MapViewModel : ViewModelBase
	{
		public static readonly Position NullPosition = new Position(0, 0);

		public List<Xamarin.Forms.Maps.Pin> MapPins;

		public MapPage mapPage;

		public MapViewModel ()
		{
			Task.Run(() => LoadPins());
		}

		public void setMapPage(MapPage mapPage) {
			this.mapPage = mapPage;
		}

		public async Task LoadPins()
		{
			var pins = await App.PinsManager.FetchPins ();

			foreach (var pin in pins) {
				Xamarin.Forms.Device.BeginInvokeOnMainThread ( () => {
					this.mapPage.AddPin (pin);
				});
			};
		}
	}
}

