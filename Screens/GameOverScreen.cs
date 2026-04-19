using Arkanoid.Screens.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkanoid.Screens
{
    public enum ResultType
    {
        Win,
        Lose
    }

    public class GameOverScreen : Screen
    {
        private ResultType _resultType;
        public GameOverScreen(ResultType resultType, GameResources gameResources) : base(gameResources) 
        { 
            _resultType = resultType;    
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (_resultType == ResultType.Lose) {
            //    DrawCentered(spriteBatch, "GAME OVER!", 250, Color.Red);
            //    DrawCentered(spriteBatch, $"Score: {_score}", 300, Color.White);
            //    DrawCentered(spriteBatch, "Press ENTER to Restart", 350, Color.Gray);
            //    return;
            //}

            //if (_resultType == ResultType.Win) {
            //    DrawCentered(spriteBatch, "YOU WIN!", 250, Color.Green);
            //    DrawCentered(spriteBatch, $"Score: {_score}", 300, Color.White);
            //    DrawCentered(spriteBatch, "Press ENTER to Restart", 350, Color.Gray);
            //    return;
            //}

            //throw new System.NotImplementedException();
        }

        public override void HandleInput(KeyboardState keyboard)
        {
            throw new System.NotImplementedException();
        }

        public override ScreenResult Update(GameTime gameTime)
        {
            throw new System.NotImplementedException();
        }
    }
}
