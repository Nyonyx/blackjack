using System;

namespace Gamecodeur{

    class Util{

        static Random RandomGen = new Random(System.DateTime.Now.Millisecond);
        public static void SetRandomSeed(int pSeed){
            RandomGen = new Random(pSeed);
        }

        public static int GetRandomInt(int pMin, int pMax){
            return RandomGen.Next(pMin,pMax + 1);
        }
    
        // Distance between two points
        //function math.dist(x1,y1, x2,y2) return ((x2-x1)^2+(y2-y1)^2)^0.5 end
        public static float dist(float x1,float y1, float x2,float y2){
            return (float)Math.Pow((Math.Pow(x2-x1,2.0f) + Math.Pow(y2-y1,2.0f)),0.5f);     
            
        }

        //-- Returns the angle between two points.
        //function math.angle(x1,y1, x2,y2) return math.atan2(y2-y1, x2-x1) end
        public static float angle(float x1,float y1, float x2,float y2){
            return (float)Math.Atan2(y2-y1,x2-x1);
        }


    }

}