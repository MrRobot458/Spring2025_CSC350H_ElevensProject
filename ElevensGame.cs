using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=================================");
        Console.WriteLine("  Welcome to Elevens Card Game!");
        Console.WriteLine("=================================");
        Console.WriteLine("\nRules:");
        Console.WriteLine("- Remove pairs of cards that add up to 11");
        Console.WriteLine("- Remove J, Q, K as a set of three");
        Console.WriteLine("- Win by removing all cards from the deck!\n");
        
        PlayGame();
    }

    static void PlayGame()
    {
        // Initialize deck and shuffle
        Deck deck = new Deck();
        deck.Shuffle();
        
        // Create the 2x4 layout (8 cards)
        Card[] layout = new Card[8];
        
        // Deal initial 8 cards
        for (int i = 0; i < 8; i++)
        {
            layout[i] = deck.TakeTopCard();
            if (layout[i] != null)
            {
                layout[i].FlipOver();
            }
        }
        
        // Game loop
        bool gameWon = false;
        bool gameLost = false;
        
        while (!gameWon && !gameLost)
        {
            // Display the layout
            DisplayLayout(layout, deck.RemainingCards);
            
            // Check if any moves are available (regardless of deck status)
            if (!HasValidMoves(layout))
            {
                gameLost = true;
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║  No valid moves available!             ║");
                Console.WriteLine("║  GAME OVER - You Lose!                 ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                break;
            }
            
            // Check win condition
            if (AllCardsRemoved(layout) && deck.RemainingCards == 0)
            {
                gameWon = true;
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║  🎉 Congratulations! You Won! 🎉       ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
                break;
            }
            
            // Get user input
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Remove a pair that adds to 11 (enter two positions)");
            Console.WriteLine("2. Remove J-Q-K set (enter three positions)");
            Console.WriteLine("3. Quit");
            Console.Write("\nYour choice: ");
            
            string choice = Console.ReadLine();
            
            if (choice == "1")
            {
                RemovePair(layout, deck);
            }
            else if (choice == "2")
            {
                RemoveFaceCards(layout, deck);
            }
            else if (choice == "3")
            {
                Console.WriteLine("Thanks for playing!");
                return;
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
        
        // Ask to play again
        Console.Write("\nWould you like to play again? (y/n): ");
        string playAgain = Console.ReadLine();
        if (playAgain?.ToLower() == "y")
        {
            Console.Clear();
            PlayGame();
        }
    }
    
    static void DisplayLayout(Card[] layout, int remainingCards)
    {
        Console.WriteLine("\n=================================");
        Console.WriteLine($"Cards remaining in deck: {remainingCards}");
        Console.WriteLine("=================================\n");
        
        // Display positions 0-3 (top row)
        Console.WriteLine("Top Row:");
        for (int i = 0; i < 4; i++)
        {
            if (layout[i] != null)
            {
                Console.WriteLine($"  [{i}] {layout[i].ToString()} (Value: {layout[i].GetValue()})");
            }
            else
            {
                Console.WriteLine($"  [{i}] [Empty]");
            }
        }
        
        Console.WriteLine("\nBottom Row:");
        // Display positions 4-7 (bottom row)
        for (int i = 4; i < 8; i++)
        {
            if (layout[i] != null)
            {
                Console.WriteLine($"  [{i}] {layout[i].ToString()} (Value: {layout[i].GetValue()})");
            }
            else
            {
                Console.WriteLine($"  [{i}] [Empty]");
            }
        }
        Console.WriteLine();
    }
    
    static void RemovePair(Card[] layout, Deck deck)
    {
        Console.Write("Enter first card position (0-7): ");
        if (!int.TryParse(Console.ReadLine(), out int pos1) || pos1 < 0 || pos1 > 7)
        {
            Console.WriteLine("Invalid position!");
            return;
        }
        
        Console.Write("Enter second card position (0-7): ");
        if (!int.TryParse(Console.ReadLine(), out int pos2) || pos2 < 0 || pos2 > 7)
        {
            Console.WriteLine("Invalid position!");
            return;
        }
        
        if (layout[pos1] == null || layout[pos2] == null)
        {
            Console.WriteLine("One or both positions are empty!");
            return;
        }
        
        if (pos1 == pos2)
        {
            Console.WriteLine("You must select two different cards!");
            return;
        }
        
        int value1 = layout[pos1].GetValue();
        int value2 = layout[pos2].GetValue();
        
        if (value1 + value2 == 11)
        {
            Console.WriteLine($"Success! {layout[pos1].ToString()} + {layout[pos2].ToString()} = 11");
            layout[pos1] = null;
            layout[pos2] = null;
            
            // Replace with new cards from deck
            ReplaceCards(layout, deck);
        }
        else
        {
            Console.WriteLine($"Invalid pair! {value1} + {value2} = {value1 + value2}, not 11.");
        }
    }
    
    static void RemoveFaceCards(Card[] layout, Deck deck)
    {
        Console.Write("Enter first card position (0-7): ");
        if (!int.TryParse(Console.ReadLine(), out int pos1) || pos1 < 0 || pos1 > 7)
        {
            Console.WriteLine("Invalid position!");
            return;
        }
        
        Console.Write("Enter second card position (0-7): ");
        if (!int.TryParse(Console.ReadLine(), out int pos2) || pos2 < 0 || pos2 > 7)
        {
            Console.WriteLine("Invalid position!");
            return;
        }
        
        Console.Write("Enter third card position (0-7): ");
        if (!int.TryParse(Console.ReadLine(), out int pos3) || pos3 < 0 || pos3 > 7)
        {
            Console.WriteLine("Invalid position!");
            return;
        }
        
        if (layout[pos1] == null || layout[pos2] == null || layout[pos3] == null)
        {
            Console.WriteLine("One or more positions are empty!");
            return;
        }
        
        if (pos1 == pos2 || pos1 == pos3 || pos2 == pos3)
        {
            Console.WriteLine("You must select three different cards!");
            return;
        }
        
        // Check if we have J, Q, K
        List<Rank> ranks = new List<Rank> 
        { 
            layout[pos1].Rank, 
            layout[pos2].Rank, 
            layout[pos3].Rank 
        };
        
        bool hasJack = ranks.Contains(Rank.Jack);
        bool hasQueen = ranks.Contains(Rank.Queen);
        bool hasKing = ranks.Contains(Rank.King);
        
        if (hasJack && hasQueen && hasKing)
        {
            Console.WriteLine($"Success! Removed J-Q-K set!");
            layout[pos1] = null;
            layout[pos2] = null;
            layout[pos3] = null;
            
            // Replace with new cards from deck
            ReplaceCards(layout, deck);
        }
        else
        {
            Console.WriteLine("Invalid set! You need exactly one Jack, one Queen, and one King.");
        }
    }
    
    static void ReplaceCards(Card[] layout, Deck deck)
    {
        for (int i = 0; i < layout.Length; i++)
        {
            if (layout[i] == null && deck.RemainingCards > 0)
            {
                layout[i] = deck.TakeTopCard();
                if (layout[i] != null)
                {
                    layout[i].FlipOver();
                }
            }
        }
    }
    
    static bool HasValidMoves(Card[] layout)
    {
        // Check for pairs that add to 11
        for (int i = 0; i < layout.Length; i++)
        {
            if (layout[i] == null) continue;
            
            for (int j = i + 1; j < layout.Length; j++)
            {
                if (layout[j] == null) continue;
                
                if (layout[i].GetValue() + layout[j].GetValue() == 11)
                {
                    return true;
                }
            }
        }
        
        // Check for J-Q-K set
        bool hasJack = false;
        bool hasQueen = false;
        bool hasKing = false;
        
        for (int i = 0; i < layout.Length; i++)
        {
            if (layout[i] == null) continue;
            
            if (layout[i].Rank == Rank.Jack) hasJack = true;
            if (layout[i].Rank == Rank.Queen) hasQueen = true;
            if (layout[i].Rank == Rank.King) hasKing = true;
        }
        
        return hasJack && hasQueen && hasKing;
    }
    
    static bool AllCardsRemoved(Card[] layout)
    {
        foreach (Card card in layout)
        {
            if (card != null) return false;
        }
        return true;
    }
}
