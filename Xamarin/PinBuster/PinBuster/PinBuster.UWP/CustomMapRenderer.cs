using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Input;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using System.Collections.Specialized;
using System.ComponentModel;
using PinBuster.Pages;
using System.Diagnostics;


[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PinBuster.UWP
{
    class CustomMapRenderer : MapRenderer
    {
        const int EarthRadiusInMeteres = 6371000;
        MapControl nativeMap;
        XamarinMapOverlay mapOverlay;
        bool xamarinOverlayShown = false;
        RandomAccessStreamReference imageSecret, imageNormal, imageReview, imageAchievement;
        List<MapIcon> markers = new List<MapIcon>();
        int userRadius;
        MapPolygon userCircle;

        Models.Pin selectedPin;

        public List<Models.Pin> customPins;

        public object Thread { get; private set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                nativeMap.MapElementClick -= OnMapElementClick;
                nativeMap.Children.Clear();
                mapOverlay = null;
                nativeMap = null;
            }

            if (e.NewElement != null)
            {
                nativeMap = Control as MapControl;
                customPins = new List<Models.Pin>();
                userRadius = PinBuster.App.radius * 1000;

                nativeMap.Children.Clear();
                nativeMap.MapElementClick += OnMapElementClick;

                imageSecret = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin_secret.png"));
                imageNormal = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin_normal.png"));
                imageReview = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin_review.png"));
                imageAchievement = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin_achievement.png"));

                foreach (var pin in PinBuster.App.Locator.Map.Pins)
                {
                    positionPin(pin);
                }

                PinBuster.App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PinsChangedMethod);

                PinBuster.App.loc.locationObtained += (object sender, ILocationEventArgs eLoc) =>
                {
                    var coordinates = new List<BasicGeoposition>();
                    Position pos = new Position(eLoc.lat, eLoc.lng);
                    var positions = GenerateCircleCoordinates(pos, userRadius);
                    foreach (var position in positions)
                    {
                        coordinates.Add(new BasicGeoposition { Latitude = position.Latitude, Longitude = position.Longitude });
                    }

                    if(userCircle != null)
                        nativeMap.MapElements.Remove(userCircle);

                    userCircle = new MapPolygon();
                    userCircle.FillColor = Windows.UI.Color.FromArgb(75, 255, 255, 255);
                    userCircle.StrokeColor = Windows.UI.Color.FromArgb(128, 27, 67, 76);
                    userCircle.StrokeThickness = 5;
                    userCircle.Path = new Geopath(coordinates);
                    userCircle.ZIndex = 1;
                    nativeMap.MapElements.Add(userCircle);
                    
                };
               }
        }

        private double ToRadians(double x)
        {
            return x * (Math.PI / 180);
        }

        private double ToDegrees(double x)
        {
            return x * (180 / Math.PI);
        }

        private List<Position> GenerateCircleCoordinates(Position position, double radius)
        {
            double latitude = ToRadians(position.Latitude);
            double longitude = ToRadians(position.Longitude);
            double distance = radius / EarthRadiusInMeteres;
            var positions = new List<Position>();

            for (int angle = 0; angle <= 360; angle++)
            {
                double angleInRadians = ToRadians((double)angle);
                double latitudeInRadians = Math.Asin(Math.Sin(latitude) * Math.Cos(distance) + Math.Cos(latitude) * Math.Sin(distance) * Math.Cos(angleInRadians));
                double longitudeInRadians = longitude + Math.Atan2(Math.Sin(angleInRadians) * Math.Sin(distance) * Math.Cos(latitude), Math.Cos(distance) - Math.Sin(latitude) * Math.Sin(latitudeInRadians));

                var pos = new Position(ToDegrees(latitudeInRadians), ToDegrees(longitudeInRadians));
                positions.Add(pos);
            }

            return positions;
        }

        private void MapOverlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            nativeMap.Children.Remove(mapOverlay);
            xamarinOverlayShown = false;
        }

        private void PinsChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Models.Pin pin in e.NewItems)
                    {
                        positionPin(pin);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    System.Diagnostics.Debug.WriteLine("Pin removido");
                    foreach (Models.Pin pin in e.NewItems)
                    {
                        pin.PropertyChanged -= this.OnItemPropertyChanged;
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    break;

            }
        }

        private void positionPin(Models.Pin pin)
        {
            var snPosition = new BasicGeoposition { Latitude = pin.Latitude, Longitude = pin.Longitude };
            var snPoint = new Geopoint(snPosition);

            var mapIcon = new MapIcon();
            switch (pin.Categoria)
            {
                case "Secret":
                    mapIcon.Image = imageSecret;
                    break;
                case "Normal":
                    mapIcon.Image = imageNormal;
                    break;
                case "Review":
                    mapIcon.Image = imageReview;
                    break;
                case "Exploration":
                    mapIcon.Image = imageAchievement;
                    break;
                default:
                    mapIcon.Image = imageNormal;
                    break;
            }
            mapIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
            mapIcon.Location = snPoint;
            mapIcon.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);
            mapIcon.ZIndex = 10;
            nativeMap.MapElements.Add(mapIcon);

            var pinToAdd = (new Pin
            {
                Position = new Position(pin.Latitude, pin.Longitude),
                Address = pin.Conteudo,
                Label = pin.Nome,
                Type = PinType.Place
            });
            pin.ActualPin = pinToAdd;
            customPins.Add(pin);

            pin.PropertyChanged += this.OnItemPropertyChanged;
        }



        private void OnMapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            var mapIcon = args.MapElements.FirstOrDefault(x => x is MapIcon) as MapIcon;
            if (mapIcon != null)
            {
                if (!xamarinOverlayShown)
                {
                    var customPin = GetCustomPin(mapIcon.Location.Position);
                    if (customPin == null)
                    {
                        throw new Exception("Custom pin not found");
                    }

                    if (customPin.Visivel == 1)
                    {
                        mapOverlay = new XamarinMapOverlay(customPin);
                        mapOverlay.Tapped += MapOverlay_Tapped;

                        selectedPin = customPin;

                        var snPosition = new BasicGeoposition { Latitude = customPin.Latitude, Longitude = customPin.Longitude };
                        var snPoint = new Geopoint(snPosition);

                        nativeMap.Children.Add(mapOverlay);
                        MapControl.SetLocation(mapOverlay, snPoint);
                        MapControl.SetNormalizedAnchorPoint(mapOverlay, new Windows.Foundation.Point(0.5, 1.0));
                        xamarinOverlayShown = true;
                    }
                    else
                    {
                        mapOverlay = new XamarinMapOverlay(null);
                        mapOverlay.Tapped += MapOverlay_Tapped;

                        selectedPin = customPin;

                        var snPosition = new BasicGeoposition { Latitude = customPin.Latitude, Longitude = customPin.Longitude };
                        var snPoint = new Geopoint(snPosition);

                        nativeMap.Children.Add(mapOverlay);
                        MapControl.SetLocation(mapOverlay, snPoint);
                        MapControl.SetNormalizedAnchorPoint(mapOverlay, new Windows.Foundation.Point(0.5, 1.0));
                        xamarinOverlayShown = true;
                    }

                }
                else
                {
                    nativeMap.Children.Remove(mapOverlay);
                    xamarinOverlayShown = false;
                }
            }
        }

        Models.Pin GetCustomPin(BasicGeoposition position)
        {
            var pos = new Position(position.Latitude, position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.ActualPin.Position == pos)
                {
                    return pin;
                }
            }
            return null;
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Switch");


            System.Diagnostics.Debug.WriteLine("Numero de elementos: " + this.nativeMap.MapElements.Count());

            Models.Pin temp = (Models.Pin)sender;
            foreach (MapIcon m in markers)
                if (m.Location.Position.Latitude == temp.Latitude && m.Location.Position.Longitude == temp.Longitude)
                    m.Visible = temp.Show;
        }

    }
}


