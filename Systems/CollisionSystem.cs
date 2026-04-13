using Arkanoid.Core;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Arkanoid.Systems
{
    public static class CollisionSystem
    {
        public static void HandleBallPaddle(Ball ball, Paddle paddle)
        {
            if (Intersects(ball.Bounds, paddle.Bounds)) {
                ball.OnPaddleCollision(paddle);
            }
        }
        public static void HandleBallBrick(Ball ball, List<Brick> bricks)
        {
            for (int i = 0; i < bricks.Count; i++) {
                if (Intersects(ball.Bounds, bricks[i].Bounds)) {
                    ResolveBallBrickCollision(ball, bricks[i]);
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

        private static void ResolveBallBrickCollision(Ball ball, Brick brick)
        {
            var ballRect = ball.Bounds;
            var brickRect = brick.Bounds;

            float overlapLeft = ballRect.Right - brickRect.Left;
            float overlapRight = brickRect.Right - ballRect.Left;
            float overlapTop = ballRect.Bottom - brickRect.Top;
            float overlapBottom = brickRect.Bottom - ballRect.Top;

            //overlapX → насколько “влезли” по горизонтали
            //overlapY → по вертикали
            float minOverlapX = Math.Min(overlapLeft, overlapRight);
            float minOverlapY = Math.Min(overlapTop, overlapBottom);

            if (minOverlapX < minOverlapY) {
                // удар сбоку
                ball.ReflectX();
            }
            else {
                // удар сверху/снизу
                ball.ReflectY();
            }
        }
    }
}
