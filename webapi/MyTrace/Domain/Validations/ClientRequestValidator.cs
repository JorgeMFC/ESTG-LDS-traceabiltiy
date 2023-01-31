using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public static class ClientRequestValidator
    {
        public static void Validate(Client client)
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (client == null)
            {
                throw new ClientArgumentException($"{nameof(client)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(client.Name)){
                throw new ClientArgumentException($"{nameof(client.Name)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(client.Email))
            {
                throw new ClientArgumentException("Client email is Null or Empty");
            }
            else if (!(regex.Match(client.Email).Success))
            {
                throw new ClientArgumentException("Client email format is not valid!");
            }
        }
    }
}
