using Arkanoid.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Arkanoid.Core
{
    public enum GameState
    {
        Playing,
        GameOver
    }

    public class GameWorld
    {
        public Paddle Paddle => _paddle;
        private Paddle _paddle;
        private Ball _ball;
        private List<Brick> _bricks = new List<Brick>();
        private List<Particle> _particles = new();

        const int BrickWidth = 80;
        const int BrickHeight = 20;
        const int OffsetX = 1;
        const int OffsetY = 40;

        //screen shaking
        private float _shakeTime;
        private float _shakeStrength;

        private GameState _state = GameState.Playing;
        private int _score;

        private SpriteFont _font;

        public GameWorld()
        {
            _paddle = new Paddle(new Vector2(400, 500));
            _ball = new Ball(new Vector2(400, 300));

            for (int y = 0; y < 5; y++) {
                for(int x = 0; x < 10; x++) {
                    _bricks.Add(new Brick(new Vector2(x * 80 + 1, y * 20 + 40)));
                }
            }
        }

        public void Update(GameTime gameTime, int screenWidth, int screenHeight)
        {
            CheckGameOver(screenHeight);

            _paddle.Update(gameTime, screenWidth);
            _ball.Update(gameTime, screenWidth, screenHeight, _paddle);
            
            for (int i = _particles.Count - 1; i >= 0; i--) {
                var p = _particles[i];

                _particles[i].Update(gameTime);

                if (p.Life <= 0)
                    _particles.RemoveAt(i);
            }

            if (CollisionSystem.HandleBallPaddle(_ball, _paddle)) {
                //AddShake(0.5f);
                SpawnParticles(_ball.Bounds.Location.ToVector2());
            }

            if (CollisionSystem.HandleBallBrick(this, _ball, _bricks)) {
                AddShake(0.2f);
                SpawnParticles(_ball.Bounds.Location.ToVector2());
            }

            if (CollisionSystem.HandleBallWall(_ball, screenWidth, screenHeight)) {
                //AddShake(0.2f);
            }

            //screen shaking
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_shakeTime > 0) {
                _shakeTime -= dt;
                if (_shakeTime <= 0)
                    _shakeStrength = 0;
            }

        }
        

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            _paddle.Draw(spriteBatch, pixel);
            _ball.Draw(spriteBatch, pixel);

            foreach (var brick in _bricks) {
                brick.Draw(spriteBatch, pixel);
            }

            foreach(var particle in _particles) {
                particle.Draw(spriteBatch, pixel);
            }


            spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(10, 10), Color.White);

            if (_state == GameState.GameOver) {
                spriteBatch.DrawString(_font, "GAME OVER", new Vector2(300, 250), Color.Red);
            }
        }


        private void SpawnParticles(Vector2 position)
        {
            for (int i = 0; i < 10; i++) {
                float angle = MathHelper.ToRadians(Random.Shared.Next(0, 360));
                float speed = Random.Shared.Next(50, 200);

                var velocity = new Vector2(
                    MathF.Cos(angle),
                    MathF.Sin(angle)
                ) * speed;

                _particles.Add(new Particle
                {
                    Position = position,
                    Velocity = velocity,
                    Life = 0.5f,
                    MaxLife = 0.5f
                });
            }
        }

        public void AddShake(float strength)
        {
            _shakeStrength = MathF.Max(_shakeStrength, strength);
            _shakeTime = 0.2f;
        }

        public Vector2 GetShakeOffset()
        {
            if (_shakeTime <= 0)
                return Vector2.Zero;

            return new Vector2( Random.Shared.Next(-5, 6), Random.Shared.Next(-5, 6) ) * _shakeStrength;
        }

        public void AddScore(int value)
        {
            _score += value;
        }

        public void CheckGameOver(int screenHeight)
        {
            if (_ball.Position.Y > screenHeight) {
                _state = GameState.GameOver;
            }
        }

        public void Restart()
        {
            _state = GameState.Playing;
            _score = 0;

            _ball = new Ball(new Vector2(400, 300));
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("font");
        }
    }


}
