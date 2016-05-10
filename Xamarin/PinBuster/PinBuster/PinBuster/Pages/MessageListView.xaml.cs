using PinBuster.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster.Pages
{
    public partial class MessageListView : TabbedPage
    {
        private MessageListModel _viewModel = new MessageListModel();
        public MessageListView()
        {
            InitializeComponent();
            
            _viewModel.loadTask.Wait();
            this.BindingContext = this._viewModel;
        }

        public void MenuItemClicked(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var item = menuItem.CommandParameter as Item;

            if (menuItem.IsDestructive)
            {
                _viewModel.All_M.Remove(item);
            }
            else
            {
                DisplayAlert("Informações", item.Description, "Ok");
            }
        }
    }
}
