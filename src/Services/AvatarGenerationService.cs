using System;
using System.Collections.Generic;
using System.Drawing;

namespace Avatarize.Services
{
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

        public virtual string GenerateAvatar(AvatarQuery query)
        {
            var hash = _hashService.GetHash(query.Input);

            var images = new List<Image>();

            if (query.Background.HasValue)
                images.Add(_assetsService.Background);

            if (query.Gradient.HasValue)
                images.Add(_assetsService.Gradient);

            if (query.Vignette.HasValue)
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

            if (query.Frame.HasValue)
                images.Add(_assetsService.Frame);

            return _imageService.GenerateBase64AvatarImage(images, query.Size?? 200);
        }

        public uint GetPartOf(uint value, int startIndex, int size)
        {
            var result = value << startIndex;
            result = result >> (32 - size);
            
            return result;
        }
    }
}
