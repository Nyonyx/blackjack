using System;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GCMonogame
{
    public enum cardNumber{
        two=2,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        jack,
        queen,
        king,
        ace
    }
    public enum cardColor{
        club=0,
        diamond,
        spades,
        hearts
    }

    public class Card {

        public cardNumber number;
        public cardColor color;
        public Rectangle quadFace {get; private set;}
        public Rectangle quadBack {get; private set;}
        public Rectangle currentQuad {get; private set;}

        // Position relative to the hand !!
        public Vector2 position {get;private set;}

        public Card(cardNumber pNumber,cardColor pColor,Vector2 pPosition){
            this.number = pNumber;
            this.color = pColor;
            int lig = (int)pColor;
            int col = (int)pNumber;

            quadFace = new Rectangle((14-col)*163,lig*238,163,238);
            quadBack = new Rectangle(163,4*238,163,238);

            position = pPosition;
            currentQuad = quadFace;
            //Console.WriteLine("Card add : {0},{1}",pNumber,pColor);
        }
        public void setPosition(Vector2 pPosition){
            position = pPosition;
        }
        public void setQuad(Rectangle pQuad){
            currentQuad = pQuad;
        }
    }
}
