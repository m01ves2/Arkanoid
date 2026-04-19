using Arkanoid.Core;
using Arkanoid.Screens;

namespace Arkanoid.Systems
{
    public enum WallHitType
    {
        Left,
        Right,
        Top
    }

    public class BallWallCollision : ICollision
    {
        public Ball Ball { get; }
        public WallHitType WallHitType {  get; }
        
        public BallWallCollision(Ball ball, WallHitType wallHitType)
        {
            Ball = ball;
            WallHitType = wallHitType;
        }
        public void Resolve(GameWorld world)
        {
            world.HandleWallHit(Ball, WallHitType);
        }
    }
}
