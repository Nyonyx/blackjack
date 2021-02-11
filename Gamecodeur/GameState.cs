using System;
using GCMonogame;
using Microsoft.Xna.Framework;

namespace Gamecodeur
{
    public class GameState
    {
        public enum SceneType
        {
            Menu,
            GamePlay,
            GameOver,
        }

        protected MainGame mainGame;
        public Scene CurrentScene { get; set; }
        public static Rectangle Screen;
        public static float scalingTable = 0.6f;
        public static bool DEBUG = false;

        public GameState(MainGame pGame)
        {
            this.mainGame = pGame;
            Screen = mainGame.Window.ClientBounds;
        }

        public void ChangeScene(SceneType pSceneType)
        {
            if (CurrentScene != null)
            {
                CurrentScene.UnLoad();
                CurrentScene = null;
            }

            switch (pSceneType)
            {
                case SceneType.Menu:
                    CurrentScene = new SceneMenu(mainGame);
                    break;
                case SceneType.GamePlay:
                    CurrentScene = new SceneGameplay(mainGame);
                    break;
                case SceneType.GameOver:
                    CurrentScene = new SceneGameover(mainGame);
                    break;
                default:
                    break;
            }
            CurrentScene.Load();
        }
    }
}
