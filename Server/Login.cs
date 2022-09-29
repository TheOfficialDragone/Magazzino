using System.Runtime.Serialization;

namespace DbManager
{
    [DataContract]
    public class Login
    {
        [DataMember]
        public string Email { get; set; }
        
        [DataMember]
        public string Password { get; set; }
    }
}
