using Arkanoid.Screens.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkanoid.Screens
{
    public abstract class Screen
    {
        protected GameResources _gameResources;
        protected int _screenWidth;
        protected int _screenHeight;
        public abstract ScreenResult Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void HandleInput(KeyboardState keyboard);
        protected KeyboardState _previous;

        public Screen(GameResources gameResources)
        {
            _gameResources = gameResources;
            _screenWidth = gameResources.ScreenWidth;
            _screenHeight = gameResources.ScreenHeight;
        }

        protected void DrawCentered(SpriteBatch spriteBatch, string text, float y, Color color)
        {
            var size = _gameResources.Font.MeasureString(text);
            float x = _screenWidth / 2f - size.X / 2f;

            spriteBatch.DrawString(_gameResources.Font, text, new Vector2(x, y), color);
        }
    }
}
