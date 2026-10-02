using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotLever : MonoBehaviour
{
    // Visual shown when the lever is in its normal position.
    [SerializeField] private GameObject normalLever;

    // Visual shown while the lever is being pulled.
    [SerializeField] private GameObject pulledLever;

    // References needed to start the spin and manage the bet.
    [SerializeField] private SlotMachineController slotMachine;
    [SerializeField] private BetManager betManager;
    [SerializeField] private BalanceManager balanceManager;

    // Sound played when the lever is pulled.
    [SerializeField] private AudioSource leverSound;

    // Prevents the lever from being pulled multiple times.
    private bool canPull = true;

    // Button component of the lever.
    private Button leverButton;

    private void Awake()
    {
        // Get the Button component attached to the lever.
        leverButton = GetComponent<Button>();
    }

    private void Update()
    {
        // Allow the player to pull the lever using Space.
        if (Input.GetKeyDown(KeyCode.Space))
            PullLever();
    }

    public void PullLever()
    {
        // Do not allow another pull while the current spin is running.
        if (!canPull)
            return;

        // Check whether the entered bet is valid.
        if (!betManager.ValidateBet())
            return;

        // Deduct the bet from the player's balance.
        if (!balanceManager.Spend(betManager.GetBet()))
            return;

        // Disable Exit as soon as the bet is placed.
        slotMachine.SetExitEnabled(false);

        // Lock the bet input while the reels are spinning.
        betManager.SetBetInputEnabled(false);

        // Play the lever sound.
        leverSound.Play();

        // Play the lever pull animation.
        StartCoroutine(Pull());
    }

    private IEnumerator Pull()
    {
        // Disable the lever button during the pull animation.
        canPull = false;
        leverButton.interactable = false;

        // Hide the normal lever.
        normalLever.GetComponent<Image>().enabled = false;

        // Show the pulled lever.
        pulledLever.SetActive(true);

        // Keep the pulled position visible briefly.
        yield return new WaitForSeconds(0.25f);

        // Return to the normal lever.
        pulledLever.SetActive(false);
        normalLever.GetComponent<Image>().enabled = true;

        // Start the slot machine spin.
        slotMachine.Spin();
    }

    public void EnableLever()
    {
        // Allow the lever to be pulled again.
        canPull = true;
        leverButton.interactable = true;
    }
}