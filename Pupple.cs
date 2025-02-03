using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Pupple.Managers;
using Pupple.States;
using Microsoft.Xna.Framework.Media;

namespace Pupple;

public class Pupple : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameManager _gameManager;

    public Pupple()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = Globals.ScreenW;
        _graphics.PreferredBackBufferHeight = Globals.ScreenH;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();

        Window.Title = "Pubble Shooter";
        Window.AllowUserResizing = false;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        
        Globals.Content = Content;
        Globals.GraphicsDevice = GraphicsDevice;
        Globals.SpriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);

        Globals.ShooterSceneSheet = Content.Load<Texture2D>("objects/ShooterSceneSheet");
        Globals.CardSheet = Content.Load<Texture2D>("objects/CardSheet");
        Globals.Font = Content.Load<SpriteFont>("fonts/gameFont");

        Globals.Pixel = pixel;
        Globals.GameState = new GameState();
        Globals.PlayerState = new PlayerState();
        Globals.PopSound = Content.Load<SoundEffect>("sounds/BubblePop");
        Globals.CollideSound = Content.Load<SoundEffect>("sounds/BubbleCollide");
        Globals.DropSound = Content.Load<SoundEffect>("sounds/Drop");
        Globals.WinSound = Content.Load<SoundEffect>("sounds/Win");
        Globals.LoseSound = Content.Load<SoundEffect>("sounds/Lose");
        Globals.ExplodeSound = Content.Load<SoundEffect>("sounds/Bomber");
        Globals.FreezeSound = Content.Load<SoundEffect>("sounds/Freeze");
        Globals.RainbowSound = Content.Load<SoundEffect>("sounds/Rainbow");
        Globals.MagicSound = Content.Load<SoundEffect>("sounds/Magic");
        Globals.PickCardSound = Content.Load<SoundEffect>("sounds/PickCard");
        Globals.ExtraLifeConsumeSound = Content.Load<SoundEffect>("sounds/ExtraLifeConsume");

        Globals.PopSoundInstance = Globals.PopSound.CreateInstance();
        Globals.CollideSoundInstance = Globals.CollideSound.CreateInstance();
        Globals.DropSoundInstance = Globals.DropSound.CreateInstance();
        Globals.WinSoundInstance = Globals.WinSound.CreateInstance();
        Globals.LoseSoundInstance = Globals.LoseSound.CreateInstance();
        Globals.ExplodeSoundInstance = Globals.ExplodeSound.CreateInstance();
        Globals.FreezeSoundInstance = Globals.FreezeSound.CreateInstance();
        Globals.RainbowSoundInstance = Globals.RainbowSound.CreateInstance();
        Globals.MagicSoundInstance = Globals.MagicSound.CreateInstance();
        Globals.PickCardSoundInstance = Globals.PickCardSound.CreateInstance();
        Globals.ExtraLifeConsumeSoundInstance = Globals.ExtraLifeConsumeSound.CreateInstance();
        //Globals.DropSoundInstance = Globals.DropSound.CreateInstance();
        //Globals.DropSoundInstance.Volume = 0.3f;
        Globals.backgroundMusic = Content.Load<Song>("sounds/BackgroundMusic");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.15f;
        MediaPlayer.Play(Globals.backgroundMusic);
        _gameManager = new();
    }

    protected override void Update(GameTime gameTime)
    {
        Globals.Update(gameTime);
        _gameManager.Update();
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        Globals.GraphicsDevice.Clear(Color.Black);

        _gameManager.Draw();

        base.Draw(gameTime);
    }
}