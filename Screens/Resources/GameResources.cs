using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Arkanoid.Screens.Resources
{
    public class GameResources
    {
        public SpriteFont Font;
        public Song Music;
        public Texture2D _whitePixel;
        public int ScreenWidth;
        public int ScreenHeight;

        public void Load(ContentManager Content, Texture2D whitePixel, int screenWidth, int screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            _whitePixel = whitePixel;
            Font = Content.Load<SpriteFont>("font");
            Music = Content.Load<Song>("background_music");
        }
    }
}
