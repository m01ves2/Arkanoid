using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace Arkanoid.Core
{
    public class GameWorld
    {
        public Paddle Paddle => _paddle;
        private Paddle _paddle;
        private Ball _ball;
        private List<Brick> _bricks = new List<Brick>();

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
            _paddle.Update(gameTime);
            _ball.Update(gameTime, screenWidth, screenHeight, _paddle);

            ProcessCollidingBallWithBricks();
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            _paddle.Draw(spriteBatch, pixel);
            _ball.Draw(spriteBatch, pixel);

            foreach (var brick in _bricks) {
                brick.Draw(spriteBatch, pixel);
            }
        }

        private void ProcessCollidingBallWithBricks()
        {
            var ballRect = _ball.Bounds;

            for (int i = 0; i < _bricks.Count; i++) {
                var brickRect = _bricks[i].Bounds;
                if (ballRect.Intersects(brickRect)) {
                    _bricks.RemoveAt(i);
                    _ball.OnBrickCollision();
                    break;
                }
            }
        }
    }
}
