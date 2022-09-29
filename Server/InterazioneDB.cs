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


    }
}
