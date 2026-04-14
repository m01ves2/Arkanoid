using Arkanoid.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Arkanoid.Systems
{
    public static class CollisionSystem
    {
        public static bool HandleBallPaddle(Ball ball, Paddle paddle)
        {
            if (Intersects(ball.Bounds, paddle.Bounds)) {
                ball.ResolvePaddleCollision(paddle);
                return true;
            }
            return false;
        }
        public static bool HandleBallBrick(GameWorld gameWorld, Ball ball, List<Brick> bricks)
        {
            for (int i = 0; i < bricks.Count; i++) {
                if (Intersects(ball.Bounds, bricks[i].Bounds)) {
                    ball.ResolveBrickCollision(bricks[i]);
                    bricks.RemoveAt(i);
                    gameWorld.AddScore(10);
                    return true;
                }
            }
            return false;
        }
        public static bool HandleBallWall(Ball ball, int screenWidth, int screenHeight)
        {
            return ball.ResolveWallCollision(screenWidth, screenHeight);
        }

        private static bool Intersects(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }
    }
}
