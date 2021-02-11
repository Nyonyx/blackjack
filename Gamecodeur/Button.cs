using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gamecodeur{

    public delegate void OnClick(Button pSender);

    public class Button : Sprite{

        public bool isHover{get;private set;}
        private MouseState oldMouseState;
        public OnClick onClick{get;set;}
        public string label {get;set;}
        public Sprite icon = null;

        public Button(Texture2D pTexture) : base(pTexture){
            label = "";
            scaling = new Vector2(0.4f,0.4f);
        }
        public Button(Texture2D pTexture,string pLabel,Vector2 pPosition,OnClick pOnClick) : base(pTexture){
            label = pLabel;
            scaling = new Vector2(0.4f,0.4f);
            Position = pPosition;
            onClick = pOnClick;
        }

        public void setIcon(Sprite pIcon){
            icon = pIcon;
        }

        

        public override void Update(GameTime pGameTime){

            MouseState newMouseState = Mouse.GetState();
            Point MousePos = newMouseState.Position;

            if (BoudingBox.Contains(MousePos)){
                if (!isHover){
                    isHover = true;
                }
            }else {
                if (isHover){
          
                }
                isHover = false;
            }

            if (isHover){
                if (newMouseState.LeftButton == ButtonState.Pressed &&
                 oldMouseState.LeftButton == ButtonState.Released){
                    if (onClick != null){
                        onClick(this);
                    }
                }
            }
            oldMouseState = newMouseState;
            base.Update(pGameTime);
        }

        public override void Draw(SpriteBatch pSpriteBatch)
        {
            base.Draw(pSpriteBatch);
            pSpriteBatch.DrawString(AssetManager.MainFont,label,Position,Color.White);
            if (icon != null){
                pSpriteBatch.Draw(icon.Texture,this.Position + (new Vector2(Texture.Width/2,Texture.Height/2)*this.scaling),null,Color.White,0,icon.origin,icon.scaling,icon.spriteEffect,1);
            }
        }
    }
}