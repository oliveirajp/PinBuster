using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using PinBuster.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using System.Collections.Specialized;

namespace PinBuster.Pages
{
	public class MapPage : ContentPage {

		public Map map;
		
		public MapPage() {

			BindingContext = App.Locator.Map;

			//App.Locator.Map.MapPins.CollectionChanged += this.HandleChange;

			App.Locator.Map.setMapPage (this);

			map = new Map ();

			map.IsShowingUser = true;

			var stack = new StackLayout { Spacing = 0 };

			map.VerticalOptions = LayoutOptions.FillAndExpand;
			map.HeightRequest = 100;
			map.WidthRequest = 960;

			stack.Children.Add(map);
			Content = stack;
		}

		private void PositionMap()
		{
			var mapPins = map.Pins;

			if (mapPins == null || !mapPins.Any()) return;

			var centerPosition = new Position(mapPins.Average(x => x.Position.Latitude), mapPins.Average(x => x.Position.Longitude));

			var minLongitude = mapPins.Min(x => x.Position.Longitude);
			var minLatitude = mapPins.Min(x => x.Position.Latitude);

			var maxLongitude = mapPins.Max(x => x.Position.Longitude);
			var maxLatitude = mapPins.Max(x => x.Position.Latitude);

			var distance = MapHelper.CalculateDistance(minLatitude, minLongitude,
				maxLatitude, maxLongitude, 'M') / 2;

			map.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));
		}

		public void AddPin(PinBuster.Models.Pin pin) {
			this.map.Pins.Add (new Xamarin.Forms.Maps.Pin {
				Position = new Position(pin.latitude, pin.longitude),
				Address = pin.content,
				Label = pin.title,
				Type = PinType.Place
			});
			this.PositionMap ();
		}

		private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
		{
			Debug.WriteLine ("ASURA SNIPER");

			foreach (var x in e.NewItems)
			{
				var pin = (Xamarin.Forms.Maps.Pin) x;
				Debug.WriteLine (pin.Label);
				map.Pins.Add(pin);
				//PositionMap();
			}

			foreach (var y in e.OldItems)
			{
				//do something
			}
			if (e.Action == NotifyCollectionChangedAction.Move)
			{
				//do something
			}
		}

	}
}


