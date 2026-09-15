using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SlotGameManager : MonoBehaviour
{
    [SerializeField] private SlotReel reel1;
    [SerializeField] private SlotReel reel2;
    [SerializeField] private SlotReel reel3;

    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private SlotLever lever;

    [SerializeField] private BetManager betManager;
    [SerializeField] private BalanceManager balanceManager;
    [SerializeField] private TMP_Text payoutText;

    // Payout multipliers for the symbol order.
    [SerializeField] private float sevenMultiplier = 2.5f;
    [SerializeField] private float cherryMultiplier = 1.5f;
    [SerializeField] private float bellMultiplier = 2f;
    [SerializeField] private float barMultiplier = 5f;

    public void CheckResult()
    {
        if (reel1.IsSpinning || reel2.IsSpinning || reel3.IsSpinning)
            return;

        bool win = reel1.ResultIndex == reel2.ResultIndex &&
                   reel2.ResultIndex == reel3.ResultIndex;

        if (win)
        {
            int bet = betManager.GetBet();
            float multiplier = GetMultiplier(reel1.ResultIndex);
            int payout = Mathf.RoundToInt(bet * multiplier);

            balanceManager.Add(payout);

            payoutText.text = "YOU WON ₹" + payout +"! ";
        }
        else
        {
            payoutText.text = "OPSS..YOU LOST IT!";
        }

        StartCoroutine(ShowResult(win));
    }

    // Matches the payout to the actual symbol index.
    private float GetMultiplier(int resultIndex)
    {
        switch (resultIndex)
        {
            case 0:
                return sevenMultiplier;

            case 1:
                return cherryMultiplier;

            case 2:
                return bellMultiplier;

            case 3:
                return barMultiplier;

            default:
                return 1f;
        }
    }

    // Show the result panel shortly after the reels stop.
    private IEnumerator ShowResult(bool win)
    {
        yield return new WaitForSeconds(0.6f);

        continueButton.GetComponentInChildren<TMP_Text>().text =
            win ? "LET'S SPIN AGAIN" : "STILL HAVE COURAGE";

        exitButton.GetComponentInChildren<TMP_Text>().text =
            win ? "I'M OUT" : "BEING A LOSER ACCEPTED";

        resultPanel.SetActive(true);
    }

    public void ContinueBetting()
    {
        resultPanel.SetActive(false);

        // Enable the lever for the next manual spin.
        lever.EnableLever();
    }

    public void AcceptLoss()
    {
        resultPanel.SetActive(false);
    }
}