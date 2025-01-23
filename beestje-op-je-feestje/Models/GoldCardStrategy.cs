using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.Models.Validation;

internal class GoldCardStrategy : IValidation
{
    public bool CanBook(List<Animal> animal, Booking booking)
    {
        throw new NotImplementedException();
    }
    public int GetBookingLimit() => 1000000000;

}