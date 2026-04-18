using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Arkanoid.Core
{
    public enum BonusType
    {
        ExpandPaddle,
        ShrinkPaddle,
        MultiBall,
        PiercingBall,
        SlowBall,
    }
    public class Bonus
    {
        private Vector2 _position;

        public const int Width = 48;
        public const int Height = 38;

        public BonusType BonusType { get; init; }
        private Vector2 _velocity;
        private float _speed = 100f;

        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, Width, Height);

        public Bonus(Vector2 position, BonusType type)
        {
            _position = position;
            BonusType = type;


            // стартовая скорость
            var direction = new Vector2(0, 1); //летим строго вниз
            _velocity = Vector2.Normalize(direction) * _speed;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _velocity * dt;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var rect = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}
