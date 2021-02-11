using Microsoft.Xna.Framework;
using static GCMonogame.Hand;

namespace GCMonogame
{
    public class Croupier{
        public Hand hand{get; private set;}
        SceneGameplay board;
        public Vector2 position;
        public Croupier(SceneGameplay pBoard,Vector2 pPosition){
            this.board = pBoard;
            hand = new Hand(board,pPosition);
            
            position = pPosition;
            hand.position = pPosition;
            
        }
        public void tireCartes(){
            do{
                Card c = hand.addCard();
                c.isActive = false;
            }while(hand.score < 17);
        }

        public void giveStartCards(){
         
            Hand h = new Hand(board,board.player.position);
            Card c = h.addCard();
            Card c2 = h.addCard();
            Card cc = hand.addCard();
            c.isActive = false;
            c2.isActive = false;
            cc.isActive = false;

            h.setMise(board.potentialBet);
            board.player.addHand(h);
        }
        public void giveResult(Hand h){
            switch (h.state)
            {
                case Hand.State.BUSTED:
                    break;
                case Hand.State.EQUALS:
                    board.player.setMoney(board.player.money + board.player.lst_hands[0].mise);
                    break;
                case Hand.State.WIN:
                    board.player.setMoney(board.player.money + (board.player.lst_hands[0].mise*2));
                    break;
                default:
                break;
            }
        }

        public void compare(Hand pPlayerHAnd){
            if (pPlayerHAnd.state == State.PLAYING){

              
                if (hand.state == State.BUSTED){
                    pPlayerHAnd.setState(State.WIN);
                    return;
                }
                if (hand.state == State.WIN){
                    pPlayerHAnd.setState(State.BUSTED);
                    return;
                }

                if(hand.score > pPlayerHAnd.score){
                    pPlayerHAnd.setState(State.BUSTED);
                }else if(hand.score == pPlayerHAnd.score){
                    pPlayerHAnd.setState(State.EQUALS);
                }else if(hand.score < pPlayerHAnd.score){
                    pPlayerHAnd.setState(State.WIN);
                }
            }
        }
    }
}