using System;
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

        // settings UI
        Button buttonTable;
        Button buttonLeather;
        Button buttonBackground;
        Button buttonSettings;

        // Game Buttons
        Button buttonHit;
        Button buttonStand;
        Button buttonDouble;
        Button buttonSplit;
        Button buttonBet;

        int backgroundIndex;
        int tableIndex;
        int leatherIndex;

        bool settingsOn = false;

        Hand playerHand;
        Hand dealerHand;
        Deck deck;

        KeyboardState oldKB;

        Card movingCard;
    
        float movingCardvx;
        float movingCardvy;
        float movingCardAngle;
        float movingCardScaleX;
        Hand movingCardHand;
        bool is_movingCard;

        public enum State{
            giveFirstCards,
            game,
            dealerTurn,
            endGame
        }
        static State turnState;

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
            
            // TODO: use this.Content to load your game content here
            
            AssetManager.Load(Content,GraphicsDevice);
            SoundManager.Load(Content);
            
            buttonTable = new Button(AssetManager.button1,AssetManager.button2,"Table",new Vector2(screenWidth/2,(screenHeight/2)-100),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),changeTable);

            buttonLeather = new Button(AssetManager.button1,AssetManager.button2,"Leather",new Vector2(screenWidth/2,(screenHeight/2)),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),changeLeather);

            buttonBackground = new Button(AssetManager.button1,AssetManager.button2,"Back",new Vector2(screenWidth/2,(screenHeight/2)+100),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),changeBackGround);
        
            buttonSettings = new Button(AssetManager.settingsBtn,AssetManager.settingsBtn,"",new Vector2(screenWidth-50,50),
            new Vector2(AssetManager.settingsBtn.Width/2,AssetManager.settingsBtn.Height/2),new Vector2(0.4f,0.4f),toggleSettings);
        

            // Game Buttons
            buttonHit = new Button(AssetManager.button1,AssetManager.button2,"Hit",new Vector2(100,screenHeight-150),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onHit);

            buttonStand = new Button(AssetManager.button1,AssetManager.button2,"Stand",new Vector2(300,screenHeight-150),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onStand);

            buttonDouble = new Button(AssetManager.button1,AssetManager.button2,"Double",new Vector2(500,screenHeight-150),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onDouble);
        
            initGame();
        }

        public void initGame(){
            deck = new Deck();
            playerHand = new Hand(new Vector2(worldWidth/2,worldHeight - 300));
            dealerHand = new Hand(new Vector2(worldWidth/2,worldHeight/2));
            turnState = State.giveFirstCards;
            backgroundIndex = Util.GetRandomInt(0,AssetManager.background.Length-1);
            tableIndex = Util.GetRandomInt(0,AssetManager.table.Length-1);
            leatherIndex = Util.GetRandomInt(0,AssetManager.leather.Length-1);
            movingCard = null;
            movingCardvx = 0;
            movingCardvy = 0;
            movingCardAngle = 0;
            movingCardScaleX = 1;
            movingCardHand = null;
            is_movingCard = false;
            giveStartCards();
        }

        public void giveStartCards(){
            // Donne les cartes si il en manque
            if (playerHand.lst_cards.Count < 2){
                shootCard(playerHand);
                return;
            }
            if (dealerHand.lst_cards.Count < 1){
                shootCard(dealerHand);
                return;
            }
            // Si on a finit de donner les cartes du debut
            if (dealerHand.lst_cards.Count > 2 && playerHand.lst_cards.Count > 2){
                turnState = State.game;
            }
            
        }

        public void giveDealerEndCard(){
           if (dealerHand.score <= 16){
               shootCard(dealerHand);
           }else{
               turnState = State.endGame;
           }
        }


        public void shootCard(Hand h){
            movingCardHand = h;
            movingCard = deck.pickup();
            movingCardAngle = (float)Util.angle(1096f/scaling,370f/scaling,movingCardHand.nextCardPosition.X,movingCardHand.nextCardPosition.Y);
            movingCard.setPosition(new Vector2(1096/scaling,370/scaling));
            movingCardvx = (float)(Math.Cos(movingCardAngle)*16);
            movingCardvy = (float)(Math.Sin(movingCardAngle)*16);
            movingCard.setQuad(movingCard.quadBack);  
            movingCardScaleX = 1;
            is_movingCard = true;

            //Play random sound
            int s_index = Util.GetRandomInt(0,SoundManager.snd_card_slap.Length-1);
            SoundManager.snd_card_slap[s_index].Play();
        }


        public void onHit(Button pButton){
            shootCard(playerHand);
        }

        public void onStand(Button pButton ){
            shootCard(dealerHand);
            turnState = State.dealerTurn;
        }
        public void onBet(Button pButton ){

        }
        public void onDouble(Button pButton ){

        }
        public void onSplit(Button pButton ){

        }
        // UI button functions
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

            if (newKB.IsKeyDown(Keys.Space) && !oldKB.IsKeyDown(Keys.Space)){
                Console.WriteLine("Game Restart");
                initGame();
            }
            if (settingsOn){
                buttonTable.Update(gameTime);
                buttonLeather.Update(gameTime);
                buttonBackground.Update(gameTime);  
            }
            if (!is_movingCard && turnState != State.endGame){
                buttonHit.Update(gameTime);
                buttonStand.Update(gameTime);
                buttonDouble.Update(gameTime);

                buttonHit.setDrawColor(Color.White);
                buttonStand.setDrawColor(Color.White);
                buttonDouble.setDrawColor(Color.White);
            }else{
                buttonHit.setDrawColor(Color.Gray);
                buttonStand.setDrawColor(Color.Gray);
                buttonDouble.setDrawColor(Color.Gray);
            }
            
            
            // Update moving card
            if (is_movingCard){
                Vector2 v = new Vector2(movingCard.position.X-movingCardHand.nextCardPosition.X,movingCard.position.Y-movingCardHand.nextCardPosition.Y);
                movingCard.setPosition(movingCard.position + new Vector2(movingCardvx,movingCardvy));
                // Si carte arrive a destination
                if (v.Length() < 18){

                    movingCard.setPosition(movingCardHand.nextCardPosition);
                    if (movingCard.currentQuad == movingCard.quadBack){
                        movingCardScaleX -= 0.06f;
                        if (movingCardScaleX < 0){
                            movingCard.setQuad(movingCard.quadFace);
                        }
                    }else if(movingCard.currentQuad == movingCard.quadFace){
                        movingCardScaleX += 0.06f;
                        // Si carte arrive a destination
                        if (movingCardScaleX > 1){
                            // ajoute a la main
                            movingCardHand.addCardToHand(movingCard);
                            is_movingCard = false;

                            // Si notre score >= 21, on arrete la game
                            if(playerHand.score >= 21){
                                turnState = State.endGame;
                            }

                            // Machine a etat
                            if (turnState == State.giveFirstCards){
                                //Continue de donner des cartes
                                giveStartCards();
                            }else if(turnState == State.dealerTurn){
                                //Continue de donner des cartes jusqua avoir 16 points 
                                giveDealerEndCard();
                            }

                            if(turnState == State.endGame){
                                // Détermine qui a gagné
                                string message = "";

                                if (playerHand.score > 21){
                                    message = "you loose";
                                }else if(playerHand.score == 21){
                                    message = "you win";
                                }else if(dealerHand.score > 21){
                                    message = "you win";
                                }else if(dealerHand.score == 21){
                                    message = "you loose";
                                }else if(playerHand.score < dealerHand.score){
                                    message = "you loose";
                                }else if(playerHand.score > dealerHand.score){
                                    message = "you win";
                                }else if(playerHand.score == dealerHand.score){
                                    message = "Hand equals";
                                }
                                Console.WriteLine(message);
                            }
                        }       
                    }
                }   
            }
 


            buttonSettings.Update(gameTime);
            oldKB = newKB;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw game World (scale 0.6)
            _spriteBatch.Begin(SpriteSortMode.Deferred,null,null,null,null,null,Matrix.CreateScale(scaling,scaling,1));
            
            //Draw background
            _spriteBatch.Draw(AssetManager.background[backgroundIndex],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.table[tableIndex],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.textLines,new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.leather[leatherIndex],new Vector2(0,0),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
            _spriteBatch.Draw(AssetManager.chips,new Vector2(worldWidth/2,0),null,Color.White,0,new Vector2(AssetManager.chips.Width/2,0),new Vector2(1,1),SpriteEffects.None,1);  
            _spriteBatch.Draw(AssetManager.cardBox,new Vector2(worldWidth-300,(worldHeight/2)-500),null,Color.White,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);  
            
            //Draw Player Hand
            playerHand.draw(_spriteBatch);
            // Draw Dealer Hand
            dealerHand.draw(_spriteBatch);
            // Draw moving card
            if (is_movingCard){
                _spriteBatch.Draw(AssetManager.imgCard,movingCard.position,movingCard.currentQuad,Color.White,0,new Vector2(163/2,238/2),new Vector2(movingCardScaleX,1),SpriteEffects.None,1);
            }
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
            // Draw UI Buttons
            buttonHit.Draw(_spriteBatch);
            buttonStand.Draw(_spriteBatch);
            buttonDouble.Draw(_spriteBatch);

            _spriteBatch.DrawString(AssetManager.MainFont,"X"+Mouse.GetState().Position.X.ToString(),new Vector2(0,0),Color.White);
            _spriteBatch.DrawString(AssetManager.MainFont,"Y"+Mouse.GetState().Position.Y.ToString(),new Vector2(0,30),Color.White);

            

            buttonSettings.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
