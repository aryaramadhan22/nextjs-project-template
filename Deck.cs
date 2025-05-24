using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private Transform deckPosition;
    
    private List<Card> cards = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    
    private void InitializeDeck()
    {
        cards.Clear();
        discardPile.Clear();
        
        // Daftar kartu
        string[] ranks = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        int[] values = { 11, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10 };
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };

        // Buat kartu untuk setiap jenis dan angka
        foreach (string suit in suits)
        {
            for (int i = 0; i < ranks.Length; i++)
            {
                GameObject cardObj = Instantiate(cardPrefab, deckPosition.position, Quaternion.identity);
                Card card = cardObj.GetComponent<Card>();
                card.Initialize(values[i], suit, ranks[i], LoadCardSprite(suit, ranks[i]));
                cards.Add(card);
                cardObj.SetActive(false);
            }
        }
        
        Shuffle();
    }

    public void Shuffle()
    {
        // Gabungkan kartu dari discard pile
        cards.AddRange(discardPile);
        discardPile.Clear();

        // Acak kartu
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }

        // Atur posisi kartu
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.position = deckPosition.position + new Vector3(0, -0.01f * i, 0);
            cards[i].transform.rotation = Quaternion.identity;
            cards[i].FlipCard(false);
            cards[i].gameObject.SetActive(true);
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            if (discardPile.Count > 0)
            {
                Shuffle();
            }
            else
            {
                Debug.LogError("Kartu habis!");
                return null;
            }
        }

        Card drawnCard = cards[0];
        cards.RemoveAt(0);
        drawnCard.gameObject.SetActive(true);
        return drawnCard;
    }

    public void DiscardCard(Card card)
    {
        if (card != null)
        {
            discardPile.Add(card);
            card.gameObject.SetActive(false);
        }
    }

    private Sprite LoadCardSprite(string suit, string rank)
    {
        string spritePath = $"Cards/{suit}_{rank.ToLower()}";
        return Resources.Load<Sprite>(spritePath);
    }

    public void Reset()
    {
        InitializeDeck();
    }
}
