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
        

        public MessageListModel _viewModel = new MessageListModel();
        public MessageListView()
        {
            InitializeComponent();
            this.BindingContext = this._viewModel;
        

        }

        public void MenuItemClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem.CommandParameter as Pin;

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
                var temp_Item = (Pin)e.SelectedItem;
                var Message_Page = new DetailMessageList(temp_Item); // so the new page shows correct data
                 Navigation.PushAsync(Message_Page);
                ((ListView)sender).SelectedItem = null;
            }
        }

    }
}
