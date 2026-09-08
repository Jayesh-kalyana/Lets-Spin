using UnityEngine;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    // References to the three reels.
    [SerializeField] private SlotReel reel1;
    [SerializeField] private SlotReel reel2;
    [SerializeField] private SlotReel reel3;

    // Reference to the game manager.
    [SerializeField] private SlotGameManager gameManager;

    // Time to wait before starting Reel 1.
    [SerializeField] private float reel1StartDelay = 1.5f;

    // Time to wait before starting Reel 2.
    [SerializeField] private float reel2StartDelay = 1f;

    // Prevents another spin during the current spin.
    private bool isSpinning = false;


    // Starts a new spin.
    public void Spin()
    {
        if (isSpinning)
            return;

        StartCoroutine(SpinAllReels());
    }


    // Controls the reel sequence.
    private IEnumerator SpinAllReels()
    {
        isSpinning = true;

        // Start Reel 3 first.
        reel3.Spin();

        // Wait before starting Reel 1.
        yield return new WaitForSeconds(reel1StartDelay);

        // Start Reel 1.
        reel1.Spin();

        // Wait before starting Reel 2.
        yield return new WaitForSeconds(reel2StartDelay);

        // Start Reel 2.
        reel2.Spin();

        // Wait until all reels stop.
        yield return new WaitUntil(() =>
            !reel1.IsSpinning &&
            !reel2.IsSpinning &&
            !reel3.IsSpinning
        );

        // Check whether the player won or lost.
        gameManager.CheckResult();

        // Allow another spin.
        isSpinning = false;
    }
}