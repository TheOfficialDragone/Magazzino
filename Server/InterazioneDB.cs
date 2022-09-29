using DbManager;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class InterazioneDB
    {
        #region Attributi
        private static readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];
        private static MySqlConnection connessione = null;
        #endregion

        #region Getters & Setters
        public static string ConnectionString => connectionString;
        public static MySqlConnection Connessione { get => connessione; set => connessione = value; }
        #endregion
        /// <summary>
        /// Controlla la presenza dell'email nel sistema
        /// </summary>
        /// <param name="email">Email da controllare</param>
        /// <returns>True se l'email non è presente nel sistema. False in caso contrario</returns>
        public bool CheckEmail(string email)
        {
            try
            {
                bool risultato = true;
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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
        /// Lista delle categorie
        /// </summary>
        /// <returns>Lista degli identificativi delle Categorie</returns>
        public List<int> ListaCategorie()
        {
            try
            {
                List<int> lista = new List<int>();

                using (SqlConnection conn = new SqlConnection(connectionString))
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
        /// Lista dei prodotti disponibili
        /// </summary>
        /// <returns>Lista degli identificativi dei prodotti disponibili</returns>
        public List<int> ListaProdottiDisponibili()
        {
            try
            {
                List<int> lista = new List<int>();

                using (SqlConnection conn = new SqlConnection(ConnectionString))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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
                using (SqlConnection conn = new SqlConnection(connectionString))
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






    }
}



        

   



