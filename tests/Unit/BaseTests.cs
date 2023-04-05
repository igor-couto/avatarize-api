using Bogus;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace UnitTests;

public class BaseTests
{
    protected Faker Faker { get; private set; }

    [SetUp]
    public void AllTestsSetUp()
    {
        Faker = new Faker("en");

        AssertionOptions.AssertEquivalencyUsing(options =>
        {
            options.Using<DateTime>(ctx =>
                    ctx.Subject.ToLocalTime().Should()
                        .BeCloseTo(ctx.Expectation.ToLocalTime(), new TimeSpan(10000)))
                .WhenTypeIs<DateTime>();
            return options;
        });
    }

    public bool Compare<T>(T expected, T result)
    {
        try
        {
            expected.Should().BeEquivalentTo(result);
            return true;
        }
        catch
        {
            return false;
        }
    }
}