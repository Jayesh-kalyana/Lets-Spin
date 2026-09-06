using UnityEngine;
using System.Collections;

public class SlotLever : MonoBehaviour
{
    // Normal lever image.
    [SerializeField] private GameObject normalLever;

    // Pulled lever image.
    [SerializeField] private GameObject pulledLever;

    // Reference to the slot machine controller.
    [SerializeField] private SlotMachineController slotMachine;

    // Prevents the lever from being pulled repeatedly during one pull.
    private bool isPulling = false;


    private void Update()
    {
        // Pull the lever when Space is pressed.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PullLever();
        }
    }


    // Called when the lever is clicked or Space is pressed.
    public void PullLever()
    {
        if (isPulling)
            return;

        StartCoroutine(PullLeverAnimation());
    }


    // Handles the lever animation.
    private IEnumerator PullLeverAnimation()
    {
        isPulling = true;

        // Hide the normal lever.
        normalLever.SetActive(false);

        // Show the pulled lever.
        pulledLever.SetActive(true);

        // Start the slot machine.
        slotMachine.Spin();

        // Keep the lever pulled briefly.
        yield return new WaitForSeconds(0.25f);

        // Return the lever to its normal position.
        pulledLever.SetActive(false);
        normalLever.SetActive(true);

        // Allow another lever pull.
        isPulling = false;
    }
}