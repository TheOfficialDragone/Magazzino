using Client;
using Magazzino.ServiceReference1;
using Server;
using System;
using System.Linq;
using System.ServiceModel;

namespace Magazzino
{
    internal class NewMagazziniere
    {
        public static void View()
        {
            try
            {
                Service1Client client = new Magazzino.ServiceReference1.Service1Client();
                string emailRegister;
                try
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("***Registrazione***");
                        Console.WriteLine("Email:");
                        emailRegister = Console.ReadLine();

                        if (Validation.ValidateEmail(emailRegister) == false)
                        {
                            Console.WriteLine("Email non valida!");
                            Console.WriteLine("\nPremi un tasto per riprovare");
                            Console.ReadKey();
                        }

                    } while (Validation.ValidateEmail(emailRegister) == false);

                    //email già presente nel db?
                    if (client.CheckEmail(emailRegister))
                    {
                        //email non trovata nel sistema, quindi creo nuovo utente

                        Utente nuovo = new Utente { Email = emailRegister };
                        string password;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("***Registrazione***");
                            Console.WriteLine("Password:");
                            password = Console.ReadLine();

                            if (Validation.ValidatePassword(password) == false)
                            {
                                Console.WriteLine("Password non valida!");
                                Console.WriteLine("La password deve avere le seguenti caratteristiche:");
                                Console.WriteLine("- Deve contenere almeno 6 caratteri.");
                                Console.WriteLine("- Deve contenere almeno un carattere minuscolo");
                                Console.WriteLine("- Deve contenere almeno un carattere maiuscolo ");
                                Console.WriteLine("- Deve contenere almeno un numero");
                                Console.WriteLine("- Deve contenere almeno un carattere speciale ('@', '#', '$', '%', '^', '&', '+', '=', '.')");
                                Console.WriteLine("\nPremi un tasto per riprovare");
                                Console.ReadKey();
                            }
                            else
                            {
                                string salt = BCrypt.Net.BCrypt.GenerateSalt(6);
                                string hash = BCrypt.Net.BCrypt.HashPassword(password, salt);

                                nuovo.Psw = hash;

                            }
                        } while (Validation.ValidatePassword(password) == false);

                        string nome;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("***Registrazione***");
                            Console.WriteLine("Nome:");
                            nome = Console.ReadLine();
                        } while (nome == "" || nome == " " || nome.Any(char.IsDigit));
                        nuovo.Nome = nome;

                        string cognome;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("***Registrazione***");
                            Console.WriteLine("Cognome:");
                            cognome = Console.ReadLine();
                        } while (cognome == "" || cognome == " " || cognome.Any(char.IsDigit));
                        nuovo.Cognome = cognome;

                        string indirizzo;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("***Registrazione***");
                            Console.WriteLine("Indirizzo:");
                            indirizzo = Console.ReadLine();
                        } while (indirizzo == "" || indirizzo == " ");
                        nuovo.Indirizzo = indirizzo;

                        string dataString;
                        bool checkData = false;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("***Registrazione***");
                            Console.WriteLine("Data di nascita (Formati accettati: gg/mm/aaaa, gg-mm-aaaa):");
                            dataString = Console.ReadLine();
                            if (dataString.Length == 10 && DateTime.TryParse(dataString, out DateTime data))
                            {
                                checkData = true;
                                nuovo.Data_nascita = data;
                            }
                        } while (checkData == false);

                        string telefono;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("***Registrazione***");
                            Console.WriteLine("Telefono:");
                            telefono = Console.ReadLine();

                        } while (Validation.ValidateTelephone(telefono) == false);
                        nuovo.Telefono = telefono;

                        Console.Clear();

                        if (client.Registrazione(nuovo))
                            Console.WriteLine("Registrazione completata con successo!");
                        else
                            Console.WriteLine("Errore: registrazione non riuscita!");
                    }
                    else
                    {
                        //email già presente nel server
                        Console.Clear();
                        Console.WriteLine("Email già presente nel sistema!");
                    }
                    Console.WriteLine("\nPremi un tasto per continuare");
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

            }
            catch (Exception) { Console.WriteLine("Errore nella visualizzazione del menu di creazione di un nuovo magazziniere"); }
        }
    }
}
