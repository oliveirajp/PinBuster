using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBuster.Models
{

class MessageListModel 
    {

    public Task loadTask;
	public ObservableCollection<Item> All_M
    {
        get;
        set;
    }

    public ObservableCollection<Item> Secret_M
    {
        get;
        set;
    }

    public ObservableCollection<Item> Review_M
    {
        get;
        set;
    }


    public async Task LoadInfo()
        {
            string response = "";
            PinBuster.Data.Utilities u = new PinBuster.Data.Utilities();
            response = await u.MakeGetRequest("message_user");

            this.All_M.Add(new Item
          {
                Title = "response",
                Description = response,
                Image = "http://graph.facebook.com//picture?type=square",
                Selected = true
            });
                       System.Diagnostics.Debug.WriteLine("----------------------------------------\n-------------------------------------------------");



        }
        public MessageListModel()
    {
            
        this.All_M = new ObservableCollection<Item>();
        this.Secret_M = new ObservableCollection<Item>();
        this.Review_M = new ObservableCollection<Item>();
            /*     this.All_M.Add(new Item
             {
                 Title = "Corinthians",
                 Description = "Sport Club Corinthians Paulista",
                 Image = "http://graph.facebook.com//picture?type=square",
                 Selected = true
             });
             this.All_M.Add(new Item
             {
                 Title = "São Paulo",
                 Description = "São Paulo Futebol Clube",
                 Image = "http://graph.facebook.com//picture?type=square",
                 Selected = false
             });
             this.All_M.Add(new Item
             {
                 Title = "Santos",
                 Description = "Santos Futebol Clube",
                 Image = "http://graph.facebook.com//picture?type=square",
                 Selected = true
             });
             this.All_M.Add(new Item
             {
                 Title = "Portuguesa",
                 Description = "Associação Portuguesa de Desportos",
                 Image = "http://graph.facebook.com//picture?type=square",
                 Selected = false
             });
                 */

            loadTask = Task.Run(() => LoadInfo());

            // this.Review_M = this.Secret_M = this.All_M; 
        }
}

public class Item : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    string title;

    
    public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
            this.Notify("Title");
        }
    }

    string description;
    public string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
            this.Notify("Description");
        }
    }

    string image;
    public string Image
    {
        get
        {
            return image;
        }
        set
        {
            image = value;
            this.Notify("Image");
        }
    }

    bool selected;
    public bool Selected
    {
        get
        {
            return selected;
        }
        set
        {
            selected = value;
            this.Notify("Selected");
        }
    }

    public override string ToString()
    {
        return this.Title;
    }

    private void Notify(string propertyName)
    {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}
}
