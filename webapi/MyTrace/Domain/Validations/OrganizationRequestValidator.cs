using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public class OrganizationRequestValidator
    {
        public static void Validate(Organization organization)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (organization == null)
            {
                throw new OrganizationArgumentException($"{nameof(organization)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(organization.Name))
            {
                throw new OrganizationArgumentException($"{nameof(organization.Name)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(organization.Email))
            {
                throw new OrganizationArgumentException($"{nameof(organization.Email)} is Null or Empty");
            }
            else if (!(regex.Match(organization.Email).Success))
            {
                throw new OrganizationArgumentException("Organization email format is not valid!");
            }
            else if (string.IsNullOrWhiteSpace(organization.WalletAddress))
            {
                throw new OrganizationArgumentException($"{nameof(organization.WalletAddress)} is Null or Empty");
            }
        }
    }
}
