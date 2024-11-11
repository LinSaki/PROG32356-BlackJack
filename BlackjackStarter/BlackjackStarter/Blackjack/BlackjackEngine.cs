using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace BlackJackStarter.Blackjack
{
    internal class BlackjackEngine
    {
        private string[] SuitsList = { "Hearts", "Clubs", "Spades", "Diamonds" };
        private int[] RanksList = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };

        public List<Card> Deck = new List<Card>();

        public List<Card> PlayerHand = new List<Card>();

        public List<Card> DealerHand = new List<Card>();

        public void Init() {
            // Initialize the deck
            Deck.Clear();
            
            foreach (string suit in SuitsList)
            {
                foreach (int rank in RanksList)
                {
                    Deck.Add(new Card(rank, suit));
                }
            }
            ShuffleDeck();
        }

        public void PrintDeck() {
            foreach (var card in Deck)
            {
                Trace.WriteLine(card.Rank + " " + card.Suit); //trace. for debug
            }
            Trace.WriteLine("Number of cards: " + Deck.Count);
        }

        public void ShuffleDeck() {
            Random random = new Random();
            int deckSize = Deck.Count;

            while (deckSize > 1) { 
                deckSize--;
                int x = random.Next(deckSize + 1); // index between 0 and 51
                Card value = Deck[x]; //random card at x
                Deck[x] = Deck[deckSize]; //swap random card spot with the last card in deck
                Deck[deckSize] = value; //last card is now that random card
            }
        }

        public void BeginGame() {
            //New hand of cards
            PlayerHand.Clear();
            DealerHand.Clear();

            DealCard(DealerHand);

            DealCard(PlayerHand);
            DealCard(PlayerHand);
        }

        public void DealCard(List<Card> hand) //extract one card from the deck
        {
            if (Deck.Count == 0)
            {
                Trace.WriteLine("No more cards in the deck! Can't deal card");
            }
            else 
            {
                //grab card from shuffled deck
                Card dealtcard = Deck[0]; //grab first card
                Deck.RemoveAt(0);
                hand.Add(dealtcard);
            }
        }

        public int HandValue(List<Card> hand)
        {
            int totalValue = 0;
            int aceCount = 0; //cant have more than 4 aces in a hand

            foreach (var card in hand) 
            {
                totalValue += card.GetBlackjackValue();
                if (card.Rank == 1)
                {
                    aceCount++;
                }
            }

            while (totalValue > 21 && aceCount > 0) //if value of ace goes over 21, change the original ace(s) to 1
            {
                totalValue -= 10;
                aceCount--;
            }

            return totalValue;
        }

        public bool IsPlayerBusted(List<Card> hand)
        {
            if(HandValue(hand) > 21)
            {
                return true;
            }
            return false;
        }

        public bool IsBlackjack(List<Card> hand)
        {
            if(HandValue(hand) == 21)
            {
                return true;
            }
            return false;
        }

        public void DealersTurn(Panel dealerPanel)
        {
            dealerPanel.Children.RemoveAt(dealerPanel.Children.Count - 1); //remove last card in panel (flipped image)
            DealCard(DealerHand);
        }

        public void CheckWinner()
        {
            // logic to check if player wins/loses
            if (HandValue(DealerHand) > HandValue(PlayerHand))
            {
                MessageBox.Show("Dealer's Hand: " + HandValue(DealerHand) + " Player's's Hand: " + HandValue(PlayerHand)
                    + "\nDealer has the better hand! YOU LOSE, thanks for playing!");
            }
            else if (HandValue(DealerHand) < HandValue(PlayerHand))
            {
                MessageBox.Show("Dealer's Hand: " + HandValue(DealerHand) + " Player's's Hand: " + HandValue(PlayerHand)
                    + "\nPlayer has the better hand! YOU WIN, thanks for playing!");
            }
            else //tied
            {
                MessageBox.Show("Dealer's Hand: " + HandValue(DealerHand) + " Player's's Hand: " + HandValue(PlayerHand)
                    + "\nIT'S A TIE! Thanks for playing!");
            }
        }


    }
}
