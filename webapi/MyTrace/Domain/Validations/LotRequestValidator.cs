namespace MyTrace.Domain.Validations
{
    public class LotRequestValidator
    {
        public static void Validate(Lot lot)
        {

            if (lot == null)
            {
                throw new LotArgumentException($"{nameof(lot)} is null!");
            }
            else if (string.IsNullOrWhiteSpace(lot.Id))
            {
                throw new LotArgumentException($"{nameof(lot.Id)} is null or empty");
            }
            else if (lot.CanceledAt != null)
            {
                throw new LotArgumentException("This lot can't be created or updated because has been canceled!");
            }
        }
    }
}
