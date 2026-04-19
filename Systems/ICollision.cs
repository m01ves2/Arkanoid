using Arkanoid.Screens;

namespace Arkanoid.Systems
{
    public interface ICollision
    {
        void Resolve(GameWorld world);
    }
}
