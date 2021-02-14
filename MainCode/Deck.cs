using System;
using System.Collections.Generic;
using Gamecodeur;
using Microsoft.Xna.Framework;

namespace GCMonogame
{
    public class Deck{
        public List<Card> cards;
        private MainGame mainGame;
        public Deck(){ 
            cards = new List<Card>();

            int nbCards = Enum.GetNames(typeof(cardNumber)).Length;
            int nbColor = Enum.GetNames(typeof(cardColor)).Length;

            Array num = Enum.GetValues(typeof(cardNumber));
            Array col = Enum.GetValues(typeof(cardColor));

            // create a 52 card deck (x6)
            for (int i = 0; i < 6; i++)
            {
                for (int i1 = 0; i1 < nbColor; i1++)
                {
                    for (int i2 = 0; i2 < nbCards; i2++)
                    {  
                        cards.Add(new Card((cardNumber)num.GetValue(i2),
                        (cardColor)col.GetValue(i1),new Vector2(0,0)));
                    }
                }              
            }
        }
        public Card pickup(){
            int index = Util.GetRandomInt(0,cards.Count-1);
            Card c = cards[index];
            cards.RemoveAt(index);
            return c;
        }
    }
    
}