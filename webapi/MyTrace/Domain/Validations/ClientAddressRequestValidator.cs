using MyTrace.Models;
using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public static class ClientAddressRequestValidator
    {
        public static void Validate(ClientsAddress clientAddress)
        {
            Regex regex = new Regex(@"^\d{4}(-\d{3})?$");

            if (clientAddress == null)
            {
                throw new ClientArgumentException($"{nameof(clientAddress)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(clientAddress.Address)){
                throw new ClientArgumentException($"{nameof(clientAddress.Address)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(clientAddress.Zipcode))
            {
                throw new ClientArgumentException($"{nameof(clientAddress.Zipcode)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(clientAddress.Id))
            {
                throw new ClientArgumentException($"ClientAddressId is Null or Empty");
            }
            else if (!(regex.Match(clientAddress.Zipcode).Success))
            {
                throw new ClientArgumentException("ClientAddress zipcode format is not valid!");
            }
        }
    }
}
