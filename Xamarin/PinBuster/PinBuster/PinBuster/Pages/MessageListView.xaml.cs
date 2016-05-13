using PinBuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster.Pages
{
    public partial class MessageListView : TabbedPage
    {

        Message_L lista_mensagens;

        public async Task LoadInfo()
        {
            string response = "";
            PinBuster.Data.Utilities u = new PinBuster.Data.Utilities();
            response = await u.MakeGetRequest("message_user");

            try
            {
                //Pins = JsonConvert.DeserializeObject<List<Pin>>(content);

                //message_L = JsonConvert.DeserializeObject<List<Message_L>>(response);
                lista_mensagens = (Message_L)Newtonsoft.Json.JsonConvert.DeserializeObject(response, typeof(Message_L));
                Device.BeginInvokeOnMainThread(() =>    
                {

                    foreach (var msg in lista_mensagens.data)
                    {
                        _viewModel.All_M.Add(msg);
                        if(msg.Categoria == "Secret")
                            _viewModel.Secret_M.Add(msg);
                        else if (msg.Categoria == "Review")
                            _viewModel.Review_M.Add(msg);
                    }
                    
                });
               
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Erro: " + e);
            }
           
        }

        private MessageListModel _viewModel = new MessageListModel();
        public MessageListView()
        {
            InitializeComponent();
            Task.Run(() => LoadInfo());
            this.BindingContext = this._viewModel;
        

        }

        public void MenuItemClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem.CommandParameter as Message_i;

            if (menuItem.IsDestructive)
            {
                _viewModel.All_M.Remove(item);
            }
            else
            {
                DisplayAlert("Message Information", item.Nome + "\n" +item.Conteudo + "\n" + item.Data, "Ok");
            }
        }

        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            else
            {
                var temp_Item = (Message_i)e.SelectedItem;
                var Message_Page = new DetailMessageList(temp_Item); // so the new page shows correct data
                 Navigation.PushAsync(Message_Page);
                ((ListView)sender).SelectedItem = null;
            }
        }

    }
}
