using System;
using Gamecodeur;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using static Gamecodeur.GameState;

namespace GCMonogame
{
    public class SceneMenu : Scene
    {
        private KeyboardState oldKBState;
        private Button myButton;


        public SceneMenu(MainGame pGame) : base(pGame)
        {
            Console.WriteLine("New Scene Menu !");

        }
        public void onClickPlay(Button pSender){
            mainGame.gameState.ChangeScene(GameState.SceneType.GamePlay);
        }

        public override void Load()
        {
            Console.WriteLine("SceneMenu.load !");

            oldKBState = Keyboard.GetState();
            

            myButton = new Button(mainGame.Content.Load<Texture2D>("UI/button"));
            myButton.Position = new Vector2((Screen.Width/2)-(myButton.Texture.Width/2),
            (Screen.Height/2)-(myButton.Texture.Height/2));

            myButton.onClick = onClickPlay;
            addActor(myButton);

            base.Load();
        }
        public override void UnLoad()
        {
            Console.WriteLine("SceneMenu.Unload");
            
            base.UnLoad();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState newKBState = Keyboard.GetState();
            if(newKBState.IsKeyDown(Keys.Enter) && !oldKBState.IsKeyDown(Keys.Enter)){
                mainGame.gameState.ChangeScene(SceneType.GamePlay);
                Console.WriteLine("Changement de scene !");
            }
            oldKBState = newKBState;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //mainGame._spriteBatch.DrawString(AssetManager.MainFont,"This is the menu !",new Vector2(1,1),Color.White);
            base.Draw(gameTime);
        }

    }
}
