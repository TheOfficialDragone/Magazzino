/**
 * Developed by Rocco Carpi & Riccardo Versetti
 * 16/10/2022
 * 
 */
using System.Runtime.Serialization;

namespace Server
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
