using UnityEngine;
using TMPro;

public class BetManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField betInput;
    [SerializeField] private TMP_Text rupeeSymbol;
    [SerializeField] private GameObject placeholder;
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private BalanceManager balanceManager;

    [SerializeField] private int minimumBet = 50;

    private int currentBet = 0;

    private void Start()
    {
        // Start with an empty input so the placeholder is visible.
        betInput.text = "";

        rupeeSymbol.gameObject.SetActive(false);
        warningPanel.SetActive(false);
        placeholder.SetActive(true);

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

    // Update the rupee position while typing.
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

    // Checks whether the entered bet is valid.
    public bool ValidateBet()
    {
        if (!int.TryParse(betInput.text, out int bet))
        {
            ShowWarning("Minimum Bet is ₹" + minimumBet);
            return false;
        }

        if (bet < minimumBet)
        {
            ShowWarning("Minimum Bet is ₹" + minimumBet);
            return false;
        }

        if (bet > balanceManager.Balance)
        {
            ShowWarning("CHECK YOUR BALANCE!");
            return false;
        }

        currentBet = bet;
        return true;
    }

    private void ShowWarning(string message)
    {
        warningText.text = message;
        warningPanel.SetActive(true);
    }

    public void CloseWarning()
    {
        warningPanel.SetActive(false);
    }

    public int GetBet()
    {
        return currentBet;
    }

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