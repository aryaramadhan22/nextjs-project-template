using UnityEngine;

public class HumanPlayer : AbstractPlayer
{
    [SerializeField]
    private Transform cardSpawnPoint;    // Posisi untuk meletakkan kartu

    public override void AddCard(Card card)
    {
        base.AddCard(card);
        
        // Atur posisi kartu di area tangan pemain
        if (cardSpawnPoint != null)
        {
            card.transform.position = cardSpawnPoint.position + new Vector3(hand.Count * 0.5f, 0, 0);
            card.FlipCard(true); // Tampilkan kartu menghadap atas
        }
    }

    // Dipanggil saat tombol Hit ditekan
    public void OnHitButtonPressed()
    {
        if (!GameManager.Instance.IsPlayerTurn() || HasBust()) return;
        GameManager.Instance.PlayerHit();
    }

    // Dipanggil saat tombol Stand ditekan
    public void OnStandButtonPressed()
    {
        if (!GameManager.Instance.IsPlayerTurn()) return;
        GameManager.Instance.PlayerStand();
    }
}
