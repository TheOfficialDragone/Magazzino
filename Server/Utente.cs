using System;
using System.Runtime.Serialization;

namespace DbManager
{
    [DataContract]
    public class Utente
    {
        [DataMember]
        public string Email { get; set; }
        
        [DataMember]
        public string Psw { get; set; }
        
        [DataMember]
        public string Nome { get; set; }
        
        [DataMember]
        public string Cognome { get; set; }
        
        [DataMember]
        public string Indirizzo { get; set; }
       
        [DataMember]
        public string Telefono { get; set; }
        
        [DataMember]
        public DateTime Data_nascita { get; set; }
        
        [DataMember]
        public string Cap { get; set; }
    }
}
