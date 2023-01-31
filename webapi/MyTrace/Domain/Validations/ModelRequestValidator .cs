using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyTrace.Domain.Exceptions;
using MyTrace.Models;
using System.Text.RegularExpressions;

namespace MyTrace.Domain.Validations
{
    public static class ModelRequestValidator
    {
        public static void Validate(Model model)
        {

            if (model == null)
            {
                throw new ModelArgumentException($"{nameof(model)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(model.Id))
            {
                throw new ModelArgumentException("ModelId is null or empty");
            }
            else if (string.IsNullOrEmpty(model.Name))
            {
                throw new ModelArgumentException($"{nameof(model.Name)} is Null or Empty");
            }
            else if (model.DeletedAt != null)
            {
                throw new ModelArgumentException("This model can't be created or updated because has been desactivated!");
            }
        }
    }
}
