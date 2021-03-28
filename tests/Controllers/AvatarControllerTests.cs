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
            A.CallTo( () => _avatarGenerationService.GenerateAvatar(input)).MustNotHaveHappened();
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

            A.CallTo( () => _avatarGenerationService.GenerateAvatar(input)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void ShouldReturnOkWithGeneratedImage()
        {
            var settingsService = new SettingsService();
            var imageService = new ImageService(settingsService);
            var hashService = new HashService();
            var avatarGenerationService = new AvatarGenerationService(hashService, imageService, settingsService);

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
