using MyTrace.Domain.Exceptions;
using MyTrace.Models;

namespace MyTrace.Domain.Validations
{
    public class ColorRequestValidator
    {
        public static void Validate(Color color)
        {

            if (color == null)
            {
                throw new ModelArgumentException($"{nameof(color)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(color.Id))
            {
                throw new ModelArgumentException("ColorId is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(color.Color1))
            {
                throw new ModelArgumentException("Color is Null or Empty");
            }
        }
    }
}
