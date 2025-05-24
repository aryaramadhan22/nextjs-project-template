public interface IPlayer
{
    void AddCard(Card card);     // Menambah kartu ke tangan
    void ClearHand();           // Membersihkan kartu di tangan
    int GetHandValue();         // Mendapatkan total nilai kartu
    bool HasBust();             // Cek apakah total kartu lebih dari 21
}
