using System;
using System.Collections.Generic;
using Gamecodeur;
using Microsoft.Xna.Framework;

namespace GCMonogame
{
    public class Player
    {
        SceneGameplay board;
        public List<Hand> lst_hands {get ; private set;}
        public int money {get ; private set;}
        public Vector2 position;
        
        public Player(SceneGameplay pBoard,Vector2 pPosition){
            this.board = pBoard;
            lst_hands = new List<Hand>();
            position = pPosition;
            money = 1000;
        }
        public void setMoney(int pMoney){
            money = pMoney;
        }

        public void hit(Button pButton){
         
        }
    }
}