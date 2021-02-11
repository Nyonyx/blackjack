using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gamecodeur{

    public interface IActor{
        Vector2 Position{get;}
        Rectangle BoudingBox {get;}
        void Update(GameTime pGameTime);
        void Draw(SpriteBatch pSpriteBatch);
        void TouchedBy(IActor pBy);
        bool ToRemove {get;set;}
        bool isActive {get;set;} // if isActive = false, we dont update and draw  
        int zOrder {get;set;} // order to draw the sprites
    }

}