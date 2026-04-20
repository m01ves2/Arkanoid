using System;
using System.Collections.Generic;
using System.IO;

namespace Arkanoid.Core
{
    public static class LevelLoader
    {
        private static HashSet<char> allowed = new() { '#', '1', '2', '3', '.', ' ' };
        public static char[,] Load(string path)
        {
            var lines = File.ReadAllLines(path);

            return GenerateLevelField(lines);
        }

        public static char[,] CreateFallbackLevel()
        {
            string[] lines = new string[] { "1111111111", "1111111112" };
            return GenerateLevelField(lines);
        }

        public static char[,] GenerateLevelField(string[] lines)
        {
            var maxWidth = 0;
            foreach (var line in lines) {
                maxWidth = line.Length < maxWidth ? maxWidth : line.Length;
            }

            char[,] result = new char[lines.Length, maxWidth];


            for (int i = 0; i < lines.Length; i++) {
                var line = lines[i];
                for (int j = 0; j < line.Length; j++) {
                    char c = line[j];
                    if (!allowed.Contains(c)) {
                        c = ' ';
                        System.Diagnostics.Debug.WriteLine($"Warning: invalid symbol '{c}' replaced");
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
