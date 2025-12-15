using System;

public class Card
{
    private Rank rank;
    private Suit suit;
    private bool isFaceUp;

    // Card constructor
    public Card(Suit suit, Rank rank)
    {
        this.suit = suit;
        this.rank = rank;
        this.isFaceUp = false;
    }

    // Properties for all of the above fields
    public Suit Suit
    {
        get
        {
            return suit;
        }
    }
    public Rank Rank 
    {
        get
        {
            return rank;
        }
    }
    public bool IsFaceUp
    {
        get
        {
            return isFaceUp;
        }
    }

    // Gets the value of the card
    public int GetValue()
    {
        if (rank == Rank.Jack || rank == Rank.Queen || rank == Rank.King)
            return 10;
        else if (rank == Rank.Ace)
            return 1;
        else
            return (int)rank + 1;
    }

    public void FlipOver()
    {
        isFaceUp = !isFaceUp;
    }

    // Overrides ToString for debugging
    public override string ToString()
    {
        return $"{rank} of {suit}";
    }
}
