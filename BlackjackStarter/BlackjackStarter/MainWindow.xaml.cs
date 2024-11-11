using BlackJackStarter.Blackjack;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackjackStarter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlackjackEngine game = new BlackjackEngine();
        string path = "C:/Sheridan/2024Winter/SEMESTER 4/PROG32356 - .NET Technologies using C#/BlackjackStarter/BlackjackStarter/BlackjackStarter/JPEG/";
    public MainWindow()
    {
        InitializeComponent();
        MessageBox.Show("Welcome to Blackjack!");
        StartNewGame();
    }

    private void StartNewGame()
        {
            game.Init(); // Initialize deck of cards
            game.BeginGame(); // Deal new set of cards

            RefreshScreen("dealer");
            ShowCard(path + "purple_back.jpg", DealerPanel); // Add flipped card
            RefreshScreen("player");
            UpdateHandValueLabels();
            
            CheckForInitialBlackjack();
        }

        private void CheckForInitialBlackjack() // IF the player has blackjack
        {
            if (game.IsBlackjack(game.PlayerHand))
            {
                game.DealersTurn(DealerPanel);
                RefreshScreen("dealer");
                UpdateHandValueLabels();

                if (game.IsBlackjack(game.DealerHand))
                {
                    game.CheckWinner();
                    ReplayGame();
                    return;
                }
                else
                {
                    MessageBox.Show("BLACKJACK! You win, thanks for playing!");
                    ReplayGame();
                    return;
                }
            }
        }

        private void UpdateHandValueLabels()
        {
            PlayerHandValueLabel.Content = "Player Hand Value: " + game.HandValue(game.PlayerHand);
            DealerHandValueLabel.Content = "Dealer Hand Value: " + game.HandValue(game.DealerHand);
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            game.DealCard(game.PlayerHand);

            RefreshScreen("player");// display new card
            UpdateHandValueLabels();

            if (game.IsPlayerBusted(game.PlayerHand))
            {
                MessageBox.Show("BUST! You lose.");
                ReplayGame();
                return;
            }
            else if (game.IsBlackjack(game.PlayerHand))
            {
                CheckForInitialBlackjack();
            }

        }

        public void RefreshScreen(string player) // wipe out all the cards & display each card in player hand it displays
        {
            Panel panelToUpdate = (player == "player") ? PlayerPanel : DealerPanel;
            List<Card> handToUpdate = (player == "player") ? game.PlayerHand : game.DealerHand;

            panelToUpdate.Children.Clear();

            foreach (Card c in handToUpdate)
            {
                ShowCard(path + c.GetImageFilename(), panelToUpdate);
            }
        }

        private void ShowCard(string filename, Panel panel)
        {
            // Create a BitmapImage object
            BitmapImage cardBitmap = new BitmapImage(
                new Uri(filename));

            // Create a new Image control and set its source
            Image cardImage = new Image();
            cardImage.Source = cardBitmap;
            cardImage.Height = 145;
            cardImage.Width = 171;
            cardImage.Margin = new Thickness(7);

            // Add the new Image control to the specified WrapPanel
            panel.Children.Add(cardImage);
        }

        private void Stand_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Dealer's Turn!");

            game.DealersTurn(DealerPanel);
            RefreshScreen("dealer");
            UpdateHandValueLabels();

            if (game.IsBlackjack(game.DealerHand))
            {
                MessageBox.Show("Dealer has BLACKJACK! You LOSE, thanks for playing!");
                ReplayGame();
                return; // Exit the method 
            }

            while (game.HandValue(game.DealerHand) < 17) // dealer hand value < 17 = HIT
            {
                game.DealCard(game.DealerHand); // Hit
                RefreshScreen("dealer");
                UpdateHandValueLabels();

                if(IsDealerBlackjackOrBust()) //if blackjack or bust is TRUE
                {
                    return;
                }
            }
            game.CheckWinner(); // if hand over 17, check winner
            ReplayGame();
        }

        private bool IsDealerBlackjackOrBust()
        {
            if (game.IsPlayerBusted(game.DealerHand))
            {
                MessageBox.Show("Dealer BUST! You WIN, thanks for playing!");
                ReplayGame();
                return true; 
            }
            else if (game.IsBlackjack(game.DealerHand))
            {
                MessageBox.Show("Dealer has BLACKJACK! You LOSE, thanks for playing");
                ReplayGame();
                return true; 
            }
            return false; //not blackjack or bust
        }

        public void ReplayGame()
        {
            if (MessageBox.Show("Would you like to play again?",
                    "Play Blackjack Again?",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                StartNewGame(); // Restart the game
            }
            else
            {
                MessageBox.Show("Goodbye!");
                System.Windows.Application.Current.Shutdown(); //close main window
            }
        }
    }
}