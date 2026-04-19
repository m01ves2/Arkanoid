using Arkanoid.Core;
using Arkanoid.Screens;

namespace Arkanoid.Systems
{
    public class BallBrickCollision : ICollision
    {
        public Ball Ball { get;}
        public Brick Brick {  get;}

        public BallBrickCollision(Ball ball, Brick brick)
        {
            Ball = ball;
            Brick = brick;
        }

        public void Resolve(GameWorld world)
        {
            world.HandleBrickHit(Ball, Brick);
        }
    }
}
