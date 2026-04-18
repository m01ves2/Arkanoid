using Arkanoid.Core;

namespace Arkanoid.Systems
{
    public class PaddleBonusCollision : ICollision
    {
        public Paddle Paddle { get; }
        public Bonus Bonus { get; }

        public PaddleBonusCollision(Paddle paddle, Bonus bonus)
        {
            Paddle = paddle;
            Bonus = bonus;
        }

        public void Resolve(GameWorld world)
        {
            world.HandleBonusPickup(Paddle, Bonus);
        }
    }
}
