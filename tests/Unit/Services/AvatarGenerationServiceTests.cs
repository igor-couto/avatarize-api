using System.Collections.Generic;
using NUnit.Framework;
using FakeItEasy;
using Avatarize.Services;
using Avatarize.Requests;
using SixLabors.ImageSharp;

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
    public void Create_ShouldCall_GetHash()
    {
        var query = new AvatarQueryParameters { Input = Faker.Person.FullName };
        _avatarGenerationService.Create(query);

        A.CallTo(() => _hashService.GetHash(query.Input)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_ShouldCall_GenerateAvatarImage()
    {
        var query = new AvatarQueryParameters { Input = Faker.Person.FullName };
        _avatarGenerationService.Create(query);

        A.CallTo(() => _imageService.GenerateAvatarImage(A<List<Image>>.Ignored, query.Size.Value)).MustHaveHappenedOnceExactly();
    }

    [Test]
    public void Create_ShouldCall_GenerateAvatarImage_WithExpectedParameters()
    {
        var query = new AvatarQueryParameters
        {
            Input = Faker.Person.FullName,
            Size = Faker.Random.Number(24, 1000),
            Background = true,
            Frame = true,
            Gradient = true,
            Vignette = true
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
            .GenerateAvatarImage(
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
