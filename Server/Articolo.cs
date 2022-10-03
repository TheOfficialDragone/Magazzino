using System.Runtime.Serialization;

namespace Server
{
    [DataContract]
    public class Articolo
    {
        [DataMember]
        public int IDprodotto { get; set; }
        
        [DataMember]
        public string Nome { get; set; }
        
        [DataMember]
        public string Descrizione { get; set; }
       
        /*
          [DataMember]
          public bool Disponibilita { get; set; } 
        */
        
        
        [DataMember]
        public double Prezzo { get; set; }
        
        [DataMember]
        public string Categoria { get; set; }
        
        [DataMember]
        public int Quantita { get; set; } = 0;
    }
}
