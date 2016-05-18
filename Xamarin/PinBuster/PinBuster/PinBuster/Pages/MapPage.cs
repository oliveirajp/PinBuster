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
using PinBuster.Data;
using System.Threading;

namespace PinBuster.Pages
{
    public class MapPage : ContentPage
    {

        public Map map;
        public bool update, test;
        public Position updPos;
        Button recenterBtn;
        private int time;

        public MapPage()
        {

            BindingContext = App.Locator.Map;

            App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler
                (PinsChangedMethod);

            map = new CustomMap
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            map.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                    new Position(App.lat, App.lng), Distance.FromMiles(0.3)));
            update = true;

            test = false;

            try
            {
                App.loc.locationObtained += (object sender, ILocationEventArgs e) =>
                 {
                     //updPos = new Position(e.lat, e.lng);
                     //if (update)
                     //{
                     //    map.MoveToRegion(MapSpan.FromCenterAndRadius(updPos, Distance.FromMiles(0.1)));
                     //}
                     if (update)
                     {
                         test = true;
                         Device.StartTimer(new TimeSpan(0, 0, 2), () => { test = false; return false; });
                         map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.lat, App.lng), Distance.FromMiles(0.1)));
                     }
                 };
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e);
            }

            map.PropertyChanged += SpanChanged;

            recenterBtn = new Button
            {
                Text = "Re-Center",
                Font = Font.SystemFontOfSize(NamedSize.Micro),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            recenterBtn.Clicked += OnRecenterClicked;
            recenterBtn.IsEnabled = false;
            recenterBtn.IsVisible = false;


            var stack = new StackLayout
            {
                Spacing = 0,
                Children =
                {
                    //new PanContainer
                    //{
                    //   Content = map,
                    //   VerticalOptions =LayoutOptions.FillAndExpand
                    //}
                  map,recenterBtn
                 }
            };


            Content = stack;
        }
        

        private void SpanChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRegion" && !test)
            {
                update = false;
                recenterBtn.IsEnabled = true;
                recenterBtn.IsVisible = true;
            }
        }

        private void OnRecenterClicked(object sender, EventArgs e)
        {
            update = test = true;
            
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.lat, App.lng), Distance.FromMiles(0.1)));

            recenterBtn.IsEnabled = false;
            recenterBtn.IsVisible = false;
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

            map.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance * 1.5)));
        }

        public void AddPin(PinBuster.Models.Pin pin)
        {
            this.map.Pins.Add(new Pin
            {
                Position = new Position(pin.latitude, pin.longitude),
                Address = pin.mensagem,
                Label = pin.title,
                Type = PinType.Place
            });
            this.PositionMap();
        }

        private void PinsChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var pin in e.NewItems)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        this.AddPin((PinBuster.Models.Pin)pin);
                    });
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Replace)
            {
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
            }

            if (e.Action == NotifyCollectionChangedAction.Move)
            {
            }
        }

    }

}


