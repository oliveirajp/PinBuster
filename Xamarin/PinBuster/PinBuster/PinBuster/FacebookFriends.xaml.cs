using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using static PinBuster.App;

namespace PinBuster
{
    public partial class FacebookFriends : ContentPage
    {
        public static string loginID;
        public static string loginName;
        public static string textFriends;


        public class Auth
        {
            public FacebookFriends L;
            public string LoginName;
            public string LoginID
            {

                get { return loginID; }
                set
                {
                    loginID = value;
                    if (loginID != "")
                    {
                        Debug.WriteLine("About to do a popsync");
                        L.Navigation.PopAsync();

                    }
                }
            }
        }



        public static Auth authFacebook;
        public FacebookFriends()
        {
            Task t = new Task(LoadData);
            InitializeComponent();
            t.Start();

            Debug.WriteLine("heeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeereeeeeeeeeeeeee");

            loginID = "";
            loginName = "";
            textFriends = "";
            authFacebook = new Auth();
            authFacebook.LoginID = "";
            authFacebook.LoginName = "";
            authFacebook.L = this;

           // IFacebookLogin face = DependencyService.Get<IFacebookLogin>();
            //face.IFacebookLogin();
            Auth a = new Auth();
            Debug.WriteLine("oooooooooooooooooooooooooooooooooo");
          
;

        }



       static void  LoadData()
        {
            while (textFriends=="")
            {
                Debug.WriteLine("---");
            }
        }

    }
}
