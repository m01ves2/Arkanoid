using Arkanoid.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Arkanoid.Core
{
    public enum GameState
    {
        Playing,
        GameOver,
        Menu,
        Win,
    }

    public class GameWorld
    {
        public Paddle Paddle => _paddle;
        private Paddle _paddle;
        private List<Ball> _balls = new List<Ball>();
        private List<Brick> _bricks = new List<Brick>();
        private List<Particle> _particles = new();

        //const int BrickWidth = 78;
        //const int BrickHeight = 18;
        const int Spacing = 2;
        const int OffsetX = 1;
        const int OffsetY = 40;

        //screen shaking
        private float _shakeTime;
        private float _shakeStrength;

        private GameState _state = GameState.Menu;
        private int _score;
        private int _lives = 3;

        private SpriteFont _font;

        private Texture2D _ballTexture;
        private Texture2D _paddleTexture;
        private Texture2D _brickTexture;
        private Texture2D _background;

        private SoundEffect _hitSound;
        private SoundEffect _brickSound;
        private SoundEffect _loseSound;
        //private Song _music;

        private int _screenWidth;
        private int _screenHeight;

        public GameWorld(int screenWidth, int screenHeight) //todo screenWidth, screenHeight
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _paddle = new Paddle(new Vector2(400, 500));
            var ball = new Ball(new Vector2(400, 300));
            _balls.Add(ball);

            //for (int y = 0; y < 5; y++) {
            //    for(int x = 0; x < 10; x++) {
            //        _bricks.Add(new Brick(new Vector2(x * 80 + 1, y * 20 + 40)));
            //    }
            //}
            //var path = Path.Combine(AppContext.BaseDirectory, "Levels", "level.txt");
            //char[,] brickChars = LevelLoader.Load(path);
            //MakeBricks(brickChars, screenWidth, screenHeight);

        }

        public void MakeBricks(char[,] brickChars, int screenWidth, int screenHeight)
        {

            int bricksInScreenWidth = screenWidth / Brick.Width;
            int bricksInScreenHeight = (int)(screenHeight * 0.3 / Brick.Height);

            var maxHeight = Math.Min(bricksInScreenHeight, brickChars.GetLength(0));
            var maxWidth = Math.Min(bricksInScreenWidth, brickChars.GetLength(1));

            for (int row = 0; row < maxHeight; row++) {
                for(int col = 0; col < maxWidth; col++) {
                    var brickType = GetBrickBySymbol(brickChars[row, col]);
                    if(brickType == BrickType.None) continue;

                    var x = col * (Brick.Width + Spacing) + OffsetX;
                    var y = row * (Brick.Height + Spacing) + OffsetY;
                    var position = new Vector2(x, y);
                    Brick brick = new Brick(position, brickType);
                    _bricks.Add(brick);
                }
            }
        }

        private static BrickType GetBrickBySymbol(char c) => c switch
        {
            '#' => BrickType.Unbreakable,
            '1' => BrickType.Fragile,
            '2' => BrickType.Normal,
            '3' => BrickType.Strong,
            ' ' => BrickType.None,
            '.' => BrickType.None,
             _  => BrickType.None
        };

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("font");

            _ballTexture = content.Load<Texture2D>("ball");
            _paddleTexture = content.Load<Texture2D>("paddle");
            _brickTexture = content.Load<Texture2D>("brick");

            _hitSound = content.Load<SoundEffect>("hitSound");
            _brickSound = content.Load<SoundEffect>("brickSound");
            _loseSound = content.Load<SoundEffect>("loseSound");
            _background = content.Load<Texture2D>("background");
        }

        public void Update(GameTime gameTime)
        {
            if (_state != GameState.Playing)
                return;
            CheckGameOver();

            UpdateSimulation(gameTime);

            HandleCollisions();

            UpdateGameState();

            UpdateEffects(gameTime);
        }

        public void UpdateSimulation(GameTime gameTime)
        {

            _paddle.Update(gameTime, _screenWidth);

            foreach (var ball in _balls)
                ball.Update(gameTime);

            UpdateParticles(gameTime);
        }
        private void UpdateParticles(GameTime gameTime)
        {
            for (int i = _particles.Count - 1; i >= 0; i--) {
                _particles[i].Update(gameTime);

                if (_particles[i].Life <= 0)
                    _particles.RemoveAt(i);
            }
        }
        private void HandleCollisions()
        {
            var collisions = new List<ICollision>();
            collisions.AddRange(CollisionSystem.DetectBallBrickCollision(_balls, _bricks));
            collisions.AddRange(CollisionSystem.DetectBallPaddleCollision(_balls, _paddle));
            collisions.AddRange(CollisionSystem.DetectBallWallCollision(_balls, _screenWidth, _screenHeight));

            foreach (var c in collisions)
                c.Resolve(this);
        }
        private void UpdateGameState()
        {
            CheckGameOver();
            CheckWin();
        }
        private void UpdateEffects(GameTime gameTime)
        {
            UpdateShake(gameTime);
        }
        private void UpdateShake(GameTime gameTime)
        {
            if (_shakeTime <= 0)
                return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _shakeTime -= dt;

            if (_shakeTime <= 0)
                _shakeStrength = 0;
        }


        public void HandleBrickHit(Ball ball, Brick brick)
        {
            AddShake(0.2f);

            float randomPitch = Random.Shared.NextSingle() * 0.2f - 0.1f;
            _brickSound.Play(volume: 0.2f, pitch: randomPitch, pan: 0f);

            ball.ResolveBrickCollision(brick);

            brick.LoseHp();

            if (brick.Hp == 0)
                _bricks.Remove(brick);

            if (brick._brickType != BrickType.Unbreakable)
                AddScore(10);

            SpawnParticles(ball.Bounds.Location.ToVector2());

            if (Random.Shared.NextDouble() < 0.1)
                SpawnExtraBall();
        }
        public void HandlePaddleHit(Ball ball, Paddle paddle)
        {
            float randomPitch = Random.Shared.NextSingle() * 0.2f - 0.1f;
            _hitSound.Play(volume: 0.2f, pitch: randomPitch, pan: 0f);

            ball.ResolvePaddleCollision(paddle);
            SpawnParticles(ball.Bounds.Location.ToVector2());
        }
        public void HandleWallHit(Ball ball, WallHitType wallHitType)
        {
            // обычно без эффектов или минимально
            ball.ResolveWallCollision(wallHitType);
        }
        private void SpawnExtraBall()
        {
            var ball = new Ball(new Vector2(_screenWidth / 2f, _screenHeight / 2f));
            _balls.Add(ball);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel, int screenWidth, int screenHeight)
        {
            spriteBatch.Draw(_background, new Rectangle(0, 0, screenWidth, screenHeight), Color.White * 0.8f);

            if (_state == GameState.Menu) {
                DrawCentered(spriteBatch, "ARKANOID", 200, Color.White);
                DrawCentered(spriteBatch, "Press ENTER to Start", 300, Color.Gray);
                return;
            }

            if (_state == GameState.GameOver) {
                DrawCentered(spriteBatch, "GAME OVER!", 250, Color.Red);
                DrawCentered(spriteBatch, $"Score: {_score}", 300, Color.White);
                DrawCentered(spriteBatch, "Press ENTER to Restart", 350, Color.Gray);
                return;
            }

            if (_state == GameState.Win) {
                DrawCentered(spriteBatch, "YOU WIN!", 250, Color.Green);
                DrawCentered(spriteBatch, $"Score: {_score}", 300, Color.White);
                DrawCentered(spriteBatch, "Press ENTER to Restart", 350, Color.Gray);
                return;
            }


            _paddle.Draw(spriteBatch, _paddleTexture);

            foreach (var ball in _balls) {
                ball.Draw(spriteBatch, _ballTexture);
            }

            foreach (var brick in _bricks) {
                brick.Draw(spriteBatch, _brickTexture);
            }

            foreach(var particle in _particles) {
                particle.Draw(spriteBatch, pixel);
            }

             
            //if (_state == GameState.GameOver) {
            //    spriteBatch.DrawString(_font, "GAME OVER", new Vector2(300, 250), Color.Red);
            //}

            spriteBatch.DrawString(_font, $"Score: {_score}  Lives: {_lives}", new Vector2(10, 10), Color.White);
        }


        private void SpawnParticles(Vector2 position)
        {
            for (int i = 0; i < 10; i++) {
                float angle = MathHelper.ToRadians(Random.Shared.Next(0, 360));
                float speed = Random.Shared.Next(50, 200);

                var velocity = new Vector2(
                    MathF.Cos(angle),
                    MathF.Sin(angle)
                ) * speed;

                _particles.Add(new Particle
                {
                    Position = position,
                    Velocity = velocity,
                    Life = 0.5f,
                    MaxLife = 0.5f
                });
            }
        }

        public void AddShake(float strength)
        {
            _shakeStrength = MathF.Max(_shakeStrength, strength);
            _shakeTime = 0.2f;
        }

        private void StopShake()
        {
            _shakeTime = 0;
            _shakeStrength = 0;
        }

        public Vector2 GetShakeOffset()
        {
            if (_shakeTime <= 0)
                return Vector2.Zero;

            return new Vector2( Random.Shared.Next(-5, 6), Random.Shared.Next(-5, 6) ) * _shakeStrength;
        }

        public void AddScore(int value)
        {
            _score += value;
        }

        public void CheckGameOver()
        {
            if (_state == GameState.GameOver) return;

            for (int i = _balls.Count - 1; i >= 0; i--) { 
                if (_balls[i].Position.Y > _screenHeight) {
                    _balls.RemoveAt(i);
                }
            }

            if (_balls.Count == 0) {
               LoseLife();
            } 
        }

        public void CheckWin()
        {
            if (_state != GameState.Playing) return;

            if (_bricks.Where(b => b._brickType != BrickType.Unbreakable).Any()) return;

            _state = GameState.Win;
        }

        private void LoseLife()
        {
            _loseSound.Play();
            AddShake(0.3f);
            _lives--;

            if (_lives <= 0) {
                _state = GameState.GameOver;
                StopShake();
                return;
            }

            ResetBall();
        }

        private void ResetBall()
        {
            var ball = new Ball(new Vector2(_screenWidth / 2f, _screenHeight / 2f));
            _balls.Clear();
            _balls.Add(ball);
        }

        public void HandleInput(KeyboardState keyboard)
        {
            if (_state == GameState.Menu) {
                if (keyboard.IsKeyDown(Keys.Enter)) {
                    StartGame();
                }
            }
            else if (_state == GameState.GameOver) {
                if (keyboard.IsKeyDown(Keys.Enter)) {
                    Restart();
                }
            }
            else if (_state == GameState.Win) { //вынес отдельно, потому что, возможно, будут доп действия
                if (keyboard.IsKeyDown(Keys.Enter)) {
                    Restart();
                }
            }
        }

        private void StartGame()
        {
            _score = 0;
            _lives = 3;
            _state = GameState.Playing;

            var path = Path.Combine(AppContext.BaseDirectory, "Levels", "level.txt");
            char[,] brickChars = LevelLoader.Load(path);
            MakeBricks(brickChars, _screenWidth, _screenHeight);

            ResetBall(); // временно, потом уберём хардкод
        }

        public void Restart()
        {
            StartGame();
        }

        private void DrawCentered(SpriteBatch spriteBatch, string text, float y, Color color)
        {
            var size = _font.MeasureString(text);
            float x = _screenWidth / 2f - size.X / 2f;

            spriteBatch.DrawString(_font, text, new Vector2(x, y), color);
        }
    }
}
