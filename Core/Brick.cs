using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO.Pipelines;

namespace Arkanoid.Core
{
    public class Brick
    {
        private Vector2 _position;
        private const int Width = 78;
        private const int Height = 18;
        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, Width, Height);

        public Brick(Vector2 position)
        {
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
        {
            var rect = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);

            spriteBatch.Draw(pixel, rect, Color.Red);
        }
    }
}
