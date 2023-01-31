using MyTrace.Domain.Exceptions;
using MyTrace.Models;

namespace MyTrace.Domain.Validations
{
    public class StageRequestValidator
    {
        public static void Validate(Stage stage)
        {
            if (stage == null)
            {
                throw new StageArgumentException($"{nameof(stage)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(stage.Id))
            {
                throw new StageArgumentException($"{nameof(stage.Id)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(stage.StageName))
            {
                throw new StageArgumentException($"{nameof(stage.StageName)} is Null or Empty");
            }
            else if (string.IsNullOrWhiteSpace(stage.StageDescription))
            {
                throw new StageArgumentException($"{nameof(stage.StageDescription)} is Null or Empty");
            }
        }
    }
}
