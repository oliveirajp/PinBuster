﻿using System;
using System.ComponentModel;

namespace PinBuster.Models
{
    public class Pin : IEquatable<Pin> , INotifyPropertyChanged
    {
        public Xamarin.Forms.Maps.Pin ActualPin { get; set; }
            
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
            }
        }

        int visivel;
        public int Visivel
        {
            get
            {
                return visivel;
            }
            set
            {
                visivel = value;
            }
        }
        bool show = true;
        public bool Show {

            get
            {
                return show;
            }
            set
            {
                show = value;
                NotifyPropertyChanged("SHOW");
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
            }
        }

        string title;
        public string Nome
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        string imagem;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        public string Imagem
        {
            get
            {
                return imagem;
            }
            set
            {
                imagem = value;
            }
        }

        public bool Equals(Pin other)
        {
            return this.conteudo == other.Conteudo && this.face_id == other.Face_id;
        }
    }
}