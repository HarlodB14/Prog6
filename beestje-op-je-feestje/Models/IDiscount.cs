using beestje_op_je_feestje.Models;

public interface IDiscountStrategy
{
    bool CanBook(List<Animal> animal, Booking booking);
    int GetBookingLimit();
}
