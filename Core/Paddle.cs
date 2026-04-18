using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkanoid.Core
{
    public class Paddle
    {
        private Vector2 _position;

        public const int Width = 120;
        public const int Height = 20;
        private const float Speed = 800f;
        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
        public Paddle(Vector2 startPosition)
        {
            _position = startPosition;
        }

        public void Update(GameTime gameTime, int screenWidth)
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Left))
                _position.X -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboard.IsKeyDown(Keys.Right))
                _position.X += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _position.X = MathHelper.Clamp(_position.X, 0, screenWidth - Width);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            //var rect = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);

            //spriteBatch.Draw(pixel, rect, Color.White);
            //spriteBatch.Draw(texture, _position, Color.White);
            spriteBatch.Draw(texture, new Rectangle((int)_position.X, (int)_position.Y, Width, Height), Color.White);
        }

    }
}
