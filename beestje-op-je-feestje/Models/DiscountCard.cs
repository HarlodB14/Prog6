using beestje_op_je_feestje.Models;

public class DiscountCard : IDiscountStrategy
{
    private readonly IDiscountStrategy _strategy;

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
