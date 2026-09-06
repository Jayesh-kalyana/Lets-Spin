using UnityEngine;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    // References to the three reels.
    [SerializeField] private SlotReel reel1;
    [SerializeField] private SlotReel reel2;
    [SerializeField] private SlotReel reel3;

    // Time to wait before starting Reel 1 after Reel 3 starts.
    [SerializeField] private float reel1StartDelay = 1.5f;

    // Time to wait before starting Reel 2 after Reel 1 starts.
    [SerializeField] private float reel2StartDelay = 1f;

    // Prevents starting another spin while the current spin is running.
    private bool isSpinning = false;


    // Starts a new spin.
    public void Spin()
    {
        // Ignore the request while the current spin is running.
        if (isSpinning)
            return;

        StartCoroutine(SpinAllReels());
    }


    // Controls the complete spin sequence.
    private IEnumerator SpinAllReels()
    {
        isSpinning = true;

        // Reel 3 starts first.
        reel3.Spin();

        // Wait before starting Reel 1.
        yield return new WaitForSeconds(reel1StartDelay);

        // Start Reel 1.
        reel1.Spin();

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

        // Allow the player to spin again.
        isSpinning = false;
    }
}