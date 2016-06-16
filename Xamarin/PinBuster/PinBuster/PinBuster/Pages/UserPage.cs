using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PinBuster.Models;

namespace PinBuster
{
  partial class UserPage : TabbedPage
    {
        public string userid;
        public UserPage(string userid)
        {
            var UserInfo = new UserInfo(userid);
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
