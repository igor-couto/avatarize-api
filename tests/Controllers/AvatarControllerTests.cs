using NUnit.Framework;
using avatarize.Controllers;
using avatarize.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

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
            _avatarController.Get(input);
            A.CallTo(() => _avatarGenerationService.GenerateAvatar(input)).MustNotHaveHappened();
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void ShouldRespondBadRequestWithMessageWhenInputIsEmpty(string input)
        {
            var response = _avatarController.Get(input) as OkObjectResult;

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
            var input = Faker.Person.FullName;

            _avatarController.Get(input);

            A.CallTo(() => _avatarGenerationService.GenerateAvatar(input)).MustHaveHappenedOnceExactly();
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

                var input = "Igor Couto";
                var expectedBase64Image = "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";

                var response = avatarController.Get(input) as OkObjectResult;

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
