using System.Collections.Generic;

namespace Arkanoid.Core
{
    public class GameContext
    {
        public List<Brick> Bricks;
        public List<Ball> Balls;

        public int Score;
        public int Lives;

        public void AddScore(int value) => Score += value;

        public void RemoveBrick(Brick brick) => Bricks.Remove(brick);
    }
}
