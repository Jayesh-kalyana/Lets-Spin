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

    public void CheckResult()
    {
        if (reel1.IsSpinning || reel2.IsSpinning || reel3.IsSpinning)
            return;

        bool win = reel1.ResultIndex == reel2.ResultIndex &&
                   reel2.ResultIndex == reel3.ResultIndex;

        StartCoroutine(ShowResult(win));
    }

    // Show the result options shortly after the reels stop.
    private IEnumerator ShowResult(bool win)
    {
        yield return new WaitForSeconds(0.6f);

        continueButton.GetComponentInChildren<TMP_Text>().text =
            win ? "LET'S SPIN AGAIN 🔥" : "STILL HAVE COURAGE 😎";

        exitButton.GetComponentInChildren<TMP_Text>().text =
            win ? "I'M OUT 🥱" : "BEING A LOSER ACCEPTED 🥲";

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