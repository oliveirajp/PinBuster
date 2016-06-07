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

            Navigation.PushAsync(new Post(App.lat, App.lng, App.town));
        }

        public MapPage()
        {

            BindingContext = App.Locator.Map;

            map = new CustomMap
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            map.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                    new Position(App.lat, App.lng), Distance.FromMiles(0.1)));

            update = true;
            isCentering = true;

            try
            {
                App.loc.locationObtained += (object sender, ILocationEventArgs e) =>
                 {

                     System.Diagnostics.Debug.WriteLine(App.lat);
                     System.Diagnostics.Debug.WriteLine(App.lng);
                     if (update)
                     {
                         System.Diagnostics.Debug.WriteLine("Centrando");
                         isCentering = true;
                         Device.StartTimer(new TimeSpan(0, 0, 2), () => { isCentering = false; return false; });
                         map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(App.lat, App.lng), Distance.FromMiles(0.1)));
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


            var stack = new RelativeLayout { };

            map.VerticalOptions = LayoutOptions.FillAndExpand;
            map.HeightRequest = 100;
            map.WidthRequest = 960;

            stack.Children.Add(map, Constraint.RelativeToParent((parent) =>
            {
                return parent.X;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y * .15;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height;
            }));

            stack.Children.Add(button, Constraint.RelativeToParent((parent) =>
            {
                return parent.X + parent.Width / 2 - parent.Width * 0.5 * 0.5;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y * .15;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width * 0.5;
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
                return parent.Y * 0.95 + parent.Height * 0.9;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width * 0.5;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            map.PropertyChanged += SpanChanged;

            //Normal
            Switch switcherAll = new Switch
            {
                IsToggled = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ClassId = "Normal"
            };
            switcherAll.Toggled += switcher_Toggled;

            stack.Children.Add(switcherAll, Constraint.RelativeToParent((parent) =>
            {
                return parent.X - parent.Width * 0.35;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y + parent.Height * 0.9;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            Label labelAll = new Label
            {
                Text = "Normal",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Black,
            };
            stack.Children.Add(labelAll, Constraint.RelativeToParent((parent) =>
            {
                if (Device.OS.ToString() == "Android")
                    return parent.X - parent.Width * 0.11;
                else
                    return parent.X - parent.Width * 0.205;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y + parent.Height * 0.9;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            //Secret 
            Switch switcherSecret = new Switch
            {
                IsToggled = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ClassId = "Secret"
            };
            switcherSecret.Toggled += switcher_Toggled;

            stack.Children.Add(switcherSecret, Constraint.RelativeToParent((parent) =>
            {
                return parent.X - parent.Width * 0.35;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y + parent.Height * 0.85;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            Label labelSecret = new Label
            {
                Text = "Secrets",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Black,
            };
            stack.Children.Add(labelSecret, Constraint.RelativeToParent((parent) =>
            {
                if (Device.OS.ToString() == "Android")
                    return parent.X - parent.Width * 0.11;
                else
                    return parent.X - parent.Width * 0.205;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y + parent.Height * 0.85;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            //Review 
            Switch switcherReview = new Switch
            {
                IsToggled = true,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                ClassId = "Review"
            };
            switcherReview.Toggled += switcher_Toggled;

            stack.Children.Add(switcherReview, Constraint.RelativeToParent((parent) =>
            {
                return parent.X - parent.Width * 0.35;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y + parent.Height * 0.8;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            Label labelReview = new Label
            {
                Text = "Reviews",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.Black,
            };
            stack.Children.Add(labelReview, Constraint.RelativeToParent((parent) =>
            {
                if (Device.OS.ToString() == "Android")
                    return parent.X - parent.Width * 0.11;
                else
                    return parent.X - parent.Width * 0.205;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Y + parent.Height * 0.8;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height * .1;
            }));

            Content = stack;
        }

        async private void setTown()
        {
            try
            {

                var geo = new Geocoder();
                town = await geo.GetAddressesForPositionAsync(new Position(App.lat, App.lng));
                parseTown(town.First());
            }
            catch (Exception err)
            {

                System.Diagnostics.Debug.WriteLine(err.Message);
            }
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
            Device.StartTimer(new TimeSpan(0, 0, 2), () => { isCentering = false; return false; });

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
        private void switcher_Toggled(object sender, ToggledEventArgs e)
        {
            var s = (Switch)sender;
            foreach (Models.Pin i in App.Locator.Map.Pins)
                if (i.Categoria == s.ClassId)
                {
                    i.Show = s.IsToggled;

                }

        }

    }

}


