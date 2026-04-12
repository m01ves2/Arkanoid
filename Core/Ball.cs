using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arkanoid.Core
{
    public class Ball
    {
        private Vector2 _position;
        private Vector2 _velocity;
        private const int Size = 16;
        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, Size, Size);


        public Ball(Vector2 startPosition)
        {
            _position = startPosition;

            // стартовая скорость
            _velocity = new Vector2(200f, -200f);
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight, Paddle paddle)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _position += _velocity * dt;

            // отскок от стен

            // левая / правая
            if (_position.X <= 0 || _position.X + Size >= screenWidth) {
                _velocity.X *= -1;
            }

            // верх
            if (_position.Y <= 0) {
                _velocity.Y *= -1;
            }

            // низ (пока просто отскок)
            if (_position.Y + Size >= screenHeight) {
                _velocity.Y *= -1;
            }

            if (IsCollidingWithPaddle(paddle)) {
                var paddleRect = paddle.Bounds;

                float paddleCenter = paddleRect.X + paddleRect.Width / 2f;
                float ballCenter = _position.X + Size / 2f;

                float offset = ballCenter - paddleCenter;
                float normalized = offset / (paddleRect.Width / 2f);

                float maxAngleX = 300f;

                _velocity.X = normalized * maxAngleX;
                _velocity.Y = -MathF.Abs(_velocity.Y);


                // чуть “раздвигаем” шар, чтобы не залипал
                _position.Y = paddle.Bounds.Y - Size;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            var rect = new Rectangle((int)_position.X, (int)_position.Y, Size, Size);

            spriteBatch.Draw(pixel, rect, Color.Yellow);
        }

        private bool IsCollidingWithPaddle(Paddle paddle)
        {
            var ballRect = this.Bounds;

            var paddleRect = paddle.Bounds;

            return ballRect.Intersects(paddleRect);
        }

        public void OnBrickCollision()
        {
            _velocity.Y *= -1;
        }
    }
}
