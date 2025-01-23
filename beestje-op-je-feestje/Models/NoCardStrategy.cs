
using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.Models.Validation;

internal class NoCardStrategy : IValidation
{
    public NoCardStrategy()
    {
    }

    public bool CanBook(List<Animal> animal, Booking booking)
    {
        return booking.AmountOfAnimals < 3;
    }

    public int GetBookingLimit() => 3;
}