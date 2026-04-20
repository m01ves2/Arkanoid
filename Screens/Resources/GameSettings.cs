using System;
using System.IO;

namespace Arkanoid.Screens.Resources
{
    public class GameSettings
    {
        public string SelectedLevel;

        public GameSettings()
        {
           SelectedLevel = GetFirstLevelOrNull();// = "level1.txt";
        }

        private string GetFirstLevelOrNull()
        {
            var levelsDir = Path.Combine(AppContext.BaseDirectory, "Levels");

            if (!Directory.Exists(levelsDir))
                return null;

            var files = Directory.GetFiles(levelsDir, "*.txt");

            if (files.Length == 0)
                return null;

            //return Path.GetFileName(files[0]); // имя файла
            return files[0]; // полный путь
        }
    }
}
