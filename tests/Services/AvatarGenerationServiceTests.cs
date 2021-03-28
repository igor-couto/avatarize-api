using avatarize.Services;
using Avatarize_Tests;
using FakeItEasy;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;

namespace AvatarizeTests.Services
{
    public class AvatarGenerationServiceTests : BaseTests
    {
        private AvatarGenerationService _avatarGenerationService;
        private HashService _hashService;
        private ImageService _imageService;
        private AssetsService _assetsService;

        [SetUp]
        public void SetUp() 
        {
            _hashService = A.Fake<HashService>();
            _imageService = A.Fake<ImageService>();
            _assetsService = A.Fake<AssetsService>();

            _avatarGenerationService = new AvatarGenerationService(_hashService, _imageService, _assetsService);
        }

        [Test]
        public void ShouldCallGetHash() 
        {
            var input = Faker.Person.FullName;
            _avatarGenerationService.GenerateAvatar(input);

            A.CallTo( () => _hashService.GetHash(input)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void ShouldCallGenerateBase64AvatarImage() 
        {
            var input = Faker.Person.FullName;
            _avatarGenerationService.GenerateAvatar(input);

            A.CallTo(() => _imageService.GenerateBase64AvatarImage(A<List<Image>>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}
