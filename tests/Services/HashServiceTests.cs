using Avatarize.Services;
using Avatarize_Tests;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace AvatarizeTests.Services
{
    public class HashServiceTests : BaseTests
    {
        //[Test]
        //[TestCase("6e843b70-f175-bf76-032a-3e5193c53b35", -966225594)]
        //[TestCase("Igor Freitas Couto", -1601021125)]
        //[TestCase("04111991", 1457457348)]
        //public void ShoudReturnExpectedHashWhenUsingSDBMHash(string input, int expectedHash) 
        //{
        //    var hashService = new HashService();
        //    hashService.GetHash(input).Should().Be(expectedHash);
        //}

        [Test]
        [TestCase("6e843b70-f175-bf76-032a-3e5193c53b35", 2073821555u)]
        [TestCase("Igor Freitas Couto", 1299966643u)]
        [TestCase("04111991", 1851254486u)]
        [TestCase("00000001111001110100111111011101", 2841252511u)]
        public void ShoudReturnExpectedHashWhenUsingHash(string input, uint expectedHash)
        {
            var hashService = new HashService();
            hashService.GetHash(input).Should().Be(expectedHash);
        }
    }
}
