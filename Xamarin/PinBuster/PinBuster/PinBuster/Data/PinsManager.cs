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
        PinBuster.Data.Utilities u;
        Pins_L lista_mensagens;

        public class Pins_L
        {
            public List<Pin> data { get; set; }
        }

        public PinsManager()
        {
            u = new PinBuster.Data.Utilities();
        }

        public async Task<List<Pin>> FetchPins()
        {

            //  System.Diagnostics.Debug.WriteLine("FETCH PINS");
            string response = "";
            if (App.lat == 0 && App.lng == 0)
            {
                return null;
            }
            else
            {
                response = await u.MakeGetRequest("message_user?latitude=" + App.lat.ToString().Replace(',','.') + "&longitude=" + App.lng.ToString().Replace(',', '.') + "&raio=" + App.radius);
                try
                {
                    lista_mensagens = (Pins_L)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(Pins_L));

                    List<PinBuster.Models.Pin> myList = new List<PinBuster.Models.Pin>(App.listView._viewModel.All_M);

                    foreach (var x in myList)
                    {
                        if (!lista_mensagens.data.Contains(x))
                            App.listView._viewModel.All_M.Remove(x);
                    }


                    foreach (var msg in lista_mensagens.data)
                    {
                        if (!App.listView._viewModel.All_M.Contains(msg) && msg.Visivel == 1)
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
                    System.Diagnostics.Debug.WriteLine(@"@@@@@@@@@@@@@@ERROR {0}", ex.Message);
                }

                return lista_mensagens.data;
            }
        }
    }
}