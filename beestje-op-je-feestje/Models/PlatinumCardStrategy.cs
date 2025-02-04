using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.Models.Validation;
//TODO VIP DIEREN kunnnen kiezen
internal class PlatinumCardStrategy : IValidation
{
    public bool CanBook(List<Animal> animal, Booking booking)
    {
        throw new NotImplementedException();
    }

    public int GetBookingLimit() => 1000000000;
}