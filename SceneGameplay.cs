using System;
using System.Collections.Generic;
using System.Windows;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static Gamecodeur.GameState;

namespace GCMonogame
{


    public class SceneGameplay : Scene
    {

        public SceneGameplay(MainGame pGame) : base(pGame)
        {
            init();
        }

        public void init(){
            //Test Commit
            
        }

        public override void Load()
        {
            base.Load();
        }
        public override void UnLoad()
        {
            Console.WriteLine("SceneGameplay.Unload");
            base.UnLoad();
        }


        public override void Update(GameTime gameTime)
        {
      
            base.Update(gameTime);
        }


        public override void Draw(GameTime gameTime)
        {
            // Draw sprites & stuff..
            base.Draw(gameTime);
        }
    }
}
