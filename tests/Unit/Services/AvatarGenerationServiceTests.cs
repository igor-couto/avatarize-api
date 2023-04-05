using System.Collections.Generic;
using System.Drawing;
using NUnit.Framework;
using FakeItEasy;
using Avatarize.Services;
using Avatarize.Requests;

namespace UnitTests.Services;

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
        var query = new AvatarRequest { Input = Faker.Person.FullName };
        _avatarGenerationService.Create(query);

        A.CallTo(() => _hashService.GetHash(query.Input)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ShouldCallGenerateBase64AvatarImage()
    {
        var query = new AvatarRequest { Input = Faker.Person.FullName };
        _avatarGenerationService.Create(query);

        A.CallTo(() => _imageService.GenerateBase64AvatarImage(A<List<Image>>.Ignored, A<int>.Ignored)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void ShouldCallGenerateBase64AvatarImageWithExpectedParameters()
    {
        var query = new AvatarRequest
        {
            Input = Faker.Person.FullName,
            Background = true,
            Frame = true,
            Gradient = true,
            Vignette = true,
            Size = Faker.Random.Number(24, 1000)
        };

        var expectedImageArray = new List<Image>()
        {
            _assetsService.Background,
            _assetsService.Gradient,
            _assetsService.Vignette,
            _assetsService.Frame

        };

        _avatarGenerationService.Create(query);

        A.CallTo(() => _imageService
            .GenerateBase64AvatarImage(
                A<List<Image>>.That.Matches(x =>
                    x.Count == 7 &&
                    Compare(_assetsService.Background, x[0]) &&
                    Compare(_assetsService.Gradient, x[1]) &&
                    Compare(_assetsService.Vignette, x[2]) &&
                    Compare(_assetsService.Frame, x[6])),
                A<int>.That.Matches(x => Compare(query.Size, x))))
            .MustHaveHappenedOnceExactly();
    }
}
