using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gamecodeur{
    public class TextLabel{

        public Vector2 Position{get; private set; }
        public bool isCenter {get ; private set;}
        private Vector2 offset;
        private string label;

        public TextLabel(string pLabel){
            label = pLabel;
        }
        public void alignCenter(){
            isCenter = true;
        }

        public void Update(GameTime pGameTime)
        {
            if (isCenter){
                Vector2 size = AssetManager.MainFont.MeasureString(label);
                offset = size - new Vector2(size.X/2,size.Y/2);
            }
        }       
        public void Draw(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.DrawString(AssetManager.MainFont,label,Position-offset,Color.White);
        }
    }
}