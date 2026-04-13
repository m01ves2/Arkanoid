using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arkanoid.Core
{
    public class Particle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Life;
        public float MaxLife;

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * dt;
            Life -= dt;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            float alpha = Life / MaxLife;

            spriteBatch.Draw( pixel, new Rectangle((int)Position.X, (int)Position.Y, 2, 2),  Color.White * alpha );
        }
    }
}
