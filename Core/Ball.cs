using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;

namespace Arkanoid.Core
{
    public class Ball
    {
        private const int Size = 16;
        private int _angle;
        private Vector2 _position;
        public Vector2 Position => _position;
        private Vector2 _velocity;
        private float _speed = 300f;
        private const float _maxSpeed = 800f;
        private const float _acceleration = 8f;
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

            //еще один вариант ускорения шарика (тут с течением времени)
            //_speed = MathF.Min(_speed + _acceleration * dt, _maxSpeed);
            //_velocity = Vector2.Normalize(_velocity) * _speed;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            var rect = new Rectangle((int)_position.X, (int)_position.Y, Size, Size);
            spriteBatch.Draw(pixel, rect, Color.Yellow);
        }

        public void ResolvePaddleCollision(Paddle paddle)
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

            IncreaseSpeed();
        }

        public bool ResolveWallCollision(int screenWidth, int screenHeight)
        {
            // отскок от стен
            // левая / правая
            if (_position.X <= 0 || _position.X + Size >= screenWidth) {
                ReflectX();
                return true;
            }

            // верх
            if (_position.Y <= 0) {
                ReflectY();
                return true;
            }

            // низ (пока просто отскок)
            //if (_position.Y + Size >= screenHeight) {
            //    ReflectY();
            //    return true;
            //}

            return false;
        }

        public void ResolveBrickCollision(Brick brick)
        {
            var ballRect = this.Bounds;
            var brickRect = brick.Bounds;

            float overlapLeft = ballRect.Right - brickRect.Left;
            float overlapRight = brickRect.Right - ballRect.Left;
            float overlapTop = ballRect.Bottom - brickRect.Top;
            float overlapBottom = brickRect.Bottom - ballRect.Top;

            //overlapX → насколько “влезли” по горизонтали
            //overlapY → по вертикали
            float minOverlapX = Math.Min(overlapLeft, overlapRight);
            float minOverlapY = Math.Min(overlapTop, overlapBottom);

            if (minOverlapX < minOverlapY) {
                // удар сбоку
                this.ReflectX();
            }
            else {
                // удар сверху/снизу
                this.ReflectY();
            }

            IncreaseSpeed();
        }

        public void ReflectX()
        {
            _velocity.X = -_velocity.X;
            NormalizeVelocity();
        }

        public void ReflectY()
        {
            _velocity.Y = -_velocity.Y;
            NormalizeVelocity();
        }
        private void IncreaseSpeed()
        {
            _speed *= 1.03f;
            _speed = MathF.Min(_speed, _maxSpeed);
        }

        private void NormalizeVelocity()
        {
            _velocity = Vector2.Normalize(_velocity) * _speed;
        }
    }
}
