using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using NUnit.Framework;
using Avatarize.Endpoints;
using Avatarize.Requests;
using IntegrationTests;

namespace IntegrationTests;

public class EndpointsTests
{
    private HttpClient _httpClient;
    private Faker Faker;
    private AvatarRequest _avatarRequest;

    [SetUp]
    public void SetUp()
    {
        _httpClient = new TestApplication().CreateClient();

        Faker = new Faker("en");

        _avatarRequest = new()
        {
            Input = Faker.Random.Word(),
            Size = Faker.Random.Number(24, 100),
            Background = Faker.Random.Bool(),
            Gradient = Faker.Random.Bool(),
            Frame = Faker.Random.Bool(),
            Vignette = Faker.Random.Bool()
        };
    }

    [Test]
    public async Task ShouldReturnSuccessStatusCode()
    {
        var response = await _httpClient.GetAsync($@"/avatar?input={_avatarRequest.Input}&size={_avatarRequest.Size}&background={_avatarRequest.Background}&gradient={_avatarRequest.Gradient}&frame={_avatarRequest.Frame}&vignette={_avatarRequest.Vignette}");

        response.EnsureSuccessStatusCode();
    }

    [Test]
    [Ignore("TODO: Check this test")]
    public async Task ShouldReturnOkWithGeneratedImage()
    {
        _avatarRequest = new AvatarRequest
        {
            Input = "Igor Couto",
            Size = 50,
            Background = true,
            Gradient = false,
            Frame = true,
            Vignette = false
        };

        var expectedBase64Image = "\"iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAQ4SURBVGhD7ZnpUhNREIVTvoGCYgiQEAjZCVkIZDFhC4gKCIL7vu+lUqUv4iv4HJa/fSjse2bO5HKZAUGl9JZd9f0gM7f7nEn3nRkSMuPb1+87ir8xqM2Vun/880ZMA4WZLTAxew2U5m86LNwClcW7YHLpnsP5+6C6/MDloQ/OMZ7LtczF3MX5G4C1qYWxr6F/3kiQgeLcdVBaUOJv7pQX7wAWp7ipi4/B9KUnLk9BbeXZHniM53ItczF3uSO1BNamliBDMGWNEfczHwO7W4htweIUl8/nj0x99TmgQeZm6+1puQBDKuwz4g31HgPO1852YHE/YUelcfkVqEleBQ3x4nUNOa1GrXrYZ4TbK4eaSboGXgA/Ib+L5vprwItFQ2w1auP2rId9Rsqd24CLmIRJ/Qr/KWjIbDVuz2x/PewzYg43t9eGJFRkMpljo7X5DnAT4PZMbWwxPewzwq+t21LOcJ/beAui0eixMXN1GzQ33gBqobaK6FToYZ8RttbUhUeAi5tiQnEiFAJfPnXAcCQM+DePm/D4Yda0t7ZBc12MCLUVGXqB2qhVD/uMcPutnpcboVCT4VI0ZdgU7c33wK+wzkEi92P2+kcQOOxyW1BQqx72GeFt32sxc+ivyNALFGqyWo/74ncu+fy6Bd5ulkF76wPotpRzC+C2W5EtV7HvI4o1RrwXK3lwVJjbcU3MKPwEHRYaMI10t1v3odEYbhootEWroMd/I7/CHzWSO7cOuobcmVmSJEJVEirYt34CD8IUThrrsjsJ5kyUl+SZSvAMuNqoVQ/7jKSnL4JsYw2Mt64A/ueCe7f3viKmFCzeWHsJgsTrBngu7w9sX+ZmLb6b50WHgtoytUtAD/uMJCsdkJ5aBtn6Csg3L4NCexN4xuQNTcFBrNfrICR36CB4TiKRALwv8I2vKKIVrMXaWRGtSFWXAbXqYZ+RkYkZkCjNg+TkIugak2RCThIrxlsbgANIkcQU7wcNMVeuuQbYOqydrIgWIVGaA9Sqh31GYtk6GM43wUihDUaLs2Cs3AGpySUHKaCgGD+hQZhrPMGSVzFWXgCjxTlALXHRpaBWPewzMpCsgMFUFUQz08A0GFcJVeKJWfAzLRQE17JV4uMtwFqxnNQWomnRIlDbwFgZ6GGfkXB8HPSPToBIogS4aDA1CYbSUyCaqQEa9RN6EFzLi8bcrMXakUQRUBu16mGfkTNDKdAXzYCzsazDcA54RkcKIKLMwrBThG1yGLiWuZg7HM8D1qaWvmgaUKse9hgxf9XtjYw6DIyB04NJcGZQFis8w07SPlVA4JZKPBEa5jl9MbloCk+gi1uLtXsHEg4RB8auX3WtMcIwDZ08GwOnwsO76OmPg95+MSv0REaAdwFcTktBhfm5Hz39kkMnLDWEU6q+q0PB2CNeD2uMMExDf1P8lAHGfyPHEMFGQqEfthL8sjTZeFIAAAAASUVORK5CYII=\"";

        var response = await _httpClient.GetAsync($@"/avatar?input={_avatarRequest.Input}&size={_avatarRequest.Size}&background={_avatarRequest.Background}&gradient={_avatarRequest.Gradient}&frame={_avatarRequest.Frame}&vignette={_avatarRequest.Vignette}");

        response?.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        (await response.Content.ReadAsStringAsync())
            .Should()
            .BeEquivalentTo(expectedBase64Image);
    }

