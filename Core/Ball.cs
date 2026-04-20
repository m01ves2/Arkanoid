using Arkanoid.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Arkanoid.Core
{
    public class Ball
    {
        public const int Size = 16;
        private Vector2 _position;

        //public Vector2 Position => _position;
        private Vector2 _velocity;
        private float _speed = 300f;
        private float _baseSpeed = 300f;
        private const float _maxSpeed = 800f;
        //private const float _acceleration = 8f;
        private const float SpeedMultiplier = 1.03f;
        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, Size, Size);
        private Vector2 _previousPosition;

        private bool _isPiercing = false;
        public bool IsPiercing => _isPiercing;

        public Ball(Vector2 startPosition)
        {
            _position = startPosition;

            // стартовая скорость
            var direction = new Vector2(0, -1); //летим строго вверх
            _velocity = Vector2.Normalize(direction)*_speed;
        }

        public void Update(GameTime gameTime)
        {
            _previousPosition = _position;
            
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _position += _velocity * dt;

            //еще один вариант ускорения шарика (тут с течением времени)
            //_speed = MathF.Min(_speed + _acceleration * dt, _maxSpeed);
            //_velocity = Vector2.Normalize(_velocity) * _speed;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, new Rectangle((int)_position.X, (int)_position.Y, Size, Size), Color.White);
        }

        public void ResolvePaddleCollision(Paddle paddle)
        {
            _position = _previousPosition;

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

            NormalizeVelocity();
            IncreaseSpeed();
        }

        public void ResolveWallCollision(WallHitType wallHitType)
        {
            switch (wallHitType) {
                //отскок от стен
                case WallHitType.Left: //левая
                case WallHitType.Right: //правая
                    _position = _previousPosition;
                    ReflectX();
                    break;

                case WallHitType.Top:
                    _position = _previousPosition;
                    _position.Y = 0;
                    ReflectY();
                    break;
            }

            NormalizeVelocity();
        }

        public void ResolveBrickCollision(Brick brick)
        {
            if (_isPiercing) return;

            _position = _previousPosition;

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
                ReflectX();
            }
            else {
                // удар сверху/снизу
                ReflectY();
            }

            NormalizeVelocity();
            IncreaseSpeed();
        }

        public void ReflectX()
        {
            _velocity.X = -_velocity.X;

        }

        public void ReflectY()
        {
            _velocity.Y = -_velocity.Y;
        }

        public void IncreaseSpeed()
        {
            _speed *= SpeedMultiplier;
            _speed = MathF.Min(_speed, _maxSpeed);
        }

        public void SetPiercing(bool value)
        {
            _isPiercing = value;

            if (_isPiercing) {
                _speed = _maxSpeed;
            }
            else {
                _speed = _baseSpeed;
            }
        }

        public void ResetSpeed()
        {
            _speed = _baseSpeed;
        }

        private void NormalizeVelocity()
        {
            _velocity = Vector2.Normalize(_velocity);

            if (_velocity == Vector2.Zero)
                _velocity = new Vector2(0, -1);

            _velocity.Y = Math.Sign(_velocity.Y) * Math.Max(Math.Abs(_velocity.Y), 0.3f);
            _velocity *= _speed;
        }
    }
}
