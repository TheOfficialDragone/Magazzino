using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Server
{
    [DataContract]
    internal class Ordine
    {
        [DataMember]
        public int ID_ordine { get; set; }

        [DataMember]
        public List<Articolo> ProdottiOrdinati { get; set; }

        [DataMember]
        public string data { get; set; }

    }
}
