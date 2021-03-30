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
    }
}
