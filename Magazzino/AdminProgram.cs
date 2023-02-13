using Magazzino.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Server;

namespace Magazzino
{
    internal class AdminProgram
    {
        public static void View()
        {
            try
            {
                Service1Client client = new Service1Client();

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
                    Console.WriteLine("2.Lista magazzinieri");
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
                                                List<Articolo> prodotti = new List<Articolo>();
                                                prodotti = client.ListaProdotti().ToList();

                                                foreach (var p in prodotti)
                                                { 
                                                    if (p.Quantita >= 1)
                                                        disponibile = "DISPONIBILE";
                                                    else
                                                        disponibile = "NON DISPONIBILE";
                                                    Console.WriteLine(p.IDprodotto + " - " + p.Nome + " - " + String.Format("{0:0.00}", p.Prezzo) + " euro - " + disponibile + " - " + p.Quantita + " - " + p.Categoria);
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
                                                    client.ListaCategorie();
                                                    foreach (var c in client.ListaCategorie())
                                                    {
                                                        // verificare client e query
                                                        Console.WriteLine("c");
                                                    }
                                                }
                                                else
                                                    Console.WriteLine("Non è presente nessuna categoria nel sistema");

                                                Console.WriteLine("\nPremi un tasto per cotinuare");
                                                Console.ReadKey();
                                                break;
                                            //aggiungi UN NUOVO prodotto
                                            case 3:
                                                if (client.ListaCategorie().Count() > 0)
                                                {
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
                                                            string input;
                                                            input = Console.ReadLine();
                                                            prezzoNuovoProdotto = Double.Parse(input);
                                                        }
                                                        catch (FormatException)
                                                        {
                                                            Console.WriteLine("Prezzo non valido!");
                                                            Console.WriteLine("Premi un tasto per riprovare");
                                                            Console.ReadKey();
                                                        }
                                                    } while (prezzoNuovoProdotto <= 0);

                                                    Console.Clear();
                                                    Console.WriteLine("prezzo: " + prezzoNuovoProdotto);
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
                                                        // mostro tutte le categorie all'admin per facilitare la scelta
                                                        foreach (var c in client.ListaCategorie())
                                                        {
                                                            // qua forse conviene stampare tutte le categorie subito
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

                                                    // chiedi di inserire quantità del prodotto e aggiungilo al db
                                                    int quantitaNuovoProdotto = 0;
                                                    do
                                                    {
                                                        Console.Clear();
                                                        Console.WriteLine("***AGGIUNGI PRODOTTO***");
                                                        Console.WriteLine("Inserisci la quantità del prodotto: ");
                                                        try
                                                        {
                                                            quantitaNuovoProdotto = Convert.ToInt32(Console.ReadLine());
                                                        }
                                                        catch (FormatException)
                                                        {
                                                            Console.WriteLine("Quantità non valida!");
                                                            Console.WriteLine("\nPremi un tasto per riprovare");
                                                            Console.ReadKey();
                                                        }
                                                    } while (quantitaNuovoProdotto < 0);

                                                    // aggiungi il prodotto al db
                                                    Articolo nuovoProdotto = new Articolo
                                                    {
                                                        Nome = nomeNuovoProdotto,
                                                        Prezzo = prezzoNuovoProdotto,
                                                        Descrizione = descrizioneNuovoProdotto,
                                                        Categoria = client.GetCategoria(categoriaNuovoProdotto),
                                                        Quantita = quantitaNuovoProdotto
                                                    };

                                                    if (client.NuovoProdotto(nuovoProdotto))
                                                    {
                                                        Console.WriteLine("Prodotto aggiunto con successo!");
                                                        Console.WriteLine("\nPremi un tasto per continuare");
                                                        Console.ReadKey();
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Errore nell'aggiunta del prodotto!");
                                                        Console.WriteLine("\nPremi un tasto per continuare");
                                                        Console.ReadKey();
                                                    }

                                                }
                                                else
                                                {
                                                    Console.WriteLine("Devi prima creare una categoria!");
                                                    Console.WriteLine("\nPremi un tasto per continuare");
                                                    Console.ReadKey();
                                                }
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
                                                        Articolo articolo = client.GetProdotto(p);
                                                        if (articolo.Quantita > 0)
                                                            disponibile = "DISPONIBILE";
                                                        else
                                                            disponibile = "NON DISPONIBILE";
                                                        Console.WriteLine(articolo.IDprodotto + " - " + articolo.Nome + " - " + String.Format("{0:0.00}", articolo.Prezzo) + " euro - " + disponibile + " - " + articolo.Categoria);
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
                                                                Console.WriteLine("3.Prezzo: " + String.Format("{0:0.00}", prodottoDaModificare.Prezzo) + " euro");
                                                                Console.WriteLine("4.Categoria: " + prodottoDaModificare.Categoria);
                                                                Console.WriteLine("5.Annulla");
                                                                Console.WriteLine("6.Salva modifiche");

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
                                                                        case 4:
                                                                            int codiceCat = 0;
                                                                            //modifica categoria
                                                                            do
                                                                            {
                                                                                Console.Clear();
                                                                                Console.WriteLine("***MODIFICA CATEGORIA***");

                                                                                foreach (var c in client.ListaCategorie())
                                                                                {
                                                                                    // anche qui metodo per stampare subito tutte le categorie
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
                                                                        case 5:
                                                                            Console.WriteLine("***OPERAZIONE ANNULLATA***");
                                                                            break;
                                                                        case 6:
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
                                                        Articolo articolo = client.GetProdotto(p);
                                                        if (client.GetProdotto(p).Quantita > 0)
                                                            disponibile = "DISPONIBILE";
                                                        else
                                                            disponibile = "NON DISPONIBILE";
                                                        Console.WriteLine(articolo.IDprodotto + " - " + articolo.Nome + " - " + String.Format("{0:0.00}", articolo.Prezzo) + " euro - " + disponibile + " - " + articolo.Categoria);
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
                                                }
                                                else
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
                            case 2:
                                Console.Clear();
                                if (client.ListaMagazzinieri().Count() > 0)
                                {
                                    Console.WriteLine("***LISTA MAGAZZINIERI***");
                                    // print lista magazzinieri
                                    foreach (var m in client.ListaMagazzinieri())
                                    {
                                        Console.WriteLine(m);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Lista dei magazzinieri vuota");
                                }

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
                } while (sceltaMenuAdmin != 3);
            }

            catch (Exception) { Console.WriteLine("Errore nella visualizzazione del menu Admin"); }
        }
    }
}
