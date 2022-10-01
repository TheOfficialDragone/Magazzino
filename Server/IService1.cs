using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Server
{
    // NOTA: è possibile utilizzare il comando "Rinomina" del menu "Refactoring" per modificare il nome di interfaccia "IService1" nel codice e nel file di configurazione contemporaneamente.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        bool CheckEmail(string email);

        [OperationContract]
        bool EliminaProdotto(int id);

        [OperationContract]
        string GetCategoria(int id);

        [OperationContract]
        Articolo GetProdotto(int IDProdotto);

        [OperationContract]
        List<int> ListaCategorie();

        [OperationContract]
        List<int> ListaProdotti();

        [OperationContract]
        bool ModificaPassword(string email, string psw);

        [OperationContract]
        bool ModificaProdotto(Articolo prodottoDaModificare);

        [OperationContract]
        bool NuovaCategoria(string nome);

        [OperationContract]
        bool NuovoProdotto(Articolo nuovo);

        [OperationContract]
        bool Signin(Utente nuovo);

        [OperationContract]
        int UserLogin(Login user);

        [OperationContract]
        List<string> ListaClienti();

        [OperationContract]
        Utente GetMagazziniere(string id);

        [OperationContract]
        List<int> ListaProdottiDisponibili();

        [OperationContract]
        bool AumentaGiacenze(int id,int quantita);

        [OperationContract]
        bool DiminuisciGiacenze(int id, int quantita);


    }
}
