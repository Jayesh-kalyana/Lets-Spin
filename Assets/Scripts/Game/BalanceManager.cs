using UnityEngine;
using TMPro;

public class BalanceManager : MonoBehaviour
{
    // Text used to display the player's current balance.
    [SerializeField] private TMP_Text balanceText;

    // Starting balance when the game begins.
    [SerializeField] private int startingBalance = 1000;

    private int balance;

    private void Start()
    {
        // Set the balance to the starting amount.
        balance = startingBalance;

        // Update the balance text on the screen.
        UpdateBalance();
    }

    public bool Spend(int amount)
    {
        // Prevent the player from spending more than the available balance.
        if (balance < amount)
            return false;

        // Deduct the amount from the balance.
        balance -= amount;

        // Refresh the displayed balance.
        UpdateBalance();

        return true;
    }

    public void Add(int amount)
    {
        // Add the payout amount to the player's balance.
        balance += amount;

        // Refresh the displayed balance.
        UpdateBalance();
    }

    private void UpdateBalance()
    {
        // Display the current balance in the UI.
        balanceText.text = "BALANCE: ₹" + balance;
    }

    // Provides the current balance to other game scripts.
    public int Balance => balance;
}