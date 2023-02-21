/**
 * Developed by Rocco Carpi & Riccardo Versetti
 * 16/10/2022
 * 
 */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;


namespace Server
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di classe "Service1" nel codice e nel file di configurazione contemporaneamente.
    public class Service1 : IService1
    {
        #region Attributi
        // dico di prendere i parametri della connectionstring dal file di configiurazione app.config
        private static readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        // inializzo la connessione di tipo mysql
        private static MySqlConnection conn = null;
        #endregion

        #region Getters & Setters
        // definisco la connectionstring come quella definita ereditata in precedenza
        public static string ConnectionString => connectionString;
        public static MySqlConnection Conn { get => conn; set => conn = value; }
        #endregion

        /// <summary>
        /// Controllo se email presente in DB
        /// </summary>
        /// <param name="email"></param>
        /// <returns> true or false a seconda del risultato /returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questa versione, la query è stata modificata per includere un parametro chiamato @Email, che corrisponde all'indirizzo email passato come argomento alla funzione. 
         * La funzione Parameters.AddWithValue viene utilizzata per specificare il valore del parametro in modo sicuro rispetto alle SQL injection.
         */
        public bool CheckEmail(string email)
        {
            try
            {
                bool risultato = true;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT * FROM account WHERE email=@Email";
                    command1.Parameters.AddWithValue("@Email", email.Trim().ToLower());
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        if (reader.HasRows)
                            risultato = false;
                    }
                }

                return risultato;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
                throw new Exception("Errore durante il controllo dell'email");
            }

        }



        /// <summary>
        /// Elimina un prodotto 
        /// </summary>
        /// <param name="id">Identificativo del prodotto da eliminare</param>
        /// <returns>True se il prodotto viene eliminato con successo. False in caso contrario</returns>
        /*
         * In questa versione della funzione, anziché concatenare il valore della variabile id direttamente nella stringa SQL, si utilizza il parametro SQL @id. 
         * In questo modo, il valore della variabile id verrà passato come parametro separato e verrà trattato come dato, 
         * evitando che possa essere interpretato come parte del comando SQL.
         */
        public bool EliminaProdotto(int id)
        {
            try
            {
                bool risultato = false;

                using (MySqlTransaction t = conn.BeginTransaction())
                {
                    try
                    {
                        using (MySqlCommand command1 = conn.CreateCommand())
                        {
                            //cancellare il prodotto dal db mantenendo la categoria
                            command1.CommandText = "UPDATE prodotto SET quantita=0 WHERE IDprodotto=@id";
                            command1.Parameters.AddWithValue("@id", id);
                            if (command1.ExecuteNonQuery() > 0)
                                risultato = true;
                        }
                        t.Commit();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            t.Rollback();
                            Console.WriteLine("Eccezione nel commit", ex.GetType());
                            Console.WriteLine("  Messaggio da commit:", ex.Message.ToString());
                        }
                        catch (Exception ex2)
                        {
                            Console.WriteLine("Eccezione nel rollback", ex2.GetType());
                            Console.WriteLine("  Messaggio del rollback", ex2.Message);
                        }
                    }
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception("Errore in fase di eliminazione del prodotto");
            }
        }

        /// <summary>
        /// Nome di una categoria
        /// </summary>
        /// <param name="id">Identificativo della categoria</param>
        /// <returns>Il nome della categoria</returns>
        /*
         * In questa versione della funzione, anziché concatenare il valore della variabile id direttamente nella stringa SQL, si utilizza il parametro SQL @id. 
         * In questo modo, il valore della variabile id verrà passato come parametro separato e verrà trattato come dato, evitando che possa essere interpretato come parte del comando SQL.
         * Questa versione della funzione è meno vulnerabile alle SQL injection rispetto alla versione originale.
         */
        public string GetCategoria(int id)
        {
            string nome = null;
            try
            {
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT nome FROM categoria WHERE IDcategoria = @id";
                    command1.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nome = reader.GetString(0).TrimEnd();
                        }
                    }
                }

                return nome;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
                throw new Exception("Errore nel recupero della categoria");
            }
        }




        /// <summary>
        /// Ritorna un prodotto in base all'id passato
        /// </summary>
        /// <param name="IDProdotto">Identificativo del prodotto</param>
        /// <returns>Oggetto Articolo</returns>
         /*
         * In questa versione della funzione, anziché concatenare il valore della variabile id direttamente nella stringa SQL, si utilizza il parametro SQL @idProdotto. 
         * In questo modo, il valore della variabile idProdotto verrà passato come parametro separato e verrà trattato come dato, evitando che possa essere interpretato come parte del comando SQL.
         * Questa versione della funzione è meno vulnerabile alle SQL injection rispetto alla versione originale.
         */
        public Articolo GetProdotto(int IDProdotto)
        {
            try
            {
                //creo l'oggetto da restituire
                Articolo prodotto = new Articolo() { IDprodotto = IDProdotto };

                using (MySqlCommand command1 = conn.CreateCommand())
                {   
                    command1.CommandText = "SELECT prodotto.*, categoria.nome FROM prodotto, categoria WHERE prodotto.fk_categoria=categoria.IDcategoria AND IDprodotto=@IDprodotto";
                    command1.Parameters.AddWithValue("@IDprodotto", IDProdotto);

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // controllare ordine campi in tabella db
                            prodotto.Nome = reader.GetString(1).TrimEnd().ToUpper();
                            prodotto.Descrizione = reader.GetString(2).TrimEnd().ToUpper();
                            prodotto.Quantita = reader.GetInt32(4);
                            prodotto.Prezzo = reader.GetDouble(3);
                            prodotto.Categoria = reader.GetString(6).TrimEnd();
                        }
                    }
                }

                //restituisco il prodotto
                return prodotto;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception("Errore nel recupero del prodotto");
            }
        }
        


        /// <summary>
        /// Lista dei nomi delle categorie
        /// </summary>
        /// <returns>lista di stringhe contenti id e nome categroria</returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questa funzione non venendo passato nulla dall'utente inutile prevenire le sql injection in quanto impossibile
         */
        public List<string> ListaCategorie()
        {
            List<string> lista = new List<string>();
            string s;

            try
            {

                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT nome,IDCategoria FROM categoria";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            s = reader.GetString(1) + " " + reader.GetString(0);
                            // lista.Add(reader.GetString(0));
                            // lista.Add(reader.GetString(1));
                            lista.Add(s);
                        }
                    }
                }
                return lista; //ritorno la lista
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
                Console.WriteLine(ex.Message);
                throw new Exception("errore nel recupero della lista delle categorie");
            }

        }


        /// <summary>
        /// Modifica password
        /// </summary>
        /// <param name="email">Email dell'utente</param>
        /// <param name="psw">Password dell'utente</param>
        /// <returns>True se la password è stata modificata. False in caso contrario</returns>

        /* In questa versione della funzione, le variabili email, psw e idLogin non sono concatenate direttamente nella stringa SQL,
         * ma invece utilizzate come parametri nei comandi SQL. Questo rende il codice immune all'SQL injection perché i parametri dei comandi SQL 
         * vengono sanificati automaticamente dal provider del database prima di essere utilizzati nella query
         */
        public bool ModificaPassword(string email, string psw)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    int idLogin = 0;

                    // Utilizza un parametro per la variabile email
                    command1.CommandText = "SELECT IDutente FROM account WHERE email=@Email";
                    command1.Parameters.AddWithValue("@Email", email.Trim().ToLower());

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idLogin = reader.GetInt32(0);
                        }
                    }

                    // Utilizza dei parametri per le variabili password e idLogin
                    command1.CommandText = "UPDATE account SET password=@Password WHERE IDutente=@IDLogin";
                    command1.Parameters.AddWithValue("@Password", psw);
                    command1.Parameters.AddWithValue("@IDLogin", idLogin);

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception("Errore durante la modifica della password");
            }
        }

        /// <summary>
        /// Modifica di un prodotto
        /// </summary>
        /// <param name="prodottoDaModificare">Prodotto da modificare</param>
        /// <returns>True se il prodotto è stato modificato. False in caso contrario</returns>
        /*
         * In questa versione della funzione, le variabili prodottoDaModificare.Categoria, prodottoDaModificare.Nome, prodottoDaModificare.Descrizione, 
         * prodottoDaModificare.Prezzo, id_cat e prodottoDaModificare.IDprodotto non sono concatenate direttamente nella stringa SQL,
         * ma invece utilizzate come parametri nei comandi SQL. Questo rende il codice immune all'SQL injection perché i parametri dei comandi SQL 
         * vengono sanificati automaticamente dal provider del database prima di essere utilizzati nella query.
         */
        public bool ModificaProdotto(Articolo prodottoDaModificare)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    int id_cat = 0;

                    // Utilizza un parametro per la variabile prodottoDaModificare.Categoria
                    command1.CommandText = "SELECT IDcategoria FROM categoria WHERE nome=@NomeCategoria";
                    command1.Parameters.AddWithValue("@NomeCategoria", prodottoDaModificare.Categoria.Trim().ToLower());

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id_cat = reader.GetInt32(0);
                        }
                    }

                    // Utilizza dei parametri per le variabili prodottoDaModificare.Nome, prodottoDaModificare.Descrizione, prodottoDaModificare.Prezzo, id_cat e prodottoDaModificare.IDprodotto
                    command1.CommandText = "UPDATE prodotto SET nome=@Nome, descrizione=@Descrizione, prezzo=@Prezzo, fk_categoria=@IDCategoria WHERE IDprodotto=@IDProdotto";
                    command1.Parameters.AddWithValue("@Nome", prodottoDaModificare.Nome.Trim().ToLower());
                    command1.Parameters.AddWithValue("@Descrizione", prodottoDaModificare.Descrizione == null ? "..." : prodottoDaModificare.Descrizione.Trim().ToLower());
                    command1.Parameters.AddWithValue("@Prezzo", prodottoDaModificare.Prezzo);
                    command1.Parameters.AddWithValue("@IDCategoria", id_cat);
                    command1.Parameters.AddWithValue("@IDProdotto", prodottoDaModificare.IDprodotto);

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Errore nella selezione del prodotto da modificare");
            }
        }


        /// <summary>
        /// Creazione di una nuova categoria data la stringa passata
        /// </summary>
        /// <param name="nome">Nome categoria</param>
        /// <returns>True se la categoria è stata creata. False in caso contrario</returns>
        /*
         * In questa versione, il nome della categoria viene passato come parametro al comando, e viene poi sostituito con il segnaposto "@nome" nel comando SQL. 
         * Il metodo Parameters.AddWithValue() viene usato per associare il valore del parametro con il segnaposto. 
         * In questo modo si evita che il valore passato possa essere utilizzato per un attacco di SQL injection.
         */
        public bool NuovaCategoria(string nome)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "INSERT INTO categoria(nome) VALUES(@nome)";
                    command1.Parameters.AddWithValue("@nome", nome.Trim().ToLower());

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }

                return risultato;
            }
            catch (Exception)
            {
                throw new Exception("Errore nella creazione di una nuova categoria");
            }
        }


        ///<summary>
        /// Crea un nuovo prodotto
        /// </summary>
        /// <param name="nuovo"></param>
        /// <returns>true se successo o false in caso contrario</returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questa versione, anziché concatenare i valori delle variabili direttamente nella query, si utilizzano dei parametri che vengono aggiunti al comando. 
         * In questo modo, il valore effettivo delle variabili viene separato dalla query, eliminando il rischio di SQL injection. 
         * Inoltre, viene utilizzato un comando separato per recuperare l'ID della categoria, anch'esso parametrizzato.
         */
        public bool NuovoProdotto(Articolo nuovo)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    // Preparazione del comando parametrizzato
                    command1.CommandText = "INSERT INTO prodotto(nome, descrizione, prezzo, quantita, fk_categoria) VALUES(@nome, @descrizione, @prezzo, @quantita, @fk_categoria)";

                    // Aggiunta dei parametri al comando
                    command1.Parameters.AddWithValue("@nome", nuovo.Nome.Trim().ToLower());
                    command1.Parameters.AddWithValue("@descrizione", nuovo.Descrizione == null ? "..." : nuovo.Descrizione.Trim().ToLower());
                    command1.Parameters.AddWithValue("@prezzo", nuovo.Prezzo.ToString().Replace(",", "."));
                    command1.Parameters.AddWithValue("@quantita", Convert.ToInt32(nuovo.Quantita));

                    // Recupero dell'ID della categoria
                    int id_cat = 0;
                    using (MySqlCommand command2 = conn.CreateCommand())
                    {
                        command2.CommandText = "SELECT IDcategoria FROM categoria WHERE nome=@nome_categoria";
                        command2.Parameters.AddWithValue("@nome_categoria", nuovo.Categoria.Trim().ToLower());
                        using (MySqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id_cat = reader.GetInt32(0);
                            }
                        }
                    }
                    command1.Parameters.AddWithValue("@fk_categoria", id_cat);

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception("Errore nella creazione di un nuovo prodotto");
            }
        }


        /// <summary>
        /// Registrazione di un nuovo magazziniere
        /// </summary>
        /// <param name="nuovo">Oggetto di tipo Utente</param>
        /// <returns>True se l'utente è stato creato con successo. False in caso contrario</returns>
        /*
         * La funzione utilizza i parametri della query per prevenire le SQL injection. Al posto di concatenare i valori nella stringa di query, 
         * si utilizzano dei placeholder denominati (ad esempio, "@password") e si aggiungono i valori tramite il metodo Parameters.AddWithValue(). In questo modo, 
         * l'interprete SQL eseguirà la query in modo sicuro anche se i valori contengono caratteri speciali o comandi SQL.
         */
        public bool Registrazione(Utente nuovo)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "INSERT INTO account(password,nome,cognome,email,indirizzo,data_nascita,telefono,TipoAccount) " + "VALUES(@password, @nome, @cognome, @email, @indirizzo, @data_nascita, @telefono, '2')";
                    command1.Parameters.AddWithValue("@password", nuovo.Psw);
                    command1.Parameters.AddWithValue("@nome", nuovo.Nome.Trim().ToLower());
                    command1.Parameters.AddWithValue("@cognome", nuovo.Cognome.Trim().ToLower());
                    command1.Parameters.AddWithValue("@email", nuovo.Email.Trim().ToLower());
                    command1.Parameters.AddWithValue("@indirizzo", nuovo.Indirizzo.Trim().ToLower());
                    command1.Parameters.AddWithValue("@data_nascita", nuovo.Data_nascita);
                    command1.Parameters.AddWithValue("@telefono", nuovo.Telefono.Trim());

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception("Errore nella registrazione di un nuovo account");
            }
        }


        /// <summary>
        /// Login di un utente 
        /// </summary>
        /// <param name="user">Oggetto di tipo Login</param>
        /// <returns>0 se le credenziali sono errate. 1 se l'accesso è stato effettuato dall'admin. 2 se l'accesso è stato effettuato dal magazziniere</returns>
        /*
         * In questa versione della funzione, la query SQL contiene il parametro "@Email" invece di concatenare direttamente l'input dell'utente.
         * Il valore dell'input dell'utente viene quindi passato come un parametro separato alla query utilizzando il metodo Parameters.AddWithValue(). 
         * Ciò garantisce che qualsiasi input dannoso inserito dall'utente non possa essere interpretato come parte della query SQL.
         */
        public int UserLogin(Login user)
        {
            try
            {
                int codice = 0;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    // check password with hash
                    command1.CommandText = "SELECT password, TipoAccount FROM account WHERE email=@Email";
                    command1.Parameters.AddWithValue("@Email", user.Email.Trim().ToLower());
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (BCrypt.Net.BCrypt.Verify(user.Password, reader.GetString(0)))
                            {
                                codice = reader.GetInt32(1);
                            }
                        }
                    }
                }
                return codice;
            }
            catch (Exception)
            {
                throw new Exception("Errore in fase di login");
            }
        }



        /// <summary>
        /// lista magazzinieri
        /// </summary>
        /// <returns>lista magazzinieri </returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questa funzione non venendo passato nulla dall'utente inutile prevenire le sql injection in quanto impossibile
         */

        public List<string> ListaMagazzinieri()
        {
            try
            {
                List<string> listaMagazzinieri = new List<string>();

                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    //ottengo email solo magazzinieri
                    command1.CommandText = "SELECT email FROM account WHERE TipoAccount = 2";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaMagazzinieri.Add(reader.GetString(0).TrimEnd().ToUpper());
                        }
                    }
                }
                //restituisco la lista degli identificativi dei clienti
                return listaMagazzinieri;
            }
            catch (Exception)
            {
                throw new Exception("Errore durante il recupero della lista dei magazzinieri");
            }
        }

        /// <summary>
        /// Dati del magazziniere
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questo modo, la query viene costruita utilizzando il parametro @id, invece di concatenare la stringa direttamente nella query. 
         * In questo modo, la funzione è protetta dalle SQL injection perché i valori passati come parametro verranno trattati come tali e non come parte della query.
         */
        public Utente GetMagazziniere(string id)
        {
            try
            {
                // Creo l'oggetto Utente da restituire
                Utente magazziniere = new Utente() { Email = id };

                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT * FROM account WHERE email=@id";
                    command1.Parameters.AddWithValue("@id", id.Trim().ToLower());

                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            magazziniere.Email = reader.GetString(4).TrimEnd().ToUpper();
                            magazziniere.Nome = reader.GetString(2).TrimEnd().ToUpper();
                            magazziniere.Cognome = reader.GetString(3).TrimEnd().ToUpper();
                            magazziniere.Indirizzo = reader.GetString(5).TrimEnd().ToUpper();
                            magazziniere.Data_nascita = reader.GetDateTime(6);
                            magazziniere.Telefono = reader.GetString(7).TrimEnd().ToUpper();
                        }
                    }
                }

                // Restituisco il magazziniere
                return magazziniere;
            }
            catch (Exception)
            {
                throw new Exception("Errore! Impossibile recuperare il magazziniere");
            }
        }


        ///<summary>
        /// Aumenta le giacenze di un prodotto tramite l'id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantita"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questa versione della funzione, viene utilizzato l'utilizzo di parametri per la query, invece di concatenare direttamente i valori delle variabili nella stringa della query. 
         * In particolare, vengono creati i parametri @quantita e @id e, tramite il metodo AddWithValue(), 
         * viene impostato il valore di ciascun parametro in modo sicuro rispetto alle sql injection.
         */
        public bool AumentaGiacenze(int id, int quantita)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "UPDATE prodotto SET quantita = quantita + @quantita WHERE IDProdotto = @id";
                    command1.Parameters.AddWithValue("@quantita", quantita);
                    command1.Parameters.AddWithValue("@id", id);
                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception("Errore! Impossibile aumentare la giacenza del prodotto");
            }
        }


        ///<summary>
        /// Dimuniisce le giacenze di un prodotto tramite l'id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="quantita"></param>
        /// <returns>true or false a seconda del risultato </returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questa versione della funzione, invece di concatenare i valori delle variabili id e quantita direttamente nella stringa SQL, 
         * si utilizzano i parametri SQL @id e @quantita.
         * In questo modo i valori delle variabili vengono passati come parametri separati e verranno trattati come dati, evitando che possano essere interpretati come parte del comando SQL.
         * Inoltre, è stata rimossa la stampa su console del messaggio di errore, in modo che la funzione possa essere utilizzata in contesti diversi dal console. 
         * Al suo posto, è stata aggiunta una eccezione personalizzata che verrà lanciata in caso di errore.
         */

        public bool DiminuisciGiacenze(int id, int quantita)
        {
            int attuale = 0;
            try
            {
                bool risultato = false;

                // controllo della correttezza della quantita spostato nel programma
                // in seguito a un'attenta riflessione avvenuta in doccia

                using (MySqlCommand command0 = conn.CreateCommand())
                {
                    //controlla numero giacenze
                    command0.CommandText = "SELECT quantita from prodotto where IDProdotto = @id";
                    command0.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader reader = command0.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            attuale = reader.GetInt32(0);
                        }
                    }

                    if (attuale < quantita)
                    {
                        Console.WriteLine("Impossibile diminuire giacenze, numero troppo alto");
                        return risultato; //false perchè att < quantita e non è possibile
                    }
                    else
                    {
                        using (MySqlCommand command1 = conn.CreateCommand())
                        {
                            command1.CommandText = "UPDATE prodotto SET quantita = quantita - @quantita WHERE IDProdotto = @id";
                            command1.Parameters.AddWithValue("@quantita", quantita);
                            command1.Parameters.AddWithValue("@id", id);
                            if (command1.ExecuteNonQuery() > 0)
                                risultato = true;
                        }
                    }
                }

                return risultato;
            }
            catch (Exception)
            {
                throw new Exception("Errore! Impossibile diminuire la giacenza del prodotto");
            }
        }


        /// <summary>
        /// lista dei prodotti presenti nel DB
        /// </summary>
        /// <returns>lista di articoli p </returns>
        /// <exception cref="Exception"></exception>
        /*
         * In questa funzione non venendo passato nulla dall'utente inutile prevenire le sql injection in quanto impossibile
         */
        public List<Articolo> ListaProdotti()
        {
            List<Articolo> p = new List<Articolo>();
            try
            {


                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "Select  prodotto.*, categoria.nome from prodotto,categoria where prodotto.fk_categoria=categoria.IDcategoria";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Articolo nuovo = new Articolo
                            {
                                IDprodotto = reader.GetInt32(0),
                                Nome = reader.GetString(1),
                                Descrizione = reader.GetString(2),
                                Prezzo = reader.GetDouble(3),
                                Quantita = reader.GetInt32(4),
                                Categoria = reader.GetString(6)
                            };
                            p.Add(nuovo);

                        }
                    }

                }
                return p;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Errore nel recupero della lista prodotti");
            }

        }

        /// <summary>
        /// controllo se l'ID del prodotto passato è presente o meno nel DB
        /// </summary>
        /// <param name="id">id del prodotto da cercare</param>
        /// <returns> True or False a seconda se è presente o meno </returns>
        /*
         * Nella funzione modificata, la stringa di query SQL include un parametro segnaposto @id invece di concatenare direttamente il valore id.
         * Il metodo cmd.Parameters.AddWithValue() viene quindi utilizzato per legare il valore id al parametro @id in modo sicuro. 
         * Questo approccio aiuta a prevenire gli attacchi di SQL injection, perché il valore id viene trattato come un parametro e non come parte della stringa di query SQL.
         */
        public bool CheckID(int id)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT IDProdotto FROM Prodotto WHERE IDProdotto = @id";
                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }


    }
}
