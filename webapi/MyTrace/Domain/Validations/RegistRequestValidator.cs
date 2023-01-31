namespace MyTrace.Domain.Validations
{
    public class RegistRequestValidator
    {
        public static void Validate(RegistRequest registRequest)
        {
            if (registRequest == null)
            {
                throw new AuthenticationArgumentException($"{nameof(registRequest)} is null!");
            }
            if (string.IsNullOrWhiteSpace(registRequest.Email))
            {
                throw new AuthenticationArgumentException("Email is Null or Empty");
            }
            if (string.IsNullOrWhiteSpace(registRequest.Name))
            {
                throw new AuthenticationArgumentException("Name is Null or Empty");
            }
            if (!Validations.IsValidEmail(registRequest.Email))
            {
                throw new AuthenticationArgumentException("Email format is not valid!");
            }
            if (!Validations.IsValidName(registRequest.Name))
            {
                throw new AuthenticationArgumentException("Name format is not valid!");
            }
        }
    }
}
