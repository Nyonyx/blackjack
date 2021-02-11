using System;
using System.Collections.Generic;
using Gamecodeur;
using Microsoft.Xna.Framework;

namespace GCMonogame
{
    //Hand: class only for logic, not for drawing
    public class Hand{
        public List<Card> lst_cards;
        public int score {get; private set;}
        public int mise {get; private set;}
        public State state {get; private set;}
        public SceneGameplay board;
        private int offset_position = 0;
        public Vector2 position;
        private Sprite borderCircle;
        public enum State
        {
            BUSTED,
            WIN,
            PLAYING,
            EQUALS
        }
        public Hand(SceneGameplay pboard,Vector2 pPosition) {
            mise = 5;
            lst_cards = new List<Card>();
            state = State.PLAYING;
            board = pboard;
            position = pPosition;
            borderCircle = new Sprite(AssetManager.circleBoard);
        }
        public void setMise(int pNumber){
            mise = pNumber;
        }
        public void setState(State pState){
            state = pState;
        }

        public Card addCard(){
            Card card = board.deck.pickup();
            
            //card.Position = new Vector2(this.position.X + offset_position,this.position.Y);
            card.Position = new Vector2(board.boxPosition.X + 40,board.boxPosition.Y + 40);
            var angle = Util.angle(board.boxPosition.X,board.boxPosition.Y,position.X+offset_position,position.Y);
            card.vx = (float)Math.Cos(angle)*12;
            card.vy = (float)Math.Sin(angle)*12;
            card.stopPosition = new Vector2(position.X + offset_position,position.Y);
            lst_cards.Add(card);
            board.addActor(card);
            score = this.examine();
            if (score == 21){
                state = State.WIN;
            }else if(score > 21){
                state = State.BUSTED;
            }
            offset_position += 80;


            SceneGameplay.allmovingcards.Add(card);
            return card;
        }


        // return number of points of hand
        public int examine(){
            int points = 0;
            // ajoute les points des cartes sauf les as
            foreach (var card in lst_cards)
            {
                if((int)card.number <= 10){
                    points += (int)card.number;
                }
                else if ((int)card.number > 10 && (int)card.number < 14 ){
                    points += 10;
                }
            }
            // traitement des as
            foreach (var card in lst_cards)
            {
                if (card.number == cardNumber.ace){
                    // as vaut 1 ou 11
                    if (points + 1 == 21 || points + 11 > 21){
                        points += 1;
                    }else if (points + 11 == 21 || points + 11 < 21){
                        points += 11;
                    }
                }
            }
            return points;
        }        
    }
}