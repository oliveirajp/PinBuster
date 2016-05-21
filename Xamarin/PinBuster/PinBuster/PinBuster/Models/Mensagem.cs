using System;

namespace PinBuster.Models
{
    class Mensagem
    {
        public int mensagem_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public String data { get; set; }
        public int tempo_limite { get; set; }
        public int raio { get; set; }
        public int face_id { get; set; }
        public String conteudo { get; set;  }
        public String localizacao { get; set; }
        public String categoria { get; set;  }

        public Mensagem(int mensagem_id, double latitude, double longitude, String data, int tempo_limite, int raio, int face_id, String conteudo, String localizacao, String categoria)
        {
            this.mensagem_id = mensagem_id;
            this.latitude = latitude;
            this.longitude = longitude;
            this.data = data;
            this.tempo_limite = tempo_limite;
            this.raio = raio;
            this.face_id = face_id;
            this.conteudo = conteudo;
            this.localizacao = localizacao;
            this.categoria = categoria;
        }
    }
}
