using Arkanoid.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Arkanoid;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    
    ////fps counter
    //private int _frameCount;
    //private double _elapsedTime;
    //private int _fps;

    private Texture2D _whitePixel;
    private Core.GameWorld _world;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;

        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _world = new Core.GameWorld();
        _world.LoadContent(Content);

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

        // TODO: Add your update logic here
        //_frameCount++;

        //_elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        //if (_elapsedTime >= 1.0) {
        //    _fps = _frameCount;
        //    _frameCount = 0;
        //    _elapsedTime = 0;
        //}

        _world.Update( gameTime, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height );

        base.Update(gameTime);
        
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        //System.Diagnostics.Debug.WriteLine($"FPS: {_fps}");

        var shakeOffset = _world.GetShakeOffset();

        _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0)
);

        _world.Draw(_spriteBatch, _whitePixel);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
