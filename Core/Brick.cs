using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO.Pipelines;

namespace Arkanoid.Core
{
    public enum BrickType
    {
        None,
        Fragile,
        Normal,
        Strong,
        Unbreakable
    }

    public class Brick
    {
        private Vector2 _position;
        public const int Width = 78;
        public const int Height = 18;
        public BrickType _brickType { get; }
        int _hp = 3;
        public int Hp => _hp;
        public Rectangle Bounds => new Rectangle((int)_position.X, (int)_position.Y, Width, Height);

        public Brick(Vector2 position, BrickType brickType = BrickType.Normal)
        {
            _position = position;
            _brickType = brickType;

            switch (brickType) {
                case BrickType.Fragile: _hp = 1; break;
                case BrickType.Normal: _hp = 2; break;
                case BrickType.Strong: _hp = 3; break;
                case BrickType.Unbreakable: _hp = -1; break;
                default: break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var rect = new Rectangle((int)_position.X, (int)_position.Y, Width, Height);
            Color color = _hp == 1 ? Color.LightGreen : _hp == 2 ? Color.Yellow : _hp == 3 ? Color.Red : Color.DarkGray;
            spriteBatch.Draw(texture, rect, color);
        }

        public void LoseHp()
        {
            if (_brickType != BrickType.Unbreakable && _hp > 0)
                _hp--;
        }

        public void Destroy()
        {
            if (_brickType != BrickType.Unbreakable && _hp > 0)
                _hp = 0;
        }
    }
}
