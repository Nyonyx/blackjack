using System;
using GCMonogame;
using Microsoft.Xna.Framework;

namespace Gamecodeur
{
    public static class GameState
    {
        public enum SceneType
        {
            Menu,
            GamePlay,
            GameOver,
        }
        public static Rectangle Screen;
        public static float scalingTable = 0.6f;
        public static bool DEBUG = false;

    }
}
