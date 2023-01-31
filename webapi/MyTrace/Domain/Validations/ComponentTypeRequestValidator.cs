using MyTrace.Domain.Exceptions;
using MyTrace.Models;

namespace MyTrace.Domain.Validations
{
    public class ComponentTypeRequestValidator
    {
        public static void Validate(ComponentsType componentsType)
        {

            if (componentsType == null)
            {
                throw new ComponentsTypeArgumentException($"{nameof(componentsType)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(componentsType.ComponentType))
            {
                throw new ComponentsTypeArgumentException($"{nameof(componentsType.ComponentType)} is Null or Empty");
            }
        }
    }
}