    [Test]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("  ")]
    [TestCase(null)]
    public async Task ShouldReturnBadRequestWithMessageWhenInputIsEmpty(string input)
    {
        var response = await _httpClient.GetAsync($@"/avatar?input={input}&size={_avatarRequest.Size}&background={_avatarRequest.Background}&gradient={_avatarRequest.Gradient}&frame={_avatarRequest.Frame}&vignette={_avatarRequest.Vignette}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);

        (await response.Content.ReadAsStringAsync())
            .Should()
            .BeEquivalentTo("\"The input cannot be empty\"");
    }

    [Test]
    [TestCase(23)]
    [TestCase(0)]
    [TestCase(int.MinValue)]
    public async Task ShouldBeInvalidWhenSizeIsLessThanAllowed(int size)
    {
        var response = await _httpClient.GetAsync($@"/avatar?input={_avatarRequest.Input}&size={size}&background={_avatarRequest.Background}&gradient={_avatarRequest.Gradient}&frame={_avatarRequest.Frame}&vignette={_avatarRequest.Vignette}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);

        (await response.Content.ReadAsStringAsync())
            .Should()
            .BeEquivalentTo("\"The image size should not be less than 24 pixels\"");
    }

    [Test]
    [TestCase(1001)]
    [TestCase(int.MaxValue)]
    public async Task ShouldBeInvalidWhenSizeIsGreaterThanAllowed(int size)
    {
        var response = await _httpClient.GetAsync($@"/avatar?input={_avatarRequest.Input}&size={size}&background={_avatarRequest.Background}&gradient={_avatarRequest.Gradient}&frame={_avatarRequest.Frame}&vignette={_avatarRequest.Vignette}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);

        (await response.Content.ReadAsStringAsync())
            .Should()
            .BeEquivalentTo("\"The image size should not be greater than 1000 pixels\"");
    }

    [Test]
    [TestCase(24)]
    [TestCase(1000)]
    public async Task ShouldBeValidWhenSizeIsExpected(int size)
    {
        var response = await _httpClient.GetAsync($@"/avatar?input={_avatarRequest.Input}&size={size}&background={_avatarRequest.Background}&gradient={_avatarRequest.Gradient}&frame={_avatarRequest.Frame}&vignette={_avatarRequest.Vignette}");

        response.EnsureSuccessStatusCode();
    }
}
