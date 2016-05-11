using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PinBuster.Pages
{
    public class MasterDetail : MasterDetailPage
    {
        public MasterDetail(IGetCurrentPosition loc)
        {          
            var menuPage = new MenuPage(loc);
            menuPage.OnMenuSelect = (categoryPage) => {
                Detail = new NavigationPage(categoryPage);
                IsPresented = false;
            };
            
            Master = menuPage;

            //Detail = new NavigationPage(new MapPage(loc));
            Detail = new NavigationPage(new UserPage());
        }
    }
}
