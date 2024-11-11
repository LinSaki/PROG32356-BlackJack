using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackStarter.Blackjack
{
    class Card
    {
        public int Rank;
        public string Suit;

        public Card(int Rank, string Suit)
        {
            this.Rank = Rank;
            this.Suit = Suit;
        }


        public int GetBlackjackValue()
        { //set value of rank
            if (Rank == 1)
            {
                return 11; // Ace can be 11 or 1
            }
            else if (Rank >= 2 && Rank <= 10)
            {
                return Rank; // ranks from 2 to 10 are its set value
            }
            else
            { //Else if they are face cards (11 (jack), 12 (queen), 13(king))
                return 10;
            }
        }

        public string GetImageFilename()
        {
            string RankName;
            if (Rank == 1)
            {
                RankName = "A";
            }
            else if (Rank == 11)
            {
                RankName = "J";
            }
            else if (Rank == 12)
            {
                RankName = "Q";
            }
            else if (Rank == 13)
            {
                RankName = "K";
            }
            else
            {
                RankName = Rank.ToString();
            }

            string SuitName;
            if (Suit == "Hearts")
            {
                SuitName = "H";
            }
            else if (Suit == "Diamonds")
            {
                SuitName = "D";
            }
            else if (Suit == "Clubs")
            {
                SuitName = "C";
            }
            else
            {
                SuitName = "S";
            }

            return RankName + SuitName + ".jpg";
            //image of that card assuming they are all named "2D.jpg", rank and then suit
        }
    }

}
