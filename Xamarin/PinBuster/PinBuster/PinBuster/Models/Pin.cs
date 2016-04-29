using System;

namespace PinBuster.Models
{
	public class Pin
	{
		public string title { get; set; }
		public string content { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }

		public Pin (string title, string content, double latitude, double longitude)
		{
			this.title = title;
			this.content = content;
			this.latitude = latitude;
			this.longitude = longitude;
		}
	}
}

