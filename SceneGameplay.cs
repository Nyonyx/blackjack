using System;
using System.Collections.Generic;
using System.Windows;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static Gamecodeur.GameState;

namespace GCMonogame
{

    public enum Turnstate{
        Mise,
        GiveCards,
        Game,
        CroupierTurn,
        EndGame
    }

    public class SceneGameplay : Scene
    {
        public gameSettings gameSettings;


        public Deck deck;
        public Player player;
        public Croupier croupier;
        private KeyboardState oldkeyboard;

        Button hitButton;
        Button standButton;
        Button doubleButton;
        Button splitButton;
        Button betButton;

        Button settingsButton;

        Slider slider;
        public int potentialBet = 0;

        public static Turnstate state;
        public static Turnstate oldState;

        public static List<Card> allmovingcards; // reference de toute les cartes qui bougent

        // background table
        public Sprite table;
        public Sprite leather;

        Sprite textLines;
        Sprite cardBox;
        Sprite background;

        public Vector2 boxPosition { get; private set;}

        public SceneGameplay(MainGame pGame) : base(pGame)
        {
            init();
        }

        public void setState(Turnstate pT){
            state = pT;
        }
        public void init(){
            //Test Commit
            allmovingcards = new List<Card>();
            state = Turnstate.Mise;
            deck = new Deck();
            player = new Player(this,new Vector2(Screen.Width/2,Screen.Height-100));
            croupier = new Croupier(this,new Vector2(Screen.Width/2,Screen.Height/2));
            
            hitButton = new Button(mainGame.Content.Load<Texture2D>("UI/button1"),"hit",new Vector2(0,Screen.Height-100),player.hit);
            standButton = new Button(mainGame.Content.Load<Texture2D>("UI/button1"),"stand",new Vector2(200,Screen.Height-100),player.stand);
            betButton = new Button(mainGame.Content.Load<Texture2D>("UI/button1"),"bet",new Vector2(400,Screen.Height-100),player.bet);

            
            hitButton.isActive = false;
            standButton.isActive = false;
            betButton.isActive = true;

            slider = new Slider(mainGame.Content.Load<Texture2D>("UI/slider3"));
            slider.Position = new Vector2((Screen.Width/3)-(slider.Texture.Width/2),
            (Screen.Height)-slider.Texture.Height);
            slider.onSlide = slide;

            // background sprites        
            table = new Sprite(AssetManager.table[0]);
            leather = new Sprite(AssetManager.leather[3]);
            textLines = new Sprite(AssetManager.textLines);
            cardBox = new Sprite(AssetManager.cardBox);
            background = new Sprite(AssetManager.background[2]);

            table.scaling = new Vector2(scalingTable,scalingTable);
            leather.scaling = new Vector2(scalingTable,scalingTable);
            textLines.scaling = new Vector2(scalingTable,scalingTable);
            cardBox.scaling = new Vector2(scalingTable,scalingTable);
            background.scaling = new Vector2(scalingTable,scalingTable);
            
            table.Position = new Vector2(-152*scalingTable,-13*scalingTable);
            textLines.Position = new Vector2(88*scalingTable,279*scalingTable);
            leather.Position = new Vector2(0,1052*scalingTable);
            cardBox.Position = new Vector2(1667*scalingTable,152*scalingTable);
            background.Position = new Vector2(-382*scalingTable,-76*scalingTable);

            boxPosition = cardBox.Position;
            addActor(background);
            addActor(table);
            addActor(textLines);
            addActor(leather);
            addActor(cardBox);
            //add buttons
            addActor(hitButton);
            addActor(standButton);
            addActor(betButton);
            addActor(slider);

            gameSettings = new gameSettings(mainGame,this);

            
            
        }

        public override void Load()
        {
            base.Load();
        }
        public override void UnLoad()
        {
            Console.WriteLine("SceneGameplay.Unload");
            base.UnLoad();
        }

