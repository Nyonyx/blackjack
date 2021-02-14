
using System;
using System.Runtime.Serialization;
using GCMonogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gamecodeur{

    public class Sprite
    {
        //IACTOR
        public Vector2 Position{get;set;}
        public Rectangle BoudingBox{get; private set;}
        public Vector2 origin;
        public float vx;
        public float vy;
        public bool ToRemove { get ; set ; }
        
        //Sprite
        public Texture2D Texture {get;private set;}
        public bool isActive{get;set;}
        public int zOrder { get; set;}
        public SpriteEffects spriteEffect = SpriteEffects.None;

        public Sprite(Texture2D pTexture){
            this.Texture = pTexture;
            ToRemove = false;
            origin = new Vector2(0,0);
            isActive = true;
            zOrder = 0;
        }

        public Sprite(Texture2D pTexture,Vector2 pPosition){
            this.Texture = pTexture;
            ToRemove = false;
            origin = new Vector2(0,0);
            isActive = true;
            zOrder = 0;         
            Position = pPosition;  
        }
        public void setTexture(Texture2D pTexture){
            this.Texture = pTexture;
        }
        public void setSpriteEffect(SpriteEffects pS){
            this.spriteEffect = pS;
        }

        public void Move(float pX,float pY){
            
            Position = new Vector2(Position.X + pX, Position.Y + pY);
        }

        public virtual void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(Texture,this.Position,null,Color.White,0,origin,new Vector2(1,1),spriteEffect,1);
        }

        public virtual void Update(GameTime pGameTime)
        {
           Move(vx,vy);   
           BoudingBox = new Rectangle((int)Position.X,(int)Position.Y,Texture.Width,Texture.Height);
           //this.BoudingBox = new Rectangle(300,300,100,100);
        }
    }

}