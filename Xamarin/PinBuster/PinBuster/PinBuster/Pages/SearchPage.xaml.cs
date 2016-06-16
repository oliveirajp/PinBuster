using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinBuster.Data;
using PinBuster.Models;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace PinBuster.Pages
{
    public partial class SearchPage : ContentPage
    {
        ObservableCollection<User> listUsers;
        Utilities u;
        public SearchPage()
        {
            InitializeComponent();
            listUsers = new ObservableCollection<User>();
            u = new Utilities();

        }

        public async void OnSearch(object sender, EventArgs e)
        {
            UserView.BeginRefresh();

            string result = await u.MakeGetRequest("utilizador?searchName=" + searchBar.Text);
            listUsers.Clear();
            notFoundLabel.Text = "";
            JObject json = JObject.Parse(result);

            if (!(json["data"].GetType() == typeof(JValue)))
            {
                UserView.IsVisible = true;
                notFoundLabel.IsVisible = false;
                JArray array = JArray.Parse(json["data"].ToString());
                foreach (JObject o in array.Children())
                {
                    User u = new User(o["utilizador_id"].ToString(), o["nome"].ToString(), o["imagem"].ToString(), Double.Parse(o["raio"].ToString()), o["face_id"].ToString());
                    listUsers.Add(u);
                }
                UserView.ItemsSource = listUsers;
            }
            else
            {
                UserView.IsVisible = false;
                notFoundLabel.IsVisible = true;
                
                notFoundLabel.Text = "Sorry! User " + searchBar.Text + " not found!";

            }

            UserView.EndRefresh();
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs args)
        {
            var user = args.Item as User;
            notFoundLabel.IsVisible = true;
            notFoundLabel.Text = user.face_id;
            var upage = new UserPage(user.face_id);
            Navigation.PushAsync(upage);

            ((ListView)sender).SelectedItem = null;

        }
    }
}
