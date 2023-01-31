using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public static class ProviderRequestValidator
    {
        public static void Validate(Provider provider)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (provider == null)
            {
                throw new ProviderArgumentException($"{nameof(provider)} is null!");
            } 
            else if (string.IsNullOrEmpty(provider.Id))
            {
                throw new ProviderArgumentException($"ProviderId is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(provider.Name)){
                throw new ProviderArgumentException($"{nameof(provider.Name)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(provider.Email))
            {
                throw new ProviderArgumentException("Provider email is Null or Empty");
            }
            else if (provider.DeletedAt != null) 
            {
                throw new ProviderArgumentException("This provider can't be created or updated because has been desactivated!");
            }
            else if (!(regex.Match(provider.Email).Success))
            {
                throw new ProviderArgumentException("Provider email format is not valid!");
            }
            else if (DateTime.Compare(provider.CreatedAt, DateTime.Now) > 0)
            {
                throw new ProviderArgumentException("Provider account createdDate cannot be greater than current date");
            }
        }
    }
}
