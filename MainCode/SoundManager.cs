using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace GCMonogame
{
    public class SoundManager{

        public static SoundEffect[] snd_card_slap = new SoundEffect[7];
        public static SoundEffect snd_soft_win;
        public static SoundEffect snd_equals;
        public static SoundEffect snd_busted;
        public static SoundEffect snd_applause;
        public static Song song_casino_ambiance;

        public static void Load(ContentManager pContent){

            song_casino_ambiance = pContent.Load<Song>("sounds/casino_ambiance");
            snd_soft_win = pContent.Load<SoundEffect>("sounds/soft_win");
            snd_equals = pContent.Load<SoundEffect>("sounds/tap");
            snd_busted = pContent.Load<SoundEffect>("sounds/busted");
            snd_applause = pContent.Load<SoundEffect>("sounds/applause");

            for (int i = 0; i < snd_card_slap.Length; i++)
            {
                snd_card_slap[i] = pContent.Load<SoundEffect>("sounds/card_slap_"+i);
            }
            
        }
    }
}