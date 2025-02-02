using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pupple.Managers;
using Pupple.States;

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
