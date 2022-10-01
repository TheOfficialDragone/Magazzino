using DbManager;
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
        private static readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        private static MySqlConnection connessione = null;
        #endregion

        #region Getters & Setters
        public static string ConnectionString => connectionString;
        public static MySqlConnection Connessione { get => connessione; set => connessione = value; }
        #endregion

        public bool CheckEmail(string email)
        {
            try
            {
                bool risultato = true;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT * FROM cliente WHERE cliente.email='" + email.Trim().ToLower() + "'";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.HasRows)
                                risultato = false;
                        }
                    }
                    conn.Close();
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Elimina un prodotto (lo rende indisponibile)
        /// </summary>
        /// <param name="id">Identificativo del prodotto da eliminare</param>
        /// <returns>True se il prodotto viene eliminato con successo. False in caso contrario</returns>
        public bool EliminaProdotto(int id)
        {
            try
            {
                bool risultato = false;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "UPDATE prodotto SET disponibilita=0 WHERE IDprodotto=" + id;
                        if (command1.ExecuteNonQuery() > 0)
                            risultato = true;
                    }
                    conn.Close();
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
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT nome_categoria FROM categoria WHERE IDcategoria=" + id;
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nome = reader.GetString(0).TrimEnd().ToUpper();
                            }
                        }
                    }
                    conn.Close();
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

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT prodotto.*, categoria.nome_categoria FROM prodotto, categoria " +
                                               "WHERE prodotto.IDcategoria=categoria.IDcategoria AND IDprodotto=" + IDProdotto;
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                prodotto.Nome = reader.GetString(1).TrimEnd().ToUpper();
                                prodotto.Descrizione = reader.GetString(2).TrimEnd().ToUpper();

                                prodotto.Disponibilita = reader.GetBoolean(3);
                                prodotto.Prezzo = reader.GetDouble(4);
                                prodotto.Categoria = reader.GetString(6).TrimEnd().ToUpper();
                            }
                        }
                    }
                    conn.Close();
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

        /// <summary>
        /// Lista delle categorie
        /// </summary>
        /// <returns>Lista degli identificativi delle Categorie</returns>
        public List<int> ListaCategorie()
        {
            try
            {
                List<int> lista = new List<int>();

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT IDcategoria FROM categoria";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(reader.GetInt32(0));
                            }
                        }
                    }
                    conn.Close();
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
        /// <returns>Lsta degli identificativi dei prodotti</returns>
        public List<int> ListaProdotti()
        {
            try
            {
                List<int> lista = new List<int>();
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT IDprodotto FROM prodotto";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(reader.GetInt32(0));
                            }
                        }
                    }
                    conn.Close();
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
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        int idLogin = 0;
                        command1.CommandText = "SELECT IDlogin FROM cliente WHERE email='" + email.Trim().ToLower() + "'";
                        using (SqlDataReader reader = command1.ExecuteReader())
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
                    conn.Close();
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
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        int id_cat = 0;
                        command1.CommandText = "SELECT IDcategoria FROM categoria WHERE nome_categoria='" + prodottoDaModificare.Categoria.Trim().ToLower() + "'";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id_cat = reader.GetInt32(0);
                            }
                        }
                        int disp = Convert.ToInt32(prodottoDaModificare.Disponibilita);
                        string descrizione;
                        if (prodottoDaModificare.Descrizione == null)
                            descrizione = "...";
                        else
                            descrizione = prodottoDaModificare.Descrizione.Trim().ToLower();

                        command1.CommandText = "UPDATE prodotto SET " +
                                                "nome='" + prodottoDaModificare.Nome.Trim().ToLower() + "', " +
                                                "descrizione='" + descrizione + "', " +
                                                "disponibilita=" + disp +
                                                ", prezzo=" + prodottoDaModificare.Prezzo.ToString().Replace(",", ".") +
                                                ", IDcategoria=" + id_cat +
                                               " WHERE IDprodotto=" + prodottoDaModificare.IDprodotto;
                        if (command1.ExecuteNonQuery() > 0)
                            risultato = true;
                    }
                    conn.Close();
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
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "INSERT INTO categoria(nome_categoria) VALUES('" + nome.Trim().ToLower() + "')";
                        if (command1.ExecuteNonQuery() > 0)
                            risultato = true;
                    }
                    conn.Close();
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
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        int id_cat = 0;
                        command1.CommandText = "SELECT IDcategoria FROM categoria WHERE nome_categoria='" + nuovo.Categoria.Trim().ToLower() + "'";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id_cat = reader.GetInt32(0);
                            }
                        }
                        int disp = Convert.ToInt32(nuovo.Disponibilita);
                        string descrizione;
                        if (nuovo.Descrizione == null)
                            descrizione = "...";
                        else
                            descrizione = nuovo.Descrizione.Trim().ToLower();

                        command1.CommandText = "INSERT INTO prodotto(nome,descrizione,disponibilita,prezzo,IDcategoria) " +
                                               "VALUES( " + "'" + nuovo.Nome.Trim().ToLower() + "','" +
                                                descrizione + "'," + disp + "," +
                                                nuovo.Prezzo.ToString().Replace(",", ".") + "," + id_cat + ")";
                        if (command1.ExecuteNonQuery() > 0)
                            risultato = true;
                    }
                    conn.Close();
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Registrazione cliente
        /// </summary>
        /// <param name="nuovo">Oggetto di tipo Utente</param>
        /// <returns>True se l'utente è stato creato con successo. False in caso contrario</returns>
        public bool Signin(Utente nuovo)
        {
            try
            {
                bool risultato = false;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "INSERT INTO account(password) VALUES('" + nuovo.Psw + "')";
                        if (command1.ExecuteNonQuery() > 0)
                        {
                            command1.CommandText = "SELECT MAX(account.IDlogin) FROM account";
                            int id = 0;
                            using (SqlDataReader reader = command1.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    id = reader.GetInt32(0);
                                }
                            }
                            command1.CommandText = "INSERT INTO cliente(email,nome,cognome,indirizzo,data_nascita,telefono,IDcitta,IDlogin) " +
                                                   "VALUES('" + nuovo.Email.Trim().ToLower() +
                                                    "','" + nuovo.Nome.Trim().ToLower() + "','" + nuovo.Cognome.Trim().ToLower() +
                                                    "','" + nuovo.Indirizzo.Trim().ToLower() + "','" + nuovo.Data_nascita +
                                                    "','" + nuovo.Telefono.Trim() + "','" + nuovo.Cap + "'," + id + ")";
                            if (command1.ExecuteNonQuery() > 0)
                                risultato = true;
                        }
                    }
                    conn.Close();
                }
                return risultato;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Login utente
        /// </summary>
        /// <param name="user">Oggetto di tipo Login</param>
        /// <returns>0 se le credenziali sono errate. 1 se l'accesso è stato effettuato dall'admin. 2 se l'accesso è stato effettuato dal cliente</returns>
        public int UserLogin(Login user)
        {
            try
            {
                int codice = 0;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {

                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT * FROM account JOIN amministratore ON account.IDlogin=amministratore.IDlogin " +
                                               "WHERE amministratore.email='" + user.Email.Trim().ToLower() + "' AND account.password='" + user.Password + "'";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.HasRows)
                                codice = 1;
                        }
                        command1.CommandText = "SELECT * FROM account JOIN cliente ON account.IDlogin=cliente.IDlogin " +
                                               "WHERE cliente.email='" + user.Email.Trim().ToLower() + "' AND account.password='" + user.Password + "'";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            if (reader.HasRows)
                                codice = 2;
                        }
                    }
                    conn.Close();
                }
                return codice;
            }
            catch (Exception)
            {
                throw new Exception();
            }



        }

        /// <summary>
        /// Lista dei magazzinieri
        /// </summary>
        /// <returns>Lista degli identificativi dei clienti</returns>
        public List<string> ListaClienti()
        {
            try
            {
                List<string> listaClienti = new List<string>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT email FROM cliente";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listaClienti.Add(reader.GetString(0).TrimEnd().ToUpper());
                            }
                        }
                    }
                    conn.Close();
                }
                //restituisco la lista degli identificativi dei clienti
                return listaClienti;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Dati del cliente
        /// </summary>
        /// <param name="id">Identificativo del magazziniere</param>
        /// <returns>Oggetto Utente contenente i dati del cliente</returns>
        public Utente GetMagazziniere(string id)
        {
            try
            {
                //creo l'oggetto Utente da restituire
                Utente cliente = new Utente() { Email = id };

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT * FROM cliente WHERE email='" + id.Trim().ToLower() + "'";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cliente.Email = reader.GetString(0).TrimEnd().ToUpper();
                                cliente.Nome = reader.GetString(1).TrimEnd().ToUpper();
                                cliente.Cognome = reader.GetString(2).TrimEnd().ToUpper();
                                cliente.Indirizzo = reader.GetString(3).TrimEnd().ToUpper();
                                cliente.Data_nascita = reader.GetDateTime(4);
                                cliente.Telefono = reader.GetString(5).TrimEnd().ToUpper();
                                cliente.Cap = reader.GetString(6).TrimEnd().ToUpper();
                            }
                        }
                    }
                    conn.Close();
                }
                //restituisco il magazziniere
                return cliente;
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

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "SELECT IDprodotto FROM prodotto WHERE disponibilita=1 ORDER BY IDcategoria,nome";
                        using (SqlDataReader reader = command1.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lista.Add(reader.GetInt32(0));
                            }
                        }
                    }
                    conn.Close();
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
        public bool AumentaGiacenze(int id, int quantita)
        {
            try
            {
                bool risultato = false;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand command1 = conn.CreateCommand())
                    {
                        command1.CommandText = "UPDATE prodotto SET disponibilita = disponibilita +' "
                            + quantita
                            + " ' WHERE idprodotto = "
                            + id
                            + " ' ";
                        using (SqlDataReader reader = command1.ExecuteReader());
                    }
                    conn.Close();
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
            int attuale = 0;
            try
            {
                bool risultato = false;

                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (SqlCommand command0 = conn.CreateCommand())
                    {
                        //controlla numero giacenze
                        command0.CommandText = "SELECT disponibilita from prodotto where idprodotto ='" + id + "'";
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

                            using (SqlCommand command1 = conn.CreateCommand())
                            {
                                command1.CommandText = "UPDATE prodotto SET disponibilita = disponibilita -' "
                                    + quantita
                                    + " ' WHERE idprodotto = "
                                    + id
                                    + " ' ";
                                using (SqlDataReader reader = command1.ExecuteReader()) ;
                            }
                            conn.Close();
                            risultato = true;
                        }
                        return risultato;
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

    }

    

    
}
