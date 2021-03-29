using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace avatarize.Services
{
    public class AssetsService
    {
        public List<Image> Skins { get; }
        public List<Image> Hairs { get; }
        public List<Image> Clothes { get; }

        public Image Background { get; }
        public Image Frame { get; }

        public AssetsService() 
        {
            var currentPath = Directory.GetCurrentDirectory();

            var skinPath = currentPath + "\\Images\\Skin\\";
            var hairPath = currentPath + "\\Images\\Hair\\";
            var clothesPath = currentPath + "\\Images\\Clothes\\";

            var skinCount = Directory.GetFiles(skinPath, "*", SearchOption.TopDirectoryOnly).Length;
            var hairCount = Directory.GetFiles(hairPath, "*", SearchOption.TopDirectoryOnly).Length;
            var clothesCount = Directory.GetFiles(clothesPath, "*", SearchOption.TopDirectoryOnly).Length;

            Skins = new List<Image>();
            Hairs = new List<Image>();
            Clothes = new List<Image>();

            for (var i = 1; i <= skinCount; i++) 
                Skins.Add(Image.FromFile(skinPath + i + ".png"));

            for (var i = 1; i <= clothesCount; i++)
                Clothes.Add(Image.FromFile(clothesPath + i + ".png"));

            for (var i = 1; i <= hairCount; i++)
                Hairs.Add(Image.FromFile(hairPath + i + ".png"));

            Background = Image.FromFile(currentPath + "\\Images\\" + "Background.png");
            Frame = Image.FromFile(currentPath + "\\Images\\" + "Frame.png");
        }
    }
}
