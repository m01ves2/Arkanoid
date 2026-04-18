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
        private List<Particle> _particles = new List<Particle>();
        private List<Bonus> _bonuses = new List<Bonus>();

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
        //private Texture2D _bonusTexture;
        private Dictionary<BonusType, Texture2D> _bonusTextures;

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

            //_paddle = new Paddle(new Vector2(400, 500));
            _paddle = new Paddle(new Vector2(_screenWidth / 2 - 120 / 2, _screenHeight - 50));
            //var ball = new Ball(new Vector2(400, 300));
            //var ball = new Ball(new Vector2(_paddle.Bounds.Left + _paddle.Bounds.Width / 2, _paddle.Bounds.Top - Ball.Size));
            //_balls.Add(ball);
        }

        public void MakeBricks(char[,] brickChars, int screenWidth, int screenHeight)
        {

            int bricksInScreenWidth = screenWidth / Brick.Width;
            int bricksInScreenHeight = (int)(screenHeight * 0.3 / Brick.Height);

            var maxHeight = Math.Min(bricksInScreenHeight, brickChars.GetLength(0));
            var maxWidth = Math.Min(bricksInScreenWidth, brickChars.GetLength(1));

            for (int row = 0; row < maxHeight; row++) {
                for (int col = 0; col < maxWidth; col++) {
                    var brickType = GetBrickTypeBySymbol(brickChars[row, col]);
                    if (brickType == BrickType.None) continue;

                    var x = col * (Brick.Width + Spacing) + OffsetX;
                    var y = row * (Brick.Height + Spacing) + OffsetY;
                    var position = new Vector2(x, y);
                    Brick brick = new Brick(position, brickType);
                    _bricks.Add(brick);
                }
            }
        }

        private static BrickType GetBrickTypeBySymbol(char c) => c switch
        {
            '#' => BrickType.Unbreakable,
            '1' => BrickType.Fragile,
            '2' => BrickType.Normal,
            '3' => BrickType.Strong,
            ' ' => BrickType.None,
            '.' => BrickType.None,
            _ => BrickType.None
        };

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("font");

            _ballTexture = content.Load<Texture2D>("ball");
            _paddleTexture = content.Load<Texture2D>("paddle");
            _brickTexture = content.Load<Texture2D>("brick");
            //_bonusTexture = content.Load<Texture2D>("bonus");
            _bonusTextures = new Dictionary<BonusType, Texture2D>();
            _bonusTextures[BonusType.ExpandPaddle] = content.Load<Texture2D>("bonus_expand");
            _bonusTextures[BonusType.ShrinkPaddle] = content.Load<Texture2D>("bonus_shrink");
            _bonusTextures[BonusType.MultiBall] = content.Load<Texture2D>("bonus_multiball");
            _bonusTextures[BonusType.SlowBall] = content.Load<Texture2D>("bonus_slow");
            _bonusTextures[BonusType.PiercingBall] = content.Load<Texture2D>("bonus_piercing");


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

            foreach (var ball in _balls) {
                ball.Update(gameTime);
            }

            for (int i = 0; i < _bonuses.Count; i++) {
                var bonus = _bonuses[i];
                bonus.Update(gameTime);

                if (bonus.Bounds.Y > _screenHeight) {
                    _bonuses.Remove(bonus);
                }
            }

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
            collisions.AddRange(CollisionSystem.DetectPaddleBonusCollision(_paddle, _bonuses));

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

            if (ball.IsPiercing) {
                brick.Destroy();
            }
            else {
                brick.LoseHp();
            }
            
            if (brick.Hp == 0)
                _bricks.Remove(brick);

            if (brick._brickType != BrickType.Unbreakable)
                AddScore(10);

            SpawnParticles(ball.Bounds.Location.ToVector2());

            if (Random.Shared.NextDouble() < 0.2) {
                //SpawnExtraBall();
                SpawnBonus(new Vector2(brick.Bounds.Left + brick.Bounds.Width / 2, brick.Bounds.Bottom));
            }
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

        public void HandleBonusPickup(Paddle paddle, Bonus bonus)
        {
            AddShake(0.2f);

            float randomPitch = Random.Shared.NextSingle() * 0.2f - 0.1f;
            _brickSound.Play(volume: 0.2f, pitch: randomPitch, pan: 0f);

            _bonuses.Remove(bonus);

            //if (Random.Shared.NextDouble() < 0.1)
            //    SpawnExtraBall();
            switch (bonus.BonusType) {
                case BonusType.ExpandPaddle:
                    ExpandPaddle();
                    break;
                case BonusType.ShrinkPaddle:
                    ShrinkPaddle();
                    break;
                case BonusType.SlowBall:
                    SlowBall();
                    break;
                case BonusType.PiercingBall:
                    PiercingBall();
                    break;
                case BonusType.MultiBall:
                    MultiBall();
                    break;
            }
        }

        private void SpawnBonus(Vector2 position)
        {
            int i = Random.Shared.Next() % Enum.GetValues<BonusType>().Length;

            var bonusPosition = new Vector2(position.X - Bonus.Width / 2, position.Y);
            Bonus bonus;

            switch (i) {
                case 0:
                    bonus = new Bonus(bonusPosition, BonusType.ExpandPaddle);
                    break;
                case 1:
                    bonus = new Bonus(bonusPosition, BonusType.ShrinkPaddle);
                    break;
                case 2:
                    bonus = new Bonus(bonusPosition, BonusType.SlowBall);
                    break;
                case 3:
                    bonus = new Bonus(bonusPosition, BonusType.PiercingBall);
                    break;
                default:
                    bonus = new Bonus(bonusPosition, BonusType.MultiBall);
                    break;
            }

            _bonuses.Add(bonus);
        }

        private void MultiBall()
        {
            var ball = new Ball(new Vector2(_paddle.Bounds.Left + _paddle.Bounds.Width / 2, _paddle.Bounds.Top - Ball.Size));
            _balls.Add(ball);
        }

        private void ExpandPaddle()
        {
            _paddle.Expand(30);
        }

        private void ShrinkPaddle()
        {
            _paddle.Expand(-30);
        }

        private void SlowBall()
        {
            foreach(var ball in _balls) {
                //if (ball.IsPiercing) {
                //    ball.SetPiercing(false);
                //}
                ball.ResetSpeed();
            }
        }

        private void PiercingBall()
        {
            foreach (var ball in _balls)
                ball.SetPiercing(true);
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

            foreach(var bonus in _bonuses) {
                bonus.Draw(spriteBatch, _bonusTextures[bonus.BonusType]);
            }

            foreach (var particle in _particles) {
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

                var life = 0.5f;
                var maxLife = 0.5f;

                _particles.Add(new Particle(position, velocity, life, maxLife));
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

            return new Vector2(Random.Shared.Next(-5, 6), Random.Shared.Next(-5, 6)) * _shakeStrength;
        }

        public void AddScore(int value)
        {
            _score += value;
        }

        public void CheckGameOver()
        {
            if (_state == GameState.GameOver) return;

            for (int i = _balls.Count - 1; i >= 0; i--) {
                if (_balls[i].Bounds.Y > _screenHeight) {
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
            StopShake();
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
            ResetBonuses();
            _paddle.ResetWidth();
        }

        private void ResetBall()
        {
            var ball = new Ball(new Vector2(_paddle.Bounds.Left + _paddle.Bounds.Width / 2, _paddle.Bounds.Top - Ball.Size));
            //var ball = new Ball(new Vector2(_screenWidth / 2f, _screenHeight / 2f));
            _balls.Clear();
            _balls.Add(ball);
        }

        private void ResetBonuses()
        {
            _bonuses.Clear();
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

            var path = Path.Combine(AppContext.BaseDirectory, "Levels", "level2.txt");
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
