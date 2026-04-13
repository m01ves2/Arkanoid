using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;

namespace Arkanoid.Core
{
    public class Ball
    {
        private int _angle;
        private Vector2 _position;
        private Vector2 _velocity;
        private const float _speed = 400f;
        private const int Size = 16;
        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, Size, Size);


        public Ball(Vector2 startPosition)
        {
            _position = startPosition;

            // стартовая скорость
            var direction = new Vector2(0, -1); //летим строго вверх
            _velocity = Vector2.Normalize(direction)*_speed;
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight, Paddle paddle)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _velocity * dt;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            var rect = new Rectangle((int)_position.X, (int)_position.Y, Size, Size);
            spriteBatch.Draw(pixel, rect, Color.Yellow);
        }
        public void ReflectX()
        {
            _velocity.X = -_velocity.X;
        }

        public void ReflectY()
        {
            _velocity.Y = -_velocity.Y;
        }

        public void OnPaddleCollision(Paddle paddle)
        {
            var paddleRect = paddle.Bounds;

            float paddleCenter = paddleRect.X + paddleRect.Width / 2f;
            float ballCenter = _position.X + Size / 2f;

            float offset = ballCenter - paddleCenter;
            float normalized = offset / (paddleRect.Width / 2f);

            //модель через X, Y
            //normalized = Math.Clamp(normalized, -1f, 1f); //ограничения угла
            //Vector2 direction = new Vector2(normalized, -0.75f);
            //direction = Vector2.Normalize(direction);
            //_velocity = direction * _speed;

            //через углы
            float maxAngle = 75f;

            float angle = normalized * maxAngle;
            float rad = MathHelper.ToRadians(angle);
            Vector2 direction = new Vector2( MathF.Sin(rad), -MathF.Cos(rad));
            _velocity = direction * _speed;


            // чуть “раздвигаем” шар, чтобы не залипал
            _position.Y = paddle.Bounds.Y - Size;
        }

        public void OnWallCollision(int screenWidth, int screenHeight)
        {
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
        }
    }
}
