using System;


namespace PinBuster.Models
{
    class User
    {
        public int utilizador_id { get; set; }
        public string nome { get; set; }
        public string imagem { get; set; }
        public double raio { get; set; }
        public int face_id { get; set; }

        public User(int utilizador_id, string nome, string imagem, double raio, int face_id)
        {
            this.utilizador_id = utilizador_id;
            this.nome = nome;
            this.imagem = imagem;
            this.raio = raio;
            this.face_id = face_id;
        }
    }
}
