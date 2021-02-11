using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gamecodeur{

    public delegate void OnSlide(Slider pSender);

    public class Slider : Sprite{

        public bool isHover {get;private set;}
        private MouseState oldMouseState;
        public OnSlide onSlide{get;set;}
        public float percent {get;private set;}

        public Slider(Texture2D pTexture): base(pTexture){
            percent = 0;
        }
        public override void Update(GameTime pGameTime){

            MouseState newMouseState = Mouse.GetState();
            Point MousePos = newMouseState.Position;

            if (BoudingBox.Contains(MousePos)){
                if (!isHover){
                    isHover = true;
                    Console.WriteLine("Slider Hover !");
                }
            }else {
                if (isHover){
                    Console.WriteLine("Slider no more Hover !");
                }
                isHover = false;
            }

            if (isHover){
                if (newMouseState.LeftButton == ButtonState.Pressed){
                    Console.WriteLine("Slider is Click !");
                    if (onSlide != null){
                        onSlide(this);
                    }
                    float sizeS = MousePos.X - Position.X;
                    percent = sizeS/this.Texture.Width;
                }
            }
            oldMouseState = newMouseState;
            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            base.Draw(pSpriteBatch);
            pSpriteBatch.Draw(this.Texture,this.Position,new Rectangle(0,0,(int)(this.Texture.Width*percent),this.Texture.Height),Color.Green,0,new Vector2(0,0),new Vector2(1,1),SpriteEffects.None,1);
        }
    }
}