using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Avatarize.Services
{
    public class AssetsService
    {
        public List<Image> Skins { get; }
        public List<Image> Hairs { get; }
        public List<Image> Clothes { get; }

        public Image Background { get; }
        public Image Frame { get; }
        public Image Gradient { get; }
        public Image Vignette { get; }

        public AssetsService()
        {
            var currentPath = Directory.GetCurrentDirectory();

            Skins = new List<Image>();
            Hairs = new List<Image>();
            Clothes = new List<Image>();

            LoadSkins(currentPath);
            LoadHairs(currentPath);
            LoadClothes(currentPath);
            
            Background = Image.FromFile(currentPath + @"\Images\" + "Background.png");
            Frame = Image.FromFile(currentPath + @"\Images\" + "Frame.png");
            Gradient = Image.FromFile(currentPath + @"\Images\" + "Gradient.png");
            Vignette = Image.FromFile(currentPath + @"\Images\" + "Vignette.png");
        }

        private void LoadSkins(string currentPath) 
        {
            var skinPath = currentPath + "\\Images\\Skin\\";
            var skinCount = Directory.GetFiles(skinPath, "*", SearchOption.TopDirectoryOnly).Length;
            
            for (var i = 1; i <= skinCount; i++)
                Skins.Add(Image.FromFile(skinPath + i + ".png"));
        }

        private void LoadHairs(string currentPath) 
        {
            var hairPath = currentPath + "\\Images\\Hair\\";
            var hairCount = Directory.GetFiles(hairPath, "*", SearchOption.TopDirectoryOnly).Length;

            for (var i = 1; i <= hairCount; i++)
                Hairs.Add(Image.FromFile(hairPath + i + ".png"));
        }

        private void LoadClothes(string currentPath)
        {
            var clothesPath = currentPath + "\\Images\\Clothes\\";
            var clothesCount = Directory.GetFiles(clothesPath, "*", SearchOption.TopDirectoryOnly).Length;

            for (var i = 1; i <= clothesCount; i++)
                Clothes.Add(Image.FromFile(clothesPath + i + ".png"));
        }
    }
}
