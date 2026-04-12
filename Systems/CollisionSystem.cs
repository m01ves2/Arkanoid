using Arkanoid.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Arkanoid.Systems
{
    public static class CollisionSystem
    {
        public static void HandleBallPaddle(Ball ball, Paddle paddle)
        {
            if (Intersects(ball.Bounds, paddle.Bounds)) {
                if (Intersects(ball.Bounds, paddle.Bounds)) {
                    ball.OnPaddleCollision(paddle);
                }
            }
        }
        public static void HandleBallBrick(Ball ball, List<Brick> bricks)
        {
            for (int i = 0; i < bricks.Count; i++) {
                if (Intersects(ball.Bounds, bricks[i].Bounds)) {
                    ball.OnBrickCollision();
                    bricks.RemoveAt(i);
                    break;
                }
            }
        }
        public static void HandleBallWall(Ball ball, int screenWidth, int screenHeight)
        {
            ball.OnWallCollision(screenWidth, screenHeight);
        }

        private static bool Intersects(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }
    }
}
