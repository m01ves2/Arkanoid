using Arkanoid.Core;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Arkanoid.Screens.Resources
{
    public class GameAssets
    {
        public Texture2D Ball;
        public Texture2D Paddle;
        public Texture2D Brick;

        public Texture2D BallTexture;
        public Texture2D PaddleTexture;
        public Texture2D BrickTexture;
        public Texture2D BackgroundTexture;
        public Dictionary<BonusType, Texture2D> BonusTextures;

        public SoundEffect HitSound;
        public SoundEffect BrickSound;
        public SoundEffect LoseSound;

        public void Load(ContentManager content)
        {
            BallTexture = content.Load<Texture2D>("ball");
            PaddleTexture = content.Load<Texture2D>("paddle");
            BrickTexture = content.Load<Texture2D>("brick");
            BackgroundTexture = content.Load<Texture2D>("background");

            BonusTextures = new Dictionary<BonusType, Texture2D>();
            BonusTextures[BonusType.ExpandPaddle] = content.Load<Texture2D>("bonus_expand");
            BonusTextures[BonusType.ShrinkPaddle] = content.Load<Texture2D>("bonus_shrink");
            BonusTextures[BonusType.MultiBall] = content.Load<Texture2D>("bonus_multiball");
            BonusTextures[BonusType.SlowBall] = content.Load<Texture2D>("bonus_slow");
            BonusTextures[BonusType.PiercingBall] = content.Load<Texture2D>("bonus_piercing");


            HitSound = content.Load<SoundEffect>("hitSound");
            BrickSound = content.Load<SoundEffect>("brickSound");
            LoseSound = content.Load<SoundEffect>("loseSound");

        }
    }
}
