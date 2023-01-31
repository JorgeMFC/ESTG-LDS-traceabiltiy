using MyTrace.Domain.Exceptions;
using MyTrace.Models;

namespace MyTrace.Domain.Validations
{
    public class StagesModelStageRequestValidator
    {
        public static void Validate(StagesModelStage stagesModelStage)
        {

            if (stagesModelStage == null)
            {
                throw new StagesModelStageArgumentException($"{nameof(stagesModelStage)} is null!");
            }
        }
    }
}
