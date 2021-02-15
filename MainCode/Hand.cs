using System;
using System.Collections.Generic;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GCMonogame
{

    public enum TypeHand{
        player,
        deal
    }

    //Hand: class only for logic, not for drawing
    public class Hand{
        public List<Card> lst_cards {get;private set;}
        public Vector2 Position;
        public int score = 0;
        private int offsetX = 0;
        public Vector2 nextCardPosition {get;private set;}
        public Color colorHand;
        public TypeHand type;

        
        public Hand(Vector2 pPosition,TypeHand pType) {
           lst_cards = new List<Card>();
           
           Position = pPosition;
           nextCardPosition = Position + new Vector2(180/2,0);
           colorHand = Color.White;
           type = pType;
        }
        public void looseColor(){
            colorHand = new Color(0.4f,0.4f,0.4f);
        }

        public void addCardToHand(Card c){
            lst_cards.Add(c);
            float centerX = (lst_cards.Count*180)/2;
            for (int i = 0; i < lst_cards.Count; i++)
            {
                Card card = lst_cards[i];
                card.setPosition(new Vector2((Position.X + (i*180)) - centerX,
                Position.Y - (238/2)));
            }
            calculNextPosition();
            score = examine();
        }

        private void calculNextPosition(){
            Card last = lst_cards[lst_cards.Count-1];
            nextCardPosition = last.position + new Vector2(180 + (163/2),238/2);
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
                    if (points + 11 > 21){
                        points += 1;
                    }else if (points + 11 == 21 || points + 11 < 21){
                        points += 11;
                    }
                }
            }
            return points;
        }      

        public void draw(SpriteBatch pSpriteBatch){
            
            for (int i = 0; i < lst_cards.Count; i++)
            {
                Card c = lst_cards[i];
                pSpriteBatch.Draw(AssetManager.imgCard,c.position,c.quadFace,colorHand,0,new Vector2(0,0),
                new Vector2(1,1),SpriteEffects.None,1);      
            }

            Color color = Color.White;
            bool drawStar = false;
            if (score > 21){
                color = Color.Red;
            }else if(score == 21){
                color = Color.White;
                drawStar = true;
            }else if(score < 21)
            {
                color = colorHand;
            }

            if (drawStar && type == TypeHand.player){
                pSpriteBatch.Draw(AssetManager.starIcon,Position + new Vector2(0,-230),null,Color.White,0,new Vector2(AssetManager.starIcon.Width/2,AssetManager.starIcon.Height/2),new Vector2(0.7f,0.7f),SpriteEffects.None,1);       
            }

            Vector2 size = AssetManager.MainFont.MeasureString(score.ToString());
            pSpriteBatch.DrawString(AssetManager.MainFont,score.ToString(),Position + new Vector2(0,-230),color,0,new Vector2(size.X/2,size.Y/2),new Vector2(2,2),SpriteEffects.None,1);
        }  
    }
}