using beestje_op_je_feestje.Models.Validation;
using beestje_op_je_feestje.Models;

[TestFixture]
public class RestrictedAnimalValidationTests
{
    private RestrictedAnimalValidation _restrictedValidation;

    [SetUp]
    public void Setup()
    {
        _restrictedValidation = new RestrictedAnimalValidation();
    }

    [Test]
    public void Validate_SnowAnimalAndFarmAnimal_ReturnsFalse()
    {
        var animals = new List<Animal>
        {
            new Animal { Type = TypeEnum.snow },
            new Animal { Type = TypeEnum.farm }
        };
        var booking = new Booking();

        bool result = _restrictedValidation.Validate(animals, booking);

        Assert.IsFalse(result);
    }

    [Test]
    public void Validate_OnlyFarmAnimals_ReturnsTrue()
    {
        var animals = new List<Animal> { new Animal { Type = TypeEnum.farm } };
        var booking = new Booking();

        bool result = _restrictedValidation.Validate(animals, booking);

        Assert.IsTrue(result);
    }
}
