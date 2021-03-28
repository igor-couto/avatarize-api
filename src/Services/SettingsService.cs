using System.IO;

namespace avatarize.Services
{
    public class SettingsService
    {
        public string SkinPath { get; }
        public string HairPath { get; }
        public string ClothesPath { get; }

        public int SkinCount { get; }
        public int HairCount { get; }
        public int ClothesCount { get; }


        public SettingsService() 
        {
            var currentPath = Directory.GetCurrentDirectory();

            SkinPath = currentPath + "\\Images\\Skin\\";
            //HairPath = currentPath + "\\Images\\Hair\\";
            ClothesPath = currentPath + "\\Images\\Clothes\\";

            SkinCount = Directory.GetFiles(SkinPath, "*", SearchOption.TopDirectoryOnly).Length;
            //HairCount = Directory.GetFiles(HairPath, "*", SearchOption.TopDirectoryOnly).Length;
            ClothesCount = Directory.GetFiles(ClothesPath, "*", SearchOption.TopDirectoryOnly).Length;
        }
    }
}
