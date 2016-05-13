using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinBuster
{
    public partial class UserPage : TabbedPage
    {
        public UserPage()
        {
            var UserInfo = new UserInfo();
            UserInfo.Title = "Info";

            var Achievements = new UserAchievements();
            Achievements.Title = "Achievements";

            var RecentActivity = new UserRecentActivity();
            RecentActivity.Title = "RecentActivity";

            Children.Add(UserInfo);
            Children.Add(Achievements);
            Children.Add(RecentActivity);

        }
    }
}
