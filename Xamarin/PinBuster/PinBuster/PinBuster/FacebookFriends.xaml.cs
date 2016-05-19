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
                    
                }
            }
        }



        public static Auth authFacebook;
        public static FacebookFriends l;
        private StackLayout layout;

        public FacebookFriends()
        {
           
            

        }

        public FacebookFriends(StackLayout layout)
        {
            this.layout = layout;
            l = this;
            //Followers.Default.L = this;
            //followers.L = this;
            //followers.followers = "ola";

            Task t = new Task(LoadData);
            InitializeComponent();
            t.Start();
        }

        static void  LoadData()
        {
            ISaveAndLoad FacebookFriends = DependencyService.Get<ISaveAndLoad>();
          //  FacebookFriends.DeleteFile("followers2.txt");
            FacebookFriends.SaveText("followers2.txt", "");
            String result = FacebookFriends.LoadText("followers2.txt");

            while (result == "")
            {
                 result = FacebookFriends.LoadText("followers2.txt");
                Debug.WriteLine("result no ciclo"+result);
                int milliseconds = 100;
                Task.Delay(milliseconds).Wait();
            }
            Debug.WriteLine("here2222222222222222222222222222222222222222222222222222222222222");
            // Debug.WriteLine("result:" + result);
            
            l.Navigation.PopModalAsync();

        }

    }
}
