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

                for (int k = line.Length; k < maxWidth - line.Length; k++) {
                    result[i, k] = ' ';
                }
            }
            return result;
        }

            //int bricksInScreenWidth = screenWidth / Brick.Width;
            //int bricksInScreenHeight = (int)(screenHeight * 0.3 / Brick.Height);
            //var maxHeight = Math.Min(lines.Length, bricksInScreenHeight);
            //for(int row = 0; row < maxHeight; row++) {
            //    var line = lines[row];
            //    var maxLength = Math.Min(bricksInScreenWidth, line.Length);

            //    for (int col = 0; col < maxLength; col++) {
            //        int hp = GetBrickHpBySymbol(line[col]);
            //        if (hp == 0) continue;

            //        var x = col * (Brick.Width + 2) + offsetX;
            //        var y = row * (Brick.Height + 2) + offsetY;
            //        var position = new Vector2(x, y);
            //        Brick brick = new Brick(position, hp);
            //        bricks.Add(brick);
            //    }
            //}
            //return bricks;
        //}

        //private static int GetBrickHpBySymbol(char c) => c switch
        //{
        //    '#' => int.MaxValue,
        //    '1' => 1,
        //    '2' => 2,
        //    '3' => 3,
        //    _ => 0,
        //};
    }
}
