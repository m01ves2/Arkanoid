using Arkanoid.Screens.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkanoid.Screens
{
    public class MenuScreen : Screen
    {
        private string[] menuItems = new string[] { 
            "New Game",
            "Settings",
            "Exit"
        };

        private int _selectedIndex = 0;
        private bool _isConfirmed = false;

        public MenuScreen(GameResources gameResources) : base(gameResources) 
        {
            _previous = Keyboard.GetState();
        }
        public override ScreenResult Update(GameTime gameTime)
        {
            if (_isConfirmed) {
                switch (_selectedIndex) {
                    case 0: return new ScreenResult(ScreenResultType.StartGame);
                    case 1: return new ScreenResult(ScreenResultType.OpenSettings);
                    case 2: return new ScreenResult(ScreenResultType.Exit);
                }
            }

            return new ScreenResult(ScreenResultType.None);
        }

        public override void Draw(SpriteBatch spriteBatch) //TODO
        {
            DrawCentered(spriteBatch, "ARKANOID", 100, Color.White);

            for(int i = 0; i < menuItems.Length; i++) {
                Color color = ( i == _selectedIndex ) ? Color.Yellow : Color.Gray;
                DrawCentered(spriteBatch, menuItems[i], 200 + i * 50, color);
            }
        }

        public override void HandleInput(KeyboardState keyboard)
        {
            var current = keyboard;

            if (current.IsKeyDown(Keys.Enter) && _previous.IsKeyUp(Keys.Enter)) {
                _isConfirmed = true;
            }
            else if(current.IsKeyDown(Keys.Down) && _previous.IsKeyUp(Keys.Down)) {
                _selectedIndex++;
                if (_selectedIndex > menuItems.Length - 1) 
                    _selectedIndex = menuItems.Length - 1;
            }
            else if (current.IsKeyDown(Keys.Up) && _previous.IsKeyUp(Keys.Up)) {
                _selectedIndex--;
                if(_selectedIndex < 0) 
                    _selectedIndex = 0;
            }

            _previous = current;
        }

    }
}
