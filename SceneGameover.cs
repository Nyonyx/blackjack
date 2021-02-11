using System;
using Gamecodeur;
using Microsoft.Xna.Framework;

namespace GCMonogame
{
    public class SceneGameover : Scene
    {
        public SceneGameover(MainGame pGame) : base(pGame)
        {
            Console.WriteLine("New Scene SceneGameover!");

        }
        public override void Load()
        {
            Console.WriteLine("SceneGameover.load !");
            base.Load();
        }
        public override void UnLoad()
        {
            Console.WriteLine("SceneGameover.Unload");
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            //mainGame._spriteBatch.DrawString(AssetManager.MainFont,"This is the gameOver !",new Vector2(1,1),Color.White);
            base.Draw(gameTime);
        }

    }
}
