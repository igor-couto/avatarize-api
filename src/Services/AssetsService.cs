using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Avatarize.Services
{
    public class AssetsService : IDisposable
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
            
            using (var bmpTemp = new Bitmap(currentPath + @"\Images\" + "Background.png"))
                Background = new Bitmap(bmpTemp);
            
            using (var bmpTemp = new Bitmap(currentPath + @"\Images\" + "Frame.png"))
                Frame = new Bitmap(bmpTemp);

            using (var bmpTemp = new Bitmap(currentPath + @"\Images\" + "Gradient.png"))
                Gradient = new Bitmap(bmpTemp);

            using (var bmpTemp = new Bitmap(currentPath + @"\Images\" + "Vignette.png"))
                Vignette = new Bitmap(bmpTemp);
        }

        private void LoadSkins(string currentPath) 
        {
            var skinPath = currentPath + "\\Images\\Skin\\";
            var skinCount = Directory.GetFiles(skinPath, "*", SearchOption.TopDirectoryOnly).Length;

            for (var i = 1; i <= skinCount; i++) 
            {
                using var bmpTemp = new Bitmap(skinPath + i + ".png");
                Skins.Add(new Bitmap(bmpTemp));
            }
        }

        private void LoadHairs(string currentPath) 
        {
            var hairPath = currentPath + "\\Images\\Hair\\";
            var hairCount = Directory.GetFiles(hairPath, "*", SearchOption.TopDirectoryOnly).Length;

            for (var i = 1; i <= hairCount; i++)
            {
                using var bmpTemp = new Bitmap(hairPath + i + ".png");
                Hairs.Add(new Bitmap(bmpTemp));
            }
        }

        private void LoadClothes(string currentPath)
        {
            var clothesPath = currentPath + "\\Images\\Clothes\\";
            var clothesCount = Directory.GetFiles(clothesPath, "*", SearchOption.TopDirectoryOnly).Length;

            for (var i = 1; i <= clothesCount; i++)
            {
                using var bmpTemp = new Bitmap(clothesPath + i + ".png");
                Clothes.Add(new Bitmap(bmpTemp));
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
}
