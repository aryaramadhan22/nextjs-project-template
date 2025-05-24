using UnityEngine;

public class Card : MonoBehaviour
{
    private int value;          // nilai kartu (1-11)
    private string suit;        // jenis kartu ("hearts", "diamonds", "clubs", "spades")
    private string rank;        // nomor/huruf kartu ("A","2","3"..."J","Q","K")
    private SpriteRenderer spriteRenderer;
    private bool isFaceUp = false;
    
    public int Value { get { return value; } }
    public string Suit { get { return suit; } }
    public string Rank { get { return rank; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int cardValue, string cardSuit, string cardRank, Sprite cardSprite)
    {
        value = cardValue;
        suit = cardSuit;
        rank = cardRank;
        spriteRenderer.sprite = cardSprite;
    }

    public void FlipCard(bool faceUp)
    {
        isFaceUp = faceUp;
        // Nanti bisa ditambahkan:
        // spriteRenderer.sprite = faceUp ? frontSprite : backSprite;
    }
}
