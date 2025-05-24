using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    [Header("Player Controls")]
    [SerializeField] private Button hitButton;
    [SerializeField] private Button standButton;
    [SerializeField] private Button skillButton;

    [Header("Score Display")]
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Deck View Panel")]
    [SerializeField] private GameObject deckViewPanel;
    [SerializeField] private Transform cardViewContainer;
    [SerializeField] private Button[] cardSelectButtons;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button restartButton;

    [Header("Round Wins")]
    [SerializeField] private Image[] playerRoundWins;
    [SerializeField] private Image[] aiRoundWins;
    [SerializeField] private Color winColor = Color.green;
    [SerializeField] private Color loseColor = Color.red;

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
        // Initialize UI elements
        hitButton.onClick.AddListener(() => GameManager.Instance.PlayerHit());
        standButton.onClick.AddListener(() => GameManager.Instance.PlayerStand());
        skillButton.onClick.AddListener(() => GameManager.Instance.humanPlayer.OnSkillButtonPressed());
        restartButton.onClick.AddListener(() => GameManager.Instance.RestartGame());

        // Initialize card select buttons
        for (int i = 0; i < cardSelectButtons.Length; i++)
        {
            int index = i;
            cardSelectButtons[i].onClick.AddListener(() => OnCardSelected(index));
        }
    }

    public void UpdateUI()
    {
        // Update scores
        var player = GameManager.Instance.humanPlayer;
        var ai = GameManager.Instance.aiPlayer;

        playerScoreText.text = $"Your Hand: {player.GetHandValue()}";
        aiScoreText.text = $"AI Hand: {ai.GetHandValue()}";
        roundText.text = $"Round {GameManager.Instance.currentRound}/3";

        // Update round win indicators
        for (int i = 0; i < playerRoundWins.Length; i++)
        {
            playerRoundWins[i].color = i < player.GetRoundsWon() ? winColor : Color.white;
            aiRoundWins[i].color = i < ai.GetRoundsWon() ? winColor : Color.white;
        }

        // Update button states
        bool isPlayerTurn = GameManager.Instance.IsPlayerTurn();
        hitButton.interactable = isPlayerTurn && !player.HasBust();
        standButton.interactable = isPlayerTurn;
        skillButton.interactable = isPlayerTurn && player.CanUseSkill();
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
    }

    public void ShowDeckViewPanel(List<Card> viewableCards)
    {
        deckViewPanel.SetActive(true);

        // Display viewable cards
        for (int i = 0; i < cardSelectButtons.Length; i++)
        {
            if (i < viewableCards.Count)
            {
                cardSelectButtons[i].gameObject.SetActive(true);
                // Update button visual to show card
                Image cardImage = cardSelectButtons[i].GetComponent<Image>();
                cardImage.sprite = viewableCards[i].GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                cardSelectButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideDeckViewPanel()
    {
        deckViewPanel.SetActive(false);
    }

    private void OnCardSelected(int index)
    {
        GameManager.Instance.humanPlayer.SelectCardToSwap(index);
        HideDeckViewPanel();
    }

    public void ShowGameOver(bool playerWon)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = playerWon ? "Congratulations! You Won!" : "Game Over! AI Wins!";
        hitButton.interactable = false;
        standButton.interactable = false;
        skillButton.interactable = false;
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }
}
