using Arkanoid.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Arkanoid.Core
{
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
            _paddle.Update(gameTime, screenWidth);
            _ball.Update(gameTime, screenWidth, screenHeight, _paddle);
            
            for (int i = _particles.Count - 1; i >= 0; i--) {
                var p = _particles[i];

                _particles[i].Update(gameTime);

                if (p.Life <= 0)
                    _particles.RemoveAt(i);
            }

            CollisionSystem.HandleBallPaddle(_ball, _paddle);

            if (CollisionSystem.HandleBallBrick(_ball, _bricks)) {
                SpawnParticles(_ball.Bounds.Location.ToVector2());
            }

            CollisionSystem.HandleBallWall(_ball, screenWidth, screenHeight);
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
    }
}
