using System;

namespace avatarize.Services
{
    public class AvatarGenerationService
    {
        private readonly HashService _hashService;
        private readonly ImageService _imageService;

        public AvatarGenerationService(HashService hashService, ImageService imageService)
        {
            _hashService = hashService;
            _imageService = imageService;
        }

        public string GenerateAvatar(string input)
        {
            var seed = _hashService.SDBMHash(input);
            var random = new Random(seed);

            var skinTone = random.Next(1, 9);
            var clothing = random.Next(1, 23);

            return _imageService.MergeImages(clothing.ToString(), skinTone.ToString());
        }
    }
}
