using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.Models.Validation;

[TestFixture]
public class DateRestrictionTests
{
    private DateRestriction _dateRestriction;

    [SetUp]
    public void Setup()
    {
        _dateRestriction = new DateRestriction();
    }

    [Test]
    public void Validate_PinguinOnWeekend_ReturnsTrue()
    {
        var animals = new List<Animal> { new Animal { Name = "Pinguïn" } };
        var booking = new Booking { SelectedDate = new DateTime(2024, 6, 8) }; 

        bool result = _dateRestriction.Validate(animals, booking);

        Assert.IsTrue(result);
    }

    [Test]
    public void Validate_DesertAnimalInWinter_ReturnsTrue()
    {
        var animals = new List<Animal> { new Animal { Type = TypeEnum.dessert } };
        var booking = new Booking { SelectedDate = new DateTime(2024, 12, 15) };
        bool result = _dateRestriction.Validate(animals, booking);

        Assert.IsTrue(result);
    }

    [Test]
    public void Validate_SnowAnimalInSummer_ReturnsTrue()
    {
        var animals = new List<Animal> { new Animal { Type = TypeEnum.snow } };
        var booking = new Booking { SelectedDate = new DateTime(2024, 7, 10) };

        bool result = _dateRestriction.Validate(animals, booking);

        Assert.IsTrue(result);
    }

    [Test]
    public void Validate_NoSpecialConditions_ReturnsFalse()
    {
        var animals = new List<Animal> { new Animal { Type = TypeEnum.farm } };
        var booking = new Booking { SelectedDate = new DateTime(2024, 5, 10) };

        bool result = _dateRestriction.Validate(animals, booking);

        Assert.IsFalse(result);
    }
}
