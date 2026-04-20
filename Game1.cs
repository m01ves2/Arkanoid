using Arkanoid.Screens;
using Arkanoid.Screens.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Arkanoid;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _whitePixel; //pixel — это инструмент рисования, создаётся в Game1 с помощью GraphicsDevice. используем для рисования частиц

    private int _screenWidth = 800;
    private int _screenHeight = 600;

    private ScreenManager _screenManager;
    private GameAssets _assets;
    private GameResources _resources;
    private GameSettings _gameSettings;

    ////fps counter
    //private int _frameCount;
    //private double _elapsedTime;
    //private int _fps;

    //private GameWorld _gameWorld;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = _screenWidth;
        _graphics.PreferredBackBufferHeight = _screenHeight;

        _graphics.ApplyChanges();

        _assets = new GameAssets();
        _resources = new GameResources();
        _gameSettings = new GameSettings();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        //_screenManager = new ScreenManager();
        
        //_gameWorld = new GameWorld(_screenWidth, screenHeight);
        //_gameWorld.LoadContent(Content, _whitePixel);

        base.Initialize();
    }

    protected override void LoadContent() //TODO перетащить сюда все загрузки ресурсов
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _whitePixel = new Texture2D(GraphicsDevice, 1, 1);
        _whitePixel.SetData(new[] { Color.White });

        _assets.Load(Content);
        _resources.Load(Content, _whitePixel, _screenWidth, _screenHeight);

        _screenManager = new ScreenManager(new MenuScreen(_resources), _resources, _assets, _gameSettings);
        //_screenManager.SetScreen();
        StartMusic();
    }

    private void StartMusic()
    {
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.1f;
        MediaPlayer.Play(_resources.Music);
    }

    protected override void Update(GameTime gameTime)
    {
        var keyboard = Keyboard.GetState();

        //if (keyboard.IsKeyDown(Keys.Escape))
            //Exit();

        _screenManager.HandleInput(keyboard);
        _screenManager.Update(gameTime);

        if (_screenManager.ShouldExit)
            Exit();

        //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
        //    Exit();

        ////_gameWorld.HandleInput(Keyboard.GetState());

        //// TODO: Add your update logic here
        ////_frameCount++;

        ////_elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        ////if (_elapsedTime >= 1.0) {
        ////    _fps = _frameCount;
        ////    _frameCount = 0;
        ////    _elapsedTime = 0;
        ////}

        ////_gameWorld.Update(gameTime);
        //_screenManager.Update(gameTime);

        base.Update(gameTime);
        
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //System.Diagnostics.Debug.WriteLine($"FPS: {_fps}");

        //var shakeOffset = _gameWorld.GetShakeOffset();
        //_spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0));
        _spriteBatch.Begin();
        //_gameWorld.Draw(_spriteBatch);
        _screenManager.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
