using System;
using System.Collections.Generic;
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

        List<Hand> playerHands;
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

        private int _currentHandIndex; // never use this value
        public int currentHandIndex{
            get{
                return _currentHandIndex;
            }
            private set{
                _currentHandIndex = value;
                if (_currentHandIndex > playerHands.Count-1){
                    _currentHandIndex = playerHands.Count-1;
                }
            }
        }

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
            playerHands = new List<Hand>();
            playerHands.Add(new Hand(new Vector2(worldWidth/2,worldHeight - 500),TypeHand.player));
            dealerHand = new Hand(new Vector2(worldWidth/2,(worldHeight/2)-300),TypeHand.deal);
            // Pick Random backgrounds
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
            currentHandIndex = 0;
        }

        public void switchHand(){      
            
            // si on est a la derniere main
            if (currentHandIndex >= playerHands.Count-1){                   
                if (remainHands()){
                     turnState = State.dealerTurn;
                }else{
                    turnState = State.endGame;
                }            
            }
            currentHandIndex += 1;
        }

        public void shootCard(Hand h){
            movingCardHand = h;
            movingCard = deck.pickup();

            float movingCardAngle = (float)Util.angle(1096f/scaling,370f/scaling,movingCardHand.nextCardPosition.X,movingCardHand.nextCardPosition.Y);
            movingCardvx = (float)(Math.Cos(movingCardAngle)*25);
            movingCardvy = (float)(Math.Sin(movingCardAngle)*25);            
            
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
            shootCard(playerHands[currentHandIndex]);
            can_double = false;
        }

        public void onStand(Button pButton ){  
            // So on a finit de jouer toutes nos mains
            if (currentHandIndex == playerHands.Count-1){
                turnState = State.dealerTurn;
                shootCard(dealerHand);
            }
            switchHand();
            
        }

        public void onDouble(Button pButton ){
            shootCard(playerHands[currentHandIndex]);
            moneyBalance -= mise;
            mise = mise *2;
            switchHand();
        }
        public void onSplit(Button pButton ){
            if (playerHands.Count == 1){
                // move current hand x00
                playerHands[0].setPosition(new Vector2((worldWidth/2)-500,(worldHeight/2)));
                // create a new hand 00x
                Hand hand = new Hand(new Vector2((worldWidth/2)+500,(worldHeight/2)),TypeHand.player);
                playerHands.Add(hand);

                hand.addCardToHand(playerHands[0].lst_cards[1]);
                playerHands[0].removeLastCard();
                currentHandIndex = 0;
                // Give 2 Cards Each Hands
                turnState = State.giveFirstCards;
                shootCard(playerHands[0]);
            }else if(playerHands.Count == 2){
                // Create a new hand
                Hand hand = new Hand(new Vector2((worldWidth/2),(worldHeight/2)+500),TypeHand.player);
                playerHands.Add(hand);
                hand.addCardToHand(playerHands[currentHandIndex].lst_cards[1]);            
                playerHands[currentHandIndex].removeLastCard();
                turnState = State.giveFirstCards;
                shootCard(playerHands[currentHandIndex]);
            }

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

            if (playerHands[0].lst_cards.Count < 2)
            {
                shootCard(playerHands[0]);
                return;
            }
            if (playerHands.Count > 1)
            {
                if (playerHands[1].lst_cards.Count < 2)
                {
                    shootCard(playerHands[1]);
                    return;
                }
            }
            if (playerHands.Count > 2)
            {
                if (playerHands[2].lst_cards.Count < 2)
                {
                    shootCard(playerHands[2]);
                    return;
                }
            }

            if (dealerHand.lst_cards.Count < 1)
            {
                shootCard(dealerHand);
                return;
            }
            turnState = State.game;
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

            #if DEBUG
                if (newKB.IsKeyDown(Keys.Space) && !oldKB.IsKeyDown(Keys.Space)){
                    Console.WriteLine("Game Restart");
                    initGame();
                }
            #endif

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)){
                Exit();
            }
            foreach (Hand hand in playerHands)
            {
                hand.update(gameTime);
            }
            
            dealerHand.update(gameTime);

            // Update button states
            buttonBet.setActive(false);
            buttonContinue.setActive(false);
            buttonHit.setActive(false);
            buttonStand.setActive(false);
            buttonSplit.setActive(false);
            buttonDouble.setActive(false);
            buttonTable.setActive(false);
            buttonLeather.setActive(false);
            buttonBackground.setActive(false);

            if (turnState == State.placeBet){
                sliderBet.Update(gameTime);
                buttonBet.setActive(true);
            }
            if (turnState == State.endGame){
                buttonContinue.setActive(true);
            }

            if (!is_movingCard && turnState == State.game){
                buttonHit.setActive(true);
                buttonStand.setActive(true);
                if (playerHands.Count < 3 && playerHands[currentHandIndex].lst_cards.Count == 2){
                    buttonSplit.setActive(true);
                }
                if (can_double){
                    buttonDouble.setActive(true);
                }                 
            }
            if (settingsOn){
                buttonTable.setActive(true);
                buttonLeather.setActive(true);
                buttonBackground.setActive(true);
            }
            // Update Game buttons
            buttonTable.Update(gameTime);
            buttonLeather.Update(gameTime);
            buttonBackground.Update(gameTime);  

            buttonBet.Update(gameTime);
            buttonHit.Update(gameTime);
            buttonStand.Update(gameTime);
            buttonSplit.Update(gameTime);
            buttonDouble.Update(gameTime);
            buttonContinue.Update(gameTime);
            buttonSettings.Update(gameTime);

            // Update moving card
            if (is_movingCard){
                Vector2 v = new Vector2(movingCard.position.X-movingCardHand.nextCardPosition.X,movingCard.position.Y-movingCardHand.nextCardPosition.Y);
                
                // add friction
                movingCardvx *= 0.987f;
                movingCardvy *= 0.987f;
                
                // add velocity
                movingCard.setPosition(movingCard.position + new Vector2(movingCardvx,movingCardvy));
                  
                // Si carte arrive au point
                if (v.Length() < 18){

                    movingCard.setPosition(movingCardHand.nextCardPosition);
                    if (movingCard.currentQuad == movingCard.quadBack){
                        movingCardScaleX -= 0.06f;
                        if (movingCardScaleX < 0){
                            movingCard.setQuad(movingCard.quadFace);
                        }
                    }else if(movingCard.currentQuad == movingCard.quadFace){
                        movingCardScaleX += 0.06f;


                        // Carte ajoute a la main
                        if (movingCardScaleX > 1){
                
                            movingCardHand.addCardToHand(movingCard);
                            is_movingCard = false;

                            Hand currentHand = playerHands[currentHandIndex];
                            
                             if (currentHand.score >= 21){
                                switchHand();
                             }

                            // Continue de donner des cartes si on en a besoin
                            if (turnState == State.giveFirstCards){
                                giveStartCards();
                            }else if(turnState == State.dealerTurn){
                                giveDealerEndCard();
                            }

                            if(turnState == State.endGame){
                                // Détermine qui a gagné
                            
                                string state = "null";
                                if (playerHands[currentHandIndex].score > 21){   
                                                             
                                    state = "loose";
                                }else if(playerHands[currentHandIndex].score == 21){    
                                                               
                                    state = "win";
                                    SoundManager.snd_soft_win.Play();
                                }else if(dealerHand.score > 21){                                  
                                    state = "win";
                                }else if(dealerHand.score == 21){                               
                                    state = "loose";
                                    SoundManager.snd_busted.Play();
                                }else if(playerHands[currentHandIndex].score < dealerHand.score){                                  
                                    state = "loose";
                                }else if(playerHands[currentHandIndex].score > dealerHand.score){
                                    state = "win";
                                }else if(playerHands[currentHandIndex].score == dealerHand.score){                               
                                    state = "equals";
                                }
                                switch (state)
                                {
                                    case "loose":
                                        for (int i = 0; i < playerHands.Count; i++)
                                        {
                                            playerHands[i].looseColor();
                                        }
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

        private bool remainHands(){
            bool c = false;
            foreach (Hand hand in playerHands)
            {
                if (hand.score < 21){
                    c = true;
                }
            }
            return c;
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
            
            //Draw Player Hands
            foreach (Hand hand in playerHands)
            {
                if (hand.score != 0){
                    hand.draw(_spriteBatch);
                }   
                // Draw cursor on the hand
                if (hand == playerHands[currentHandIndex] && !is_movingCard && playerHands.Count > 1){
                    _spriteBatch.Draw(AssetManager.handIcon,hand.nextCardPosition,null,Color.White,(float)(-Math.PI/2),new Vector2(AssetManager.handIcon.Width/2,AssetManager.handIcon.Height/2),new Vector2(0.4f,0.4f),SpriteEffects.None,1);  
                           
                }             
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
            buttonSettings.Draw(_spriteBatch);
            _spriteBatch.End();
            
            // Draw DEBUG UI
            
            #if DEBUG
                _spriteBatch.Begin(SpriteSortMode.Deferred,null,null,null,null,null,Matrix.CreateScale(0.5f,0.5f,1));
                _spriteBatch.DrawString(AssetManager.MainFont,"X"+Mouse.GetState().Position.X.ToString(),new Vector2(0,0),Color.White);
                _spriteBatch.DrawString(AssetManager.MainFont,"Y"+Mouse.GetState().Position.Y.ToString(),new Vector2(0,40),Color.White);
                
                _spriteBatch.DrawString(AssetManager.MainFont,"State"+turnState.ToString(),new Vector2(140,0),Color.White);
                _spriteBatch.End();
            #endif
            
            


            base.Draw(gameTime);
        }
    }
}
