using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public static class ComponentRequestValidator
    {
        public static void Validate(Component component)
        {

            if (component == null)
            {
                throw new ComponentArgumentException($"{nameof(component)} is null");
            }
            else if (string.IsNullOrWhiteSpace(component.Id))
            {
                throw new ComponentArgumentException("ComponentId is null or empty");
            }
            else if (component.DeletedAt != null)
            {
                throw new ComponentArgumentException("This is component can't be created or updated because has been desactivated!");
            }
        }
    }
}
