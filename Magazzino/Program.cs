using Magazzino;
using Magazzino.ServiceReference1;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto.Generators;
using Server;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Configuration;

#pragma warning disable IDE0060
/**
 * Developed by Rocco Carpi & Riccardo Versetti
 * 16/10/2022
 * Main principale del programma
 */
namespace Client
{

    class Program
    {

        static void Main(string[] args)

        {
            try
            {
                Service1Client client = new Magazzino.ServiceReference1.Service1Client();

                int sceltaMenuShop = 0;
                do
                {
                    Console.Clear();
                    Console.WriteLine("***GESTIONE MAGAZZINO***");
                    Console.WriteLine("1.Login");
                    Console.WriteLine("2.Registrati");
                    Console.WriteLine("3.Esci");

                    Console.WriteLine("Scelta: ");
                    try
                    {
                        sceltaMenuShop = Convert.ToInt32(Console.ReadLine());

                        switch (sceltaMenuShop)
                        {
                            case 1:
                                Console.Clear();
                                string email, psw;
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("***Login***");
                                    Console.WriteLine("Email:");
                                    email = Console.ReadLine();

                                    if (Validation.ValidateEmail(email) == false)
                                    {
                                        Console.WriteLine("Email non valida!");
                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                        Console.ReadKey();
                                    }

                                } while (Validation.ValidateEmail(email) == false); // chiedo la mail finchè non è valida

                                do
                                {
                                    Console.WriteLine("Inserisci password: ");
                                    // instead of Console.ReadLine() use the following code to hide the password
                                    psw = string.Empty;
                                    ConsoleKeyInfo key;
                                    do
                                    {
                                        key = Console.ReadKey(true);
                                        // Backspace Should Not Work
                                        if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                                        {
                                            psw += key.KeyChar;
                                            Console.Write("*");
                                        }
                                        else
                                        {
                                            if (key.Key == ConsoleKey.Backspace && psw.Length > 0)
                                            {
                                                psw = psw.Substring(0, (psw.Length - 1));
                                                Console.Write("\b \b");
                                            }
                                        }
                                    }
                                    // Stops Receving Keys Once Enter is Pressed
                                    while (key.Key != ConsoleKey.Enter);
                                    Console.WriteLine();

                                } while (psw == "" || psw == " "); //controllo che la password non sia vuota

                                Login login = new Login()
                                {
                                    Email = email.Trim(), //rimuovo gli spazi
                                    Password = psw
                                };

                                int code = client.UserLogin(login); //invio i dati al server e assegno un codice di ritorno

                                switch (code)
                                {
                                    //login errato
                                    case 0:
                                        Console.WriteLine("Login errato!");
                                        Console.WriteLine("\nPremi un tasto per continuare");
                                        Console.ReadKey();
                                        break;

                                    //admin
                                    case 1:
                                        AdminProgram.View();
                                        break;


                                    //Magazziniere
                                    case 2:
                                        MagazziniereProgram.View(login);
                                        //IsMagazziniere(login, psw);
                                        break;
                                }
                                break;
                            //registrazione Magazziniere
                            case 2:
                                NewMagazziniere.View();
                                break;

                            //esci
                            case 3:
                                Console.WriteLine("\nPremi un tasto per terminare il programma");
                                Console.ReadKey();
                                break;
                        }
                    }
                    catch (FormatException)
                    {
                        Console.Clear();
                        Console.WriteLine("Scelta non valida!");
                        Console.WriteLine("\nPremi un tasto per riprovare");
                        Console.ReadKey();
                    }
                    catch (CommunicationException)
                    {
                        Console.Clear();
                        Console.WriteLine("Errore: impossibile raggiungere il server!");
                        Console.WriteLine("\nPremi un tasto per continuare");
                        Console.ReadKey();
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        Console.WriteLine("Errore!");
                        Console.WriteLine("\nPremi un tasto per continuare");
                        Console.ReadKey();
                    }
                } while (sceltaMenuShop != 3);

            }
            catch (Exception)
            {
                Console.WriteLine("Controllare che il server sia attivo");
                Console.WriteLine("Premi un tasto per riprovare\n");
                Console.ReadKey();

            }
        }

    }
}



