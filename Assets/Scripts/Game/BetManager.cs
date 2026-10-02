using UnityEngine;
using TMPro;

public class BetManager : MonoBehaviour
{
    // Input field used to enter the player's bet.
    [SerializeField] private TMP_InputField betInput;

    // Rupee symbol displayed next to the entered amount.
    [SerializeField] private TMP_Text rupeeSymbol;

    // Placeholder shown before the player enters a bet.
    [SerializeField] private GameObject placeholder;

    // Panel used to display invalid bet warnings.
    [SerializeField] private GameObject warningPanel;

    // Text used to display the warning message.
    [SerializeField] private TMP_Text warningText;

    // Reference to the player's balance.
    [SerializeField] private BalanceManager balanceManager;

    // Minimum amount allowed for a bet.
    [SerializeField] private int minimumBet = 50;

    private int currentBet = 0;

    private void Start()
    {
        // Start with an empty input so the placeholder is visible.
        betInput.text = "";

        rupeeSymbol.gameObject.SetActive(false);
        warningPanel.SetActive(false);
        placeholder.SetActive(true);

        // Listen for input selection and value changes.
        betInput.onSelect.AddListener(OnBetSelected);
        betInput.onValueChanged.AddListener(OnValueChanged);
    }

    // Hide the placeholder and show the rupee symbol when the player clicks the input.
    private void OnBetSelected(string value)
    {
        placeholder.SetActive(false);
        rupeeSymbol.gameObject.SetActive(true);

        UpdateRupeePosition();
    }

    // Update the bet value and rupee position while typing.
    private void OnValueChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            currentBet = 0;
            rupeeSymbol.gameObject.SetActive(true);
            return;
        }

        if (int.TryParse(value, out int amount))
        {
            currentBet = amount;
            rupeeSymbol.gameObject.SetActive(true);

            UpdateRupeePosition();
        }
    }

    // Checks whether the entered bet is valid before starting a spin.
    public bool ValidateBet()
    {
        // Reject empty or non-numeric input.
        if (!int.TryParse(betInput.text, out int bet))
        {
            ShowWarning("Minimum Bet is ₹" + minimumBet);
            return false;
        }

        // Reject bets below the minimum allowed amount.
        if (bet < minimumBet)
        {
            ShowWarning("Minimum Bet is ₹" + minimumBet);
            return false;
        }

        // Reject bets higher than the player's available balance.
        if (bet > balanceManager.Balance)
        {
            ShowWarning("CHECK YOUR BALANCE!");
            return false;
        }

        currentBet = bet;
        return true;
    }

    // Displays a warning message to the player.
    private void ShowWarning(string message)
    {
        warningText.text = message;
        warningPanel.SetActive(true);
    }

    // Closes the warning panel.
    public void CloseWarning()
    {
        warningPanel.SetActive(false);
    }

    // Returns the currently selected bet amount.
    public int GetBet()
    {
        return currentBet;
    }

    // Enables or disables the bet input.
    public void SetBetInputEnabled(bool enabled)
    {
        betInput.interactable = enabled;
    }

    // Keeps the rupee symbol positioned just before the entered amount.
    private void UpdateRupeePosition()
    {
        if (string.IsNullOrEmpty(betInput.text))
            return;

        float textWidth = betInput.textComponent.GetPreferredValues(
            betInput.text
        ).x;

        // Keep the rupee symbol just before the entered amount.
        rupeeSymbol.rectTransform.anchoredPosition =
            new Vector2(-textWidth / 2f - 18f, 0f);
    }
}