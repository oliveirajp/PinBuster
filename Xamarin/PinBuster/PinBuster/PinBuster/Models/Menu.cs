using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PinBuster.Models
{
    class Menu
    {
        public string Name
        {
            get;
            set;
        }

        public Func<Page> PageFn
        {
            get;
            set;
        }

        public string Image
        {
            get;
            set;
        }

        public Menu(string name, string image, Func<Page> pageFn)
        {
            Name = name;
            Image = image;
            PageFn = pageFn;
        }
    }
}
