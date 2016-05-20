using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBuster.Models
{

    public class MessageListModel
    {
       
        public ObservableCollection<Pin> All_M
        {
            get;
            set;
        }

        public ObservableCollection<Pin> Secret_M
        {
            get;
            set;
        }

        public ObservableCollection<Pin> Review_M
        {
            get;
            set;
        }



        public MessageListModel()
        {

            this.All_M = new ObservableCollection<Pin>();
            this.Secret_M = new ObservableCollection<Pin>();
            this.Review_M = new ObservableCollection<Pin>();

        }
    }

    public class Message_L
    {

        public List<Pin> data { get; set; }
    }
    
}
