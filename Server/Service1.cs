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

        public bool CheckEmail(string email)
        {
            try
            {
                bool risultato = true;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT * FROM account WHERE email='" + email.Trim().ToLower() + "'";
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


        /// Elimina un prodotto 
        /// </summary>
        /// <param name="id">Identificativo del prodotto da eliminare</param>
        /// <returns>True se il prodotto viene eliminato con successo. False in caso contrario</returns>
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
                            //cancellare il prodotto dal db mantenedo la categoria
                            command1.CommandText = "UPDATE prodotto SET quantita=0 WHERE IDprodotto=" + id;
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

        /// Nome di una categoria
        /// </summary>
        /// <param name="id">Identificativo della categoria</param>
        /// <returns>Il nome della categoria</returns>
        public string GetCategoria(int id)
        {
            string nome = null;
            try
            {
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT nome FROM categoria WHERE IDcategoria=" + id;
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



        /// Dati del prodotto
        /// <param name="IDProdotto">Identificativo del prodotto</param>
        /// <returns>Oggetto Articolo</returns>
        /// 
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
        


        //Lista dei nomi delle categorie
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


        /// Modifica la password
        /// <param name="email">Email dell'utente</param>
        /// <param name="psw">Password dell'utente</param>
        /// <returns>True se la password è stata modificata. False in caso contrario</returns>
        public bool ModificaPassword(string email, string psw)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    int idLogin = 0;
                    command1.CommandText = "SELECT IDutente FROM account WHERE email='" + email.Trim().ToLower() + "'";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idLogin = reader.GetInt32(0);
                        }
                    }

                    command1.CommandText = "UPDATE account SET " +
                                            "password='" + psw + "' " +
                                           "WHERE IDutente=" + idLogin;
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

        //Modifica del prodotto
        /// <param name="prodottoDaModificare">Prodotto da modificare</param>
        /// <returns>True se il prodotto è stato modificato. False in caso contrario</returns>
        public bool ModificaProdotto(Articolo prodottoDaModificare)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    int id_cat = 0;
                    command1.CommandText = "SELECT IDcategoria FROM categoria WHERE nome='" + prodottoDaModificare.Categoria.Trim().ToLower() + "'";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id_cat = reader.GetInt32(0);
                        }
                    }
                    int disp = Convert.ToInt32(prodottoDaModificare.Quantita);
                    string descrizione;
                    if (prodottoDaModificare.Descrizione == null)
                        descrizione = "...";
                    else
                        descrizione = prodottoDaModificare.Descrizione.Trim().ToLower();

                    command1.CommandText = "UPDATE prodotto SET " + "nome='" + prodottoDaModificare.Nome.Trim().ToLower() + "', " + "descrizione='" + descrizione + "', " + "prezzo=" + prodottoDaModificare.Prezzo.ToString() + ", fk_categoria=" + id_cat + " WHERE IDprodotto=" + prodottoDaModificare.IDprodotto;

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

        //Crea una nuova categoria
        /// <param name="nome">Nome categoria</param>
        /// <returns>True se la categoria è stata creata. False in caso contrario</returns>
        public bool NuovaCategoria(string nome)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "INSERT INTO categoria(nome) VALUES('" + nome.Trim().ToLower() + "')";
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

        //Crea un nuovo prodotto
        /// <param name="nuovo">Oggetto di tipo Articolo</param>
        /// <returns>True se il prodotto è stato creato. False in caso contrario</returns>
        public bool NuovoProdotto(Articolo nuovo)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    int id_cat = 0;
                    command1.CommandText = "SELECT IDcategoria FROM categoria WHERE nome='" + nuovo.Categoria.Trim().ToLower() + "'";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id_cat = reader.GetInt32(0);
                        }
                    }
                    int disp = Convert.ToInt32(nuovo.Quantita);
                    string descrizione;
                    if (nuovo.Descrizione == null)
                        descrizione = "...";
                    else
                        descrizione = nuovo.Descrizione.Trim().ToLower();

                    command1.CommandText = "INSERT INTO prodotto(nome,descrizione,prezzo,quantita, fk_categoria) " + "VALUES('" + nuovo.Nome.Trim().ToLower() + "','" + descrizione + "','" + nuovo.Prezzo.ToString().Replace(",", ".") + "','" + disp + "','" + id_cat + "')";

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

        //Registrazione magazziniere
        /// <param name="nuovo">Oggetto di tipo Utente</param>
        /// <returns>True se l'utente è stato creato con successo. False in caso contrario</returns>
        public bool Registrazione(Utente nuovo)
        {

            // tipo di query: <nomeConnessioneVar> = new MySqlConnection(<connStringVar>
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "INSERT INTO account(password,nome,cognome,email,indirizzo,data_nascita,telefono,TipoAccount) " + "VALUES('" + nuovo.Psw + "','" + nuovo.Nome.Trim().ToLower() + "','" + nuovo.Cognome.Trim().ToLower() + "','" + nuovo.Email.Trim().ToLower() + "','" + nuovo.Indirizzo.Trim().ToLower() + "','" + nuovo.Data_nascita + "','" + nuovo.Telefono.Trim() + "', '2')";

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

        //Login utente magazziniere o ADMIN
        /// <param name="user">Oggetto di tipo Login</param>
        /// <returns>0 se le credenziali sono errate. 1 se l'accesso è stato effettuato dall'admin. 2 se l'accesso è stato effettuato dal magazziniere</returns>
        public int UserLogin(Login user)
        {
            try
            {
                int codice = 0;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    // check password with hash
                    command1.CommandText = "SELECT password, TipoAccount FROM account WHERE email='" + user.Email.Trim().ToLower() + "'";
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

        //Lista dei magazzinieri
        /// <returns>Lista dei nomi dei magazzinieri </returns>
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

        // Dati del magazziniere
        /// <param name="id">Identificativo del magazziniere</param>
        /// <returns>Oggetto Utente contenente i dati del magazziniere</returns>
        public Utente GetMagazziniere(string id)
        {
            try
            {
                //creo l'oggetto Utente da restituire
                Utente magazziniere = new Utente() { Email = id };

                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT * FROM account WHERE email='" + id.Trim().ToLower() + "'";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            magazziniere.Email = reader.GetString(4).TrimEnd().ToUpper();
                            magazziniere.Nome = reader.GetString(2).TrimEnd().ToUpper();
                            magazziniere.Cognome = reader.GetString(3).TrimEnd().ToUpper();
                            magazziniere.Indirizzo = reader.GetString(5).TrimEnd().ToUpper();
                            magazziniere.Data_nascita = reader.GetDateTime(6); // scoppia qua
                            magazziniere.Telefono = reader.GetString(7).TrimEnd().ToUpper();
                        }
                    }
                }
                //restituisco il magazziniere
                return magazziniere;
            }
            catch (Exception)
            {
                throw new Exception("Errore! Impossibile recuperare il magazziniere");
            }
        }

        /// Aumenta le giacenze di un prodotto tramite l'id
        public bool AumentaGiacenze(int id, int quantita)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "UPDATE prodotto SET quantita = quantita +' " + quantita + " ' WHERE IDProdotto =' " + id + " ' ";
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

        /// Dimuniisce le giacenze di un prodotto tramite l'id
        /// <returns>risultato true or false a seconda dell'esito</returns>
        public bool DiminuisciGiacenze(int id, int quantita)
        {
            int attuale = 0;
            try
            {
                bool risultato = false;

                // controllo della correttezza della quantita spostato nel program
                // in seguito a un'attenta riflessione avvenuta in doccia

                using (MySqlCommand command0 = conn.CreateCommand())
                {
                    //controlla numero giacenze
                    command0.CommandText = "SELECT quantita from prodotto where IDProdotto ='" + id + "'";
                    using (MySqlDataReader reader = command0.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            attuale = reader.GetInt32(0);
                        }

                    }

                    if (attuale < quantita)
                    {
                        Console.WriteLine("impossibile diminuire giacenze, numero troppo alto");
                        return risultato; //false perchè att < quantita e non è possibile
                    }
                    else
                    {
                        using (MySqlCommand command1 = conn.CreateCommand())
                        {
                            command1.CommandText = "UPDATE prodotto SET quantita = quantita -' " + quantita + " ' WHERE IDProdotto = ' " + id + " ' ";
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

        public bool CheckID(int id)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "Select IDProdotto from Prodotto Where IDProdotto = " + id;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    return reader.HasRows;
                }

            }
               
        }

    }
}
