using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Gamecodeur{

    public delegate void OnClick(Button pSender);

    public class Button{

        public bool isHover{get;private set;}
        private MouseState oldMouseState;
        public OnClick onClick{get;set;}
        public string label {get;set;}
        public Sprite icon = null;
        public Texture2D texture;
        public Texture2D texturePressed;
        public Texture2D currentTexture;

        public Vector2 Position;
        public Rectangle BoudingBox;
        public Vector2 origin;
        private Vector2 scaling;
        private bool can_use {get ; set ;}

        public Button(Texture2D pTexture,Texture2D pTexturePressed,string pLabel,Vector2 pPosition,Vector2 pOrigin,Vector2 pScaling,OnClick pOnClick){
            texture = pTexture;
            texturePressed = pTexturePressed;
            currentTexture = texture;
            label = pLabel;
            onClick = pOnClick;
            Position = pPosition;
            origin = pOrigin;
            scaling = pScaling;
            BoudingBox = new Rectangle((int)(Position.X-(origin.X*scaling.X)),
            (int)(Position.Y-(origin.Y*scaling.Y)),(int)(texture.Width*scaling.X),
            (int)(texture.Height*scaling.Y));
            can_use = true;
        }

        public void setActive(bool pActive){
            can_use = pActive;
        }

        public void setIcon(Sprite pIcon){
            icon = pIcon;
        }

        public void Update(GameTime pGameTime){
            if (can_use){
                MouseState newMouseState = Mouse.GetState();
                Point MousePos = newMouseState.Position;

                if (BoudingBox.Contains(MousePos)){
                    if (!isHover){
                        isHover = true;
                    }
                }else {
                    isHover = false;
                }
                currentTexture = texture;
                if (isHover){
                    if (newMouseState.LeftButton == ButtonState.Pressed &&
                    oldMouseState.LeftButton == ButtonState.Released){
                        if (onClick != null){
                            onClick(this);
                        }
                    }
                    if (newMouseState.LeftButton == ButtonState.Pressed){
                        currentTexture = texturePressed;
                    }
                }
                oldMouseState = newMouseState;
            }
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
            Vector2 press; 
            if (currentTexture == texture){
                press = new Vector2(0,0);
            }else{
                press = new Vector2(0,(texture.Height*scaling.Y)/10);
            }

            Color drawColor = Color.White;
            if (!can_use){drawColor = Color.DarkGray;}

            pSpriteBatch.Draw(currentTexture,Position+press,null,drawColor,0,origin,scaling,SpriteEffects.None,1);
            Vector2 size = AssetManager.MainFont.MeasureString(label);
            pSpriteBatch.DrawString(AssetManager.MainFont,label,Position+press,drawColor,0,new Vector2(size.X/2,size.Y/2),scaling*2,SpriteEffects.None,1);
            //if (icon != null){
                //pSpriteBatch.Draw(icon.Texture,Position,null,Color.White,0,icon.origin,icon.scaling,icon.spriteEffect,1);
            //}
        }
    }
}