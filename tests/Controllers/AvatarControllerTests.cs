using NUnit.Framework;
using avatarize.Controllers;
using avatarize.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Avatarize;

namespace Avatarize_Tests.Controllers
{
    public class AvatarControllerTests : BaseTests
    {
        private AvatarController _avatarController;
        private AvatarGenerationService _avatarGenerationService;

        [SetUp]
        public void SetUp()
        {
            _avatarGenerationService = A.Fake<AvatarGenerationService>();
            _avatarController = new AvatarController(_avatarGenerationService);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void ShouldNotCallGenerateAvatarWhenInputIsEmpty(string input)
        {
            var query = new AvatarQuery { Input = input };

            _avatarController.Get(query);
            A.CallTo(() => _avatarGenerationService.GenerateAvatar(query)).MustNotHaveHappened();
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void ShouldRespondBadRequestWithMessageWhenInputIsEmpty(string input)
        {
            var query = new AvatarQuery { Input = input };

            var response = _avatarController.Get(query) as OkObjectResult;

            response?.StatusCode
                .Should()
                .Be(StatusCodes.Status400BadRequest);

            response?.Value
                .Should()
                .BeEquivalentTo("The input cannot be empty");
        }

        [Test]
        public void ShouldCallGenerateAvatar()
        {
            var query = new AvatarQuery { Input = Faker.Person.FullName };

            _avatarController.Get(query);

            A.CallTo(() => _avatarGenerationService.GenerateAvatar(query)).MustHaveHappenedOnceExactly();
        }

        public class CompleteTest : BaseTests
        {
            [Test]
            public void ShouldReturnOkWithGeneratedImage()
            {
                var assetsService = new AssetsService();
                var imageService = new ImageService();
                var hashService = new HashService();
                var avatarGenerationService = new AvatarGenerationService(hashService, imageService, assetsService);

                var avatarController = new AvatarController(avatarGenerationService);

                var query = new AvatarQuery { Input = "Igor Couto" };
                var expectedBase64Image = "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";

                var response = avatarController.Get(query) as OkObjectResult;

                response?.StatusCode
                    .Should()
                    .Be(StatusCodes.Status200OK);

                response?.Value
                    .Should()
                    .BeEquivalentTo(expectedBase64Image);
            }
        }
    }
}
