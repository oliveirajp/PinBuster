using Xamarin.Forms;
using PinBuster.Data;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using PinBuster.Models;
using Newtonsoft.Json;
using System;

namespace PinBuster.Pages
{
    
    public partial class SearchPage : ContentPage
    {
        Label resultsLabel;
        SearchBar searchBar;
        Utilities getsDb;

        public SearchPage()
        {
            var stack = new StackLayout { Spacing = 0 };
            resultsLabel = new Label
            {
                Text = "Result will appear here.",
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 10
            };

            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                SearchCommand = new Command(async () =>
                {

                    string result = await getsDb.MakeGetRequest("utilizador?searchName=" + searchBar.Text);
                    List<User> listUsers = new List<User>();
                    JObject json = JObject.Parse(result);

                    JArray array = JArray.Parse(json["data"].ToString());
                    resultsLabel.Text = "";
                    foreach (JObject o in array.Children())
                    {
                        resultsLabel.Text = resultsLabel.Text + "Nome: " + o["nome"].ToString() + "\n" + "Face id" + o["face_id"] + "\n-----------";
                        //listUsers.Add(new User(Int32.Parse(o["utilizador_id"].ToString()), o["nome"].ToString(), o["imagem"].ToString(), Double.Parse(o["raio"].ToString()), Int64.Parse(o["face_id"].ToString())));

                    }
                    /*foreach(User user in listUsers)
                    {
                        resultsLabel.Text = resultsLabel.Text + "Nome: " + user.nome.ToString() + "\n" + "Imagem: " + user.imagem.ToString();
                    }*/
                })
            };

            getsDb = new Utilities();

            stack.Children.Add(searchBar);
            stack.Children.Add(resultsLabel);

            Content = stack;

            
        }

    }
}