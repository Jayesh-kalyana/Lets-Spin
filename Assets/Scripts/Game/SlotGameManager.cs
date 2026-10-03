using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SlotGameManager : MonoBehaviour
{
    // References to the three slot reels.
    [SerializeField] private SlotReel reel1;
    [SerializeField] private SlotReel reel2;
    [SerializeField] private SlotReel reel3;

    // Result panel and its buttons.
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button resultExitButton;

    // References used to control the lever and slot machine.
    [SerializeField] private SlotLever lever;
    [SerializeField] private SlotMachineController slotMachine;

    // Win sounds for each symbol.
    [SerializeField] private AudioSource cherryWinSound;
    [SerializeField] private AudioSource bellWinSound;
    [SerializeField] private AudioSource sevenWinSound;

    // Special sound for the BAR jackpot.
    [SerializeField] private AudioSource jackpotSound;

    // Plays when the player loses.
    [SerializeField] private AudioSource loseSound;

    // References for betting, balance and result text.
    [SerializeField] private BetManager betManager;
    [SerializeField] private BalanceManager balanceManager;
    [SerializeField] private TMP_Text payoutText;

    // Intro hint shown when the game starts.
    [SerializeField] private GameObject spaceHintPanel;
    [SerializeField] private GameObject thanksPanel;

    // Background music played after the intro hint is closed.
    [SerializeField] private AudioSource backgroundMusic;


    // Payout multipliers for each symbol.
    [SerializeField] private float sevenMultiplier = 2.5f;
    [SerializeField] private float cherryMultiplier = 1.5f;
    [SerializeField] private float bellMultiplier = 2f;
    [SerializeField] private float barMultiplier = 5f;

    // Exit confirmation popup.
    [SerializeField] private GameObject exitConfirmation;

    private void Start()
{
    // Lock gameplay until the intro hint is closed.
    betManager.SetBetInputEnabled(false);
    lever.SetLeverEnabled(false);
}

    // Closes the Space-to-Spin hint.
    public void CloseSpaceHint()
    {
        spaceHintPanel.SetActive(false);

        betManager.SetBetInputEnabled(true);
        lever.SetLeverEnabled(true);

        backgroundMusic.Play();
    }    
    public void CheckResult()
    {
        // Do not check the result while any reel is still spinning.
        if (reel1.IsSpinning || reel2.IsSpinning || reel3.IsSpinning)
            return;

        int result1 = reel1.ResultIndex;
        int result2 = reel2.ResultIndex;
        int result3 = reel3.ResultIndex;

        // Check for three matching symbols.
        bool tripleWin = result1 == result2 && result2 == result3;

        // Check for two adjacent matching symbols.
        bool pairWin = !tripleWin &&
                       (result1 == result2 || result2 == result3);

        if (tripleWin)
        {
            int bet = betManager.GetBet();

            // Get the payout multiplier for the winning symbol.
            float multiplier = GetMultiplier(result1);

            // Calculate the final payout.
            int payout = Mathf.RoundToInt(bet * multiplier);

            // Add the payout to the player's balance.
            balanceManager.Add(payout);

            // Play the winning sound.
            PlayWinSound(result1);

            payoutText.text =
                "CONGRATULATIONS!\n" +
                "YOU WON ₹" + payout + "\n" +
                multiplier + "X PAYOUT";
        }
        else if (pairWin)
        {
            int bet = betManager.GetBet();

            // Use the matching adjacent symbol for the win sound.
            int matchingSymbol = result1 == result2 ? result1 : result2;

            // Small win for two adjacent matching symbols.
            float multiplier = 1.25f;

            // Calculate the final payout.
            int payout = Mathf.RoundToInt(bet * multiplier);

            // Add the payout to the player's balance.
            balanceManager.Add(payout);

            // Play the winning sound.
            PlayWinSound(matchingSymbol);

            payoutText.text =
                "NICE!\n" +
                "YOU WON ₹" + payout + "\n" +
                multiplier + "X PAYOUT";
        }
        else
        {
            // Play lose sound.
            loseSound.Play();

            // Show the loss message.
            payoutText.text = "OPPS YOU LOST IT !";
        }

        // Show the result panel after a short delay.
        StartCoroutine(ShowResult(tripleWin || pairWin));
    }

    // Plays the correct sound based on the winning symbol.
    private void PlayWinSound(int resultIndex)
    {
        switch (resultIndex)
        {
            case 0:
                // 7 x 7 x 7
                sevenWinSound.Play();
                break;

            case 1:
                // Cherry x Cherry x Cherry
                cherryWinSound.Play();
                break;

            case 2:
                // Bell x Bell x Bell
                bellWinSound.Play();
                break;

            case 3:
                // BAR x BAR x BAR = Jackpot
                jackpotSound.Play();
                break;
        }
    }

    // Returns the payout multiplier for the selected symbol.
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

    // Shows the result panel shortly after the reels stop.
    private IEnumerator ShowResult(bool win)
    {
        yield return new WaitForSeconds(0.6f);

        // Change the button text based on the result.
        continueButton.GetComponentInChildren<TMP_Text>().text =
            win ? "LET'S SPIN AGAIN" : "STILL HAVE COURAGE";

        resultExitButton.GetComponentInChildren<TMP_Text>().text =
            win ? "I'M OUT" : "BEING A LOSER ACCEPTED";

        resultPanel.SetActive(true);
    }

    public void ContinueBetting()
    {
        resultPanel.SetActive(false);

        // Enable betting for the next spin.
        betManager.SetBetInputEnabled(true);

        // Enable the lever for the next spin.
        lever.EnableLever();

        // Allow the player to use Exit again.
        slotMachine.SetExitEnabled(true);
    }

    public void AcceptLoss()
    {
        resultPanel.SetActive(false);

        // Enable betting for the next spin.
        betManager.SetBetInputEnabled(true);

        // Enable the lever for the next spin.
        lever.EnableLever();

        // Allow the player to use Exit again.
        slotMachine.SetExitEnabled(true);
    }

    public void ShowExitConfirmation()
    {
        // Show the exit confirmation popup.
        exitConfirmation.SetActive(true);
    }

    public void CancelExit()
    {
        // Hide the exit confirmation popup.
        exitConfirmation.SetActive(false);
    }

    public void ExitGame()
    {
        // Close the game.
        Application.Quit();
        // Show the final message instead of closing the WebGL browser tab.
        thanksPanel.SetActive(true);
        Debug.Log("Game exited.");
    }
}