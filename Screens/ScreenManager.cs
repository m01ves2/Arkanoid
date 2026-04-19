using Arkanoid.Screens.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime;

namespace Arkanoid.Screens
{
    public class ScreenManager
    {

        private GameAssets _assets;
        private GameResources _resources;
        private GameSettings _settings;

        private Screen _currentScreen;
        private bool _shouldExit = false;
        public bool ShouldExit => _shouldExit;

        public ScreenManager(Screen startScreen, GameResources resources, GameSettings settings)
        {
            _currentScreen = startScreen;
            _resources = resources;
            _settings = settings;
        }

        public void Update(GameTime gameTime)
        {
            var result = _currentScreen.Update(gameTime);

            if (result != null)
                HandleResult(result);
        }

        public void HandleResult(ScreenResult result)
        {
            switch (result.Type) {
                case ScreenResultType.StartGame:
                    _currentScreen = new GameWorld(_resources, _assets, _settings);
                    break;

                case ScreenResultType.OpenSettings:
                    _currentScreen = new SettingsScreen(_resources, _settings);
                    break;

                case ScreenResultType.Menu:
                    _currentScreen = new MenuScreen(_resources);
                    break;

                case ScreenResultType.Exit:
                    _shouldExit = true;
                    break;
            }
        }

        public void HandleInput(KeyboardState keyboard)
        {
            _currentScreen.HandleInput(keyboard);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentScreen.Draw(spriteBatch);
        }
    }
}
