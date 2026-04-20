using Arkanoid.Screens.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arkanoid.Screens
{
    public class SettingsScreen : Screen
    {
        GameSettings _gameSettings;

        private readonly string[] _levels;
        private int _selectedIndex;

        private ScreenResult _result; 
        private bool isExit = false;

        public SettingsScreen(GameResources gameResources, GameSettings gameSettings ) : base(gameResources) 
        {
            _previous = Keyboard.GetState();
            _gameSettings = gameSettings;
            _levels = Directory.GetFiles("Levels", "*.txt");
        }

        public  override void Draw(SpriteBatch spriteBatch)
        {
            DrawCentered(spriteBatch, "CHOSE LEVEL:", 100, Color.White);

            for (int i = 0; i < _levels.Length; i++) {
                var color = i == _selectedIndex ? Color.Yellow : Color.Gray;
                //spriteBatch.DrawString(_gameResources.Font, Path.GetFileNameWithoutExtension(_levels[i]), new Vector2(100, 100 + i * 30), color);
                DrawCentered(spriteBatch, Path.GetFileNameWithoutExtension(_levels[i]), 200 + i * 50, color);
            }
        }

        public override void HandleInput(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Escape)) {
                isExit = true;
            }

            var current = keyboard;

            if (current.IsKeyDown(Keys.Enter) && _previous.IsKeyUp(Keys.Enter)) {
                _gameSettings.SelectedLevel = _levels[_selectedIndex];
                _result = new ScreenResult(ScreenResultType.Menu);
            }

            if (current.IsKeyDown(Keys.Down) && _previous.IsKeyUp(Keys.Down)) {
                _selectedIndex = Math.Min(_selectedIndex + 1, _levels.Length - 1);
            }

            if (current.IsKeyDown(Keys.Up) && _previous.IsKeyUp(Keys.Up)) {
                _selectedIndex = Math.Max(_selectedIndex - 1, 0);
            }

            _previous = current;
        }

        public override ScreenResult Update(GameTime gameTime)
        {
            if (isExit)
                return new ScreenResult(ScreenResultType.Menu);

            if (_result != null && _result.Type != ScreenResultType.None) {
                var r = _result;
                _result = new ScreenResult(ScreenResultType.None);
                return r;
            }

            return null;
        }
    }
}
