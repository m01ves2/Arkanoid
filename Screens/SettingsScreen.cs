using Arkanoid.Screens.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Arkanoid.Screens
{
    public class SettingsScreen : Screen
    {
        GameSettings _gameSettings;

        private readonly string[] _levels;
        private int _selectedIndex;
        private bool _confirmed;

        public SettingsScreen(GameResources gameResources, GameSettings gameSettings ) : base(gameResources) 
        {
            _gameSettings = gameSettings;
            _levels = Directory.GetFiles("Levels", "*.txt");
        }

        public  override void Draw(SpriteBatch spriteBatch)
        {
            DrawCentered(spriteBatch, "CHOSE LEVEL:", 200, Color.White);

            for (int i = 0; i < _levels.Length; i++) {
                var color = i == _selectedIndex ? Color.Yellow : Color.White;
                //spriteBatch.DrawString(_gameResources.Font, Path.GetFileNameWithoutExtension(_levels[i]), new Vector2(100, 100 + i * 30), color);
                DrawCentered(spriteBatch, Path.GetFileNameWithoutExtension(_levels[i]), 300 + i * 100, color);
            }
        }

        public override void HandleInput(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Enter)) {
                _confirmed = true;
            }
            else if (keyboard.IsKeyDown(Keys.Up)) {
                _selectedIndex++;
                if (_selectedIndex > _levels.Length)
                    _selectedIndex = _levels.Length - 1;
            }
            else if (keyboard.IsKeyDown(Keys.Up)) {
                _selectedIndex--;
                if (_selectedIndex < 0)
                    _selectedIndex = 0;
            }
        }

        public override ScreenResult Update(GameTime gameTime)
        {
            if (_confirmed) {
                _gameSettings.SelectedLevel = _levels[_selectedIndex];
                //return ScreenResult.Switch(new GameWorld(_gameAssets, _gameResources, _screenWidth, _screenHeight));
                return new ScreenResult(ScreenResultType.)
            }

            return null;
        }
    }
}
