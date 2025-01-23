namespace beestje_op_je_feestje.Models.Validation
{
    public interface IValidation
    {
        bool CanBook(List<Animal> animal, Booking booking);
        int GetBookingLimit();
    }
}