
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


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
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Elimina un prodotto 
        /// </summary>
        /// <param name="id">Identificativo del prodotto da eliminare</param>
        /// <returns>True se il prodotto viene eliminato con successo. False in caso contrario</returns>
        public bool EliminaProdotto(int id)
        {
            try
            {
                bool risultato = false;
                
                    using (MySqlCommand command1 = conn.CreateCommand())
                    {   
                        //cancellare il prodotto dal db mantenedo la categoria
                        command1.CommandText = "UPDATE prodotto SET disponibilita=0 WHERE IDprodotto=" + id;
                        if (command1.ExecuteNonQuery() > 0)
                            risultato = true;
                    }
                   
                
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Nome di una categoria
        /// </summary>
        /// <param name="id">Identificativo della categoria</param>
        /// <returns>Il nome della categoria</returns>
        public string GetCategoria(int id)
        {
            try
            {
                string nome = null;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT nome FROM categoria WHERE IDcategoria=" + id;
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nome = reader.GetString(0).TrimEnd().ToUpper();
                        }
                    }
                }
                return nome;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        /// <summary>
        /// Dati del prodotto
        /// </summary>
        /// <param name="IDProdotto">Identificativo del prodotto</param>
        /// <returns>Oggetto Articolo</returns>
        public Articolo GetProdotto(int IDProdotto)
        {
            try
            {
                //creo l'oggetto da restituire
                Articolo prodotto = new Articolo() { IDprodotto = IDProdotto };

                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT prodotto.*, categoria.nome FROM prodotto, categoria " + "WHERE prodotto.IDcategoria=categoria.IDcategoria AND IDprodotto=" + IDProdotto;
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // controllare ordine campi in tabella db
                            prodotto.Nome = reader.GetString(1).TrimEnd().ToUpper();
                            prodotto.Descrizione = reader.GetString(2).TrimEnd().ToUpper();
                            prodotto.Quantita = reader.GetInt32(3);
                            prodotto.Prezzo = reader.GetDouble(4);
                            prodotto.Categoria = reader.GetString(6).TrimEnd().ToUpper();
                        }
                    }
                }
                //restituisco il prodotto
                return prodotto;
            }
            catch (Exception)
            {
                throw new Exception();
            }

           
        }

        /// <summary>
        /// Stato dell'ordine
        /// </summary>
        /// <param name="id">Identificativo dello stato dell'ordine</param>
        /// <returns>Stringa contenente lo stato</returns>
       /* 
          public string GetStatoOrdine(int id)
        {
            try
            {
                string stato = null;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT stato_ordine FROM stato_ordine WHERE IDstato=" + id;
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                stato = reader.GetString(0).TrimEnd().ToUpper();
                            }
                        }
                    }
                    conn.Close();
                }
                return stato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
       */
        
        /// <summary>
        /// Lista delle categorie
        /// </summary>
        /// <returns>Lista degli identificativi delle Categorie</returns>
        public List<int> ListaCategorie()
        {
            try
            {
                List<int> lista = new List<int>();

                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT IDcategoria FROM categoria";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(reader.GetInt32(0));
                        }
                    }
                }
                //restituisco la lista degli identificativi delle categorie
                return lista;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        /// <summary>
        /// Lista dei prodotti
        /// </summary>
        /// <returns>Lista dei dei prodotti con dispo > 0</returns>
        public List<int> ListaProdotti()
        {
            try
            {   
                //lista stringhe no interi 
                List<int> lista = new List<int>();
                using (MySqlCommand command1 = conn.CreateCommand())
                {   //select nome from prodotto where disponibilita > 0

                    command1.CommandText = "SELECT IDprodotto FROM prodotto";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        { //stile stringa
                            lista.Add(reader.GetInt32(0));
                        }
                    }
                }
                //restituisco la lista degli identificativi dei prodotti
                return lista;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Modifica la password
        /// </summary>
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
                    command1.CommandText = "SELECT IDlogin FROM account WHERE email='" + email.Trim().ToLower() + "'";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idLogin = reader.GetInt32(0);
                        }
                    }

                    command1.CommandText = "UPDATE account SET " +
                                            "password='" + psw + "' " +
                                           "WHERE IDlogin=" + idLogin;
                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Modifica il prodotto
        /// </summary>
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

                    command1.CommandText = "UPDATE prodotto SET " + "nome='" + prodottoDaModificare.Nome.Trim().ToLower() + "', " + "descrizione='" + descrizione + "', " + "disponibilita=" + disp + ", prezzo=" + prodottoDaModificare.Prezzo.ToString().Replace(",", ".") + ", IDcategoria=" + id_cat + " WHERE IDprodotto=" + prodottoDaModificare.IDprodotto;

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Crea una nuova categoria
        /// </summary>
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
                throw new Exception();
            }
        }
        /// <summary>
        /// Crea un nuovo prodotto
        /// </summary>
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

                    command1.CommandText = "INSERT INTO prodotto(nome,descrizione,prezzo, quantita, fk_categoria) " + "VALUES( " + "'" + nuovo.Nome.Trim().ToLower() + "','" + descrizione  + "," + nuovo.Prezzo.ToString().Replace(",", ".") + "'," + disp + "," + id_cat + ")";

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Registrazione magazziniere
        /// </summary>
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
                
                    /*
                    command1.CommandText = "SELECT MAX(account.IDutente) FROM account";
                    int id = 0;
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            id = reader.GetInt32(0);
                        }
                    }
                    */
                    command1.CommandText = "INSERT INTO account(password,nome,cognome,email,indirizzo,data_nascita,telefono,TipoAccount) " + "VALUES('"+nuovo.Psw + "','"+nuovo.Nome.Trim().ToLower() + "','" + nuovo.Cognome.Trim().ToLower() + "','" + nuovo.Email.Trim().ToLower() + "','"  + nuovo.Indirizzo.Trim().ToLower() + "','" + nuovo.Data_nascita + "','" + nuovo.Telefono.Trim() + "', '2')";

                    if (command1.ExecuteNonQuery() > 0)
                        risultato = true;
                    
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Login utente magazziniere o ADMIN
        /// </summary>
        /// <param name="user">Oggetto di tipo Login</param>
        /// <returns>0 se le credenziali sono errate. 1 se l'accesso è stato effettuato dall'admin. 2 se l'accesso è stato effettuato dal magazziniere</returns>
        public int UserLogin(Login user)
        {
            try
            {
                int codice = 0;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "SELECT * FROM account WHERE email='" + user.Email.Trim().ToLower() + "' AND password='" + user.Password + "'";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        // controllare che il campo isAdmin sia vero 
                        if (reader.HasRows)
                            // allora esiste un utente con quelle credenziali
                            // ora controlla se è admin o no
                            // se è admin allora codice = 1
                            // se non è admin allora codice = 2

                          if (reader.Read())
                          {
                                if (reader.GetInt32(7) == 1)
                                    codice = 1;
                                // l'utente loggato è un admin
                                // altrimenti un magazziniere
                                else
                                    codice = 2;  //metto 2 perchè ho gia controllato esistenza utente
                          }
                    }
                    // se invece non vengono restituite righe vuol dire che le credenziali sono errate
                    return codice;
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Lista dei magazzinieri
        /// </summary>
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
                throw new Exception();
            }
        }

        /// <summary>
        /// Dati del magazziniere
        /// </summary>
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
                            magazziniere.Email = reader.GetString(3).TrimEnd().ToUpper();
                            magazziniere.Nome = reader.GetString(1).TrimEnd().ToUpper();
                            magazziniere.Cognome = reader.GetString(2).TrimEnd().ToUpper();
                            magazziniere.Indirizzo = reader.GetString(4).TrimEnd().ToUpper();
                            magazziniere.Data_nascita = reader.GetDateTime(5);
                            magazziniere.Telefono = reader.GetString(6).TrimEnd().ToUpper();
                        }
                    }
                }
                //restituisco il magazziniere
                return magazziniere;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Lista dei prodotti disponibili
        /// </summary>
        /// <returns>Lista degli identificativi dei prodotti disponibili</returns>
        public List<int> ListaProdottiDisponibili()
        {
            try
            {
                List<int> lista = new List<int>();

                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    // verso TODO: aggiornare query per la formula corretta di disponibilità
                    command1.CommandText = "SELECT IDprodotto FROM prodotto WHERE quantita > 0 ORDER BY IDcategoria,nome";
                    using (MySqlDataReader reader = command1.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(reader.GetInt32(0));
                            // aggiungere altri campi?
                        }
                    }
                }
                //restituisco la lista degli identificativi dei prodotto disponibili
                return lista;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Aumenta le giacenze di un prodotto tramite l'id
        /// </summary>
        /// <returns>risultato true or false a seconda dell'esito</returns>
        /// 

        
        public bool AumentaGiacenze(int id, int quantita)
        {
            try
            {
                bool risultato = false;
                using (MySqlCommand command1 = conn.CreateCommand())
                {
                    command1.CommandText = "UPDATE prodotto SET disponibilita = disponibilita +' " + quantita + " ' WHERE IDProdotto = " + id + " ' ";
                    using (MySqlDataReader reader = command1.ExecuteReader()); //necessario? da warning
                    risultato = true;
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Dimuniisce le giacenze di un prodotto tramite l'id
        /// </summary>
        /// <returns>risultato true or false a seconda dell'esito</returns>
        public bool DiminuisciGiacenze(int id, int quantita)
        {
            // int attuale = 0;
            try
            {
                bool risultato = false;

                

                    // controllo della correttezza della quantita spostato nel program
                    // in seguito a un'attenta riflessione avvenuta in doccia

                    /*
                    using (SqlCommand command0 = conn.CreateCommand())
                    {
                        //controlla numero giacenze
                        command0.CommandText = "SELECT disponibilita from prodotto where IDProdotto ='" + id + "'";
                        using (SqlDataReader reader = command0.ExecuteReader())
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
                        */

                    using (MySqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "UPDATE prodotto SET disponibilita = disponibilita -' " + quantita + " ' WHERE IDProdotto = " + id + " ' ";
                        using (MySqlDataReader reader = command1.ExecuteReader()) ; // necessario? da warning

                    }
                    
                    risultato = true;
                
                return risultato;

            }catch (Exception)
            {
                throw new Exception();
            }
           
        }
            
    }

    
}
