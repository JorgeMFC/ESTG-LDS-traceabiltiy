using MyTrace.Domain.Exceptions;
using MyTrace.Models;

namespace MyTrace.Domain.Validations
{
    public class SizeRequestValidator
    {
        public static void Validate(Size size)
        {

            if (size == null)
            {
                throw new ModelArgumentException($"{nameof(size)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(size.Id))
            {
                throw new ModelArgumentException("SizeId is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(size.Size1))
            {
                throw new ModelArgumentException("Size is Null or Empty");
            }
        }
    }
}
