using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinBuster.Models;
using Newtonsoft.Json.Linq;

namespace PinBuster.Data
{
	public class PinsManager
	{
		HttpClient client;

		public List<Pin> Pins { get; private set; }

		public PinsManager ()
		{
			client = new HttpClient ();
			client.MaxResponseContentBufferSize = 256000;
		}

		public async Task<List<Pin>> FetchPins ()
		{
			Pins = new List<Pin> ();

			var uri = new Uri (string.Format (Constants.RestUrl + "/mensagem", string.Empty));
			Debug.WriteLine (uri);

			try {
				var response = await client.GetAsync (uri);
				if (response.IsSuccessStatusCode) {
					var content = await response.Content.ReadAsStringAsync ();
					var contentJson = JObject.Parse(content);
					foreach(JObject msg in contentJson["data"]) {
						Pin newPin = new Pin("Titulo", (string) msg["conteudo"], (double) msg["latitude"], (double) msg["longitude"]);
						newPin.raio = (int) msg["raio"];
						newPin.face_id = (int) msg["face_id"];
						newPin.tempo_limite = (int) msg["tempo_limite"];
						newPin.categoria = (string) msg["categoria"];
						string date = (string) msg["data"];
						string formattedDate = date.Substring(0, 24);
						newPin.data = DateTime.Parse(formattedDate); 
						Pins.Add(newPin);
					}
					Debug.WriteLine(Pins);
				}
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}

			return Pins;
		}
	}
}

