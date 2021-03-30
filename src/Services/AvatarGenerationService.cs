using Avatarize;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace avatarize.Services
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
            var seed = _hashService.GetHash(query.Input);
            var random = new Random(seed);

            var images = new List<Image>();

            if (query.Background.HasValue)
                images.Add(_assetsService.Background);

            if (query.Gradient.HasValue)
                images.Add(_assetsService.Gradient);

            if (query.Vignette.HasValue)
                images.Add(_assetsService.Vignette);

            images.Add(_assetsService.Skins[random.Next(1, _assetsService.Skins.Count)]);
            images.Add(_assetsService.Hairs[random.Next(1, _assetsService.Hairs.Count)]);
            images.Add(_assetsService.Clothes[random.Next(1, _assetsService.Clothes.Count)]);

            if (query.Frame.HasValue)
                images.Add(_assetsService.Frame);

            return _imageService.GenerateBase64AvatarImage(images, query.Size?? 200);
        }
    }
}
