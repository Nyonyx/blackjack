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
        public static float scaling = 0.6f; // scaling down all assets
        public int worldWidth;
        public int worldHeight;
        public int screenWidth;
        public int screenHeight;


        int backgroundIndex = 0;
        int tableIndex = 0;
        int leatherIndex = 0;

        Button buttonTable;
        Button buttonLeather;
        Button buttonBackground;
        Button buttonSettings;

        bool settingsOn = false;
        Hand hand;
        Deck deck;

        KeyboardState oldKB;
        Card movingCard;


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

            screenWidth = 1200;
            screenHeight = 938;

            worldWidth = (int)(screenWidth/scaling);
            worldHeight = (int)(screenHeight/scaling);

            //_graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            IsMouseVisible = true;

            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            deck = new Deck();
            // TODO: use this.Content to load your game content here
            AssetManager.Load(Content,GraphicsDevice);
            buttonTable = new Button(AssetManager.button1,AssetManager.button2,"Table",new Vector2(screenWidth/2,(screenHeight/2)-100),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),changeTable);

            buttonLeather = new Button(AssetManager.button1,AssetManager.button2,"Leather",new Vector2(screenWidth/2,(screenHeight/2)),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),changeLeather);

            buttonBackground = new Button(AssetManager.button1,AssetManager.button2,"Back",new Vector2(screenWidth/2,(screenHeight/2)+100),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),changeBackGround);
        
            buttonSettings = new Button(AssetManager.settingsBtn,AssetManager.settingsBtn,"",new Vector2(screenWidth-50,50),
            new Vector2(AssetManager.settingsBtn.Width/2,AssetManager.settingsBtn.Height/2),new Vector2(0.4f,0.4f),toggleSettings);
        
            hand = new Hand(new Vector2(worldWidth/2,worldHeight/2));
            hand.addCard(deck.pickup());
            hand.addCard(deck.pickup());

        }

        public void changeTable(Button pButton){
            tableIndex +=1;
            if (tableIndex > AssetManager.background.Length-1){
                tableIndex = 0;
            }
        }
        public void changeLeather(Button pButton){
            leatherIndex +=1;
            if (leatherIndex > AssetManager.leather.Length-1){
                leatherIndex = 0;
            }
        }
        public void changeBackGround(Button pButton){
            backgroundIndex +=1;
            if (backgroundIndex > AssetManager.background.Length-1){
                backgroundIndex = 0;
            }
        }

        public void toggleSettings(Button pButton){
            settingsOn = !settingsOn;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState newKB = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
                Exit();
            }

            if (newKB.IsKeyDown(Keys.X) && !oldKB.IsKeyDown(Keys.X)){
                movingCard = deck.pickup();
            }
            if (settingsOn){
                buttonTable.Update(gameTime);
                buttonLeather.Update(gameTime);
                buttonBackground.Update(gameTime);
                
            }
            hand.Update();
            buttonSettings.Update(gameTime);
            oldKB = newKB;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw game World
            // World to screen projection matrix
            _spriteBatch.Begin(SpriteSortMode.Deferred,null,null,null,null,null,Matrix.CreateScale(scaling,scaling,1));

            _spriteBatch.Draw(AssetManager.background[backgroundIndex],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.table[tableIndex],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.textLines,new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.leather[leatherIndex],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.chips,new Vector2(worldWidth/2,0),null,Color.White,0,new Vector2(AssetManager.chips.Width/2,0),new Vector2(1,1),SpriteEffects.None,1);  
            
            hand.draw(_spriteBatch);
            _spriteBatch.End();

            // Draw UI
            _spriteBatch.Begin();
            if (settingsOn){
                _spriteBatch.Draw(AssetManager.blueBG,new Vector2(screenWidth/2,screenHeight/2),null,Color.White,0,
                new Vector2(AssetManager.blueBG.Width/2,AssetManager.blueBG.Height/2),new Vector2(1.5f,1.5f),SpriteEffects.None,1);
                buttonTable.Draw(_spriteBatch);
                buttonLeather.Draw(_spriteBatch);
                buttonBackground.Draw(_spriteBatch);
                Vector2 size = AssetManager.MainFont.MeasureString("Settings");
                _spriteBatch.DrawString(AssetManager.MainFont,"Settings",new Vector2(screenWidth/2,(screenHeight/2)-175),Color.White,0,new Vector2(size.X/2,size.Y/2),new Vector2(1.2f,1.2f),SpriteEffects.None,1);
            }

            _spriteBatch.DrawString(AssetManager.MainFont,"X"+Mouse.GetState().Position.X.ToString(),new Vector2(0,0),Color.White);
            _spriteBatch.DrawString(AssetManager.MainFont,"Y"+Mouse.GetState().Position.Y.ToString(),new Vector2(0,30),Color.White);

            

            buttonSettings.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
