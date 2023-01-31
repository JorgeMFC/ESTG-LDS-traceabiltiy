using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public static class UserRequestValidator
    {
        public static void Validate(User user)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (user == null)
            {
                throw new UserArgumentException($"{nameof(user)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(user.WalletAddress))
            {
                throw new UserArgumentException($"{nameof(user.WalletAddress)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(user.Name))
            {
                throw new UserArgumentException($"{nameof(user.Name)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(user.Email))
            {
                throw new UserArgumentException("User email is Null or Empty");
            }
            else if (!(regex.Match(user.Email).Success))
            {
                throw new UserArgumentException("User email format is not valid!");
            }
            else if (DateTime.Compare(user.CreatedAt, DateTime.Now) > 0)
            {
                throw new UserArgumentException("User account createdDate cannot be greater than current date");
            }
            else if (user.DeletedAt != null)
            {
                throw new UserArgumentException("This is user can't be created or updated because has been desactivated!");
            }
        }
    }
}
