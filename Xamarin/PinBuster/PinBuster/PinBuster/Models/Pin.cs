using System;

namespace PinBuster.Models
{
	public class Pin
	{
		public string title { get; set; }
		public string mensagem { get; set; }
		public double latitude { get; set; }
		public double longitude { get; set; }
		public int raio { get; set; }
		public int face_id { get; set; }
		public string categoria { get; set; }
		public int tempo_limite { get; set; }

		public Pin (string title, string mensagem, double latitude, double longitude)
		{
			this.title = title;
			this.mensagem = mensagem;
			this.latitude = latitude;
			this.longitude = longitude;
		}
	}
}

