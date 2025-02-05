using beestje_op_je_feestje.Models.Validation;
using beestje_op_je_feestje.Models;

[TestFixture]
public class FlyingValidationTests
{
    private FlyingValidation _flyingValidation;

    [SetUp]
    public void Setup()
    {
        _flyingValidation = new FlyingValidation();
    }

    [Test]
    public void Validate_LessThanMaxFlyingAnimals_ReturnsTrue()
    {
        var animals = new List<Animal> {
            new Animal { Type = TypeEnum.flying },
            new Animal { Type = TypeEnum.flying }
        };
        var booking = new Booking();

        bool result = _flyingValidation.Validate(animals, booking);

        Assert.IsTrue(result);
    }

    [Test]
    public void Validate_ExceedsMaxFlyingAnimals_ReturnsFalse()
    {
        var animals = new List<Animal>
        {
            new Animal { Type = TypeEnum.flying },
            new Animal { Type = TypeEnum.flying },
            new Animal { Type = TypeEnum.flying }
        };
        var booking = new Booking();

        bool result = _flyingValidation.Validate(animals, booking);

        Assert.IsFalse(result);
    }
}
