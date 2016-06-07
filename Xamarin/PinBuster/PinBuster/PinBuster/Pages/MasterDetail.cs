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
        NavigationPage page;
        public MasterDetail()
        {          
            var menuPage = new MenuPage();
            menuPage.OnMenuSelect = (categoryPage) => {
                page = new NavigationPage(categoryPage);
                page.BarBackgroundColor = Color.White;
                Detail = page;
                IsPresented = false;
            };
            
            Master = menuPage;
            MasterBehavior = MasterBehavior.Popover;
            
            page = new NavigationPage(App.mapPage);
            page.BarBackgroundColor = Color.White;
            NavigationPage.SetTitleIcon(page, "icon.png");
            Detail = page;
        }
    }
}
