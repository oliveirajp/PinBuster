using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PinBuster.Models;

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

			var uri = new Uri (string.Format (Constants.RestUrl + "/pins", string.Empty));

			try {
				var response = await client.GetAsync (uri);
				if (response.IsSuccessStatusCode) {
					var content = await response.Content.ReadAsStringAsync ();
					Pins = JsonConvert.DeserializeObject <List<Pin>> (content);
				}
			} catch (Exception ex) {
				Debug.WriteLine (@"				ERROR {0}", ex.Message);
			}

			return Pins;
		}
	}
}

