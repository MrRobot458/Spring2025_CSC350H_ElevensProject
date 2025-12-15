using System;
using System.Collections.Generic;

public class Deck
{
    private List<Card> cards = new List<Card>();

    // Deck constructor
    public Deck()
    {
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                // Create new card and add to back
                Card newCard = new Card(suit, rank);
                cards.Add(newCard);
            }
        }
    }

    public int RemainingCards
    {
        get
        {
            return cards.Count;
        }
    }

    // Property to get access to Cards
    public List<Card> Cards
    {
        get
        {
            return cards;
        
        }
    }

    // Takes top card from deck
    public Card TakeTopCard()
    {
        if (cards.Count == 0) 
        {
            return null;
        }
        else
        {
            Card topCard = cards[0];
            cards.RemoveAt(0);
            return topCard;
        }
    }

    // Shuffles the cards
    public void Shuffle()
    {
        Random rngCard = new Random();

        // List to hold the shuffled deck:
        List<Card> shuffledDeck = new List<Card>();

        while (cards.Count > 0)
        {
            int rngIndex = rngCard.Next(cards.Count);
            Card selectedCard = cards[rngIndex];
            shuffledDeck.Add(selectedCard);

            cards.RemoveAt(rngIndex);
        }
        cards = shuffledDeck;
    }

    // Cut the deck
    public void Cut(int index)
    {
        // Check for invalid index
        if (index <= 0 || index >= cards.Count)
        {
            return;
        }
        List<Card> topPortion = cards.GetRange(0, index);
        cards.RemoveRange(0, index);
        cards.AddRange(topPortion);
    }
}
