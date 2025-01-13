using Bogus;

namespace UnitTests;

public class BaseTests
{
    protected Faker Faker { get; private set; } = new Faker("en");
}