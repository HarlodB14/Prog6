using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.Models.Validation;

public class DiscountCard : IValidation
{
    private readonly IValidation _strategy;

    public DiscountCard(TypeEnum cardType)
    {
        _strategy = cardType switch
        {
            TypeEnum.nothing => new NoCardStrategy(),
            TypeEnum.silver => new SilverCardStrategy(),
            TypeEnum.gold => new GoldCardStrategy(),
            TypeEnum.platinum => new PlatinumCardStrategy(),
            _ => throw new ArgumentException("Invalide KaartType")
        };
    }

    public bool CanBook(List<Animal> animal, Booking booking)
    {
        return _strategy.CanBook(animal, booking);
    }

    public int GetBookingLimit()
    {
        return _strategy.GetBookingLimit();
    }
}
