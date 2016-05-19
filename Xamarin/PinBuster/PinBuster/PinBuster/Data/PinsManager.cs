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
        Pins_L lista_mensagens;

        public class Pins_L
        {
            public List<Pin> data { get; set; }
        }

		public PinsManager ()
		{
			client = new HttpClient ();
			client.MaxResponseContentBufferSize = 256000;
		}

		public async Task<List<Pin>> FetchPins ()
		{
            string response = "";
            PinBuster.Data.Utilities u = new PinBuster.Data.Utilities();
            if (App.lat == 0 && App.lng == 0)
            {
                return null;
            }
            else
            {
                response = await u.MakeGetRequest("message_user?latidude=" + App.lat + "&longitude=" + App.lng + "&raio=10");
                try
                {
                    lista_mensagens = (Pins_L)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(Pins_L));


                    foreach (var x in App.listView._viewModel.All_M)
                    {
                        if (!lista_mensagens.data.Contains(x))
                            App.listView._viewModel.All_M.Remove(x);
                    }

                    foreach (var msg in lista_mensagens.data)
                    {
                        if (!App.listView._viewModel.All_M.Contains(msg))
                        {
                            App.listView._viewModel.All_M.Add(msg);
                            if (msg.Categoria == "Secret")
                                App.listView._viewModel.Secret_M.Add(msg);
                            else if (msg.Categoria == "Review")
                                App.listView._viewModel.Review_M.Add(msg);
                        }
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"				ERROR {0}", ex.Message);
                }

                return lista_mensagens.data;
            }
		}
	}
}

