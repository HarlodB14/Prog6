namespace beestje_op_je_feestje.Models.Validation
{
    public interface IGeneralValidation
    {
        bool Validate(List<Animal> animals, Booking booking);
    }
}
