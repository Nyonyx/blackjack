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
        public float scaling = 0.6f; // scaling down all assets
        public int screenWidth;
        public int screenHeight;
        public MainGame()
        {
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        protected override void Initialize()
        {
            
            // TODO: Add your initialization logic here

            _graphics.PreferredBackBufferWidth = 1200; 
            _graphics.PreferredBackBufferHeight = 938;

            screenWidth = (int)(_graphics.PreferredBackBufferWidth/scaling);
            screenHeight = (int)(_graphics.PreferredBackBufferHeight/scaling);

            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
  

  
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


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //scale 0.6
            _spriteBatch.Begin(SpriteSortMode.Deferred,null,null,null,null,null,Matrix.CreateScale(scaling,scaling,1));

            _spriteBatch.Draw(AssetManager.background[2],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.table[0],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.textLines,new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.leather[3],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.chips,new Vector2(screenWidth/2,0),null,Color.White,0,new Vector2(AssetManager.chips.Width/2,0),new Vector2(1,1),SpriteEffects.None,1);



            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
