using UnityEngine;
using TMPro;

public class BalanceManager : MonoBehaviour
{
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private int startingBalance = 1000;

    private int balance;

    private void Start()
    {
        balance = startingBalance;
        UpdateBalance();
    }

    public bool Spend(int amount)
    {
        if (balance < amount)
            return false;

        balance -= amount;
        UpdateBalance();
        return true;
    }

    public void Add(int amount)
    {
        balance += amount;
        UpdateBalance();
    }

    private void UpdateBalance()
    {
        balanceText.text = "BALANCE: ₹" + balance;
    }

    public int Balance => balance;
}