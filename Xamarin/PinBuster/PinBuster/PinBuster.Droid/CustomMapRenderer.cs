using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using PinBuster;
using PinBuster.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using PinBuster.Pages;
using System.Collections.Specialized;
using Android.Graphics;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PinBuster.Droid
{
    class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        GoogleMap map;
        List<Models.Pin> customPins;
        bool isDrawn;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                map.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = new List<Models.Pin>();
                App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PinsChangedMethod);
                ((MapView)Control).GetMapAsync(this);
            }
        }

        private void PinsChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                //map.Clear();

                foreach (Models.Pin pin in e.NewItems)
                {
                    var marker = new MarkerOptions();
                    marker.SetPosition(new LatLng(pin.Latitude, pin.Longitude));
                    marker.SetTitle(pin.Categoria);
                    marker.SetSnippet(pin.Conteudo);
                    marker.SetIcon(resizeMapIcons(Resource.Drawable.pin,100,100));

                    map.AddMarker(marker);
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
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                System.Diagnostics.Debug.WriteLine("Pin removido");
            }
        }

        public BitmapDescriptor resizeMapIcons(int iconName, int width, int height)
        {
            Bitmap imageBitmap = BitmapFactory.DecodeResource(Resources, iconName); 
            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(imageBitmap, width, height, false);

            BitmapDescriptor icon = BitmapDescriptorFactory.FromBitmap(resizedBitmap);

            return icon;   
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            map.InfoWindowClick += OnInfoWindowClick;
            map.SetInfoWindowAdapter(this);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (changed)
            {
                isDrawn = false;
            }
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Mostrar info");
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                if (customPin.Categoria == "Xamarin")
                {
                    view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);
                }
                else if (customPin.Visivel == 1)
                {
                    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                }
                else
                    return null;

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = marker.Snippet;
                }

                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        Models.Pin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.ActualPin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
    }
}