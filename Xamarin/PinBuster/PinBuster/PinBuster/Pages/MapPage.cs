﻿using System;
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
using PinBuster;

namespace PinBuster.Pages
{
    public class MapPage : ContentPage
    {
        
        public CustomMap map;
        public bool update, isCentering;
        Button recenterBtn;
        public IEnumerable<String> town;

        Label label;
        int clickTotal = 0;

        void postmessageaction(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(App.lat);
            System.Diagnostics.Debug.WriteLine(App.lng);

            Navigation.PushAsync(new Post(App.lat,App.lng,App.town));
        }

        public MapPage()
        {

            BindingContext = App.Locator.Map;

            //App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PinsChangedMethod);
            
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
            isCentering = true;

            try
            {
                App.loc.locationObtained += (object sender, ILocationEventArgs e) =>
                 {
                     if (update)
                     {
                         isCentering = true;
                         Device.StartTimer(new TimeSpan(0, 0, 2), () => { isCentering = false; return false; });
                         map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.lat,App.lng), Distance.FromMiles(0.1)));
                         if (App.town == null)
                         {
                             setTown();
                         }
                     }
                 };
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error: " + e);
            }

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

            recenterBtn = new Button
            {
                Text = "Re-Center",
                Font = Font.SystemFontOfSize(NamedSize.Micro),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            recenterBtn.Clicked += OnRecenterClicked;
            recenterBtn.IsVisible = false;

            stack.Children.Add(recenterBtn, Constraint.RelativeToParent((parent) =>
            {
                return parent.X + parent.Width / 2 - parent.Width * 0.5 * 0.5;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y * 0.95 + 3*App.screenHeight / 4;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width * 0.5;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            map.PropertyChanged += SpanChanged;

            

            Content = stack;
        }
        
        async private void setTown()
        {
            var geo = new Geocoder();
            town = await geo.GetAddressesForPositionAsync(new Position(App.lat, App.lng));
            parseTown(town.First());
        }

        private void parseTown(string first)
        {           
            App.town = first.Split(' ')[0];
        }

        private void SpanChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "VisibleRegion" && !isCentering)
            {
                update = false;
                recenterBtn.IsVisible = true;
            }
        }

        private void OnRecenterClicked(object sender, EventArgs e)
        {
            update = isCentering = true;
            
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.lat, App.lng), Distance.FromMiles(0.1)));
            
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
            var pinToAdd = (new Pin
            {
                Position = new Position(pin.Latitude, pin.Longitude),
                Address = pin.Conteudo,
                Label = pin.Nome,
                Type = PinType.Place
            });
            pin.ActualPin = pinToAdd;
            map.CustomPins.Add(pin);
            map.Pins.Add(pin.ActualPin);
           // this.PositionMap();
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


