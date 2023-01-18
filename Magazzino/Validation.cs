using System.Text.RegularExpressions;
/**
 * Developed by Rocco Carpi & Riccardo Versetti
 * 16/10/2022
 * Controlla la validità o meno della mail e della password quando c'è un login 
 */
namespace Client
{
    public class Validation
    {
        public static bool ValidateEmail(string email)
        {
            //validazione email
            string pattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";

            return Regex.IsMatch(email, pattern);
        }

        public static bool ValidatePassword(string psw)
        {
            int validConditions = 0;
            //la password non può essere minore di 6 caratteri
            if (psw.Length < 6)
                return false;
            else
            {
                validConditions++;
                //deve contenere almeno una lettera minuscola
                foreach (char c in psw)
                {
                    if (c >= 'a' && c <= 'z')
                    {
                        validConditions++;
                        break;
                    }
                }
                //deve contenere una lettera maiuscola
                foreach (char c in psw)
                {
                    if (c >= 'A' && c <= 'Z')
                    {
                        validConditions++;
                        break;
                    }
                }
                //deve contenere almeno un numero
                foreach (char c in psw)
                {

                    if (c >= '0' && c <= '9')
                    {
                        validConditions++;
                        break;
                    }
                }
                //deve contenere un carattere speciale
                if (validConditions == 4)
                {
                    char[] special = { '@', '#', '$', '%', '^', '&', '+', '=', '.' };
                    if (psw.IndexOfAny(special) != -1)
                        return true;
                }
            }
            return false;
        }

        public static bool ValidateTelephone(string numero)
        {
            string pattern;
            if (numero.Length == 10)
            {
                pattern = @"^([0-9]{10})$";
                return Regex.Match(numero, pattern).Success;
            }
            else if (numero.Length == 13)
            {
                pattern = @"^(\+[0-9]{12})$";
                return Regex.Match(numero, pattern).Success;
            }

            return false;
        }
    }
}

