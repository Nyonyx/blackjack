using System;
using System.Collections.Generic;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GCMonogame
{
    //Hand: class only for logic, not for drawing
    public class Hand{
        private List<Card> lst_cards;
        public Vector2 Position;
        private int offsetX = 0;
        public Hand(Vector2 pPosition) {
           lst_cards = new List<Card>();
           
           Position = pPosition;
        }

        public void addCard(Card c){
            lst_cards.Add(c);


            float centerX = (lst_cards.Count*180)/2;

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

        public void Update(){
            float centerX = (lst_cards.Count*180)/2;
            for (int i = 0; i < lst_cards.Count; i++)
            {

                Card c = lst_cards[i];
                c.setPosition(new Vector2((Position.X + (i*180)) - centerX,
                Position.Y - (238/2)));
            }

        }

        public void draw(SpriteBatch pSpriteBatch){
            
            for (int i = 0; i < lst_cards.Count; i++)
            {
                Card c = lst_cards[i];
                pSpriteBatch.Draw(AssetManager.imgCard,c.position,c.quadFace,Color.White,0,new Vector2(0,0),
                new Vector2(1,1),SpriteEffects.None,1);      
            }
        }  
    }
}