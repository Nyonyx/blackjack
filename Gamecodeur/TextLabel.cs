using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gamecodeur{
    public class TextLabel : IActor{

        string label;
        public TextLabel(string pLabel){
            label = pLabel;
            isActive = true;
        }

        public Vector2 Position{get;set;}
        private Vector2 offset;

        public Rectangle BoudingBox{get; private set;}

        public bool ToRemove { get; set; }
        public bool isActive { get; set; }
        public int zOrder { get; set; }
        public bool isCenter {get;set;}

        public void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.DrawString(AssetManager.MainFont,label,Position-offset,Color.White);
        }

        public void TouchedBy(IActor pBy)
        {
            
        }

        public void Update(GameTime pGameTime)
        {
            if (isCenter){
                Vector2 size = AssetManager.MainFont.MeasureString(label);
                offset = size - new Vector2(size.X/2,size.Y/2);
            }
        }
    }
}