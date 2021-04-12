using NUnit.Framework;
using Avatarize.Controllers;
using Avatarize.Services;
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
                var expectedBase64Image = "iVBORw0KGgoAAAANSUhEUgAAAMgAAADICAYAAACtWK6eAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAArLSURBVHhe7dw9ymXHFYXhi2dhFDQ2dNLQkbMOlDm1E8/CwqmHoURT0DiEYg1Kho8TbBcvu85y1fGtXbUWPFmf+rm1V/RBv/6X/PrLb79nHOeJ0Ky1rhF9b+hgkeM8EZq11jWi7w0dLHKcJ0Kz1rpG9L2hg0WO80Ro1lrXiL43dLDIcZ4IzVrrGtFnQxtHvfz9nz+aTXcnNK/RNeJjoYWjXuhyZqPuhOY1ukZ8LLRw1AtdzmzUndC8RteIj4UWjnqhy5mNuhOa1+ga8bHQwlEvdDmzUXdC8xpdIz4WWjjqhS5nNupOaF6ja8THQgtHvdDlzEbdCc1rdI34WGjhqBe6nNmoO6F5ja4Rvx9apBc6vNkKeqF5z7ggtpVeaN4zLohtpRea94wLYlvpheY944LYVnqhec+4ILaVXmjeMy6IbaUXmveMC2Jb6YXmPfO6vvuv0MY7+vr16xHo7qdS44IcgO5+KjUuyAHo7qdS44IcgO5+KjUuyAHo7qdS44IcgO5+KjUuyAHo7qdS44IcgO5+KjUuyAHo7qdSs3VBaFiM0e+3IzUuiH2g329HalwQ+0C/347UuCD2gX6/HalxQewD/X47UuOC2Af6/XakxgWxD/T77UiNC2If6PfbkZqyBaFHtufQG1SkxgWxW+gNKlLjgtgt9AYVqXFB7BZ6g4rUuCB2C71BRWpcELuF3qAiNS6I3UJvUJEaF8RuoTeoSM2yBaFHsnXRG65IjQtiU9AbrkiNC2JT0BuuSI0LYlPQG65IjQtiU9AbrkiNC2JT0BuuSI0LYlPQG65IjQtiU9AbrkiNC2JT0BuuSM2yBfny5YsVQm+4IjUuiE1Bb7giNS6ITUFvuCI1LohNQW+4IjUuiE1Bb7giNS6ITUFvuCI1LohNQW+4IjUuiE1Bb7giNcsW5NOnT1YIveGK1LggNgW94YrUuCA2Bb3hitS4IDYFveGK1LggNgW94YrUuCA2Bb3hitS4IDYFveGK1LggNgW94YrULFuQP7xeqZ///deuP333xxR9E9G+s9G+LTp7RN9EtO9s9IYrUuOCJGjf2WjfFp09om8i2nc2esMVqXFBErTvbLRvi84e0TcR7TsbveGK1LggCdp3Ntq3RWeP6JuI9p2N3nBFalyQBO07G+3borNH9E1E+85Gb7giNS5IgvadjfZt0dkj+iaifWejN1yRGhckQfvORvu26OwRfRPRvrPRG65IjQuSoH1no31bdPaIvolo39noDVekxgVJ0L6z0b4tOntE30S072z0hitSs2xBeuiR3oEGMqJvKqI3qEiNCzKIShHRNxXRG1SkxgUZRKWI6JuK6A0qUuOCDKJSRPRNRfQGFalxQQZRKSL6piJ6g4rUuCCDqBQRfVMRvUFFalyQQVSKiL6piN6gIjUuyCAqRUTfVERvUJGasgWhYZztb9/+/Djad7affvi+61//+EuK3qAiNS5IggZ6Ntp3NipEi0oR0RtUpMYFSdBAz0b7zkaFaFEpInqDitS4IAka6Nlo39moEC0qRURvUJEaFyRBAz0b7TsbFaJFpYjoDSpS44IkaKBno31no0K0qBQRvUFFalyQBA30bLTvbFSIFpUiojeoSI0LkqCBno32nY0K0aJSRPQGFalxQTZAA6+iUkT0BhWpcUE2QAOvolJE9AYVqXFBNkADr6JSRPQGFalxQTZAA6+iUkT0BhWpcUE2QAOvolJE9AYVqXFBNkADr6JSRPQGFalxQTZAA6+iUkT0BhWpcUE2QAOvolJE9AYVqXFBNkADr6JSRPQGFakpW5A7aJgqooGOaKBV9PvtSI0LUgCVIqKBV9HvtyM1LkgBVIqIBl5Fv9+O1LggBVApIhp4Ff1+O1LjghRApYho4FX0++1IjQtSAJUiooFX0e+3IzUuSAFUiogGXkW/347UuCAFUCkiGngV/X47UrN1Qf4faKAVNPAtGuiIzmVMjQsyiIZeQYVoUSkiOpcxNS7IIBp6BRWiRaWI6FzG1Lggg2joFVSIFpUionMZU+OCDKKhV1AhWlSKiM5lTI0LMoiGXkGFaFEpIjqXMTUuyCAaegUVokWliOhcxtS4IINo6BVUiBaVIqJzGVPjgiS+ffvW9Xq9Hkf7Rp8/f07R3U6lxgVJ0DC2aKBno30jKkVEdzuVGhckQcPYooGejfaNqBQR3e1UalyQBA1jiwZ6Nto3olJEdLdTqXFBEjSMLRro2WjfiEoR0d1OpcYFSdAwtmigZ6N9IypFRHc7lRoXJEHD2KKBno32jagUEd3tVGpckAQNY4sGejbaN6JSRHS3U6lxQRI0jC0a6Nlo34hKEdHdTqXGBUnQMKpo4CP6ZjYqTYvuvyM1LkiChk1FpYjom9moEC26/47UuCAJGjYVlSKib2ajQrTo/jtS44IkaNhUVIqIvpmNCtGi++9IjQuSoGFTUSki+mY2KkSL7r8jNS5IgoZNRaWI6JvZqBAtuv+O1LggCRo2FZUiom9mo0K06P47UuOCJGjYVFSKiL6ZjQrRovvvSM3RBaFBiWjYVkRnj+ibFv0+O1LjgiRokFZEZ4/omxb9PjtS44IkaJBWRGeP6JsW/T47UuOCJGiQVkRnj+ibFv0+O1LjgiRokFZEZ4/omxb9PjtS44IkaJBWRGeP6JsW/T47UuOCJGiQVkRnj+ibFv0+O1LjgiRokFZEZ4/omxb9PjtSc3RB6A93EQ1SRXS3Fv0+O1LjgiRo2Cqiu7Xo99mRGhckQcNWEd2tRb/PjtS4IAkatorobi36fXakxgVJ0LBVRHdr0e+zIzUuSIKGrSK6W4t+nx2pcUESNGwV0d1a9PvsSI0LkqBhq4ju1qLfZ0dqXJAEDVtFdLcW/T47UnN0QXpo2Cqiu51KjQuSoGGriO52KjUuSIKGrSK626nUuCAJGraK6G6nUuOCJGjYKqK7nUqNC5KgYauI7nYqNS5IgoatIrrbqdS4IAkatorobqdS44Ik6A9qFdHdTqXGBUnQsFVEdzuVGhckQcNWEd3tVGpckAQNW0V0t1OpcUESNGwV0d1OpcYFSdCwVUR3O5UaFyRBw1YR3e1UalyQBA1bRXS3U6l5/frLb7+3eqGNd0T/S6GK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXJAEDZuK1lXRugpac1e90LxnXhT6h1EvdHCzUXdC8xpdIz4WWjjqhS5nNupOaF6ja8THQgtHvdDlzEbdCc1rdI34WGjhqBe6nNmoO6F5ja4RHwstHPVClzMbdSc0r9E14mOhhaNe6HJmo+6E5jW6RnwstHDUC13ObNSd0LxG14iPhRaOeqHLmY26E5rX6BrxZ0MbR47zRGjWWteIvjd0sMhxngjNWusa0feGDhY5zhOhWWtdI/re0MEix3kiNGuta0TfGzpY5DhPhGatdY3oe0MHixznidCsta4RfW/oYJHjPBGatdY1ou8NHSxynCdCs9a6RvS9oYNFjvNEaNZa14i+N3SwyHGeCM1a6xrR94YOFjnOE6FZa10j+t7QwSLHeSI0a61rRCfl9foPkgO+3VkI7aMAAAAASUVORK5CYII=";

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
