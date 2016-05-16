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
using PinBuster;

namespace PinBuster.Pages
{
    public class MapPage : ContentPage
    {

        public Map map;

        Label label;
        int clickTotal = 0;

        double lng = 0;
        double lat = 0;

        void postmessageaction(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(lat);
            System.Diagnostics.Debug.WriteLine(lng);

            Navigation.PushModalAsync(new post(lat,lng));
        }

        public MapPage(IGetCurrentPosition loc)
        {

            BindingContext = App.Locator.Map;

            App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler
                (PinsChangedMethod);



            map = new Map(
            MapSpan.FromCenterAndRadius(
                    new Position(0, 0), Distance.FromMiles(0.3)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            map.IsShowingUser = true;

      

            Button button = new Button
            {
                Text = "Pin it here!",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            button.Clicked += postmessageaction;


            var stack = new RelativeLayout {  };

            map.VerticalOptions = LayoutOptions.FillAndExpand;
            map.HeightRequest = 100;
            map.WidthRequest = 960;

            stack.Children.Add(map, Constraint.RelativeToParent((parent) => {
                return parent.X;
            }), Constraint.RelativeToParent((parent) => {
                return parent.Y * .15;
            }), Constraint.RelativeToParent((parent) => {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) => {
                return parent.Height;
            }));

            stack.Children.Add(button, Constraint.RelativeToParent((parent) =>
            {
                return parent.X + parent.Width/2 - parent.Width * 0.5 * 0.5;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y * .15;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width*0.5;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));



            try
            {
                loc.locationObtained += (object sender, ILocationEventArgs e) =>
                {
                    map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(e.lat, e.lng), Distance.FromMiles(0.1)));

                    System.Diagnostics.Debug.WriteLine("Dentro do try lat:" + e.lat);
                    System.Diagnostics.Debug.WriteLine("Dentro do try lng:" + e.lng);

                    lat = e.lat;
                    lng = e.lng;
                };
                loc.IGetCurrentPosition();


            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e);
            }




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

            map.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance * 1.5)));
        }

        public void AddPin(PinBuster.Models.Pin pin)
        {
            this.map.Pins.Add(new Xamarin.Forms.Maps.Pin
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


