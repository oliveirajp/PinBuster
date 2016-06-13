using System;
using MapKit;

namespace PinBuster.iOS
{
	public class CustomMKPinAnnotationView : MKAnnotationView
	{
		public string Id { get; set; }

		public string Url { get; set; }

		public int OpenCount { get; set; }

		public CustomMKPinAnnotationView (IMKAnnotation annotation, string id)
			: base (annotation, id)
		{
			OpenCount = 0;
		}
	}
}

