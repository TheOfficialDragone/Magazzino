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
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                             + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                             + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex emailRegex = new Regex(pattern, RegexOptions.IgnoreCase);

            return emailRegex.IsMatch(email);
        }

        public static bool ValidatePassword(string password)
        {
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$";

            Regex passwordRegex = new Regex(pattern);

            return passwordRegex.IsMatch(password);
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

