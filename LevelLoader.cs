using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Arkanoid
{
    public static class LevelLoader
    {
        private static readonly HashSet<char> allowed = new() { '#', '1', '2', '3', '.', ' ' };
       public static LevelLoadResult LoadSafe(string path)
        {
            try {
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) {
                    return new LevelLoadResult( CreateFallbackLevel(), "Default level loaded");
                }

                var lines = File.ReadAllLines(path);
                return new LevelLoadResult( GenerateLevelField(lines), null);
            }
            catch {
                return new LevelLoadResult( CreateFallbackLevel(), "Level failed to load. Default level started.");
            }
        }

        public static char[,] CreateFallbackLevel()
        {
            string[] lines = new string[] { "111", "111" };
            return GenerateLevelField(lines);
        }

        public static char[,] GenerateLevelField(string[] lines)
        {
            var maxWidth = 0;
            foreach (var line in lines) {
                maxWidth = Math.Max(maxWidth, line.Length);
            }

            char[,] result = new char[lines.Length, maxWidth];


            for (int i = 0; i < lines.Length; i++) {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++) {
                    char c = line[j];
                    if (!allowed.Contains(c)) {
                        System.Diagnostics.Debug.WriteLine($"Warning: invalid symbol '{c}' replaced");
                        c = ' ';
                    }
                    result[i, j] = c;
                }

                for (int k = line.Length; k < maxWidth; k++) {
                    result[i, k] = ' ';
                }
            }
            return result;
        }
    }
}
