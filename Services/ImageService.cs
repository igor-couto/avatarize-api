using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Image = System.Drawing.Image;

namespace avatarize.Services
{
    public class ImageService
    {
        public string MergeImages(string overlayImageName, string bottomImageName)
        {
            var currentPath = Directory.GetCurrentDirectory();
            var bottomImage = Image.FromFile(Path.GetFullPath(currentPath + "\\Images\\Skin\\" + bottomImageName + ".png"));
            var overlayImage = Image.FromFile(Path.GetFullPath(currentPath + "\\Images\\Clothes\\" + overlayImageName + ".png"));

            var generatedImage = bottomImage;
            var graphics = Graphics.FromImage(generatedImage);

            graphics.DrawImage(overlayImage, 0, 0, overlayImage.Width, overlayImage.Height);

            generatedImage = ResizeImage(generatedImage, 200, 200);

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
