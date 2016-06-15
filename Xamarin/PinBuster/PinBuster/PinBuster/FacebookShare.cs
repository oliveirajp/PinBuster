using Android.Content;
using PinBuster;
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

    public partial class FacebookShare : ContentPage
    {
        public static string loginID;
        public static string loginName;
        public static string textFriends;


        public class Auth
        {
            public FacebookShare L;
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
        private StackLayout layout;
        private string result;
        private Label labelPublic;

        public static FacebookShare l;
        
        public FacebookShare(String message)
        {
            StackLayout stack = new StackLayout();
            Label label = new Label { Text = "Clica no butao <- por agora" };
            stack.Children.Add(label);



            // Navigation.PushAsync(new FacebookShare());


            Content = stack;

            this.labelPublic = labelPublic;
            Task t = new Task(LoadData);
            t.Start();
            l = this;


        }

        static void LoadData()
        {
            ISaveAndLoad FacebookFriends = DependencyService.Get<ISaveAndLoad>();
            FacebookFriends.SaveText("facebookshare.txt", "");
            String result = FacebookFriends.LoadText("facebookshare.txt");

            while (result == "")
            {
                result = FacebookFriends.LoadText("facebookshare.txt");
                Debug.WriteLine("result no ciclo" + result);
                int milliseconds = 100;
                Task.Delay(milliseconds).Wait();
            }
            //Debug.WriteLine("ola sou o filipe");
            // Debug.WriteLine("result:" + result);
            //l.Navigation.PushAsync(new ListFollowers("ola"),false);
            //l.Navigation.PushAsync(new ListFollowers("ola"), false);
            l.Navigation.PopToRootAsync();
            //App.NavigateToApp();


        }

        public static implicit operator FacebookFriends(FacebookShare v)
        {
            throw new NotImplementedException();
        }
    }
}
