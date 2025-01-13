using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FakeItEasy;
using SixLabors.ImageSharp;
using FluentAssertions;
using Avatarize.Requests;
using Avatarize.Services;

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
}
