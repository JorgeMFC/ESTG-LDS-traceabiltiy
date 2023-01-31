using MyTrace.Domain.Exceptions;
using MyTrace.Models;

namespace MyTrace.Domain.Validations
{
    public class StagesModelRequestValidator
    {
        public static void Validate(StagesModel stagesModel)
        {

            if (stagesModel == null)
            {
                throw new StagesModelArgumentException($"{nameof(stagesModel)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(stagesModel.Id))
            {
                throw new StagesModelArgumentException($"StagesModelId is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(stagesModel.StagesModelName))
            {
                throw new StagesModelArgumentException($"{nameof(stagesModel.StagesModelName)} is Null or Empty");
            }
        }
    }
}
