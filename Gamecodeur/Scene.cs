using System;
using System.Collections.Generic;
using System.Linq;
using GCMonogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Gamecodeur
{
    public abstract class Scene
    {
        protected MainGame mainGame;
        private List<IActor> listActors;
        private List<IActor> SortedActors; // copy of list actors sorted by z order (used for drawing)
        protected string name;

        public Scene(MainGame pGame)
        {
            this.mainGame = pGame;
            listActors = new List<IActor>();
            name = this.ToString();
        }

        public void addActor(IActor actor){
            listActors.Add(actor);
            SortedActors = listActors.OrderBy(o=>o.zOrder).ToList();
        }
        public void Clean(){
            listActors.RemoveAll(item => item.ToRemove == true);
        }
        public virtual void Load()
        {
          
        }
        public virtual void UnLoad()
        {
            MediaPlayer.Stop();
        }

        public virtual void Update(GameTime gameTime)
        { 
            for (int i = 0; i < listActors.Count; i++)
            {
                IActor actor = listActors[i];
                if (actor.isActive){
                    actor.Update(gameTime);
                }
            }
            Clean();
        }

        public virtual void Draw(GameTime gameTime)
        {

            
            foreach(IActor actor in SortedActors){
                if (actor.isActive){
                    actor.Draw(mainGame._spriteBatch);
                }
            }
            if (GameState.DEBUG){
                mainGame._spriteBatch.DrawString(AssetManager.MainFont,"SCENE:"+name,new Vector2(0,800),Color.White);
            }
        }
    }
}
