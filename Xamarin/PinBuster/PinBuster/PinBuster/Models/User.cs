using System;


namespace PinBuster.Models
{
    class User
    {
        public string utilizador_id { get; set; }
        public string nome { get; set; }
        public string imagem { get; set; }
        public double raio { get; set; }
        public string face_id { get; set; }

        public User(string utilizador_id, string nome, string imagem, double raio, string face_id)
        {
            this.utilizador_id = utilizador_id;
            this.nome = nome;
            this.imagem = imagem;
            this.raio = raio;
            this.face_id = face_id;
        }
    }
}
