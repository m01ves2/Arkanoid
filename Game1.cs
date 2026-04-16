using Arkanoid.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Arkanoid;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Song _music;
    private int _screenWidth = 800;
    private int screenHeight = 600;

    ////fps counter
    //private int _frameCount;
    //private double _elapsedTime;
    //private int _fps;

    private Texture2D _whitePixel;
    private Core.GameWorld _gameWorld;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = _screenWidth;
        _graphics.PreferredBackBufferHeight = screenHeight;

        _graphics.ApplyChanges();

        _music = Content.Load<Song>("background_music");

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.1f;
        MediaPlayer.Play(_music);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _gameWorld = new Core.GameWorld(_screenWidth, screenHeight);
        _gameWorld.LoadContent(Content);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });
        _font = Content.Load<SpriteFont>("font");

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _gameWorld.HandleInput(Keyboard.GetState());

        // TODO: Add your update logic here
        //_frameCount++;

        //_elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        //if (_elapsedTime >= 1.0) {
        //    _fps = _frameCount;
        //    _frameCount = 0;
        //    _elapsedTime = 0;
        //}

        _gameWorld.Update(gameTime);

        base.Update(gameTime);
        
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //System.Diagnostics.Debug.WriteLine($"FPS: {_fps}");

        var shakeOffset = _gameWorld.GetShakeOffset();

        _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0)
);

        _gameWorld.Draw(_spriteBatch, _whitePixel, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
