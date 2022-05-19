using Avatarize;
using FluentAssertions;
using NUnit.Framework;

namespace AvatarizeTests;

public class AvatarRequestTests : BaseTests
{
    [Test]
    public void ShouldBeValidWhenInputIsPresent()
    {
        var query = new AvatarRequest { Input = Faker.Person.FullName };

        query.Validate().Should().Be(string.Empty);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("  ")]
    [TestCase(null)]
    public void ShouldBeInvalidWhenInputIsEmpty(string input)
    {
        var query = new AvatarRequest { Input = input };

        query.Validate().Should().Be("The input cannot be empty");
    }

    [Test]
    [TestCase(23)]
    [TestCase(0)]
    [TestCase(int.MinValue)]
    public void ShouldBeInvalidWhenSizeIsLessThanAllowed(int size)
    {
        var query = new AvatarRequest()
        {
            Input = Faker.Person.FullName,
            Size = size
        };

        query.Validate().Should().Be("The image size should not be less than 24 pixels");
    }

    [Test]
    [TestCase(1001)]
    [TestCase(int.MaxValue)]
    public void ShouldBeInvalidWhenSizeIsGreaterThanAllowed(int size)
    {
        var query = new AvatarRequest()
        {
            Input = Faker.Person.FullName,
            Size = size
        };

        query.Validate().Should().Be("The image size should not be greater than 1000 pixels");
    }

    [Test]
    [TestCase(24)]
    [TestCase(1000)]
    public void ShouldBeValidWhenSizeIsExpected(int size)
    {
        var query = new AvatarRequest()
        {
            Input = Faker.Person.FullName,
            Size = size
        };


        query.Validate().Should().Be(string.Empty);
    }
}