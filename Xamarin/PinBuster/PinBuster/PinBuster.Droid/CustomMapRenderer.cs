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
using System.Net;
using Android.Support.Design.Widget;
using Android.Views;
using Android.App;
using static Android.Gms.Maps.GoogleMap;
using PinBuster.Models;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PinBuster.Droid
{
    class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        GoogleMap map;
        List<Models.Pin> customPins;
        BitmapDescriptor imageNormal, imageSecret, imageAchiv, imageReview;
        Bitmap imageWarning;
        Android.Views.View view;
        Android.Views.LayoutInflater inflater;
        bool infoClicked;
        float maxZoom = 2;
        List<Marker> markers = new List<Marker>();
        CircleOptions warningCircle;
        Circle drawnCircle;
        Circle userCircle;
        int userRadius, fillColor, strokeColor;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                map.InfoWindowClick -= OnInfoWindowClick;
                map.MapClick -= Map_MapClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = new List<Models.Pin>();
                App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PinsChangedMethod);
                ((MapView)Control).GetMapAsync(this);
                imageNormal = resizeMapIcons(Resource.Drawable.pin_normal, 100, 100);
                imageSecret = resizeMapIcons(Resource.Drawable.pin_secreto, 100, 100);
                imageReview = resizeMapIcons(Resource.Drawable.pin_review, 100, 100);
                imageAchiv = resizeMapIcons(Resource.Drawable.pin_achievements, 100, 100);

                Bitmap imageBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.warning);
                imageWarning = Bitmap.CreateScaledBitmap(imageBitmap, 120, 120, false);

                infoClicked = false;
                var c = Android.Graphics.Color.Argb(75, 255, 255, 255);
                fillColor = c.GetHashCode();
                c = Android.Graphics.Color.Argb(128, 27, 67, 76);
                strokeColor = c.GetHashCode();
                userRadius = App.radius * 1000;
                App.updatedPins = true;
            }
        }

        private void Map_MapClick(object sender, MapClickEventArgs e)
        {
            if (warningCircle != null)
            {
                drawnCircle.Remove();
                warningCircle = null;
            }
        }

        private void Map_CameraChange(object sender, GoogleMap.CameraChangeEventArgs e)
        {
            if (e.Position.Zoom > maxZoom)
                map.AnimateCamera(CameraUpdateFactory.ZoomTo(maxZoom));
        }

        private void PinsChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Models.Pin pin in e.NewItems)
                {
                    if (!CheckIfExists(pin))
                        positionPin(pin);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Models.Pin pin in e.OldItems)
                {
                    pin.PropertyChanged -= this.OnItemPropertyChanged;
                    LatLng pos = new LatLng(pin.Latitude, pin.Longitude);
                    Marker ms = null;
                    foreach (var m in markers)
                    {
                        if (m.Position.Latitude == pos.Latitude && m.Position.Longitude == pos.Longitude)
                        {
                            ms = m;
                            break;
                        }
                    }
                    if (ms != null)
                    {
                        markers.Remove(ms);
                        ms.Remove();
                    }

                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(e.Action.ToString());
            }
        }

        private bool CheckIfExists(Models.Pin pin)
        {

            foreach (Models.Pin p in customPins)
            {
                if (p.Longitude == pin.Longitude && p.Latitude == pin.Latitude && p.Conteudo == pin.Conteudo && p.Data == pin.Data)
                {
                    p.Visivel = pin.Visivel;
                    return true;
                }
            }

            return false;
        }

        private void positionPin(Models.Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Latitude, pin.Longitude));
            marker.SetTitle(pin.Categoria);
            marker.SetSnippet(pin.Conteudo);

            switch (pin.Categoria)
            {
                case "Secret":
                    marker.SetIcon(imageSecret);
                    break;
                case "Normal":
                    marker.SetIcon(imageNormal);
                    break;
                case "Review":
                    marker.SetIcon(imageReview);
                    break;
                case "Exploration":
                    marker.SetIcon(imageAchiv);
                    break;
                default:
                    marker.SetIcon(imageNormal);
                    break;
            }
            Marker mr = map.AddMarker(marker);
            markers.Add(mr);
            var pinToAdd = (new Xamarin.Forms.Maps.Pin
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

        public BitmapDescriptor resizeMapIcons(int iconName, int width, int height)
        {
            Bitmap imageBitmap = BitmapFactory.DecodeResource(Resources, iconName);
            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(imageBitmap, width, height, false);

            BitmapDescriptor icon = BitmapDescriptorFactory.FromBitmap(resizedBitmap);

            return icon;
        }

        public void OnMapReady(GoogleMap googleMap) { 

            map = googleMap;

            App.loc.locationObtained += (object sender, ILocationEventArgs e) =>
            {
                if (App.updatedPins)
                {
                    var circleOptions = new CircleOptions();
                    circleOptions.InvokeCenter(new LatLng(e.lat, e.lng));
                    circleOptions.InvokeRadius(userRadius);
                    circleOptions.InvokeFillColor(fillColor);
                    circleOptions.InvokeStrokeColor(strokeColor);
                    circleOptions.InvokeStrokeWidth(5);
                    if (userCircle != null)
                        userCircle.Remove();
                    userCircle = map.AddCircle(circleOptions);
                    App.updatedPins = false;
                }
            };
                

            map.InfoWindowClick += OnInfoWindowClick;
            map.SetInfoWindowAdapter(this);

            map.UiSettings.MyLocationButtonEnabled = false;
            map.UiSettings.CompassEnabled = true;

            map.MapClick += Map_MapClick;

            foreach (var pin in App.Locator.Map.Pins)
            {
                positionPin(pin);
            }
        }



        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);


            var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
            var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
            var image = view.FindViewById<ImageView>(Resource.Id.UserImage);

            if (image != null)
            {
                Bitmap imageBitmap = GetImageBitmapFromUrl(customPin.Imagem, 120, 120);
                image.SetImageBitmap(imageBitmap);
            }

            if (!infoClicked)
            {
                if (infoTitle != null)
                {
                    infoTitle.Text = customPin.Nome;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = customPin.Data;
                }
                infoClicked = true;
            }
            else
            {
                if (infoTitle != null)
                {
                    infoTitle.Text = customPin.Categoria;
                }
                if (infoSubtitle != null)
                {
                    infoSubtitle.Text = customPin.Conteudo;
                }
                infoClicked = false;
            }
            e.Marker.HideInfoWindow();
            e.Marker.ShowInfoWindow();
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var customPin = GetCustomPin(marker);
            if (warningCircle != null)
            {
                drawnCircle.Remove();
                warningCircle = null;
            }
            if (infoClicked && customPin.Visivel == 1)
            {
                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoContent = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                if (customPin.Nome == infoTitle.Text && infoContent.Text == customPin.Data)
                {
                    return view;
                }
            }

            inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {

                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }
                infoClicked = false;
                view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                var image = view.FindViewById<ImageView>(Resource.Id.UserImage);

                if (customPin.Visivel == 1)
                {
                    if (infoTitle != null)
                    {
                        infoTitle.Text = marker.Title;
                    }
                    if (infoSubtitle != null)
                    {
                        infoSubtitle.Text = marker.Snippet;
                    }
                    if (image != null)
                    {
                        Bitmap imageBitmap = GetImageBitmapFromUrl(customPin.Imagem, 120, 120);
                        image.SetImageBitmap(imageBitmap);
                    }

                    return view;
                }
                else
                {
                    if (infoTitle != null)
                    {
                        infoTitle.Text = "Warning!";
                    }
                    if (infoSubtitle != null)
                    {
                        infoSubtitle.Text = "Get closer to read the pin";
                    }
                    if (image != null)
                    {
                        image.SetImageBitmap(imageWarning);
                    }
                    warningCircle = new CircleOptions();
                    warningCircle.InvokeCenter(new LatLng(customPin.Latitude, customPin.Longitude));
                    warningCircle.InvokeRadius(customPin.Raio);
                    warningCircle.InvokeFillColor(0X66FF0000);
                    warningCircle.InvokeStrokeColor(0X66FF0000);
                    warningCircle.InvokeStrokeWidth(1);
                    drawnCircle = map.AddCircle(warningCircle);

                    return view;
                }
            }
            return null;
        }


        private Bitmap GetImageBitmapFromUrl(string imagem, int width, int height)
        {
            String[] parsed = imagem.Split('?');
            string img = parsed[0] + "?width=" + width + "&height=" + height;

            using (WebClient webClient = new WebClient())
            {
                byte[] bytes = webClient.DownloadData(img);
                if (bytes != null && bytes.Length > 0)
                {
                    Bitmap bm = BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                    return Bitmap.CreateScaledBitmap(bm, width, height, false);
                }
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

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Switch");

            Models.Pin temp = (Models.Pin)sender;
            foreach (Marker m in markers)
                if (m.Position.Latitude == temp.Latitude && m.Position.Longitude == temp.Longitude && m.Snippet == temp.Conteudo)
                    m.Visible = temp.Show;
        }

    }
}
