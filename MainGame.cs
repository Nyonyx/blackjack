using System.IO;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteFontPlus;

namespace GCMonogame
{
    public class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public GameState gameState;

        public MainGame()
        {
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = (int)(2000*0.6f);
            _graphics.PreferredBackBufferHeight = (int)(1563*0.6f);
            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            gameState = new GameState(this);
            gameState.ChangeScene(GameState.SceneType.Menu);
            IsMouseVisible = true;
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            AssetManager.Load(Content,GraphicsDevice);
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (gameState.CurrentScene != null)
            {
                gameState.CurrentScene.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            if (gameState.CurrentScene != null)
            {
                gameState.CurrentScene.Draw(gameTime);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