        public void slide(Slider pSender){
            potentialBet = (int)(slider.percent * 200);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newkeyboard = Keyboard.GetState();
            if(newkeyboard.IsKeyDown(Keys.Space) && !oldkeyboard.IsKeyDown(Keys.Space)){
                
               mainGame.gameState.ChangeScene(SceneType.GamePlay);
            }

            oldkeyboard = newkeyboard;

            bool b  = false;
            switch (state)
            {
                case Turnstate.Mise:
                    hitButton.isActive = false;
                    standButton.isActive = false;
                    betButton.isActive = true;
                    slider.isActive = true;
                    break;
                case Turnstate.GiveCards:
                    betButton.isActive = false;
                    // execute only 1 time (give cards)
                    if (oldState != state){
                        croupier.giveStartCards();
                    }
                    b = moveCards();
                    if (b == true){
                        state = Turnstate.Game;
                    }   

                    break;
                case Turnstate.Game:

                    if (player.lst_hands[0].state == Hand.State.PLAYING){
                        hitButton.isActive = true;
                        standButton.isActive = true;
                        betButton.isActive = false;
                        slider.isActive = false;
                    }else{
                        hitButton.isActive = false;
                        standButton.isActive = false;
                        betButton.isActive = false;
                        slider.isActive = false;
                    }

                    break;
                case Turnstate.CroupierTurn:
                    hitButton.isActive = false;
                    standButton.isActive = false;
                    betButton.isActive = false;
                    slider.isActive = false;

                    if (oldState != state){
                        croupier.tireCartes();
                        croupier.compare(player.lst_hands[0]);
                        croupier.giveResult(player.lst_hands[0]);
                    }
                    b = moveCards();
                    if (b == true){
                        state = Turnstate.EndGame;
                    }   
                    break;
                case Turnstate.EndGame:
                    
                    break;

                default:
                    break;
            }
            oldState = state;
            base.Update(gameTime);
        }

        // return true if all cards have finished moving
        // else return false
        public bool moveCards(){
            // move the cards
            if (allmovingcards.Count > 0){
                allmovingcards[0].isActive = true;
                if (allmovingcards[0].is_moving == false){
                    allmovingcards.RemoveAt(0);
                }
            }else{
                return true;
            }           
            return false;
        }

        public override void Draw(GameTime gameTime)
        {
            // Draw sprites & stuff..
            base.Draw(gameTime);

            // Draw debug Info
            if (DEBUG){
                // Dessine infos main joueur
                for (int i1 = 0; i1 < player.lst_hands.Count; i1++)
                {
                    Hand hand = player.lst_hands[i1];   
                    mainGame._spriteBatch.DrawString(AssetManager.MainFont,"state:" + hand.state.ToString(),new Vector2(0,0),Color.White);
                    mainGame._spriteBatch.DrawString(AssetManager.MainFont,"score:" + hand.score.ToString(),new Vector2(140,0),Color.White);
                    mainGame._spriteBatch.DrawString(AssetManager.MainFont,"mise:" + hand.mise.ToString(),new Vector2(280,0),Color.White);
                }    
                mainGame._spriteBatch.DrawString(AssetManager.MainFont,"Turn:" +state.ToString(),new Vector2(0,20),Color.White);
                mainGame._spriteBatch.DrawString(AssetManager.MainFont,"money:" + player.money.ToString(),new Vector2(140,20),Color.White);

                if (state == Turnstate.Mise){
                    mainGame._spriteBatch.DrawString(AssetManager.MainFont,potentialBet.ToString(),new Vector2(500,0),Color.White);
                }

                mainGame._spriteBatch.DrawString(AssetManager.MainFont,"score2:"+croupier.hand.score.ToString(),new Vector2(560,0),Color.White);

                mainGame._spriteBatch.DrawString(AssetManager.MainFont,"mx"+Mouse.GetState().Position.X,new Vector2(0,600),Color.White);
                mainGame._spriteBatch.DrawString(AssetManager.MainFont,"my"+Mouse.GetState().Position.Y,new Vector2(0,630),Color.White);
            
            }
        

        }
    }
}
