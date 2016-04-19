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

		public ObservableCollection<PinBuster.Models.Pin> Pins = new ObservableCollection<PinBuster.Models.Pin>();

		public MapViewModel ()
		{
			Task.Run(() => LoadPins());
		}

		public async Task LoadPins()
		{
			var pins = await App.PinsManager.FetchPins ();

			foreach (var pin in pins) {
				this.Pins.Add (pin);
			}
		}
	}
}

