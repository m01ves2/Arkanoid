using Arkanoid.Core;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Arkanoid.Systems
{
    public static class CollisionSystem
    {
        public static List<BallWallCollision> DetectBallWallCollision(List<Ball> balls, int screenWidth, int screenHeight)
        {
            var result = new List<BallWallCollision>();
            foreach (var ball in balls) {
                // отскок от стен
                // левая
                if (ball.Bounds.X <= 0) {
                    result.Add(new BallWallCollision(ball, WallHitType.Left));
                }

                //правая
                if (ball.Bounds.X + Ball.Size >= screenWidth) {
                    result.Add(new BallWallCollision(ball, WallHitType.Right));
                }

                // верх
                if (ball.Bounds.Y < 0) {
                    result.Add(new BallWallCollision(ball, WallHitType.Top));
                }
            }
            return result;
        }

        public static List<BallBrickCollision> DetectBallBrickCollision(List<Ball> balls, List<Brick> bricks)
        {
            var result = new List<BallBrickCollision>();

            foreach (var ball in balls) {
                foreach (var brick in bricks) {
                    if (Intersects(ball.Bounds, brick.Bounds)) {
                        result.Add(new BallBrickCollision(ball, brick));
                        break; // один кирпич на шар
                    }
                }
            }

            return result;
        }

        public static List<BallPaddleCollision> DetectBallPaddleCollision(List<Ball> balls, Paddle paddle)
        {
            var result = new List<BallPaddleCollision>();

            foreach (var ball in balls) {
                if (Intersects(ball.Bounds, paddle.Bounds)) {
                    result.Add(new BallPaddleCollision(ball, paddle));
                }
            }
            return result;
        }

        public static List<PaddleBonusCollision> DetectPaddleBonusCollision(Paddle paddle, List<Bonus> bonuses)
        {
            var result = new List<PaddleBonusCollision>();

            foreach (var bonus in bonuses) {
                if (Intersects(paddle.Bounds, bonus.Bounds)) {
                    result.Add(new PaddleBonusCollision(paddle, bonus));
                }

            }
            return result;
        }

        private static bool Intersects(Rectangle a, Rectangle b)
        {
            return a.Intersects(b);
        }
    }
}
