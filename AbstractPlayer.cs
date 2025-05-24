using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPlayer : MonoBehaviour, IPlayer
{
    protected List<Card> hand = new List<Card>();    // Kartu di tangan
    protected int roundsWon = 0;                     // Jumlah ronde yang dimenangkan

    // Menambah kartu ke tangan
    public virtual void AddCard(Card card)
    {
        hand.Add(card);
    }

    // Membersihkan kartu di tangan
    public virtual void ClearHand()
    {
        hand.Clear();
    }

    // Menghitung total nilai kartu
    public virtual int GetHandValue()
    {
        int total = 0;
        int aces = 0;

        foreach (Card card in hand)
        {
            if (card.Value == 11) // Ace
            {
                aces++;
            }
            total += card.Value;
        }

        // Ubah nilai Ace dari 11 ke 1 jika total lebih dari 21
        while (total > 21 && aces > 0)
        {
            total -= 10;
            aces--;
        }

        return total;
    }

    // Cek apakah total kartu lebih dari 21
    public bool HasBust()
    {
        return GetHandValue() > 21;
    }

    // Mendapatkan jumlah ronde yang dimenangkan
    public int GetRoundsWon()
    {
        return roundsWon;
    }

    // Menambah jumlah kemenangan
    public void WinRound()
    {
        roundsWon++;
    }

    // Mendapatkan daftar kartu di tangan
    public List<Card> GetHand()
    {
        return hand;
    }
}
