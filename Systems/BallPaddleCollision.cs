using Arkanoid.Core;
using Arkanoid.Screens;

namespace Arkanoid.Systems
{
    public class BallPaddleCollision : ICollision
    {
        public Ball Ball { get; }
        public Paddle Paddle { get; }

        public BallPaddleCollision(Ball ball, Paddle paddle )
        {
            Ball = ball;
            Paddle = paddle;
        }
        public void Resolve(GameWorld world)
        {
            world.HandlePaddleHit(Ball, Paddle);
        }
    }
}
