using System;
using System.Collections.Generic;
using Gamecodeur;

namespace GCMonogame
{
    public class Deck{
        public List<Card> cards;
        private Random random;
        private MainGame mainGame;
        public Deck(){ 
            random = new Random();
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
                        cards.Add(new Card(AssetManager.imgCard,(cardNumber)num.GetValue(i2),
                        (cardColor)col.GetValue(i1)));
                    }
                }              
            }
        }
        public Card pickup(){
            int index = random.Next(cards.Count);
            Card c = cards[index];
            cards.RemoveAt(index);
            return c;
        }
    }
    
}