
using DbManager;
using Magazzino.ServiceReference1;
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
                Console.WriteLine("***SHOP***");
                Console.WriteLine("1.Login");
                Console.WriteLine("2.Registrati");
                Console.WriteLine("3.Esci");

                Console.WriteLine("Scelta: ");
                try
                {
                    sceltaMenuShop = Convert.ToInt32(Console.ReadLine());

                    switch (sceltaMenuShop)
                    {
                        //login
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

                            } while (Validation.ValidateEmail(email) == false);

                            do
                            {
                                Console.WriteLine("Inserisci password: ");
                                psw = Console.ReadLine();

                            } while (psw == "" || psw == " ");

                            Login login = new Login()
                            {
                                Email = email.Trim(),
                                Password = psw
                            };
                            
                            int code = client.UserLogin(login);

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

                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("***ADMIN***");
                                        Console.WriteLine("1.Gestisci prodotti");
                                        Console.WriteLine("2.Gestisci clienti");
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
                                                                    if (client.ListaProdotti().Count > 0)
                                                                    {
                                                                        Console.WriteLine("***LISTA PRODOTTI***");
                                                                        //stampo tutti i prodotti
                                                                        foreach (var p in client.ListaProdotti())
                                                                        {
                                                                            if (client.GetProdotto(p).Disponibilita)
                                                                                disponibile = "DISPONIBILE";
                                                                            else
                                                                                disponibile = "NON DISPONIBILE";
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
                                                                    if (client.ListaCategorie().Count > 0)
                                                                    {
                                                                        Console.WriteLine("***LISTA CATEGORIE***");
                                                                        //stampo tutte le categorie
                                                                        foreach (var c in client.ListaCategorie())
                                                                        {
                                                                            Console.WriteLine("-" + client.GetCategoria(c));
                                                                        }
                                                                    }
                                                                    else
                                                                        Console.WriteLine("Non è presente nessuna categoria nel sistema");

                                                                    Console.WriteLine("\nPremi un tasto per cotinuare");
                                                                    Console.ReadKey();
                                                                    break;
                                                                //aggiungi prodotto
                                                                case 3:
                                                                    if (client.ListaCategorie().Count > 0) {
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
                                                                        string descrizioneNuovoProdotto = Console.ReadLine();

                                                                        int categoriaNuovoProdotto = 0;
                                                                        //categoria del prodotto
                                                                        do
                                                                        {
                                                                            Console.Clear();
                                                                            Console.WriteLine("***AGGIUNGI PRODOTTO***");

                                                                            Console.WriteLine("\nLista categorie: ");

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
                                                                        trovato = false;
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
                                                                    if (client.ListaProdotti().Count > 0)
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
                                                                            Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + disponibile + " - " + client.GetProdotto(p).Categoria);
                                                                        }
                                                                        Console.WriteLine("\nInserisci codice prodotto da modificare: ");
                                                                        try
                                                                        {
                                                                            idModifica = Convert.ToInt32(Console.ReadLine());
                                                                            if (client.ListaProdotti().Contains(idModifica))
                                                                            {
                                                                                int sceltaModifica = 0;
                                                                                Articolo prodottoDaModificare = client.GetProdotto(idModifica);

                                                                                do
                                                                                {
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
                                                                    if (client.ListaProdotti().Count > 0)
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

                                                //gestione clienti
                                                case 2:
                                                    Console.Clear();
                                                    if (client.ListaClienti().Count > 0)
                                                    {
                                                        Console.WriteLine("***GESTIONE CLIENTI***");
                                                        //lista dei clienti
                                                        foreach (var c in client.ListaClienti())
                                                        {
                                                            Console.WriteLine(client.GetCliente(c).Email + " - " + client.GetCliente(c).Nome + 
                                                                " - " + client.GetCliente(c).Cognome + " - " + client.GetCliente(c).Telefono);
                                                        }
                                                    }
                                                    else
                                                        Console.WriteLine("Lista dei clienti vuota");

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

                                //cliente
                                case 2:
                                    Console.Clear();
                                    Console.WriteLine("Benvenuto cliente " + login.Email);
                                    Console.WriteLine("\nPremi un tasto per continuare");
                                    Console.ReadKey();

                                    int sceltaMenuCliente = 0;
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("***" + login.Email + "***");
                                        Console.WriteLine("1.Lista prodotti");
                                        Console.WriteLine("2.Acquista prodotti");
                                        Console.WriteLine("3.I miei prodotti preferiti");
                                        Console.WriteLine("4.I miei ordini");
                                        Console.WriteLine("5.I miei indirizzi");
                                        Console.WriteLine("6.Il mio profilo");
                                        Console.WriteLine("7.Esci");

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
                                                //Acquisto
                                                case 2:
                                                    if (client.ListaIndirizzi(login.Email).Count > 0)
                                                    {
                                                        //lista dei prodotti
                                                        List<int> listaprodotti = client.ListaProdottiDisponibili();
                                                        //carrello
                                                        List<Articolo> carrello = new List<Articolo>();
                                                        //oggetto acquisto
                                                        Acquisto acquisto = new Acquisto();
                                                        Articolo prodottoScelto;
                                                        int scelta_prodotto = 0, quantita = 0;
                                                        double importo = 0;
                                                        bool terminato = false;
                                                        string scelta_terminazione;
                                                        do
                                                        {
                                                            try
                                                            {
                                                                do
                                                                {
                                                                    Console.Clear();
                                                                    if (carrello.Count == 0)
                                                                        Console.WriteLine("Carrello: vuoto\n");
                                                                    else
                                                                        Console.WriteLine("Numero prodotti nel carrello: {0}\n", carrello.Count);

                                                                    foreach (var p in listaprodotti)
                                                                    {
                                                                        Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " + client.GetProdotto(p).Nome + " - " + String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " + client.GetProdotto(p).Categoria);
                                                                    }
                                                                    Console.WriteLine("\n\nInserisci codice del prodotto: ");
                                                                    scelta_prodotto = Convert.ToInt32(Console.ReadLine());

                                                                    if(listaprodotti.Contains(scelta_prodotto) == false)
                                                                    {
                                                                        Console.WriteLine("Scelta non valida!");
                                                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                                                        Console.ReadKey();
                                                                    }
                                                                } while (listaprodotti.Contains(scelta_prodotto) == false);
                                                                
                                                                Console.WriteLine("Quantita: ");

                                                                quantita = Convert.ToInt32(Console.ReadLine());
                                                                if (quantita > 0)
                                                                {
                                                                    //prodotto scelto
                                                                    prodottoScelto = client.GetProdotto(scelta_prodotto);
                                                                    //imposto la quantità del prodotto scelto
                                                                    prodottoScelto.Quantita = quantita;
                                                                    //se il carrello non contiene già il prodotto, lo aggiunge e calcola l'importo
                                                                    if (carrello.Contains(prodottoScelto) == false)
                                                                    {
                                                                        carrello.Add(prodottoScelto);
                                                                        importo += (prodottoScelto.Prezzo * quantita);
                                                                    }
                                                                    else
                                                                    {
                                                                        //il prodotto è già presente nel carrello, quindi basta cambiare la quantità
                                                                        carrello.Find((p) => p.IDprodotto == prodottoScelto.IDprodotto).Quantita += quantita;
                                                                    }
                                                                    
                                                                    Console.WriteLine("\nProdotto aggiunto al carrello!");
                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    do
                                                                    {
                                                                        Console.Clear();
                                                                        //stampa del carrello
                                                                        Console.WriteLine("Carrello: ");
                                                                        foreach(var p in carrello)
                                                                        {
                                                                            Console.WriteLine("{0} x{1}",p.Nome, p.Quantita);
                                                                        }
                                                                        Console.WriteLine("\nVuoi terminare l'acquisto?(s/n)");
                                                                        scelta_terminazione = Console.ReadLine();
                                                                        if (scelta_terminazione == "s")
                                                                            terminato = true;

                                                                    } while (scelta_terminazione != "s" && scelta_terminazione != "n");
                                                                }
                                                            }
                                                            catch (FormatException)
                                                            {
                                                                Console.WriteLine("Scelta non valida!");
                                                                Console.WriteLine("\nPremi un tasto per riprovare");
                                                                Console.ReadKey();
                                                            }
                                                        } while (terminato == false);

                                                        if (terminato)
                                                        {
                                                            //salvo il carrello e l'importo nell'oggetto acquisto
                                                            acquisto.Carrello = carrello;
                                                            acquisto.Importo = importo;
                                                            
                                                            //scelta dell'indirizzo di spedizione
                                                            int indirizzo = 0;
                                                            List<int> ListaIndirizzi = client.ListaIndirizzi(login.Email);
                                                            do
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine("***Scelta indirizzo di spedizione***");

                                                                foreach (var i in ListaIndirizzi)
                                                                {
                                                                    Console.WriteLine(client.GetIndirizzo(i).ID +
                                                                        "." + client.GetIndirizzo(i).Alias +
                                                                        "," + client.GetIndirizzo(i).Nome +
                                                                        "," + client.GetIndirizzo(i).Cognome +
                                                                        "," + client.GetIndirizzo(i).Recapito +
                                                                        "," + client.GetIndirizzo(i).Citta +
                                                                        "," + client.GetIndirizzo(i).Cap +
                                                                        "," + "(" + client.GetIndirizzo(i).Provincia + ")");
                                                                }

                                                                Console.WriteLine("\nInserisci codice indirizzo: ");
                                                                try
                                                                {
                                                                    indirizzo = Convert.ToInt32(Console.ReadLine());
                                                                }
                                                                catch (FormatException)
                                                                {
                                                                    Console.WriteLine("Scelta non valida!");
                                                                    Console.WriteLine("\nPremi un tasto per riprovare");
                                                                    Console.ReadKey();
                                                                }


                                                            } while (ListaIndirizzi.Contains(indirizzo) == false);
                                                            acquisto.IDindirizzo = indirizzo;

                                                            int metodo_spedizione = 0;
                                                            //scelta del metodo di spedizione
                                                            do
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine("***Scelta metodo spedizione***");

                                                                foreach (var ms in client.ListaMetodiSpedizione())
                                                                {
                                                                    Console.WriteLine(ms + "." + client.GetMetodoSpedizione(ms));
                                                                }

                                                                Console.WriteLine("\nScelta: ");
                                                                try
                                                                {
                                                                    metodo_spedizione = Convert.ToInt32(Console.ReadLine());

                                                                }
                                                                catch (FormatException)
                                                                {
                                                                    Console.WriteLine("Scelta metodo spedizione non valida!");
                                                                    Console.WriteLine("\nPremi un tasto per riprovare");
                                                                    Console.ReadKey();
                                                                }
                                                            } while (client.ListaMetodiSpedizione().Contains(metodo_spedizione) == false);
                                                            acquisto.MetodoSpedizione = metodo_spedizione;

                                                            int metodo_pagamento = 0;
                                                            //scelta del metodo di pagamento
                                                            do
                                                            {
                                                                Console.Clear();
                                                                Console.WriteLine("***Scelta metodo pagamento***");

                                                                foreach (var mp in client.ListaMetodiPagamento())
                                                                {
                                                                    Console.WriteLine(mp + "." + client.GetMetodoPagamento(mp));
                                                                }

                                                                Console.WriteLine("\nScelta: ");
                                                                try
                                                                {
                                                                    metodo_pagamento = Convert.ToInt32(Console.ReadLine());
                                                                }
                                                                catch (FormatException)
                                                                {
                                                                    Console.WriteLine("Scelta metodo pagamento non valida!");
                                                                    Console.WriteLine("\nPremi un tasto per riprovare");
                                                                    Console.ReadKey();
                                                                }
                                                            } while (client.ListaMetodiPagamento().Contains(metodo_pagamento) == false);
                                                            acquisto.MetodoPagamento = metodo_pagamento;

                                                            acquisto.User = login.Email;
                                                            //creo l'acquisto nel server
                                                            if (client.Acquisto(acquisto))
                                                                Console.WriteLine("Acquisto concluso");
                                                            else
                                                                Console.WriteLine("Errore");
                                                        }
                                                    }
                                                    else
                                                        Console.WriteLine("Devi inserire almeno un indirizzo di spedizione nei tuoi indirizzi!");

                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                    Console.ReadKey();
                                                    break;
                                                //lista prodotti preferiti
                                                case 3:
                                                    int sceltaMenuPreferiti = 0, codice = 0;
                                                    do
                                                    {
                                                        Console.Clear();
                                                        if (client.ListaProdottiPreferiti(login.Email).Count > 0)
                                                        {
                                                            Console.WriteLine("***I MIEI PREFERITI***");

                                                            foreach (var p in client.ListaProdottiPreferiti(login.Email))
                                                            {
                                                                Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " +
                                                                    client.GetProdotto(p).Nome + " - " +
                                                                    String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " +
                                                                    client.GetProdotto(p).Categoria);
                                                            }
                                                        }
         
                                                        else
                                                            Console.WriteLine("Non hai aggiunto nessun prodotto ai tuoi preferiti!");

                                                        Console.WriteLine("\n1.Aggiungi nuovo preferito");
                                                        Console.WriteLine("2.Rimuovi preferito");
                                                        Console.WriteLine("3.Esci");

                                                        Console.WriteLine("Scelta: ");
                                                        try
                                                        {
                                                            sceltaMenuPreferiti = Convert.ToInt32(Console.ReadLine());

                                                            switch (sceltaMenuPreferiti)
                                                            {
                                                                //aggiunta preferito
                                                                case 1:
                                                                    codice = 0;
                                                                    Console.Clear();
                                                                    Console.WriteLine("***NUOVO PREFERITO***");
                                                                    Console.WriteLine("Lista prodotti:");
                                                                    foreach (var p in client.ListaProdottiDisponibili())
                                                                    {
                                                                        Console.WriteLine(client.GetProdotto(p).IDprodotto + " - " +
                                                                            client.GetProdotto(p).Nome + " - " +
                                                                            String.Format("{0:0.00}", client.GetProdotto(p).Prezzo) + " euro - " +
                                                                            client.GetProdotto(p).Categoria);
                                                                    }

                                                                    Console.WriteLine("\nInserisci codice del prodotto da aggiungere ai preferiti: ");
                                                                    try
                                                                    {
                                                                        codice = Convert.ToInt32(Console.ReadLine());

                                                                        if (client.ListaProdottiDisponibili().Contains(codice))
                                                                            client.NuovoPreferito(login.Email, codice);
                                                                        else
                                                                        {
                                                                            Console.WriteLine("\nScelta non valida!");
                                                                            Console.WriteLine("\nPremi un tasto per riprovare");
                                                                            Console.ReadKey();
                                                                        }
                                                                    }
                                                                    catch (FormatException)
                                                                    {
                                                                        Console.WriteLine("\nScelta non valida!");
                                                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                                                        Console.ReadKey();
                                                                    }

                                                                    break;
                                                                //rimozione preferito
                                                                case 2:
                                                                    codice = 0;
                                                                    Console.WriteLine("\n***ELIMINA PREFERITO***");
                                                                    Console.WriteLine("\nInserisci codice del prodotto da rimuovere dai preferiti: ");
                                                                    try
                                                                    {
                                                                        codice = Convert.ToInt32(Console.ReadLine());

                                                                        if (client.ListaProdottiPreferiti(login.Email).Contains(codice))
                                                                            client.EliminaPreferito(login.Email, codice);
                                                                        else
                                                                        {
                                                                            Console.WriteLine("Scelta non valida!");
                                                                            Console.WriteLine("\nPremi un tasto per riprovare");
                                                                            Console.ReadKey();
                                                                        }
                                                                    }
                                                                    catch (FormatException)
                                                                    {
                                                                        Console.WriteLine("Scelta non valida!");
                                                                        Console.WriteLine("\nPremi un tasto per riprovare");
                                                                        Console.ReadKey();
                                                                    }
                                                                    break;
                                                                //esci
                                                                case 3:
                                                                    break;
                                                            }
                                                        }
                                                        catch (FormatException)
                                                        {
                                                            Console.WriteLine("\nScelta non valida!");
                                                            Console.WriteLine("\nPremi un tasto per riprovare");
                                                            Console.ReadKey();
                                                        }
                                                    } while (sceltaMenuPreferiti != 3);
                                                    break;
                                                //lista degli ordini effettuati
                                                case 4:
                                                    if (client.ListaOrdini(login.Email).Count > 0)
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("***I MIEI ORDINI***");
                                                        Acquisto infoAcquisto = new Acquisto();
                                                        foreach (var acq in client.ListaOrdini(login.Email))
                                                        {
                                                            infoAcquisto = client.GetOrdine(acq);
                                                            Console.WriteLine("Numero d'ordine: {0}", infoAcquisto.IDacquisto);
                                                            Console.WriteLine("Importo: {0} euro", String.Format("{0:0.00}", infoAcquisto.Importo));
                                                            Console.WriteLine("Data: {0}", infoAcquisto.Data_ora.ToString("d", CultureInfo.CreateSpecificCulture("es-ES")));
                                                            Console.WriteLine("Indirizzo: {0}", client.GetIndirizzo(infoAcquisto.IDindirizzo).Alias);
                                                            Console.WriteLine("Metodo pagamento: {0}", client.GetMetodoPagamento(infoAcquisto.MetodoPagamento));
                                                            Console.WriteLine("Metodo spedizione: {0}", client.GetMetodoSpedizione(infoAcquisto.MetodoSpedizione));
                                                            Console.WriteLine("Stato ordine: {0}", client.GetStatoOrdine(infoAcquisto.IDstatoOrdine));
                                                            Console.WriteLine("\nLista prodotti:");
                                                            
                                                            foreach (var art in client.GetOrdine(acq).Carrello)
                                                            {
                                                                Console.WriteLine("{0} x{1}", art.Nome, art.Quantita);
                                                            }
                                                            Console.WriteLine("\n");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Non hai fatto nessun ordine!");
                                                    }

                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                    Console.ReadKey();
                                                    break;
                                                //lista degli indirizzi
                                                case 5:
                                                    int sceltaMenuIndirizzi = 0, codiceIndirizzo = 0;
                                                    do
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("***I MIEI INDIRIZZI***");
                                                        if (client.ListaIndirizzi(login.Email).Count > 0)
                                                        {
                                                            foreach (var ind in client.ListaIndirizzi(login.Email))
                                                            {
                                                                Console.WriteLine(client.GetIndirizzo(ind).ID +
                                                                    ". " + client.GetIndirizzo(ind).Alias +
                                                                    ", " + client.GetIndirizzo(ind).Nome +
                                                                    ", " + client.GetIndirizzo(ind).Cognome +
                                                                    ", " + client.GetIndirizzo(ind).Recapito +
                                                                    ", " + client.GetIndirizzo(ind).Citta +
                                                                    ", " + client.GetIndirizzo(ind).Cap +
                                                                    ", " + "(" + client.GetIndirizzo(ind).Provincia + ")");
                                                            }
                                                        }
                                                        else
                                                            Console.WriteLine("Non hai ancora aggiunto un indirizzo!");

                                                        Console.WriteLine("\n1.Aggiungi indirizzo");
                                                        Console.WriteLine("2.Modifica indirizzo");
                                                        Console.WriteLine("3.Elimina Indirizzo");
                                                        Console.WriteLine("4.Esci");

                                                        Console.WriteLine("Scelta: ");
                                                        try
                                                        {
                                                            sceltaMenuIndirizzi = Convert.ToInt32(Console.ReadLine());

                                                            switch (sceltaMenuIndirizzi)
                                                            {
                                                                //nuovo indirizzo
                                                                case 1:
                                                                    Indirizzo nuovoIndirizzo = new Indirizzo();
                                                                    do
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine("***NUOVO INDIRIZZO***");
                                                                        Console.WriteLine("Alias: ");
                                                                        nuovoIndirizzo.Alias = Console.ReadLine();
                                                                    } while (nuovoIndirizzo.Alias == "" || nuovoIndirizzo.Alias == " ");

                                                                    do
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine("***NUOVO INDIRIZZO***");
                                                                        Console.WriteLine("Nome: ");
                                                                        nuovoIndirizzo.Nome = Console.ReadLine();
                                                                    } while (nuovoIndirizzo.Nome == "" || nuovoIndirizzo.Nome == " " || nuovoIndirizzo.Nome.Any(char.IsDigit));

                                                                    do
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine("***NUOVO INDIRIZZO***");
                                                                        Console.WriteLine("Cognome: ");
                                                                        nuovoIndirizzo.Cognome = Console.ReadLine();
                                                                    } while (nuovoIndirizzo.Cognome == "" || nuovoIndirizzo.Cognome == " " || nuovoIndirizzo.Cognome.Any(char.IsDigit));

                                                                    do
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine("***NUOVO INDIRIZZO***");
                                                                        Console.WriteLine("Recapito: ");
                                                                        nuovoIndirizzo.Recapito = Console.ReadLine();
                                                                    } while (nuovoIndirizzo.Recapito == "" || nuovoIndirizzo.Recapito == " ");

                                                                    string cap;
                                                                    bool checkCap = false;
                                                                    do
                                                                    {
                                                                        Console.Clear();
                                                                        Console.WriteLine("***NUOVO INDIRIZZO***");
                                                                        Console.WriteLine("Cap: ");
                                                                        cap = Console.ReadLine();

                                                                        if (client.ListaCap().Contains(cap))
                                                                        {
                                                                            nuovoIndirizzo.Cap = cap;
                                                                            checkCap = true;
                                                                        }
                                                                        else
                                                                        {
                                                                            Console.WriteLine("Cap non valido!");
                                                                            Console.WriteLine("Premi un tasto per riprovare");
                                                                            Console.ReadKey();
                                                                        }
                                                                    } while (checkCap == false);

                                                                    nuovoIndirizzo.User = login.Email;

                                                                    Console.Clear();

                                                                    if (client.NuovoIndirizzo(nuovoIndirizzo))
                                                                        Console.WriteLine("Indirizzo aggiunto!");
                                                                    else
                                                                        Console.WriteLine("Errore:aggiunta non riuscita!");

                                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                                    Console.ReadKey();
                                                                    break;
                                                                //modifica indirizzo
                                                                case 2:
                                                                    if (client.ListaIndirizzi(login.Email).Count > 0)
                                                                    {
                                                                        int sceltaModifica = 0;
                                                                        codiceIndirizzo = 0;
                                                                        Console.WriteLine("\n***MODIFICA INDIRIZZO***");

                                                                        Console.WriteLine("Inserisci codice dell'indirizzo da modificare: ");
                                                                        try
                                                                        {
                                                                            codiceIndirizzo = Convert.ToInt32(Console.ReadLine());

                                                                            if (client.ListaIndirizzi(login.Email).Contains(codiceIndirizzo))
                                                                            {

                                                                                Indirizzo ind = client.GetIndirizzo(codiceIndirizzo);
                                                                                do
                                                                                {
                                                                                    Console.Clear();
                                                                                    Console.WriteLine("Indirizzo: " + ind.ID);
                                                                                    Console.WriteLine("1.Alias: " + ind.Alias);
                                                                                    Console.WriteLine("2.Nome: " + ind.Nome);
                                                                                    Console.WriteLine("3.Cognome: " + ind.Cognome);
                                                                                    Console.WriteLine("4.Recapito: " + ind.Recapito);
                                                                                    Console.WriteLine("5.Citta': {0}, {1}", ind.Citta, ind.Cap);
                                                                                    Console.WriteLine("\n6.Annulla");
                                                                                    Console.WriteLine("7.Salva modifiche");

                                                                                    Console.WriteLine("\nScelta: ");
                                                                                    try
                                                                                    {
                                                                                        sceltaModifica = Convert.ToInt32(Console.ReadLine());

                                                                                        switch (sceltaModifica)
                                                                                        {
                                                                                            case 1:
                                                                                                string nAlias;
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("\n***MODIFICA ALIAS***");
                                                                                                    Console.WriteLine("Alias attuale: " + ind.Alias);
                                                                                                    Console.WriteLine("\nInserisci nuovo alias: ");
                                                                                                    nAlias = Console.ReadLine();
                                                                                                } while (nAlias == "" || nAlias == " ");

                                                                                                ind.Alias = nAlias;
                                                                                                break;
                                                                                            case 2:
                                                                                                string nNome;
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("\n***MODIFICA NOME***");
                                                                                                    Console.WriteLine("Nome attuale: " + ind.Nome);
                                                                                                    Console.WriteLine("\nInserisci nuovo nome: ");
                                                                                                    nNome = Console.ReadLine();
                                                                                                } while (nNome == "" || nNome == " " || nNome.Any(char.IsDigit));
                                                                                                ind.Nome = nNome;
                                                                                                break;
                                                                                            case 3:
                                                                                                string nCognome;
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("\n***MODIFICA COGNOME***");
                                                                                                    Console.WriteLine("Cognome attuale: " + ind.Cognome);
                                                                                                    Console.WriteLine("\nInserisci nuovo cognome: ");
                                                                                                    nCognome = Console.ReadLine();
                                                                                                } while (nCognome == "" || nCognome == " " || nCognome.Any(char.IsDigit));
                                                                                                ind.Cognome = nCognome;
                                                                                                break;
                                                                                            case 4:
                                                                                                string nRecap;
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("\n***MODIFICA RECAPITO***");
                                                                                                    Console.WriteLine("Recapito attuale: " + ind.Recapito);
                                                                                                    Console.WriteLine("\nInserisci nuovo recapito: ");
                                                                                                    nRecap = Console.ReadLine();
                                                                                                } while (nRecap == "" || nRecap == " ");
                                                                                                ind.Recapito = nRecap;
                                                                                                break;
                                                                                            case 5:
                                                                                                string nCap;
                                                                                                bool checkNcap = false;
                                                                                                do
                                                                                                {
                                                                                                    Console.Clear();
                                                                                                    Console.WriteLine("\n***MODIFICA CITTA' E CAP***");
                                                                                                    Console.WriteLine("Cap attuale: " + ind.Cap);
                                                                                                    Console.WriteLine("\nInserisci nuovo cap: ");
                                                                                                    nCap = Console.ReadLine();

                                                                                                    if (client.ListaCap().Contains(nCap))
                                                                                                    {
                                                                                                        ind.Cap = nCap;
                                                                                                        checkNcap = true;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        Console.WriteLine("Cap non valido!");
                                                                                                        Console.WriteLine("Premi un tasto per riprovare");
                                                                                                        Console.ReadKey();
                                                                                                    }

                                                                                                } while (nCap == " " || nCap == "" || checkNcap == false);
                                                                                                break;
                                                                                            case 6:
                                                                                                Console.WriteLine("\n***OPERAZIONE ANNULLATA***");
                                                                                                break;
                                                                                            case 7:
                                                                                                if (client.ModificaIndirizzo(ind))
                                                                                                    Console.WriteLine("***Indirizzo modificato!***");
                                                                                                else
                                                                                                    Console.WriteLine("Errore: modifica non riuscita!");
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
                                                                                Console.WriteLine("Indirizzo non valido!");
                                                                            }
                                                                        }
                                                                        catch (FormatException)
                                                                        {
                                                                            Console.WriteLine("Scelta non valida!");
                                                                        }

                                                                        Console.WriteLine("\nPremi un tasto per continuare");
                                                                        Console.ReadKey();
                                                                    }
                                                                    break;
                                                                //elimina indirizzo
                                                                case 3:
                                                                    if (client.ListaIndirizzi(login.Email).Count > 0)
                                                                    {
                                                                        Console.WriteLine("\n***ELIMINA INDIRIZZO***");
                                                                        codiceIndirizzo = 0;
                                                                        Console.WriteLine("Inserisci codice dell'indirizzo da rimuovere: ");
                                                                        try
                                                                        {
                                                                            codiceIndirizzo = Convert.ToInt32(Console.ReadLine());
                                                                            if (client.ListaIndirizzi(login.Email).Contains(codiceIndirizzo))
                                                                            {
                                                                                if (client.EliminaIndirizzo(codiceIndirizzo))
                                                                                    Console.WriteLine("Indirizzo eliminato!");
                                                                                else
                                                                                    Console.WriteLine("Errore: eliminazione non riuscita!");
                                                                            }
                                                                            else
                                                                            {
                                                                                Console.WriteLine("Indirizzo non valido!");
                                                                            }

                                                                            Console.WriteLine("\nPremi un tasto per continuare");
                                                                            Console.ReadKey();
                                                                        }
                                                                        catch (FormatException)
                                                                        {
                                                                            Console.WriteLine("Scelta indirizzo non valida!");
                                                                            Console.WriteLine("\nPremi un tasto per continuare");
                                                                            Console.ReadKey();
                                                                        }
                                                                    }
                                                                    break;
                                                                //esci
                                                                case 4:
                                                                    break;
                                                            }
                                                        }
                                                        catch (FormatException)
                                                        {
                                                            Console.WriteLine("Scelta non valida!");
                                                            Console.WriteLine("\nPremi un tasto per riprovare");
                                                            Console.ReadKey();
                                                        }
                                                    } while (sceltaMenuIndirizzi != 4);
                                                    break;
                                                case 6:
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

                                                                    Utente profilo = client.GetCliente(login.Email);
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
                                    } while (sceltaMenuCliente != 7);
                                    break;
                            }
                            break;
                        //registrazione cliente
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

                                string cap;
                                bool checkCap = false;
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("***Registrazione***");
                                    Console.WriteLine("Cap:");
                                    cap = Console.ReadLine();

                                    if (client.ListaCap().Contains(cap))
                                    {
                                        nuovo.Cap = cap;
                                        checkCap = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cap non valido!");
                                        Console.WriteLine("Premi un tasto per riprovare");
                                        Console.ReadKey();
                                    }
                                } while (checkCap == false);

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