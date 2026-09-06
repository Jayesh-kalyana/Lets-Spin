using UnityEngine;
using System.Collections;

public class SlotMachineController : MonoBehaviour
{
    // References to the three reels.
    [SerializeField] private SlotReel reel1;
    [SerializeField] private SlotReel reel2;
    [SerializeField] private SlotReel reel3;

    // Delay before Reel 1 starts.
    [SerializeField] private float reel1StartDelay = 1.5f;

    // Delay before Reel 2 starts after Reel 1.
    [SerializeField] private float reel2StartDelay = 1f;


    // Starts the reels automatically for testing.
    private void Start()
    {
        Spin();
    }


    // Starts the complete reel sequence.
    public void Spin()
    {
        StartCoroutine(SpinAllReels());
    }


    // Controls the reel start order.
    private IEnumerator SpinAllReels()
    {
        // Reel 3 starts first.
        reel3.Spin();

        // Wait before starting Reel 1.
        yield return new WaitForSeconds(reel1StartDelay);

        reel1.Spin();

        // Wait before starting Reel 2.
        yield return new WaitForSeconds(reel2StartDelay);

        reel2.Spin();
    }
}