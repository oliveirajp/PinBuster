using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using PinBuster
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

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PinBuster.Droid
{
    class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
    {
        GoogleMap map;
        List<Models.Pin> customPins;
        BitmapDescriptor imageNormal, imageSecret, imageAchiv, imageReview;
        Android.Views.View view;
        Android.Views.LayoutInflater inflater;
        AlertDialog alertDialog;
        bool infoClicked;
        float maxZoom = 2;
        List<Marker> markers = new List<Marker>();
        Models.CustomCircle circle;

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                map.InfoWindowClick -= OnInfoWindowClick;
                //map.CameraChange -= Map_CameraChange;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = new List<Models.Pin>();
                App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PinsChangedMethod);

                    circle = formsMap.Circle;

                    ((MapView)Control).GetMapAsync(this);
                    imageNormal = resizeMapIcons(Resource.Drawable.pin_normal, 100, 100);
                    imageSecret = resizeMapIcons(Resource.Drawable.pin_secreto, 100, 100);
                    imageReview = resizeMapIcons(Resource.Drawable.pin_review, 100, 100);
                    imageAchiv = resizeMapIcons(Resource.Drawable.pin_achievements, 100, 100);
                    infoClicked = false;
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
                //map.Clear();

                foreach (Models.Pin pin in e.NewItems)
                {
                    positionPin(pin);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                System.Diagnostics.Debug.WriteLine("Pin removido");
                foreach (Models.Pin pin in e.NewItems)
                {
                    pin.PropertyChanged -= this.OnItemPropertyChanged;
                    LatLng pos = new LatLng(pin.Latitude, pin.Longitude);
                    foreach(var m in markers)
                    {
                        if (m.Position == pos && m.Title == pin.Categoria && m.Snippet == pin.Conteudo)
                            m.Remove();
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("EVENT");
            }
        }

        private void positionPin(Models.Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Latitude, pin.Longitude));
            if (pin.Visivel == 1)
            {
                marker.SetTitle(pin.Categoria);
                marker.SetSnippet(pin.Conteudo);
            }

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

        public BitmapDescriptor resizeMapIcons(int iconName, int width, int height)
        {
            Bitmap imageBitmap = BitmapFactory.DecodeResource(Resources, iconName);
            Bitmap resizedBitmap = Bitmap.CreateScaledBitmap(imageBitmap, width, height, false);

            BitmapDescriptor icon = BitmapDescriptorFactory.FromBitmap(resizedBitmap);

            return icon;
        }

        public void OnMapReady(GoogleMap googleMap) { 

            map = googleMap;

            var circleOptions = new CircleOptions();
            circleOptions.InvokeCenter(new LatLng(circle.Position.Latitude, circle.Position.Longitude));
            circleOptions.InvokeRadius(circle.Radius);
            circleOptions.InvokeFillColor(0XFFFFFF);
            circleOptions.InvokeStrokeColor(0X1B434C);
            circleOptions.InvokeStrokeWidth(0);
            map.AddCircle(circleOptions);

            map.InfoWindowClick += OnInfoWindowClick;

            //map.CameraChange += Map_CameraChange;
            map.SetInfoWindowAdapter(this);
            
            AlertDialog.Builder alertDialogB = new AlertDialog.Builder(Xamarin.Forms.Forms.Context);
            alertDialogB.SetTitle("Warning");
            alertDialogB.SetMessage("Get closer to read the pin!");
            alertDialogB.SetPositiveButton("Ok", myEventHandler);
            alertDialog = alertDialogB.Create();
            
            map.UiSettings.MyLocationButtonEnabled = false;
            map.UiSettings.CompassEnabled = true;

            foreach (var pin in App.Locator.Map.Pins)
            {
                positionPin(pin);
            }
        }

        void onPositionClick(object sender, GoogleMap.MyLocationButtonClickEventArgs e)
        {

        }

        private void myEventHandler(object sender, DialogClickEventArgs e)
        {
            alertDialog.Hide();
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
        //async void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        //{
        //    var pin = GetCustomPin(e.Marker);
        //    await App.NavigateToEditPost(pin);
        //}


        public Android.Views.View GetInfoContents(Marker marker)
        {
            var customPin = GetCustomPin(marker);
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
                if (customPin.Visivel == 1)
                {
                    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                    var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                    var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                    var image = view.FindViewById<ImageView>(Resource.Id.UserImage);

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
                    alertDialog.Show();
                    return null;
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
