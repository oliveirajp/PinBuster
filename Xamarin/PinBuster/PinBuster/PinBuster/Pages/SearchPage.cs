using Xamarin.Forms;
using PinBuster.Data;
using System.Linq

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
                    JArr
                    resultsLabel.Text = "Result: \n" + result;


                })
            };

            getsDb = new Utilities();

            stack.Children.Add(searchBar);
            stack.Children.Add(resultsLabel);

            Content = stack;

            
        }

    }
}