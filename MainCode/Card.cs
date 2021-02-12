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
        public Rectangle quadFace;
        private Rectangle quadBack;
        private Rectangle currentQuad;
        public bool is_spinning {get ; private set;}
        private float spin_speed = 0.035f;
        public bool is_moving;
        public Vector2 position {get;private set;}

        public Card(cardNumber pNumber,cardColor pColor,Vector2 pPosition){
            this.number = pNumber;
            this.color = pColor;
            int lig = (int)pColor;
            int col = (int)pNumber;
            quadFace = new Rectangle((14-col)*163,lig*238,163,238);
            quadBack = new Rectangle(0,4*238,163,238);
            currentQuad = quadBack;
            //Console.WriteLine("Card add : {0},{1}",pNumber,pColor);
            is_moving = true;
            position = pPosition;
        }
        public void setPosition(Vector2 pPosition){
            position = pPosition;
        }

    }
}
