using UnityEngine;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    // References to the three slot reels.
    [SerializeField] private SlotReel reel1;
    [SerializeField] private SlotReel reel2;
    [SerializeField] private SlotReel reel3;

    // Delay before Reel 1 and Reel 2 start spinning.
    [SerializeField] private float reel1StartDelay = 1.5f;
    [SerializeField] private float reel2StartDelay = 1f;

    // Reference used to check the final spin result.
    [SerializeField] private SlotGameManager gameManager;

    // Main Exit button.
    [SerializeField] private UnityEngine.UI.Button exitButton;

    // Audio sources for the two reel spinning sounds.
    [SerializeField] private AudioSource reelSpinSound1;
    [SerializeField] private AudioSource reelSpinSound2;

    // Exit confirmation popup.
    [SerializeField] private GameObject exitConfirmation;

    // Tracks whether the slot machine is currently spinning.
    private bool isSpinning = false;

    public void Spin()
    {
        // Prevent another spin from starting while the machine is already spinning.
        if (isSpinning)
            return;

        StartCoroutine(SpinAllReels());
    }

    // Enables or disables the main Exit button.
    public void SetExitEnabled(bool enabled)
    {
        exitButton.interactable = enabled;

        // Close the Exit popup when Exit is disabled.
        if (!enabled)
            exitConfirmation.SetActive(false);

        Debug.Log("EXIT INTERACTABLE = " + enabled);
    }

    private IEnumerator SpinAllReels()
    {
        // Mark the machine as spinning.
        isSpinning = true;

        // Disable Exit while the reels are spinning.
        SetExitEnabled(false);

        // Start Reel 3 first.
        reel3.Spin();

        // Play the first reel spinning sound.
        reelSpinSound1.Play();

        // Wait before starting Reel 1.
        yield return new WaitForSeconds(reel1StartDelay);

        // Start Reel 1.
        reel1.Spin();

        // Play the second reel spinning sound.
        reelSpinSound2.Play();

        // Wait before starting Reel 2.
        yield return new WaitForSeconds(reel2StartDelay);

        // Start Reel 2.
        reel2.Spin();

        // Wait until all reels have stopped.
        yield return new WaitUntil(() =>
            !reel1.IsSpinning &&
            !reel2.IsSpinning &&
            !reel3.IsSpinning
        );

        // Mark the machine as no longer spinning.
        isSpinning = false;

        // Check the final result.
        gameManager.CheckResult();

        // Enable Exit after the result has been shown.
        yield return new WaitForSeconds(0.6f);

        SetExitEnabled(true);
    }
}