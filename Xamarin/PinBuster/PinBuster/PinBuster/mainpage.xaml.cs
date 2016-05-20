using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster
{
    public partial class mainpage : ContentPage
    {
        public mainpage()
        {
            InitializeComponent();
        }


        private void Button_OnClicked2(object sender, EventArgs e)
        {
            var pop = new XLabs.Forms.Controls.PopupLayout();

            // PatientSearch is a ContentView I was to display in the popup
            var search = new View1();
            search.WidthRequest = 300;
            search.HeightRequest = 300;
            search.BackgroundColor = new Color(1, 1, 1, 0.8);
            pop.ShowPopup(search);

        }




    }
}