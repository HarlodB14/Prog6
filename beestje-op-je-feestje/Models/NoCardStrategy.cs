
using beestje_op_je_feestje.Models;

internal class NoCardStrategy : IDiscountStrategy
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