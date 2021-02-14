
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SpriteFontPlus;

namespace Gamecodeur
{
    public class AssetManager
    {

        public static SpriteFont MainFont {get;private set;}
        public static Texture2D imgCard;

        // background sprites
        public static Texture2D[] table = new Texture2D[10];
        public static Texture2D textLines;
        public static Texture2D[] leather = new Texture2D[15];
        public static Texture2D cardBox;
        public static Texture2D[] background = new Texture2D[7];
        public static Texture2D chips;

        public static Texture2D circleBoard;
        public static Texture2D button1;
        public static Texture2D button2;
        public static Texture2D blueBG;
        public static Texture2D settingsBtn;
        public static Texture2D starIcon;

        public static void Load(ContentManager pContent, GraphicsDevice pGraphicsDevice){
            var fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes("Content/PixelMaster.ttf"), 60, 1024, 1024, new[]
            {CharacterRange.BasicLatin,CharacterRange.Latin1Supplement,CharacterRange.LatinExtendedA,CharacterRange.Cyrillic});

            MainFont = fontBakeResult.CreateSpriteFont(pGraphicsDevice);
            imgCard =  pContent.Load<Texture2D>("table/allcards");    
            cardBox = pContent.Load<Texture2D>("table/cardBox");
            chips = pContent.Load<Texture2D>("table/chips");
            button1 = pContent.Load<Texture2D>("UI/button1");
            button2 = pContent.Load<Texture2D>("UI/button2");
            blueBG = pContent.Load<Texture2D>("UI/blueBG");
            settingsBtn = pContent.Load<Texture2D>("UI/settingsBtn");
            starIcon = pContent.Load<Texture2D>("UI/starIcon");

            for (int i = 0; i < table.Length; i++)
            {
                table[i] = pContent.Load<Texture2D>("table/table"+(i+1));
            }
            for (int i = 0; i < leather.Length; i++)
            {
                leather[i] = pContent.Load<Texture2D>("leather/Leather"+(i+1));
            }
            for (int i = 0; i < background.Length; i++)
            {
                background[i] = pContent.Load<Texture2D>("background/bg"+(i+1));
            }

            textLines = pContent.Load<Texture2D>("table/textLines");
            circleBoard = pContent.Load<Texture2D>("UI/borderCircle");
        }
    }
}