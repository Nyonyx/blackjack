using System;
using System.Collections.Generic;
using Gamecodeur;
using Microsoft.Xna.Framework;

namespace GCMonogame
{
    //Hand: class only for logic, not for drawing
    public class Hand{
        public List<Card> lst_cards;
 
        public Hand(Vector2 pPosition) {
           
        }
        // return number of points of hand
        public int examine(){
            int points = 0;
            // ajoute les points des cartes sauf les as
            foreach (var card in lst_cards)
            {
                if((int)card.number <= 10){
                    points += (int)card.number;
                }
                else if ((int)card.number > 10 && (int)card.number < 14 ){
                    points += 10;
                }
            }
            // traitement des as
            foreach (var card in lst_cards)
            {
                if (card.number == cardNumber.ace){
                    // as vaut 1 ou 11
                    if (points + 1 == 21 || points + 11 > 21){
                        points += 1;
                    }else if (points + 11 == 21 || points + 11 < 21){
                        points += 11;
                    }
                }
            }
            return points;
        }        
    }
}