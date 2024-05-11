using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LifeSim;

public class LifeSimGame : Game
{
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    Playboard playboard;

    public LifeSimGame()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        playboard = new Playboard(100, 100, spriteBatch, graphics, Content.Load<Texture2D>("cell"));
        FramerateCounter.Initialize();
    }

    protected override void Update(GameTime gameTime)
    {
        // exit
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // update playboard
        if (playboard != null)
        {
            playboard.Update(deltaSeconds);
        }

        // update framerate counter
        if (FramerateCounter.Instance != null)
        {
            FramerateCounter.Instance.Update(deltaSeconds);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        if (playboard != null)
        {
            playboard.Draw();
        }

        base.Draw(gameTime);
    }
}

