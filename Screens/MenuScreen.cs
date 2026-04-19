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
            DrawCentered(spriteBatch, "ARKANOID", 200, Color.White);
            //DrawCentered(spriteBatch, "Press ENTER to Start", 300, Color.Gray);

            for(int i = 0; i < menuItems.Length; i++) {
                Color color = ( i == _selectedIndex ) ? Color.YellowGreen : Color.Gray;
                DrawCentered(spriteBatch, menuItems[i], 300 + i * 100, color);
            }
        }

        public override void HandleInput(KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Enter)) {
                _isConfirmed = true;
            }
            else if(keyboard.IsKeyDown(Keys.Up)) {
                _selectedIndex++;
                if (_selectedIndex > menuItems.Length) 
                    _selectedIndex = menuItems.Length - 1;
            }
            else if (keyboard.IsKeyDown(Keys.Up)) {
                _selectedIndex--;
                if(_selectedIndex < 0) 
                    _selectedIndex = 0;
            }
        }

    }
}
