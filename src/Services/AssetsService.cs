namespace Avatarize.Services;

public class AssetsService : IDisposable
{
    public List<Image> Skins { get; }
    public List<Image> Hairs { get; }
    public List<Image> Clothes { get; }

    public Image Background { get; }
    public Image Frame { get; }
    public Image Gradient { get; }
    public Image Vignette { get; }

    private static readonly char Separator = Path.DirectorySeparatorChar;

    public AssetsService()
    {
        var currentPath = $"{Directory.GetCurrentDirectory()}{Separator}Images{Separator}";

        Skins = new List<Image>();
        Hairs = new List<Image>();
        Clothes = new List<Image>();

        LoadSkins(currentPath);
        LoadHairs(currentPath);
        LoadClothes(currentPath);

        Background = Image.Load<Rgba32>(currentPath + "Background.png");

        Frame = Image.Load<Rgba32>(currentPath + "Frame.png");

        Gradient = Image.Load<Rgba32>(currentPath + "Gradient.png");

        Vignette = Image.Load<Rgba32>(currentPath + "Vignette.png");
    }

    private void LoadSkins(string currentPath)
    {
        var skinPath = currentPath + "Skin" + Separator;
        var skinCount = Directory.GetFiles(skinPath, "*", SearchOption.TopDirectoryOnly).Length;

        for (var i = 1; i <= skinCount; i++)
        {
            var skinImage = Image.Load<Rgba32>(skinPath + i + ".png");
            Skins.Add(skinImage);
        }
    }

    private void LoadHairs(string currentPath)
    {
        var hairPath = currentPath + "Hair" + Separator;
        var hairCount = Directory.GetFiles(hairPath, "*", SearchOption.TopDirectoryOnly).Length;

        for (var i = 1; i <= hairCount; i++)
        {
            var hairImage = Image.Load<Rgba32>(hairPath + i + ".png");
            Hairs.Add(hairImage);
        }
    }

    private void LoadClothes(string currentPath)
    {
        var clothesPath = currentPath + "Clothes" + Separator;
        var clothesCount = Directory.GetFiles(clothesPath, "*", SearchOption.TopDirectoryOnly).Length;

        for (var i = 1; i <= clothesCount; i++)
        {
            var clothingImage = Image.Load<Rgba32>(clothesPath + i + ".png");
            Clothes.Add(clothingImage);
        }
    }

    public void Dispose()
    {
        Background.Dispose();
        Frame.Dispose();
        Gradient.Dispose();
        Vignette.Dispose();

        foreach (var skin in Skins)
            skin.Dispose();

        foreach (var hair in Hairs)
            hair.Dispose();

        foreach (var clothe in Clothes)
            clothe.Dispose();
    }
}