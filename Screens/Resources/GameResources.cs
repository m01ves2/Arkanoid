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
        public int ScreenWidth;
        public int ScreenHeight;

        public void Load(ContentManager Content, int screenWidth, int screenHeight)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            Font = Content.Load<SpriteFont>("font");
            Music = Content.Load<Song>("background_music");
        }
    }
}
