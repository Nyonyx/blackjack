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

    public class Card : Sprite{

        public cardNumber number;
        public cardColor color;
        public Rectangle quadFace;
        public Rectangle quadBack;
        private Rectangle currentQuad;
        public bool is_spinning {get ; private set;}
        private float spin_speed = 0.035f;
        public Vector2 stopPosition = new Vector2(0,0);
        public bool is_moving;

        public Card(Texture2D pTexture,cardNumber pNumber,cardColor pColor) : base(pTexture){
            this.number = pNumber;
            this.color = pColor;
            int lig = (int)pColor;
            int col = (int)pNumber;
            quadFace = new Rectangle((14-col)*163,lig*238,163,238);
            quadBack = new Rectangle(0,4*238,163,238);
            currentQuad = quadBack;
            scaling = new Vector2(GameState.scalingTable,GameState.scalingTable);
            //Console.WriteLine("Card add : {0},{1}",pNumber,pColor);
            this.origin = new Vector2(quadFace.Width/2,quadFace.Height/2);
            is_moving = true;
        }
        public override void Update(GameTime pGameTime)
        {
            bool changeQuad = false;
            if (is_spinning){
                if (this.scaling.X < 0){
                    this.scaling = new Vector2(0,this.scaling.Y);
                    spin_speed = -spin_speed;
                    changeQuad = true;
                }
                if (this.scaling.X > GameState.scalingTable){
                    this.scaling = new Vector2(GameState.scalingTable,this.scaling.Y);
                    is_spinning = false;
                }
                if (changeQuad){
                    currentQuad = quadFace; 
                }
                this.scaling = new Vector2(this.scaling.X - spin_speed,this.scaling.Y);
            }

            if (Position.Y > stopPosition.Y){
                is_moving = false;
                vx = 0;
                vy = 0;
                Position = new Vector2(stopPosition.X,stopPosition.Y);
                spin();
            }


            base.Update(pGameTime);
        }
        public override void Draw(SpriteBatch pSpriteBatch)
        {

            pSpriteBatch.Draw(this.Texture,this.Position,currentQuad,Color.White,0,this.origin,this.scaling,SpriteEffects.None,1);
        }

        public void spin(){
            is_spinning = true;
        }
    }
}
