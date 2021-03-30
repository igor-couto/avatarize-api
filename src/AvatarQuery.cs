using System.Collections.Generic;

namespace Avatarize
{
    public class AvatarQuery
    {
        public string Input { get; set; }

        public int? Size { get; set; }

        public bool? Background { get; set; } = true;

        public bool? Vignette { get; set; }

        public bool? Gradient { get; set; }

        public bool? Frame { get; set; } = true;

        public string Validate()
        {
            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(Input))
                errorMessages.Add("The input cannot be empty");

            if (Size.HasValue) 
            {
                if (Size < 24)
                    errorMessages.Add("The image size should not be less than 24 pixels");
                if (Size > 1000)
                    errorMessages.Add("The image size should not be greater than 1000 pixels");
            }

            return string.Join("\n", errorMessages);
        }
    }
}
