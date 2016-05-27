﻿using PinBuster.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.Devices.Geolocation;
using Windows.Storage.Streams;
using System.Collections.Specialized;
using System.Collections;
using PinBuster.Pages;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PinBuster.UWP
{
    class CustomMapRenderer : MapRenderer
    {
        MapControl nativeMap;
        XamarinMapOverlay mapOverlay;
        bool xamarinOverlayShown = false;
        RandomAccessStreamReference image;

        public List<Models.Pin> customPins;

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
                var formsMap = (CustomMap)e.NewElement;
                nativeMap = Control as MapControl;
                customPins = new List<Models.Pin>();

                nativeMap.Children.Clear();
                nativeMap.MapElementClick += OnMapElementClick;
                image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///pin.png"));


                foreach (var pin in PinBuster.App.Locator.Map.Pins)
                {
                    System.Diagnostics.Debug.WriteLine("entrei2s");
                    positionPin(pin);
                }

                PinBuster.App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PinsChangedMethod);

            }
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
                        System.Diagnostics.Debug.WriteLine("entrei");
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
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
            mapIcon.Image = image;
            mapIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
            mapIcon.Location = snPoint;
            mapIcon.NormalizedAnchorPoint = new Windows.Foundation.Point(0.5, 1.0);

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


                    mapOverlay = new XamarinMapOverlay(customPin);
                    mapOverlay.Tapped += MapOverlay_Tapped;


                    var snPosition = new BasicGeoposition { Latitude = customPin.Latitude, Longitude = customPin.Longitude };
                    var snPoint = new Geopoint(snPosition);

                    nativeMap.Children.Add(mapOverlay);
                    MapControl.SetLocation(mapOverlay, snPoint);
                    MapControl.SetNormalizedAnchorPoint(mapOverlay, new Windows.Foundation.Point(0.5, 1.0));
                    xamarinOverlayShown = true;

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
    }
}

