using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arkanoid.Core
{
    public class Particle
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private float _life;
        private float _maxLife;
        public float Life => _life;

        public Particle(Vector2 position, Vector2 velocity, float life, float maxLife)
        {
            _position = position;
            _velocity = velocity;
            _life = life;
            _maxLife = maxLife;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _velocity * dt;
            _life -= dt;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            float alpha = _life / _maxLife;

            spriteBatch.Draw( pixel, new Rectangle((int)_position.X, (int)_position.Y, 2, 2), Color.Yellow * alpha );
        }
    }
}
