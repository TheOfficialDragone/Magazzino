/**
 * Developed by Rocco Carpi & Riccardo Versetti
 * 16/10/2022
 * 
 */
using MySql.Data.MySqlClient;
using System;
using System.ServiceModel;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ServiceHost svcHost = new ServiceHost(typeof(Service1));
                svcHost.Open(); //Apro il servizio

                Service1.Conn = new MySqlConnection(Service1.ConnectionString);
                Service1.Conn.Open();
                Console.WriteLine("Servizio Server aperto.");
                Console.WriteLine("Premere un tasto per chiudere il server");
                Console.ReadLine();

                svcHost.Close();
                Console.WriteLine("Servizio chiuso.");
            }

            catch (Exception ex)
            {
                Console.WriteLine("ERRORE nel Server:" + ex.Message.ToString());
                Console.ReadLine();
            }
        }
    }
}
