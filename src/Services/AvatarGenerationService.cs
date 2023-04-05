using System.Drawing;
using Avatarize.Requests;

namespace Avatarize.Services;

public class AvatarGenerationService
{
    private readonly HashService _hashService;
    private readonly ImageService _imageService;
    private readonly AssetsService _assetsService;

    public AvatarGenerationService(HashService hashService, ImageService imageService, AssetsService assetsService)
    {
        _hashService = hashService;
        _imageService = imageService;
        _assetsService = assetsService;
    }

    public string Create(AvatarQueryParameters request)
    {
        var hash = _hashService.GetHash(request.Input);

        var images = new List<Image>();

        if (request.Background.HasValue)
            images.Add(_assetsService.Background);

        if (request.Gradient.HasValue)
            images.Add(_assetsService.Gradient);

        if (request.Vignette.HasValue)
            images.Add(_assetsService.Vignette);

        images.Add(_assetsService.Skins[
            (int)(GetPartOf(hash, 8, 8) % _assetsService.Skins.Count)
        ]);

        images.Add(_assetsService.Hairs[
            (int)(GetPartOf(hash, 16, 8) % _assetsService.Hairs.Count)
        ]);

        images.Add(_assetsService.Clothes[
            (int)(GetPartOf(hash, 24, 8) % _assetsService.Clothes.Count)
        ]);

        if (request.Frame.HasValue)
            images.Add(_assetsService.Frame);

        return _imageService.GenerateBase64AvatarImage(images, request.Size ?? 200);
    }

    public uint GetPartOf(uint value, int startIndex, int size)
    {
        var result = value << startIndex;
        result = result >> (32 - size);

        return result;
    }
}