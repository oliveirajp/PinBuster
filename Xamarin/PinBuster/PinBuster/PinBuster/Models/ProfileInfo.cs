using System;

namespace PinBuster.Models
{
    class ProfileInfo
    {
        public int nr_mensagens { get; set; }
        public int nr_followers { get; set; }
        public int nr_followed { get; set; }


        public ProfileInfo(int nr_mensagens, int nr_followers, int nr_followed)
        {
            this.nr_mensagens = nr_mensagens;
            this.nr_followers = nr_followers;
            this.nr_followed = nr_followed;
        }
    }
}
