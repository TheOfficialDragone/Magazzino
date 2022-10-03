using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceHost svcHost = new ServiceHost(typeof(Service1)); //Istanzio il servizio di tipo ServiceAgendaAziendale
                svcHost.Open(); //Apro il servizio

                Console.WriteLine("Servizio Server aperto.");
                Console.WriteLine("Premere un tasto per chiuere il server");
                Console.ReadLine();

                svcHost.Close();
                Console.WriteLine("Servizio chiuso.");
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERRORE nel Server:" + ex.Message);
                Console.ReadLine();
            }
        }
    }
}
