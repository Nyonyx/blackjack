using System;
using System.IO;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        Button buttonContinue;

        //slider
        Slider sliderBet;

        int backgroundIndex;
        int tableIndex;
        int leatherIndex;

        bool settingsOn = false;

        Hand playerHand;
        Hand dealerHand;
        Deck deck;
        int moneyBalance;
        int mise;

        KeyboardState oldKB;

        Card movingCard;
    
        float movingCardvx;
        float movingCardvy;
        float movingCardScaleX;
        Hand movingCardHand;
        bool is_movingCard;

        bool can_double = true;

        public enum State{
            placeBet,
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
            buttonHit = new Button(AssetManager.button1,AssetManager.button2,"Hit",new Vector2(100,screenHeight-60),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onHit);

            buttonStand = new Button(AssetManager.button1,AssetManager.button2,"Stand",new Vector2(300,screenHeight-60),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onStand);

            buttonDouble = new Button(AssetManager.button1,AssetManager.button2,"Double",new Vector2(500,screenHeight-60),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onDouble);

            buttonSplit = new Button(AssetManager.button1,AssetManager.button2,"Split",new Vector2(700,screenHeight-60),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onSplit);

            buttonBet = new Button(AssetManager.button1,AssetManager.button2,"Bet",new Vector2(100,screenHeight-60),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onBet);

            buttonContinue = new Button(AssetManager.button1,AssetManager.button2,"Continue",new Vector2(100,screenHeight-60),
            new Vector2(AssetManager.button1.Width/2,AssetManager.button1.Height/2),new Vector2(0.4f,0.4f),onContinue);

            sliderBet = new Slider(AssetManager.slider,new Vector2(100,screenHeight-190),
            new Vector2(AssetManager.slider.Width/2,AssetManager.slider.Height),new Vector2(0.4f,0.8f),onSlide);

            // start ambiance
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(SoundManager.song_casino_ambiance);
            moneyBalance = 1000;
            initGame();
        }

        public void initGame(){
            deck = new Deck();
            playerHand = new Hand(new Vector2(worldWidth/2,worldHeight - 350),TypeHand.player);
            dealerHand = new Hand(new Vector2(worldWidth/2,(worldHeight/2)-300),TypeHand.deal);
            // Random backgrounds
            backgroundIndex = Util.GetRandomInt(0,AssetManager.background.Length-1);
            tableIndex = Util.GetRandomInt(0,AssetManager.table.Length-1);
            leatherIndex = Util.GetRandomInt(0,AssetManager.leather.Length-1);

            // Initalise moving card variables
            movingCard = null;
            movingCardvx = 0;
            movingCardvy = 0;
            movingCardScaleX = 1;
            movingCardHand = null;
            is_movingCard = false;

            mise = 200;
            sliderBet.setLevel(0.2f);
            can_double = true;

            // ETAT : Placez vos mises
            turnState = State.placeBet; 
            //playerHand.setVelocity(0.1f,0);
        }

        public void shootCard(Hand h){
            movingCardHand = h;
            movingCard = deck.pickup();

            float movingCardAngle = (float)Util.angle(1096f/scaling,370f/scaling,movingCardHand.nextCardPosition.X,movingCardHand.nextCardPosition.Y);
            movingCardvx = (float)(Math.Cos(movingCardAngle)*19);
            movingCardvy = (float)(Math.Sin(movingCardAngle)*19);            
            
            movingCard.setPosition(new Vector2(1096/scaling,370/scaling));

            movingCard.setQuad(movingCard.quadBack);  
            movingCardScaleX = 1;
            is_movingCard = true;

            //Play random sound
            int s_index = Util.GetRandomInt(0,SoundManager.snd_card_slap.Length-1);
            SoundManager.snd_card_slap[s_index].Play();
        }



        public void onSlide(Slider pSlider){
            mise = (int)(sliderBet.percent * 500);
        }

        public void onContinue(Button pButton){
            initGame();
        }

        public void onBet(Button pButton ){
            giveStartCards();
            turnState = State.giveFirstCards;
            moneyBalance -= mise;
        }
        public void onHit(Button pButton){
            shootCard(playerHand);
            can_double = false;
        }

        public void onStand(Button pButton ){
            shootCard(dealerHand);
            turnState = State.dealerTurn;
        }

        public void onDouble(Button pButton ){
            shootCard(playerHand);
            moneyBalance -= mise;
            mise = mise *2;
            turnState = State.dealerTurn;
        }
        public void onSplit(Button pButton ){

        }

        public void toggleSettings(Button pButton){
            settingsOn = !settingsOn;
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
            if (dealerHand.lst_cards.Count > 1 && playerHand.lst_cards.Count > 2){
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
        protected override void Update(GameTime gameTime)
        {
            KeyboardState newKB = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
                Exit();
            }
            if (GameState.DEBUG){
                if (newKB.IsKeyDown(Keys.Space) && !oldKB.IsKeyDown(Keys.Space)){
                    Console.WriteLine("Game Restart");
                    initGame();
                }
            }
            if (turnState == State.placeBet){
                sliderBet.Update(gameTime);
                buttonBet.Update(gameTime);
            }

            if (settingsOn){
                buttonTable.Update(gameTime);
                buttonLeather.Update(gameTime);
                buttonBackground.Update(gameTime);  
            }

            // Update Game buttons
            if (!is_movingCard && turnState != State.endGame && turnState != State.placeBet){
                buttonHit.Update(gameTime);
                buttonStand.Update(gameTime);
                buttonSplit.Update(gameTime);

                if (can_double){
                    buttonDouble.Update(gameTime);
                    buttonDouble.setDrawColor(Color.White);
                }else{
                    buttonDouble.setDrawColor(Color.Gray);
                }
                
                buttonHit.setDrawColor(Color.White);
                buttonStand.setDrawColor(Color.White);
                buttonSplit.setDrawColor(Color.White);
            }else{
                buttonHit.setDrawColor(Color.Gray);
                buttonStand.setDrawColor(Color.Gray);
                buttonDouble.setDrawColor(Color.Gray);
                buttonSplit.setDrawColor(Color.Gray);
            }
            if (turnState == State.endGame){
                buttonContinue.Update(gameTime);
            }
            buttonSettings.Update(gameTime);

            // Update moving card
            if (is_movingCard){
                Vector2 v = new Vector2(movingCard.position.X-movingCardHand.nextCardPosition.X,movingCard.position.Y-movingCardHand.nextCardPosition.Y);
                
                // add friction
                movingCardvx *= 0.987f;
                movingCardvy *= 0.987f;
                
                // add velocity
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
                            
                            if (playerHand.score >= 21){
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
                            
                                string state = "null";
                                if (playerHand.score > 21){   
                                                             
                                    state = "loose";
                                }else if(playerHand.score == 21){    
                                                               
                                    state = "win";
                                    SoundManager.snd_soft_win.Play();
                                }else if(dealerHand.score > 21){                                  
                                    state = "win";
                                }else if(dealerHand.score == 21){                               
                                    state = "loose";
                                    SoundManager.snd_busted.Play();
                                }else if(playerHand.score < dealerHand.score){                                  
                                    state = "loose";
                                }else if(playerHand.score > dealerHand.score){
                                    state = "win";
                                }else if(playerHand.score == dealerHand.score){                               
                                    state = "equals";
                                }
                                switch (state)
                                {
                                    case "loose":
                                        playerHand.looseColor();
                                        break;
                                    case "win":
                                        moneyBalance = moneyBalance + (mise*2);
                                        SoundManager.snd_applause.Play();
                                        dealerHand.looseColor();
                                        break;
                                    case "equals":
                                        SoundManager.snd_equals.Play();
                                        moneyBalance += mise;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }       
                    }
                }   
            }
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
            if (playerHand.score != 0){
                playerHand.draw(_spriteBatch);
            }
            // Draw Dealer Hand
            if (dealerHand.score != 0){
                dealerHand.draw(_spriteBatch);
            }
            // Draw moving card
            if (is_movingCard){
                _spriteBatch.Draw(AssetManager.imgCard,movingCard.position,movingCard.currentQuad,Color.White,0,new Vector2(163/2,238/2),new Vector2(movingCardScaleX,1),SpriteEffects.None,1);
            }
            _spriteBatch.End();

            // Draw UI
            _spriteBatch.Begin();
            // Settings
            if (settingsOn){
                _spriteBatch.Draw(AssetManager.blueBG,new Vector2(screenWidth/2,screenHeight/2),null,Color.White,0,
                new Vector2(AssetManager.blueBG.Width/2,AssetManager.blueBG.Height/2),new Vector2(1.5f,1.5f),SpriteEffects.None,1);
                buttonTable.Draw(_spriteBatch);
                buttonLeather.Draw(_spriteBatch);
                buttonBackground.Draw(_spriteBatch);
                Vector2 size = AssetManager.MainFont.MeasureString("Settings");
                _spriteBatch.DrawString(AssetManager.MainFont,"Settings",new Vector2(screenWidth/2,(screenHeight/2)-175),Color.White,0,new Vector2(size.X/2,size.Y/2),new Vector2(1.2f,1.2f),SpriteEffects.None,1);
            }

            // Draw UI Bet
            if (turnState == State.placeBet){
                sliderBet.Draw(_spriteBatch);
                buttonBet.Draw(_spriteBatch);
                _spriteBatch.DrawString(AssetManager.MainFont,mise.ToString(),new Vector2(sliderBet.Position.X,150),Color.White);

            }

            // Draw UI Game Buttons
            if (turnState != State.placeBet && turnState != State.endGame){
                buttonHit.Draw(_spriteBatch);
                buttonStand.Draw(_spriteBatch);
                buttonDouble.Draw(_spriteBatch);
                buttonSplit.Draw(_spriteBatch);
            }

            if (turnState == State.endGame){
                buttonContinue.Draw(_spriteBatch);
            }

            // Draw Money Balance
            _spriteBatch.Draw(AssetManager.coinIcon,new Vector2(screenWidth-250,screenHeight-70),null,Color.White,0,
            new Vector2(AssetManager.coinIcon.Width/2,AssetManager.coinIcon.Height/2),new Vector2(0.3f,0.3f),SpriteEffects.None,1);
            
            _spriteBatch.DrawString(AssetManager.MainFont,moneyBalance.ToString(),new Vector2(screenWidth-200,screenHeight-100),Color.White);


            // Draw DEBUG UI
            if (GameState.DEBUG){
                _spriteBatch.DrawString(AssetManager.MainFont,"X"+Mouse.GetState().Position.X.ToString(),new Vector2(0,0),Color.White);
                _spriteBatch.DrawString(AssetManager.MainFont,"Y"+Mouse.GetState().Position.Y.ToString(),new Vector2(0,30),Color.White);
            }
            
            

            buttonSettings.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
