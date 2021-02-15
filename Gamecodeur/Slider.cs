using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gamecodeur{

    public delegate void OnSlide(Slider pSender);

    public class Slider{

        public bool isHover {get;private set;}
        private MouseState oldMouseState;
        public OnSlide onSlide{get;set;}
        public float percent {get;private set;}
        public Rectangle BoudingBox;
        public Texture2D texture;
        public Vector2 Position;
        private Vector2 scaling;
        public Vector2 origin;
        private Color drawColor;
        private float cursorY;
        private bool is_click = false;

        public Slider(Texture2D pTexture,Vector2 pPosition,Vector2 pOrigin,Vector2 pScaling,OnSlide pOnSlide){
            Position = pPosition;
            origin = pOrigin;
            scaling = pScaling;
            texture = pTexture;
            percent = 0;
            BoudingBox = new Rectangle((int)(Position.X-(origin.X*scaling.X)),
            (int)(Position.Y-(origin.Y*scaling.Y)),(int)(texture.Width*scaling.X),
            (int)(texture.Height*scaling.Y));
            drawColor = Color.White;
            onSlide = pOnSlide;
            cursorY = Position.Y;

        }
        // Set the slider Level
        // 0 = 0% slide
        // 1 = 100% slide
        public void setLevel(float pLevel){
            percent = Math.Clamp(pLevel,0,1);
        }
        public void Update(GameTime pGameTime){

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
                    is_click = true;
                    Console.WriteLine("Slider is Click !");


                }
            }
            if (is_click){
                if (onSlide != null){
                    onSlide(this);
                }
                float s = Math.Abs(MousePos.Y - (BoudingBox.Y+BoudingBox.Height));
                setLevel(s/BoudingBox.Height);

                cursorY = (float)((BoudingBox.Y + BoudingBox.Height) - (percent*BoudingBox.Height));
                if (newMouseState.LeftButton == ButtonState.Released){
                    is_click = false;
                }
            }

            oldMouseState = newMouseState;

        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            
            // Draw slider back
            pSpriteBatch.Draw(texture,this.Position,null,Color.Green,0,origin,scaling,SpriteEffects.None,1);
            // Draw slider front
            pSpriteBatch.Draw(texture,this.Position,new Rectangle(0,0,texture.Width,(int)(texture.Height -(percent*texture.Height))),Color.Pink,0,origin,scaling,SpriteEffects.None,1);

            pSpriteBatch.Draw(AssetManager.circleBoard,new Vector2(Position.X,cursorY),null,Color.White,0,new Vector2(AssetManager.circleBoard.Width/2,AssetManager.circleBoard.Height/2),new Vector2(0.2f,0.2f),SpriteEffects.None,1);
        
        }
    }
}