using MyTrace.Models;
using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public class Validations
    {
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidName(string name)
        {
            Regex regex = new Regex(@"^(([A-Z\u00C0-\u017F][a-z\u00C0-\u017F]+)+\s?)+$");
            return regex.Match(name).Success;
        }
    }
}
