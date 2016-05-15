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
    public partial class Login : ContentPage
    {
        public static string loginID;
        public static string loginName;


        public class Auth
        {
            public Login L;
            public string LoginName;
            public string LoginID
            {

                get { return loginID; }
                set
                {
                    loginID = value;
                    if (loginID != "")
                    {
                        Debug.WriteLine("Changed");


                        ISaveCredentials saveCredentials = DependencyService.Get<ISaveCredentials>();
                        saveCredentials.ISaveCredentials(LoginID, LoginName);

                        IGetCredentials getCredentials = DependencyService.Get<IGetCredentials>();
                        String userID = getCredentials.IGetCredentials()[0];
                        Debug.WriteLine("user id after saving:" + userID);

                        L.Navigation.PushModalAsync(new TestPage(LoginName, LoginID));
                        L.Navigation.RemovePage(L);

                    }
                }
            }
        }



        public static Auth authFacebook;
        public Login()
        {
            InitializeComponent();
            loginID = "";
            loginName = "";
            authFacebook = new Auth();
            authFacebook.LoginID = "";
            authFacebook.LoginName = "";
            authFacebook.L = this;

            IFacebookLogin face = DependencyService.Get<IFacebookLogin>();
            //face.IFacebookLogin();
            Auth a = new Auth();

            ThreadA();



            if (Device.OS == TargetPlatform.Windows)
            {
                Debug.WriteLine("antes");
                face.IFacebookLogin();
            }

            //DependencyService.Get<IFacebookLogin>().IFacebookLogin();




        }

        private async void ThreadA()
        {
            while (loginID != "" && loginName != "")
            {
                System.Diagnostics.Debug.WriteLine("Not");
            }
          //  await Navigation.PushAsync(new TestPage(loginName, loginID));
        }



        public async void ChangeUsername(object sender, EventArgs e)
        {



            // await Navigation.PushAsync(new TestPage(loginName,loginID));

        }

        public interface IFacebookLogin
        {
            void IFacebookLogin(); //note that interface members are public by default
        }


    }
}
