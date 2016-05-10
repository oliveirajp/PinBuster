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

    class MessageListModel
    {
       
        public ObservableCollection<Message_i> All_M
        {
            get;
            set;
        }

        public ObservableCollection<Message_i> Secret_M
        {
            get;
            set;
        }

        public ObservableCollection<Message_i> Review_M
        {
            get;
            set;
        }



        public MessageListModel()
        {

            this.All_M = new ObservableCollection<Message_i>();
            this.Secret_M = new ObservableCollection<Message_i>();
            this.Review_M = new ObservableCollection<Message_i>();
        /*    this.All_M.Add(new Message_i
            {
                Nome = "Corinthians",
                Conteudo = "Sport Club Corinthians Paulista",
                Imagem = "http://graph.facebook.com//picture?type=square"
            });
            this.All_M.Add(new Message_i
            {
                Nome = "São Paulo",
                Conteudo = "São Paulo Futebol Clube",
                Imagem = "http://graph.facebook.com//picture?type=square"
            });
            this.All_M.Add(new Message_i
            {
                Nome = "Santos",
                Conteudo = "Santos Futebol Clube",
                Imagem = "http://graph.facebook.com//picture?type=square"
            });
            this.All_M.Add(new Message_i
            {
                Nome = "Portuguesa",
                Conteudo = "Associação Portuguesa de Desportos",
                Imagem = "http://graph.facebook.com//picture?type=square"
            });
            */
            // this.Review_M = this.Secret_M = this.All_M; 
        }
    }

    public class Message_L
    {

        public List<Message_i> data { get; set; }
    }

    public class Message_i : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string mensagem_id;
        public string Mensagem_id
        {
            get
            {
                return mensagem_id;
            }
            set
            {
                mensagem_id = value;
                this.Notify("mensagem_id");
            }
        }

        double latitude;
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                this.Notify("latitude");
            }
        }

        double longitude;
        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                longitude = value;
                this.Notify("longitude");
            }
        }


        string data;
        public string Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
                this.Notify("data");
            }
        }

        int tempo_limite;
        public int Tempo_limite
        {
            get
            {
                return tempo_limite;
            }
            set
            {
                tempo_limite = value;
                this.Notify("tempo_limite");
            }
        }

        int raio;
        public int Raio
        {
            get
            {
                return raio;
            }
            set
            {
                raio = value;
                this.Notify("raio");
            }
        }


        string face_id;
        public string Face_id
        {
            get
            {
                return face_id;
            }
            set
            {
                face_id = value;
                this.Notify("face_id");
            }
        }

        string conteudo;
        public string Conteudo
        {
            get
            {
                return conteudo;
            }
            set
            {
                conteudo = value;
                this.Notify("conteudo");
            }
        }

        string localizacao;
        public string Localizacao
        {
            get
            {
                return localizacao;
            }
            set
            {
                localizacao = value;
                this.Notify("localizacao");
            }
        }

        string categoria;
        public string Categoria
        {
            get
            {
                return categoria;
            }
            set
            {
                categoria = value;
                this.Notify("categoria");
            }
        }

        string nome;
        public string Nome
        {
            get
            {
                return nome;
            }
            set
            {
                nome = value;
                this.Notify("nome");
            }
        }

        string imagem;
        public string Imagem
        {
            get
            {
                return imagem;
            }
            set
            {
                imagem = value;
                this.Notify("nome");
            }
        }
        public override string ToString()
        {
            return this.conteudo;
        }

        private void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
