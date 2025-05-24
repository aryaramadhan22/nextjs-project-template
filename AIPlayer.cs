using UnityEngine;
using System.Collections.Generic;

public class AIPlayer : AbstractPlayer
{
    [SerializeField]
    private Transform cardSpawnPoint;
    private int difficultyLevel = 1;
    private float skillUseThreshold = 0.7f;

    public override void UseSkill(Deck deck)
    {
        if (!CanUseSkill()) return;

        // AI skill: Analyze and manipulate deck based on current situation
        int currentValue = GetHandValue();
        
        // Different strategies based on difficulty level
        switch (difficultyLevel)
        {
            case 1: // Easy
                if (currentValue < 15)
                {
                    deck.ShuffleTopCards(3); // Randomly shuffle top 3 cards
                }
                break;
                
            case 2: // Medium
                if (currentValue < 20)
                {
                    List<Card> topCards = deck.PeekTopCards(3);
                    // Find highest value card and move it to top
                    int highestValue = 0;
                    int highestIndex = 0;
                    
                    for (int i = 0; i < topCards.Count; i++)
                    {
                        if (topCards[i].Value > highestValue)
                        {
                            highestValue = topCards[i].Value;
                            highestIndex = i;
                        }
                    }
                    
                    deck.SwapCardToTop(highestIndex);
                }
                break;
                
            case 3: // Hard
                if (currentValue < 25)
                {
                    List<Card> topCards = deck.PeekTopCards(3);
                    // Calculate optimal card based on current hand
                    int targetValue = 31 - currentValue;
                    int bestIndex = 0;
                    int bestDifference = 31;
                    
                    for (int i = 0; i < topCards.Count; i++)
                    {
                        int difference = Mathf.Abs(targetValue - topCards[i].Value);
                        if (difference < bestDifference)
                        {
                            bestDifference = difference;
                            bestIndex = i;
                        }
                    }
                    
                    deck.SwapCardToTop(bestIndex);
                }
                break;
        }
        
        hasUsedSkill = true;
    }

    public override void AddCard(Card card)
    {
        base.AddCard(card);
        
        // Position the card in the AI's hand area
        if (cardSpawnPoint != null)
        {
            card.transform.position = cardSpawnPoint.position + new Vector3(hand.Count * 0.5f, 0, 0);
            // Show only the first card face down
            card.FlipCard(hand.Count > 1);
        }
    }

    public void SetDifficulty(int level)
    {
        difficultyLevel = Mathf.Clamp(level, 1, 3);
        skillUseThreshold = 0.5f + (difficultyLevel * 0.1f); // Adjust skill usage frequency based on difficulty
    }

    public bool DecideAction()
    {
        int currentValue = GetHandValue();
        
        // Consider using skill
        if (CanUseSkill() && Random.value > skillUseThreshold)
        {
            UseSkill(GameManager.Instance.deck);
        }

        // Decision to hit or stand based on difficulty and current hand
        switch (difficultyLevel)
        {
            case 1: // Easy - more likely to bust
                return currentValue < 25;
                
            case 2: // Medium - more conservative
                return currentValue < 27;
                
            case 3: // Hard - strategic decision
                int remainingToTarget = 31 - currentValue;
                // Calculate probability of getting a useful card
                float hitProbability = remainingToTarget > 10 ? 0.8f : (remainingToTarget / 10f);
                return Random.value < hitProbability && currentValue < 29;
                
            default:
                return currentValue < 26;
        }
    }

    public void RevealHand()
    {
        foreach (Card card in hand)
        {
            card.FlipCard(true);
        }
    }
}
