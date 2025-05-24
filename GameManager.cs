using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Make these public so they can be accessed by GameUI
    public Deck deck;
    public HumanPlayer humanPlayer;
    public AIPlayer aiPlayer;
    public GameUI gameUI;
    
    private bool isPlayerTurn = true;
    public int currentRound { get; private set; } = 1;
    private const int ROUNDS_TO_WIN = 2; // Best of 3
    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        currentRound = 1;
        humanPlayer.ClearHand();
        aiPlayer.ClearHand();
        deck.Reset();
        gameEnded = false;
        
        aiPlayer.SetDifficulty(2); // Medium difficulty by default
        
        StartRound();
    }

    private void StartRound()
    {
        isPlayerTurn = true;
        
        // Deal initial cards
        for (int i = 0; i < 2; i++)
        {
            humanPlayer.AddCard(deck.DrawCard());
            aiPlayer.AddCard(deck.DrawCard());
        }

        gameUI.UpdateUI();
        gameUI.ShowMessage($"Round {currentRound} - Your Turn!");
    }

    public void PlayerHit()
    {
        if (!isPlayerTurn || gameEnded) return;

        Card card = deck.DrawCard();
        humanPlayer.AddCard(card);
        
        if (humanPlayer.HasBust())
        {
            EndRound(false);
        }
        
        gameUI.UpdateUI();
    }

    public void PlayerStand()
    {
        if (!isPlayerTurn || gameEnded) return;
        
        isPlayerTurn = false;
        StartCoroutine(AITurn());
    }

    private IEnumerator AITurn()
    {
        aiPlayer.RevealHand();
        gameUI.ShowMessage("AI's Turn");
        yield return new WaitForSeconds(1f);

        while (aiPlayer.DecideAction())
        {
            Card card = deck.DrawCard();
            aiPlayer.AddCard(card);
            gameUI.UpdateUI();
            
            if (aiPlayer.HasBust())
            {
                EndRound(true);
                yield break;
            }
            
            yield return new WaitForSeconds(1f);
        }

        // AI stands - compare hands
        EndRound(DetermineWinner());
    }

    private bool DetermineWinner()
    {
        int playerValue = humanPlayer.GetHandValue();
        int aiValue = aiPlayer.GetHandValue();

        if (playerValue > 31) return false;
        if (aiValue > 31) return true;
        
        return playerValue > aiValue;
    }

    private void EndRound(bool playerWins)
    {
        if (playerWins)
        {
            humanPlayer.WinRound();
            gameUI.ShowMessage("You win this round!");
        }
        else
        {
            aiPlayer.WinRound();
            gameUI.ShowMessage("AI wins this round!");
        }

        // Check if game is over
        if (humanPlayer.GetRoundsWon() >= ROUNDS_TO_WIN || 
            aiPlayer.GetRoundsWon() >= ROUNDS_TO_WIN)
        {
            EndGame();
        }
        else
        {
            StartCoroutine(StartNextRound());
        }
    }

    private IEnumerator StartNextRound()
    {
        yield return new WaitForSeconds(2f);
        
        currentRound++;
        humanPlayer.ClearHand();
        aiPlayer.ClearHand();
        deck.Shuffle();
        
        StartRound();
    }

    private void EndGame()
    {
        gameEnded = true;
        bool playerWonGame = humanPlayer.GetRoundsWon() >= ROUNDS_TO_WIN;
        gameUI.ShowGameOver(playerWonGame);
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn && !gameEnded;
    }

    public void RestartGame()
    {
        InitializeGame();
    }
}
