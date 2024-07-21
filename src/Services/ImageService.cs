using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace Avatarize.Services;

public class ImageService
{
    public virtual byte[] GenerateAvatarImage(List<Image> images, int size)
    {
        var generatedImage = MergeImages(images);

        generatedImage = ResizeImage(generatedImage, size, size);

        return GetImageBytes(generatedImage);
    }

    private static Image MergeImages(List<Image> images)
    {        
        int width = images[0].Width;
        int height = images[0].Height;

        var outputImage = new Image<Rgba32>(width, height);

        foreach (var inputImage in images)
        {
            outputImage.Mutate(ctx => ctx.DrawImage(inputImage, new Point(0, 0), 1));
        }

        return outputImage;
    }

    private static Image ResizeImage(Image image, int width, int height)
    {
        image.Mutate(x => x
            .Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Sampler = new NearestNeighborResampler(),
                Compand = false
            }));

        image.Metadata.HorizontalResolution = 72;
        image.Metadata.VerticalResolution = 72;

        return image;
    }

    private static byte[] GetImageBytes(Image image)
    {
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());
        return memoryStream.ToArray();
    }
}