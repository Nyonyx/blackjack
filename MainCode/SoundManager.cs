using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace GCMonogame
{
    public class SoundManager{

        public static SoundEffect[] snd_card_slap = new SoundEffect[7];

        public static void Load(ContentManager pContent){
            for (int i = 0; i < snd_card_slap.Length; i++)
            {
                snd_card_slap[i] = pContent.Load<SoundEffect>("sounds/card_slap_"+i);
            }
            
        }
    }
}