using avatarize.Services;
using Avatarize_Tests;
using FluentAssertions;
using NUnit.Framework;

namespace AvatarizeTests.Services
{
    public class HashServiceTests : BaseTests
    {
        [Test]
        [TestCase("6e843b70-f175-bf76-032a-3e5193c53b35", -966225594)]
        [TestCase("Igor Freitas Couto", -1601021125)]
        [TestCase("04111991", 1457457348)]
        public void ShoudReturnExpectedHash(string input, int expectedHash) 
        {
            var hashService = new HashService();
            hashService.GetHash(input).Should().Be(expectedHash);
        }
    }
}
