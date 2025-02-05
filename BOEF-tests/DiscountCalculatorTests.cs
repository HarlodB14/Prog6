using NUnit.Framework;
using System;
using System.Collections.Generic;
using beestje_op_je_feestje.Models;

[TestFixture]
public class DiscountCalculatorTests
{
    private DiscountCalculator _discountCalculator;

    [SetUp]
    public void Setup()
    {
        _discountCalculator = new DiscountCalculator("None");
    }

    [Test]
    public void BookingDayDiscount_Monday_Returns15()
    {
        DateTime bookingDate = new DateTime(2024, 2, 5); 
        double result = _discountCalculator.BookingDayDiscount(bookingDate);
        Assert.AreEqual(15.0, result);
    }

    [Test]
    public void BookingDayDiscount_Tuesday_Returns15()
    {
        DateTime bookingDate = new DateTime(2024, 2, 6); 
        double result = _discountCalculator.BookingDayDiscount(bookingDate);
        Assert.AreEqual(15.0, result);
    }

    [Test]
    public void BookingDayDiscount_Friday_Returns0()
    {
        DateTime bookingDate = new DateTime(2024, 2, 9);
        double result = _discountCalculator.BookingDayDiscount(bookingDate);
        Assert.AreEqual(0.0, result);
    }

    [Test]
    public void AnimalNameDiscount_SingleAnimal_ReturnsCorrectValue()
    {
        var animals = new List<Animal> { new Animal { Name = "Aap" } }; 
        double result = _discountCalculator.AnimalNameDiscount(animals);
        Assert.AreEqual(21.0, result);
    }


    [Test]
    public void AnimalNameDiscount_EmptyList_ReturnsZero()
    {
        var animals = new List<Animal>();
        double result = _discountCalculator.AnimalNameDiscount(animals);
        Assert.AreEqual(0.0, result);
    }

    // Discount Card Tests
    [Test]
    public void DiscountCardDiscount_WithCard_Returns10()
    {
        double result = _discountCalculator.DiscountCardDiscount(true);
        Assert.AreEqual(10.0, result);
    }

    [Test]
    public void DiscountCardDiscount_WithoutCard_Returns0()
    {
        double result = _discountCalculator.DiscountCardDiscount(false);
        Assert.AreEqual(0.0, result);
    }

    // Three of a Kind Discount Tests
    [Test]
    public void ThreeOfAKind_ThreeSameTypeAnimals_Returns10()
    {
        var animals = new List<Animal>
        {
            new Animal { Type = TypeEnum.farm },
            new Animal { Type = TypeEnum.farm },
            new Animal { Type = TypeEnum.farm }
        };
        double result = _discountCalculator.ThreeOfAKind(animals);
        Assert.AreEqual(10.0, result);
    }

    [Test]
    public void ThreeOfAKind_TwoSameTypeAnimals_Returns0()
    {
        var animals = new List<Animal>
        {
            new Animal { Type = TypeEnum.farm },
            new Animal { Type = TypeEnum.farm },
            new Animal { Type = TypeEnum.snow }
        };
        double result = _discountCalculator.ThreeOfAKind(animals);
        Assert.AreEqual(0.0, result);
    }

    [Test]
    public void ThreeOfAKind_EmptyList_Returns0()
    {
        var animals = new List<Animal>();
        double result = _discountCalculator.ThreeOfAKind(animals);
        Assert.AreEqual(0.0, result);
    }

    [Test]
    public void DuckDiscount_WithDuck_HasChanceToReturn50()
    {
        var animals = new List<Animal> { new Animal { Name = "Eend" } };
        double result = _discountCalculator.DuckDiscount(animals);
        Assert.That(result, Is.EqualTo(50.0).Or.EqualTo(0.0)); 
    }

    [Test]
    public void DuckDiscount_NoDuck_Returns0()
    {
        var animals = new List<Animal> { new Animal { Name = "Kat" } };
        double result = _discountCalculator.DuckDiscount(animals);
        Assert.AreEqual(0.0, result);
    }

    [Test]
    public void ApplyMaxDiscount_LessThan60_ReturnsSameValue()
    {
        double result = _discountCalculator.ApplyMaxDiscount(40.0);
        Assert.AreEqual(40.0, result);
    }

    [Test]
    public void ApplyMaxDiscount_MoreThan60_Returns60()
    {
        double result = _discountCalculator.ApplyMaxDiscount(70.0);
        Assert.AreEqual(60.0, result);
    }

    [Test]
    public void CalculateTotalDiscount_WithVariousDiscounts_ReturnsExpectedValue()
    {
        var animals = new List<Animal>
        {
            new Animal { Name = "Aap", Type = TypeEnum.farm },
            new Animal { Name = "Eend", Type = TypeEnum.farm },
            new Animal { Name = "Eend", Type = TypeEnum.farm }
        };
        DateTime bookingDate = new DateTime(2024, 2, 5);
        bool hasDiscountCard = true;
        _discountCalculator = new DiscountCalculator("ThreeOfKind");

        double result = _discountCalculator.CalculateTotalDiscount(animals, bookingDate, hasDiscountCard);
        Assert.That(result, Is.LessThanOrEqualTo(60.0));
    }

    [Test]
    public void CalculateTotalDiscount_NoDiscounts_ReturnsZero()
    {
        var animals = new List<Animal>();
        DateTime bookingDate = new DateTime(2024, 2, 9); 
        bool hasDiscountCard = false;
        _discountCalculator = new DiscountCalculator("None");

        double result = _discountCalculator.CalculateTotalDiscount(animals, bookingDate, hasDiscountCard);
        Assert.AreEqual(0.0, result);
    }
}