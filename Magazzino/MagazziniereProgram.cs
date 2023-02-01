using Client;
using Magazzino.ServiceReference1;
using Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Magazzino
{
    internal class MagazziniereProgram
    {
        public static void View(Login login)
        {
            try
            {
                Service1Client client = new Magazzino.ServiceReference1.Service1Client();

                Console.Clear();
                Console.WriteLine("Benvenuto magazziniere " + login.Email);
                Console.WriteLine("\nPremi un tasto per continuare");
                Console.ReadKey();

                int sceltaMenuCliente = 0;
                do
                {
                    Console.Clear();
                    Console.WriteLine("***" + login.Email + "***");
                    Console.WriteLine("1.Lista prodotti"); // visualizza prodotti in magazzino
                    Console.WriteLine("2.Aumenta giacenza"); // aumenta giacenza prodotto
                    Console.WriteLine("3.Diminuisci giacenza");
                    Console.WriteLine("4.Prodotti in esaurimento");
                    Console.WriteLine("5.Il mio profilo"); // visualizza informazioni del profilo, no modifica
                    Console.WriteLine("6.Esci");


                    Console.WriteLine("Scelta: ");
                    try
                    {
                        sceltaMenuCliente = Convert.ToInt32(Console.ReadLine());
                        string disponibile;
                        string disponibilita;
                        switch (sceltaMenuCliente)
                        {
                            case 1:
                                Console.Clear();
                                Console.WriteLine("***LISTA PRODOTTI***");
                                Console.Clear();
                                if (client.ListaProdotti().Count() > 0)
                                {
                                    Console.WriteLine("***LISTA PRODOTTI***");
                                    //stampo tutti i prodotti
                                    foreach (var p in client.ListaProdotti())
                                    {
                                        Articolo a = client.GetProdotto(p);
                                        if (client.GetProdotto(p).Quantita >= 1)
                                            disponibile = "DISPONIBILE";
                                        else
                                            disponibile = "NON DISPONIBILE";
                                        Console.WriteLine(a.IDprodotto + " - " + a.Nome + " - " + String.Format("{0:0.00}", a.Prezzo) + " euro - " + disponibile + " - " + a.Quantita + " - " + a.Categoria);
                                    }
                                }
                                else
                                    Console.WriteLine("\nNessun prodotto presente nel sistema");
                                Console.WriteLine("\nPremi un tasto per continuare");
                                Console.ReadKey();
                                break;

                            case 2:
                                Console.Clear();
                                Console.WriteLine("***AUMENTA GIACENZE***");
                                //stampo tutti i prodotti disponibili

                                if (client.ListaProdotti().Count() > 0)
                                {
                                    Console.WriteLine("***LISTA PRODOTTI***");
                                    //stampo tutti i prodotti
                                    foreach (var z in client.ListaProdotti())
                                    {
                                        Articolo a = client.GetProdotto(z);
                                        if (client.GetProdotto(z).Quantita >= 1)
                                            disponibile = "DISPONIBILE";
                                        else
                                            disponibile = "NON DISPONIBILE";

                                        Console.WriteLine(a.IDprodotto + " - " + a.Nome + " - " + String.Format("{0:0.00}", a.Prezzo) + " euro - " + disponibile + " - " + a.Quantita + " - " + a.Categoria);
                                    }
                                }
                                int id = 0;
                                int quantita = 0;
                                Console.WriteLine("\nInserire l'id del prodotto di cui si vuole aumentare la quantità");
                                id = Convert.ToInt32(Console.ReadLine());

                                Console.WriteLine("\nInserire la quantità da aumentare");
                                quantita = Convert.ToInt32(Console.ReadLine());

                                if (client.AumentaGiacenze(id, quantita))
                                {
                                    Console.WriteLine("GIACENZA AUMENTATA");
                                    Console.ReadLine();
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("ERRORE GIACENZA");
                                    Console.ReadLine();
                                    break;
                                }

                            case 3:
                                try
                                {
                                    if (client.ListaProdotti().Count() > 0)
                                    {
                                        Console.WriteLine("***LISTA PRODOTTI***");
                                        //stampo tutti i prodotti
                                        foreach (var z in client.ListaProdotti())
                                        {
                                            Articolo a = client.GetProdotto(z);
                                            if (client.GetProdotto(z).Quantita >= 1)
                                                disponibile = "DISPONIBILE";
                                            else
                                                disponibile = "NON DISPONIBILE";
                                            Console.WriteLine(a.IDprodotto + " - " + a.Nome + " - " + String.Format("{0:0.00}", a.Prezzo) + " euro - " + disponibile + " - " + a.Quantita + " - " + a.Categoria);
                                        }
                                    }
                                    int id_prod = 0;
                                    int quantita_diminuita = 0;

                                    Console.WriteLine("\nInserire l'id del prodotto di cui si vuole aumentare la quantità");
                                    id_prod = Convert.ToInt32(Console.ReadLine());

                                    Console.WriteLine("\nInserire la quantità da diminuire");
                                    quantita_diminuita = Convert.ToInt32(Console.ReadLine());

                                    if (client.DiminuisciGiacenze(id_prod, quantita_diminuita))
                                    {
                                        Console.WriteLine("GIACENZA DIMINUITA");
                                        Console.ReadLine();
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERRORE GIACENZA");
                                        Console.ReadLine();
                                        break;
                                    }
                                }
                                catch (FormatException)
                                {
                                    Console.WriteLine("Scelta non valida!");
                                    Console.ReadLine();
                                }

                                break;

                            case 4:
                                Console.Clear();
                                Console.WriteLine("***PRODOTTI IN ESAURIMENTO***");
                                Console.Clear();
                                if (client.ListaProdotti().Count() > 0)
                                {
                                    Console.WriteLine("***LISTA PRODOTTI IN ESAURIMENTO***");
                                    //stampo tutti i prodotti
                                    foreach (var p in client.ListaProdotti())
                                    {
                                        Articolo a = client.GetProdotto(p);
                                        if (client.GetProdotto(p).Quantita <= 2)
                                        {
                                            disponibilita = "IN ESAURIMENTO";
                                            Console.WriteLine(a.IDprodotto + " - " + a.Nome + " - " + String.Format("{0:0.00}", a.Prezzo) + " euro - " + disponibilita + " - " + a.Quantita + " - " + a.Categoria);
                                        }


                                    }
                                }
                                else
                                    Console.WriteLine("\nNessun prodotto presente nel sistema");
                                Console.WriteLine("\nPremi un tasto per continuare");
                                Console.ReadKey();
                                break;


                            case 5:
                                int sceltaMenuDati = 0;
                                //dati dell'utente
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("***IL MIO PROFILO***");
                                    Console.WriteLine("1.I miei dati");
                                    Console.WriteLine("2.Aggiorna password");
                                    Console.WriteLine("3.Esci");

                                    Console.WriteLine("\nScelta: ");
                                    try
                                    {
                                        sceltaMenuDati = Convert.ToInt32(Console.ReadLine());

                                        switch (sceltaMenuDati)
                                        {
                                            case 1:
                                                Console.Clear();
                                                //informazioni dell'utente
                                                Console.WriteLine("***I MIEI DATI***");

                                                Utente profilo = client.GetMagazziniere(login.Email);
                                                CultureInfo ci = CultureInfo.InvariantCulture;
                                                Console.WriteLine("Email: " + profilo.Email);
                                                Console.WriteLine("Nome: " + profilo.Nome);
                                                Console.WriteLine("Cognome: " + profilo.Cognome);
                                                Console.WriteLine("Data nascita: " + profilo.Data_nascita.ToString("dd-MM-yyyy"));
                                                Console.WriteLine("Indirizzo: " + profilo.Indirizzo);
                                                Console.WriteLine("Telefono: " + profilo.Telefono);

                                                Console.WriteLine("\nPremi un tasto per continuare");
                                                Console.ReadKey();
                                                break;
                                            case 2:
                                                //modifica password
                                                string psw_attuale;
                                                Console.WriteLine("\nInserisci la password attuale: ");
                                                psw_attuale = Console.ReadLine();
                                                if (psw_attuale == login.Password)
                                                {
                                                    string psw1, psw2;
                                                    do
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("***AGGIORNA PASSWORD***");
                                                        Console.WriteLine("Inserisci nuova password: ");
                                                        psw1 = Console.ReadLine();

                                                        if (Validation.ValidatePassword(psw1) == false)
                                                        {
                                                            Console.WriteLine("Password non valida!");
                                                            Console.WriteLine("La password deve avere le seguenti caratteristiche:");
                                                            Console.WriteLine("- Deve contenere almeno 6 caratteri.");
                                                            Console.WriteLine("- Deve contenere almeno un carattere minuscolo");
                                                            Console.WriteLine("- Deve contenere almeno un carattere maiuscolo ");
                                                            Console.WriteLine("- Deve contenere almeno un numero");
                                                            Console.WriteLine("- Deve contenere almeno un carattere speciale ('@', '#', '$', '%', '^', '&', '+', '=', '.')");
                                                            Console.WriteLine("Premi un tasto per riprovare");
                                                            Console.ReadKey();
                                                        }
                                                    } while (Validation.ValidatePassword(psw1) == false);

                                                    do
                                                    {
                                                        Console.WriteLine("Conferma nuova password: ");
                                                        psw2 = Console.ReadLine();
                                                    } while (psw2 == " " || psw2 == "");



                                                    if (psw1 == psw2)
                                                    {
                                                        string salt = BCrypt.Net.BCrypt.GenerateSalt(6);
                                                        string hash = BCrypt.Net.BCrypt.HashPassword(psw2, salt);

                                                        //salvo hash e salt nel db
                                                        psw2 = hash;

                                                        if (client.ModificaPassword(login.Email, psw2))
                                                        {
                                                            login.Password = psw2;
                                                            Console.WriteLine("Password modificata!");
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Errore: modifica non effettuata!");
                                                            Console.WriteLine("\nLa password deve avere le seguenti caratteristiche:");
                                                            Console.WriteLine("- Deve contenere almeno 6 caratteri.");
                                                            Console.WriteLine("- Deve contenere almeno un carattere minuscolo");
                                                            Console.WriteLine("- Deve contenere almeno un carattere maiuscolo ");
                                                            Console.WriteLine("- Deve contenere almeno un numero");
                                                            Console.WriteLine("- Deve contenere almeno un carattere speciale ('@', '#', '$', '%', '^', '&', '+', '=', '.')");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Errore: le due password non coincidono!");
                                                        Console.WriteLine("\nLa password deve avere le seguenti caratteristiche:");
                                                        Console.WriteLine("- Deve contenere almeno 6 caratteri.");
                                                        Console.WriteLine("- Deve contenere almeno un carattere minuscolo");
                                                        Console.WriteLine("- Deve contenere almeno un carattere maiuscolo ");
                                                        Console.WriteLine("- Deve contenere almeno un numero");
                                                        Console.WriteLine("- Deve contenere almeno un carattere speciale ('@', '#', '$', '%', '^', '&', '+', '=', '.')");
                                                    }
                                                }
                                                else
                                                    Console.WriteLine("La password inserita è errata!");

                                                Console.WriteLine("\nPremi un tasto per continuare");
                                                Console.ReadKey();
                                                break;
                                            //esci
                                            case 3:
                                                break;
                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        Console.WriteLine("Scelta non valida!");
                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                        Console.ReadKey();
                                    }
                                } while (sceltaMenuDati != 3);
                                break;
                            case 6:
                                break;
                        }
                    }
                    catch (FormatException)
                    {
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
                } while (sceltaMenuCliente != 6);

            }
            catch (Exception) { Console.WriteLine("Errore nella visualizzazione del menu Magazziniere"); }
        }
    }
}
