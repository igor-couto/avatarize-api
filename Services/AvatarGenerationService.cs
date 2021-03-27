using System;
using System.IO;

namespace avatarize.Services
{
    public class AvatarGenerationService
    {
        private readonly HashService _hashService;
        private readonly ImageService _imageService;
        private readonly SettingsService _settingsService;

        public AvatarGenerationService(HashService hashService, ImageService imageService, SettingsService settingsService)
        {
            _hashService = hashService;
            _imageService = imageService;
            _settingsService = settingsService;
        }

        public string GenerateAvatar(string input)
        {
            var seed = _hashService.SDBMHash(input);
            var random = new Random(seed);

            var skin = random.Next(1, _settingsService.SkinCount);
            var clothing = random.Next(1, _settingsService.ClothesCount);

            return _imageService.MergeImages(clothing.ToString(), skin.ToString());
        }
    }
}
