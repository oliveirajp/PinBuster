using System;

namespace PinBuster.Models
{
    class Achievement
    {
        public String face_id { get; set; }
        public String nome { get; set; }
        public int MessagesNeeded { get; set; }
        public int MessagesFound { get; set; }

        public Achievement(String face_id, String nome, int MessagesNeeded, int MessagesFound)
        {
            this.face_id = face_id;
            this.nome = nome;
            this.MessagesNeeded = MessagesNeeded;
            this.MessagesFound = MessagesFound;
        }
    }
}
