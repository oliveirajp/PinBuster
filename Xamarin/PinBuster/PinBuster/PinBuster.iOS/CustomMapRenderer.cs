using System;
using System.Collections.Generic;
using CoreGraphics;
using PinBuster;
using PinBuster.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using PinBuster.Pages;
using System.Collections.Specialized;

[assembly:ExportRenderer (typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PinBuster.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		UIView customPinView;
		List<PinBuster.Models.Pin> customPins;
		MKMapView nativeMap;

		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				nativeMap = Control as MKMapView;
				nativeMap.GetViewForAnnotation = null;
				nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
			}

			if (e.NewElement != null) {
				var formsMap = (CustomMap)e.NewElement;
				nativeMap = Control as MKMapView;
				customPins = new List<Models.Pin>();
				App.Locator.Map.Pins.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PinsChangedMethod);

				nativeMap.GetViewForAnnotation = GetViewForAnnotation;
				nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
			}
		}

		private void PinsChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				//map.Clear();

				foreach (Models.Pin pin in e.NewItems)
				{
					var marker = new MKPointAnnotation();
					marker.SetCoordinate (new CoreLocation.CLLocationCoordinate2D (pin.Latitude, pin.Longitude));
					marker.Title = pin.Nome;
					marker.Subtitle = pin.Conteudo;

					nativeMap.AddAnnotation (marker);
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

		MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			if (annotation is MKUserLocation)
				return null;
		
			var customPin = GetCustomPin (annotation);
			if (customPin == null) {
				System.Diagnostics.Debug.WriteLine ("customPin is null");
				return null;
			} else {
				System.Diagnostics.Debug.WriteLine (customPin.Conteudo);
			}

			annotationView = mapView.DequeueReusableAnnotation (customPin.Mensagem_id);
			if (annotationView == null) {
				annotationView = new CustomMKPinAnnotationView (annotation, customPin.Mensagem_id);

				switch (customPin.Categoria)
				{
					case "Secret":
						annotationView.Image = UIImage.FromFile("pin_secreto.png").Scale(new CGSize(40f, 40f));
						break;
					case "Normal":
						annotationView.Image = UIImage.FromFile("pin_normal.png").Scale(new CGSize(40f, 40f));
						break;
					case "Review":
						annotationView.Image = UIImage.FromFile("pin_review.png").Scale(new CGSize(40f, 40f));
						break;
					case "Exploration":
						annotationView.Image = UIImage.FromFile("pin_achievements.png").Scale(new CGSize(40f, 40f));
						break;
					default:
						annotationView.Image = UIImage.FromFile("pin_normal.png").Scale(new CGSize(40f, 40f));
						break;
				}

				if (customPin.Visivel == 0) {
					annotationView.CalloutOffset = new CGPoint (1000000, 0);
				} else {
					annotationView.CalloutOffset = new CGPoint (0, 0);
				}

				annotationView.LeftCalloutAccessoryView = new UIImageView (UIImage.FromFile ("monkey.png"));
				annotationView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);
				((CustomMKPinAnnotationView)annotationView).Id = customPin.Mensagem_id;
				((CustomMKPinAnnotationView)annotationView).Url = "fakeurl";
			}
			annotationView.CanShowCallout = true;
			return annotationView;
		}

		void OnCalloutAccessoryControlTapped (object sender, MKMapViewAccessoryTappedEventArgs e)
		{
			var customView = e.View as CustomMKPinAnnotationView;
			if (!string.IsNullOrWhiteSpace (customView.Url)) {
				UIApplication.SharedApplication.OpenUrl (new Foundation.NSUrl (customView.Url));
			}

			customPinView = new UIView();

			customPinView.Frame = new CGRect(0, 0, 200, 84);
			var text = new UILabel();
			text.Text = "This is more info LUL";
			customPinView.AddSubview(text);
			customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
			customView.AddSubview(customPinView);
		}

		void OnDidSelectAnnotationView (object sender, MKAnnotationViewEventArgs e)
		{
			var customView = e.View as CustomMKPinAnnotationView;
			PinBuster.Models.Pin pin = null;
			foreach (var pinIt in customPins) {
				if (pinIt.Mensagem_id == customView.Id) {
					pin = pinIt;
				}
			}

			if (pin == null) {
				return;
			}


			if (pin.Visivel == 0) {
				UIAlertView alert = new UIAlertView () { 
					Title = "Pin not visible", Message = "Get closer to the pin"
				};
				alert.AddButton("OK");
				alert.Show ();

				return;
			}
		}

		void OnDidDeselectAnnotationView (object sender, MKAnnotationViewEventArgs e)
		{
			if (!e.View.Selected) {
				if (customPinView == null)
				{
					return;
				}
				customPinView.RemoveFromSuperview ();
				customPinView.Dispose ();
				customPinView = null;
			}
		}

		PinBuster.Models.Pin GetCustomPin (IMKAnnotation annotation)
		{
			var position = new Position (annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
			foreach (var pin in customPins) {

				System.Diagnostics.Debug.WriteLine(pin.Latitude);
				System.Diagnostics.Debug.WriteLine(position.Latitude);
				if (pin.Latitude == position.Latitude) {
					return pin;
				}
			}
			return null;
		}
	}
}

