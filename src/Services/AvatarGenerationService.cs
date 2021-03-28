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

        public virtual string GenerateAvatar(string input)
        {
            var seed = _hashService.GetHash(input);
            var random = new Random(seed);

            var images = new List<Image>()
            {
                _assetsService.Skins[random.Next(1, _assetsService.Skins.Count)],
                _assetsService.Clothes[random.Next(1, _assetsService.Clothes.Count)]
            };

            return _imageService.GenerateBase64AvatarImage(images);
        }
    }
}
