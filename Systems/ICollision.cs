using Arkanoid.Core;

namespace Arkanoid.Systems
{
    public interface ICollision
    {
        void Resolve(GameWorld world);
    }
}
