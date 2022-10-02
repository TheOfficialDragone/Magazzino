using Magazzino.ServiceReference1;
using Server;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
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
                                psw = Console.ReadLine();

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
                                    Console.Clear();
                                    Console.WriteLine("Benvenuto admin");
                                    Console.WriteLine("\nPremi un tasto per continuare");
                                    Console.ReadKey();

                                    int sceltaMenuAdmin = 0;


                                    //INIZIO PAGINA CONTROLLO ADMIN
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("***ADMIN***");
                                        Console.WriteLine("1.Gestisci i prodotti in magazzino");
                                        Console.WriteLine("2.Gestisci magazzinieri");
                                        Console.WriteLine("3.Esci");

                                        Console.WriteLine("Scelta: ");
                                        try
                                        {
                                            sceltaMenuAdmin = Convert.ToInt32(Console.ReadLine());

                                            switch (sceltaMenuAdmin)
                                            {
                                                //gestione prodotti
                                                case 1:
                                                    int sceltaMenuProdotti = 0;
                                                    string disponibile;
                                                    do
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("***GESTIONE PRODOTTI***");
                                                        Console.WriteLine("1.Lista prodotti");
                                                        Console.WriteLine("2.Lista categorie");
                                                        Console.WriteLine("3.Aggiungi prodotto");
                                                        Console.WriteLine("4.Aggiungi categoria");
                                                        Console.WriteLine("5.Modifica prodotto");
                                                        Console.WriteLine("6.Elimina prodotto");
                                                        Console.WriteLine("7.Esci");

                                                        Console.WriteLine("Scelta: ");
                                                        try
                                                        {
                                                            sceltaMenuProdotti = Convert.ToInt32(Console.ReadLine());

                                                            switch (sceltaMenuProdotti)
                                                            {
                                                                //lista prodotti
                                                                case 1:
                                                                    Console.Clear();
                                                                    if (client.ListaProdotti().Count() > 0)
                                                                    {
                                                                        Console.WriteLine("***LISTA PRODOTTI***");
                                                                        //stampo tutti i prodotti
                                                                        foreach (var p in client.ListaProdotti())
                                                                        {
                                                                            if (client.GetProdotto(p).Disponibilita)
                                                                                disponibile = "DISPONIBILE";
                                                                            else
                                                                                disponibile = "NON DISPONIBILE";
                                                                            // TODO: verificare query ed eventuali modifiche
                                                                            Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + disponibile + " - " + client.GetProdotto(p).Categoria);
                                                                        }
                                                                    }
                                                                    else
                                                                        Console.WriteLine("\nNessun prodotto presente nel sistema");

                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    break;
                                                                //lista categorie
                                                                case 2:
                                                                    Console.Clear();
                                                                    if (client.ListaCategorie().Count() > 0)
                                                                    {
                                                                        Console.WriteLine("***LISTA CATEGORIE***");
                                                                        //stampo tutte le categorie
                                                                        foreach (var c in client.ListaCategorie())
                                                                        {
                                                                            // verificare client e query
                                                                            Console.WriteLine("-" + client.GetCategoria(c));
                                                                        }
                                                                    }
                                                                    else
                                                                        Console.WriteLine("Non è presente nessuna categoria nel sistema");

                                                                    Console.WriteLine("\nPremi un tasto per cotinuare");
                                                                    Console.ReadKey();
                                                                    break;
                                                                //aggiungi UN NUOVO prodotto
                                                                case 3:
                                                                    if (client.ListaCategorie().Count() > 0) {
                                                                        string nomeNuovoProdotto;
                                                                        //nome prodotto
                                                                        do
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine("***AGGIUNGI PRODOTTO***");
                                                                            Console.WriteLine("Inserisci il nome del prodotto: ");
                                                                            nomeNuovoProdotto = Console.ReadLine();
                                                                        } while (nomeNuovoProdotto == "" || nomeNuovoProdotto == "");

                                                                        double prezzoNuovoProdotto = 0;
                                                                        //prezzo del prodotto
                                                                        do
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine("***AGGIUNGI PRODOTTO***");
                                                                            Console.WriteLine("Inserisci il prezzo del prodotto: ");
                                                                            try
                                                                            {
                                                                                prezzoNuovoProdotto = Convert.ToDouble(Console.ReadLine());
                                                                            }
                                                                            catch (FormatException)
                                                                            {
                                                                                Console.WriteLine("Prezzo non valido!");
                                                                                Console.WriteLine("Premi un tasto per riprovare");
                                                                                Console.ReadKey();
                                                                            }
                                                                        } while (prezzoNuovoProdotto <= 0);

                                                                        Console.Clear();
                                                                        Console.WriteLine("***AGGIUNGI PRODOTTO***");
                                                                        Console.WriteLine("Inserisci una descrizione del prodotto: ");
                                                                        // verificare coerenza con il DB - campo nullable
                                                                        string descrizioneNuovoProdotto = Console.ReadLine();

                                                                        int categoriaNuovoProdotto = 0;
                                                                        //categoria del prodotto
                                                                        do
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine("***AGGIUNGI PRODOTTO***");

                                                                            Console.WriteLine("\nLista categorie: ");
                                                                            // mostro tutte le categorie all'admin per facilitare la scelta
                                                                            foreach (var c in client.ListaCategorie())
                                                                            {
                                                                                Console.WriteLine(c + "." + client.GetCategoria(c));
                                                                            }

                                                                            Console.WriteLine("\nInserisci codice categoria del prodotto: ");
                                                                            try
                                                                            {
                                                                                categoriaNuovoProdotto = Convert.ToInt32(Console.ReadLine());
                                                                            }
                                                                            catch (FormatException)
                                                                            {
                                                                                Console.WriteLine("Codice categoria non valido!");
                                                                                Console.WriteLine("\nPremi un tasto per riprovare");
                                                                                Console.ReadKey();
                                                                            }

                                                                        } while (client.ListaCategorie().Contains(categoriaNuovoProdotto) == false);

                                                                        // TODO verificare funzionamento con campi db attuali
                                                                        char disponibilitaNuovoProdotto;
                                                                        //disponibilità prodotto
                                                                        do
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine("***AGGIUNGI PRODOTTO***");
                                                                            Console.WriteLine("Il prodotto è disponibile? (s/n)");
                                                                            disponibilitaNuovoProdotto = Convert.ToChar(Console.ReadLine());

                                                                        } while (disponibilitaNuovoProdotto != 's' && disponibilitaNuovoProdotto != 'n');

                                                                        //creo il nuovo prodotto
                                                                        Articolo nuovoProdotto = new Articolo
                                                                        {
                                                                            Nome = nomeNuovoProdotto,
                                                                            Prezzo = prezzoNuovoProdotto,
                                                                            Descrizione = descrizioneNuovoProdotto,
                                                                            Categoria = client.GetCategoria(categoriaNuovoProdotto)
                                                                        };
                                                                        if (disponibilitaNuovoProdotto == 's')
                                                                            nuovoProdotto.Disponibilita = true;
                                                                        else
                                                                            nuovoProdotto.Disponibilita = false;

                                                                        Console.Clear();
                                                                        //salvo il nuovo prodotto nel server
                                                                        if (client.NuovoProdotto(nuovoProdotto))
                                                                            Console.WriteLine("Prodotto aggiunto!");
                                                                        else
                                                                            Console.WriteLine("Errore: prodotto non aggiunto!");
                                                                    }else
                                                                        Console.WriteLine("Devi prima creare una categoria!");

                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    break;

                                                                //aggiungi categoria
                                                                case 4:
                                                                    string nuovaCategoria;
                                                                    bool trovato;
                                                                    //aggiungo nuova categoria e controllo che non sia già presente nel server
                                                                    do
                                                                    {
                                                                        trovato = false; // verifico che la categoria non sia gia presente
                                                                        Console.Clear();
                                                                        Console.WriteLine("***AGGIUNGI CATEGORIA***");
                                                                        Console.WriteLine("\nInserisci la categoria del prodotto: ");
                                                                        nuovaCategoria = Console.ReadLine();

                                                                        foreach (var c in client.ListaCategorie())
                                                                        {
                                                                            if (trovato == false && client.GetCategoria(c) == nuovaCategoria)
                                                                            {
                                                                                trovato = true;
                                                                                Console.WriteLine("\nCategoria già presente!");
                                                                                Console.WriteLine("\nPremi un tasto per continuare");
                                                                                Console.ReadKey();
                                                                            }
                                                                        }

                                                                    } while (nuovaCategoria == "" || nuovaCategoria == " " || trovato == true);

                                                                    if (client.NuovaCategoria(nuovaCategoria))
                                                                        Console.WriteLine("Categoria aggiunta!");
                                                                    else
                                                                        Console.WriteLine("Errore!");

                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    break;

                                                                //modifica prodotto
                                                                case 5:
                                                                    if (client.ListaProdotti().Count() > 0)
                                                                    {
                                                                        int idModifica = 0;

                                                                        Console.Clear();
                                                                        Console.WriteLine("***MODIFICA PRODOTTO***");

                                                                        foreach (var p in client.ListaProdotti())
                                                                        {
                                                                            if (client.GetProdotto(p).Disponibilita)
                                                                                disponibile = "DISPONIBILE";
                                                                            else
                                                                                disponibile = "NON DISPONIBILE";

                                                                                // verificare conversioni e cose stampate
                                                                            Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + disponibile + " - " + client.GetProdotto(p).Categoria);
                                                                        }
                                                                        Console.WriteLine("\nInserisci codice prodotto da modificare: ");
                                                                        try
                                                                        {
                                                                            // sanificato input utente
                                                                            idModifica = Convert.ToInt32(Console.ReadLine());
                                                                            if (client.ListaProdotti().Contains(idModifica))
                                                                            {
                                                                                int sceltaModifica = 0;
                                                                                Articolo prodottoDaModificare = client.GetProdotto(idModifica);

                                                                                do
                                                                                {
                                                                                    // mostro all'utente tutti i campi e i dati del prodotto da modificare
                                                                                    Console.Clear();
                                                                                    Console.WriteLine("Prodotto: " + prodottoDaModificare.IDprodotto);
                                                                                    Console.WriteLine("1.Nome: " + prodottoDaModificare.Nome);
                                                                                    Console.WriteLine("2.Descrizione: " + prodottoDaModificare.Descrizione);
                                                                                    Console.WriteLine("3.Disponibilità: " + prodottoDaModificare.Disponibilita);
                                                                                    Console.WriteLine("4.Prezzo: " + String.Format("{0:0.00}", prodottoDaModificare.Prezzo) + " euro");
                                                                                    Console.WriteLine("5.Categoria: " + prodottoDaModificare.Categoria);
                                                                                    Console.WriteLine("\n6.Annulla");
                                                                                    Console.WriteLine("7.Salva modifiche");

                                                                                    Console.WriteLine("\nScelta: ");
                                                                                    // chiedo quale campo modificare
                                                                                    try
                                                                                    {
                                                                                        sceltaModifica = Convert.ToInt32(Console.ReadLine());

                                                                                        switch (sceltaModifica)
                                                                                        {
                                                                                            case 1:
                                                                                                string nomeModifica;
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("***MODIFICA NOME***");
                                                                                                    Console.WriteLine("Nome attuale: " + prodottoDaModificare.Nome);
                                                                                                    Console.WriteLine("\nInserisci nuovo nome: ");
                                                                                                    nomeModifica = Console.ReadLine();

                                                                                                } while (nomeModifica == "" || nomeModifica == " ");
                                                                                                prodottoDaModificare.Nome = nomeModifica;
                                                                                                break;
                                                                                            case 2:
                                                                                                Console.Clear();
                                                                                                Console.WriteLine("***MODIFICA DESCRIZIONE***");
                                                                                                Console.WriteLine("Descrizione attuale: " + prodottoDaModificare.Descrizione);
                                                                                                Console.WriteLine("\nInserisci nuova descrizione: ");
                                                                                                string descrizioneModifica = Console.ReadLine();

                                                                                                prodottoDaModificare.Descrizione = descrizioneModifica;
                                                                                                break;
                                                                                            case 3:
                                                                                                if (prodottoDaModificare.Disponibilita)
                                                                                                    prodottoDaModificare.Disponibilita = false;
                                                                                                else
                                                                                                    prodottoDaModificare.Disponibilita = true;
                                                                                                break;
                                                                                            case 4:
                                                                                                double prezzoModifica = 0;
                                                                                                //modifica prezzo
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("***MODIFICA PREZZO***");
                                                                                                    Console.WriteLine("Prezzo attuale: " + prodottoDaModificare.Prezzo);
                                                                                                    Console.WriteLine("\nInserisci nuovo prezzo: ");
                                                                                                    try
                                                                                                    {
                                                                                                        prezzoModifica = Convert.ToDouble(Console.ReadLine());
                                                                                                    }
                                                                                                    catch (FormatException)
                                                                                                    {
                                                                                                        Console.WriteLine("Prezzo non valido!");
                                                                                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                                                                                        Console.ReadKey();
                                                                                                    }
                                                                                                } while (prezzoModifica <= 0);
                                                                                                prodottoDaModificare.Prezzo = prezzoModifica;

                                                                                                break;
                                                                                            case 5:
                                                                                                int codiceCat = 0;
                                                                                                //modifica categoria
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("***MODIFICA CATEGORIA***");

                                                                                                    foreach (var c in client.ListaCategorie())
                                                                                                    {
                                                                                                        Console.WriteLine(c + "." + client.GetCategoria(c));
                                                                                                    }
                                                                                                    Console.WriteLine("Inserisci codice categoria: ");
                                                                                                    try
                                                                                                    {
                                                                                                        codiceCat = Convert.ToInt32(Console.ReadLine());
                                                                                                    }
                                                                                                    catch (FormatException)
                                                                                                    {
                                                                                                        Console.WriteLine("Codice categoria non valido!");
                                                                                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                                                                                        Console.ReadKey();
                                                                                                    }
                                                                                                } while (client.ListaCategorie().Contains(codiceCat) == false);
                                                                                                prodottoDaModificare.Categoria = client.GetCategoria(codiceCat);
                                                                                                break;
                                                                                            case 6:
                                                                                                Console.WriteLine("***OPERAZIONE ANNULLATA***");
                                                                                                break;
                                                                                            case 7:
                                                                                                if (client.ModificaProdotto(prodottoDaModificare))
                                                                                                    Console.WriteLine("Prodotto modificato!");
                                                                                                else
                                                                                                    Console.WriteLine("Errore: eliminazione non riuscita!");
                                                                                                break;
                                                                                        }
                                                                                    }
                                                                                    catch (FormatException)
                                                                                    {
                                                                                        Console.WriteLine("Scelta non valida!");
                                                                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                                                                        Console.ReadKey();
                                                                                    }
                                                                                } while (sceltaModifica != 6 && sceltaModifica != 7);
                                                                            }
                                                                            else
                                                                            {
                                                                                Console.WriteLine("Codice prodotto non valido!");
                                                                            }
                                                                        }
                                                                        catch (FormatException)
                                                                        {
                                                                            Console.WriteLine("Scelta non valida!");
                                                                        }
                                                                    }
                                                                    else
                                                                        Console.WriteLine("Non hai creato nessun prodotto");

                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    break;
                                                                //elimina prodotto
                                                                case 6:
                                                                    if (client.ListaProdotti().Count() > 0)
                                                                    {
                                                                        int idElimina = 0;

                                                                        Console.Clear();
                                                                        Console.WriteLine("***ELIMINA PRODOTTO***");
                                                                        foreach (var p in client.ListaProdotti())
                                                                        {
                                                                            if (client.GetProdotto(p).Disponibilita)
                                                                                disponibile = "DISPONIBILE";
                                                                            else
                                                                                disponibile = "NON DISPONIBILE";
                                                                            Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + disponibile + " - " + client.GetProdotto(p).Categoria);
                                                                        }

                                                                        Console.WriteLine("\nInserisci codice prodotto da eliminare: ");
                                                                        try
                                                                        {
                                                                            idElimina = Convert.ToInt32(Console.ReadLine());
                                                                            if (client.ListaProdotti().Contains(idElimina))
                                                                            {
                                                                                Console.Clear();
                                                                                if (client.EliminaProdotto(idElimina))
                                                                                    Console.WriteLine("Prodotto eliminato!");
                                                                                else
                                                                                    Console.WriteLine("Errore: eliminazione non riuscita!");
                                                                            }
                                                                            else
                                                                                Console.WriteLine("Codice prodotto non valido!");
                                                                        }
                                                                        catch (FormatException)
                                                                        {
                                                                            Console.WriteLine("Scelta non valida!");
                                                                        }
                                                                    }else
                                                                        Console.WriteLine("Non hai creato nessun prodotto");

                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    break;
                                                                //esci
                                                                case 7:
                                                                    break;
                                                            }
                                                        }
                                                        catch (FormatException)
                                                        {
                                                            Console.WriteLine("Scelta non valida!");
                                                            Console.WriteLine("\nPremi un tasto per riprovare");
                                                            Console.ReadKey();
                                                        }
                                                    } while (sceltaMenuProdotti != 7);
                                                    break;

                                                //gestione magazzinieri

                                                // controllare coerenza campi ed entita
                                                
                                                case 2:
                                                    Console.Clear();
                                                    if (client.ListaMagazzinieri().Count() > 0)
                                                    {
                                                        Console.WriteLine("***GESTIONE MAGAZZINIERE***");
                                                        //lista dei magazzinieri
                                                        foreach (var c in client.ListaMagazzinieri())
                                                        {
                                                            Console.WriteLine(client.GetMagazziniere(c).Email + " - " + client.GetMagazziniere(c).Nome + 
                                                                " - " + client.GetMagazziniere(c).Cognome + " - " + client.GetMagazziniere(c).Telefono);
                                                        }
                                                    }
                                                    else
                                                        Console.WriteLine("Lista dei magazzinieri vuota");

                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                    Console.ReadKey();
                                                    break;
                                                
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
                                    } while (sceltaMenuAdmin != 3);
                                    break;

                                //FINE CONTROLLI ADMIN 

                                // caso LOGIN Magazziniere
                                case 2:
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
                                        Console.WriteLine("4.Il mio profilo"); // visualizza informazioni del profilo, no modifica
                                        Console.WriteLine("5.Esci");
                                        
                                        
                                        Console.WriteLine("Scelta: ");
                                        try
                                        {
                                            sceltaMenuCliente = Convert.ToInt32(Console.ReadLine());

                                            switch (sceltaMenuCliente)
                                            {
                                                case 1:
                                                    Console.Clear();
                                                    Console.WriteLine("***LISTA PRODOTTI***");
                                                    //stampo tutti i prodotti disponibili
                                                    foreach (var p in client.ListaProdottiDisponibili())
                                                    {
                                                        Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + client.GetProdotto(p).Categoria);
                                                    }

                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                    Console.ReadKey();
                                                    break;

                                                case 2:
                                                    Console.Clear();
                                                    Console.WriteLine("***LISTA GIACENZE***");
                                                    //stampo tutti i prodotti disponibili
                                                    foreach (var p in client.ListaProdottiDisponibili())
                                                    {
                                                        Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + client.GetProdotto(p).Categoria);
                                                    }

                                                    Console.WriteLine("\nInserire l'id del prodotto di cui si vuole aumentare la quantità");

                                                    // prendere in input variabile utente
                                                    // principali controlli su input
                                                    Console.ReadLine();

                                                    break;

                                                case 3:
                                                    Console.Clear();
                                                    Console.WriteLine("***LISTA GIACENZE***");
                                                    //stampo tutti i prodotti disponibili
                                                    foreach (var p in client.ListaProdottiDisponibili())
                                                    {
                                                        Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + client.GetProdotto(p).Categoria);
                                                    }

                                                    Console.WriteLine("\nInserire l'id del prodotto di cui si vuole diminuire la quantità");

                                                     try
                                                     {
                                                        idRiduci = Convert.ToInt32(Console.ReadLine());
                                                        if (client.ListaProdotti().Contains(idRiduci))
                                                            {
                                                                // allora chiedo di quanto ridurlo
                                                                Console.WriteLine("Inserire di quanto si vuole ridurre la quantità");
                                                                quantitaRiduzione = Convert.ToInt32(Console.ReadLine());
                                                                if (quantitaRiduzione > 0)
                                                                {
                                                                    // controllo se la quantita ridotta è maggiore della giacenza
                                                                    if (quantitaRiduzione > client.GetProdotto(idRiduci).disponibilita)
                                                                    {
                                                                        Console.WriteLine("La quantità da ridurre è maggiore della giacenza");
                                                                        Console.WriteLine("\nPremi un tasto per continuare");
                                                                        Console.ReadKey();
                                                                    }
                                                                    else
                                                                    {
                                                                        // allora posso ridurre la giacenza
                                                                        client.DiminuisciGiacenza(idRiduci, quantitaRiduzione);
                                                                        Console.WriteLine("Giacenza ridotta");
                                                                        Console.WriteLine("\nPremi un tasto per continuare");
                                                                        Console.ReadKey();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Console.WriteLine("La quantità da ridurre deve essere maggiore di 0");
                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                }
                                                                Console.Clear();
                                                                if (client.DiminuisciGiacenza(idRiduci, quantitaRiduzione))
                                                                    Console.WriteLine("Quantità diminuita!");
                                                                else
                                                                    Console.WriteLine("Errore: diminuzione della quantità non riuscita!");
                                                            }
                                                        else
                                                            Console.WriteLine("Codice prodotto non valido!");
                                                        }
                                                    catch (FormatException)
                                                        {
                                                            Console.WriteLine("Scelta non valida!");
                                                        }
                                                         
                                                    break;

                                                case 4:
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
                                                                    Console.WriteLine("Email: " + profilo.Email);
                                                                    Console.WriteLine("Nome: " + profilo.Nome);
                                                                    Console.WriteLine("Cognome: " + profilo.Cognome);
                                                                    Console.WriteLine("Data nascita: " + profilo.Data_nascita.ToString("d", CultureInfo.CreateSpecificCulture("es-ES")));
                                                                    Console.WriteLine("Indirizzo: " + profilo.Indirizzo + ", " + profilo.Cap);
                                                                    Console.WriteLine("Telefono: " + profilo.Telefono);

                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    break;
                                                                case 2:
                                                                    //modifica password
                                                                    Console.WriteLine("\nInserisci la password attuale: ");
                                                                    if (Console.ReadLine() == login.Password)
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
                                                                        } while (psw2 == " " || psw == "");


                                                                        if (psw1 == psw2)
                                                                        {
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
                                                case 5:
                                                    break;
                                            }
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Scelta non valida!");
                                            Console.WriteLine("\nPremi un tasto per riprovare");
                                            Console.ReadKey();
                                        }
                                    } while (sceltaMenuCliente != 7);
                                    break;
                            }
                            break;
                        //registrazione Magazziniere
                        case 2:
                            string emailRegister;
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
                                //email non trovata nel sistema

                                //creo nuovo utente
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
                                        nuovo.Psw = password;
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

                                if (client.Signin(nuovo))
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
    }
}