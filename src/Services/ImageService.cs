using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Image = System.Drawing.Image;

namespace Avatarize.Services
{
    public class ImageService
    {
        public virtual string GenerateBase64AvatarImage(List<Image> images, int size) 
        {
            var generatedImage = MergeImages(images);

            generatedImage = ResizeImage(generatedImage, size, size);

            return ConvertToBase64String(generatedImage);
        }

        private Image MergeImages(List<Image> images) 
        {
            var generatedImage = (Image)images.First().Clone();

            var graphics = Graphics.FromImage(generatedImage);

            foreach (var image in images)
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);

            return generatedImage;
        }

        private string ConvertToBase64String(Image generatedImage) { 
            using var memoryStream = new MemoryStream();
            generatedImage.Save(memoryStream, ImageFormat.Png);
            var imageBytes = memoryStream.ToArray();

            return Convert.ToBase64String(imageBytes);
        }

        private Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.Default;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.PixelOffsetMode = PixelOffsetMode.None;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
            return destImage;
        }
    }
}
