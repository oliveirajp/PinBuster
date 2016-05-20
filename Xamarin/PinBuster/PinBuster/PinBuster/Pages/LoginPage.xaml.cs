using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PinBuster
{
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("ola");


        }



        public async void NavigateButton(object sender, EventArgs e)
        {

            System.Diagnostics.Debug.WriteLine("async-----------");

            await Navigation.PushAsync(new Login());

        }


    }
}

